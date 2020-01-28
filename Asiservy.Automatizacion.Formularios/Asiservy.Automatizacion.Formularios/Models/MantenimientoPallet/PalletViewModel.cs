using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.MantenimientoPallet
{
    public class PalletViewModel
    {
        public int IdPallet { get; set; }
        public string IdProveedor { get; set; }
        public string Proveedor { get; set; }
        public Nullable<int> Numero_Pallet { get; set; }
        public string Envase { get; set; }
        public Nullable<int> Lamina { get; set; }
        public Nullable<int> Unidades { get; set; }
        public string EstadoRegistro { get; set; }
        public Nullable<System.DateTime> FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    }
}