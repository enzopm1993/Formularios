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
    
    public partial class CONTROL_HORA_MAQUINA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CONTROL_HORA_MAQUINA()
        {
            this.CONTROL_HORA_MAQUINA_DETALLE = new HashSet<CONTROL_HORA_MAQUINA_DETALLE>();
        }
    
        public int IdControlHoraMaquina { get; set; }
        public string OrdenFabricacion { get; set; }
        public string OrdenVenta { get; set; }
        public string Turno { get; set; }
        public string Cliente { get; set; }
        public string LineaNegocio { get; set; }
        public string CodigoProducto { get; set; }
        public string Producto { get; set; }
        public int PesoNeto { get; set; }
        public System.DateTime Fecha { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONTROL_HORA_MAQUINA_DETALLE> CONTROL_HORA_MAQUINA_DETALLE { get; set; }
    }
}