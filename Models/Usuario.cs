
using System;

namespace WebAppSistemaMedico.Models
{
    public class Usuario
    {

        public int UsuarioId { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Contrasena { get; set; }
        //public string? Email { get; set; }
        //public int PersonaId { get; set; }
        public int TipoUsuarioId { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        virtual public TipoUsuario? TipoUsuario { get; set; }
        //virtual public Persona? Persona { get; set; }
    }
}
