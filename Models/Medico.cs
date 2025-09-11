
using System;

namespace WebAppSistemaMedico.Models
{
    public class Medico
    {
        
            public int Id { get; set; }
            public int PersonaId { get; set; }
            public int EspecialidadId { get; set; }
            public bool? Activo { get; set; }
            public DateTime? FechaCreacion { get; set; }
            public DateTime? FechaModificacion { get; set; }
            public string? Matricula { get; set; }
            public string? Telefono { get; set; }
            public string? Email { get; set; }
        
    }
}
