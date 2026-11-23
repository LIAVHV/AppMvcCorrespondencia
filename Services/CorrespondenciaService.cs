using AppMvcCorrespondencia.Data;
using AppMvcCorrespondencia.Models;
using AppMvcCorrespondencia.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AppMvcCorrespondencia.Services
{
    public class CorrespondenciaService : ICorrespondenciaService
    {
        private readonly ApplicationDbContext _context;

        public CorrespondenciaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> ObtenerMetricasDashboardAsync(int idUsuario)
        {
            return new DashboardViewModel
            {
                Pendientes = await _context.Documentos
                    .AsNoTracking()
                    .CountAsync(x => x.UsuarioActual == idUsuario && x.Estado == "pendiente"),
                TotalDocumentos = await _context.Documentos
                    .AsNoTracking()
                    .CountAsync(x => x.UsuarioCreador == idUsuario),
                TotalEnvios = await _context.Historial
                    .AsNoTracking()
                    .CountAsync(x => x.UsuarioOrigen == idUsuario && x.Accion == "enviado"),
                TotalRecepciones = await _context.Historial
                    .AsNoTracking()
                    .CountAsync(x => x.UsuarioDestino == idUsuario && x.Accion == "recibido"),
                Concluidos = await _context.Documentos
                    .AsNoTracking()
                    .CountAsync(x => x.UsuarioCreador == idUsuario && x.Estado == "concluido")
            };
        }

        public async Task<List<Documentos>> ObtenerDocumentosDisponiblesParaEnviarORecibirAsync(int idUsuario)
        {
            return await _context.Documentos
                .AsNoTracking()
                .Where(x => x.UsuarioActual == idUsuario && x.Estado != "concluido")
                .OrderByDescending(x => x.FechaUltimoMovimiento ?? x.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<Documentos>> ObtenerDocumentosParaSeguimientoAsync(int idUsuario)
        {
            return await _context.Documentos
                .AsNoTracking()
                .Where(x => x.UsuarioActual == idUsuario || x.UsuarioCreador == idUsuario)
                .OrderByDescending(x => x.FechaUltimoMovimiento ?? x.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<Usuarios>> ObtenerDestinatariosDisponiblesAsync(int idUsuarioActual)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Where(x => x.Rol == "usuario" && x.IdUsuario != idUsuarioActual)
                .OrderBy(x => x.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Equivalente EF Core al procedimiento almacenado sp_CrearDocumento.
        /// </summary>
        public async Task<Documentos> CrearDocumentoAsync(string nombre, string? detalle, int usuarioCreador)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var ahora = DateTime.Now;
            var nombreUsuario = await _context.Usuarios
                .AsNoTracking()
                .Where(u => u.IdUsuario == usuarioCreador)
                .Select(u => u.Nombre)
                .FirstOrDefaultAsync() ?? string.Empty;

            var documento = new Documentos
            {
                Nombre = nombre.Trim(),
                Detalle = detalle ?? string.Empty,
                UsuarioCreador = usuarioCreador,
                FechaCreacion = ahora,
                Estado = "pendiente",
                Avance = 0,
                UsuarioActual = usuarioCreador,
                FechaUltimoMovimiento = ahora
            };

            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();

            var historial = new Historial
            {
                IdDocumento = documento.IdDocumento,
                UsuarioOrigen = null,
                UsuarioDestino = usuarioCreador,
                Accion = "creado",
                Estado = $"Documento creado por {nombreUsuario}",
                Avance = 0,
                FechaMovimiento = ahora
            };

            _context.Historial.Add(historial);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return documento;
        }

        /// <summary>
        /// Equivalente EF Core al procedimiento almacenado sp_EnviarDocumento.
        /// </summary>
        public async Task<(bool Exito, string? Error)> EnviarDocumentoAsync(int idDocumento, int usuarioOrigen, int usuarioDestino)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var documento = await _context.Documentos.FirstOrDefaultAsync(d => d.IdDocumento == idDocumento);
            if (documento == null)
            {
                return (false, "El documento seleccionado no existe.");
            }

            if (documento.UsuarioActual != usuarioOrigen || documento.Estado == "concluido")
            {
                return (false, "El documento no está disponible para ser enviado por el usuario actual.");
            }

            var nombreUsuarioDestino = await _context.Usuarios
                .AsNoTracking()
                .Where(u => u.IdUsuario == usuarioDestino)
                .Select(u => u.Nombre)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(nombreUsuarioDestino))
            {
                return (false, "El destinatario seleccionado no existe.");
            }

            var ahora = DateTime.Now;

            var historial = new Historial
            {
                IdDocumento = idDocumento,
                UsuarioOrigen = usuarioOrigen,
                UsuarioDestino = usuarioDestino,
                Accion = "enviado",
                Estado = $"Documento enviado a {nombreUsuarioDestino}",
                Avance = 0,
                FechaMovimiento = ahora
            };

            _context.Historial.Add(historial);

            documento.UsuarioAnterior = usuarioOrigen;
            documento.UsuarioActual = usuarioDestino;
            documento.Estado = "pendiente";
            documento.FechaUltimoMovimiento = ahora;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return (true, null);
        }

        /// <summary>
        /// Equivalente EF Core al procedimiento almacenado sp_RecibirDocumento.
        /// </summary>
        public async Task<(bool Exito, string? Error)> RecibirDocumentoAsync(
            int idDocumento,
            int usuarioReceptor,
            string observaciones,
            int avance,
            string estadoDocumento)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var documento = await _context.Documentos.FirstOrDefaultAsync(d => d.IdDocumento == idDocumento);
            if (documento == null)
            {
                return (false, "El documento seleccionado no existe.");
            }

            if (documento.UsuarioActual != usuarioReceptor || documento.Estado == "concluido")
            {
                return (false, "El documento no se encuentra asignado a usted o ya fue concluido.");
            }

            // Consistencia de regla de negocio: si avance == 100, estado debe ser concluido
            var estadoNormalizado = avance >= 100 ? "concluido" : estadoDocumento;
            var usuarioAnterior = documento.UsuarioAnterior;
            var ahora = DateTime.Now;

            var historial = new Historial
            {
                IdDocumento = idDocumento,
                UsuarioOrigen = usuarioAnterior,
                UsuarioDestino = usuarioReceptor,
                Accion = "recibido",
                Estado = observaciones,
                Avance = avance,
                FechaMovimiento = ahora
            };

            _context.Historial.Add(historial);

            documento.Estado = estadoNormalizado;
            documento.Avance = avance;
            documento.Observaciones = observaciones;
            documento.UsuarioAnterior = usuarioAnterior;
            documento.UsuarioActual = usuarioReceptor;
            documento.FechaUltimoMovimiento = ahora;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return (true, null);
        }

        /// <summary>
        /// Equivalente EF Core al procedimiento almacenado sp_ConsultarHistorial.
        /// </summary>
        public async Task<List<HistorialItemViewModel>> ConsultarHistorialAsync(int idDocumento)
        {
            return await _context.Historial
                .AsNoTracking()
                .Include(h => h.IdDocumentoNavigation)
                .Include(h => h.UsuarioOrigenNavigation)
                .Include(h => h.UsuarioDestinoNavigation)
                .Where(h => h.IdDocumento == idDocumento)
                .OrderBy(h => h.FechaMovimiento)
                .Select(h => new HistorialItemViewModel
                {
                    FechaMovimiento = h.FechaMovimiento,
                    Documento = h.IdDocumentoNavigation.Nombre,
                    Accion = h.Accion,
                    Estado = h.IdDocumentoNavigation.Estado,
                    Observaciones = h.Estado,
                    Avance = h.Avance ?? 0,
                    UsuarioOrigenNombre = h.UsuarioOrigenNavigation != null ? h.UsuarioOrigenNavigation.Nombre : null,
                    UsuarioDestinoNombre = h.UsuarioDestinoNavigation != null ? h.UsuarioDestinoNavigation.Nombre : null
                })
                .ToListAsync();
        }
    }
}
