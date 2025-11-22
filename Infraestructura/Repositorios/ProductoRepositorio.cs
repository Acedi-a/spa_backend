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
    public class ProductoRepositorio : IProductoRepositorio
    {
        private readonly AppDbContext _context;

      public ProductoRepositorio(AppDbContext context)
  {
            _context = context;
        }

public async Task<Producto?> ObtenerPorIdAsync(int id)
{
            return await _context.Productos
         .Include(p => p.Categoria)
  .FirstOrDefaultAsync(p => p.Id == id);
        }

    public async Task<IEnumerable<Producto>> ListarTodosAsync()
        {
       return await _context.Productos
       .Include(p => p.Categoria)
                .ToListAsync();
   }

        public async Task CrearAsync(Producto producto)
        {
          // Validar que la categoría existe
    var categoriaExiste = await _context.Categorias.FindAsync(producto.CategoriaId);
     if (categoriaExiste == null)
        {
    throw new ArgumentException($"La categoría con ID {producto.CategoriaId} no existe.");
            }

     await _context.Productos.AddAsync(producto);
     await _context.SaveChangesAsync();
        }

    public async Task ActualizarAsync(Producto producto)
        {
      // Validar que la categoría existe
            var categoriaExiste = await _context.Categorias.FindAsync(producto.CategoriaId);
   if (categoriaExiste == null)
    {
          throw new ArgumentException($"La categoría con ID {producto.CategoriaId} no existe.");
}

    _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
     }

        public async Task EliminarAsync(int id)
        {
  var producto = await ObtenerPorIdAsync(id);
      if (producto != null)
            {
                _context.Productos.Remove(producto);
          await _context.SaveChangesAsync();
  }
        }
    }
}
