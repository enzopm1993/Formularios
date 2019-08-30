using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models
{
    public class SolicitudPermisoViewModel
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
        public string NombreEmpleado { get; set; }

        /// [Required(ErrorMessage ="Campo Requerido")]
        [DisplayName("Motivo")]
        public string CodigoMotivo { get; set; }

        [DisplayName("Descripcion Motivo")]
        public string DescripcionMotivo { get; set; }

        public string Observacion { get; set; }

       // [Required(ErrorMessage ="Campo Requerido")]
        [DisplayName("Fecha Salida")]
        public DateTime? FechaSalida { get; set; }
        public DateTime? FechaSalidaEntrada { get; set; }
        public DateTime? HoraSalida { get; set; }

        // [Required(ErrorMessage ="Campo Requerido")]
        [DisplayName("Fecha Regreso")]
        public DateTime? HoraRegreso { get; set; }
        public DateTime? FechaRegreso { get; set; }

        // [Required(ErrorMessage ="Campo Requerido")]
        public string EstadoSolicitud { get; set; }

        [DisplayName("Fecha Biometrico")]
        public DateTime? FechaBiometrico { get; set; }

      //  [Required(ErrorMessage ="Campo Requerido")]
        public char Origen { get; set; }

       // [Required(ErrorMessage ="Campo Requerido")]
        public string CodigoDiagnostico { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public string CodigoClasificador { get; set; }

      
        public DateTime? FechaIngresoLog { get; set; }
       
        public string UsuarioIngresoLog { get; set; }
      
        public string TerminalIngresoLog { get; set; }

        public DateTime? FechaModificacionLog { get; set; }

        public string UsuarioModificacionLog { get; set; }

        public string TerminalModificacionLog { get; set; }

        List<JUSTICA_SOLICITUD> JustificaSolicitudes { get; set; } 


    }
}