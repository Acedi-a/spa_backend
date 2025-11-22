using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;

namespace Aplication.UseCases
{
    public class CrearCurso
    {
       private readonly ICursoRepositorio _cursoRepositorio;

        public CrearCurso(ICursoRepositorio cursoRepositorio)
        {
            _cursoRepositorio = cursoRepositorio;
        }

        public async Task EjecutarAsync (Curso curso)
        {
            ValidarCurso(curso);
            await _cursoRepositorio.CrearAsync(curso);
        }

        private void ValidarCurso(Curso curso)
        {
            if (string.IsNullOrEmpty(curso.Nombre) || curso.Nombre.Length < 3)
            {
                throw new ArgumentException("El título del curso es inválido. Debe tener al menos 5 caracteres.");
            }
            if (string.IsNullOrEmpty(curso.Descripcion) || curso.Descripcion.Length < 10)
            {
                throw new ArgumentException("La descripción del curso es inválida. Debe tener al menos 10 caracteres.");
            }

        }
    }
}
