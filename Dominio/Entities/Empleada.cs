using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
    public class Empleada
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string? Nombre { get; set; }
        
        [MaxLength(20)]
        public string? Telefono { get; set; }
        
        [MaxLength(100)]
        public string? Email { get; set; }
        
        [MaxLength(100)]
        public string? Especialidad { get; set; }
        
        public decimal PorcentajeComision { get; set; }
        
        [DataType(DataType.Date)]
        [Column(TypeName = "timestamp without time zone")]
        public DateTime FechaContratacion { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public ICollection<Cita>? Citas { get; set; }
        public ICollection<Servicio>? Servicios { get; set; }
        public ICollection<Valoracion>? Valoraciones { get; set; }
    }
}
