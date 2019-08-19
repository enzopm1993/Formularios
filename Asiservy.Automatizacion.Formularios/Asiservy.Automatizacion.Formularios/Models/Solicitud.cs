using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models
{

    public class Solicitud
    {
        public string codigo { get; set; }
        public string fecha { get; set; }
        public string Motivo { get; set; }
        public string Area { get; set; }
        public string Empleado { get; set; }
        public string TipoFiltro { get; set; }
        public bool AprobadoProduccion { get; set; }
    }

}