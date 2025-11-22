using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface ICursoRepositorio
    {
        Task<Curso?> ObtenerPorIdAsync(Guid id);
        Task<IEnumerable<Curso>> ObtenerListaDocentes();
        Task CrearAsync(Curso curso);
        Task ActualizarAsync(Curso curso);
        Task EliminarAsync(Guid id);
    }
}
