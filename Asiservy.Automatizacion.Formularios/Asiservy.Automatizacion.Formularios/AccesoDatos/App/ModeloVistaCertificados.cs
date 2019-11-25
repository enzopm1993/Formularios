using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.App
{
    public class ModeloVistaCertificados
    {
        public List<Certificados> Certificados { get; set; }
        public List<Controllers.ClsKeyValue> TagsPlantilla { get; set; }
    }
}