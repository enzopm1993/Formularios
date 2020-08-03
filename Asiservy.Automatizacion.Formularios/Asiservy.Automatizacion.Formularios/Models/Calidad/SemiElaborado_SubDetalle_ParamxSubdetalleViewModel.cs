using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.CALIDAD
{
    public class SemiElaborado_SubDetalle_ParamxSubdetalleViewModel
    {
        public int IdSubdetalle { get; set; }
        public int? NMuestra { get; set; }
        public string CodTipoProducto { get; set; }
        public string TipoProducto { get; set; }
        public string CodArea { get; set; }
        public string Area { get; set; }
        public int IdParametro { get; set; }
        public string Parametro { get; set; }
        public decimal? Cantidad { get; set; }
        public int IdTipoxParametro { get; set; }
        public decimal? Mascara { get; set; }
    }
}