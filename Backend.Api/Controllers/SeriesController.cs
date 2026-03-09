using Backend.Data.Models;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;
using ROP.APIExtensions;

namespace Aplicacion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ILogger<SeriesController> _log;
        private readonly SeriesService _seriesService;

        public SeriesController(ILogger<SeriesController> log, SeriesService seriesService)
        {
            _log = log;
            _seriesService = seriesService;
        }

        [HttpGet("VerTodos")]
        public IActionResult GetTodos()
        {
            _log.LogInformation("Obteniendo todas las series");
            return _seriesService.GetSeries().ToActionResult();
        }

        [HttpPost("Agregar")]
        public IActionResult Post([FromBody] Serie nuevaSerie)
        {
            _log.LogInformation("Agregando una nueva serie");
            return _seriesService.AddSerie(nuevaSerie).ToActionResult();
        }

        [HttpPut("Actualizar/{id}")]
        public IActionResult Update(string id, [FromBody] Serie serieActualizada)
        {
            _log.LogInformation("Actualizando la serie con ID: {Id}", id);
            return _seriesService.UpdateSerie(id, serieActualizada).ToActionResult();
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Delete(string id)
        {
            _log.LogInformation("Eliminando la serie con ID: {Id}", id);
            return _seriesService.DeleteSerie(id).ToActionResult();
        }
    }
}