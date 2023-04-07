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
    public class RolesController : Controller
    {
        private readonly eacDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotyfService _toastNotification;

        public RolesController( eacDbContext context,
            UserManager<IdentityUser> userManager,
            INotyfService toastNotification
        )
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }

        // GET: Catalogs/Roles
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
            var f_roles =
                from a in _context.cat_roles
                join b in _context.tbl_usuarios_controles
                    on a.id_usuario_modifico equals b.id_usuario_control

                select new cat_role
                {
                    id_rol = a.id_rol,
                    rol_desc = a.rol_desc,
                    usuario_modifico_desc = b.nombre_usuario,
                    fecha_registro = a.fecha_registro,
                    id_estatus_registro = a.id_estatus_registro
                };

            return View(await f_roles.ToListAsync());
        }

        // GET: Catalogs/Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.cat_roles == null)
            {
                return NotFound();
            }

            var cat_role = await _context.cat_roles
                .FirstOrDefaultAsync(m => m.id_rol == id);
            if (cat_role == null)
            {
                return NotFound();
            }

            return View(cat_role);
        }

        // GET: Catalogs/Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catalogs/Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_rol,rol_desc,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro")] cat_role cat_role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cat_role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cat_role);
        }

        // GET: Catalogs/Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.cat_roles == null)
            {
                return NotFound();
            }

            var cat_role = await _context.cat_roles.FindAsync(id);
            if (cat_role == null)
            {
                return NotFound();
            }
            return View(cat_role);
        }

        // POST: Catalogs/Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_rol,rol_desc,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro")] cat_role cat_role)
        {
            if (id != cat_role.id_rol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cat_role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cat_roleExists(cat_role.id_rol))
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
            return View(cat_role);
        }

        // GET: Catalogs/Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.cat_roles == null)
            {
                return NotFound();
            }

            var cat_role = await _context.cat_roles
                .FirstOrDefaultAsync(m => m.id_rol == id);
            if (cat_role == null)
            {
                return NotFound();
            }

            return View(cat_role);
        }

        // POST: Catalogs/Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.cat_roles == null)
            {
                return Problem("Entity set 'eacDbContext.cat_roles'  is null.");
            }
            var cat_role = await _context.cat_roles.FindAsync(id);
            if (cat_role != null)
            {
                _context.cat_roles.Remove(cat_role);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool cat_roleExists(int id)
        {
          return (_context.cat_roles?.Any(e => e.id_rol == id)).GetValueOrDefault();
        }
    }
}
