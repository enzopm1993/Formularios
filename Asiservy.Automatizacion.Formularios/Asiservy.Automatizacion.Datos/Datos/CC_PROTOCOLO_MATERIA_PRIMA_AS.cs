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
    
    public partial class CC_PROTOCOLO_MATERIA_PRIMA_AS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CC_PROTOCOLO_MATERIA_PRIMA_AS()
        {
            this.CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS = new HashSet<CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS>();
        }
    
        public int IdProtocoloMateriaPrima { get; set; }
        public System.DateTime Fecha { get; set; }
        public System.DateTime FechaEvaluacion { get; set; }
        public System.DateTime FechaDescarga { get; set; }
        public int OrdenFabricacion { get; set; }
        public string Lote { get; set; }
        public string LoteDescarga { get; set; }
        public string CodigoProtocolo { get; set; }
        public int Pcc { get; set; }
        public string Observacion { get; set; }
        public bool EstadoReporte { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
        public string AprobadoPor { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS> CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS { get; set; }
    }
}