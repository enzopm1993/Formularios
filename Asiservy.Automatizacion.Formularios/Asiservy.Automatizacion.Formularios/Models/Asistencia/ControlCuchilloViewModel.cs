using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Asistencia
{
    public class ControlCuchilloViewModel
    {
        public int IdControlCuchillo { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> CuchilloBlanco { get; set; }
        public string ValidaBlanco { get; set; }
        public Nullable<int> CuchilloRojo { get; set; }
        public string ValidaRojo { get; set; }
        public Nullable<int> CuchilloNegro { get; set; }
        public string ValidaNegro { get; set; }
        public System.DateTime Fecha { get; set; }
        public string EstadoCuchillo { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
        public string Observacion { get; set; }

    }
}