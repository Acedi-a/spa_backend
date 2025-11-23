using System;
using System.ComponentModel.DataAnnotations;

namespace Aplication.DTOs
{
    public class DisponibilidadDTO
    {
     [Required(ErrorMessage = "La empleada es obligatoria")]
        public int EmpleadaId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
     public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria")]
     public TimeSpan Hora { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(1, 480, ErrorMessage = "La duración debe estar entre 1 y 480 minutos")]
        public int Duracion { get; set; }
    }
}
