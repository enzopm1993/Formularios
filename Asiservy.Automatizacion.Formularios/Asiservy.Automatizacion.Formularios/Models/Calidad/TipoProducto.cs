using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.CALIDAD
{
    public class TipoProducto
    {
     
        public int IdTipoAnalisisQuimicoProductoSe { get; set; }
        public string TipoProductoNombre { get; set; }
        public int? OrdenFabricacion { get; set; }
        public string Lote { get; set; }


    }
}