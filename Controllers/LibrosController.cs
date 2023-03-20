using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using webapiautores.Entidades;
using webapiautores.DTOs;
using AutoMapper;

namespace webapiautores.Controllers
{
    [ApiController]
    [Route("/api/libros")]
    public class LibrosController: ControllerBase
    {
        public ApplicationDbContext context { get; }
        public IMapper Mapper { get; }

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.Mapper = mapper;
            this.context = context;
        }

        [HttpGet("{id:int}", Name ="ObtenerLibro")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id){
            var libro = await context.Libros
            .Include(x=>x.Comentarios)
            .Include(x=>x.AutoresLibros)
            .ThenInclude(AutorLibroDB => AutorLibroDB.Autor)
            .FirstOrDefaultAsync(x => x.Id == id);

            if(libro==null){
                return BadRequest($"No existe el libro con id {id}");
            }

            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x=>x.Orden).ToList();

            return Mapper.Map<LibroDTOConAutores>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO){

            if(libroCreacionDTO.AutoresIds == null){
                return BadRequest("No se puede crear un libro sin autores");
            }

            var autoresIds = await context.Autores.Where(autorDB=> libroCreacionDTO.AutoresIds.Contains(autorDB.Id)).Select(x=>x.Id).ToListAsync();
            
            if(libroCreacionDTO.AutoresIds.Count != autoresIds.Count){
                return BadRequest("No existe alguno de los autores enviados");
            }
            
            var libro = Mapper.Map<Libro>(libroCreacionDTO);
            AsignarOrdenAutores(libro);

            context.Add(libro);
            await context.SaveChangesAsync();
            var libroDTO = Mapper.Map<LibroDTO>(libro);
            return CreatedAtRoute("ObtenerLibro",new {id=libro.Id},libroDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, LibroCreacionDTO libroCreacionDTO){
            var libroDB = await context.Libros.Include(x=>x.AutoresLibros).FirstOrDefaultAsync(x=>x.Id == id);
            if(libroDB == null){
                return NotFound();
            }

            libroDB = Mapper.Map(libroCreacionDTO,libroDB);
            AsignarOrdenAutores(libroDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private void AsignarOrdenAutores(Libro libro){
                if(libro.AutoresLibros != null){
                for(int i = 0; i < libro.AutoresLibros.Count; i++){
                    libro.AutoresLibros[i].Orden = i;
                }
            }
        }
    }
}