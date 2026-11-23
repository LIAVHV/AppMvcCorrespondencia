using AppMvcCorrespondencia.Models;
using AppMvcCorrespondencia.ViewModels;

namespace AppMvcCorrespondencia.Services
{
    public interface ICorrespondenciaService
    {
        Task<DashboardViewModel> ObtenerMetricasDashboardAsync(int idUsuario);
        Task<List<Documentos>> ObtenerDocumentosDisponiblesParaEnviarORecibirAsync(int idUsuario);
        Task<List<Documentos>> ObtenerDocumentosParaSeguimientoAsync(int idUsuario);
        Task<List<Usuarios>> ObtenerDestinatariosDisponiblesAsync(int idUsuarioActual);
        Task<Documentos> CrearDocumentoAsync(string nombre, string? detalle, int usuarioCreador);
        Task<(bool Exito, string? Error)> EnviarDocumentoAsync(int idDocumento, int usuarioOrigen, int usuarioDestino);
        Task<(bool Exito, string? Error)> RecibirDocumentoAsync(int idDocumento, int usuarioReceptor, string observaciones, int avance, string estadoDocumento);
        Task<List<HistorialItemViewModel>> ConsultarHistorialAsync(int idDocumento);
    }
}
