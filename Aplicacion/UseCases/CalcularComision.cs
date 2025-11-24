using Aplication.DTOs;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
 public class CalcularComision
 {
 private readonly IComisionRepositorio _comisionRepositorio;

 public CalcularComision(IComisionRepositorio comisionRepositorio)
 {
 _comisionRepositorio = comisionRepositorio;
 }

 /// <summary>
 /// Calcula la comisión total de un empleado en un rango de fechas
 /// </summary>
 public async Task<ComisionDTO> EjecutarAsync(int empleadaId, DateTime fechaInicio, DateTime fechaFin)
 {
 ValidarDatos(empleadaId, fechaInicio, fechaFin);

 // Obtener información de la empleada
 var empleada = await _comisionRepositorio.ObtenerEmpleadaAsync(empleadaId);
 if (empleada == null)
 {
 throw new ArgumentException($"La empleada con ID {empleadaId} no existe.");
 }

 // Obtener ventas del empleado en el rango de fechas
 var ventasDetalles = await _comisionRepositorio.ObtenerVentasPorEmpleadoAsync(empleadaId, fechaInicio, fechaFin);

 // Calcular comisiones
 var detallesComision = new List<DetalleComisionDTO>();
 decimal totalVentas =0;
 decimal comisionTotal =0;

 foreach (var detalle in ventasDetalles)
 {
 var subtotal = detalle.Subtotal;
 var comision = subtotal * (empleada.PorcentajeComision /100);

 detallesComision.Add(new DetalleComisionDTO
 {
 VentaId = detalle.VentaId,
 FechaVenta = detalle.Venta!.Fecha,
 NombreServicio = detalle.Servicio?.Nombre,
 PrecioServicio = detalle.PrecioUnitario,
 Cantidad = detalle.Cantidad,
 SubtotalServicio = subtotal,
 ComisionServicio = comision
 });

 totalVentas += subtotal;
 comisionTotal += comision;
 }

 // Crear reporte de comisión
 var comisionDto = new ComisionDTO
 {
 EmpleadaId = empleadaId,
 NombreEmpleada = empleada.Nombre,
 PorcentajeComision = empleada.PorcentajeComision,
 FechaInicio = fechaInicio,
 FechaFin = fechaFin,
 TotalVentas = totalVentas,
 CantidadVentas = ventasDetalles.Count,
 ComisionTotal = Math.Round(comisionTotal,2),
 DetalleServicios = detallesComision
 };

 return comisionDto;
 }

 private void ValidarDatos(int empleadaId, DateTime fechaInicio, DateTime fechaFin)
 {
 if (empleadaId <=0)
 {
 throw new ArgumentException("El ID de la empleada es inválido.");
 }

 if (fechaInicio > fechaFin)
 {
 throw new ArgumentException("La fecha de inicio no puede ser mayor a la fecha de fin.");
 }

 if (fechaInicio.Date > DateTime.Now.Date)
 {
 throw new ArgumentException("La fecha de inicio no puede ser en el futuro.");
 }
 }
 }
}
