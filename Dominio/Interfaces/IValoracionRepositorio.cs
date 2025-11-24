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
       
        Task<Valoracion?> ObtenerPorIdAsync(int id);

       
        Task<IEnumerable<Valoracion>> ListarTodosAsync();

     
      Task<IEnumerable<Valoracion>> ObtenerPorEmpleadoAsync(int empleadoId);

        
        Task<IEnumerable<Valoracion>> ObtenerPorClienteAsync(Guid clienteId);

     
        Task<IEnumerable<Valoracion>> ObtenerPorEmpleadoYFechaAsync(int empleadoId, DateTime fechaInicio, DateTime fechaFin);

      
        Task CrearAsync(Valoracion valoracion);

     
        Task ActualizarAsync(Valoracion valoracion);

        
   Task EliminarAsync(int id);

   
     Task<double> ObtenerPromedioCalificacionAsync(int empleadoId);

   
        Task<double> ObtenerPromedioCalificacionPorFechaAsync(int empleadoId, DateTime fechaInicio, DateTime fechaFin);
    }
}
