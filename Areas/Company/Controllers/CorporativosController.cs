using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ease_admin_cloud.Areas.Company.Models;
using ease_admin_cloud.Data;
using Microsoft.AspNetCore.Identity;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ease_admin_cloud.Areas.Company.Controllers
{
    [Area("Company")]
    public class CorporativosController : Controller
    {
        private readonly eacDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotyfService _toastNotification;

        public CorporativosController( eacDbContext context,
            UserManager<IdentityUser> userManager,
            INotyfService toastNotification
        )
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }

        // GET: Company/Corporativos
        public async Task<IActionResult> Index()
        {
              return _context.tbl_corporativos != null ? 
                          View(await _context.tbl_corporativos.ToListAsync()) :
                          Problem("Entity set 'eacDbContext.tbl_corporativos'  is null.");
        }

        // GET: Company/Corporativos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.tbl_corporativos == null)
            {
                return NotFound();
            }

            var tbl_corporativo = await _context.tbl_corporativos
                .FirstOrDefaultAsync(m => m.id_corporativo == id);
            if (tbl_corporativo == null)
            {
                return NotFound();
            }

            return View(tbl_corporativo);
        }

        // GET: Company/Corporativos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Company/Corporativos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_corporativo,nombre_corporativo,calle,codigo_postal,id_colonia,colonia,localidad_municipio,ciudad,estado,correo_electronico,telefono,id_empresa,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro")] tbl_corporativo tbl_corporativo)
        {
            if (ModelState.IsValid)
            {
                tbl_corporativo.id_corporativo = Guid.NewGuid();
                _context.Add(tbl_corporativo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbl_corporativo);
        }

        // GET: Company/Corporativos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.tbl_corporativos == null)
            {
                return NotFound();
            }

            var tbl_corporativo = await _context.tbl_corporativos.FindAsync(id);
            if (tbl_corporativo == null)
            {
                return NotFound();
            }
            return View(tbl_corporativo);
        }

        // POST: Company/Corporativos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_corporativo,nombre_corporativo,calle,codigo_postal,id_colonia,colonia,localidad_municipio,ciudad,estado,correo_electronico,telefono,id_empresa,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro")] tbl_corporativo tbl_corporativo)
        {
            if (id != tbl_corporativo.id_corporativo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbl_corporativo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tbl_corporativoExists(tbl_corporativo.id_corporativo))
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
            return View(tbl_corporativo);
        }

        // GET: Company/Corporativos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.tbl_corporativos == null)
            {
                return NotFound();
            }

            var tbl_corporativo = await _context.tbl_corporativos
                .FirstOrDefaultAsync(m => m.id_corporativo == id);
            if (tbl_corporativo == null)
            {
                return NotFound();
            }

            return View(tbl_corporativo);
        }

        // POST: Company/Corporativos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.tbl_corporativos == null)
            {
                return Problem("Entity set 'eacDbContext.tbl_corporativos'  is null.");
            }
            var tbl_corporativo = await _context.tbl_corporativos.FindAsync(id);
            if (tbl_corporativo != null)
            {
                _context.tbl_corporativos.Remove(tbl_corporativo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tbl_corporativoExists(Guid id)
        {
          return (_context.tbl_corporativos?.Any(e => e.id_corporativo == id)).GetValueOrDefault();
        }
    }
}
