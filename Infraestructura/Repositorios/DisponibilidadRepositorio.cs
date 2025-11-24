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
    public class DisponibilidadRepositorio : IDisponibilidadRepositorio
    {
     private readonly AppDbContext _context;

    public DisponibilidadRepositorio(AppDbContext context)
        {
      _context = context;
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

       
        public async Task<bool> ExisteConflictoHorarioAsync(int empleadaId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            var conflicto = await _context.Citas
        .Where(c => c.EmpleadaId == empleadaId
         && c.Fecha.Date == fecha.Date
      && !(c.HoraFin <= horaInicio || c.HoraInicio >= horaFin))
  .AnyAsync();

     return conflicto;
        }
    }
}
