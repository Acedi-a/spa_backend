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
    public class VentasController : ControllerBase
    {
        private readonly IVentaRepositorio _ventaRepositorio;
        private readonly ConsultarHistorialCliente _consultarHistorialCliente;
        private readonly IMapper _mapper;

        public VentasController(IVentaRepositorio ventaRepositorio, IMapper mapper)
        public VentasController(
            IVentaRepositorio ventaRepositorio, 
            ConsultarHistorialCliente consultarHistorialCliente,
            IMapper mapper)
        {
            _ventaRepositorio = ventaRepositorio;
            _consultarHistorialCliente = consultarHistorialCliente;
            _mapper = mapper;
        }

        // GET: api/ventas/historial/{qrCode}
        [HttpGet("historial/{qrCode}")]
        public async Task<IActionResult> ConsultarHistorialPorQr(Guid qrCode)
        {
            try
            {
                // Paso 2: La Aplicación llama al UseCase
                var historial = await _consultarHistorialCliente.EjecutarAsync(qrCode);

                if (historial == null)
                {
                    return NotFound(new { error = "Cliente no encontrado con el código QR proporcionado" });
                }

                // Paso 6: Retornar historial completo
                return Ok(historial);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al consultar el historial del cliente", detalle = ex.Message });
            }
        }

        // GET: api/ventas
        [HttpGet]
        public async Task<IActionResult> ListarVentas()
        {
            try
            {
                var ventas = await _ventaRepositorio.ListarTodosAsync();
                var ventasDto = _mapper.Map<IEnumerable<VentaDTO>>(ventas);
                return Ok(ventasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener las ventas", detalle = ex.Message });
            }
        }

        // GET: api/ventas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerVenta(int id)
        {
            try
            {
                var venta = await _ventaRepositorio.ObtenerPorIdAsync(id);

                if (venta == null)
                {
                    return NotFound(new { error = "Venta no encontrada" });
                }

                var ventaDto = _mapper.Map<VentaDTO>(venta);
                return Ok(ventaDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener la venta", detalle = ex.Message });
            }
        }

        // GET: api/ventas/cliente/{clienteId}
        [HttpGet("cliente/{clienteId}")]
        public async Task<IActionResult> ListarVentasPorCliente(Guid clienteId)
        {
            try
            {
                var ventas = await _ventaRepositorio.ListarPorClienteAsync(clienteId);
                var ventasDto = _mapper.Map<IEnumerable<VentaDTO>>(ventas);
                return Ok(ventasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener las ventas del cliente", detalle = ex.Message });
            }
        }

        // POST: api/ventas
        [HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] VentaDTO ventaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Validar que tenga al menos un detalle
                if (ventaDto.DetalleVentas == null || !ventaDto.DetalleVentas.Any())
                {
                    return BadRequest(new { error = "La venta debe tener al menos un detalle" });
                }

                // Validar que cada detalle tenga producto O servicio (no ambos, no ninguno)
                foreach (var detalle in ventaDto.DetalleVentas)
                {
                    if ((detalle.ProductoId == null && detalle.ServicioId == null) ||
                        (detalle.ProductoId != null && detalle.ServicioId != null))
                    {
                        return BadRequest(new { error = "Cada detalle debe tener exactamente un producto O un servicio" });
                    }
                }

                var venta = _mapper.Map<Venta>(ventaDto);
                
                // Calcular subtotales de los detalles
                if (venta.DetalleVentas != null)
                {
                    foreach (var detalle in venta.DetalleVentas)
                    {
                        detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                    }
                    
                    // Recalcular el total
                    venta.Total = venta.DetalleVentas.Sum(d => d.Subtotal);
                }

                await _ventaRepositorio.CrearAsync(venta);

                return CreatedAtAction(
                    nameof(ObtenerVenta),
                    new { id = venta.Id },
                    new { id = venta.Id, mensaje = "Venta registrada exitosamente", total = venta.Total }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al registrar la venta", detalle = ex.Message });
            }
        }

        // PUT: api/ventas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarVenta(int id, [FromBody] VentaDTO ventaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var ventaExistente = await _ventaRepositorio.ObtenerPorIdAsync(id);

                if (ventaExistente == null)
                {
                    return NotFound(new { error = "Venta no encontrada" });
                }

                // Actualizar campos básicos
                ventaExistente.ClienteId = ventaDto.ClienteId;
                ventaExistente.Fecha = ventaDto.Fecha;
                ventaExistente.MetodoPago = ventaDto.MetodoPago;
                ventaExistente.Estado = ventaDto.Estado;
                ventaExistente.Total = ventaDto.Total;

                await _ventaRepositorio.ActualizarAsync(ventaExistente);

                return Ok(new { mensaje = "Venta actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar la venta", detalle = ex.Message });
            }
        }

        // DELETE: api/ventas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarVenta(int id)
        {
            try
            {
                var venta = await _ventaRepositorio.ObtenerPorIdAsync(id);

                if (venta == null)
                {
                    return NotFound(new { error = "Venta no encontrada" });
                }

                await _ventaRepositorio.EliminarAsync(id);

                return Ok(new { mensaje = "Venta eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al eliminar la venta", detalle = ex.Message });
            }
        }
    }
}
