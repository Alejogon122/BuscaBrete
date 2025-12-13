using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuscaBrete.Data;
using BuscaBrete.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace BuscaBrete.Controllers
{
    [Authorize]
    public class PostulacionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public PostulacionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // GET: Postulacions
        public async Task<IActionResult> Index(int? ofertaId)
        {
            // total in DB for debugging
            var total = await _context.Postulacion.CountAsync();

            var query = _context.Postulacion
                .Include(p => p.Oferta)
                .Include(p => p.Postulante)
                .AsQueryable();

            if (ofertaId.HasValue)
            {
                query = query.Where(p => p.OfertaId == ofertaId.Value);
            }

            // Si el usuario es empresa, mostrar solo postulaciones para sus ofertas
            if (User.IsInRole("Empresa"))
            {
                var userId = _userManager.GetUserId(User);
                query = query.Where(p => p.Oferta.EmpresaId == userId);
            }

            // Si el usuario es postulante, mostrar solo sus postulaciones
            if (User.IsInRole("Postulante"))
            {
                var userId = _userManager.GetUserId(User);
                query = query.Where(p => p.PostulanteId == userId);
            }

            var list = await query.ToListAsync();

            ViewBag.TotalPostulaciones = total;
            ViewBag.VisiblePostulaciones = list.Count;

            return View(list);
        }

        // POST: Postulacions/Open
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Open(int? selectedId)
        {
            if (!selectedId.HasValue)
            {
                // No selection made, return to list with message (TempData)
                TempData["Error"] = "Debe seleccionar una postulación.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Details), new { id = selectedId.Value });
        }

        // GET: Postulacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postulacion = await _context.Postulacion
                .Include(p => p.Oferta)
                .Include(p => p.Postulante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postulacion == null)
            {
                return NotFound();
            }

            return View(postulacion);
        }

        // GET: Postulacions/Create
        [Authorize(Roles = "Postulante")]
        public IActionResult Create(int? ofertaId)
        {
            ViewBag.OfertaId = ofertaId ?? 0;
            var model = new Postulacion();
            if (ofertaId.HasValue)
            {
                model.OfertaId = ofertaId.Value;
            }

            // Suministrar listado de ofertas para selección cuando no viene preseleccionada
            if (!ofertaId.HasValue || model.OfertaId == 0)
            {
                ViewBag.Ofertas = new SelectList(_context.Oferta.Include(o => o.Empresa).OrderBy(o => o.Titulo).Select(o => new {
                    o.Id,
                    Texto = o.Titulo
                }), "Id", "Texto");
            }

            return View(model);
        }

        // POST: Postulacions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Postulante")]
        public async Task<IActionResult> Create([Bind("Mensaje,OfertaId")] Postulacion postulacion)
        {
            // Build entity server-side to avoid client-side binding validation on required keys
            var userId = _userManager.GetUserId(User);
            var newPost = new Postulacion
            {
                Mensaje = postulacion.Mensaje,
                OfertaId = postulacion.OfertaId,
                PostulanteId = userId,
                FechaPostulacion = DateTime.UtcNow,
                Estado = "Pendiente"
            };

            // Validate OfertaId
            if (newPost.OfertaId == 0)
            {
                TempData["Error"] = "Oferta inválida.";
                // Reponer lista de ofertas para el formulario
                ViewBag.Ofertas = new SelectList(_context.Oferta.OrderBy(o => o.Titulo), "Id", "Titulo");
                return View(newPost);
            }

            // Ensure Oferta exists
            var oferta = await _context.Oferta.FindAsync(newPost.OfertaId);
            if (oferta == null)
            {
                TempData["Error"] = "La oferta seleccionada no existe.";
                ViewBag.Ofertas = new SelectList(_context.Oferta.OrderBy(o => o.Titulo), "Id", "Titulo");
                return View(newPost);
            }

            // Prevent duplicate application for same oferta by same postulante
            var exists = await _context.Postulacion.AnyAsync(p => p.OfertaId == newPost.OfertaId && p.PostulanteId == newPost.PostulanteId);
            if (exists)
            {
                TempData["Error"] = "Ya te has postulado a esta oferta.";
                return RedirectToAction(nameof(Index), new { ofertaId = newPost.OfertaId });
            }

            // Save
            try
            {
                _context.Postulacion.Add(newPost);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Postulación creada correctamente.";
                return RedirectToAction(nameof(Index), new { ofertaId = newPost.OfertaId });
            }
            catch (Exception)
            {
                TempData["Error"] = "Error al guardar la postulación.";
                // Reponer lista por si se vuelve al formulario
                ViewBag.Ofertas = new SelectList(_context.Oferta.OrderBy(o => o.Titulo), "Id", "Titulo");
                return View(newPost);
            }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaPostulacion,Estado,PostulanteId,OfertaId,Mensaje")] Postulacion postulacion)
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
                .Include(p => p.Oferta)
                .Include(p => p.Postulante)
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
