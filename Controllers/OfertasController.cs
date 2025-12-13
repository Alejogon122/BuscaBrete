using BuscaBrete.Data;
using BuscaBrete.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuscaBrete.Controllers
{
    [Authorize(Roles = "Administrador, Empresa")]
    public class OfertasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OfertasController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Ofertas
        public async Task<IActionResult> Index(
            string searchString,
            string ubicacion,
            decimal? salarioMin)
        {
            var ofertas = _context.Oferta.AsQueryable();

            // Filtro por texto (título o descripción)
            if (!string.IsNullOrEmpty(searchString))
            {
                ofertas = ofertas.Where(o =>
                    o.Titulo.Contains(searchString) ||
                    o.Descripcion.Contains(searchString));
            }

            // Filtro por ubicación
            if (!string.IsNullOrEmpty(ubicacion))
            {
                ofertas = ofertas.Where(o =>
                    o.Ubicacion.Contains(ubicacion));
            }

            // Filtro por salario mínimo
            if (salarioMin.HasValue)
            {
                ofertas = ofertas.Where(o => o.Salario >= salarioMin);
            }
            return View(await ofertas
            .Include(o => o.Empresa)
            .ToListAsync());
        }

        // GET: Ofertas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Oferta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oferta == null)
            {
                return NotFound();
            }

            return View(oferta);
        }

        // GET: Ofertas/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.EmpresaNombre = user?.NombreEmpresa ?? user?.UserName;
            ViewBag.EmpresaId = user?.Id;
            return View();
        }

        // POST: Ofertas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Oferta oferta)
        {
           
            oferta.EmpresaId = _userManager.GetUserId(User);
            oferta.FechaPublicacion = DateTime.Now;

            
            foreach (var key in ModelState.Keys.Where(k => k != null && k.StartsWith("Empresa")).ToList())
            {
                ModelState.Remove(key);
            }

            if (ModelState.IsValid)
            {
                _context.Add(oferta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

           
            var user = await _userManager.GetUserAsync(User);
            ViewBag.EmpresaNombre = user?.NombreEmpresa ?? user?.UserName;
            ViewBag.EmpresaId = user?.Id;

            return View(oferta);
        }

        // GET: Ofertas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Oferta.FindAsync(id);
            if (oferta == null)
            {
                return NotFound();
            }
            return View(oferta);
        }

        // POST: Ofertas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descripcion,Requisitos,FechaPublicacion,Ubicacion,Salario,EmpresaId")] Oferta oferta)
        {
            if (id != oferta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oferta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfertaExists(oferta.Id))
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
            return View(oferta);
        }

        // GET: Ofertas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Oferta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oferta == null)
            {
                return NotFound();
            }

            return View(oferta);
        }

        // POST: Ofertas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oferta = await _context.Oferta.FindAsync(id);
            if (oferta != null)
            {
                _context.Oferta.Remove(oferta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfertaExists(int id)
        {
            return _context.Oferta.Any(e => e.Id == id);
        }
    }
}
