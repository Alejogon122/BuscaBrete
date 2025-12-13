using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuscaBrete.Data;
using BuscaBrete.Models;

namespace BuscaBrete.Controllers
{
    public class PostulacionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostulacionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Postulacions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Postulacion.ToListAsync());
        }

        // GET: Postulacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postulacion = await _context.Postulacion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postulacion == null)
            {
                return NotFound();
            }

            return View(postulacion);
        }

        // GET: Postulacions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Postulacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaPostulacion,Estado,PostulanteId,OfertaId")] Postulacion postulacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postulacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(postulacion);
        }

        // GET: Postulacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postulacion = await _context.Postulacion.FindAsync(id);
            if (postulacion == null)
            {
                return NotFound();
            }
            return View(postulacion);
        }

        // POST: Postulacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaPostulacion,Estado,PostulanteId,OfertaId")] Postulacion postulacion)
        {
            if (id != postulacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postulacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostulacionExists(postulacion.Id))
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
            return View(postulacion);
        }

        // GET: Postulacions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postulacion = await _context.Postulacion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postulacion == null)
            {
                return NotFound();
            }

            return View(postulacion);
        }

        // POST: Postulacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var postulacion = await _context.Postulacion.FindAsync(id);
            if (postulacion != null)
            {
                _context.Postulacion.Remove(postulacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostulacionExists(int id)
        {
            return _context.Postulacion.Any(e => e.Id == id);
        }
    }
}
