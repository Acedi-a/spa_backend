using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
 public interface IComisionRepositorio
 {
 /// <summary>
 /// Obtiene las ventas que contienen servicios realizados por el empleado en un rango de fechas
 /// </summary>
 Task<List<Dominio.Entities.DetalleVenta>> ObtenerVentasPorEmpleadoAsync(int empleadaId, DateTime fechaInicio, DateTime fechaFin);

 /// <summary>
 /// Obtiene el porcentaje de comisión del empleado
 /// </summary>
 Task<decimal> ObtenerPorcentajeComisionAsync(int empleadaId);

 /// <summary>
 /// Obtiene información de la empleada
 /// </summary>
 Task<Dominio.Entities.Empleada?> ObtenerEmpleadaAsync(int empleadaId);
 }
}
