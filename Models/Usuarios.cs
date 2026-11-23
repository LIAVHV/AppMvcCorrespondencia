using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMvcCorrespondencia.Models;

[Index("Username", Name = "UQ__Usuarios__536C85E40B63AF06", IsUnique = true)]
public partial class Usuarios
{
    [Key]
    public int IdUsuario { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no debe exceder 100 caracteres.")]
    [Display(Name = "Nombre Completo")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El usuario es obligatorio.")]
    [StringLength(50, ErrorMessage = "El usuario no debe exceder 50 caracteres.")]
    [Display(Name = "Usuario")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(255)]
    [Display(Name = "Contraseña")]
    public string PasswordHash { get; set; } = null!;

    [Required(ErrorMessage = "El rol es obligatorio.")]
    [StringLength(15)]
    [Display(Name = "Rol")]
    public string Rol { get; set; } = null!;

    [Required(ErrorMessage = "El departamento es obligatorio.")]
    [StringLength(100, ErrorMessage = "El departamento no debe exceder 100 caracteres.")]
    [Display(Name = "Departamento")]
    public string Departamento { get; set; } = null!;

    [Column(TypeName = "datetime")]
    [Display(Name = "Fecha de Creación")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    [InverseProperty("UsuarioActualNavigation")]
    public virtual ICollection<Documentos> DocumentosUsuarioActualNavigation { get; set; } = new List<Documentos>();

    [InverseProperty("UsuarioAnteriorNavigation")]
    public virtual ICollection<Documentos> DocumentosUsuarioAnteriorNavigation { get; set; } = new List<Documentos>();

    [InverseProperty("UsuarioCreadorNavigation")]
    public virtual ICollection<Documentos> DocumentosUsuarioCreadorNavigation { get; set; } = new List<Documentos>();

    [InverseProperty("UsuarioDestinoNavigation")]
    public virtual ICollection<Historial> HistorialUsuarioDestinoNavigation { get; set; } = new List<Historial>();

    [InverseProperty("UsuarioOrigenNavigation")]
    public virtual ICollection<Historial> HistorialUsuarioOrigenNavigation { get; set; } = new List<Historial>();
}
