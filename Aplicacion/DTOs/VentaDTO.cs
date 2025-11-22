using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class VentaDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El cliente es obligatorio")]
        public Guid ClienteId { get; set; }
        
        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        
        [Required(ErrorMessage = "El total es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El total debe ser mayor o igual a 0")]
        public decimal Total { get; set; }
        
        [Required(ErrorMessage = "El método de pago es obligatorio")]
        [MaxLength(50, ErrorMessage = "El método de pago no puede exceder 50 caracteres")]
        public string? MetodoPago { get; set; }
        
        [MaxLength(50, ErrorMessage = "El estado no puede exceder 50 caracteres")]
        public string? Estado { get; set; }
        
        public List<DetalleVentaDTO>? DetalleVentas { get; set; }
    }
}
