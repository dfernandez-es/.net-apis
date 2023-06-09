using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webapiautores.DTOs
{
    public class AutorCreacionDTO
    {
        [Required]
        [StringLength(maximumLength:120,ErrorMessage = "El campo {0} no debe contener mas de {1} caracteres")]
        public string Nombre { get; set; }
    }
}