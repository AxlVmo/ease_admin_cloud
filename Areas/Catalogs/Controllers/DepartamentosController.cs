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
    public class DepartamentosController : Controller
    {
        private readonly eacDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
          private readonly INotyfService _toastNotification;

        public DepartamentosController(eacDbContext context, UserManager<IdentityUser> userManager,INotyfService toastNotification)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }

        private Task<IdentityUser> GetCurrentUserAsync() =>
            _userManager.GetUserAsync(HttpContext.User);

        // GET: Catalogs/departamentos
        public async Task<IActionResult> Index()
        {
                        var ValidaEstatus = _context.cat_estatus.ToList();

            if (ValidaEstatus.Count == 2)
            {
                ViewBag.EstatusFlag = 1;
                var ValidaEmpresa = _context.cat_empresas.ToList();

                if (ValidaEmpresa.Count == 1)
                {
                    ViewBag.EmpresaFlag = 1;
                    var ValidaCorporativo = _context.cat_corporativos.ToList();

                    if (ValidaCorporativo.Count >= 1)
                    {
                        ViewBag.CorporativoFlag = 1;
                        
                    }
                    else
                    {
                        ViewBag.CorporativoFlag = 0;
                        _toastNotification.Information("Favor de registrar los datos de Corporativo para la Aplicación", 5);
                    }
                }
                else
                {
                    ViewBag.EmpresaFlag = 0;
                    _toastNotification.Information("Favor de registrar los datos de la Empresa para la Aplicación", 5);
                }
            }
            else
            {
                ViewBag.EstatusFlag = 0;
                _toastNotification.Information("Favor de registrar los Estatus para la Aplicación", 5);
            }
            var f_departamento = from a in _context.cat_departamentos
                             join b in _context.usuarios_controles on a.id_usuario_modifico equals b.id_usuario_control

                             select new cat_departamento
                             {
                                 id_departamento = a.id_departamento,
                                 departamento_desc = a.departamento_desc,
                                 usuario_modifico_desc = b.nombre_usuario,
                                 fecha_registro = a.fecha_registro,
                                 id_estatus_registro = a.id_estatus_registro
                             };

            return View(await f_departamento.ToListAsync());
        }

        // GET: Catalogs/departamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.cat_departamentos == null)
            {
                return NotFound();
            }

            var cat_departamento = await _context.cat_departamentos.FirstOrDefaultAsync(m => m.id_departamento == id);
            if (cat_departamento == null)
            {
                return NotFound();
            }

            return View(cat_departamento);
        }

        // GET: Catalogs/departamentos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catalogs/departamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_departamento,departamento_desc")] cat_departamento cat_departamento)
        {
            if (ModelState.IsValid)
            {
                var vDuplicado = _context.cat_departamentos
                    .Where(s => s.departamento_desc == cat_departamento.departamento_desc)
                    .ToList();

                if (vDuplicado.Count == 0)
                {
                    IdentityUser usr = await GetCurrentUserAsync();

                    cat_departamento.fecha_registro = DateTime.Now;
                    cat_departamento.departamento_desc = cat_departamento.departamento_desc.ToString().ToUpper().Trim();
                    cat_departamento.id_estatus_registro = 1;
                    cat_departamento.id_usuario_modifico = Guid.Parse(usr.Id);
                    _context.SaveChanges();

                    _context.Add(cat_departamento);
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
            return View(cat_departamento);
        }

        // GET: Catalogs/departamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            List<cat_estatus> ListaCatEstatus = new List<cat_estatus>();
            ListaCatEstatus = (from c in _context.cat_estatus select c).Distinct().ToList();
            ViewBag.ListaCatEstatus = ListaCatEstatus;
            if (id == null || _context.cat_departamentos == null)
            {
                return NotFound();
            }

            var cat_departamento = await _context.cat_departamentos.FindAsync(id);
            if (cat_departamento == null)
            {
                return NotFound();
            }
            return View(cat_departamento);
        }

        // POST: Catalogs/departamentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("id_departamento,departamento_desc,id_estatus_registro")] cat_departamento cat_departamento
        )
        {
            if (id != cat_departamento.id_departamento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cat_departamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cat_departamentoExists(cat_departamento.id_departamento))
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
            return View(cat_departamento);
        }

        // GET: Catalogs/departamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.cat_departamentos == null)
            {
                return NotFound();
            }

            var cat_departamento = await _context.cat_departamentos.FirstOrDefaultAsync(m => m.id_departamento == id);
            if (cat_departamento == null)
            {
                return NotFound();
            }

            return View(cat_departamento);
        }

        // POST: Catalogs/departamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.cat_departamentos == null)
            {
                return Problem("Entity set 'eacDbContext.cat_departamentos'  is null.");
            }
            var cat_departamento = await _context.cat_departamentos.FindAsync(id);
            cat_departamento.id_estatus_registro = 2;
            if (cat_departamento != null)
            {
                _context.cat_departamentos.Update(cat_departamento);
            }

            await _context.SaveChangesAsync();
            _toastNotification.Error("Registro desactivado con éxito", 5);
            return RedirectToAction(nameof(Index));
        }

        private bool cat_departamentoExists(int id)
        {
            return _context.cat_departamentos.Any(e => e.id_departamento == id);
        }
    }
}
