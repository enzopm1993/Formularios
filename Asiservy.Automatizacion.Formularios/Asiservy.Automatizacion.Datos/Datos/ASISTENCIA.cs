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
    
    public partial class ASISTENCIA
    {
        public int IdAsistencia { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public string Cedula { get; set; }
        public string Nombres { get; set; }
        public string EstadoAsistencia { get; set; }
        public string Observacion { get; set; }
        public Nullable<System.TimeSpan> Hora { get; set; }
        public string EstadoRegistro { get; set; }
        public string Linea { get; set; }
        public string Turno { get; set; }
        public Nullable<System.DateTime> FechaCreacionLog { get; set; }
        public string UsuarioCreacionLog { get; set; }
        public string TerminalCreacionLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    }
}
