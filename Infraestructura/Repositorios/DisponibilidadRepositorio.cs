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

        /// <summary>
        /// Verifica si existe disponibilidad en un horario específico
        /// </summary>
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

     /// <summary>
      /// Obtiene todas las citas de una empleada en una fecha específica, ordenadas por hora
        /// </summary>
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

        /// <summary>
        /// Detecta si existe conflicto de horario entre una hora solicitada y las citas existentes
        /// </summary>
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
