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
    
    public partial class spReporteEsterilizacionDetalle
    {
        public string NOMBRES { get; set; }
        public Nullable<bool> UnidadPresion { get; set; }
        public Nullable<bool> AutoclaveConvencional { get; set; }
        public int IdCocheAutoclave { get; set; }
        public string Observacion { get; set; }
        public int IdDetalleControlEsterilizacionConserva { get; set; }
        public int NumeroAutoclave { get; set; }
        public int NumeroEsterilizada { get; set; }
        public string Producto { get; set; }
        public string CodigoProducto { get; set; }
        public string Envase { get; set; }
        public Nullable<decimal> TemperaturaInicial { get; set; }
        public Nullable<System.DateTime> HoraInicioViento { get; set; }
        public Nullable<System.DateTime> HoraCierreViento { get; set; }
        public Nullable<decimal> TemperaturaTermDigital { get; set; }
        public Nullable<System.DateTime> HoraInicioLlenado { get; set; }
        public Nullable<System.DateTime> HoraInicioCalentamiento { get; set; }
        public Nullable<System.DateTime> HoraInicioEsterilizacion { get; set; }
        public Nullable<System.DateTime> HoraFinalEsterilizacion { get; set; }
        public Nullable<int> MinutosTotalesEsterilizado { get; set; }
        public Nullable<System.DateTime> TiempoEnfriamiento { get; set; }
        public Nullable<decimal> TemperaturaProductoSalida { get; set; }
    }
}
