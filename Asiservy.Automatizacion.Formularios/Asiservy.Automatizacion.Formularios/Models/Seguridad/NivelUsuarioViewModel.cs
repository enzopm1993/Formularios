using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Seguridad
{
    public class NivelUsuarioViewModel
    {
        public int IdNivelUsuario { get; set; }
        [Required(ErrorMessage ="Campo Requerido")]
        public string IdUsuario { get; set; }
        public string Usuario { get; set; }
        [Required(ErrorMessage ="Campo Requerido")]
        public Nullable<int> Nivel { get; set; }           
        public string EstadoRegistro { get; set; }
    }
}