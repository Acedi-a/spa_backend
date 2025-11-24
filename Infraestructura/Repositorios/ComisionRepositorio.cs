using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositorios
{
 public class ComisionRepositorio : IComisionRepositorio
 {
 private readonly AppDbContext _context;

 public ComisionRepositorio(AppDbContext context)
 {
 _context = context;
 }

 /// <summary>
 /// Obtiene todas las ventas que contienen servicios del empleado en un rango de fechas
 /// </summary>
 public async Task<List<DetalleVenta>> ObtenerVentasPorEmpleadoAsync(int empleadaId, DateTime fechaInicio, DateTime fechaFin)
 {
 // Obtener todos los servicios de la empleada
 var serviciosEmpleada = await _context.Servicios
 .Where(s => s.EmpleadaId == empleadaId && s.Activo)
 .Select(s => s.Id)
 .ToListAsync();

 if (serviciosEmpleada.Count ==0)
 {
 return new List<DetalleVenta>();
 }

 // Obtener detalles de venta que contengan servicios del empleado en el rango de fechas
 var detallesVenta = await _context.DetalleVentas
 .Where(dv => dv.ServicioId.HasValue && 
 serviciosEmpleada.Contains(dv.ServicioId.Value) &&
 dv.Venta!.Fecha.Date >= fechaInicio.Date &&
 dv.Venta!.Fecha.Date <= fechaFin.Date)
 .Include(dv => dv.Venta)
 .Include(dv => dv.Servicio)
 .OrderByDescending(dv => dv.Venta!.Fecha)
 .ToListAsync();

 return detallesVenta;
 }

 /// <summary>
 /// Obtiene el porcentaje de comisión de un empleado
 /// </summary>
 public async Task<decimal> ObtenerPorcentajeComisionAsync(int empleadaId)
 {
 var empleada = await _context.Empleadas.FindAsync(empleadaId);
 if (empleada == null)
 {
 throw new ArgumentException($"La empleada con ID {empleadaId} no existe.");
 }

 return empleada.PorcentajeComision;
 }

 /// <summary>
 /// Obtiene información de la empleada
 /// </summary>
 public async Task<Empleada?> ObtenerEmpleadaAsync(int empleadaId)
 {
 return await _context.Empleadas
 .AsNoTracking()
 .FirstOrDefaultAsync(e => e.Id == empleadaId);
 }
 }
}
