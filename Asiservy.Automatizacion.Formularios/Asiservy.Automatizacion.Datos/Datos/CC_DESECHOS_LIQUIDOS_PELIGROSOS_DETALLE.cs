//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Asiservy.Automatizacion.Datos.Datos
{
    using System;
    using System.Collections.Generic;
    
    public partial class CC_DESECHOS_LIQUIDOS_PELIGROSOS_DETALLE
    {
        public int IdDesechosLiquidosDetalle { get; set; }
        public int IdDesechosLiquidos { get; set; }
        public System.DateTime FechaDIA { get; set; }
        public decimal Laboratorio { get; set; }
        public string Otros { get; set; }
        public string Observaciones { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    
        public virtual CC_DESECHOS_LIQUIDOS_PELIGROSOS CC_DESECHOS_LIQUIDOS_PELIGROSOS { get; set; }
    }
}
