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
    
    public partial class DETALLE_HORAS_CONTROL_PESO_CODIFICACION
    {
        public int IdDetalleHorasControlPesoCodificacion { get; set; }
        public Nullable<int> NumeroMuestra { get; set; }
        public Nullable<int> Cantidad { get; set; }
        public Nullable<int> IdDetalleControlPesoCodificacion { get; set; }
        public string EstadoRegistro { get; set; }
        public Nullable<System.DateTime> FechaCreacionLog { get; set; }
        public string UsuarioCreacionLog { get; set; }
        public string TerminalCreacionLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    
        public virtual DETALLE_CONTROL_PESO_CODIFICACION DETALLE_CONTROL_PESO_CODIFICACION { get; set; }
    }
}
