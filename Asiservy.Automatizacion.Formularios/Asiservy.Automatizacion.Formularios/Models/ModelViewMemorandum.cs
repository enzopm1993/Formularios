using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Memorandum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models
{
    public class ModelViewMemorandum
    {
        public List<Controllers.ClsKeyValue> TagsPlantilla { get; set; }
        public PlantillasMemorandum Memorandum { get; set; }
    }
    
}