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
    
    public partial class USUARIO_ROL
    {
        public int IdUsuarioRol { get; set; }
        public Nullable<int> IdRol { get; set; }
        public string IdUsuario { get; set; }
        public Nullable<System.DateTime> FechaCreacionlog { get; set; }
        public string UsuarioCreacionlog { get; set; }
        public string TerminalCreacionlog { get; set; }
        public Nullable<System.DateTime> FechaModificacionlog { get; set; }
        public string UsuarioModificacionlog { get; set; }
        public string TerminalModificacionlog { get; set; }
        public string EstadoRegistro { get; set; }
    
        public virtual ROL ROL { get; set; }
    }
}
