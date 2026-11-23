using AppMvcCorrespondencia.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AppMvcCorrespondencia.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICorrespondenciaService _correspondenciaService;

        public HomeController(ICorrespondenciaService correspondenciaService)
        {
            _correspondenciaService = correspondenciaService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Usuario");
            }

            var idUsuarioClaim = User.FindFirst(ClaimTypes.Sid)?.Value;
            int idUsuario = int.TryParse(idUsuarioClaim, out var parsedId) ? parsedId : 0;

            var model = await _correspondenciaService.ObtenerMetricasDashboardAsync(idUsuario);
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
