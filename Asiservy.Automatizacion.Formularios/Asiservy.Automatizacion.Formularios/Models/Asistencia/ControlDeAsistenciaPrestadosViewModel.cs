using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.Models.Asistencia
{
    public class ControlDeAsistenciaPrestadosViewModel
    {
        public class Cuchillos
        {
            public int? Id { get; set; }
            public int? Numero { get; set; }
        }
        public List<sp_ConsultaAsistenciaDiariaPersonalMovido> ControlAsistencia { get; set; }
        //public List<CONTROL_CUCHILLO> ControlDeCuchillos { get; set; }
        public IEnumerable<ControlCuchilloViewModel> ControlDeCuchillos { get; set; }
        public List<Cuchillos> CuchillosBlancosSobrantes { get; set; }
        public List<Cuchillos> CuchillosNegrosSobrantes { get; set; }
        public List<Cuchillos> CuchillosRojosSobrantes { get; set; }
    }
}