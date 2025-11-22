using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Entities
{
    public class Cliente
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string? Nombre { get; set; }
        
        [MaxLength(20)]
        public string? Telefono { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }
        
        [MaxLength(500)]
        public string? Preferencias { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime FechaRegistro { get; set; }
        
        [MaxLength(100)]
        public string? Email { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public ICollection<Cita>? Citas { get; set; }
        public ICollection<Venta>? Ventas { get; set; }
        public ICollection<Valoracion>? Valoraciones { get; set; }
    }
}
