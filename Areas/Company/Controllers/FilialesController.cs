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
    public class FilialesController : Controller
    {
        private readonly eacDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotyfService _toastNotification;

        public FilialesController( eacDbContext context,
            UserManager<IdentityUser> userManager,
            INotyfService toastNotification
        )
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }

        // GET: Company/Filiales
        public async Task<IActionResult> Index()
        {
              return _context.tbl_filiales != null ? 
                          View(await _context.tbl_filiales.ToListAsync()) :
                          Problem("Entity set 'eacDbContext.tbl_filiales'  is null.");
        }

        // GET: Company/Filiales/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.tbl_filiales == null)
            {
                return NotFound();
            }

            var tbl_filial = await _context.tbl_filiales
                .FirstOrDefaultAsync(m => m.id_filial == id);
            if (tbl_filial == null)
            {
                return NotFound();
            }

            return View(tbl_filial);
        }

        // GET: Company/Filiales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Company/Filiales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_filial,id_tipo_filial,nombre_filial,calle,codigo_postal,id_colonia,colonia,localidad_municipio,ciudad,estado,correo_electronico,telefono,id_corporativo,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro")] tbl_filial tbl_filial)
        {
            if (ModelState.IsValid)
            {
                tbl_filial.id_filial = Guid.NewGuid();
                _context.Add(tbl_filial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbl_filial);
        }

        // GET: Company/Filiales/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.tbl_filiales == null)
            {
                return NotFound();
            }

            var tbl_filial = await _context.tbl_filiales.FindAsync(id);
            if (tbl_filial == null)
            {
                return NotFound();
            }
            return View(tbl_filial);
        }

        // POST: Company/Filiales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_filial,id_tipo_filial,nombre_filial,calle,codigo_postal,id_colonia,colonia,localidad_municipio,ciudad,estado,correo_electronico,telefono,id_corporativo,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro")] tbl_filial tbl_filial)
        {
            if (id != tbl_filial.id_filial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbl_filial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tbl_filialExists(tbl_filial.id_filial))
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
            return View(tbl_filial);
        }

        // GET: Company/Filiales/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.tbl_filiales == null)
            {
                return NotFound();
            }

            var tbl_filial = await _context.tbl_filiales
                .FirstOrDefaultAsync(m => m.id_filial == id);
            if (tbl_filial == null)
            {
                return NotFound();
            }

            return View(tbl_filial);
        }

        // POST: Company/Filiales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.tbl_filiales == null)
            {
                return Problem("Entity set 'eacDbContext.tbl_filiales'  is null.");
            }
            var tbl_filial = await _context.tbl_filiales.FindAsync(id);
            if (tbl_filial != null)
            {
                _context.tbl_filiales.Remove(tbl_filial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tbl_filialExists(Guid id)
        {
          return (_context.tbl_filiales?.Any(e => e.id_filial == id)).GetValueOrDefault();
        }
    }
}
