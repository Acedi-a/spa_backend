using Dominio.Entities;
using Dominio.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositorios
{
    public class ServicioRepositorio : IServicioRepositorio
    {
        private readonly AppDbContext _context;

        public ServicioRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Servicio?> ObtenerPorIdAsync(int id)
        {
            return await _context.Servicios
                .Include(s => s.Categoria)
                .Include(s => s.Empleada)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Servicio>> ListarTodosAsync()
        {
            return await _context.Servicios
                .Include(s => s.Categoria)
                .Include(s => s.Empleada)
                .ToListAsync();
        }

        public async Task CrearAsync(Servicio servicio)
        {
            await _context.Servicios.AddAsync(servicio);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Servicio servicio)
        {
            _context.Servicios.Update(servicio);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var servicio = await ObtenerPorIdAsync(id);
            if (servicio != null)
            {
                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();
            }
        }
    }
}
