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
    
    public partial class ENTRADA_SALIDA_MATERIAL_DETALLE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ENTRADA_SALIDA_MATERIAL_DETALLE()
        {
            this.ENTRADA_SALIDA_MATERIAL_SUBDETALLE = new HashSet<ENTRADA_SALIDA_MATERIAL_SUBDETALLE>();
        }
    
        public int IdDetalleEntradaSalidaMateriales { get; set; }
        public int Material { get; set; }
        public int Ingreso { get; set; }
        public int IdCabeceraEntradaSalidaMaterial { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    
        public virtual ENTRADA_SALIDA_MATERIAL_CABECERA ENTRADA_SALIDA_MATERIAL_CABECERA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENTRADA_SALIDA_MATERIAL_SUBDETALLE> ENTRADA_SALIDA_MATERIAL_SUBDETALLE { get; set; }
    }
}