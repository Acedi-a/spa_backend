using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
    public class Valoracion
    {
        [Key]
        public int Id { get; set; }
        
        public Guid ClienteId { get; set; }
        [ForeignKey(nameof(ClienteId))]
        public Cliente? Cliente { get; set; }
        
        public int VentaId { get; set; }
        [ForeignKey(nameof(VentaId))]
        public Venta? Venta { get; set; }
        
        public int ServicioId { get; set; }
        [ForeignKey(nameof(ServicioId))]
        public Servicio? Servicio { get; set; }
        
        [Range(1, 5)]
        public int Calificacion { get; set; }
        
        [MaxLength(1000)]
        public string? Comentario { get; set; }
        
        [DataType(DataType.Date)]
        [Column(TypeName = "timestamp without time zone")]
        public DateTime Fecha { get; set; }
    }
}
