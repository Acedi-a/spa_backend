using Aplication.DTOs;
using Aplication.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
 public class DisponibilidadController : ControllerBase
{
      private readonly ValidarDisponibilidad _validarDisponibilidad;

    public DisponibilidadController(ValidarDisponibilidad validarDisponibilidad)
        {
            _validarDisponibilidad = validarDisponibilidad;
        }

        /// <summary>
        /// Valida si existe disponibilidad en un horario específico
        /// </summary>
   /// <remarks>
      /// Responde con 200 OK si el horario está disponible
        /// Responde con 409 Conflict si hay un conflicto de horario
        /// </remarks>
        [HttpPost("validar")]
      public async Task<IActionResult> ValidarDisponibilidadHorario([FromBody] DisponibilidadDTO disponibilidadDto)
        {
    try
       {
           if (!ModelState.IsValid)
  {
       return BadRequest(ModelState);
       }

      var disponible = await _validarDisponibilidad.VerificarDisponibilidadAsync(
  disponibilidadDto.EmpleadaId,
         disponibilidadDto.Fecha,
             disponibilidadDto.Hora,
          disponibilidadDto.Duracion
      );

    if (disponible)
        {
  return Ok(new
               {
   mensaje = "El horario está disponible",
         disponible = true,
         horaInicio = disponibilidadDto.Hora,
            horaFin = disponibilidadDto.Hora.Add(TimeSpan.FromMinutes(disponibilidadDto.Duracion))
      });
     }
      else
          {
            return StatusCode(409, new
         {
            error = "El horario no está disponible. La empleada tiene una cita en ese horario.",
             disponible = false
    });
  }
            }
      catch (ArgumentException ex)
            {
  return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
        {
         return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
 }
        }

        /// <summary>
        /// Obtiene los horarios disponibles para una empleada en una fecha específica
        /// </summary>
        /// <remarks>
        /// Retorna una lista de huecos libres donde se puede agendar una cita
   /// Si no hay disponibilidad, retorna una lista vacía
        /// </remarks>
        [HttpGet("horarios-disponibles")]
        public async Task<IActionResult> ObtenerHorariosDisponibles(
    [FromQuery] int empleadaId,
            [FromQuery] DateTime fecha,
            [FromQuery] int duracion)
        {
      try
            {
       if (empleadaId <= 0 || duracion <= 0)
           {
              return BadRequest(new { error = "EmpleadaId y Duracion deben ser mayores a 0" });
     }

    var horariosDisponibles = await _validarDisponibilidad.ObtenerHorariosDisponiblesAsync(empleadaId, fecha, duracion);

       if (horariosDisponibles.Count == 0)
                {
                  return Ok(new
            {
               mensaje = "No hay horarios disponibles para esa fecha",
               disponibles = new List<HorarioDisponibleDTO>()
   });
     }

            return Ok(new
      {
          mensaje = "Horarios disponibles obtenidos exitosamente",
      total = horariosDisponibles.Count,
                disponibles = horariosDisponibles
      });
     }
         catch (ArgumentException ex)
  {
           return BadRequest(new { error = ex.Message });
        }
            catch (Exception ex)
            {
  return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
        }
      }

/// <summary>
        /// Detecta conflictos de horario entre dos horas específicas
   /// </summary>
        [HttpPost("detectar-conflicto")]
        public async Task<IActionResult> DetectarConflicto([FromBody] DetectarConflictoRequestDTO request)
        {
     try
         {
         if (!ModelState.IsValid)
                {
  return BadRequest(ModelState);
        }

      var tieneConflicto = await _validarDisponibilidad.DetectarConflictoAsync(
   request.EmpleadaId,
          request.Fecha,
request.HoraInicio,
   request.HoraFin
);

    return Ok(new
         {
        empleadaId = request.EmpleadaId,
           fecha = request.Fecha,
          horaInicio = request.HoraInicio,
      horaFin = request.HoraFin,
         tieneConflicto = tieneConflicto,
 mensaje = tieneConflicto
            ? "Existe un conflicto de horario"
        : "No hay conflictos de horario"
       });
         }
            catch (ArgumentException ex)
       {
 return BadRequest(new { error = ex.Message });
   }
     catch (Exception ex)
 {
       return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }
    }
}
