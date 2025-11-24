using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface IBackupRepositorio
    {
  
        Task<byte[]> CrearBackupAsync();

        
      /// <param name="archivoBackup">Array de bytes del archivo de backup</param>
        
        Task<bool> RestaurarBackupAsync(byte[] archivoBackup);
    }
}
