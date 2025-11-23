using System;
using System.Collections.Generic;

namespace Aplication.DTOs
{
    public class ReporteDesempeñoDTO
    {
        public int EmpleadaId { get; set; }
      public string? NombreEmpleada { get; set; }
 public string? Especialidad { get; set; }

      public double PromedioCalificacion { get; set; }
  public int TotalValoraciones { get; set; }

   public Dictionary<int, int> DistribucionCalificaciones { get; set; } = new();

      public List<ValoracionDTO> Valoraciones { get; set; } = new();

        public DateTime FechaGeneracion { get; set; }
      public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

     // Estadísticas adicionales
       public int CalificacionesAltasCount { get; set; } // 4-5 estrellas
        public int CalificacionesMediasCount { get; set; } // 3 estrellas
  public int CalificacionesBajasCount { get; set; } // 1-2 estrellas

     public string? ObservacionesGenerales { get; set; }
    }
}
