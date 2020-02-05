using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.ControlConsumoInsumos
{
    public class DetalleCuerpo
    {
        public string Proveedor { get; set; }
        public string Lote { get; set; }
        public int Bulto { get; set; }
        public DateTime Fecha { get; set; }
        public string Linea { get; set; }
    }
}