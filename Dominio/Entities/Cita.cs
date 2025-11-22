using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }
        
        public Guid ClienteId { get; set; }
        [ForeignKey(nameof(ClienteId))]
        public Cliente? Cliente { get; set; }
        
        public int ServicioId { get; set; }
        [ForeignKey(nameof(ServicioId))]
        public Servicio? Servicio { get; set; }
        
        public int EmpleadaId { get; set; }
        [ForeignKey(nameof(EmpleadaId))]
        public Empleada? Empleada { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        
        public TimeSpan Hora { get; set; }
        
        [MaxLength(50)]
        public string? Estado { get; set; }
        
        public TimeSpan HoraInicio { get; set; }
        
        public TimeSpan HoraFin { get; set; }
    }
}
