using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models
{
    public class BitacoraSolicitud
    {
        public int idBitacoraSolicitud { get; set; }
        public int idSolicitud { get; set; }
        public string Cedula { get; set; }
        public string Nombres { get; set; }
        public string CodEstadoSolicitud { get; set; }
        public string EstadoSolicitud { get; set; }
        public string Linea { get; set; }
        public string Observacion { get; set; }
        public DateTime? FechaSalida { get; set; }
        public DateTime? FechaRegreso { get; set; }
        public DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }

    }
}