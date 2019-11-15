using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.SolicitudPermiso
{
    public class ParamSolicitud
    {
        public string Identificacion { get; set; }
        public string CodigoMotivo { get; set; }
        public string Observacion { get; set; }
        public string UsuarioIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaRegreso { get; set; }
    }
}