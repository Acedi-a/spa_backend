using Aplication.DTOs;
using Aplication.UseCases;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
  [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly CrearProducto _crearProducto;
        private readonly IProductoRepositorio _productoRepositorio;
        private readonly IMapper _mapper;

        public ProductosController(CrearProducto crearProducto, IProductoRepositorio productoRepositorio, IMapper mapper)
{
         _crearProducto = crearProducto;
         _productoRepositorio = productoRepositorio;
_mapper = mapper;
  }

        [HttpPost]
        public async Task<IActionResult> RegistrarProducto([FromBody] ProductoDTO productoDto)
        {
    try
   {
 if (!ModelState.IsValid)
      {
          return BadRequest(ModelState);
            }

                var producto = _mapper.Map<Producto>(productoDto);
  await _crearProducto.EjecutarAsync(producto);

                return CreatedAtAction(
      nameof(ObtenerProductoPorId),
     new { id = producto.Id },
         new { id = producto.Id, mensaje = "Producto registrado exitosamente" }
         );
          }
   catch (ArgumentException ex)
         {
     return BadRequest(new { error = ex.Message });
    }
       catch (Exception ex)
        {
         return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
     }
        }

   [HttpGet]
     public async Task<IActionResult> ListarProductos()
        {
  try
     {
     var productos = await _productoRepositorio.ListarTodosAsync();
    var productosDto = _mapper.Map<IEnumerable<ProductoDTO>>(productos);

              return Ok(new
             {
         mensaje = "Productos listados exitosamente",
data = productosDto
      });
            }
 catch (Exception ex)
            {
      return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
     }
        }

        [HttpGet("{id}")]
      public async Task<IActionResult> ObtenerProductoPorId(int id)
  {
     try
            {
      var producto = await _productoRepositorio.ObtenerPorIdAsync(id);

       if (producto == null)
         {
 return NotFound(new { error = $"El producto con ID {id} no existe." });
           }

    var productoDto = _mapper.Map<ProductoDTO>(producto);

        return Ok(new
        {
     mensaje = "Producto obtenido exitosamente",
data = productoDto
      });
            }
         catch (Exception ex)
      {
           return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] ProductoDTO productoDto)
      {
    try
  {
                if (!ModelState.IsValid)
       {
        return BadRequest(ModelState);
            }

   var productoExistente = await _productoRepositorio.ObtenerPorIdAsync(id);

 if (productoExistente == null)
        {
        return NotFound(new { error = $"El producto con ID {id} no existe." });
       }

      // Validaciones de negocio
   ValidarProductoActualizacion(productoDto);

 // Mapear los datos del DTO al producto existente
   productoExistente.Nombre = productoDto.Nombre;
         productoExistente.CategoriaId = productoDto.CategoriaId;
      productoExistente.Precio = productoDto.Precio;
  productoExistente.Stock = productoDto.Stock;
  productoExistente.StockMinimo = productoDto.StockMinimo;
          productoExistente.FechaVencimiento = productoDto.FechaVencimiento;
        productoExistente.Descripcion = productoDto.Descripcion;
       productoExistente.Activo = productoDto.Activo;

        await _productoRepositorio.ActualizarAsync(productoExistente);

                return Ok(new
 {
      id = productoExistente.Id,
 mensaje = "Producto actualizado exitosamente"
      });
            }
            catch (ArgumentException ex)
     {
    return BadRequest(new { error = ex.Message });
  }
      catch (Exception ex)
       {
         return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
  }

        private void ValidarProductoActualizacion(ProductoDTO productoDto)
        {
            // Validar nombre
  if (string.IsNullOrWhiteSpace(productoDto.Nombre) || productoDto.Nombre.Length < 3)
         {
         throw new ArgumentException("El nombre del producto es inválido. Debe tener al menos 3 caracteres.");
            }

            if (productoDto.Nombre.Length > 100)
     {
    throw new ArgumentException("El nombre del producto no puede exceder 100 caracteres.");
        }

         // Validar precio
            if (productoDto.Precio <= 0)
      {
          throw new ArgumentException("El precio debe ser mayor a 0.");
    }

            // Validar stock
         if (productoDto.Stock < 0)
        {
     throw new ArgumentException("El stock no puede ser negativo.");
     }

            // Validar stock mínimo
    if (productoDto.StockMinimo < 0)
   {
     throw new ArgumentException("El stock mínimo no puede ser negativo.");
          }

   // Validar que el stock sea mayor o igual al stock mínimo
       if (productoDto.Stock < productoDto.StockMinimo)
        {
   throw new ArgumentException("El stock no puede ser menor al stock mínimo.");
         }

     // Validar fecha de vencimiento
          if (productoDto.FechaVencimiento <= DateTime.Now)
          {
         throw new ArgumentException("La fecha de vencimiento debe ser posterior a la fecha actual.");
            }

            // Validar categoría
  if (productoDto.CategoriaId <= 0)
   {
      throw new ArgumentException("La categoría es obligatoria.");
        }

            // Validar descripción
            if (!string.IsNullOrEmpty(productoDto.Descripcion) && productoDto.Descripcion.Length > 500)
        {
          throw new ArgumentException("La descripción no puede exceder 500 caracteres.");
}
        }
    }
}
