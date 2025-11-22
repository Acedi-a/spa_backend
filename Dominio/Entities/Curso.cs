using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Curso
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(maximumLength:20,MinimumLength =5)]
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "timestamp without time zone")]
        public DateTime Fecha_inicio { get; set; }
        public Guid Id_docente { get; set; }
        [ForeignKey(nameof(Id_docente))]

        public Docente? docente { get; set; }

        
    }
}
