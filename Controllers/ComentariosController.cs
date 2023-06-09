using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapiautores.DTOs;
using webapiautores.Entidades;

namespace webapiautores.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController: ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;
        public ComentariosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId){

            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if(!existeLibro){
                return NotFound($"No existe el Libro con id: {libroId}");
            }

            var comentarios = await context.Comentarios.Where(x => x.LibroId == libroId).ToListAsync();
            return mapper.Map<List<ComentarioDTO>>(comentarios);
        }

        [HttpGet("{id:int}",Name = "obtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> GetPorId(int id){
            var comentario = await context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);
            if(comentario == null){return NotFound();}
            return mapper.Map<ComentarioDTO>(comentario);

        }

        [HttpPost]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO){
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if(!existeLibro){
                return NotFound($"No existe el Libro con id: {libroId}");
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            context.Add(comentario);
            await context.SaveChangesAsync();
            var ComentarioDTO = mapper.Map<ComentarioDTO>(comentario);
            return CreatedAtRoute("obtenerComentario",new {id=comentario.Id, libroId = libroId},ComentarioDTO);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int libroId, int id, ComentarioCreacionDTO comentarioCreacionDTO) { 
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if(!existeLibro){
                return NotFound($"No existe el Libro con id: {libroId}");
            }

            var existeComentario = await context.Comentarios.AnyAsync(x=>x.Id == id);
            if(!existeComentario){
                return NotFound($"No existe el Comentario con id: {id}");
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.Id = id;
            comentario.LibroId = libroId;
            context.Update(comentario);
            await context.SaveChangesAsync();
            return NoContent();

        }
    }
}