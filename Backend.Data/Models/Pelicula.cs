using System;

namespace Backend.Data.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Director { get; set; }
        public required string Genero { get; set; }
        public int DuracionMinutos { get; set; }

        public int PrecioRecaudacion { get; set; }

        public DateOnly FechaEstreno { get; set; }
        public bool Activa { get; set; }

        public string Duracion =>
            $"{DuracionMinutos} minutos.";

        //Propiedad Calculada

        public int Antiguedad => DateTime.Now.Year - FechaEstreno.Year;
       
    }
}