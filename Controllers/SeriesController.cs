using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PasantiaTI.Models;
using PasantiaTI1.Models;
using System.Collections.Generic;
using System.Linq;

namespace PasantiaTI1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ILogger<SeriesController> _logger;

        public SeriesController(ILogger<SeriesController> logger)
        {
            _logger = logger;
        }

        // Lista de series en memoria (simula base de datos)
        private static List<Serie> series = new()
        {
            new Serie
            {
                Id = 1,
                Titulo = "The God Doctor",
                Plataforma = "Netflix",
                AnioEstreno = 2017,
                Genero = "Drama",
                Activa = true,
                TemporadasEpisodios = new List<Temporada>
                {
                    new Temporada { temporada = 1, Episodios = 18 },
                    new Temporada { temporada = 2, Episodios = 18 },
                    new Temporada { temporada = 3, Episodios = 20 },
                    new Temporada { temporada = 4, Episodios = 20 },
                    new Temporada { temporada = 5, Episodios = 18 },
                    new Temporada { temporada = 6, Episodios = 18 },
                    new Temporada { temporada = 7, Episodios = 10 },
                }
            },
            new Serie
            {
                Id = 2,
                Titulo = "Loki",
                Plataforma = "Disney +",
                AnioEstreno = 2021,
                Genero = "Ciencia Ficcion",
                Activa = true,
                TemporadasEpisodios = new List<Temporada>
                {
                    new Temporada { temporada = 1, Episodios = 6 },
                    new Temporada { temporada = 2, Episodios = 6 },
                }
            }
        };

        // GET /series
        // Devuelve todas las series o filtra por estado Activa si se pasa el query ?activa=true/false
        [HttpGet]
        public IActionResult Get([FromQuery] bool? activa)
        {
            IEnumerable<Serie> resultado = series;

            if (activa.HasValue)
                resultado = resultado.Where(s => s.Activa == activa.Value);

            return Ok(resultado);
        }

        // GET /series/{id}
        [HttpGet("Buscar{id}")]
        public IActionResult GetById(int id)
        {
            var serie = series.FirstOrDefault(s => s.Id == id);
            if (serie == null)
                return NotFound();

            return Ok(serie);
        }

        // POST /series
        [HttpPost("Agregar{id}")]
        public IActionResult Post(Serie nuevaSerie)
        {
            var errores = ValidarSerie(nuevaSerie);
            if (errores.Any())
            {
                foreach (var error in errores)
                    _logger.LogWarning("Validación fallida: {Error}", error);

                return BadRequest(errores);
            }

            // Generar ID automático
            nuevaSerie.Id = series.Any() ? series.Max(s => s.Id) + 1 : 1;

            // Si Activa no se envía, por defecto es true
            nuevaSerie.Activa = nuevaSerie.Activa;

            series.Add(nuevaSerie);

            return CreatedAtAction(nameof(GetById), new { id = nuevaSerie.Id }, nuevaSerie);
        }

        // PUT /series/{id}
        [HttpPut("Actualizar{id}")]
        public IActionResult Put(int id, Serie serieActualizada)
        {
            var serie = series.FirstOrDefault(s => s.Id == id);
            if (serie == null)
                return NotFound();

            var errores = ValidarSerie(serieActualizada);
            if (errores.Any())
            {
                foreach (var error in errores)
                    _logger.LogWarning("Validación fallida: {Error}", error);

                return BadRequest(errores);
            }

            serie.Titulo = serieActualizada.Titulo;
            serie.Plataforma = serieActualizada.Plataforma;
            serie.AnioEstreno = serieActualizada.AnioEstreno;
            serie.Genero = serieActualizada.Genero;
            serie.Activa = serieActualizada.Activa;
            serie.TemporadasEpisodios = serieActualizada.TemporadasEpisodios;

            return Ok(serie);
        }

        
        // Cambia el estado activo/inactivo de una serie
        [HttpPatch("Activar{id}")]
        public IActionResult CambiarEstado(int id, [FromQuery] bool activa)
        {
            var serie = series.FirstOrDefault(s => s.Id == id);
            if (serie == null)
                return NotFound();

            serie.Activa = activa;

            _logger.LogInformation("Serie {Id} cambiada a Activa={Activa}", id, activa);

            return Ok(serie);
        }

        // DELETE /series/{id}
        [HttpDelete("Eliminar{id}")]
        public IActionResult Delete(int id)
        {
            var serie = series.FirstOrDefault(s => s.Id == id);
            if (serie == null)
                return NotFound();

            series.Remove(serie);
            return NoContent();
        }

        // Validación de campos de la serie y sus temporadas
        private List<string> ValidarSerie(Serie serie)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(serie.Titulo))
                errores.Add("El título no puede estar vacío");

            if (string.IsNullOrWhiteSpace(serie.Plataforma))
                errores.Add("La plataforma no puede estar vacía");

            if (serie.AnioEstreno <= 0)
                errores.Add("El año de estreno debe ser mayor a 0");

            if (string.IsNullOrWhiteSpace(serie.Genero))
                errores.Add("El género no puede estar vacío");

            if (serie.TemporadasEpisodios == null || !serie.TemporadasEpisodios.Any())
                errores.Add("Debe tener al menos una temporada");

            if (serie.TemporadasEpisodios != null)
            {
                foreach (var temp in serie.TemporadasEpisodios)
                {
                    if (temp.temporada <= 0)
                        errores.Add($"La temporada {temp.temporada} no puede ser 0 o negativa");
                    if (temp.Episodios <= 0)
                        errores.Add($"La temporada {temp.temporada} debe tener al menos un episodio");
                }
            }

            return errores;
        }
    }
}