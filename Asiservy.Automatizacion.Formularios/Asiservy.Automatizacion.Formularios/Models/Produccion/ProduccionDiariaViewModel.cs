using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Produccion
{
    public class ProduccionDiariaViewModel
    {
        public Nullable<int> SumaFunda { get; set; }
        public Nullable<int> OrdenFabricacion { get; set; }
        public string Producto { get; set; }
        public string TipoLimpieza { get; set; }
        public string Limpieza { get; set; }
        public string Turno { get; set; }
        public string Lote { get; set; }
        public string Textura { get; set; }
        public string Especie { get; set; }
        public string Talla { get; set; }
        public string BARCO { get; set; }
        public string PedidoVenta { get; set; }
        public string Cliente { get; set; }
        public string CodigoSap { get; set; }
    }
}