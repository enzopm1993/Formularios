using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.ControlCocheLinea
{
    public class ControlCocheLineaViewModel
    {

        [DisplayName("Id")]
        public int IdControlCocheLinea { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Turno { get; set; }
        [DisplayName("Hora Inicio")]
        public DateTime? HoraInicio { get; set; }
        [DisplayName("Hora Final")]
        public DateTime?  HoraFin { get; set; }
        [DisplayName("# Coches")]
        public int Coches { get; set; }
        public string Linea { get; set; }

        [DisplayName("Linea")]
        public string DescripcionLinea { get; set; }
        public string Talla { get; set; }
        public string Observacion { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    }
}