using System;
using System.Collections.Generic;

namespace Aplication.DTOs
{
 public class HorarioDisponibleDTO
    {
   public DateTime Fecha { get; set; }
   public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
  public int DuracionDisponible { get; set; } // en minutos
    }
}
