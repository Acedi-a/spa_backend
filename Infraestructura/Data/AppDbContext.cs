using Dominio.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infraestructura.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        //Entidades de ejemplo
        public DbSet<Docente> Docentes { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        
        // Entidades del Sistema de Spa
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Empleada> Empleadas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<Valoracion> Valoraciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de precisión para campos decimales
            modelBuilder.Entity<Empleada>()
                .Property(e => e.PorcentajeComision)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Servicio>()
                .Property(s => s.Precio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Venta>()
                .Property(v => v.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DetalleVenta>()
                .Property(dv => dv.PrecioUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DetalleVenta>()
                .Property(dv => dv.Subtotal)
                .HasPrecision(18, 2);

            // Configuración de relaciones con DeleteBehavior.Restrict para evitar cascadas no deseadas
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Cliente)
                .WithMany(cl => cl.Citas)
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Servicio)
                .WithMany(s => s.Citas)
                .HasForeignKey(c => c.ServicioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Empleada)
                .WithMany(e => e.Citas)
                .HasForeignKey(c => c.EmpleadaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Ventas)
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Venta)
                .WithMany(v => v.DetalleVentas)
                .HasForeignKey(dv => dv.VentaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Producto)
                .WithMany(p => p.DetalleVentas)
                .HasForeignKey(dv => dv.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Servicio)
                .WithMany(s => s.DetalleVentas)
                .HasForeignKey(dv => dv.ServicioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Valoracion>()
                .HasOne(val => val.Cliente)
                .WithMany(c => c.Valoraciones)
                .HasForeignKey(val => val.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Valoracion>()
                .HasOne(val => val.Venta)
                .WithMany(v => v.Valoraciones)
                .HasForeignKey(val => val.VentaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Valoracion>()
                .HasOne(val => val.Servicio)
                .WithMany(s => s.Valoraciones)
                .HasForeignKey(val => val.ServicioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Servicio>()
                .HasOne(s => s.Categoria)
                .WithMany(c => c.Servicios)
                .HasForeignKey(s => s.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Servicio>()
                .HasOne(s => s.Empleada)
                .WithMany(e => e.Servicios)
                .HasForeignKey(s => s.EmpleadaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
