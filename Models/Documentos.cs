using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMvcCorrespondencia.Models;

public partial class Documentos
{
    [Key]
    public int IdDocumento { get; set; }

    [Required(ErrorMessage = "El nombre del documento es obligatorio.")]
    [StringLength(255, ErrorMessage = "El nombre no debe exceder 255 caracteres.")]
    [Display(Name = "Nombre del Documento")]
    public string Nombre { get; set; } = null!;

    [StringLength(255, ErrorMessage = "El detalle no debe exceder 255 caracteres.")]
    [Display(Name = "Detalle / Asunto")]
    public string? Detalle { get; set; }

    public int UsuarioCreador { get; set; }

    [Column(TypeName = "datetime")]
    [Display(Name = "Fecha de Creación")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    [StringLength(20)]
    [Display(Name = "Estado")]
    public string Estado { get; set; } = "pendiente";

    [Display(Name = "Avance (%)")]
    public int Avance { get; set; }

    [Display(Name = "Observaciones")]
    public string? Observaciones { get; set; }

    public int? UsuarioActual { get; set; }

    public int? UsuarioAnterior { get; set; }

    [Column(TypeName = "datetime")]
    [Display(Name = "Último Movimiento")]
    public DateTime? FechaUltimoMovimiento { get; set; } = DateTime.Now;

    [InverseProperty("IdDocumentoNavigation")]
    public virtual ICollection<Historial> Historial { get; set; } = new List<Historial>();

    [ForeignKey("UsuarioActual")]
    [InverseProperty("DocumentosUsuarioActualNavigation")]
    public virtual Usuarios? UsuarioActualNavigation { get; set; }

    [ForeignKey("UsuarioAnterior")]
    [InverseProperty("DocumentosUsuarioAnteriorNavigation")]
    public virtual Usuarios? UsuarioAnteriorNavigation { get; set; }

    [ForeignKey("UsuarioCreador")]
    [InverseProperty("DocumentosUsuarioCreadorNavigation")]
    public virtual Usuarios UsuarioCreadorNavigation { get; set; } = null!;
}
