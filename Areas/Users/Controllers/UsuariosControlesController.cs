﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ease_admin_cloud.Areas.Users.Models;

namespace ease_admin_cloud.Areas.Users.Controllers
{
    [Area("Users")]
    public class UsuariosControlesController : Controller
    {
        private readonly Data.eacDbContext _context;
  

        public UsuariosControlesController(Data.eacDbContext context)
        {
            _context = context;

        }

        [HttpGet, Route("~/Usuarios/")]
        // GET: Address/UsuariosControles
        public async Task<IActionResult> Index()
        {
            return View(await _context.tbl_usuarios_controles.ToListAsync());
        }

        // GET: Address/UsuariosControles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.tbl_usuarios_controles == null)
            {
                return NotFound();
            }

            var usuario_control = await _context.tbl_usuarios_controles.FirstOrDefaultAsync(
                m => m.id_usuario_control == id
            );
            if (usuario_control == null)
            {
                return NotFound();
            }

            return View(usuario_control);
        }

        // GET: Address/UsuariosControles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Address/UsuariosControles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "id_usuario_control,nombres,apellido_paterno,apellido_materno,nombre_usuario,id_area,id_genero,id_perfil,id_rol,terminos_uso,fecha_nacimiento,correo_acceso,profile_picture,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro"
            )]
                tbl_usuario_control usuario_control
        )
        {
            if (ModelState.IsValid)
            {
                usuario_control.id_usuario_control = Guid.NewGuid();
                _context.Add(usuario_control);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario_control);
        }

        // GET: Address/UsuariosControles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.tbl_usuarios_controles == null)
            {
                return NotFound();
            }

            var usuario_control = await _context.tbl_usuarios_controles.FindAsync(id);
            if (usuario_control == null)
            {
                return NotFound();
            }
            return View(usuario_control);
        }

        // POST: Address/UsuariosControles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind(
                "id_usuario_control,nombres,apellido_paterno,apellido_materno,nombre_usuario,id_area,id_genero,id_perfil,id_rol,terminos_uso,fecha_nacimiento,correo_acceso,profile_picture,id_usuario_modifico,fecha_registro,fecha_actualizacion,id_estatus_registro"
            )]
                tbl_usuario_control usuario_control
        )
        {
            if (id != usuario_control.id_usuario_control)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario_control);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!usuario_controlExists(usuario_control.id_usuario_control))
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
            return View(usuario_control);
        }

        // GET: Address/UsuariosControles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.tbl_usuarios_controles == null)
            {
                return NotFound();
            }

            var usuario_control = await _context.tbl_usuarios_controles.FirstOrDefaultAsync(
                m => m.id_usuario_control == id
            );
            if (usuario_control == null)
            {
                return NotFound();
            }

            return View(usuario_control);
        }

        // POST: Address/UsuariosControles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.tbl_usuarios_controles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.tbl_usuarios_controles'  is null.");
            }
            var usuario_control = await _context.tbl_usuarios_controles.FindAsync(id);
            if (usuario_control != null)
            {
                _context.tbl_usuarios_controles.Remove(usuario_control);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool usuario_controlExists(Guid id)
        {
            return _context.tbl_usuarios_controles.Any(e => e.id_usuario_control == id);
        }

        [HttpGet, Route("~/FiltroUsuarioControl/")]
        public ActionResult FiltroUsuarioControl()
        {
            var f_usuario_control = (
                from ta in _context.tbl_usuarios_controles
                where ta.id_area == 1 & ta.id_rol == 2 & ta.id_perfil == 1
                select ta
            )
                .Distinct()
                .ToList();

            return Json(f_usuario_control);
        }
    }
}
