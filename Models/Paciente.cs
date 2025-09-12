
using System;

namespace WebAppSistemaMedico.Models
{
    public class Paciente
    {
      
            public int Id { get; set; }
            public int PersonaId { get; set; }
            public bool? Activo { get; set; }
            public int ObraSocialId { get; set; }
            public DateTime? FechaCreacion { get; set; }
            public DateTime? FechaModificacion { get; set; }
            virtual public Persona? Persona { get; set; }
            virtual public ObraSocial? ObraSocial { get; set; }


    }
};
