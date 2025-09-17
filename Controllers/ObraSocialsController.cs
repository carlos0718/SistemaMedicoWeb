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
        

        public ObraSocialsController(WebAppSistemaMedicoContext context)
        {
            
        }

        // GET: api/ObraSocials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObraSocial>>> GetObraSocial()
        {
            
        }

        // GET: api/ObraSocials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ObraSocial>> GetObraSocial(int id)
        {
            
        }

        // PUT: api/ObraSocials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutObraSocial(int id, ObraSocial obraSocial)
        {
            
        }

        // POST: api/ObraSocials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ObraSocial>> PostObraSocial(ObraSocial obraSocial)
        {
            
        }

        // DELETE: api/ObraSocials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObraSocial(int id)
        {
            
        }

        private bool ObraSocialExists(int id)
        {
            
        }
    }
}
