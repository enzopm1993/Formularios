using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.Models.Asistencia
{
    public class ControlDeAsistenciaViewModel
    {
        public List<sp_ConsultaAsistenciaDiaria> ControlAsistencia { get; set; }
        public List<CONTROL_CUCHILLO> ControlDeCuchillos { get; set; }
    }
}