using Aplication.DTOs;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class ValidarDisponibilidad
    {
        private readonly IDisponibilidadRepositorio _disponibilidadRepositorio;
   private readonly TimeSpan _horaApertura = new TimeSpan(8, 0, 0);  // 8:00 AM
        private readonly TimeSpan _horaCierre = new TimeSpan(20, 0, 0); // 8:00 PM

     public ValidarDisponibilidad(IDisponibilidadRepositorio disponibilidadRepositorio)
      {
  _disponibilidadRepositorio = disponibilidadRepositorio;
        }

        /// <summary>
    /// Valida si existe disponibilidad en la hora solicitada
        /// </summary>
    public async Task<bool> VerificarDisponibilidadAsync(int empleadaId, DateTime fecha, TimeSpan hora, int duracion)
      {
ValidarDatos(empleadaId, fecha, hora, duracion);
       return await _disponibilidadRepositorio.VerificarDisponibilidadAsync(empleadaId, fecha, hora, duracion);
        }

        /// <summary>
  /// Obtiene los horarios disponibles para una empleada en una fecha específica
        /// </summary>
  public async Task<List<HorarioDisponibleDTO>> ObtenerHorariosDisponiblesAsync(int empleadaId, DateTime fecha, int duracion)
        {
      ValidarDatos(empleadaId, fecha, _horaApertura, duracion);

 // Obtener todas las citas del día para esa empleada desde el repositorio
    var citasDelDia = await _disponibilidadRepositorio.ObtenerCitasDelDiaAsync(empleadaId, fecha);

       // Calcular horarios disponibles (lógica del Use Case)
     var horariosDisponibles = CalcularHorariosDisponibles(citasDelDia, duracion, fecha);

return horariosDisponibles;
  }

  /// <summary>
      /// Detecta conflictos de horario entre una hora solicitada y las citas existentes
      /// </summary>
     public async Task<bool> DetectarConflictoAsync(int empleadaId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
    ValidarDatos(empleadaId, fecha, horaInicio, (int)(horaFin - horaInicio).TotalMinutes);
       return await _disponibilidadRepositorio.ExisteConflictoHorarioAsync(empleadaId, fecha, horaInicio, horaFin);
 }

        /// <summary>
  /// Calcula los huecos libres entre las citas existentes
/// Esta lógica reside en el Use Case (no en el repositorio)
        /// </summary>
     private List<HorarioDisponibleDTO> CalcularHorariosDisponibles(IEnumerable<Dominio.Entities.Cita> citas, int duracion, DateTime fecha)
  {
    var horariosDisponibles = new List<HorarioDisponibleDTO>();
  var citasOrdenadas = citas.OrderBy(c => c.HoraInicio).ToList();

            var horaActual = _horaApertura;

         foreach (var cita in citasOrdenadas)
            {
              // Si hay espacio entre la hora actual y el inicio de la cita
       if (cita.HoraInicio > horaActual)
       {
          var duracionDisponible = (int)(cita.HoraInicio - horaActual).TotalMinutes;

     // Si el hueco es suficiente para la duración solicitada
           if (duracionDisponible >= duracion)
             {
           horariosDisponibles.Add(new HorarioDisponibleDTO
              {
        Fecha = fecha,
             HoraInicio = horaActual,
 HoraFin = horaActual.Add(TimeSpan.FromMinutes(duracion)),
   DuracionDisponible = duracionDisponible
             });
      }
         }

       // Actualizar hora actual al final de la cita
       horaActual = cita.HoraFin > horaActual ? cita.HoraFin : horaActual;
            }

        // Verificar si hay espacio después de la última cita hasta el cierre
      if (horaActual < _horaCierre)
          {
    var duracionDisponible = (int)(_horaCierre - horaActual).TotalMinutes;

           if (duracionDisponible >= duracion)
     {
      horariosDisponibles.Add(new HorarioDisponibleDTO
         {
  Fecha = fecha,
      HoraInicio = horaActual,
HoraFin = horaActual.Add(TimeSpan.FromMinutes(duracion)),
        DuracionDisponible = duracionDisponible
    });
      }
}

            return horariosDisponibles;
   }

        private void ValidarDatos(int empleadaId, DateTime fecha, TimeSpan hora, int duracion)
 {
   if (empleadaId <= 0)
       {
         throw new ArgumentException("El ID de la empleada es inválido.");
     }

        if (fecha.Date <= DateTime.Now.Date)
            {
 throw new ArgumentException("La fecha debe ser posterior a hoy.");
            }

       if (duracion <= 0 || duracion > 480)
          {
     throw new ArgumentException("La duración debe estar entre 1 y 480 minutos.");
    }

  if (hora.TotalHours < 8 || hora.TotalHours > 20)
            {
     throw new ArgumentException("La hora debe estar entre las 8:00 AM y las 8:00 PM.");
         }
 }
    }
}
