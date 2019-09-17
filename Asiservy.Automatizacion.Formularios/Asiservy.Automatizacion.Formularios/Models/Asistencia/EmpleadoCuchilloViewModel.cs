using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Asistencia
{
    public class EmpleadoCuchilloViewModel
    {
        public int IdEmpleadoCuchillo { get; set; }
        [Required(ErrorMessage ="Campo Requerido")]
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        [Required(ErrorMessage ="Campo Requerido")]
        public Nullable<int> CuchilloBlanco { get; set; }
        public Nullable<int> CuchilloRojo { get; set; }
        public Nullable<int> CuchilloNegro { get; set; }
        public Nullable<System.DateTime> FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
        public string EstadoRegistro { get; set; }

    }
}