namespace PasantiaTI1.Models
{
    //DTO: Solo datos que queremos enviar al cliente
    public class PeliculaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Director { get; set; }
        public string Genero { get; set; }
        public int DuracionMinutos { get; set; }
        public string PrecioRecaudacion { get; set; }
        public DateOnly FechaEstreno { get; set; }
        public bool Activa { get; set; }
    }
}
