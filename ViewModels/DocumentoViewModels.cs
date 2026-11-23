using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppMvcCorrespondencia.ViewModels
{
    public class CrearDocumentoViewModel
    {
        [Required(ErrorMessage = "El nombre del documento es obligatorio.")]
        [StringLength(255, ErrorMessage = "El nombre no debe exceder 255 caracteres.")]
        [Display(Name = "Nombre del documento, no. oficio, solicitud")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "El detalle no debe exceder 255 caracteres.")]
        [Display(Name = "Detalle")]
        public string? Detalle { get; set; }
    }

    public class EnviarDocumentoViewModel
    {
        [Required(ErrorMessage = "Debe elegir un destinatario.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe elegir un destinatario válido.")]
        [Display(Name = "Destinatario")]
        public int UsuarioDestino { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un documento.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un documento válido.")]
        [Display(Name = "Documento")]
        public int IdDocumento { get; set; }

        public SelectList? Destinatarios { get; set; }
        public SelectList? Documentos { get; set; }
    }

    public class RecibirDocumentoViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un documento.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un documento válido.")]
        [Display(Name = "Documento")]
        public int IdDocumento { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un porcentaje de avance.")]
        [Range(1, 100, ErrorMessage = "El avance debe ser entre 25 y 100.")]
        [Display(Name = "Avance (%)")]
        public int Avance { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el estado del documento.")]
        [StringLength(20)]
        [Display(Name = "Estado del Documento")]
        public string EstadoDocumento { get; set; } = "en proceso";

        [Required(ErrorMessage = "Las observaciones son obligatorias.")]
        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; } = string.Empty;

        public SelectList? Documentos { get; set; }
    }

    public class HistorialItemViewModel
    {
        public DateTime? FechaMovimiento { get; set; }
        public string Documento { get; set; } = string.Empty;
        public string? Accion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public int Avance { get; set; }
        public string? UsuarioOrigenNombre { get; set; }
        public string? UsuarioDestinoNombre { get; set; }
    }

    public class SeguimientoDocumentoViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un documento.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un documento válido.")]
        [Display(Name = "Documento")]
        public int IdDocumento { get; set; }

        public SelectList? Documentos { get; set; }
        public List<HistorialItemViewModel> Historial { get; set; } = new();
    }
}
