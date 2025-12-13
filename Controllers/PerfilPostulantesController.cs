using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuscaBrete.Data;
using BuscaBrete.Models;

namespace BuscaBrete.Controllers
{
    [Authorize] // requiere usuario autenticado; puedes cambiar a Roles = "Postulante" si quieres restringir
    public class PerfilPostulantesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor inyecta DbContext, IWebHostEnvironment (para archivos) y UserManager (Identity)
        public PerfilPostulantesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /PerfilPostulantes  -> muestra el perfil del usuario logueado
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction("Login", "Account");

            var perfil = await _context.PerfilPostulante
                .Include(p => p.Postulante)
                .Include(p => p.Experiencias)
                .Include(p => p.Educaciones)
                .FirstOrDefaultAsync(p => p.PostulanteId == userId);

            if (perfil == null)
                return RedirectToAction(nameof(Create));

            return View(perfil);
        }

        // GET: Create – solo si no existe perfil
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);
            var yaExiste = await _context.PerfilPostulante.AnyAsync(p => p.PostulanteId == userId);
            if (yaExiste) return RedirectToAction(nameof(Index));
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PerfilPostulante perfil)
        {
            var userId = _userManager.GetUserId(User);

            // Evitar duplicados
            var existe = await _context.PerfilPostulante.AnyAsync(p => p.PostulanteId == userId);
            if (existe)
            {
                ModelState.AddModelError("", "Ya tienes un perfil creado.");
                return View(perfil);
            }
            //SI HAY ERRORES DE VALIDACIÓN, MOSTRARLOS EN UNA CAJA
            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .SelectMany(kvp => kvp.Value.Errors.Select(e =>
                        (string.IsNullOrEmpty(kvp.Key) ? "" : kvp.Key + ": ") +
                        (string.IsNullOrWhiteSpace(e.ErrorMessage)
                            ? e.Exception?.Message
                            : e.ErrorMessage)
                    ))
                    .ToList();

                ViewBag.Errores = string.Join(" | ", errores);

                return View(perfil);
            }

            perfil.PostulanteId = userId;

            _context.Add(perfil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Edit -> edita el perfil del usuario actual
        public async Task<IActionResult> Edit()
        {
            var userId = _userManager.GetUserId(User);
            var perfil = await _context.PerfilPostulante.FirstOrDefaultAsync(p => p.PostulanteId == userId);
            if (perfil == null) return RedirectToAction(nameof(Create));
            return View(perfil);
        }

        // POST: Edit (permite actualizar datos; no cambia PostulanteId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PerfilPostulante model)
        {
            var userId = _userManager.GetUserId(User);
            var perfil = await _context.PerfilPostulante.FirstOrDefaultAsync(p => p.Id == id && p.PostulanteId == userId);
            if (perfil == null) return Unauthorized();

            if (!ModelState.IsValid) return View(perfil);

            // Actualizar campos
            perfil.Direccion = model.Direccion;
            perfil.FechaNacimiento = model.FechaNacimiento;
            perfil.Resumen = model.Resumen;
            perfil.Telefono = model.Telefono;
            perfil.Habilidades = model.Habilidades;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}


