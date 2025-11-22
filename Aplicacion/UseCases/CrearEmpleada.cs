using Dominio.Entities;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class CrearEmpleada
    {
        private readonly IEmpleadaRepositorio _empleadaRepositorio;

        public CrearEmpleada(IEmpleadaRepositorio empleadaRepositorio)
        {
            _empleadaRepositorio = empleadaRepositorio;
        }

        public async Task EjecutarAsync(Empleada empleada)
        {
            ValidarEmpleada(empleada);
            
            empleada.Activo = true;

            await _empleadaRepositorio.CrearAsync(empleada);
        }

        private void ValidarEmpleada(Empleada empleada)
        {
            if (string.IsNullOrEmpty(empleada.Nombre) || empleada.Nombre.Length < 3)
            {
                throw new ArgumentException("El nombre de la empleada es inválido. Debe tener al menos 3 caracteres.");
            }

            if (string.IsNullOrEmpty(empleada.Email) || !empleada.Email.Contains("@"))
            {
                throw new ArgumentException("El correo electrónico de la empleada es inválido.");
            }

            if (empleada.PorcentajeComision < 0 || empleada.PorcentajeComision > 100)
            {
                throw new ArgumentException("El porcentaje de comisión debe estar entre 0 y 100.");
            }

            if (empleada.FechaContratacion > DateTime.Now)
            {
                throw new ArgumentException("La fecha de contratación no puede ser futura.");
            }
        }
    }
}
