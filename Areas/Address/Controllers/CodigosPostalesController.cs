using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ease_admin_cloud.Areas.Address.Models;
using ease_admin_cloud.Data;
using System.Formats.Asn1;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using AspNetCoreHero.ToastNotification.Abstractions;


namespace ease_admin_cloud.Areas.Address.Controllers
{
    [Area("Address")]
    public class CodigosPostalesController : Controller
    {
        private readonly eacDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotyfService _toastNotification;
     
                public CodigosPostalesController( eacDbContext context,
            UserManager<IdentityUser> userManager,
            INotyfService toastNotification
        )
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }

        // GET: Address/CodigosPostales
        public async Task<IActionResult> Index()
        {
              return _context.cat_codigos_postales != null ? 
                          View(await _context.cat_codigos_postales.ToListAsync()) :
                          Problem("Entity set 'eacDbContext.cat_codigos_postales'  is null.");
        }
        [HttpGet]
        public ActionResult FiltroColonia(string id, string idC)
        {
            var fcatColonias = (from ta in _context.cat_codigos_postales
                                where ta.d_codigo == id
                                where ta.id_asenta_cpcons == idC
                                select ta).Distinct().ToList();

            return Json(fcatColonias);
        }

        // GET: Address/CodigosPostales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.cat_codigos_postales == null)
            {
                return NotFound();
            }

            var cat_codigo_postal = await _context.cat_codigos_postales
                .FirstOrDefaultAsync(m => m.id_codigo_postal == id);
            if (cat_codigo_postal == null)
            {
                return NotFound();
            }

            return View(cat_codigo_postal);
        }

        // GET: Address/CodigosPostales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Address/CodigosPostales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_codigo_postal,d_codigo,d_asenta,d_tipoAsenta,d_mnpio,d_estado,d_ciudad,d_cp,c_estado,c_oficina,c_cp,c_tipoAsenta,c_mnpio,id_asenta_cpcons,d_zona,c_cveCiudad")] cat_codigo_postal cat_codigo_postal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cat_codigo_postal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cat_codigo_postal);
        }

        // GET: Address/CodigosPostales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.cat_codigos_postales == null)
            {
                return NotFound();
            }

            var cat_codigo_postal = await _context.cat_codigos_postales.FindAsync(id);
            if (cat_codigo_postal == null)
            {
                return NotFound();
            }
            return View(cat_codigo_postal);
        }

        // POST: Address/CodigosPostales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_codigo_postal,d_codigo,d_asenta,d_tipoAsenta,d_mnpio,d_estado,d_ciudad,d_cp,c_estado,c_oficina,c_cp,c_tipoAsenta,c_mnpio,id_asenta_cpcons,d_zona,c_cveCiudad")] cat_codigo_postal cat_codigo_postal)
        {
            if (id != cat_codigo_postal.id_codigo_postal)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cat_codigo_postal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cat_codigo_postalExists(cat_codigo_postal.id_codigo_postal))
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
            return View(cat_codigo_postal);
        }

        // GET: Address/CodigosPostales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.cat_codigos_postales == null)
            {
                return NotFound();
            }

            var cat_codigo_postal = await _context.cat_codigos_postales
                .FirstOrDefaultAsync(m => m.id_codigo_postal == id);
            if (cat_codigo_postal == null)
            {
                return NotFound();
            }

            return View(cat_codigo_postal);
        }

        // POST: Address/CodigosPostales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.cat_codigos_postales == null)
            {
                return Problem("Entity set 'eacDbContext.cat_codigos_postales'  is null.");
            }
            var cat_codigo_postal = await _context.cat_codigos_postales.FindAsync(id);
            if (cat_codigo_postal != null)
            {
                _context.cat_codigos_postales.Remove(cat_codigo_postal);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool cat_codigo_postalExists(int id)
        {
          return (_context.cat_codigos_postales?.Any(e => e.id_codigo_postal == id)).GetValueOrDefault();
        }
    }
}
