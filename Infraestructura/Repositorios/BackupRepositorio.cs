using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositorios
{
    public class BackupRepositorio : IBackupRepositorio
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public BackupRepositorio(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        
        public async Task<byte[]> CrearBackupAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("PostgresConnection");
                var connParams = ParseConnectionString(connectionString);

                // Crear ruta temporal para el backup
                var tempBackupPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"spa_backup_{Guid.NewGuid()}.sql");

                // Crear proceso para pg_dump
                var processInfo = new ProcessStartInfo
                {
                    FileName = "pg_dump",
                    // -w: never prompt for password, -F p plain SQL, -c (--clean) include DROP statements, --no-owner/--no-privileges avoid owner/privilege commands
                    Arguments = $"-w -h {connParams["Host"]} -U {connParams["Username"]} -d {connParams["Database"]} -F p -c --no-owner --no-privileges -f \"{tempBackupPath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                // Configurar variable de entorno para la contraseña
                if (connParams.ContainsKey("Password"))
                {
                    processInfo.Environment["PGPASSWORD"] = connParams["Password"];
                }

                using (var process = Process.Start(processInfo))
                {
                    // Use a timeout to avoid indefinite hangs
                    var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(10));

                    var stdErrTask = process.StandardError.ReadToEndAsync();
                    var stdOutTask = process.StandardOutput.ReadToEndAsync();

                    try
                    {
                        await process.WaitForExitAsync(cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        try { process.Kill(); } catch { }
                        throw new Exception("La creación del backup excedió el tiempo límite y fue terminada.");
                    }

                    var error = await stdErrTask;
                    var output = await stdOutTask;

                    if (process.ExitCode != 0)
                    {
                        if (System.IO.File.Exists(tempBackupPath)) System.IO.File.Delete(tempBackupPath);
                        throw new Exception($"Error al crear backup (pg_dump): {error}");
                    }
                }

                // Verificar que el archivo se creó
                if (!System.IO.File.Exists(tempBackupPath))
                {
                    throw new Exception("El archivo de backup no se creó correctamente.");
                }

                // Leer el archivo en bytes
                var bytes = await System.IO.File.ReadAllBytesAsync(tempBackupPath);

                // Eliminar el archivo temporal
                System.IO.File.Delete(tempBackupPath);

                return bytes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error durante la creación del backup: {ex.Message}", ex);
            }
        }

        
        public async Task<bool> RestaurarBackupAsync(byte[] archivoBackup)
        {
            try
            {
                if (archivoBackup == null || archivoBackup.Length == 0)
                {
                    throw new Exception("El archivo de backup está vacío.");
                }

                var connectionString = _configuration.GetConnectionString("PostgresConnection");
                var connParams = ParseConnectionString(connectionString);

                // Crear ruta temporal para el archivo
                var tempBackupPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"spa_restore_{Guid.NewGuid()}.sql");

                // Escribir bytes a archivo temporal
                await System.IO.File.WriteAllBytesAsync(tempBackupPath, archivoBackup);

                try
                {
                    // Crear proceso para psql
                    var processInfo = new ProcessStartInfo
                    {
                        FileName = "psql",
                        // -w: never prompt for password, -v ON_ERROR_STOP=1: stop on first error
                        Arguments = $"-w -v ON_ERROR_STOP=1 -h {connParams["Host"]} -U {connParams["Username"]} -d {connParams["Database"]} -f \"{tempBackupPath}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    // Configurar variable de entorno para la contraseña
                    if (connParams.ContainsKey("Password"))
                    {
                        processInfo.Environment["PGPASSWORD"] = connParams["Password"];
                    }

                    using (var process = Process.Start(processInfo))
                    {
                        // Use a timeout to avoid indefinite hangs
                        var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(20));

                        var stdErrTask = process.StandardError.ReadToEndAsync();
                        var stdOutTask = process.StandardOutput.ReadToEndAsync();

                        try
                        {
                            await process.WaitForExitAsync(cts.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            try { process.Kill(); } catch { }
                            throw new Exception("La restauración del backup excedió el tiempo límite y fue terminada.");
                        }

                        var error = await stdErrTask;
                        var output = await stdOutTask;

                        if (process.ExitCode != 0)
                        {
                            throw new Exception($"Error al restaurar backup (psql): {error}");
                        }
                    }

                    return true;
                }
                finally
                {
                    // Eliminar archivo temporal
                    if (System.IO.File.Exists(tempBackupPath))
                    {
                        System.IO.File.Delete(tempBackupPath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error durante la restauración del backup: {ex.Message}", ex);
            }
        }

       
        private Dictionary<string, string> ParseConnectionString(string connectionString)
        {
            var parameters = new Dictionary<string, string>();

            var parts = connectionString.Split(';');
            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part)) continue;

                var keyValue = part.Split('=');
                if (keyValue.Length == 2)
                {
                    var key = keyValue[0].Trim().ToLower();
                    var value = keyValue[1].Trim();

                    // Mapear nombres de parámetros
                    switch (key)
                    {
                        case "host":
                            parameters["Host"] = value;
                            break;
                        case "database":
                            parameters["Database"] = value;
                            break;
                        case "username":
                        case "user id":
                        case "user":
                            parameters["Username"] = value;
                            break;
                        case "password":
                            parameters["Password"] = value;
                            break;
                        case "port":
                            parameters["Port"] = value;
                            break;
                    }
                }
            }

            // Valores por defecto
            if (!parameters.ContainsKey("Host")) parameters["Host"] = "localhost";
            if (!parameters.ContainsKey("Port")) parameters["Port"] = "5432";

            return parameters;
        }
    }
}
