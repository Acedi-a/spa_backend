using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class VentaReporteDto
    {
        public int Id { get; set; }
        public Guid ClienteId { get; set; }
        public string? NombreCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string? MetodoPago { get; set; }
        public string? Estado { get; set; }
        public int CantidadItems { get; set; }
    }
}
