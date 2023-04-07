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
    public class CategoriasController : Controller
    {
        private readonly eacDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotyfService _toastNotification;

        public CategoriasController(
            eacDbContext context,
            UserManager<IdentityUser> userManager,
            INotyfService toastNotification
        )
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }

        // GET: Catalogs/Categorias
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
            var f_categorias =
                from a in _context.cat_categorias
                join b in _context.tbl_usuarios_controles
                    on a.id_usuario_modifico equals b.id_usuario_control

                select new cat_categoria
                {
                    id_categoria = a.id_categoria,
                    categoria_desc = a.categoria_desc,
                    usuario_modifico_desc = b.nombre_usuario,
                    fecha_registro = a.fecha_registro,
                    id_estatus_registro = a.id_estatus_registro
                };

            return View(await f_categorias.ToListAsync());
        }

        // GET: Catalogs/Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.cat_categorias == null)
            {
                return NotFound();
            }

            var cat_categoria = await _context.cat_categorias.FirstOrDefaultAsync(
                m => m.id_categoria == id
            );
            if (cat_categoria == null)
            {
                return NotFound();
            }

            return View(cat_categoria);
        }

        // GET: Catalogs/Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catalogs/Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "id_categoria,categoria_desc,id_marca,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro"
            )]
                cat_categoria cat_categoria
        )
        {
            if (ModelState.IsValid)
            {
                _context.Add(cat_categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cat_categoria);
        }

        // GET: Catalogs/Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.cat_categorias == null)
            {
                return NotFound();
            }

            var cat_categoria = await _context.cat_categorias.FindAsync(id);
            if (cat_categoria == null)
            {
                return NotFound();
            }
            return View(cat_categoria);
        }

        // POST: Catalogs/Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind(
                "id_categoria,categoria_desc,id_marca,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro"
            )]
                cat_categoria cat_categoria
        )
        {
            if (id != cat_categoria.id_categoria)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cat_categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cat_categoriaExists(cat_categoria.id_categoria))
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
            return View(cat_categoria);
        }

        // GET: Catalogs/Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.cat_categorias == null)
            {
                return NotFound();
            }

            var cat_categoria = await _context.cat_categorias.FirstOrDefaultAsync(
                m => m.id_categoria == id
            );
            if (cat_categoria == null)
            {
                return NotFound();
            }

            return View(cat_categoria);
        }

        // POST: Catalogs/Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.cat_categorias == null)
            {
                return Problem("Entity set 'eacDbContext.cat_categorias'  is null.");
            }
            var cat_categoria = await _context.cat_categorias.FindAsync(id);
            if (cat_categoria != null)
            {
                _context.cat_categorias.Remove(cat_categoria);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool cat_categoriaExists(int id)
        {
            return (_context.cat_categorias?.Any(e => e.id_categoria == id)).GetValueOrDefault();
        }
    }
}
