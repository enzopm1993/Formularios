using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Asistencia
{
    public class BitacoraCambioPersonalModelView
    {
        [DisplayName("Id")]
        public int IdBitacoraCambioPersonal { get; set; }

        public string Cedula { get; set; }
        public string Tipo { get; set; }
        public string CodLinea { get; set; }
        public string Linea { get; set; }
        public string CodArea { get; set; }
        public string Area { get; set; }
        public string CodCargo { get; set; }
        public string Cargo { get; set; }
        public System.DateTime FechaDesde { get; set; }
        public System.DateTime FechaHasta { get; set; }

        [DisplayName("Fecha Ingreso")]
        public Nullable<System.DateTime> FechaIngresoLog { get; set; }

        [DisplayName("Usuario Ingreso")]
        public string UsuarioIngresoLog { get; set; }

        [DisplayName("Terminal Ingreso")]
        public string TerminalIngresoLog { get; set; }
    }
}