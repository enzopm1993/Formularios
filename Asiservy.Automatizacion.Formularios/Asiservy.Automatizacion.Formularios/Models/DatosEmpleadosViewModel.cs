using Asiservy.Automatizacion.Formularios.AccesoDatos.Nomina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models
{
    public class DatosEmpleadosViewModel
    {
        public ClsEmpleado DatosEmpleado { get; set; }
        public int idSolicitud { get; set; }
        public string MensajeMuestra { get; set; }
        
    }
}