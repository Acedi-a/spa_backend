using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface IVentaRepositorio
    {
        Task<Venta?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Venta>> ListarTodosAsync();
        Task<IEnumerable<Venta>> ListarPorClienteAsync(Guid clienteId);
        Task CrearAsync(Venta venta);
        Task ActualizarAsync(Venta venta);
        Task EliminarAsync(int id);
    }
}
