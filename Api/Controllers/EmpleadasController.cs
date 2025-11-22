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
    public class EmpleadasController : ControllerBase
    {
        private readonly CrearEmpleada _crearEmpleada;
        private readonly IEmpleadaRepositorio _empleadaRepositorio;
        private readonly IMapper _mapper;

        public EmpleadasController(CrearEmpleada crearEmpleada, IEmpleadaRepositorio empleadaRepositorio, IMapper mapper)
        {
            _crearEmpleada = crearEmpleada;
            _empleadaRepositorio = empleadaRepositorio;
            _mapper = mapper;
        }

        // GET: api/empleadas
        [HttpGet]
        public async Task<IActionResult> ListarEmpleadas()
        {
            try
            {
                var empleadas = await _empleadaRepositorio.ListarTodosAsync();
                var empleadasDto = _mapper.Map<IEnumerable<EmpleadaDTO>>(empleadas);
                return Ok(empleadasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener las empleadas", detalle = ex.Message });
            }
        }

        // GET: api/empleadas/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerEmpleada(int id)
        {
            try
            {
                var empleada = await _empleadaRepositorio.ObtenerPorIdAsync(id);
                
                if (empleada == null)
                {
                    return NotFound(new { error = "Empleada no encontrada" });
                }

                var empleadaDto = _mapper.Map<EmpleadaDTO>(empleada);
                return Ok(empleadaDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener la empleada", detalle = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarEmpleada([FromBody] EmpleadaDTO empleadaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var empleada = _mapper.Map<Empleada>(empleadaDto);
                await _crearEmpleada.EjecutarAsync(empleada);

                return CreatedAtAction(
                    nameof(ObtenerEmpleada),
                    new { id = empleada.Id },
                    new { id = empleada.Id, mensaje = "Empleada registrada exitosamente" }
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
