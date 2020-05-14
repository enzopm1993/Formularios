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
    
    public partial class CC_EVALUACION_PRODUCTO_ENFUNDADO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CC_EVALUACION_PRODUCTO_ENFUNDADO()
        {
            this.CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE = new HashSet<CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE>();
        }
    
        public int IdEvaluacionProductoEnfundado { get; set; }
        public Nullable<System.DateTime> FechaProduccion { get; set; }
        public Nullable<int> OrdenFabricacion { get; set; }
        public string Cliente { get; set; }
        public string Marca { get; set; }
        public string Destino { get; set; }
        public string Proveedor { get; set; }
        public string Lote { get; set; }
        public string Batch { get; set; }
        public Nullable<bool> Lomo { get; set; }
        public Nullable<bool> Trozo { get; set; }
        public Nullable<bool> Miga { get; set; }
        public string NivelLimpieza { get; set; }
        public string Observacion { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
        public Nullable<bool> EstadoControl { get; set; }
        public string AprobadoPor { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
        public string ImagenCodigo { get; set; }
        public string ImagenProducto1 { get; set; }
        public string ImagenProducto2 { get; set; }
        public string ImagenProducto3 { get; set; }
        public Nullable<int> RotacionImagenCod { get; set; }
        public Nullable<int> RotacionImagenProd1 { get; set; }
        public Nullable<int> RotacionImagenProd2 { get; set; }
        public Nullable<int> RotacionImagenProd3 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE> CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE { get; set; }
    }
}
