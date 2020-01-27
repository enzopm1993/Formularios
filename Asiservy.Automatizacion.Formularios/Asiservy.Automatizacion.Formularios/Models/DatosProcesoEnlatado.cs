using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models
{
    public class DatosProcesoEnlatado
    {
        public int TiempoMuertoProceso { get; set; }
        public int TiempoMuertoMantenimiento { get; set; }
        public int CajasHora { get; set; }
        public int CajasMinuto { get; set; }
        public int TotalCajasHora { get; set; }
        public decimal GrsXLata { get; set; }
        public int Personal { get; set; }
    }
}