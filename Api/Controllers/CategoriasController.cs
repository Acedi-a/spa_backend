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
    public class CategoriasController : ControllerBase
    {
        private readonly CrearCategoria _crearCategoria;
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IMapper _mapper;

        public CategoriasController(
            CrearCategoria crearCategoria, 
            ICategoriaRepositorio categoriaRepositorio,
            IMapper mapper)
        {
            _crearCategoria = crearCategoria;
            _categoriaRepositorio = categoriaRepositorio;
            _mapper = mapper;
        }

        // GET: api/categorias
        [HttpGet]
        public async Task<IActionResult> ListarCategorias()
        {
            try
            {
                var categorias = await _categoriaRepositorio.ListarTodosAsync();
                var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
                return Ok(categoriasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener las categorías", detalle = ex.Message });
            }
        }

        // GET: api/categorias/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerCategoria(int id)
        {
            try
            {
                var categoria = await _categoriaRepositorio.ObtenerPorIdAsync(id);
                
                if (categoria == null)
                {
                    return NotFound(new { error = "Categoría no encontrada" });
                }

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
                return Ok(categoriaDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener la categoría", detalle = ex.Message });
            }
        }

        // POST: api/categorias
        [HttpPost]
        public async Task<IActionResult> RegistrarCategoria([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);
                await _crearCategoria.EjecutarAsync(categoria);

                return CreatedAtAction(
                    nameof(ObtenerCategoria),
                    new { id = categoria.Id },
                    new { id = categoria.Id, mensaje = "Categoría registrada exitosamente" }
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

        // PUT: api/categorias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCategoria(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var categoriaExistente = await _categoriaRepositorio.ObtenerPorIdAsync(id);
                
                if (categoriaExistente == null)
                {
                    return NotFound(new { error = "Categoría no encontrada" });
                }

                categoriaExistente.Nombre = categoriaDto.Nombre;
                categoriaExistente.Descripcion = categoriaDto.Descripcion;

                await _categoriaRepositorio.ActualizarAsync(categoriaExistente);

                return Ok(new { mensaje = "Categoría actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar la categoría", detalle = ex.Message });
            }
        }

        // DELETE: api/categorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            try
            {
                var categoria = await _categoriaRepositorio.ObtenerPorIdAsync(id);
                
                if (categoria == null)
                {
                    return NotFound(new { error = "Categoría no encontrada" });
                }

                await _categoriaRepositorio.EliminarAsync(id);

                return Ok(new { mensaje = "Categoría eliminada exitosamente (desactivada)" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al eliminar la categoría", detalle = ex.Message });
            }
        }
    }
}
