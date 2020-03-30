using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Produccion.ProductoPouchCuarentena
{
    public class ReporteProductoPouchCuarentenaViewModel
    {
        public List<spReporteProdPouchCuarentenaSubDetalle> ListaSubDetalle { get; set; }
        public List<spReporteProdPouchCuarentenaDetalle> ListaReporteDetalle { get; set; }
        
    }
}