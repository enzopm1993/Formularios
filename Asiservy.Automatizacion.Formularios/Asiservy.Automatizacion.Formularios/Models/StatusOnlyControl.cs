using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Asiservy.Automatizacion.Formularios.Models
{
    [XmlRoot]
    public class StatusOnlyControl
    {
        [XmlElement]
        public string mensaje;
        [XmlElement]
        public string codigo;
        [XmlElement]
        public string dato;
    }
}