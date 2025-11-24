using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
 [ApiController]
    [Route("api/[controller]")]
   public class BackupController : ControllerBase
    {
  private readonly IBackupRepositorio _backupRepositorio;

  public BackupController(IBackupRepositorio backupRepositorio)
 {
          _backupRepositorio = backupRepositorio;
      }

      /// <summary>
       /// Descarga un backup completo de la base de datos como archivo SQL
      /// </summary>
     [HttpPost("descargar")]
 public async Task<IActionResult> DescargarBackup()
        {
       try
       {
     var archivoBackup = await _backupRepositorio.CrearBackupAsync();

   if (archivoBackup == null || archivoBackup.Length == 0)
     {
 return BadRequest(new { error = "No se pudo crear el backup" });
   }

   var nombreArchivo = $"spa_backup_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.sql";
        return File(archivoBackup, "application/octet-stream", nombreArchivo);
     }
   catch (Exception ex)
     {
     return StatusCode(500, new { error = "Error al descargar backup", detalle = ex.Message });
   }
        }

        /// <summary>
      /// Restaura la base de datos desde un archivo de backup SQL
     /// </summary>
   [HttpPost("restaurar")]
        public async Task<IActionResult> RestaurarBackup(IFormFile archivoBackup)
    {
      try
        {
  if (archivoBackup == null || archivoBackup.Length == 0)
       {
     return BadRequest(new { error = "Debe proporcionar un archivo de backup" });
           }

// Leer bytes del archivo
    using (var memoryStream = new System.IO.MemoryStream())
             {
    await archivoBackup.CopyToAsync(memoryStream);
     var bytes = memoryStream.ToArray();

        // Restaurar la base de datos
   var resultado = await _backupRepositorio.RestaurarBackupAsync(bytes);

        if (resultado)
 {
   return Ok(new
     {
      mensaje = "Base de datos restaurada exitosamente",
     fechaRestauracion = DateTime.Now
     });
          }
   else
 {
   return BadRequest(new { error = "Falló la restauración de la base de datos" });
         }
}
    }
     catch (Exception ex)
       {
         return StatusCode(500, new { error = "Error al restaurar backup", detalle = ex.Message });
      }
        }
  }
}
