using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webapiautores.Entidades
{
    public class Libro
    {
        public int Id { get; set; } 
        [Required]
        public string Titulo { get; set; }
        public Autor Autor { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public List<AutorLibros> AutoresLibros { get; set; }
    }
}