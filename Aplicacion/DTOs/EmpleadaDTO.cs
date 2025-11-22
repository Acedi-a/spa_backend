using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class EmpleadaDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        public string? Nombre { get; set; }
        
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string? Telefono { get; set; }
        
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string? Email { get; set; }
        
        [StringLength(100, ErrorMessage = "La especialidad no puede exceder 100 caracteres")]
        public string? Especialidad { get; set; }
        
        [Required(ErrorMessage = "El porcentaje de comisión es obligatorio")]
        [Range(0, 100, ErrorMessage = "El porcentaje de comisión debe estar entre 0 y 100")]
        public decimal PorcentajeComision { get; set; }
        
        [Required(ErrorMessage = "La fecha de contratación es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaContratacion { get; set; }
    }
}
