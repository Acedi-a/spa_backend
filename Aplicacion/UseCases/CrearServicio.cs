using Dominio.Entities;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class CrearServicio
    {
        private readonly IServicioRepositorio _servicioRepositorio;

        public CrearServicio(IServicioRepositorio servicioRepositorio)
        {
            _servicioRepositorio = servicioRepositorio;
        }

        public async Task EjecutarAsync(Servicio servicio)
        {
            ValidarServicio(servicio);
            
            servicio.Activo = true;

            await _servicioRepositorio.CrearAsync(servicio);
        }

        private void ValidarServicio(Servicio servicio)
        {
            if (string.IsNullOrEmpty(servicio.Nombre) || servicio.Nombre.Length < 3)
            {
                throw new ArgumentException("El nombre del servicio es inválido. Debe tener al menos 3 caracteres.");
            }

            if (servicio.Precio < 0)
            {
                throw new ArgumentException("El precio del servicio no puede ser negativo.");
            }

            if (servicio.Duracion <= 0)
            {
                throw new ArgumentException("La duración del servicio debe ser mayor a 0 minutos.");
            }

            if (servicio.Duracion > 480)
            {
                throw new ArgumentException("La duración del servicio no puede exceder 480 minutos (8 horas).");
            }

            if (servicio.CategoriaId <= 0)
            {
                throw new ArgumentException("Debe especificar una categoría válida.");
            }

            if (servicio.EmpleadaId <= 0)
            {
                throw new ArgumentException("Debe especificar una empleada encargada válida.");
            }
        }
    }
}
