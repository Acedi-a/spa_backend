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
  public class CitaRepositorio : ICitaRepositorio
    {
   private readonly AppDbContext _context;

        public CitaRepositorio(AppDbContext context)
   {
            _context = context;
  }

        public async Task<Cita?> ObtenerPorIdAsync(int id)
     {
      return await _context.Citas
         .Include(c => c.Cliente)
         .Include(c => c.Servicio)
      .Include(c => c.Empleada)
           .FirstOrDefaultAsync(c => c.Id == id);
 }

        public async Task<IEnumerable<Cita>> ListarTodosAsync()
      {
          return await _context.Citas
     .Include(c => c.Cliente)
         .Include(c => c.Servicio)
          .Include(c => c.Empleada)
         .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> ListarPorEmpleadaAsync(int empleadaId)
       {
        return await _context.Citas
     .Where(c => c.EmpleadaId == empleadaId)
    .Include(c => c.Cliente)
 .Include(c => c.Servicio)
    .Include(c => c.Empleada)
    .ToListAsync();
         }

        public async Task<IEnumerable<Cita>> ObtenerCitasDelDiaAsync(int empleadaId, DateTime fecha)
   {
    return await _context.Citas
   .Where(c => c.EmpleadaId == empleadaId && c.Fecha.Date == fecha.Date)
 .Include(c => c.Cliente)
           .Include(c => c.Servicio)
         .Include(c => c.Empleada)
            .OrderBy(c => c.HoraInicio)
   .ToListAsync();
        }

    public async Task<bool> VerificarDisponibilidadAsync(int empleadaId, DateTime fecha, TimeSpan hora, int duracion)
        {
          var horaFin = hora.Add(TimeSpan.FromMinutes(duracion));

           // Buscar si hay conflictos de horario para la empleada en esa fecha
          var conflicto = await _context.Citas
           .Where(c => c.EmpleadaId == empleadaId 
               && c.Fecha.Date == fecha.Date
   && !(c.HoraFin <= hora || c.HoraInicio >= horaFin))
       .AnyAsync();

            return !conflicto; // Retorna true si está disponible (no hay conflicto)
        }

       public async Task CrearAsync(Cita cita)
        {
       // Validar que el cliente existe
    var clienteExiste = await _context.Clientes.FindAsync(cita.ClienteId);
    if (clienteExiste == null)
        {
      throw new ArgumentException($"El cliente con ID {cita.ClienteId} no existe.");
      }

     // Validar que el servicio existe
        var servicioExiste = await _context.Servicios.FindAsync(cita.ServicioId);
   if (servicioExiste == null)
           {
        throw new ArgumentException($"El servicio con ID {cita.ServicioId} no existe.");
         }

   // Validar que la empleada existe
        var empleadaExiste = await _context.Empleadas.FindAsync(cita.EmpleadaId);
       if (empleadaExiste == null)
       {
          throw new ArgumentException($"La empleada con ID {cita.EmpleadaId} no existe.");
     }

  await _context.Citas.AddAsync(cita);
     await _context.SaveChangesAsync();
        }

   public async Task ActualizarAsync(Cita cita)
      {
        // Validar que el cliente existe
  var clienteExiste = await _context.Clientes.FindAsync(cita.ClienteId);
      if (clienteExiste == null)
         {
     throw new ArgumentException($"El cliente con ID {cita.ClienteId} no existe.");
              }

    // Validar que el servicio existe
      var servicioExiste = await _context.Servicios.FindAsync(cita.ServicioId);
       if (servicioExiste == null)
         {
    throw new ArgumentException($"El servicio con ID {cita.ServicioId} no existe.");
         }

     // Validar que la empleada existe
         var empleadaExiste = await _context.Empleadas.FindAsync(cita.EmpleadaId);
        if (empleadaExiste == null)
         {
      throw new ArgumentException($"La empleada con ID {cita.EmpleadaId} no existe.");
 }

 _context.Citas.Update(cita);
   await _context.SaveChangesAsync();
        }

     public async Task EliminarAsync(int id)
        {
var cita = await ObtenerPorIdAsync(id);
       if (cita != null)
           {
         _context.Citas.Remove(cita);
      await _context.SaveChangesAsync();
       }
   }
    }
}
