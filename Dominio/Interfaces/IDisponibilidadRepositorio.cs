using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface IDisponibilidadRepositorio
    {
        /// <summary>
        /// Verifica si existe disponibilidad en un horario específico
        /// </summary>
        Task<bool> VerificarDisponibilidadAsync(int empleadaId, DateTime fecha, TimeSpan hora, int duracion);

        /// <summary>
        /// Obtiene todas las citas de una empleada en una fecha específica
        /// </summary>
        Task<IEnumerable<Cita>> ObtenerCitasDelDiaAsync(int empleadaId, DateTime fecha);

        /// <summary>
        /// Detecta conflictos de horario con citas existentes
      /// </summary>
        Task<bool> ExisteConflictoHorarioAsync(int empleadaId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin);
    }
}
