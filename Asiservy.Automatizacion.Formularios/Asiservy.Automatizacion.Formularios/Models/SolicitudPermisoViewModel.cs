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
        [DisplayName("Solicitud")]
        public int IdSolicitudPermiso { get; set; }
        [Required(ErrorMessage ="Campo Requerido")]
        public string CodigoLinea { get; set; }

        [DisplayName("Linea")]
        public string DescripcionLinea { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        public string CodigoArea { get; set; }

        [DisplayName("Area")]
        public string DescripcionArea { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        public string CodigoCargo { get; set; }

        [DisplayName("Cargo")]
        public string DescripcionCargo { get; set; }

        [DisplayName("Identificación")]
        [Required(ErrorMessage ="Campo Requerido")]
        public string Identificacion { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string NombreEmpleado { get; set; }

        /// [Required(ErrorMessage ="Campo Requerido")]
        [Required(ErrorMessage = "Campo Requerido")]
        [DisplayName("Codigo Motivo")]
        public string CodigoMotivo { get; set; }

        [DisplayName("Motivo")]
        public string DescripcionMotivo { get; set; }

        public string Observacion { get; set; }

       // [Required(ErrorMessage ="Campo Requerido")]
        [DisplayName("Fecha Salida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm}")]
        public DateTime? FechaSalida { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FechaSalidaEntrada { get; set; }
        public DateTime? HoraSalida { get; set; }

        // [Required(ErrorMessage ="Campo Requerido")]
       
        public DateTime? HoraRegreso { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Fecha Regreso")]
        public DateTime? FechaRegreso { get; set; }

        // [Required(ErrorMessage ="Campo Requerido")]
        public string EstadoSolicitud { get; set; }
        public string DescripcionEstadoSolicitud { get; set; }
        [DisplayName("Fecha Biometrico")]
        public DateTime? FechaBiometrico { get; set; }

      //  [Required(ErrorMessage ="Campo Requerido")]
        public string Origen { get; set; }
        [DisplayName("Origen")]
        public string DescripcionOrigen { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        public string CodigoDiagnostico { get; set; }
        [DisplayName("Diagnostico")]
        public string DescripcionDiagnostico { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [DisplayName("Clasificacion")]
        public string CodigoClasificador { get; set; }

      
        public DateTime? FechaIngresoLog { get; set; }
       
        public string UsuarioIngresoLog { get; set; }
      
        public string TerminalIngresoLog { get; set; }

        public DateTime? FechaModificacionLog { get; set; }

        public string UsuarioModificacionLog { get; set; }

        public string TerminalModificacionLog { get; set; }

        public List<JUSTICA_SOLICITUD> JustificaSolicitudes { get; set; } 


    }
}