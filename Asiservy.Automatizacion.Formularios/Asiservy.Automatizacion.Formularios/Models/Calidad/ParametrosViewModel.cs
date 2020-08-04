using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.CALIDAD
{
    public class ParametrosViewModel
    {
        public int ParametroLaboratorio { get; set; }
        public string CodArea { get; set; }
        public bool? CalcMinMax { get; set; }
        public string NombreParametro { get; set; }
    }
}