using Backend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Linq;

namespace PasantiaTI1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ILogger<SeriesController> _logger;
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

        public SeriesController(ILogger<SeriesController> logger)
        {
            _logger = logger;
        }

        

        // GET /series
        // Devuelve todas las series o filtra por estado Activa si se pasa el query ?activa=true/false
        [HttpGet]
        public IActionResult Get([FromQuery] bool activa)
        {
            _logger.LogInformation("Obteniendo series con filtro Activa={Activa}", activa);
            try
            {
                IEnumerable<Serie> resultado = series.Where(s => s.Activa == activa).ToList();

                return Ok(resultado);
            }
            catch (Exception ex)

            {
                //Loguear el error con detalles para diagnostico
                _logger.LogError(ex, "Error al obtener las series");
                //Devolver un mensaje generico al cliente para no exponer detalles internos
                return StatusCode(400, "Ocurrio un error al procesar la solicitud");

            }
            
        }

        // GET /series/{id}
        [HttpGet("Buscar/{id}")]
        public IActionResult GetById(int id)
        {
            var serie = series.FirstOrDefault(s => s.Id == id);

            if (serie is null)
            {
                _logger.LogWarning("Serie con ID {Id} no encontrada", id);
                return NotFound(); 
            }
               

            return Ok(serie);
        }

        // POST /series
        [HttpPost("Agregar")]
        public IActionResult Post(Serie nuevaSerie)
        {
            var errores = ValidarSerie(nuevaSerie);

            if (errores.Any())
            {
                _logger.LogWarning($"Intento de agregar serie con datos invalidos: {nuevaSerie.Titulo}, se ha encontrado los siguientes errores:" +
                    $"{string.Join(",", errores )}");

                /*foreach (var error in errores)
                    _logger.LogWarning("Validación fallida: {Error}", error);
                */

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
            _logger.LogInformation("Intentando actualizar serie con ID {Id}", id);

            var serie = series.FirstOrDefault(s => s.Id == id);
            if (serie == null)
                return NotFound();

            var errores = ValidarSerie(serieActualizada);
            if (errores.Any())
            {
                //esta manera de registrar tambien esta bien 
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

            _logger.LogInformation("Serie {Id} actualizada exitosamente", id);

            return Ok(serie);
        }

        
        // Cambia el estado activo/inactivo de una serie
        [HttpPatch("Activar{id}")]
        public IActionResult CambiarEstado(int id, [FromQuery] bool activa)
        {
            _logger.LogInformation("Intentando cambiar estado de serie con ID {ID} a Activa={Activa}", id, activa);
            var serie = series.FirstOrDefault(s => s.Id == id);
            if (serie == null)
            {
                _logger.LogWarning("Serie con ID {Id} no encontrada para cambiar estado", id);
                return NotFound();
            }

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
            {
                _logger.LogWarning("Serie con ID {Id} no encontrada para eliminar", id);
                return NotFound();
            }

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