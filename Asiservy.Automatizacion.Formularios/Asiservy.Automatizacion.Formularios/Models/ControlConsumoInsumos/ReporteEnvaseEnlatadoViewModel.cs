using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
namespace Asiservy.Automatizacion.Formularios.Models.ControlConsumoInsumos
{
    public class ReporteEnvaseEnlatadoViewModel
    {
        public CONTROL_CONSUMO_INSUMO CabeceraControl { get; set; }
        public List<DetalleCuerpo> DetalleCuerpo { get; set; }
        public  List<DetalleMermasViewModel> DetalleMermas { get; set; }

    }
}