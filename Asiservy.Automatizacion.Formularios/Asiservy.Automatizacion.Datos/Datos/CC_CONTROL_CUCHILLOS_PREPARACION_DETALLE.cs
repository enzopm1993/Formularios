//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Asiservy.Automatizacion.Datos.Datos
{
    using System;
    using System.Collections.Generic;
    
    public partial class CC_CONTROL_CUCHILLOS_PREPARACION_DETALLE
    {
        public int IdControlCuchilloDetalle { get; set; }
        public int IdControlCuchillo { get; set; }
        public int IdCuchilloPreparacion { get; set; }
        public string CedulaEmpleado { get; set; }
        public bool Estado { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    
        public virtual CC_CUCHILLOS_PREPARACION CC_CUCHILLOS_PREPARACION { get; set; }
        public virtual CC_CONTROL_CUCHILLOS_PREPARACION CC_CONTROL_CUCHILLOS_PREPARACION { get; set; }
    }
}
