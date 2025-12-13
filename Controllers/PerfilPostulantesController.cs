using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor inyecta DbContext, IWebHostEnvironment (para archivos) y UserManager (Identity)
        public PerfilPostulantesController(
            ApplicationDbContext context,
            IWebHostEnvironment hostEnvironment,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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

        // POST: Create (recibe IFormFile para el CV)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PerfilPostulante perfil, Microsoft.AspNetCore.Http.IFormFile CVFile)
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
            if (!ModelState.IsValid) return View(perfil);

            perfil.PostulanteId = userId;

            // Manejo del archivo
            if (CVFile != null && CVFile.Length > 0)
            {
                var ext = Path.GetExtension(CVFile.FileName).ToLowerInvariant();
                if (ext != ".pdf")
                {
                    ModelState.AddModelError("CVFile", "Solo se permiten archivos PDF.");
                    return View(perfil);
                }

                var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var uniqueName = $"{Guid.NewGuid()}{ext}";
                var fullPath = Path.Combine(uploadsFolder, uniqueName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await CVFile.CopyToAsync(stream);
                }

                perfil.CVPath = "/uploads/" + uniqueName;
            }
            else
            {
                perfil.CVPath = perfil.CVPath ?? string.Empty;
            }

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
        public async Task<IActionResult> Edit(int id, PerfilPostulante model, Microsoft.AspNetCore.Http.IFormFile CVFile)
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

            // Si sube nuevo CV, reemplazar
            if (CVFile != null && CVFile.Length > 0)
            {
                var ext = Path.GetExtension(CVFile.FileName).ToLowerInvariant();
                if (ext != ".pdf")
                {
                    ModelState.AddModelError("CVFile", "Solo se permiten archivos PDF.");
                    return View(perfil);
                }

                var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                // eliminar anterior (opcional, si existe)
                try
                {
                    if (!string.IsNullOrEmpty(perfil.CVPath))
                    {
                        var old = Path.Combine(_hostEnvironment.WebRootPath, perfil.CVPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(old)) System.IO.File.Delete(old);
                    }
                }
                catch { /* no interrumpir por error al borrar */ }

                var uniqueName = $"{Guid.NewGuid()}{ext}";
                var fullPath = Path.Combine(uploadsFolder, uniqueName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await CVFile.CopyToAsync(stream);
                }
                perfil.CVPath = "/uploads/" + uniqueName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Descargar CV por Id de perfil (público si corresponde)
        public async Task<IActionResult> DescargarCV(int id)
        {
            var perfil = await _context.PerfilPostulante.FindAsync(id);
            if (perfil == null || string.IsNullOrEmpty(perfil.CVPath)) return NotFound();

            var physicalPath = Path.Combine(_hostEnvironment.WebRootPath, perfil.CVPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (!System.IO.File.Exists(physicalPath)) return NotFound();

            var bytes = await System.IO.File.ReadAllBytesAsync(physicalPath);
            return File(bytes, "application/pdf", Path.GetFileName(physicalPath));
        }
    }
}


