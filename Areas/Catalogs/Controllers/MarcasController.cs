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
    public class MarcasController : Controller
    {
        private readonly eacDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotyfService _toastNotification;

        public MarcasController(
            eacDbContext context,
            UserManager<IdentityUser> userManager,
            INotyfService toastNotification
        )
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }

        // GET: Catalogs/Marcas
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
            var f_marca =
                from a in _context.cat_marcas
                join b in _context.tbl_usuarios_controles
                    on a.id_usuario_modifico equals b.id_usuario_control

                select new cat_marca
                {
                    id_marca = a.id_marca,
                    marca_desc = a.marca_desc,
                    usuario_modifico_desc = b.nombre_usuario,
                    fecha_registro = a.fecha_registro,
                    id_estatus_registro = a.id_estatus_registro
                };

            return View(await f_marca.ToListAsync());
        }

        // GET: Catalogs/Marcas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.cat_marcas == null)
            {
                return NotFound();
            }

            var cat_marca = await _context.cat_marcas.FirstOrDefaultAsync(m => m.id_marca == id);
            if (cat_marca == null)
            {
                return NotFound();
            }

            return View(cat_marca);
        }

        // GET: Catalogs/Marcas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catalogs/Marcas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "id_marca,marca_desc,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro"
            )]
                cat_marca cat_marca
        )
        {
            if (ModelState.IsValid)
            {
                _context.Add(cat_marca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cat_marca);
        }

        // GET: Catalogs/Marcas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.cat_marcas == null)
            {
                return NotFound();
            }

            var cat_marca = await _context.cat_marcas.FindAsync(id);
            if (cat_marca == null)
            {
                return NotFound();
            }
            return View(cat_marca);
        }

        // POST: Catalogs/Marcas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind(
                "id_marca,marca_desc,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro"
            )]
                cat_marca cat_marca
        )
        {
            if (id != cat_marca.id_marca)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cat_marca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cat_marcaExists(cat_marca.id_marca))
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
            return View(cat_marca);
        }

        // GET: Catalogs/Marcas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.cat_marcas == null)
            {
                return NotFound();
            }

            var cat_marca = await _context.cat_marcas.FirstOrDefaultAsync(m => m.id_marca == id);
            if (cat_marca == null)
            {
                return NotFound();
            }

            return View(cat_marca);
        }

        // POST: Catalogs/Marcas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.cat_marcas == null)
            {
                return Problem("Entity set 'eacDbContext.cat_marcas'  is null.");
            }
            var cat_marca = await _context.cat_marcas.FindAsync(id);
            if (cat_marca != null)
            {
                _context.cat_marcas.Remove(cat_marca);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool cat_marcaExists(int id)
        {
            return (_context.cat_marcas?.Any(e => e.id_marca == id)).GetValueOrDefault();
        }
    }
}
