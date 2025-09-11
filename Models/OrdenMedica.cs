using System;

namespace WebAppSistemaMedico.Models
{
    public class OrdenMedica
    {
        
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public string? Diagnostico { get; set; }
        public string? Observaciones { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
}
