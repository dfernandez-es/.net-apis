using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapiautores.Entidades;
using Microsoft.EntityFrameworkCore;
using webapiautores.DTOs;
using AutoMapper;

namespace webapiautores.Controllers
{
    [ApiController]
    [Route("/api/autores")]
    public class AutoresController: ControllerBase
    {
        public ApplicationDbContext context { get; }
        public IMapper Mapper { get; }

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.Mapper = mapper;
            this.context = context;
            
        }

        [HttpGet]
        public async Task<List<AutorDTO>> Get(){
            var autores = await context.Autores.Include(x=>x.Libros).ToListAsync();
            return Mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpGet("{id:int}",Name = "obtenerAutor")]
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id){
            var autor = await context.Autores.Include(x=>x.AutoresLibros).ThenInclude(autorLibro=>autorLibro.Libro).FirstOrDefaultAsync(x=>x.Id == id);
            if(autor==null){
                return NotFound($"No existe el autor con id: {id}");
            }
            return Mapper.Map<AutorDTOConLibros>(autor);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDTO>>> Get(string nombre){
            var autores = await context.Autores.Include(x=>x.Libros).Where(x=>x.nombre.Contains(nombre)).ToListAsync();
            if(autores==null){
                return NotFound($"No existe ningun autor con nombre : {nombre}");
            }
            return Mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpPost]
        public async Task<ActionResult> Post(AutorCreacionDTO autor){

            var existeAutorMismoNombre = await context.Autores.AnyAsync(x=>x.nombre == autor.Nombre);

            if(existeAutorMismoNombre){
                return BadRequest($"Ya existe un autor con ese mismo nombre: {autor.Nombre}");
            }

            var nuevoAutor = Mapper.Map<Autor>(autor);
            context.Add(nuevoAutor);
            await context.SaveChangesAsync();
            var AutorDTO = Mapper.Map<AutorDTO>(nuevoAutor);
            return CreatedAtRoute("obtenerAutor", new{id= nuevoAutor.Id},AutorDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id){

            var existe = await context.Autores.AnyAsync(x=>x.Id == id);

            if(!existe){
                return NotFound();
            }

            var autor = Mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id){
            var existe = await context.Autores.AnyAsync(a => a.Id == id);
            if(!existe){
                return  NotFound();
            }
            context.Remove(new Autor() {Id = id});
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}