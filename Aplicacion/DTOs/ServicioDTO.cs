using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class ServicioDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre del servicio es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        public string? Nombre { get; set; }
        
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int CategoriaId { get; set; }
        
        [Required(ErrorMessage = "La empleada encargada es obligatoria")]
        public int EmpleadaId { get; set; }
        
        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        public decimal Precio { get; set; }
        
        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(1, 480, ErrorMessage = "La duración debe estar entre 1 y 480 minutos")]
        public int Duracion { get; set; }
        
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }
    }
}
