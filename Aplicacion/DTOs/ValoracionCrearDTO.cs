using System;
using System.ComponentModel.DataAnnotations;

namespace Aplication.DTOs
{
    public class ValoracionCrearDTO
    {
        [Required(ErrorMessage = "El cliente es obligatorio")]
        public Guid ClienteId { get; set; }

  [Required(ErrorMessage = "La empleada es obligatoria")]
        public int EmpleadaId { get; set; }

  [Required(ErrorMessage = "El servicio es obligatorio")]
        public int ServicioId { get; set; }

        [Required(ErrorMessage = "La venta es obligatoria")]
        public int VentaId { get; set; }

        [Required(ErrorMessage = "La calificación es obligatoria")]
    [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5")]
        public int Calificacion { get; set; }

        [MaxLength(1000, ErrorMessage = "El comentario no puede exceder 1000 caracteres")]
        public string? Comentario { get; set; }
    }
}
