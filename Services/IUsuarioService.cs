using AppMvcCorrespondencia.Models;
using AppMvcCorrespondencia.ViewModels;

namespace AppMvcCorrespondencia.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuarios>> ObtenerUsuariosOperativosAsync();
        Task<Usuarios?> ObtenerPorIdAsync(int id);
        Task<Usuarios?> ValidarCredencialesAsync(string username, string password);
        Task<bool> ExisteUsernameAsync(string username, int? excluirIdUsuario = null);
        Task<Usuarios> CrearUsuarioAsync(UsuarioCreateViewModel model);
        Task<bool> ActualizarUsuarioAsync(UsuarioEditViewModel model);
        Task<(bool Exito, string? Error)> EliminarUsuarioAsync(int id);
    }
}
