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
    
    public partial class sp_Reporte_Lavado_Desinfeccion_Manos
    {
        public int IdDesinfeccionManosDetalle { get; set; }
        public int IdDesinfeccionManos { get; set; }
        public System.DateTime Hora { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public string CodigoLinea { get; set; }
        public bool EstadoCumplimiento { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public bool EstadoReporteCB { get; set; }
    }
}
