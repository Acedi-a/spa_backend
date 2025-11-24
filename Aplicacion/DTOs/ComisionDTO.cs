using System;
using System.Collections.Generic;

namespace Aplication.DTOs
{
 public class ComisionDTO
 {
 public int EmpleadaId { get; set; }
 public string? NombreEmpleada { get; set; }
 public decimal PorcentajeComision { get; set; }
 public DateTime FechaInicio { get; set; }
 public DateTime FechaFin { get; set; }
 public decimal TotalVentas { get; set; }
 public int CantidadVentas { get; set; }
 public decimal ComisionTotal { get; set; }
 public List<DetalleComisionDTO> DetalleServicios { get; set; } = new();
 }

 public class DetalleComisionDTO
 {
 public int VentaId { get; set; }
 public DateTime FechaVenta { get; set; }
 public string? NombreServicio { get; set; }
 public decimal PrecioServicio { get; set; }
 public int Cantidad { get; set; }
 public decimal SubtotalServicio { get; set; }
 public decimal ComisionServicio { get; set; }
 }
}
