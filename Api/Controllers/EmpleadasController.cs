using Aplication.DTOs;
using Aplication.UseCases;
using AutoMapper;
using Dominio.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadasController : ControllerBase
    {
        private readonly CrearEmpleada _crearEmpleada;
        private readonly IMapper _mapper;

        public EmpleadasController(CrearEmpleada crearEmpleada, IMapper mapper)
        {
            _crearEmpleada = crearEmpleada;
            _mapper = mapper;
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
                    nameof(RegistrarEmpleada),
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
