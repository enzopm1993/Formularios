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
    
    public partial class CC_ANALISIS_AGUA_CLORINACION_CONTROL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CC_ANALISIS_AGUA_CLORINACION_CONTROL()
        {
            this.CC_ANALISIS_AGUA_CLORINACION_DETALLE = new HashSet<CC_ANALISIS_AGUA_CLORINACION_DETALLE>();
        }
    
        public int IdAnalisisAguaControl { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
        public bool EstadoReporte { get; set; }
        public string AprobadoPor { get; set; }
        public Nullable<System.DateTime> FechaAprobado { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CC_ANALISIS_AGUA_CLORINACION_DETALLE> CC_ANALISIS_AGUA_CLORINACION_DETALLE { get; set; }
    }
}
