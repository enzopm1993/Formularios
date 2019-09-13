using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.Models.Asistencia
{
    public class ControlDeAsistenciaViewModel
    {
        public List<ASISTENCIA> ControlAsistencia { get; set; }
        public List<CONTROL_CUCHILLO> ControlDeCuchillos { get; set; }
    }
}