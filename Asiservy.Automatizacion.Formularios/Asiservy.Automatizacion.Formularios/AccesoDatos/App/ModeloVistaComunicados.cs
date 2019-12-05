using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.App
{
    public class ModeloVistaComunicados
    {
        public List<Comunicados> Comunicados { get; set; }
        public List<Controllers.ClsKeyValue> Categorias { get; set; }
    }
}