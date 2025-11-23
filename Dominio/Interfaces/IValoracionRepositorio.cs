using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
public interface IValoracionRepositorio
    {
        /// <summary>
        /// Obtiene una valoración por su ID
        /// </summary>
        Task<Valoracion?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Lista todas las valoraciones
        /// </summary>
        Task<IEnumerable<Valoracion>> ListarTodosAsync();

        /// <summary>
        /// Obtiene todas las valoraciones de un empleado específico
        /// </summary>
      Task<IEnumerable<Valoracion>> ObtenerPorEmpleadoAsync(int empleadoId);

        /// <summary>
        /// Obtiene todas las valoraciones de un cliente
        /// </summary>
        Task<IEnumerable<Valoracion>> ObtenerPorClienteAsync(Guid clienteId);

        /// <summary>
 /// Obtiene valoraciones de un empleado en un rango de fechas
   /// </summary>
        Task<IEnumerable<Valoracion>> ObtenerPorEmpleadoYFechaAsync(int empleadoId, DateTime fechaInicio, DateTime fechaFin);

      /// <summary>
        /// Crea una nueva valoración
        /// </summary>
        Task CrearAsync(Valoracion valoracion);

     /// <summary>
    /// Actualiza una valoración existente
        /// </summary>
        Task ActualizarAsync(Valoracion valoracion);

        /// <summary>
        /// Elimina una valoración
        /// </summary>
   Task EliminarAsync(int id);

     /// <summary>
        /// Obtiene el promedio de calificación de un empleado
  /// </summary>
     Task<double> ObtenerPromedioCalificacionAsync(int empleadoId);

    /// <summary>
        /// Obtiene el promedio de calificación de un empleado en un rango de fechas
     /// </summary>
        Task<double> ObtenerPromedioCalificacionPorFechaAsync(int empleadoId, DateTime fechaInicio, DateTime fechaFin);
    }
}
