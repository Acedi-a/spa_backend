using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositorios
{
    public class ValoracionRepositorio : IValoracionRepositorio
    {
        private readonly AppDbContext _context;

      public ValoracionRepositorio(AppDbContext context)
        {
      _context = context;
        }

      
        public async Task<Valoracion?> ObtenerPorIdAsync(int id)
       {
            return await _context.Valoraciones
        .Include(v => v.Cliente)
      .Include(v => v.Servicio)
          .Include(v => v.Venta)
    .FirstOrDefaultAsync(v => v.Id == id);
        }

   
   public async Task<IEnumerable<Valoracion>> ListarTodosAsync()
    {
      return await _context.Valoraciones
        .Include(v => v.Cliente)
                .Include(v => v.Servicio)
         .Include(v => v.Venta)
      .OrderByDescending(v => v.Fecha)
    .ToListAsync();
      }

       
       public async Task<IEnumerable<Valoracion>> ObtenerPorEmpleadoAsync(int empleadoId)
 {
  // Las valoraciones se vinculan al empleado a través del Servicio
        return await _context.Valoraciones
         .Where(v => v.Servicio!.EmpleadaId == empleadoId)
         .Include(v => v.Cliente)
      .Include(v => v.Servicio)
              .Include(v => v.Venta)
             .OrderByDescending(v => v.Fecha)
            .ToListAsync();
        }

 
        public async Task<IEnumerable<Valoracion>> ObtenerPorClienteAsync(Guid clienteId)
       {
        return await _context.Valoraciones
         .Where(v => v.ClienteId == clienteId)
          .Include(v => v.Cliente)
           .Include(v => v.Servicio)
   .Include(v => v.Venta)
           .OrderByDescending(v => v.Fecha)
     .ToListAsync();
  }

  
        public async Task<IEnumerable<Valoracion>> ObtenerPorEmpleadoYFechaAsync(int empleadoId, DateTime fechaInicio, DateTime fechaFin)
   {
           return await _context.Valoraciones
      .Where(v => v.Servicio!.EmpleadaId == empleadoId
  && v.Fecha.Date >= fechaInicio.Date
                && v.Fecha.Date <= fechaFin.Date)
    .Include(v => v.Cliente)
 .Include(v => v.Servicio)
              .Include(v => v.Venta)
            .OrderByDescending(v => v.Fecha)
       .ToListAsync();
      }

   
   public async Task CrearAsync(Valoracion valoracion)
    {
    // Validar que el servicio existe
          var servicioExiste = await _context.Servicios.FindAsync(valoracion.ServicioId);
if (servicioExiste == null)
        {
       throw new ArgumentException($"El servicio con ID {valoracion.ServicioId} no existe.");
  }

      // Validar que el cliente existe
            var clienteExiste = await _context.Clientes.FindAsync(valoracion.ClienteId);
    if (clienteExiste == null)
       {
     throw new ArgumentException($"El cliente con ID {valoracion.ClienteId} no existe.");
 }

      // Validar que la venta existe si se proporciona
            if (valoracion.VentaId > 0)
   {
       var ventaExiste = await _context.Ventas.FindAsync(valoracion.VentaId);
           if (ventaExiste == null)
              {
              throw new ArgumentException($"La venta con ID {valoracion.VentaId} no existe.");
            }
   }

   await _context.Valoraciones.AddAsync(valoracion);
      await _context.SaveChangesAsync();
        }

       
     public async Task ActualizarAsync(Valoracion valoracion)
  {
  // Validar que el servicio existe
     var servicioExiste = await _context.Servicios.FindAsync(valoracion.ServicioId);
       if (servicioExiste == null)
   {
   throw new ArgumentException($"El servicio con ID {valoracion.ServicioId} no existe.");
   }

       // Validar que el cliente existe
  var clienteExiste = await _context.Clientes.FindAsync(valoracion.ClienteId);
           if (clienteExiste == null)
     {
 throw new ArgumentException($"El cliente con ID {valoracion.ClienteId} no existe.");
}

 _context.Valoraciones.Update(valoracion);
      await _context.SaveChangesAsync();
        }

    
        public async Task EliminarAsync(int id)
        {
 var valoracion = await ObtenerPorIdAsync(id);
       if (valoracion != null)
    {
       _context.Valoraciones.Remove(valoracion);
      await _context.SaveChangesAsync();
        }
        }

      
       public async Task<double> ObtenerPromedioCalificacionAsync(int empleadoId)
      {
       var promedio = await _context.Valoraciones
  .Where(v => v.Servicio!.EmpleadaId == empleadoId)
          .AverageAsync(v => (double)v.Calificacion);

        return Math.Round(promedio, 2);
       }

 
    public async Task<double> ObtenerPromedioCalificacionPorFechaAsync(int empleadoId, DateTime fechaInicio, DateTime fechaFin)
        {
       var promedio = await _context.Valoraciones
        .Where(v => v.Servicio!.EmpleadaId == empleadoId
      && v.Fecha.Date >= fechaInicio.Date
  && v.Fecha.Date <= fechaFin.Date)
   .AverageAsync(v => (double)v.Calificacion);

              return Math.Round(promedio, 2);
        }
    }
}
