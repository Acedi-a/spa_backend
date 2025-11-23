using Aplication.DTOs;
using AutoMapper;
using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> DTO
            CreateMap<Curso, CursoDTO>()
                .ForMember(dest => dest.DocenteId, opt => opt.MapFrom(src => src.Id_docente))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.Fecha_inicio))
                .ForMember(dest => dest.FechaFin, opt => opt.Ignore());

            // DTO -> Entity
            CreateMap<CursoDTO, Curso>()
                .ForMember(dest => dest.Id_docente, opt => opt.MapFrom(src => src.DocenteId))
                .ForMember(dest => dest.Fecha_inicio, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.docente, opt => opt.Ignore());

            CreateMap<Docente, DocenteDTO>().ReverseMap();
            
            CreateMap<Cliente, ClienteDTO>().ReverseMap();
            
            CreateMap<Servicio, ServicioDTO>().ReverseMap();
            
            CreateMap<Empleada, EmpleadaDTO>().ReverseMap();
            
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            
            CreateMap<Venta, VentaDTO>().ReverseMap();
            
            CreateMap<DetalleVenta, DetalleVentaDTO>().ReverseMap();

            // Producto mappings
            CreateMap<Producto, ProductoDTO>().ReverseMap();

            CreateMap<Cita, CitaDTO>().ReverseMap();

            // Valoración mappings
            // Para POST/PUT - sin propiedades calculadas
            CreateMap<ValoracionCrearDTO, Valoracion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Fecha, opt => opt.Ignore())
                .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.Servicio, opt => opt.Ignore())
                .ForMember(dest => dest.Empleada, opt => opt.Ignore())
                .ForMember(dest => dest.Venta, opt => opt.Ignore());

            // Para GET - con propiedades calculadas
            CreateMap<Valoracion, ValoracionDTO>()
                .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente!.Nombre))
                .ForMember(dest => dest.NombreServicio, opt => opt.MapFrom(src => src.Servicio!.Nombre))
                .ForMember(dest => dest.NombreEmpleada, opt => opt.MapFrom(src => src.Empleada!.Nombre));
        }
    }
}

