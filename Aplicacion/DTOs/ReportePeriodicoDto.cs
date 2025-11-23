using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class ReportePeriodicoDto
    {
        public string TipoReporte { get; set; } = string.Empty;
        public string Periodo { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        
        // Info del cliente si se filtró
        public ClienteReporteDto? ClienteFiltro { get; set; }
        
        // Estadísticas generales
        public decimal TotalIngresos { get; set; }
        public int TotalTransacciones { get; set; }
        public decimal PromedioVenta { get; set; }
        public decimal VentaMayor { get; set; }
        public decimal VentaMenor { get; set; }
        
        // Detalles de ventas
        public List<VentaReporteDto> Detalles { get; set; } = new List<VentaReporteDto>();
        
        // Estadísticas adicionales
        public Dictionary<string, int> VentasPorMetodoPago { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, decimal> IngresosPorMetodoPago { get; set; } = new Dictionary<string, decimal>();
        
        // Ventas por día
        public Dictionary<string, EstadisticaDiaDto> VentasPorDia { get; set; } = new Dictionary<string, EstadisticaDiaDto>();
    }
    
    public class ClienteReporteDto
    {
        public Guid Id { get; set; }
        public string? Nombre { get; set; }
    }
    
    public class EstadisticaDiaDto
    {
        public DateTime Fecha { get; set; }
        public int CantidadVentas { get; set; }
        public decimal TotalIngresos { get; set; }
    }
}
