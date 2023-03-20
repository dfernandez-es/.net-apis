using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using webapiautores.DTOs;
using webapiautores.Entidades;

namespace webapiautores.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(){
            CreateMap<AutorCreacionDTO,Autor>();
            CreateMap<Autor,AutorDTO>();
            CreateMap<Autor,AutorDTOConLibros>().ForMember(autorDTO=>autorDTO.Libros, opciones=>opciones.MapFrom(MapAutorDTOLibros));
            CreateMap<LibroCreacionDTO,Libro>().ForMember(libro => libro.AutoresLibros, opciones=>opciones.MapFrom(MapAutoresLibros));  
            CreateMap<Libro,LibroDTOConAutores>().ForMember(x=>x.Autores, opciones=> opciones.MapFrom(MapLibroDTOAutores));
            CreateMap<Libro,LibroDTO>();
            CreateMap<ComentarioCreacionDTO,Comentario>();
            CreateMap<Comentario,ComentarioDTO>();
        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO){
            var resultado = new List<LibroDTO>();

            if(autor.AutoresLibros == null){return resultado;}

            foreach(var autorLibro in autor.AutoresLibros){
                resultado.Add(new LibroDTO(){Id = (int)autorLibro.LibroId, Titulo= autorLibro.Libro.Titulo});
            }

            return resultado;
        }

        private List<AutorLibros> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Libro libro){
            var resultado = new List<AutorLibros>();

            if(libroCreacionDTO.AutoresIds == null){return resultado;}

            foreach(var autorId in libroCreacionDTO.AutoresIds){
                resultado.Add(new AutorLibros(){AutorId = autorId});
            }
            return resultado;
        }

        private List<AutorDTO> MapLibroDTOAutores (Libro libro, LibroDTO libroDTO){
            var resultado = new List<AutorDTO>();

            if(libro.AutoresLibros == null){return resultado;}

            foreach(var autorLibro in libro.AutoresLibros){
                resultado.Add(new AutorDTO(){Id = autorLibro.AutorId, Nombre = autorLibro.Autor.nombre});
            }

            return resultado;
        }

    }
}