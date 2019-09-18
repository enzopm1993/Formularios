using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Empleado
{
    public class EmpleadoEsferoViewModel
    {
        [Required(ErrorMessage ="Campo Requerido")]
        public string Cedula { get; set; }
        public string Nombre { get; set; }      
        [Required(ErrorMessage ="Campo Requerido")]
        public string NumeroEsfero { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
    }
}