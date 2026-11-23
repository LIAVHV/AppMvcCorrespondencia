using AppMvcCorrespondencia.Services;
using AppMvcCorrespondencia.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace AppMvcCorrespondencia.Controllers
{
    [Authorize]
    public class DocumentoController : Controller
    {
        private readonly ICorrespondenciaService _correspondenciaService;

        public DocumentoController(ICorrespondenciaService correspondenciaService)
        {
            _correspondenciaService = correspondenciaService;
        }

        private int ObtenerIdUsuarioActual()
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.Sid)?.Value;
            return int.TryParse(idUsuarioClaim, out var parsedId) ? parsedId : 0;
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new CrearDocumentoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(CrearDocumentoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int idUsuario = ObtenerIdUsuarioActual();
            var doc = await _correspondenciaService.CrearDocumentoAsync(model.Nombre, model.Detalle, idUsuario);

            TempData["SuccessMessage"] = $"Documento '{doc.Nombre}' creado. Ahora seleccione el destinatario para enviarlo.";
            return RedirectToAction(nameof(Enviar), new { idDocumento = doc.IdDocumento });
        }

        [HttpGet]
        public async Task<IActionResult> Enviar(int? idDocumento = null)
        {
            int idUsuario = ObtenerIdUsuarioActual();
            var model = new EnviarDocumentoViewModel
            {
                IdDocumento = idDocumento ?? 0
            };

            await CargarListasEnviarAsync(model, idUsuario);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enviar(EnviarDocumentoViewModel model)
        {
            int idUsuario = ObtenerIdUsuarioActual();

            if (!ModelState.IsValid)
            {
                await CargarListasEnviarAsync(model, idUsuario);
                return View(model);
            }

            var resultado = await _correspondenciaService.EnviarDocumentoAsync(
                model.IdDocumento,
                idUsuario,
                model.UsuarioDestino);

            if (!resultado.Exito)
            {
                ViewBag.Mensaje = resultado.Error;
                await CargarListasEnviarAsync(model, idUsuario);
                return View(model);
            }

            TempData["SuccessMessage"] = "Documento enviado correctamente.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Recibir()
        {
            int idUsuario = ObtenerIdUsuarioActual();
            var model = new RecibirDocumentoViewModel();
            await CargarListasRecibirAsync(model, idUsuario);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recibir(RecibirDocumentoViewModel model)
        {
            int idUsuario = ObtenerIdUsuarioActual();

            if (!ModelState.IsValid)
            {
                await CargarListasRecibirAsync(model, idUsuario);
                return View(model);
            }

            var resultado = await _correspondenciaService.RecibirDocumentoAsync(
                model.IdDocumento,
                idUsuario,
                model.Observaciones,
                model.Avance,
                model.EstadoDocumento);

            if (!resultado.Exito)
            {
                ViewBag.Mensaje = resultado.Error;
                await CargarListasRecibirAsync(model, idUsuario);
                return View(model);
            }

            TempData["SuccessMessage"] = "Recepción y avance registrados correctamente.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Seguimiento()
        {
            int idUsuario = ObtenerIdUsuarioActual();
            var model = new SeguimientoDocumentoViewModel();
            await CargarListasSeguimientoAsync(model, idUsuario);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Seguimiento(SeguimientoDocumentoViewModel model)
        {
            int idUsuario = ObtenerIdUsuarioActual();
            await CargarListasSeguimientoAsync(model, idUsuario);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Historial = await _correspondenciaService.ConsultarHistorialAsync(model.IdDocumento);
            if (!model.Historial.Any())
            {
                ViewBag.InfoMensaje = "No se encontraron movimientos registrados para este documento.";
            }

            return View(model);
        }

        private async Task CargarListasEnviarAsync(EnviarDocumentoViewModel model, int idUsuario)
        {
            var docs = await _correspondenciaService.ObtenerDocumentosDisponiblesParaEnviarORecibirAsync(idUsuario);
            var destinatarios = await _correspondenciaService.ObtenerDestinatariosDisponiblesAsync(idUsuario);

            model.Documentos = new SelectList(docs, "IdDocumento", "Nombre", model.IdDocumento);
            model.Destinatarios = new SelectList(destinatarios, "IdUsuario", "Nombre", model.UsuarioDestino);
        }

        private async Task CargarListasRecibirAsync(RecibirDocumentoViewModel model, int idUsuario)
        {
            var docs = await _correspondenciaService.ObtenerDocumentosDisponiblesParaEnviarORecibirAsync(idUsuario);
            model.Documentos = new SelectList(docs, "IdDocumento", "Nombre", model.IdDocumento);
        }

        private async Task CargarListasSeguimientoAsync(SeguimientoDocumentoViewModel model, int idUsuario)
        {
            var docs = await _correspondenciaService.ObtenerDocumentosParaSeguimientoAsync(idUsuario);
            model.Documentos = new SelectList(docs, "IdDocumento", "Nombre", model.IdDocumento);
        }
    }
}
