using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }
        
        public Guid ClienteId { get; set; }
        [ForeignKey(nameof(ClienteId))]
        public Cliente? Cliente { get; set; }
        
        [DataType(DataType.Date)]
        [Column(TypeName = "timestamp without time zone")]
        public DateTime Fecha { get; set; }
        
        public decimal Total { get; set; }
        
        [MaxLength(50)]
        public string? MetodoPago { get; set; }
        
        [MaxLength(50)]
        public string? Estado { get; set; }
        
        public ICollection<DetalleVenta>? DetalleVentas { get; set; }
        public ICollection<Valoracion>? Valoraciones { get; set; }
    }
}
