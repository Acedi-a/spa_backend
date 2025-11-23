using Aplication.DTOs;
using AutoMapper;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
   public class GenerarReporteDesempeño
    {
   private readonly IValoracionRepositorio _valoracionRepositorio;
     private readonly IEmpleadaRepositorio _empleadaRepositorio;
     private readonly IMapper _mapper;

   public GenerarReporteDesempeño(IValoracionRepositorio valoracionRepositorio, IEmpleadaRepositorio empleadaRepositorio, IMapper mapper)
        {
     _valoracionRepositorio = valoracionRepositorio;
      _empleadaRepositorio = empleadaRepositorio;
      _mapper = mapper;
        }

   public async Task<ReporteDesempeñoDTO> EjecutarAsync(int empleadaId)
        {
    return await EjecutarAsync(empleadaId, null, null);
}

   public async Task<ReporteDesempeñoDTO> EjecutarAsync(int empleadaId, DateTime? fechaInicio, DateTime? fechaFin)
         {
    ValidarDatos(empleadaId);

    // Obtener datos de la empleada
        var empleada = await _empleadaRepositorio.ObtenerPorIdAsync(empleadaId);
  if (empleada == null)
      {
     throw new ArgumentException($"La empleada con ID {empleadaId} no existe.");
    }

     // Obtener valoraciones
    IEnumerable<Dominio.Entities.Valoracion> valoraciones;
        if (fechaInicio.HasValue && fechaFin.HasValue)
  {
      valoraciones = await _valoracionRepositorio.ObtenerPorEmpleadoYFechaAsync(empleadaId, fechaInicio.Value, fechaFin.Value);
   }
 else
    {
           valoraciones = await _valoracionRepositorio.ObtenerPorEmpleadoAsync(empleadaId);
    }

     var valoracionesList = valoraciones.ToList();

     // Calcular estadísticas
        var promedio = await _valoracionRepositorio.ObtenerPromedioCalificacionAsync(empleadaId);
  var distribucion = CalcularDistribucion(valoracionesList);
 var estadisticas = CalcularEstadisticas(valoracionesList);

      // Crear reporte
  var reporte = new ReporteDesempeñoDTO
 {
         EmpleadaId = empleadaId,
    NombreEmpleada = empleada.Nombre,
         Especialidad = empleada.Especialidad,
  PromedioCalificacion = promedio,
   TotalValoraciones = valoracionesList.Count,
   DistribucionCalificaciones = distribucion,
           Valoraciones = _mapper.Map<List<ValoracionDTO>>(valoracionesList),
         FechaGeneracion = DateTime.Now,
   FechaInicio = fechaInicio,
      FechaFin = fechaFin,
    CalificacionesAltasCount = estadisticas["Altas"],
  CalificacionesMediasCount = estadisticas["Medias"],
  CalificacionesBajasCount = estadisticas["Bajas"],
  ObservacionesGenerales = GenerarObservaciones(promedio, valoracionesList.Count)
       };

    return reporte;
     }

  private Dictionary<int, int> CalcularDistribucion(List<Dominio.Entities.Valoracion> valoraciones)
       {
 var distribucion = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };

  foreach (var valoracion in valoraciones)
{
        if (distribucion.ContainsKey(valoracion.Calificacion))
{
       distribucion[valoracion.Calificacion]++;
    }
  }

    return distribucion;
  }

     private Dictionary<string, int> CalcularEstadisticas(List<Dominio.Entities.Valoracion> valoraciones)
       {
  var estadisticas = new Dictionary<string, int>
         {
         { "Altas", valoraciones.Count(v => v.Calificacion >= 4) },
  { "Medias", valoraciones.Count(v => v.Calificacion == 3) },
  { "Bajas", valoraciones.Count(v => v.Calificacion <= 2) }
            };

      return estadisticas;
     }

    private string GenerarObservaciones(double promedio, int totalValoraciones)
        {
         if (totalValoraciones == 0)
    {
   return "Sin valoraciones registradas.";
   }

        if (promedio >= 4.5)
 {
      return "Desempeño excelente. La empleada mantiene altos estándares de calidad.";
  }
       else if (promedio >= 4.0)
{
        return "Desempeño muy bueno. La empleada cumple consistentemente con expectativas.";
 }
   else if (promedio >= 3.0)
      {
       return "Desempeño aceptable. Se recomienda seguimiento y capacitación.";
        }
   else
     {
 return "Desempeño por debajo de lo esperado. Requiere intervención inmediata.";
     }
  }

   private void ValidarDatos(int empleadaId)
        {
 if (empleadaId <= 0)
           {
      throw new ArgumentException("El ID de la empleada es inválido.");
 }
    }
    }
}
