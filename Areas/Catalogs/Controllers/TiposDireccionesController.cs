using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ease_admin_cloud.Areas.Catalogs.Models;
using ease_admin_cloud.Data;

namespace ease_admin_cloud.Areas.Catalogs.Controllers
{
    [Area("Catalogs")]
    public class TiposDireccionesController : Controller
    {
        private readonly eacDbContext _context;

        public TiposDireccionesController(eacDbContext context)
        {
            _context = context;
        }

        // GET: Catalogs/TiposDirecciones
        public async Task<IActionResult> Index()
        {
              return _context.cat_tipo_direccion != null ? 
                          View(await _context.cat_tipo_direccion.ToListAsync()) :
                          Problem("Entity set 'eacDbContext.cat_tipo_direccion'  is null.");
        }

        // GET: Catalogs/TiposDirecciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.cat_tipo_direccion == null)
            {
                return NotFound();
            }

            var cat_tipo_direccion = await _context.cat_tipo_direccion
                .FirstOrDefaultAsync(m => m.id_tipo_direccion == id);
            if (cat_tipo_direccion == null)
            {
                return NotFound();
            }

            return View(cat_tipo_direccion);
        }

        // GET: Catalogs/TiposDirecciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catalogs/TiposDirecciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_tipo_direccion,tipo_direccion_desc,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro")] cat_tipo_direccion cat_tipo_direccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cat_tipo_direccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cat_tipo_direccion);
        }

        // GET: Catalogs/TiposDirecciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.cat_tipo_direccion == null)
            {
                return NotFound();
            }

            var cat_tipo_direccion = await _context.cat_tipo_direccion.FindAsync(id);
            if (cat_tipo_direccion == null)
            {
                return NotFound();
            }
            return View(cat_tipo_direccion);
        }

        // POST: Catalogs/TiposDirecciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_tipo_direccion,tipo_direccion_desc,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro")] cat_tipo_direccion cat_tipo_direccion)
        {
            if (id != cat_tipo_direccion.id_tipo_direccion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cat_tipo_direccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cat_tipo_direccionExists(cat_tipo_direccion.id_tipo_direccion))
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
            return View(cat_tipo_direccion);
        }

        // GET: Catalogs/TiposDirecciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.cat_tipo_direccion == null)
            {
                return NotFound();
            }

            var cat_tipo_direccion = await _context.cat_tipo_direccion
                .FirstOrDefaultAsync(m => m.id_tipo_direccion == id);
            if (cat_tipo_direccion == null)
            {
                return NotFound();
            }

            return View(cat_tipo_direccion);
        }

        // POST: Catalogs/TiposDirecciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.cat_tipo_direccion == null)
            {
                return Problem("Entity set 'eacDbContext.cat_tipo_direccion'  is null.");
            }
            var cat_tipo_direccion = await _context.cat_tipo_direccion.FindAsync(id);
            if (cat_tipo_direccion != null)
            {
                _context.cat_tipo_direccion.Remove(cat_tipo_direccion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool cat_tipo_direccionExists(int id)
        {
          return (_context.cat_tipo_direccion?.Any(e => e.id_tipo_direccion == id)).GetValueOrDefault();
        }
    }
}
