using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapiautores.Entidades;

namespace webapiautores.DTOs
{
    public class LibroDTO
    {
        public int Id { get; set; } 
        public string Titulo { get; set; }
        public List<ComentarioDTO> Comentarios { get; set; }
    }
}