using System;

namespace PasantiaTI.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Director { get; set; }
        public string Genero { get; set; } = string.Empty;
        public int DuracionMinutos { get; set; }
        public required decimal PrecioRecaudacion { get; set; }

        public DateOnly FechaEstreno { get; set; }
        public bool Activa { get; set; }

        public string Duracion =>
            $"{DuracionMinutos} minutos.";

        public int Antiguedad =>
            DateTime.Now.Year - FechaEstreno.Year;
       
    }
}