using Backend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;

namespace PasantiaTI1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeliculasController : ControllerBase
    {
        private readonly ILogger<PeliculasController> _logger;

        public PeliculasController(ILogger<PeliculasController> logger)
        {
            _logger = logger;
        }

        // Lista estática simulando base de datos
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
        // Devuelve todas las películas, o filtra por Activa=true/false si se pasa en query
        [HttpGet]
        public IActionResult Get([FromQuery] bool? activa)
        {
            _logger.LogInformation("Obteniendo lista de peliculas");
            IEnumerable<PeliculaDTO> resultado = peliculas.Select(p => MapearADTO(p));

            if (activa.HasValue)
                resultado = resultado.Where(p => p.Activa == activa.Value);

            return Ok(resultado);
        }

        // GET /peliculas/Buscar{id}
        [HttpGet("Buscar/{id}")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation("Buscando pelicula con ID {Id}", id);
            var pelicula = peliculas.FirstOrDefault(p => p.Id == id);
            if (pelicula == null)
            {
                _logger.LogWarning("Película no encontrada con ID {Id}", id);
                return NotFound(new { mensaje = "Película no encontrada" });
            }

            return Ok(MapearADTO(pelicula));
        }

        // POST /peliculas/Agregar{id}
        // Agrega nueva película con validación de campos
        [HttpPost("Agregar")]
        public IActionResult Add([FromBody] Pelicula nuevaPelicula)
        {
            var errores = ValidarPelicula(nuevaPelicula);

            if (errores.Any())
            {
                _logger.LogError("Errores al crear película: {Errores}", string.Join(", ", errores));
                Console.WriteLine("Errores al crear película: " + string.Join(", ", errores));
                return BadRequest(new { errores });
            }

            int nuevoId = peliculas.Any() ? peliculas.Max(p => p.Id) + 1 : 1;

            // Si Activa no se envía, por defecto true
            nuevaPelicula.Activa = nuevaPelicula.Activa;

            nuevaPelicula.Id = nuevoId;
            peliculas.Add(nuevaPelicula);

            _logger.LogInformation("Película creada correctamente con ID {Id}", nuevaPelicula.Id);

            return CreatedAtAction(nameof(GetById), new { id = nuevaPelicula.Id }, MapearADTO(nuevaPelicula));
        }

        // PUT /peliculas/Actualizar{id}
        [HttpPut("Actualizar{id}")]
        public IActionResult Update(int id, Pelicula peliculaActualizada)
        {
            _logger.LogInformation("Intentando actualizar pelicula con ID {Id}", id);

            var pelicula = peliculas.FirstOrDefault(p => p.Id == id);
            if (pelicula == null)
            {
                _logger.LogWarning("Intento de actualizar película inexistente con ID {Id}", id);
                return NotFound(new { mensaje = "Película no encontrada" });
            }

            var errores = ValidarPelicula(peliculaActualizada);
            if (errores.Any())
            {
                _logger.LogError("Error al actualizar película: {Errores}", string.Join(", ", errores));
                return BadRequest(errores);
            }

            pelicula.Titulo = peliculaActualizada.Titulo;
            pelicula.Director = peliculaActualizada.Director;
            pelicula.Genero = peliculaActualizada.Genero;
            pelicula.DuracionMinutos = peliculaActualizada.DuracionMinutos;
            pelicula.PrecioRecaudacion = peliculaActualizada.PrecioRecaudacion;
            pelicula.FechaEstreno = peliculaActualizada.FechaEstreno;
            pelicula.Activa = peliculaActualizada.Activa;

            _logger.LogInformation("Película actualizada correctamente con ID {Id}", id);
            return Ok(MapearADTO(pelicula));
        }

        // PATCH /peliculas/{id}/activar?activa=true/false
        // Cambia el estado Activa de la película
        [HttpPatch("Activar{id}")]
        public IActionResult CambiarEstado(int id, [FromQuery] bool activa)
        {
            var pelicula = peliculas.FirstOrDefault(p => p.Id == id);
            if (pelicula == null)
                return NotFound(new { mensaje = "Película no encontrada" });

            pelicula.Activa = activa;
            _logger.LogInformation("Película {Id} cambiada a Activa={Activa}", id, activa);

            return Ok(MapearADTO(pelicula));
        }

        // DELETE /peliculas/Eliminar{id}
        [HttpDelete("Eliminar{id}")]
        public IActionResult Delete(int id)
        {
            var pelicula = peliculas.FirstOrDefault(p => p.Id == id);
            if (pelicula == null)
            {
                _logger.LogWarning("Intento de eliminar película inexistente con ID {Id}", id);
                return NotFound(new { mensaje = "Película no encontrada" });
            }

            peliculas.Remove(pelicula);
            _logger.LogInformation("Película eliminada correctamente con ID {Id}", id);
            return NoContent();
        }

        // Valida los campos de una película y devuelve lista de errores
        private List<string> ValidarPelicula(Pelicula p)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(p.Titulo))
                errores.Add("El título no puede estar vacío");

            if (string.IsNullOrWhiteSpace(p.Director))
                errores.Add("El director no puede estar vacío");

            if (string.IsNullOrWhiteSpace(p.Genero))
                errores.Add("El género no puede estar vacío");

            if (p.DuracionMinutos <= 0)
                errores.Add("La duración de minutos no debe estar vacía");

            if (p.PrecioRecaudacion <= 0)
                errores.Add("El precio de recaudación no debe estar vacío");

            return errores;
        }

        // Mapear Pelicula a DTO para salida más legible
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