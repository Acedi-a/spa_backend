using System;
using System.ComponentModel.DataAnnotations;

namespace Aplication.DTOs
{
    public class ValoracionDTO
    {
        // Solo lectura - se retorna en GET
        public int Id { get; set; }

        public Guid ClienteId { get; set; }
        public int EmpleadaId { get; set; }
        public int ServicioId { get; set; }
        public int VentaId { get; set; }

        public int Calificacion { get; set; }
        public string? Comentario { get; set; }
        public DateTime Fecha { get; set; }

        // Propiedades calculadas - SOLO para respuestas GET
        public string? NombreCliente { get; set; }
        public string? NombreServicio { get; set; }
        public string? NombreEmpleada { get; set; }
    }
}
