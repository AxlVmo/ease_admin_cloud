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
    public class PerfilesController : Controller
    {
        private readonly eacDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotyfService _toastNotification;

        public PerfilesController( eacDbContext context,
            UserManager<IdentityUser> userManager,
            INotyfService toastNotification
        )
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }

        // GET: Catalogs/Perfiles
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
            var f_perfiles =
                from a in _context.cat_perfiles
                join b in _context.tbl_usuarios_controles
                    on a.id_usuario_modifico equals b.id_usuario_control

                select new cat_perfil
                {
                    id_perfil = a.id_perfil,
                    perfil_desc = a.perfil_desc,
                    usuario_modifico_desc = b.nombre_usuario,
                    fecha_registro = a.fecha_registro,
                    id_estatus_registro = a.id_estatus_registro
                };

            return View(await f_perfiles.ToListAsync());
        }

        // GET: Catalogs/Perfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.cat_perfiles == null)
            {
                return NotFound();
            }

            var cat_perfil = await _context.cat_perfiles
                .FirstOrDefaultAsync(m => m.id_perfil == id);
            if (cat_perfil == null)
            {
                return NotFound();
            }

            return View(cat_perfil);
        }

        // GET: Catalogs/Perfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catalogs/Perfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_perfil,perfil_desc,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro")] cat_perfil cat_perfil)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cat_perfil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cat_perfil);
        }

        // GET: Catalogs/Perfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.cat_perfiles == null)
            {
                return NotFound();
            }

            var cat_perfil = await _context.cat_perfiles.FindAsync(id);
            if (cat_perfil == null)
            {
                return NotFound();
            }
            return View(cat_perfil);
        }

        // POST: Catalogs/Perfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_perfil,perfil_desc,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro")] cat_perfil cat_perfil)
        {
            if (id != cat_perfil.id_perfil)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cat_perfil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cat_perfilExists(cat_perfil.id_perfil))
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
            return View(cat_perfil);
        }

        // GET: Catalogs/Perfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.cat_perfiles == null)
            {
                return NotFound();
            }

            var cat_perfil = await _context.cat_perfiles
                .FirstOrDefaultAsync(m => m.id_perfil == id);
            if (cat_perfil == null)
            {
                return NotFound();
            }

            return View(cat_perfil);
        }

        // POST: Catalogs/Perfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.cat_perfiles == null)
            {
                return Problem("Entity set 'eacDbContext.cat_perfiles'  is null.");
            }
            var cat_perfil = await _context.cat_perfiles.FindAsync(id);
            if (cat_perfil != null)
            {
                _context.cat_perfiles.Remove(cat_perfil);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool cat_perfilExists(int id)
        {
          return (_context.cat_perfiles?.Any(e => e.id_perfil == id)).GetValueOrDefault();
        }
    }
}
