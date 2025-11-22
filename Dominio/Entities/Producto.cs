using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string? Nombre { get; set; }
        
        public int CategoriaId { get; set; }
        [ForeignKey(nameof(CategoriaId))]
        public Categoria? Categoria { get; set; }
        
        public decimal Precio { get; set; }
        
        public int Stock { get; set; }
        
        public int StockMinimo { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime FechaVencimiento { get; set; }
        
        [MaxLength(500)]
        public string? Descripcion { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public ICollection<DetalleVenta>? DetalleVentas { get; set; }
    }
}
