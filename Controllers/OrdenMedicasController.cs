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
    public class OrdenMedicasController : ControllerBase
    {
       

        public OrdenMedicasController(WebAppSistemaMedicoContext context)
        {
           
        }

        // GET: api/OrdenMedicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdenMedica>>> GetOrdenMedica()
        {
            
        }

        // GET: api/OrdenMedicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrdenMedica>> GetOrdenMedica(int id)
        {
            
        }

        // PUT: api/OrdenMedicas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrdenMedica(int id, OrdenMedica ordenMedica)
        {
            
        }

        // POST: api/OrdenMedicas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrdenMedica>> PostOrdenMedica(OrdenMedica ordenMedica)
        {
            
        }

        // DELETE: api/OrdenMedicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrdenMedica(int id)
        {
           
        }

        private bool OrdenMedicaExists(int id)
        {
           
        }
    }
}
