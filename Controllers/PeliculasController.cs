using Microsoft.AspNetCore.Mvc;
using PasantiaTI.Models;
using PasantiaTI1.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PasantiaTI1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {   // lista estática que simula una "Base de datos" en memoria
        private static List<Pelicula> peliculas = new()
        {
            new Pelicula
            {
                Id = 1,
                Titulo = "Avengers Infinity War",
                Director = "Anthony and Joe Russo",
                Genero = "Ciencia Ficcion",
                DuracionMinutos = 149,
                PrecioRecaudacion = 100,
                FechaEstreno = new DateOnly(2018, 4, 27),
                Activa = true
            },
            new Pelicula
            {
                Id = 2,
                Titulo = "Rapidos y Furiosos 5: Sin control",
                Director = "Justin Lin",
                Genero = "Accion",
                DuracionMinutos = 130,
                PrecioRecaudacion = 150,
                FechaEstreno = new DateOnly(2011, 4, 29),
                Activa = true
            }
        };

        // GET /peliculas
        //Devuelve todas las peliculas
        [HttpGet]
        public IActionResult Get()
        {   // Mapear cada pelicula a peliculaDTO antes de enviarlo
            var resultado = peliculas.Select(p => MapearADTO(p)).ToList();
            return Ok(resultado);
        }

        // GET /peliculas/{id}
        // Devuelve una pelicula especifica segun su ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {   // Buscar la pelicula por ID
            var pelicula = peliculas.FirstOrDefault(p => p.Id == id);
            if (pelicula == null)
                return NotFound(); //Si no se encuentra, devuelve 404

            return Ok(MapearADTO(pelicula)); //Devuelve la pelicula encontrada
        }

        // POST /peliculas
        // Agrega una nueva pelicula a la lista
        [HttpPost]
        public IActionResult Post(Pelicula pelicula)
        {
            peliculas.Add(pelicula); // Añadir la nueva pelicula
            // Devuelve 201 created con la pelicula agregada
            return CreatedAtAction(nameof(GetById), new { id = pelicula.Id }, MapearADTO(pelicula));
        }

        // PUT /peliculas/{id}
        // Actualiza una pelicula existente
        [HttpPut("{id}")]
        public IActionResult Put(int id, Pelicula peliculaActualizada)
        {   // Buscar la pelicula por ID
            var pelicula = peliculas.FirstOrDefault(p => p.Id == id);
            if (pelicula == null)
                return NotFound(); // 404 si no existe
            // Actualiza los campos de la pelicula
            pelicula.Titulo = peliculaActualizada.Titulo;
            pelicula.Director = peliculaActualizada.Director;
            pelicula.Genero = peliculaActualizada.Genero;
            pelicula.DuracionMinutos = peliculaActualizada.DuracionMinutos;
            pelicula.PrecioRecaudacion = peliculaActualizada.PrecioRecaudacion;
            pelicula.FechaEstreno = peliculaActualizada.FechaEstreno;
            pelicula.Activa = peliculaActualizada.Activa;

            return Ok(MapearADTO(pelicula)); // Devuelve la pelicula actualizada
        }

        // DELETE /peliculas/{id}
        // Elimina una pelicula por su ID
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {   // Buscar la pelicula por ID
            var pelicula = peliculas.FirstOrDefault(p => p.Id == id);
            if (pelicula == null)
                return NotFound(); // 404 si no existe

            // Eliminar la pelicula  de la lista
            peliculas.Remove(pelicula);

            return NoContent(); // Devuelve 204 No content
        }

        // Método privado para mapear Pelicula -> PeliculaDTO
        //Convierte un Objeto pelicula a Pelicula DTO para controlar la salida
        private PeliculaDTO MapearADTO(Pelicula p)
        {
            return new PeliculaDTO
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Director = p.Director,
                Genero = p.Genero,
                DuracionMinutos = p.DuracionMinutos,
                PrecioRecaudacion = $"{p.PrecioRecaudacion} millones US",
                FechaEstreno = p.FechaEstreno,
                Activa = p.Activa
            };
        }
    }
}