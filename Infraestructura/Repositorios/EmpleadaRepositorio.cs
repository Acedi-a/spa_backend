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
    public class EmpleadaRepositorio : IEmpleadaRepositorio
    {
        private readonly AppDbContext _context;

        public EmpleadaRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Empleada?> ObtenerPorIdAsync(int id)
        {
            return await _context.Empleadas.FindAsync(id);
        }

        public async Task<IEnumerable<Empleada>> ListarTodosAsync()
        {
            return await _context.Empleadas.ToListAsync();
        }

        public async Task CrearAsync(Empleada empleada)
        {
            await _context.Empleadas.AddAsync(empleada);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Empleada empleada)
        {
            _context.Empleadas.Update(empleada);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var empleada = await ObtenerPorIdAsync(id);
            if (empleada != null)
            {
                _context.Empleadas.Remove(empleada);
                await _context.SaveChangesAsync();
            }
        }
    }
}
