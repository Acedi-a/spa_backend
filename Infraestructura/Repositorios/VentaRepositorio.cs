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
    public class VentaRepositorio : IVentaRepositorio
    {
        private readonly AppDbContext _context;

        public VentaRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Venta?> ObtenerPorIdAsync(int id)
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.DetalleVentas)
                    .ThenInclude(dv => dv.Producto)
                .Include(v => v.DetalleVentas)
                    .ThenInclude(dv => dv.Servicio)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Venta>> ListarTodosAsync()
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.DetalleVentas)
                .ToListAsync();
        }

        public async Task<IEnumerable<Venta>> ListarPorClienteAsync(Guid clienteId)
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.DetalleVentas)
                .Where(v => v.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<List<Venta>> ObtenerVentasPorRangoFechaAsync(DateTime inicio, DateTime fin)
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.DetalleVentas)
                    .ThenInclude(dv => dv.Producto)
                .Include(v => v.DetalleVentas)
                    .ThenInclude(dv => dv.Servicio)
                .Where(v => v.Fecha >= inicio && v.Fecha <= fin)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();
        }

        public async Task CrearAsync(Venta venta)
        {
            await _context.Ventas.AddAsync(venta);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Venta venta)
        {
            _context.Ventas.Update(venta);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.DetalleVentas)
                .FirstOrDefaultAsync(v => v.Id == id);
                
            if (venta != null)
            {
                // Elimina primero los detalles
                if (venta.DetalleVentas != null)
                {
                    _context.DetalleVentas.RemoveRange(venta.DetalleVentas);
                }
                
                // Luego elimina la venta
                _context.Ventas.Remove(venta);
                await _context.SaveChangesAsync();
            }
        }
    }
}
