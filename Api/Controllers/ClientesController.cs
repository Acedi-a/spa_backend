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
    public class ClientesController : ControllerBase
    {
        private readonly CrearCliente _crearCliente;
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IMapper _mapper;

        public ClientesController(CrearCliente crearCliente, IClienteRepositorio clienteRepositorio, IMapper mapper)
        {
            _crearCliente = crearCliente;
            _clienteRepositorio = clienteRepositorio;
            _mapper = mapper;
        }

        // GET: api/clientes
        [HttpGet]
        public async Task<IActionResult> ListarClientes()
        {
            try
            {
                var clientes = await _clienteRepositorio.ListarTodosAsync();
                var clientesDto = _mapper.Map<IEnumerable<ClienteDTO>>(clientes);
                return Ok(clientesDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener los clientes", detalle = ex.Message });
            }
        }

        // GET: api/clientes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerCliente(Guid id)
        {
            try
            {
                var cliente = await _clienteRepositorio.ObtenerPorIdAsync(id);
                
                if (cliente == null)
                {
                    return NotFound(new { error = "Cliente no encontrado" });
                }

                var clienteDto = _mapper.Map<ClienteDTO>(cliente);
                return Ok(clienteDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener el cliente", detalle = ex.Message });
            }
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
                    nameof(ObtenerCliente),
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
