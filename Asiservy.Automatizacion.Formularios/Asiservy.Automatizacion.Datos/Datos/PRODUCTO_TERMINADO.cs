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
    
    public partial class PRODUCTO_TERMINADO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRODUCTO_TERMINADO()
        {
            this.PRODUCTO_TERMINADO_DANIADOS = new HashSet<PRODUCTO_TERMINADO_DANIADOS>();
            this.PRODUCTO_TERMINADO_DETALLE = new HashSet<PRODUCTO_TERMINADO_DETALLE>();
            this.PRODUCTO_TERMINADO_MATERIALES = new HashSet<PRODUCTO_TERMINADO_MATERIALES>();
            this.PRODUCTO_TERMINADO_TIEMPO_PARADAS = new HashSet<PRODUCTO_TERMINADO_TIEMPO_PARADAS>();
        }
    
        public int IdProductoTerminado { get; set; }
        public System.DateTime FechaPaletizado { get; set; }
        public System.DateTime FechaProduccion { get; set; }
        public Nullable<System.DateTime> FechaVencimiento { get; set; }
        public int CodigoSap { get; set; }
        public int OrdenFabricacion { get; set; }
        public int OrdenVenta { get; set; }
        public string Producto { get; set; }
        public string CodigoProducto { get; set; }
        public string Cliente { get; set; }
        public string Etiqueta { get; set; }
        public string Observacion { get; set; }
        public string LineaNegocio { get; set; }
        public string Linea { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
        public Nullable<bool> EstadoReporte { get; set; }
        public string AprobadoPor { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUCTO_TERMINADO_DANIADOS> PRODUCTO_TERMINADO_DANIADOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUCTO_TERMINADO_DETALLE> PRODUCTO_TERMINADO_DETALLE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUCTO_TERMINADO_MATERIALES> PRODUCTO_TERMINADO_MATERIALES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUCTO_TERMINADO_TIEMPO_PARADAS> PRODUCTO_TERMINADO_TIEMPO_PARADAS { get; set; }
    }
}
