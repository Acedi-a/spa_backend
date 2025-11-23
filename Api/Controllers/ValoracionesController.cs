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
   public class ValoracionesController : ControllerBase
    {
     private readonly CrearValoracion _crearValoracion;
private readonly GenerarReporteDesempeño _generarReporteDesempeño;
       private readonly IValoracionRepositorio _valoracionRepositorio;
  private readonly IMapper _mapper;

   public ValoracionesController(CrearValoracion crearValoracion, GenerarReporteDesempeño generarReporteDesempeño, IValoracionRepositorio valoracionRepositorio, IMapper mapper)
        {
   _crearValoracion = crearValoracion;
          _generarReporteDesempeño = generarReporteDesempeño;
         _valoracionRepositorio = valoracionRepositorio;
      _mapper = mapper;
   }

     /// <summary>
        /// Registra una nueva valoración de desempeño
 /// </summary>
  [HttpPost]
   public async Task<IActionResult> RegistrarValoracion([FromBody] ValoracionCrearDTO valoracionDto)
       {
 try
    {
        if (!ModelState.IsValid)
   {
            return BadRequest(ModelState);
      }

        var valoracion = _mapper.Map<Valoracion>(valoracionDto);
       await _crearValoracion.EjecutarAsync(valoracion);

   return CreatedAtAction(
       nameof(ObtenerValoracionPorId),
    new { id = valoracion.Id },
      new { id = valoracion.Id, mensaje = "Valoración registrada exitosamente" }
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

   /// <summary>
       /// Obtiene todas las valoraciones
  /// </summary>
  [HttpGet]
 public async Task<IActionResult> ListarValoraciones()
     {
    try
      {
           var valoraciones = await _valoracionRepositorio.ListarTodosAsync();
   var valoracionesDto = _mapper.Map<IEnumerable<ValoracionDTO>>(valoraciones);

 return Ok(new
        {
  mensaje = "Valoraciones listadas exitosamente",
     data = valoracionesDto
           });
     }
  catch (Exception ex)
 {
 return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
      }
   }

        /// <summary>
      /// Obtiene una valoración por ID
 /// </summary>
 [HttpGet("{id}")]
      public async Task<IActionResult> ObtenerValoracionPorId(int id)
 {
       try
    {
  var valoracion = await _valoracionRepositorio.ObtenerPorIdAsync(id);

  if (valoracion == null)
   {
     return NotFound(new { error = $"La valoración con ID {id} no existe." });
        }

     var valoracionDto = _mapper.Map<ValoracionDTO>(valoracion);

     return Ok(new
         {
 mensaje = "Valoración obtenida exitosamente",
          data = valoracionDto
    });
      }
  catch (Exception ex)
     {
    return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
         }
    }

 /// <summary>
  /// Obtiene todas las valoraciones de un cliente
       /// </summary>
   [HttpGet("cliente/{clienteId}")]
 public async Task<IActionResult> ObtenerValoracionesPorCliente(Guid clienteId)
      {
   try
    {
   if (clienteId == Guid.Empty)
     {
  return BadRequest(new { error = "ClienteId inválido." });
 }

        var valoraciones = await _valoracionRepositorio.ObtenerPorClienteAsync(clienteId);
    var valoracionesDto = _mapper.Map<IEnumerable<ValoracionDTO>>(valoraciones);

    return Ok(new
 {
      mensaje = "Valoraciones del cliente obtenidas exitosamente",
   data = valoracionesDto
          });
        }
  catch (Exception ex)
     {
       return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
   }
    }

   /// <summary>
        /// Genera un reporte de desempeño de una empleada
      /// </summary>
     [HttpGet("reporte/empleada/{empleadaId}")]
      public async Task<IActionResult> GenerarReporteEmpleada(int empleadaId)
  {
    try
      {
       var reporte = await _generarReporteDesempeño.EjecutarAsync(empleadaId);

       return Ok(new
   {
      mensaje = "Reporte de desempeño generado exitosamente",
   data = reporte
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
        /// Genera un reporte de desempeño de una empleada en un rango de fechas
       /// </summary>
[HttpGet("reporte/empleada/{empleadaId}/fechas")]
     public async Task<IActionResult> GenerarReporteEmpleadaPorFechas(int empleadaId, [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
      {
try
     {
  if (fechaInicio > fechaFin)
      {
      return BadRequest(new { error = "La fecha de inicio no puede ser mayor a la fecha de fin." });
      }

 var reporte = await _generarReporteDesempeño.EjecutarAsync(empleadaId, fechaInicio, fechaFin);

  return Ok(new
    {
 mensaje = "Reporte de desempeño generado exitosamente",
  data = reporte
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
    /// Obtiene el promedio de calificación de una empleada
      /// </summary>
   [HttpGet("promedio/empleada/{empleadaId}")]
  public async Task<IActionResult> ObtenerPromedioEmpleada(int empleadaId)
       {
    try
           {
   if (empleadaId <= 0)
     {
 return BadRequest(new { error = "EmpleadaId inválido." });
       }

 var promedio = await _valoracionRepositorio.ObtenerPromedioCalificacionAsync(empleadaId);

    return Ok(new
       {
 empleadaId = empleadaId,
  promedioCalificacion = promedio,
     mensaje = "Promedio calculado exitosamente"
   });
      }
       catch (Exception ex)
    {
 return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
 }
   }
    }
}
