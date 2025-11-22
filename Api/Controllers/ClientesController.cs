using Aplication.DTOs;
using Aplication.UseCases;
using AutoMapper;
using Dominio.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly CrearCliente _crearCliente;
        private readonly IMapper _mapper;

        public ClientesController(CrearCliente crearCliente, IMapper mapper)
        {
            _crearCliente = crearCliente;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarCliente([FromBody] ClienteDTO clienteDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var cliente = _mapper.Map<Cliente>(clienteDto);
                await _crearCliente.EjecutarAsync(cliente);

                return CreatedAtAction(
                    nameof(RegistrarCliente),
                    new { id = cliente.Id },
                    new { id = cliente.Id, mensaje = "Cliente registrado exitosamente" }
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
