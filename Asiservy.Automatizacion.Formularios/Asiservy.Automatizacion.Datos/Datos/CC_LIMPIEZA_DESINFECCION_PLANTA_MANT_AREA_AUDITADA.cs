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
    
    public partial class CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA()
        {
            this.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA = new HashSet<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA>();
        }
    
        public int IdAuditoria { get; set; }
        public string NombreAuditoria { get; set; }
        public string DescripcionAuditoria { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA> CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA { get; set; }
    }
}
