using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.CALIDAD
{
    public class MantenimientoTipoDescongeladoModel
    {
        public int IdTipoMonitoreo { get; set; }
        public string Descripcion { get; set; }
        public string Abreviatura { get; set; }
        public string Color { get; set; }
        public bool TemperaturaAgua { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    }
}