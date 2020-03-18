using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Memorandum
{
    public class ModelMemorandum
    {
        public int IdMemorandum { get; set; }
        public string Titulo { get; set; }
        public string Plantilla { get; set; }
        public bool Estado { get; set; }
        public string userEnvia { get; set; }
        public string terminalEnvia { get; set; }

    }
}