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
    public class ObraSocialsController : ControllerBase
    {
        private readonly WebAppSistemaMedicoContext _context;

        public ObraSocialsController(WebAppSistemaMedicoContext context)
        {
            _context = context;
        }

        // GET: api/ObraSocials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObraSocial>>> GetObraSocials()
        {
            return await _context.ObraSocial.ToListAsync();
        }

        // GET: api/ObraSocials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ObraSocial>> GetObraSocial(int id)
        {
            var ObraSocial = await _context.ObraSocial.FindAsync(id);

            if (ObraSocial == null)
            {
                return NotFound();
            }

            return ObraSocial;

        }

        // PUT: api/ObraSocials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutObraSocials(int id, ObraSocials obraSocial)
        {
            if (id != obraSocial.Id)
            {
                return BadRequest();
            }

            _context.Entry(ObraSocials).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObraSocialsExists(id))
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

        // POST: api/ObraSocials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ObraSocial>> PostObraSocial(ObraSocial obraSocial)
        {
        _context.ObraSocial.Add(obraSocial);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetObraSocial", new { id = obraSocial.Id }, obraSocial);
    }

        // DELETE: api/ObraSocials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObraSocial(int id)
        {
        var obraSocial = await _context.Obrasocial.FindAsync(id);
        if (ObraSocial == null)
        {
            return NotFound();
        }
    }
    _context.ObraSocial.Remove(ObraSocial);
            await _context.SaveChangesAsync();

            return NoContent();
}
private bool ObraSocialExists(int id)
{
    return _context.ObraSocial.Any(e => e.Id == id);
}
    }
}
