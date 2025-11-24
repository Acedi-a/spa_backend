using Aplication.DTOs;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class ConsultarHistorialCliente
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public ConsultarHistorialCliente(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public async Task<HistorialClienteDto?> EjecutarAsync(Guid qrCode)
        {
            // Paso 3: Consultar cliente con su historial de ventas
            var cliente = await _clienteRepositorio.ObtenerPorQrConHistorialAsync(qrCode);

            if (cliente == null)
            {
                return null;
            }

            // Paso 5: Estructurar datos en "Historial de Cliente"
            var historial = new HistorialClienteDto
            {
                ClienteId = cliente.Id,
                Nombre = cliente.Nombre,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                FechaNacimiento = cliente.FechaNacimiento,
                FechaRegistro = cliente.FechaRegistro,
                Preferencias = cliente.Preferencias,
                Activo = cliente.Activo
            };

            // Procesar ventas si existen
            if (cliente.Ventas != null && cliente.Ventas.Any())
            {
                var ventas = cliente.Ventas.OrderByDescending(v => v.Fecha).ToList();

                // Estadísticas
                historial.TotalVentas = ventas.Count;
                historial.TotalGastado = ventas.Sum(v => v.Total);
                historial.PromedioGasto = ventas.Average(v => v.Total);
                historial.UltimaCompra = ventas.Max(v => v.Fecha);

                // Mapear ventas con detalles
                historial.Ventas = ventas.Select(v => new VentaHistorialDto
                {
                    Id = v.Id,
                    Fecha = v.Fecha,
                    Total = v.Total,
                    MetodoPago = v.MetodoPago,
                    Estado = v.Estado,
                    CantidadItems = v.DetalleVentas?.Count ?? 0,
                    Detalles = v.DetalleVentas?.Select(dv => new DetalleVentaHistorialDto
                    {
                        Id = dv.Id,
                        TipoItem = dv.ProductoId.HasValue ? "Producto" : "Servicio",
                        ProductoId = dv.ProductoId,
                        NombreProducto = dv.Producto?.Nombre,
                        ServicioId = dv.ServicioId,
                        NombreServicio = dv.Servicio?.Nombre,
                        Cantidad = dv.Cantidad,
                        PrecioUnitario = dv.PrecioUnitario,
                        Subtotal = dv.Subtotal
                    }).ToList() ?? new List<DetalleVentaHistorialDto>()
                }).ToList();
            }
            else
            {
                // Cliente sin ventas
                historial.TotalVentas = 0;
                historial.TotalGastado = 0;
                historial.PromedioGasto = 0;
                historial.UltimaCompra = null;
            }

            return historial;
        }
    }
}
