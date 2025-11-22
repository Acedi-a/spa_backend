using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
    public class CursoRepositorio : ICursoRepositorio
    {
        private readonly AppDbContext _context;
        public CursoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Curso?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Cursos.FindAsync(id);
        }

        public async Task<IEnumerable<Curso>> ObtenerListaDocentes()
        {
            return await _context.Cursos.ToListAsync();
        }

        public async Task CrearAsync(Curso curso)
        {
            await _context.Cursos.AddAsync(curso);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Curso curso)
        {
            _context.Cursos.Update(curso);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Guid id)
        {
            var curso = await ObtenerPorIdAsync(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
                await _context.SaveChangesAsync();
            }
        }



    }
}
