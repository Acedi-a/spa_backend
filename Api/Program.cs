using Aplication.Mapping;
using Aplication.UseCases;
using Dominio.Interfaces;
using Infraestructura.Data;
using Infraestructura.Repositorios;
using Microsoft.EntityFrameworkCore;

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
            // 1. AGREGA TODOS TUS SERVICIOS AQU�
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

            builder.Services.AddScoped<IVentaRepositorio, VentaRepositorio>();

            // Registrar servicios para Producto
            builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
            builder.Services.AddScoped<CrearProducto>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // -----------------------------------------------------------------
            // 2. CONSTRUYE LA APLICACI�N (SOLO UNA VEZ)
            // -----------------------------------------------------------------
            var app = builder.Build();

            // -----------------------------------------------------------------
            // 3. CONFIGURA EL PIPELINE DE HTTP (SOLO C�DIGO 'app.Use...')
            // -----------------------------------------------------------------

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.MapControllers();

            // -----------------------------------------------------------------
            // 4. EJECUTA LA APLICACI�N
            // -----------------------------------------------------------------
            app.Run();
        }
    }
}
