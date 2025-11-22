using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface IProductoRepositorio
    {
        Task<Producto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Producto>> ListarTodosAsync();
    Task CrearAsync(Producto producto);
        Task ActualizarAsync(Producto producto);
        Task EliminarAsync(int id);
  }
}
