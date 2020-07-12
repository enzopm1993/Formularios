

namespace Asiservy.Automatizacion.Formularios.Models.CALIDAD
{
    public class ParametroDefectoViewModel
    {
        public int IdParametroDefecto { get; set; }
        public string Formulario { get; set; }
        public string FormularioNombre { get; set; }
        public string Tipo { get; set; }
        public string TipoNombre { get; set; }
        public string NivelLimpieza { get; set; }
        public string NivelLimpiezaNombre { get; set; }
        public string ColorDentroDeRango { get; set; }
        public string ColorDentroDeRangoNombre { get; set; }
        public string ColorFueraDeRango { get; set; }
        public string ColorFueraDeRangoNombre { get; set; }
        public string EstadoRegistro { get; set; }
        public int Maximo { get; set; }
    }
}