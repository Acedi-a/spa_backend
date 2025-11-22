using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface IServicioRepositorio
    {
        Task<Servicio?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Servicio>> ListarTodosAsync();
        Task CrearAsync(Servicio servicio);
        Task ActualizarAsync(Servicio servicio);
        Task EliminarAsync(int id);
    }
}
