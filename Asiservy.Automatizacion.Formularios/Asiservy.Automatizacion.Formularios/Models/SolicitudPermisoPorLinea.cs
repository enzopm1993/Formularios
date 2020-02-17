using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models
{
    public class SolicitudPermisoPorLinea
    {
        public int ID { get; set; }
        public string NOMBRES { get; set; }
        public string MOTIVO { get; set; }
        public string OBSERVACION { get; set; }
        public DateTime FECHA_DESDE { get; set; }
        public DateTime FECHA_HASTA { get; set; }
        public string ESTADO { get; set; }
        public string USUARIO_CREA { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public string USUARIO_APRUEBA { get; set; }
    }
}