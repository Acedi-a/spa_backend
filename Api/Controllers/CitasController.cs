using Aplication.DTOs;
using Aplication.UseCases;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
 [Route("api/[controller]")]
    public class CitasController : ControllerBase
    {
    private readonly CrearCita _crearCita;
        private readonly ICitaRepositorio _citaRepositorio;
  private readonly IMapper _mapper;

        public CitasController(CrearCita crearCita, ICitaRepositorio citaRepositorio, IMapper mapper)
{
   _crearCita = crearCita;
     _citaRepositorio = citaRepositorio;
      _mapper = mapper;
        }

     [HttpPost]
     public async Task<IActionResult> RegistrarCita([FromBody] CitaDTO citaDto)
        {
     try
            {
      if (!ModelState.IsValid)
        {
   return BadRequest(ModelState);
              }

     var cita = _mapper.Map<Cita>(citaDto);
   await _crearCita.EjecutarAsync(cita);

   return CreatedAtAction(
        nameof(ObtenerCitaPorId),
   new { id = cita.Id },
   new { id = cita.Id, mensaje = "Cita registrada exitosamente" }
         );
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

        [HttpGet]
      public async Task<IActionResult> ListarCitas()
        {
    try
        {
      var citas = await _citaRepositorio.ListarTodosAsync();
   var citasDto = _mapper.Map<IEnumerable<CitaDTO>>(citas);

   return Ok(new
 {
     mensaje = "Citas listadas exitosamente",
  data = citasDto
        });
           }
   catch (Exception ex)
            {
       return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
 }
}

   [HttpGet("{id}")]
     public async Task<IActionResult> ObtenerCitaPorId(int id)
        {
      try
         {
   var cita = await _citaRepositorio.ObtenerPorIdAsync(id);

          if (cita == null)
    {
      return NotFound(new { error = $"La cita con ID {id} no existe." });
       }

    var citaDto = _mapper.Map<CitaDTO>(cita);

      return Ok(new
         {
   mensaje = "Cita obtenida exitosamente",
       data = citaDto
     });
    }
       catch (Exception ex)
          {
    return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
         }
    }

   [HttpGet("empleada/{empleadaId}")]
  public async Task<IActionResult> ListarCitasPorEmpleada(int empleadaId)
        {
       try
  {
   var citas = await _citaRepositorio.ListarPorEmpleadaAsync(empleadaId);
         var citasDto = _mapper.Map<IEnumerable<CitaDTO>>(citas);

  return Ok(new
  {
   mensaje = "Citas de la empleada listadas exitosamente",
        data = citasDto
    });
          }
    catch (Exception ex)
              {
     return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
           }
 }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCita(int id, [FromBody] CitaDTO citaDto)
        {
  try
      {
      if (!ModelState.IsValid)
      {
     return BadRequest(ModelState);
     }

    var citaExistente = await _citaRepositorio.ObtenerPorIdAsync(id);

     if (citaExistente == null)
    {
     return NotFound(new { error = $"La cita con ID {id} no existe." });
          }

  // Validaciones de negocio
  ValidarCitaActualizacion(citaDto);

 // Si la fecha u hora cambió, verificar disponibilidad
  if (citaExistente.Fecha != citaDto.Fecha || citaExistente.Hora != citaDto.Hora)
             {
  // Obtener la cita existente para acceder al servicio
   var citaConServicio = await _citaRepositorio.ObtenerPorIdAsync(id);
     if (citaConServicio?.Servicio == null)
   {
   return BadRequest(new { error = "No se puede obtener la información del servicio." });
    }

          var disponible = await _citaRepositorio.VerificarDisponibilidadAsync(
               citaDto.EmpleadaId,
            citaDto.Fecha,
       citaDto.Hora,
            citaConServicio.Servicio.Duracion
           );

            if (!disponible)
       {
     return BadRequest(new { error = "La empleada no está disponible en el nuevo horario." });
         }
        }

         // Mapear datos
 citaExistente.ClienteId = citaDto.ClienteId;
                citaExistente.ServicioId = citaDto.ServicioId;
                citaExistente.EmpleadaId = citaDto.EmpleadaId;
      citaExistente.Fecha = citaDto.Fecha;
  citaExistente.Hora = citaDto.Hora;
             citaExistente.Estado = citaDto.Estado ?? "Confirmada";

         // IMPORTANTE: Recalcular HoraInicio y HoraFin basados en la nueva hora y duración del servicio
                var servicio = await _citaRepositorio.ObtenerPorIdAsync(id);
         if (servicio?.Servicio != null)
        {
citaExistente.HoraInicio = citaExistente.Hora;
         citaExistente.HoraFin = citaExistente.Hora.Add(TimeSpan.FromMinutes(servicio.Servicio.Duracion));
            }

                await _citaRepositorio.ActualizarAsync(citaExistente);

        return Ok(new
    {
         id = citaExistente.Id,
         mensaje = "Cita actualizada exitosamente"
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

    private void ValidarCitaActualizacion(CitaDTO citaDto)
        {
    // Validar cliente
    if (citaDto.ClienteId == Guid.Empty)
        {
     throw new ArgumentException("El cliente es obligatorio.");
           }

   // Validar servicio
        if (citaDto.ServicioId <= 0)
           {
       throw new ArgumentException("El servicio es obligatorio.");
           }

   // Validar empleada
        if (citaDto.EmpleadaId <= 0)
           {
     throw new ArgumentException("La empleada es obligatoria.");
    }

          // Validar fecha
    if (citaDto.Fecha.Date <= DateTime.Now.Date)
{
           throw new ArgumentException("La fecha de la cita debe ser posterior a hoy.");
  }

       // Validar hora
    if (citaDto.Hora.TotalHours < 8 || citaDto.Hora.TotalHours > 20)
   {
        throw new ArgumentException("La cita debe estar entre las 8:00 AM y las 8:00 PM.");
         }
        }
    }
}
