using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMvcCorrespondencia.Models;

public partial class Historial
{
    [Key]
    public int IdHistorial { get; set; }

    public int IdDocumento { get; set; }

    public int? UsuarioOrigen { get; set; }

    public int? UsuarioDestino { get; set; }

    [StringLength(50)]
    public string? Accion { get; set; }

    [StringLength(250)]
    public string? Estado { get; set; }

    public int? Avance { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaMovimiento { get; set; } = DateTime.Now;

    [ForeignKey("IdDocumento")]
    [InverseProperty("Historial")]
    public virtual Documentos IdDocumentoNavigation { get; set; } = null!;

    [ForeignKey("UsuarioDestino")]
    [InverseProperty("HistorialUsuarioDestinoNavigation")]
    public virtual Usuarios? UsuarioDestinoNavigation { get; set; }

    [ForeignKey("UsuarioOrigen")]
    [InverseProperty("HistorialUsuarioOrigenNavigation")]
    public virtual Usuarios? UsuarioOrigenNavigation { get; set; }
}
