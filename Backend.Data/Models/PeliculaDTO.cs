namespace Backend.Data.Models
{
    //DTO: Solo datos que queremos enviar al cliente
    public class PeliculaDTO
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Director { get; set; }
        public required string Genero { get; set; }
        public int DuracionMinutos { get; set; }
        public required string PrecioRecaudacion { get; set; }
        public DateOnly FechaEstreno { get; set; }
        public bool Activa { get; set; }
    }
}
