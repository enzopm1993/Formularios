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
    
    public partial class BITACORA_SOLICITUD
    {
        public int IdBitacoraSolicitud { get; set; }
        public int IdSolicitud { get; set; }
        public string Cedula { get; set; }
        public string EstadoSolicitud { get; set; }
        public string Observacion { get; set; }
        public Nullable<System.DateTime> FechaSalida { get; set; }
        public Nullable<System.DateTime> FechaRegreso { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public string CodigoMotivo { get; set; }
        public Nullable<bool> CambioEstado { get; set; }
    }
}
