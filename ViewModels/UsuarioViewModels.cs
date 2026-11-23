using System.ComponentModel.DataAnnotations;

namespace AppMvcCorrespondencia.ViewModels
{
    public class UsuarioCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no debe exceder 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El usuario no debe exceder 50 caracteres.")]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "La contraseña debe tener entre 3 y 255 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(15)]
        [Display(Name = "Rol")]
        public string Rol { get; set; } = "usuario";

        [Required(ErrorMessage = "El departamento es obligatorio.")]
        [StringLength(100, ErrorMessage = "El departamento no debe exceder 100 caracteres.")]
        [Display(Name = "Departamento")]
        public string Departamento { get; set; } = string.Empty;
    }

    public class UsuarioEditViewModel
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no debe exceder 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El usuario no debe exceder 50 caracteres.")]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [StringLength(255)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña (opcional si no desea cambiarla)")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(15)]
        [Display(Name = "Rol")]
        public string Rol { get; set; } = "usuario";

        [Required(ErrorMessage = "El departamento es obligatorio.")]
        [StringLength(100, ErrorMessage = "El departamento no debe exceder 100 caracteres.")]
        [Display(Name = "Departamento")]
        public string Departamento { get; set; } = string.Empty;
    }
}
