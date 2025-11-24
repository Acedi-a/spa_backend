using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class HistorialClienteDto
    {
        // Información del Cliente
        public Guid ClienteId { get; set; }
        public string? Nombre { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string? Preferencias { get; set; }
        public bool Activo { get; set; }
        
        // Estadísticas del Cliente
        public int TotalVentas { get; set; }
        public decimal TotalGastado { get; set; }
        public decimal PromedioGasto { get; set; }
        public DateTime? UltimaCompra { get; set; }
        
        // Historial de Ventas
        public List<VentaHistorialDto> Ventas { get; set; } = new List<VentaHistorialDto>();
    }
    
    public class VentaHistorialDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string? MetodoPago { get; set; }
        public string? Estado { get; set; }
        public int CantidadItems { get; set; }
        public List<DetalleVentaHistorialDto> Detalles { get; set; } = new List<DetalleVentaHistorialDto>();
    }
    
    public class DetalleVentaHistorialDto
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
