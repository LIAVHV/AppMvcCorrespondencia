namespace AppMvcCorrespondencia.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalDocumentos { get; set; }
        public int TotalEnvios { get; set; }
        public int TotalRecepciones { get; set; }
        public int Concluidos { get; set; }
        public int Pendientes { get; set; }
    }
}
