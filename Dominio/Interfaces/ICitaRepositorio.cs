using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
  public interface ICitaRepositorio
    {
     Task<Cita?> ObtenerPorIdAsync(int id);
     Task<IEnumerable<Cita>> ListarTodosAsync();
        Task<IEnumerable<Cita>> ListarPorEmpleadaAsync(int empleadaId);
     Task<bool> VerificarDisponibilidadAsync(int empleadaId, DateTime fecha, TimeSpan hora, int duracion);
        Task CrearAsync(Cita cita);
        Task ActualizarAsync(Cita cita);
        Task EliminarAsync(int id);
    }
}
