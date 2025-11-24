using Aplication.DTOs;
using Aplication.UseCases;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class ComisionesController : ControllerBase
 {
 private readonly CalcularComision _calcularComision;

 public ComisionesController(CalcularComision calcularComision)
 {
 _calcularComision = calcularComision;
 }

 /// <summary>
 /// Calcula la comisión total de un empleado en un rango de fechas
 /// </summary>
 [HttpGet("calcular")]
 public async Task<IActionResult> CalcularComision([FromQuery] int empleadaId, [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
 {
 try
 {
 if (empleadaId <=0 || fechaInicio > fechaFin)
 {
 return BadRequest(new { error = "Parámetros inválidos." });
 }

 var comision = await _calcularComision.EjecutarAsync(empleadaId, fechaInicio, fechaFin);

 return Ok(new
 {
 mensaje = "Comisión calculada exitosamente",
 data = comision
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
