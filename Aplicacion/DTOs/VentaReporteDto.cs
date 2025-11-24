using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class VentaReporteDto
    {
        public int Id { get; set; }
        public Guid ClienteId { get; set; }
        public string? NombreCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string? MetodoPago { get; set; }
        public string? Estado { get; set; }
        public int CantidadItems { get; set; }
        
        // Detalles de la venta
        public List<DetalleVentaReporteDto> Detalles { get; set; } = new List<DetalleVentaReporteDto>();
    }
    
    public class DetalleVentaReporteDto
    {
        public int Id { get; set; }
        public string? TipoItem { get; set; } // "Producto" o "Servicio"
        public int? ProductoId { get; set; }
        public string? NombreProducto { get; set; }
        public int? ServicioId { get; set; }
        public string? NombreServicio { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
