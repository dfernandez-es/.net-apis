using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapiautores.DTOs
{
    public class LibroCreacionDTO
    {
        public string Titulo { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}