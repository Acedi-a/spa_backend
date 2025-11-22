using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
    public class DetalleVenta
    {
        [Key]
        public int Id { get; set; }
        
        public int VentaId { get; set; }
        [ForeignKey(nameof(VentaId))]
        public Venta? Venta { get; set; }
        
        public int? ProductoId { get; set; }
        [ForeignKey(nameof(ProductoId))]
        public Producto? Producto { get; set; }
        
        public int? ServicioId { get; set; }
        [ForeignKey(nameof(ServicioId))]
        public Servicio? Servicio { get; set; }
        
        public int Cantidad { get; set; }
        
        public decimal PrecioUnitario { get; set; }
        
        public decimal Subtotal { get; set; }
    }
}
