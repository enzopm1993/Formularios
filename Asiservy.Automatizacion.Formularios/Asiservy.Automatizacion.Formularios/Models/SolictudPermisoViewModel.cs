using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models
{
    public class SolictudPermisoViewModel
    { 
        public int IdSolicitudPermiso { get; set; }
        [Required(ErrorMessage ="Campo Requerido")]
        public string CodigoLinea { get; set; }

        [DisplayName("Descripcion Linea")]
        public string DescripcionLinea { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        public string CodigoArea { get; set; }

        [DisplayName("Descripcion Area")]
        public string DescripcionArea { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        public string CodigoCargo { get; set; }

        [DisplayName("Descripcion Cargo")]
        public string DescripcionCargo { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        public string CodigoMotivo { get; set; }

        [DisplayName("Descripcion Motivo")]
        public string DescripcionMotivo { get; set; }


        public string Observacion { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        [DisplayName("Fecha Salida")]
        public DateTime FechaSalida { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        [DisplayName("Fecha Regreso")]
        public DateTime FechaRegreso { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        public int IdEstadoSolicitud { get; set; }

        [DisplayName("Fecha Biometrico")]
        public DateTime? FechaBiometrico { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        public char Origen { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        public string CodigoDiagnostico { get; set; }


        public string CodigoClasificador { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        public DateTime FechaIngresoLog { get; set; }
        [Required(ErrorMessage ="Campo Requerido")]
        public string UsuarioIngresoLog { get; set; }
        [Required(ErrorMessage ="Campo Requerido")]
        public string TerminalIngresoLog { get; set; }

        public DateTime? FechaModificacionLog { get; set; }

        public string UsuarioModificacionLog { get; set; }

        public string TerminalModificacionLog { get; set; }

        List<JUSTICA_SOLICITUD> JustificaSolicitudes { get; set; } 


    }
}