using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.CALIDAD
{
    public class OperatividadMetalDetalleModel
    {
        public int IdOperatividadMetalDetalle { get; set; }
        public int IdOperatividadMetal { get; set; }
        public System.DateTime Hora { get; set; }
        public bool Ferroso { get; set; }
        public bool NoFerroso { get; set; }
        public bool AceroInoxidable { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    }
}