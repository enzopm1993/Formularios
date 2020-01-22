using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Seguridad
{
    public class UsuarioRolViewModel
    {
        public int IdUsuarioRol { get; set; }
        public Nullable<int> IdRol { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public Nullable<int> IdRol2 { get; set; }
        public string Rol { get; set; }
        public string IdUsuario { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string IdUsuario2 { get; set; }
        public string Usuario { get; set; }
        public Nullable<System.DateTime> FechaCreacionlog { get; set; }
        public string UsuarioCreacionlog { get; set; }
        public string TerminalCreacionlog { get; set; }
        public Nullable<System.DateTime> FechaModificacionlog { get; set; }
        public string UsuarioModificacionlog { get; set; }
        public string TerminalModificacionlog { get; set; }
        public string EstadoRegistro { get; set; }

    }
}