using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models
{
    public class ClasificadorGenerico
    {
        public int codigo { get; set; }
        public string descripcion { get; set; }

        public string Especie { get; set; }
        public string Barco { get; set; }//agregado 31/3/2020
    }
}