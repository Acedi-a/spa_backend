using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class DetalleVentaDTO
    {
        public int Id { get; set; }
        
        public int? ProductoId { get; set; }
        
        public int? ServicioId { get; set; }
        
        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }
        
        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor o igual a 0")]
        public decimal PrecioUnitario { get; set; }
        
        public decimal Subtotal { get; set; }
    }
}
