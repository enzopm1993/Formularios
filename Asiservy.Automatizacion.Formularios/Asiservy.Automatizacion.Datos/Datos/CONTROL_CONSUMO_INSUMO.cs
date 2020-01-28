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
    
    public partial class CONTROL_CONSUMO_INSUMO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CONTROL_CONSUMO_INSUMO()
        {
            this.CONSUMO_DETALLE_DANIADO = new HashSet<CONSUMO_DETALLE_DANIADO>();
            this.CONSUMO_DETALLE_POUCH = new HashSet<CONSUMO_DETALLE_POUCH>();
            this.CONSUMO_TIEMPO_MUERTO = new HashSet<CONSUMO_TIEMPO_MUERTO>();
            this.CONSUMO_PROCEDENCIA_PESCADO = new HashSet<CONSUMO_PROCEDENCIA_PESCADO>();
            this.CONSUMO_DETALLE_ADITIVO = new HashSet<CONSUMO_DETALLE_ADITIVO>();
            this.CONTROL_CONSUMO_INSUMO_DETALLE = new HashSet<CONTROL_CONSUMO_INSUMO_DETALLE>();
            this.CONSUMO_DETALLE_LATA = new HashSet<CONSUMO_DETALLE_LATA>();
        }
    
        public int IdControlConsumoInsumos { get; set; }
        public int OrdenFabricacion { get; set; }
        public int OrdenVenta { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Turno { get; set; }
        public string LineaNegocio { get; set; }
        public string CodigoProducto { get; set; }
        public string Producto { get; set; }
        public string Destino { get; set; }
        public string Cliente { get; set; }
        public Nullable<int> PesoNeto { get; set; }
        public Nullable<int> PesoEscrundido { get; set; }
        public Nullable<decimal> Lomo { get; set; }
        public Nullable<decimal> Miga { get; set; }
        public string Envase { get; set; }
        public string Tapa { get; set; }
        public Nullable<int> Aceite { get; set; }
        public Nullable<int> Agua { get; set; }
        public Nullable<int> CaldoVegetal { get; set; }
        public Nullable<int> DesperdicioSolido { get; set; }
        public Nullable<int> DesperdicioLiquido { get; set; }
        public Nullable<int> DesperdicioAceite { get; set; }
        public Nullable<System.DateTime> HoraInicio { get; set; }
        public Nullable<System.DateTime> HoraFin { get; set; }
        public Nullable<int> Empleados { get; set; }
        public Nullable<int> Cajas { get; set; }
        public string Observacion { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
        public string CodigoMaterial { get; set; }
        public Nullable<int> UnidadesRecibidas { get; set; }
        public Nullable<int> UnidadesSobrantes { get; set; }
        public Nullable<int> UnidadesProducidas { get; set; }
        public string Marca { get; set; }
        public Nullable<int> UnidadesRecibidasTapa { get; set; }
        public Nullable<int> UnidadesSobrantesTapa { get; set; }
        public Nullable<int> UnidadesProducidasTapa { get; set; }
        public Nullable<decimal> GrsLataReal { get; set; }
        public Nullable<int> SaldoInicialLamina { get; set; }
        public Nullable<int> SaldoInicialUnidad { get; set; }
        public Nullable<int> SaldoFinalLamina { get; set; }
        public Nullable<int> SaldoFinalUnidad { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONSUMO_DETALLE_DANIADO> CONSUMO_DETALLE_DANIADO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONSUMO_DETALLE_POUCH> CONSUMO_DETALLE_POUCH { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONSUMO_TIEMPO_MUERTO> CONSUMO_TIEMPO_MUERTO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONSUMO_PROCEDENCIA_PESCADO> CONSUMO_PROCEDENCIA_PESCADO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONSUMO_DETALLE_ADITIVO> CONSUMO_DETALLE_ADITIVO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONTROL_CONSUMO_INSUMO_DETALLE> CONTROL_CONSUMO_INSUMO_DETALLE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONSUMO_DETALLE_LATA> CONSUMO_DETALLE_LATA { get; set; }
    }
}
