using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppSistemaMedico.Data;
using WebAppSistemaMedico.Models;

namespace WebAppSistemaMedico.Controllers
{
    public class LineaOrdenMedicasController : Controller
    {
        private readonly WebAppSistemaMedicoContext _context;

        public LineaOrdenMedicasController(WebAppSistemaMedicoContext context)
        {
            _context = context;
        }

        // GET: LineaOrdenMedicas
        public async Task<IActionResult> Index()
        {
            var webAppSistemaMedicoContext = _context.LineaOrdenMedica.Include(l => l.OrdenMedica);
            return View(await webAppSistemaMedicoContext.ToListAsync());
        }

        // GET: LineaOrdenMedicas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineaOrdenMedica = await _context.LineaOrdenMedica
                .Include(l => l.OrdenMedica)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lineaOrdenMedica == null)
            {
                return NotFound();
            }

            return View(lineaOrdenMedica);
        }

        // GET: LineaOrdenMedicas/Create
        public IActionResult Create()
        {
            ViewData["OrdenMedicaId"] = new SelectList(_context.Set<OrdenMedica>(), "Id", "Id");
            return View();
        }

        // POST: LineaOrdenMedicas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrdenMedicaId,Nombre,Cantidad,Dosis,FrecuenciaHoras,Observacion,UnicaAplicacion,TratamientoEmpezado,Duracion")] LineaOrdenMedica lineaOrdenMedica)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lineaOrdenMedica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrdenMedicaId"] = new SelectList(_context.Set<OrdenMedica>(), "Id", "Id", lineaOrdenMedica.OrdenMedicaId);
            return View(lineaOrdenMedica);
        }

        // GET: LineaOrdenMedicas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineaOrdenMedica = await _context.LineaOrdenMedica.FindAsync(id);
            if (lineaOrdenMedica == null)
            {
                return NotFound();
            }
            ViewData["OrdenMedicaId"] = new SelectList(_context.Set<OrdenMedica>(), "Id", "Id", lineaOrdenMedica.OrdenMedicaId);
            return View(lineaOrdenMedica);
        }

        // POST: LineaOrdenMedicas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrdenMedicaId,Nombre,Cantidad,Dosis,FrecuenciaHoras,Observacion,UnicaAplicacion,TratamientoEmpezado,Duracion")] LineaOrdenMedica lineaOrdenMedica)
        {
            if (id != lineaOrdenMedica.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lineaOrdenMedica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LineaOrdenMedicaExists(lineaOrdenMedica.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrdenMedicaId"] = new SelectList(_context.Set<OrdenMedica>(), "Id", "Id", lineaOrdenMedica.OrdenMedicaId);
            return View(lineaOrdenMedica);
        }

        // GET: LineaOrdenMedicas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineaOrdenMedica = await _context.LineaOrdenMedica
                .Include(l => l.OrdenMedica)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lineaOrdenMedica == null)
            {
                return NotFound();
            }

            return View(lineaOrdenMedica);
        }

        // POST: LineaOrdenMedicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lineaOrdenMedica = await _context.LineaOrdenMedica.FindAsync(id);
            if (lineaOrdenMedica != null)
            {
                _context.LineaOrdenMedica.Remove(lineaOrdenMedica);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LineaOrdenMedicaExists(int id)
        {
            return _context.LineaOrdenMedica.Any(e => e.Id == id);
        }
    }
}
