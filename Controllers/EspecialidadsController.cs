using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppSistemaMedico.Data;
using WebAppSistemaMedico.Models;

namespace WebAppSistemaMedico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadesController : ControllerBase
    {
        //1.- Inyectar el contexto de la base de datos como una variable privada
        private readonly WebAppSistemaMedicoContext _context;

        public EspecialidadesController(WebAppSistemaMedicoContext context)
        {
            //2.- Asignar el contexto a una variable privada
            _context = context;
        }
            
        //3.- Implementar los métodos CRUD (Create, Read, Update, Delete) para la entidad Especialidad
        // GET: api/Especialidads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Especialidad>>> GetEspecialidad()
        {
           return await _context.Especialidad.ToListAsync();
        }

        // GET: api/Especialidads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Especialidad>> GetEspecialidad(int id)
        {
           
        }

        // PUT: api/Especialidads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEspecialidad(int id, Especialidad especialidad)
        {
            
        }

        // POST: api/Especialidads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Especialidad>> PostEspecialidad(Especialidad especialidad)
        {
            
        }

        // DELETE: api/Especialidads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEspecialidad(int id)
        {
            
        }

        private bool EspecialidadExists(int id)
        {
            
        }
    }
}
