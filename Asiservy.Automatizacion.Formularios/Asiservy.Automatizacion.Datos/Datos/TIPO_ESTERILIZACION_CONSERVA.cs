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
    
    public partial class TIPO_ESTERILIZACION_CONSERVA
    {
        public int IdTipoControlEsterilizacionConserva { get; set; }
        public string Tipo { get; set; }
        public Nullable<decimal> Panel { get; set; }
        public Nullable<decimal> Chart { get; set; }
        public Nullable<decimal> TermometroDigital { get; set; }
        public Nullable<int> PresionManometro { get; set; }
        public Nullable<System.DateTime> HoraChequeo { get; set; }
        public Nullable<int> M3H1 { get; set; }
        public Nullable<int> M3H2 { get; set; }
        public int IdDetalleControlEsterilizacion { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    
        public virtual DETALLE_CONTROL_ESTERILIZACION_CONSERVA DETALLE_CONTROL_ESTERILIZACION_CONSERVA { get; set; }
    }
}
