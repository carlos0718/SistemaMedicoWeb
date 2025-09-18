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
        private readonly WebAppSistemaMedicoContext _context;

        public OrdenMedicasController(WebAppSistemaMedicoContext context)
        {
            _context = context;
        }

        // GET: api/OrdenMedicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdenMedica>>> GetOrdenMedica()
        {
            return await _context.OrdenMedica.ToListAsync();
        }

        // GET: api/OrdenMedicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrdenMedica>> GetOrdenMedica(int id)
        {
            var OrdenMedica = await _context.OrdenMedica.FindAsync(id);

            if (OrdenMedica == null)
            {
                return NotFound();
            }

            return OrdenMedica;
        }

        // PUT: api/OrdenMedicas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrdenMedica(int id, OrdenMedica ordenMedica)
        {
            if (id != ordenMedica.Id)
            {
                return BadRequest();
            }

            _context.Entry(ordenMedica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdenMedicaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    }

        // POST: api/OrdenMedicas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrdenMedica>> PostOrdenMedica(OrdenMedica ordenMedica)
        {
        _context.OrdenMedica.Add(ordenMedica);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetOrdenMedica", new { id = ordenMedica.Id }, ordenMedica);
    }

        // DELETE: api/OrdenMedicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrdenMedica(int id)
        {
        var ordenMedica = await _context.OrdenMedica.FindAsync(id);
        if (ordenMedica == null)
        {
            return NotFound();
        }

        _context.Medico.Remove(ordenMedica);
        await _context.SaveChangesAsync();

        return NoContent();
    }

        private bool OrdenMedicaExists(int id)
        {
        return _context.OrdenMedica.Any(e => e.Id == id);
    }
    }
}
