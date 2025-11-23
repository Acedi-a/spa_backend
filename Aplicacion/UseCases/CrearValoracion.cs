using Dominio.Entities;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class CrearValoracion
    {
     private readonly IValoracionRepositorio _valoracionRepositorio;

        public CrearValoracion(IValoracionRepositorio valoracionRepositorio)
      {
      _valoracionRepositorio = valoracionRepositorio;
        }

   public async Task EjecutarAsync(Valoracion valoracion)
        {
     ValidarValoracion(valoracion);

   valoracion.Fecha = DateTime.Now;

  await _valoracionRepositorio.CrearAsync(valoracion);
        }

     private void ValidarValoracion(Valoracion valoracion)
{
  // Validar cliente
       if (valoracion.ClienteId == Guid.Empty)
            {
    throw new ArgumentException("El cliente es obligatorio.");
           }

  // Validar servicio
       if (valoracion.ServicioId <= 0)
 {
       throw new ArgumentException("El servicio es obligatorio.");
        }

      // Validar venta
            if (valoracion.VentaId <= 0)
    {
        throw new ArgumentException("La venta es obligatoria.");
       }

      // Validar calificación
if (valoracion.Calificacion < 1 || valoracion.Calificacion > 5)
   {
  throw new ArgumentException("La calificación debe estar entre 1 y 5.");
}

      // Validar comentario
     if (!string.IsNullOrEmpty(valoracion.Comentario) && valoracion.Comentario.Length > 1000)
        {
       throw new ArgumentException("El comentario no puede exceder 1000 caracteres.");
 }
        }
    }
}
