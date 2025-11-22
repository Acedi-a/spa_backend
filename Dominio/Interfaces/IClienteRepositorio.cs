using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface IClienteRepositorio
    {
        Task<Cliente?> ObtenerPorIdAsync(Guid id);
        Task<IEnumerable<Cliente>> ListarTodosAsync();
        Task CrearAsync(Cliente cliente);
        Task ActualizarAsync(Cliente cliente);
        Task EliminarAsync(Guid id);
    }
}
