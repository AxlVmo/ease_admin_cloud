using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ease_admin_cloud.Areas.Company.Models;
using ease_admin_cloud.Data;

namespace ease_admin_cloud.Areas.Company.Controllers
{
    [Area("Company")]
    public class EmpresasController : Controller
    {
        private readonly eacDbContext _context;

        public EmpresasController(eacDbContext context)
        {
            _context = context;
        }

        // GET: Company/Empresas
        public async Task<IActionResult> Index()
        {
              return _context.tbl_empresas != null ? 
                          View(await _context.tbl_empresas.ToListAsync()) :
                          Problem("Entity set 'eacDbContext.tbl_empresas'  is null.");
        }

        // GET: Company/Empresas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.tbl_empresas == null)
            {
                return NotFound();
            }

            var tbl_empresa = await _context.tbl_empresas
                .FirstOrDefaultAsync(m => m.id_empresa == id);
            if (tbl_empresa == null)
            {
                return NotFound();
            }

            return View(tbl_empresa);
        }

        // GET: Company/Empresas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Company/Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_empresa,nombre_empresa,rfc,giro_comercial,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro")] tbl_empresa tbl_empresa)
        {
            if (ModelState.IsValid)
            {
                tbl_empresa.id_empresa = Guid.NewGuid();
                _context.Add(tbl_empresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbl_empresa);
        }

        // GET: Company/Empresas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.tbl_empresas == null)
            {
                return NotFound();
            }

            var tbl_empresa = await _context.tbl_empresas.FindAsync(id);
            if (tbl_empresa == null)
            {
                return NotFound();
            }
            return View(tbl_empresa);
        }

        // POST: Company/Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_empresa,nombre_empresa,rfc,giro_comercial,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro")] tbl_empresa tbl_empresa)
        {
            if (id != tbl_empresa.id_empresa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbl_empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tbl_empresaExists(tbl_empresa.id_empresa))
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
            return View(tbl_empresa);
        }

        // GET: Company/Empresas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.tbl_empresas == null)
            {
                return NotFound();
            }

            var tbl_empresa = await _context.tbl_empresas
                .FirstOrDefaultAsync(m => m.id_empresa == id);
            if (tbl_empresa == null)
            {
                return NotFound();
            }

            return View(tbl_empresa);
        }

        // POST: Company/Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.tbl_empresas == null)
            {
                return Problem("Entity set 'eacDbContext.tbl_empresas'  is null.");
            }
            var tbl_empresa = await _context.tbl_empresas.FindAsync(id);
            if (tbl_empresa != null)
            {
                _context.tbl_empresas.Remove(tbl_empresa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tbl_empresaExists(Guid id)
        {
          return (_context.tbl_empresas?.Any(e => e.id_empresa == id)).GetValueOrDefault();
        }
    }
}
