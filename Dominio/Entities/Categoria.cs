using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Entities
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string? Nombre { get; set; }
        
        [MaxLength(500)]
        public string? Descripcion { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public ICollection<Producto>? Productos { get; set; }
        public ICollection<Servicio>? Servicios { get; set; }
    }
}
