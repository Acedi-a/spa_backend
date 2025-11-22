using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface IEmpleadaRepositorio
    {
        Task<Empleada?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Empleada>> ListarTodosAsync();
        Task CrearAsync(Empleada empleada);
        Task ActualizarAsync(Empleada empleada);
        Task EliminarAsync(int id);
    }
}
