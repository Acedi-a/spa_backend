using Aplication.DTOs;
using Aplication.UseCases;
using AutoMapper;
using Dominio.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController : ControllerBase
    {
        private readonly CrearServicio _crearServicio;
        private readonly IMapper _mapper;

        public ServiciosController(CrearServicio crearServicio, IMapper mapper)
        {
            _crearServicio = crearServicio;
            _mapper = mapper;
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
                    nameof(RegistrarServicio),
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
    }
}
