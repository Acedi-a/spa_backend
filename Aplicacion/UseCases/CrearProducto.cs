using Dominio.Entities;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class CrearProducto
 {
        private readonly IProductoRepositorio _productoRepositorio;

     public CrearProducto(IProductoRepositorio productoRepositorio)
        {
    _productoRepositorio = productoRepositorio;
 }

        public async Task EjecutarAsync(Producto producto)
        {
            ValidarProducto(producto);
            
   producto.Activo = true;
   await _productoRepositorio.CrearAsync(producto);
        }

private void ValidarProducto(Producto producto)
        {
   // Validar nombre
      if (string.IsNullOrWhiteSpace(producto.Nombre) || producto.Nombre.Length < 3)
       {
  throw new ArgumentException("El nombre del producto es inválido. Debe tener al menos 3 caracteres.");
      }

   if (producto.Nombre.Length > 100)
     {
     throw new ArgumentException("El nombre del producto no puede exceder 100 caracteres.");
  }

            // Validar precio
    if (producto.Precio <= 0)
{
      throw new ArgumentException("El precio debe ser mayor a 0.");
     }

      // Validar stock
     if (producto.Stock < 0)
   {
        throw new ArgumentException("El stock no puede ser negativo.");
         }

 // Validar stock mínimo
     if (producto.StockMinimo < 0)
            {
  throw new ArgumentException("El stock mínimo no puede ser negativo.");
       }

        // Validar que el stock sea mayor o igual al stock mínimo
         if (producto.Stock < producto.StockMinimo)
    {
          throw new ArgumentException("El stock no puede ser menor al stock mínimo.");
     }

          // Validar fecha de vencimiento
    if (producto.FechaVencimiento <= DateTime.Now)
      {
      throw new ArgumentException("La fecha de vencimiento debe ser posterior a la fecha actual.");
        }

 // Validar categoría
        if (producto.CategoriaId <= 0)
  {
         throw new ArgumentException("La categoría es obligatoria.");
   }

   // Validar descripción
     if (!string.IsNullOrEmpty(producto.Descripcion) && producto.Descripcion.Length > 500)
  {
     throw new ArgumentException("La descripción no puede exceder 500 caracteres.");
     }
        }
    }
}
