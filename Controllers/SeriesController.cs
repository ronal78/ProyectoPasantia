using Microsoft.AspNetCore.Mvc;
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
        // Lista estática que simula una "base de datos" en memoria
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
                //Lista de temporadas y episodios
                TemporadasEpisodios = new List<Temporada>
                {
                    new Temporada { Numero = 1, Episodios = 18 },
                    new Temporada { Numero = 2, Episodios = 18 },
                    new Temporada { Numero = 3, Episodios = 20 },
                    new Temporada { Numero = 4, Episodios = 20 },
                    new Temporada { Numero = 5, Episodios = 18 },
                    new Temporada { Numero = 6, Episodios = 18 },
                    new Temporada { Numero = 7, Episodios = 10 },
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
                    new Temporada { Numero = 1, Episodios = 6 },
                    new Temporada { Numero = 2, Episodios = 6 },
                }
            }
        };

        // GET /series
        // Devuelve todas las series
        [HttpGet]
        public IActionResult Get() => Ok(series);

        // GET /series/{id}
        // Devuelve una serie específica por su Id
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // Buscar la serie por Id
            var serie = series.FirstOrDefault(s => s.Id == id);
            if (serie == null)
                return NotFound(); // Si no se encuentra, devuelve 404

            return Ok(serie); // Devuelve la serie encontrada
        }

        // POST /series
        // Agrega una nueva serie a la lista
        [HttpPost]
        public IActionResult Post(Serie nuevaSerie)
        {
            series.Add(nuevaSerie); // Añadir la serie a la lista
            // Devuelve 201 Created con la serie agregada
            return CreatedAtAction(nameof(GetById), new { id = nuevaSerie.Id }, nuevaSerie);
        }

        // PUT /series/{id}
        // Actualiza una serie existente
        [HttpPut("{id}")]
        public IActionResult Put(int id, Serie serieActualizada)
        {
            // Buscar la serie por Id
            var serie = series.FirstOrDefault(s => s.Id == id);
            if (serie == null)
                return NotFound(); // 404 si no existe

            // Actualiza los campos básicos
            serie.Titulo = serieActualizada.Titulo;
            serie.Plataforma = serieActualizada.Plataforma;
            serie.AnioEstreno = serieActualizada.AnioEstreno;
            serie.Genero = serieActualizada.Genero;
            serie.Activa = serieActualizada.Activa;

            // Actualiza la lista de temporadas y episodios
            serie.TemporadasEpisodios = serieActualizada.TemporadasEpisodios;

            return Ok(serie); // Devuelve la serie actualizada
        }

        // DELETE /series/{id}
        // Elimina una serie por Id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Buscar la serie por Id
            var serie = series.FirstOrDefault(s => s.Id == id);
            if (serie == null)
                return NotFound(); // 404 si no existe

            // Eliminar la serie de la lista
            series.Remove(serie);

            return NoContent(); // Devuelve 204 No Content
        }
    }
}