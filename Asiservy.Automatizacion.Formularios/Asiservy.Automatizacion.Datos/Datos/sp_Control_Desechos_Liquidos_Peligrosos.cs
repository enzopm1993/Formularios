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
    
    public partial class sp_Control_Desechos_Liquidos_Peligrosos
    {
        public int IdDesechosLiquidos { get; set; }
        public System.DateTime FechaMES { get; set; }
        public bool EstadoReporte { get; set; }
        public string AprobadoPor { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
        public byte[] FirmaControl { get; set; }
        public byte[] FirmaAprobacion { get; set; }
        public int IdDesechosLiquidosDetalle { get; set; }
        public System.DateTime FechaDIA { get; set; }
        public decimal Laboratorio { get; set; }
        public string Otros { get; set; }
        public string Observaciones { get; set; }
    }
}
