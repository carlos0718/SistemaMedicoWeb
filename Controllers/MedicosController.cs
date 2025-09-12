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
    public class MedicosController : ControllerBase
    {
        private readonly WebAppSistemaMedicoContext _context;

        public MedicosController(WebAppSistemaMedicoContext context)
        {
            _context = context;
        }

        // GET: api/Medicos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medico>>> GetMedico()
        {
            return await _context.Medico.ToListAsync();
        }

        // GET: api/Medicos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Medico>> GetMedico(int id)
        {
            var medico = await _context.Medico.FindAsync(id);

            if (medico == null)
            {
                return NotFound();
            }

            return medico;
        }

        // PUT: api/Medicos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedico(int id, Medico medico)
        {
            if (id != medico.Id)
            {
                return BadRequest();
            }

            _context.Entry(medico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicoExists(id))
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

        // POST: api/Medicos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Medico>> PostMedico(Medico medico)
        {
            _context.Medico.Add(medico);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedico", new { id = medico.Id }, medico);
        }

        // DELETE: api/Medicos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedico(int id)
        {
            var medico = await _context.Medico.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }

            _context.Medico.Remove(medico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicoExists(int id)
        {
            return _context.Medico.Any(e => e.Id == id);
        }
    }
}
