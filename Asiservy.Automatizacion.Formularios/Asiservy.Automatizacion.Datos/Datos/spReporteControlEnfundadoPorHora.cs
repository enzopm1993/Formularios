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
    
    public partial class spReporteControlEnfundadoPorHora
    {
        public int IdControlEnfundado { get; set; }
        public System.DateTime Fecha { get; set; }
        public System.TimeSpan Hora { get; set; }
        public string Lote { get; set; }
        public int TeoricoFunda { get; set; }
        public decimal PesoProducto { get; set; }
        public string Funda { get; set; }
        public Nullable<int> TotalEnfundadora { get; set; }
        public Nullable<int> TotalFunda { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
    }
}