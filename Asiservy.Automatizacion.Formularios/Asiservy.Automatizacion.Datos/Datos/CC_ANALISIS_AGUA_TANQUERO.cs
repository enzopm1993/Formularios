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
    
    public partial class CC_ANALISIS_AGUA_TANQUERO
    {
        public int IdAnalisisAguaTanquero { get; set; }
        public int IdAnalisisAguaTanqueroControl { get; set; }
        public System.DateTime Hora { get; set; }
        public string Placa { get; set; }
        public int Std { get; set; }
        public int Dureza { get; set; }
        public decimal Ph { get; set; }
        public bool Olor { get; set; }
        public bool Color { get; set; }
        public bool Sabor { get; set; }
        public string Destino { get; set; }
        public string Observacion { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    
        public virtual CC_ANALISIS_AGUA_TANQUERO_CONTROL CC_ANALISIS_AGUA_TANQUERO_CONTROL { get; set; }
    }
}