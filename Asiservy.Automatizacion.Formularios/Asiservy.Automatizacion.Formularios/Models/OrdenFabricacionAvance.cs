using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models
{
    public class OrdenFabricacionAvance
    {
        public string OrdenFabricacion { get; set; }
        public string Lote { get; set; }
        public string Producto { get; set; }
        public string Especie { get; set; }
        public string Limpieza { get; set; }
        public string Talla { get; set; }
        public string Peso { get; set; }
        public string Piezas { get; set; }
        public string Promedio { get; set; }
        public string Barco { get; set; }
        public decimal Lomos { get; set; }
        public decimal Migas { get; set; }
        public decimal Recuperado { get; set; }

    }
}