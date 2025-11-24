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
    public class ServiciosController : ControllerBase
    {
        private readonly CrearServicio _crearServicio;
        private readonly IServicioRepositorio _servicioRepositorio;
        private readonly IMapper _mapper;

        public ServiciosController(CrearServicio crearServicio, IServicioRepositorio servicioRepositorio, IMapper mapper)
        {
            _crearServicio = crearServicio;
            _servicioRepositorio = servicioRepositorio;
            _mapper = mapper;
        }

        // GET: api/servicios
        [HttpGet]
        public async Task<IActionResult> ListarServicios()
        {
            try
            {
                var servicios = await _servicioRepositorio.ListarTodosAsync();
                var serviciosDto = _mapper.Map<IEnumerable<ServicioDTO>>(servicios);
                return Ok(serviciosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener los servicios", detalle = ex.Message });
            }
        }

        // GET: api/servicios/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerServicio(int id)
        {
            try
            {
                var servicio = await _servicioRepositorio.ObtenerPorIdAsync(id);
                
                if (servicio == null)
                {
                    return NotFound(new { error = "Servicio no encontrado" });
                }

                var servicioDto = _mapper.Map<ServicioDTO>(servicio);
                return Ok(servicioDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener el servicio", detalle = ex.Message });
            }
        }

        [HttpPost("crear")]
        public async Task<IActionResult> RegistrarServicio([FromBody] ServicioDTO servicioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var servicio = _mapper.Map<Servicio>(servicioDto);
                await _crearServicio.EjecutarAsync(servicio);

                return CreatedAtAction(
                    nameof(ObtenerServicio),
                    new { id = servicio.Id },
                    new { id = servicio.Id, mensaje = "Servicio registrado exitosamente" }
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

        // PUT: api/servicios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarServicio(int id, [FromBody] ServicioDTO servicioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var servicioExistente = await _servicioRepositorio.ObtenerPorIdAsync(id);

                if (servicioExistente == null)
                {
                    return NotFound(new { error = "Servicio no encontrado" });
                }

                // Actualizar campos
                servicioExistente.Nombre = servicioDto.Nombre;
                servicioExistente.CategoriaId = servicioDto.CategoriaId;
                servicioExistente.EmpleadaId = servicioDto.EmpleadaId;
                servicioExistente.Precio = servicioDto.Precio;
                servicioExistente.Duracion = servicioDto.Duracion;
                servicioExistente.Descripcion = servicioDto.Descripcion;

                await _servicioRepositorio.ActualizarAsync(servicioExistente);

                return Ok(new { mensaje = "Servicio actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar el servicio", detalle = ex.Message });
            }
        }
    }
}
