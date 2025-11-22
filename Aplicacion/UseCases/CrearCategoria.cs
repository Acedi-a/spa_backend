using Dominio.Entities;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class CrearCategoria
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;

        public CrearCategoria(ICategoriaRepositorio categoriaRepositorio)
        {
            _categoriaRepositorio = categoriaRepositorio;
        }

        public async Task EjecutarAsync(Categoria categoria)
        {
            ValidarCategoria(categoria);
            
            categoria.Activo = true;

            await _categoriaRepositorio.CrearAsync(categoria);
        }

        private void ValidarCategoria(Categoria categoria)
        {
            if (string.IsNullOrEmpty(categoria.Nombre) || categoria.Nombre.Length < 3)
            {
                throw new ArgumentException("El nombre de la categoría es inválido. Debe tener al menos 3 caracteres.");
            }
        }
    }
}
