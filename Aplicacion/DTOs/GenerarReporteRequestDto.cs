using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class GenerarReporteRequestDto
    {
        [Required(ErrorMessage = "El tipo de reporte es obligatorio")]
        public string TipoReporte { get; set; } = string.Empty;
        
        // Opción 1: Usar periodo predefinido
        public string? Periodo { get; set; }
        
        // Opción 2: Usar rango de fechas personalizado
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        
        // Opcional: Filtrar por cliente específico
        public Guid? ClienteId { get; set; }
    }
}
