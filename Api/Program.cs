using Aplication.Mapping;
using Aplication.UseCases;
using Dominio.Interfaces;
using Infraestructura.Data;
using Infraestructura.Repositorios;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            // -----------------------------------------------------------------
            // 1. AGREGA TODOS TUS SERVICIOS AQUÍ
            // -----------------------------------------------------------------



            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"),
                    b => b.MigrationsAssembly("Infraestructura")));

            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            
            builder.Services.AddScoped<IDocenteRepositorio, DocenteRepositorio>();
            builder.Services.AddScoped<CrearDocente>();

            builder.Services.AddScoped<ICursoRepositorio, CursoRepositorio>();
            builder.Services.AddScoped<CrearCurso>();

            builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            builder.Services.AddScoped<CrearCliente>();

            builder.Services.AddScoped<IServicioRepositorio, ServicioRepositorio>();
            builder.Services.AddScoped<CrearServicio>();

            builder.Services.AddScoped<IEmpleadaRepositorio, EmpleadaRepositorio>();
            builder.Services.AddScoped<CrearEmpleada>();

            builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
            builder.Services.AddScoped<CrearCategoria>();

            builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
            builder.Services.AddScoped<CrearProducto>();

            builder.Services.AddScoped<IVentaRepositorio, VentaRepositorio>();
            builder.Services.AddScoped<GenerarReportePeriodico>();

            // Registrar servicios para Producto
            builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
            builder.Services.AddScoped<CrearProducto>();

            // Registrar servicios para Cita
            builder.Services.AddScoped<ICitaRepositorio, CitaRepositorio>();
            builder.Services.AddScoped<CrearCita>();
            builder.Services.AddScoped<ValidarDisponibilidad>();

            // Registrar servicios para Disponibilidad
            builder.Services.AddScoped<IDisponibilidadRepositorio, DisponibilidadRepositorio>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            
            // Configuración de Swagger para Scalar
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Gestion de Spa - API",
                    Version = "v1",
                    Description = "API para gestión de Spa - Sistema completo de ventas, servicios, clientes y reportes"
                });
            });


            // -----------------------------------------------------------------
            // 2. CONSTRUYE LA APLICACIÓN (SOLO UNA VEZ)
            // -----------------------------------------------------------------
            var app = builder.Build();

            // -----------------------------------------------------------------
            // 3. CONFIGURA EL PIPELINE DE HTTP (SOLO CÓDIGO 'app.Use...')
            // -----------------------------------------------------------------

            if (app.Environment.IsDevelopment())
            {
                // Habilitar Swagger
                app.UseSwagger(options =>
                {
                    options.RouteTemplate = "openapi/{documentName}.json";
                });
                
                // Usar Scalar en lugar de SwaggerUI
                app.MapScalarApiReference(options =>
                {
                    options
                        .WithTitle("Spa Management API")
                        .WithTheme(ScalarTheme.Purple)
                        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                });
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.MapControllers();

            // -----------------------------------------------------------------
            // 4. EJECUTA LA APLICACIÓN
            // -----------------------------------------------------------------
            app.Run();
        }
    }
}

