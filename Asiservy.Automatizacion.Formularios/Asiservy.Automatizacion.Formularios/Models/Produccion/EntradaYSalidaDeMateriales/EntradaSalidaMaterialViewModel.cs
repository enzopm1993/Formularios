using System;


namespace Asiservy.Automatizacion.Formularios.Models.Produccion.EntradaYSalidaDeMateriales
{
    public class EntradaSalidaMaterialViewModel
    {
        public int IdControlEntradaSalidaMateriales { get; set; }
        public System.DateTime Fecha { get; set; }
        public string CodLinea { get; set; }
        public string Linea { get; set; }
        public string CodTurno { get; set; }
        public string Turno { get; set; }
        public string Observacion { get; set; }
        public bool EstadoControl { get; set; }
        public string AprobadoPor { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    }
}