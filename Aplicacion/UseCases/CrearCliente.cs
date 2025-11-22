using Dominio.Entities;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class CrearCliente
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public CrearCliente(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public async Task EjecutarAsync(Cliente cliente)
        {
            ValidarCliente(cliente);
            
            cliente.Id = Guid.NewGuid();
            cliente.FechaRegistro = DateTime.Now;
            cliente.Activo = true;

            await _clienteRepositorio.CrearAsync(cliente);
        }

        private void ValidarCliente(Cliente cliente)
        {
            if (string.IsNullOrEmpty(cliente.Nombre) || cliente.Nombre.Length < 3)
            {
                throw new ArgumentException("El nombre del cliente es inválido. Debe tener al menos 3 caracteres.");
            }

            if (!string.IsNullOrEmpty(cliente.Email) && !cliente.Email.Contains("@"))
            {
                throw new ArgumentException("El correo electrónico del cliente es inválido.");
            }

            if (cliente.FechaNacimiento > DateTime.Now)
            {
                throw new ArgumentException("La fecha de nacimiento no puede ser futura.");
            }

            var edad = DateTime.Now.Year - cliente.FechaNacimiento.Year;
            if (edad < 0 || edad > 120)
            {
                throw new ArgumentException("La fecha de nacimiento no es válida.");
            }
        }
    }
}
