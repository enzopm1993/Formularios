using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Empleado
{
    public class EmpleadoViewModel
    {
        [Required(ErrorMessage ="Campo Requerido")]
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string CodLinea { get; set; }
        public string Linea { get; set; }
        [Required(ErrorMessage ="Campo Requerido")]
        public string Turno { get; set; }
        public string EstadoRegistro { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public string UsuarioIngreso { get; set; }
        public string TerminalIngreso { get; set; }

    }
}