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
    
    public partial class CONTROL_CUCHILLO
    {
        public int IdControlCuchillo { get; set; }
        public string Cedula { get; set; }
        public Nullable<int> CuchilloBlanco { get; set; }
        public Nullable<int> CuchilloRojo { get; set; }
        public Nullable<int> CuchilloNegro { get; set; }
        public System.DateTime Fecha { get; set; }
        public string EstadoCuchillo { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
        public string Tipo { get; set; }
        public string Observacion { get; set; }
    }
}
