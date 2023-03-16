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
    public class AreasController : Controller
    {
        private readonly eacDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
          private readonly INotyfService _toastNotification;

        public AreasController(eacDbContext context, UserManager<IdentityUser> userManager,INotyfService toastNotification)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }

        private Task<IdentityUser> GetCurrentUserAsync() =>
            _userManager.GetUserAsync(HttpContext.User);

        // GET: Catalogs/Areas
        public async Task<IActionResult> Index()
        {
            return View(await _context.cat_areas.ToListAsync());
        }

        // GET: Catalogs/Areas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.cat_areas == null)
            {
                return NotFound();
            }

            var cat_area = await _context.cat_areas.FirstOrDefaultAsync(m => m.id_area == id);
            if (cat_area == null)
            {
                return NotFound();
            }

            return View(cat_area);
        }

        // GET: Catalogs/Areas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catalogs/Areas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_area,area_desc")] cat_area cat_area)
        {
            if (ModelState.IsValid)
            {
                var vDuplicado = _context.cat_areas
                    .Where(s => s.area_desc == cat_area.area_desc)
                    .ToList();

                if (vDuplicado.Count == 0)
                {
                    IdentityUser usr = await GetCurrentUserAsync();

                    cat_area.fecha_registro = DateTime.Now;
                    cat_area.area_desc = cat_area.area_desc.ToString().ToUpper().Trim();
                    cat_area.id_estatus_registro = 1;
                    cat_area.id_usuario_modifico = Guid.Parse(usr.Id);
                    _context.SaveChanges();

                    _context.Add(cat_area);
                    await _context.SaveChangesAsync();
                    _toastNotification.Success("Registro creado con éxito", 5);
                }
                else
                {
                    //_notifyService.Custom("Custom Notification - closes in 5 seconds.", 5, "whitesmoke", "fa fa-gear");
                    _toastNotification.Information(
                        "Favor de validar, existe una Estatus con el mismo nombre",
                        5
                    );
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cat_area);
        }

        // GET: Catalogs/Areas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            List<cat_estatus> ListaCatEstatus = new List<cat_estatus>();
            ListaCatEstatus = (from c in _context.cat_estatus select c).Distinct().ToList();
            ViewBag.ListaCatEstatus = ListaCatEstatus;
            if (id == null || _context.cat_areas == null)
            {
                return NotFound();
            }

            var cat_area = await _context.cat_areas.FindAsync(id);
            if (cat_area == null)
            {
                return NotFound();
            }
            return View(cat_area);
        }

        // POST: Catalogs/Areas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("id_area,area_desc,id_estatus_registro")] cat_area cat_area
        )
        {
            if (id != cat_area.id_area)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cat_area);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cat_areaExists(cat_area.id_area))
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
            return View(cat_area);
        }

        // GET: Catalogs/Areas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.cat_areas == null)
            {
                return NotFound();
            }

            var cat_area = await _context.cat_areas.FirstOrDefaultAsync(m => m.id_area == id);
            if (cat_area == null)
            {
                return NotFound();
            }

            return View(cat_area);
        }

        // POST: Catalogs/Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.cat_areas == null)
            {
                return Problem("Entity set 'eacDbContext.cat_areas'  is null.");
            }
            var cat_area = await _context.cat_areas.FindAsync(id);
            cat_area.id_estatus_registro = 2;
            if (cat_area != null)
            {
                _context.cat_areas.Update(cat_area);
            }

            await _context.SaveChangesAsync();
            _toastNotification.Error("Registro desactivado con éxito", 5);
            return RedirectToAction(nameof(Index));
        }

        private bool cat_areaExists(int id)
        {
            return _context.cat_areas.Any(e => e.id_area == id);
        }
    }
}
