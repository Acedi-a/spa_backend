using Dominio.Entities;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class CrearCita
    {
      private readonly ICitaRepositorio _citaRepositorio;
        private readonly IServicioRepositorio _servicioRepositorio;

        public CrearCita(ICitaRepositorio citaRepositorio, IServicioRepositorio servicioRepositorio)
        {
    _citaRepositorio = citaRepositorio;
         _servicioRepositorio = servicioRepositorio;
        }

        public async Task EjecutarAsync(Cita cita)
        {
       // Validar datos de entrada
            ValidarCita(cita);

            // Obtener el servicio para conocer la duración
            var servicio = await _servicioRepositorio.ObtenerPorIdAsync(cita.ServicioId);
            if (servicio == null)
   {
       throw new ArgumentException($"El servicio con ID {cita.ServicioId} no existe.");
            }

// Verificar disponibilidad de la empleada en ese horario
       var disponible = await _citaRepositorio.VerificarDisponibilidadAsync(
         cita.EmpleadaId, 
     cita.Fecha, 
     cita.Hora, 
        servicio.Duracion
         );

       if (!disponible)
            {
         throw new ArgumentException("La empleada no está disponible en ese horario. Por favor, selecciona otro horario.");
    }

  // Calcular hora de inicio y fin basado en la duración del servicio
            cita.HoraInicio = cita.Hora;
          cita.HoraFin = cita.Hora.Add(TimeSpan.FromMinutes(servicio.Duracion));

            // Establecer estado por defecto
            if (string.IsNullOrWhiteSpace(cita.Estado))
            {
     cita.Estado = "Confirmada";
          }

          // Crear la cita
            await _citaRepositorio.CrearAsync(cita);
        }

     private void ValidarCita(Cita cita)
        {
 // Validar cliente
       if (cita.ClienteId == Guid.Empty)
            {
     throw new ArgumentException("El cliente es obligatorio.");
   }

            // Validar servicio
            if (cita.ServicioId <= 0)
   {
           throw new ArgumentException("El servicio es obligatorio.");
            }

     // Validar empleada
            if (cita.EmpleadaId <= 0)
      {
     throw new ArgumentException("La empleada es obligatoria.");
         }

    // Validar fecha
       if (cita.Fecha.Date <= DateTime.Now.Date)
 {
    throw new ArgumentException("La fecha de la cita debe ser posterior a hoy.");
            }

       // Validar hora
            if (cita.Hora.TotalHours < 8 || cita.Hora.TotalHours > 20)
   {
     throw new ArgumentException("La cita debe estar entre las 8:00 AM y las 8:00 PM.");
            }
        }
    }
}
