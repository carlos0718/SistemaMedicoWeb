using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAppSistemaMedico.Models;

namespace WebAppSistemaMedico.Data
{
    public class WebAppSistemaMedicoContext : DbContext
    {
        public WebAppSistemaMedicoContext (DbContextOptions<WebAppSistemaMedicoContext> options)
            : base(options)
        {
        }

        public DbSet<WebAppSistemaMedico.Models.Persona> Persona { get; set; } = default!;
        public DbSet<WebAppSistemaMedico.Models.Especialidad> Especialidad { get; set; } = default!;
        public DbSet<WebAppSistemaMedico.Models.LineaOrdenMedica> LineaOrdenMedica { get; set; } = default!;
        public DbSet<WebAppSistemaMedico.Models.Medico> Medico { get; set; } = default!;
        public DbSet<WebAppSistemaMedico.Models.ObraSocial> ObraSocial { get; set; } = default!;
        public DbSet<WebAppSistemaMedico.Models.OrdenMedica> OrdenMedica { get; set; } = default!;
        public DbSet<WebAppSistemaMedico.Models.Paciente> Paciente { get; set; } = default!;
        public DbSet<WebAppSistemaMedico.Models.TipoUsuario> TipoUsuario { get; set; } = default!;
        public DbSet<WebAppSistemaMedico.Models.Usuario> Usuario { get; set; } = default!;
    }
}
