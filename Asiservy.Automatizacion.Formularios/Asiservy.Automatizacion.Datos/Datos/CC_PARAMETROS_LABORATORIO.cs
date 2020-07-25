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
    
    public partial class CC_PARAMETROS_LABORATORIO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CC_PARAMETROS_LABORATORIO()
        {
            this.CC_ANALISIS_QUIMICO_PRECOCCION_ELEMENTOS = new HashSet<CC_ANALISIS_QUIMICO_PRECOCCION_ELEMENTOS>();
            this.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_PARAMETROXTIPO = new HashSet<CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_PARAMETROXTIPO>();
        }
    
        public int IdParametro { get; set; }
        public string CodFormClasif { get; set; }
        public string CodArea { get; set; }
        public Nullable<decimal> Mascara { get; set; }
        public string NombreParametro { get; set; }
        public Nullable<decimal> ValorMax { get; set; }
        public Nullable<decimal> ValorMin { get; set; }
        public string DescripcionParametro { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CC_ANALISIS_QUIMICO_PRECOCCION_ELEMENTOS> CC_ANALISIS_QUIMICO_PRECOCCION_ELEMENTOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_PARAMETROXTIPO> CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_PARAMETROXTIPO { get; set; }
    }
}
