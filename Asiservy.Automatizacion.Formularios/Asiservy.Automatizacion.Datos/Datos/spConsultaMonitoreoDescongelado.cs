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
    
    public partial class spConsultaMonitoreoDescongelado
    {
        public int IdMonitoreoDescongelado { get; set; }
        public int IdMonitoreoDescongeladoControl { get; set; }
        public int IdTipoMonitoreo { get; set; }
        public System.DateTime Fecha { get; set; }
        public int TemperaturaAgua { get; set; }
        public string Turno { get; set; }
        public string Tanque { get; set; }
        public string Lote { get; set; }
        public string Especie { get; set; }
        public string Talla { get; set; }
        public System.DateTime Hora { get; set; }
        public string Observacion { get; set; }
        public string ObservacionGeneral { get; set; }
        public int IdMuestra { get; set; }
        public decimal Cantidad { get; set; }
    }
}
