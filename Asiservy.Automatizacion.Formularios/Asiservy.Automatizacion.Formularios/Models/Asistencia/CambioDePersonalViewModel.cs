using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
namespace Asiservy.Automatizacion.Formularios.Models.Asistencia
{
    public class CambioDePersonalViewModel
    {
        public List<CAMBIO_PERSONAL> Personal { get; set; }
        public string Linea { get; set; }
        public string Area { get; set; }
    }
}