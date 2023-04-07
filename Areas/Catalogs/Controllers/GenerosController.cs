using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ease_admin_cloud.Areas.Catalogs.Models;
using ease_admin_cloud.Data;
using Microsoft.AspNetCore.Identity;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ease_admin_cloud.Areas.Catalogs.Controllers
{
    [Area("Catalogs")]
    public class GenerosController : Controller
    {
        private readonly eacDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotyfService _toastNotification;

        public GenerosController(
            eacDbContext context,
            UserManager<IdentityUser> userManager,
            INotyfService toastNotification
        )
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }

        // GET: Catalogs/Generos
        public async Task<IActionResult> Index()
        {
            var val_estatus = _context.cat_estatus.ToList();

            if (val_estatus.Count == 2)
            {
                ViewBag.EstatusFlag = 1;
                var val_empresa = _context.tbl_empresas.ToList();

                if (val_empresa.Count == 1)
                {
                    ViewBag.EmpresaFlag = 1;
                    var val_corporativo = _context.tbl_corporativos.ToList();

                    if (val_corporativo.Count >= 1)
                    {
                        ViewBag.CorporativoFlag = 1;
                    }
                    else
                    {
                        ViewBag.CorporativoFlag = 0;
                        _toastNotification.Information(
                            "Favor de registrar los datos de Corporativo para la Aplicación",
                            5
                        );
                    }
                }
                else
                {
                    ViewBag.EmpresaFlag = 0;
                    _toastNotification.Information(
                        "Favor de registrar los datos de la Empresa para la Aplicación",
                        5
                    );
                }
            }
            else
            {
                ViewBag.EstatusFlag = 0;
                _toastNotification.Information(
                    "Favor de registrar los Estatus para la Aplicación",
                    5
                );
            }
            var f_genero =
                from a in _context.cat_generos
                join b in _context.tbl_usuarios_controles
                    on a.id_usuario_modifico equals b.id_usuario_control

                select new cat_genero
                {
                    id_genero = a.id_genero,
                    genero_desc = a.genero_desc,
                    usuario_modifico_desc = b.nombre_usuario,
                    fecha_registro = a.fecha_registro,
                    id_estatus_registro = a.id_estatus_registro
                };

            return View(await f_genero.ToListAsync());
        }

        // GET: Catalogs/Generos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.cat_generos == null)
            {
                return NotFound();
            }

            var cat_genero = await _context.cat_generos.FirstOrDefaultAsync(m => m.id_genero == id);
            if (cat_genero == null)
            {
                return NotFound();
            }

            return View(cat_genero);
        }

        // GET: Catalogs/Generos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catalogs/Generos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "id_genero,genero_desc,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro"
            )]
                cat_genero cat_genero
        )
        {
            if (ModelState.IsValid)
            {
                _context.Add(cat_genero);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cat_genero);
        }

        // GET: Catalogs/Generos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.cat_generos == null)
            {
                return NotFound();
            }

            var cat_genero = await _context.cat_generos.FindAsync(id);
            if (cat_genero == null)
            {
                return NotFound();
            }
            return View(cat_genero);
        }

        // POST: Catalogs/Generos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind(
                "id_genero,genero_desc,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro"
            )]
                cat_genero cat_genero
        )
        {
            if (id != cat_genero.id_genero)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cat_genero);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cat_generoExists(cat_genero.id_genero))
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
            return View(cat_genero);
        }

        // GET: Catalogs/Generos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.cat_generos == null)
            {
                return NotFound();
            }

            var cat_genero = await _context.cat_generos.FirstOrDefaultAsync(m => m.id_genero == id);
            if (cat_genero == null)
            {
                return NotFound();
            }

            return View(cat_genero);
        }

        // POST: Catalogs/Generos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.cat_generos == null)
            {
                return Problem("Entity set 'eacDbContext.cat_generos'  is null.");
            }
            var cat_genero = await _context.cat_generos.FindAsync(id);
            if (cat_genero != null)
            {
                _context.cat_generos.Remove(cat_genero);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool cat_generoExists(int id)
        {
            return (_context.cat_generos?.Any(e => e.id_genero == id)).GetValueOrDefault();
        }
    }
}
