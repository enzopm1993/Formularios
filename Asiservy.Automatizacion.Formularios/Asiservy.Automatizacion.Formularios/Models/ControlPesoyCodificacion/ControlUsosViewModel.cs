using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.ControlPesoyCodificacion
{
    public class ControlUsosViewModel
    {
        public int IdDescripcionUso { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int? Cantidad { get; set; }
    }
}