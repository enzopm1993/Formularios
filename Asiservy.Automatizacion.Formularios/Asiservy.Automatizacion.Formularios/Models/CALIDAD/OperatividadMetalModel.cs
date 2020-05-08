using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.CALIDAD
{
    public class OperatividadMetalModel
    {
        public int IdOperatividadMetal { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Pcc { get; set; }
        public bool Lomos { get; set; }
        public bool Latas { get; set; }
        public decimal Ferroso { get; set; }
        public decimal NoFerroso { get; set; }
        public decimal AceroInoxidable { get; set; }
        public string DetectorMetal { get; set; }
        public string Observacion { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string UsuarioAprobacion { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
        public string TerminalModificacionLog { get; set; }

    }
}