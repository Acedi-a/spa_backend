using Aplication.DTOs;
using Aplication.UseCases;
using AutoMapper;
using Dominio.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly CrearCategoria _crearCategoria;
        private readonly IMapper _mapper;

        public CategoriasController(CrearCategoria crearCategoria, IMapper mapper)
        {
            _crearCategoria = crearCategoria;
            _mapper = mapper;
        }

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
                    nameof(RegistrarCategoria),
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
    }
}
