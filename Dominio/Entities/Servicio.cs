using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
    public class Servicio
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string? Nombre { get; set; }
        
        public int CategoriaId { get; set; }
        [ForeignKey(nameof(CategoriaId))]
        public Categoria? Categoria { get; set; }
        
        public int EmpleadaId { get; set; }
        [ForeignKey(nameof(EmpleadaId))]
        public Empleada? Empleada { get; set; }
        
        public decimal Precio { get; set; }
        
        public int Duracion { get; set; }
        
        [MaxLength(500)]
        public string? Descripcion { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public ICollection<Cita>? Citas { get; set; }
        public ICollection<DetalleVenta>? DetalleVentas { get; set; }
        public ICollection<Valoracion>? Valoraciones { get; set; }
    }
}
