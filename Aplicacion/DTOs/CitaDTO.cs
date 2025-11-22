using System;
using System.ComponentModel.DataAnnotations;

namespace Aplication.DTOs
{
 public class CitaDTO
    {
        public int Id { get; set; }

      [Required(ErrorMessage = "El cliente es obligatorio")]
   public Guid ClienteId { get; set; }

        [Required(ErrorMessage = "El servicio es obligatorio")]
        public int ServicioId { get; set; }

        [Required(ErrorMessage = "La empleada es obligatoria")]
        public int EmpleadaId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
    [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria")]
        public TimeSpan Hora { get; set; }

        [MaxLength(50, ErrorMessage = "El estado no puede exceder 50 caracteres")]
   public string? Estado { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }
    }
}
