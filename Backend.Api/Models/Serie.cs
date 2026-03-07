using System;
using System.Collections.Generic;
using System.Linq;

namespace PasantiaTI1.Models
{
    
    public class Serie
        {
            public int Id { get; set; }
            public required string Titulo { get; set; }
            public string Plataforma { get; set; } = string.Empty;
            public int AnioEstreno { get; set; }
            public string Genero { get; set; } = string.Empty;
            public bool Activa { get; set; }

            public List<Temporada> TemporadasEpisodios { get; set; } = new();

            // Número total de temporadas
            public int Temporadas => TemporadasEpisodios.Count;

            // Número total de episodios de todas la temporadas
            public int Episodios => TemporadasEpisodios.Sum(te => te.Episodios);

            // Promedio de episodios por temporada
            public double PromedioEpisodiosPorTemporada =>
                Temporadas == 0 ? 0 : (double)Episodios / Temporadas;

            // Antigüedad de la serie en años
            public int Antiguedad => DateTime.Now.Year - AnioEstreno;
        }
}