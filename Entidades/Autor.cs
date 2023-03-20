using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webapiautores.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:120,ErrorMessage = "El campo {0} no debe contener mas de {1} caracteres")]
        public string nombre { get; set; }
        public List<Libro> Libros { get; set; }
        public List<AutorLibros> AutoresLibros { get; set; }
    }
}