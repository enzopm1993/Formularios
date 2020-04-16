using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.CALIDAD
{
    public class CabeceraEvaluacionLomosMigasViewModel
    {
        public int IdEvaluacionDeLomosYMigasEnBandejas { get; set; }
        public Nullable<System.DateTime> FechaProduccion { get; set; }
        public Nullable<int> OrdenFabricacion { get; set; }
        public string Cliente { get; set; }
        public Nullable<bool> Lomo { get; set; }
        public Nullable<bool> Miga { get; set; }
        public Nullable<bool> Empaque { get; set; }
        public Nullable<bool> Enlatado { get; set; }
        public Nullable<bool> Pouch { get; set; }
        public string NivelLimpieza { get; set; }
        public string Observacion { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
        public Nullable<bool> EstadoControl { get; set; }
        public string NivelLimpiezaDescripcion { get; set; }
        public string AprobadoPor { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
    }
}