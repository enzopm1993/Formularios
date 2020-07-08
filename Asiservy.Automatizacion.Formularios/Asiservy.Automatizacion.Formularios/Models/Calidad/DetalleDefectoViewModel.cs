
using System;

namespace Asiservy.Automatizacion.Formularios.Models.CALIDAD
{
    public class DetalleDefectoViewModel
    {
        public int IdParametroDefectoDetalle { get; set; }
        public Nullable<int> IdCabeceraParametro { get; set; }
        public int Defecto { get; set; }
        public string DefectoNombre { get; set; }
        public Nullable<int> Maximo { get; set; }
        public string EstadoRegistro { get; set; }
    }
}