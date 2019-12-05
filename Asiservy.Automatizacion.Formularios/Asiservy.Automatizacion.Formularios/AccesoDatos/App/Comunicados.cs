using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.App
{
    public class Comunicados
    {

        public int id { get; set; }
        public string Asunto { get; set; }
        public string Contenido { get; set; }
        public string fechaDesde { get; set; }
        public string fechaHasta { get; set; }
        public bool Estado { get; set; }
        public string Prioridad { get; set; }
        public int idCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public string ColorCategoria { get; set; }
        public string ColorPrioridad { get; set; }
        public string EstadoPublicacion { get; set; }
    }
}