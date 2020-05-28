using System;


namespace Asiservy.Automatizacion.Formularios.Models.CALIDAD
{
    public class ReporteFotosEvaluacionLomosyMigasViewModel
    {
        public int IdFoto { get; set; }
        public string Novedad { get; set; }
        public string Imagen { get; set; }
        public int Rotacion { get; set; }
        public TimeSpan? Hora { get; set; }
    }
}