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
    
    public partial class ENTRADA_SALIDA_MATERIAL_CABECERA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ENTRADA_SALIDA_MATERIAL_CABECERA()
        {
            this.ENTRADA_SALIDA_MATERIAL_DETALLE = new HashSet<ENTRADA_SALIDA_MATERIAL_DETALLE>();
        }
    
        public int IdControlEntradaSalidaMateriales { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Linea { get; set; }
        public string Turno { get; set; }
        public string Observacion { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
        public bool EstadoControl { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENTRADA_SALIDA_MATERIAL_DETALLE> ENTRADA_SALIDA_MATERIAL_DETALLE { get; set; }
    }
}
