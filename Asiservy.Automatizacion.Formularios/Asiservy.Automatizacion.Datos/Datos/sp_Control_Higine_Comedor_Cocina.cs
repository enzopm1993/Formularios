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
    
    public partial class sp_Control_Higine_Comedor_Cocina
    {
        public int IdControlHigiene { get; set; }
        public System.DateTime Fecha { get; set; }
        public System.DateTime Hora { get; set; }
        public bool EstadoReporte { get; set; }
        public string ObservacionCabecera { get; set; }
        public byte[] FirmaAprobado { get; set; }
        public string AprobadoPor { get; set; }
        public Nullable<System.DateTime> FechaAprobado { get; set; }
        public int IdControlDetalle { get; set; }
        public string LimpiezaEstado { get; set; }
        public string Observacion { get; set; }
        public string AccionCorrectiva { get; set; }
        public int IdMantenimiento { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string ObservacionMantenimiento { get; set; }
    }
}
