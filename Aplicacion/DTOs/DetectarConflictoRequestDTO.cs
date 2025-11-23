using System;
using System.ComponentModel.DataAnnotations;

namespace Aplication.DTOs
{
   public class DetectarConflictoRequestDTO
    {
        [Required(ErrorMessage = "La empleada es obligatoria")]
        public int EmpleadaId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

  [Required(ErrorMessage = "La hora de inicio es obligatoria")]
  public TimeSpan HoraInicio { get; set; }

 [Required(ErrorMessage = "La hora de fin es obligatoria")]
    public TimeSpan HoraFin { get; set; }
  }
}
