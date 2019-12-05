using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models
{
    public class RespuestaGeneral
    {
        public int Codigo { get; set; }
        public bool Respuesta { get; set; }
        public string Mensaje { get; set; }
        public string Observacion { get; set; }
        public string Descripcion { get; set; }
    }
}