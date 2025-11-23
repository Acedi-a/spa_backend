using Aplication.DTOs;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class GenerarReportePeriodico
    {
        private readonly IVentaRepositorio _ventaRepositorio;

        public GenerarReportePeriodico(IVentaRepositorio ventaRepositorio)
        {
            _ventaRepositorio = ventaRepositorio;
        }

        public async Task<ReportePeriodicoDto> EjecutarAsync(GenerarReporteRequestDto request)
        {
            // Validar que tenga rango de fechas
            if (request.FechaInicio == null || request.FechaFin == null)
            {
                throw new ArgumentException("Debe especificar FechaInicio y FechaFin");
            }

            DateTime fechaInicio = request.FechaInicio.Value;
            DateTime fechaFin = request.FechaFin.Value;

            // Validar que fechaInicio sea menor que fechaFin
            if (fechaInicio > fechaFin)
            {
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha de fin");
            }

            // Recopilar datos de ventas
            var ventas = await _ventaRepositorio.ObtenerVentasPorRangoFechaAsync(fechaInicio, fechaFin);

            // Filtrar por cliente si se especificó
            if (request.ClienteId.HasValue)
            {
                ventas = ventas.Where(v => v.ClienteId == request.ClienteId.Value).ToList();
            }

            // Generar estadísticas
            var reporte = GenerarEstadisticas(fechaInicio, fechaFin, ventas, request.ClienteId);

            return reporte;
        }

        private ReportePeriodicoDto GenerarEstadisticas(
            DateTime inicio, 
            DateTime fin, 
            List<Dominio.Entities.Venta> ventas,
            Guid? clienteId = null)
        {
            var reporte = new ReportePeriodicoDto
            {
                TipoReporte = "Ventas",
                Periodo = $"{inicio:dd/MM/yyyy} - {fin:dd/MM/yyyy}",
                FechaInicio = inicio,
                FechaFin = fin,
                TotalTransacciones = ventas.Count,
                TotalIngresos = ventas.Sum(v => v.Total),
                PromedioVenta = ventas.Any() ? ventas.Average(v => v.Total) : 0,
                VentaMayor = ventas.Any() ? ventas.Max(v => v.Total) : 0,
                VentaMenor = ventas.Any() ? ventas.Min(v => v.Total) : 0
            };

            // Si hay filtro por cliente, agregar info
            if (clienteId.HasValue && ventas.Any())
            {
                var primerCliente = ventas.First().Cliente;
                reporte.ClienteFiltro = primerCliente != null ? new ClienteReporteDto
                {
                    Id = primerCliente.Id,
                    Nombre = primerCliente.Nombre
                } : null;
            }

            // Detalles de ventas
            reporte.Detalles = ventas.Select(v => new VentaReporteDto
            {
                Id = v.Id,
                ClienteId = v.ClienteId,
                NombreCliente = v.Cliente?.Nombre ?? "N/A",
                Fecha = v.Fecha,
                Total = v.Total,
                MetodoPago = v.MetodoPago,
                Estado = v.Estado,
                CantidadItems = v.DetalleVentas?.Count ?? 0
            }).OrderByDescending(v => v.Fecha).ToList();

            // Estadísticas por método de pago
            reporte.VentasPorMetodoPago = ventas
                .GroupBy(v => v.MetodoPago ?? "Sin especificar")
                .ToDictionary(g => g.Key, g => g.Count());

            reporte.IngresosPorMetodoPago = ventas
                .GroupBy(v => v.MetodoPago ?? "Sin especificar")
                .ToDictionary(g => g.Key, g => g.Sum(v => v.Total));

            // Estadísticas por día
            reporte.VentasPorDia = ventas
                .GroupBy(v => v.Fecha.Date)
                .OrderBy(g => g.Key)
                .ToDictionary(
                    g => g.Key.ToString("yyyy-MM-dd"),
                    g => new EstadisticaDiaDto
                    {
                        Fecha = g.Key,
                        CantidadVentas = g.Count(),
                        TotalIngresos = g.Sum(v => v.Total)
                    }
                );

            return reporte;
        }
    }
}
