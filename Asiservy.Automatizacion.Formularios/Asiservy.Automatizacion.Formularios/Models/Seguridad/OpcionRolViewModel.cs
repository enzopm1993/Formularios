using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Seguridad
{
    public class OpcionRolViewModel
    {
        public int IdOpcionRol { get; set; }
        public int? IdRol { get; set; }
        public string NombreRol { get; set; }
        public int? IdOpcion { get; set; }
        public string NombreOpcion { get; set; }
        public string Estado { get; set; }
        public string Clase { get; set; }
        public string Formulario { get; set; }

    }
}