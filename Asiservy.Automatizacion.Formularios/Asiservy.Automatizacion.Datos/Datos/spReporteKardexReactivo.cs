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
    
    public partial class spReporteKardexReactivo
    {
        public int IdKardex { get; set; }
        public System.DateTime Fecha { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string UsuarioIngreso { get; set; }
        public string AprobadoPor { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
        public bool EstadoReporte { get; set; }
        public int IdKardexReactivoDetalle { get; set; }
        public int IdKardexReactivo { get; set; }
        public int IdReactivo { get; set; }
        public string Valor { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    }
}
