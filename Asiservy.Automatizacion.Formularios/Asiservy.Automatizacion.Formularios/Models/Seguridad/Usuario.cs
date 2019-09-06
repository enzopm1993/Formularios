using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Seguridad
{
    public class Usuario
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        //public string Nombre { get; set; }
        public string UserName { get; set; }
        public string Mensaje { get; set; }
        public bool Estado { get; set; }
    }
}