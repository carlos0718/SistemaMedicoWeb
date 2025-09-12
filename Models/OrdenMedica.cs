using System;

namespace WebAppSistemaMedico.Models
{
    public class OrdenMedica
    {
        
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public int ObraSocialId { get; set; }
        public string? Diagnostico { get; set; }
        public string? Observaciones { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        virtual public Paciente? Paciente { get; set; }
        virtual public Medico? Medico { get; set; } 
        virtual public ObraSocial? ObraSocial { get; set; } 
    }
};

