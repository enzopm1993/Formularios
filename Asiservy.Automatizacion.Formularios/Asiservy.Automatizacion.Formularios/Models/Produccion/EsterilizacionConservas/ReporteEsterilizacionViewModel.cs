using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Produccion.EsterilizacionConservas
{
    public class ReporteEsterilizacionViewModel
    {
        public List<spReporteEsterilizacionDetalle> ListDetalleReporte { get; set; }
        public List<COCHE_AUTOCLAVE_DETALLE> ListDetalleCoches { get; set; }
        public List<TIPO_ESTERILIZACION_CONSERVA> ListTipoEsterilizacion { get; set; }
            
    }
}