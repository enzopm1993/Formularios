

namespace Asiservy.Automatizacion.Formularios.Models.Produccion.EntradaYSalidaDeMateriales
{
    public class EntradaSalidaMaterialDetalleViewModel
    {
        public int IdDetalleEntradaSalidaMateriales { get; set; }
        public int Material { get; set; }
        public string MaterialNombre { get; set; }
        public int Ingreso { get; set; }
        public int IdCabeceraEntradaSalidaMaterial { get; set; }
    }
}