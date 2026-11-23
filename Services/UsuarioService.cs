using AppMvcCorrespondencia.Data;
using AppMvcCorrespondencia.Models;
using AppMvcCorrespondencia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppMvcCorrespondencia.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Usuarios> _passwordHasher;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Usuarios>();
        }

        public async Task<List<Usuarios>> ObtenerUsuariosOperativosAsync()
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Where(x => x.Rol != "admin")
                .OrderBy(x => x.Nombre)
                .ToListAsync();
        }

        public async Task<Usuarios?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
        }

        public async Task<Usuarios?> ValidarCredencialesAsync(string username, string password)
        {
            var usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == username);

            if (usuario == null)
            {
                return null;
            }

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, password);
            return resultado == PasswordVerificationResult.Success || resultado == PasswordVerificationResult.SuccessRehashNeeded
                ? usuario
                : null;
        }

        public async Task<bool> ExisteUsernameAsync(string username, int? excluirIdUsuario = null)
        {
            var query = _context.Usuarios.AsNoTracking().Where(u => u.Username == username);
            if (excluirIdUsuario.HasValue)
            {
                query = query.Where(u => u.IdUsuario != excluirIdUsuario.Value);
            }
            return await query.AnyAsync();
        }

        /// <summary>
        /// Equivalente EF Core al procedimiento almacenado sp_CrearUsuario.
        /// </summary>
        public async Task<Usuarios> CrearUsuarioAsync(UsuarioCreateViewModel model)
        {
            var usuario = new Usuarios
            {
                Nombre = model.Nombre.Trim().ToUpper(),
                Username = model.Username.Trim(),
                Rol = model.Rol,
                Departamento = model.Departamento.Trim(),
                FechaCreacion = DateTime.Now
            };

            usuario.PasswordHash = _passwordHasher.HashPassword(usuario, model.Password);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> ActualizarUsuarioAsync(UsuarioEditViewModel model)
        {
            var usuario = await _context.Usuarios.FindAsync(model.IdUsuario);
            if (usuario == null)
            {
                return false;
            }

            usuario.Nombre = model.Nombre.Trim().ToUpper();
            usuario.Username = model.Username.Trim();
            usuario.Rol = model.Rol;
            usuario.Departamento = model.Departamento.Trim();

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                usuario.PasswordHash = _passwordHasher.HashPassword(usuario, model.Password);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool Exito, string? Error)> EliminarUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return (false, "El usuario no existe.");
            }

            try
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message?.ToLower().Contains("reference constraint") == true ||
                    ex.InnerException?.Message?.ToLower().Contains("fk_") == true)
                {
                    return (false, "No se puede eliminar el usuario porque tiene documentos o historial relacionados.");
                }
                return (false, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
