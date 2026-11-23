using AppMvcCorrespondencia.Services;
using AppMvcCorrespondencia.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppMvcCorrespondencia.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioService.ObtenerUsuariosOperativosAsync();
            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _usuarioService.ObtenerPorIdAsync(id.Value);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new UsuarioCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _usuarioService.ExisteUsernameAsync(model.Username.Trim()))
            {
                ModelState.AddModelError(nameof(model.Username), "El nombre de usuario ya está registrado.");
                return View(model);
            }

            await _usuarioService.CrearUsuarioAsync(model);
            TempData["SuccessMessage"] = "Usuario creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _usuarioService.ObtenerPorIdAsync(id.Value);
            if (usuario == null)
            {
                return NotFound();
            }

            var model = new UsuarioEditViewModel
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Username = usuario.Username,
                Rol = usuario.Rol,
                Departamento = usuario.Departamento
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioEditViewModel model)
        {
            if (id != model.IdUsuario)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _usuarioService.ExisteUsernameAsync(model.Username.Trim(), model.IdUsuario))
            {
                ModelState.AddModelError(nameof(model.Username), "El nombre de usuario ya está asignado a otro registro.");
                return View(model);
            }

            var actualizado = await _usuarioService.ActualizarUsuarioAsync(model);
            if (!actualizado)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Usuario actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _usuarioService.ObtenerPorIdAsync(id.Value);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resultado = await _usuarioService.EliminarUsuarioAsync(id);
            if (!resultado.Exito)
            {
                var usuario = await _usuarioService.ObtenerPorIdAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                ViewBag.Mensaje = resultado.Error;
                return View(usuario);
            }

            TempData["SuccessMessage"] = "Usuario eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
