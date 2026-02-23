using Microsoft.AspNetCore.Mvc;

namespace PasantiaTI.Controllers
{ 
    // CONTROLADOR DE PELICULAS

    [Route("[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private static List<Pelicula> peliculas = new List<Pelicula>
        {
            new Pelicula { 
                Id = 1, 
                Titulo = "Avengers Infinity War", 
                Director = "Ahthony and Joe Russo", 
                Año = 2017,
                Genero = "Ciencia Ficcion", 
                Precio = 150, 
                Activa = false },

            new Pelicula { 
                Id = 2, 
                Titulo = "Rapidos y Furiosos 5: Sin control", 
                Director = "Justin Lin", 
                Año = 2011, 
                Genero = "Accion", 
                Precio = 100, 
                Activa = true }
        };

        // GET: /Peliculas
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(peliculas);
        }

        // GET: /Peliculas/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var pelicula = peliculas.FirstOrDefault(p => p.Id == id);

            if (pelicula == null)
                return NotFound();

            return Ok(pelicula);
        }

        // POST: /Peliculas
        [HttpPost]
        public IActionResult Post(Pelicula pelicula)
        {
            peliculas.Add(pelicula);
            return CreatedAtAction(nameof(GetById), new { id = pelicula.Id }, pelicula);
        }

        // Actualizar una pelicula PUT: /Peliculas/1
        [HttpPut("{id}")] //Indica que este método responde a una petición HTTP PUT "{id}" Significa  que el id viene  en la URl
        public IActionResult Put(int id, Pelicula peliculaActualizada)
        {
            var pelicula = peliculas.FirstOrDefault(p => p.Id == id); //Busca la primera pelicula cuyo ID coincida

            if (pelicula == null)
                return NotFound(); //En esta, si no existe devuelve el error 404

            pelicula.Titulo = peliculaActualizada.Titulo; //Si existe actualiza propiedades, copia los valores nuevos sobre la pelicula existente
            pelicula.Director = peliculaActualizada.Director;
            pelicula.Año = peliculaActualizada.Año;
            pelicula.Genero = peliculaActualizada.Genero;
            pelicula.Precio = peliculaActualizada.Precio;
            pelicula.Activa = peliculaActualizada.Activa;

            return Ok(pelicula); //Devuelve la pelicula actualizada, devuelve codigo 200 con la pelicula actualizada
        }

        // Eliminar una pelicula DELETE: /Peliculas/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pelicula = peliculas.FirstOrDefault(p => p.Id == id); //Busca la pelicula 

            if (pelicula == null)
                return NotFound(); //Si no existe

            peliculas.Remove(pelicula); //Si existe

            return Ok(peliculas);
        }
    }

    // CONTROLADOR DE SERIES

    [Route("[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private static List<Serie> series = new List<Serie>
        {
            new Serie { 
                Id = 1, 
                Titulo = "Breaking Bad", 
                Temporadas = 5, 
                Episodios = 62, 
                Plataforma = "Netflix", 
                Activa = true },
            new Serie { 
                Id = 2, 
                Titulo = "The Walking Dead", 
                Temporadas = 4, 
                Episodios = 3, 
                Plataforma = "HBO", 
                Activa = true }
        };

        // GET: /Series
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(series);
        }

        // GET: /Series/1
        [HttpGet("{id}")] //Este método  responde una petición
        public IActionResult GetById(int id)
        {
            var serie = series.FirstOrDefault(s => s.Id == id); //Busca la serie por ID, la primera serie cuyo id coincida.

            if (serie == null)
                return NotFound(); //Si no existe, devuelve 404 Not found

            return Ok(serie); //Si existe, devuelve un OK con la serie encontrada
        }

        // Crea una serie POST: /Series
        [HttpPost]
        public IActionResult Post(Serie serie)
        {
            series.Add(serie); //Agrega la serie a la lista 
            return CreatedAtAction(nameof(GetById), new { id = serie.Id }, serie); //Devuelve  createdAtAction, Indica donde se puede consultar el recurso creado,
            //Llama al metodo  GetById y envia el id recién creado, "La serie fue creada y puedes verla en /Series/{id}"
        }

        // Actualizar PUT: /Series/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, Serie serieActualizada) 
        {
            var serie = series.FirstOrDefault(s => s.Id == id); //Buscar la serie 

            if (serie == null) 
                return NotFound(); //Si no existe

            serie.Titulo = serieActualizada.Titulo; //Si existe actualiza propiedades, copia los nuevos valores sobre el objeto existente
            serie.Temporadas = serieActualizada.Temporadas;
            serie.Episodios = serieActualizada.Episodios;
            serie.Plataforma = serieActualizada.Plataforma;
            serie.Activa = serieActualizada.Activa;

            return Ok(serie); //Devuelve Ok con la serie actualizada 
        }

        // Eliminar DELETE: /Series/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var serie = series.FirstOrDefault(s => s.Id == id);

            if (serie == null)
                return NotFound();

            series.Remove(serie);

            return NoContent(); //En Apis Rest profesionales normalmente se devuelve return NoContent(); porque DELETE suele responder con código 204 sin contendido
        }
    }

    
    // MODELO PELICULA
    public class Pelicula
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Director { get; set; }
        public int Año { get; set; }
        public string Genero { get; set; }
        public decimal Precio { get; set; }
        public bool Activa { get; set; }

        // Propiedad calculada
        public int Antiguedad => DateTime.Now.Year - Año;
    }

    // MODELO SERIE
    public class Serie
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public int Temporadas { get; set; }
        public int Episodios { get; set; }
        public string Plataforma { get; set; }
        public bool Activa { get; set; }

        // Propiedad calculada
        public double PromedioEpisodiosPorTemporada =>
            Temporadas == 0 ? 0 : (double)Episodios / Temporadas;   // Se calcula el promedio de episodios por temporada
    }
}