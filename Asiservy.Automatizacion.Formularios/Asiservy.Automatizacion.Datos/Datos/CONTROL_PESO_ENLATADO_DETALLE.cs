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
    
    public partial class CONTROL_PESO_ENLATADO_DETALLE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CONTROL_PESO_ENLATADO_DETALLE()
        {
            this.CONTROL_PESO_ENLATADO_SUBDETALLE = new HashSet<CONTROL_PESO_ENLATADO_SUBDETALLE>();
        }
    
        public int IdControlPesoEnlatadoDetallado { get; set; }
        public int IdControlPesoEnlatado { get; set; }
        public System.DateTime Hora { get; set; }
        public int TemperaturaAgua { get; set; }
        public int TemperaturaAceite { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    
        public virtual CONTROL_PESO_ENLATADO CONTROL_PESO_ENLATADO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONTROL_PESO_ENLATADO_SUBDETALLE> CONTROL_PESO_ENLATADO_SUBDETALLE { get; set; }
    }
}
