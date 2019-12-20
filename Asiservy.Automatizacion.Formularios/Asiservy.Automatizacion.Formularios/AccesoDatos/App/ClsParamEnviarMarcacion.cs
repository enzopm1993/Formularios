using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.App
{
    public class ClsParamEnviarMarcacion
    {
        public string cedula { get; set; }
        public string diaMarcacion { get; set; }
        public string horaMarcacionCorrecta { get; set; }
        public string tipoMarcacion { get; set; }
        public int idRegistro { get; set; }
        public string usuarioActualiza { get; set; }
    }
}