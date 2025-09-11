

namespace WebAppSistemaMedico.Models
{
    public class LineaOrdenMedica 
    {
        public int Id { get; set; }
        public int OrdenmedicaId { get; set; }
        public string? medicamento { get; set; }
        public string? Dosis { get; set; }
        public string? Frecuencia { get; set; }
        public string? Duracion { get; set; }
        public string? Instrucciones { get; set; }
    }
}
