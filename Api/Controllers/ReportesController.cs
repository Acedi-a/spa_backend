using Aplication.DTOs;
using Aplication.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly GenerarReportePeriodico _generarReportePeriodico;

        public ReportesController(GenerarReportePeriodico generarReportePeriodico)
        {
            _generarReportePeriodico = generarReportePeriodico;
        }

        /// <summary>
        /// Genera un reporte de ventas por rango de fechas
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio del reporte (formato: 2024-11-01)</param>
        /// <param name="fechaFin">Fecha de fin del reporte (formato: 2024-11-30)</param>
        /// <param name="clienteId">ID del cliente (opcional, para filtrar ventas de un cliente específico)</param>
        /// <returns>Reporte con estadísticas y detalles de ventas</returns>
        [HttpGet("ventas")]
        public async Task<IActionResult> GenerarReporteVentas(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin,
            [FromQuery] Guid? clienteId = null)
        {
            try
            {
                // Validar fechas
                if (fechaInicio > fechaFin)
                {
                    return BadRequest(new { error = "La fecha de inicio no puede ser mayor que la fecha de fin" });
                }

                var request = new GenerarReporteRequestDto
                {
                    TipoReporte = "Ventas",
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    ClienteId = clienteId
                };

                var reporte = await _generarReportePeriodico.EjecutarAsync(request);

                return Ok(reporte);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al generar el reporte", detalle = ex.Message });
            }
        }
    }
}
