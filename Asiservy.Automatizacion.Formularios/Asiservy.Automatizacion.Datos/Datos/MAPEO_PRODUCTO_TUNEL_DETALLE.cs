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
    
    public partial class MAPEO_PRODUCTO_TUNEL_DETALLE
    {
        public int IdMapeoProductoTunelDetalle { get; set; }
        public int IdMapeoProductoTunel { get; set; }
        public int Tunel { get; set; }
        public int Coche { get; set; }
        public string Producto { get; set; }
        public string Especie { get; set; }
        public int Fundas { get; set; }
        public System.DateTime HoraInicio { get; set; }
        public System.DateTime HoraFin { get; set; }
        public Nullable<System.DateTime> HoraFinLote { get; set; }
        public string TotalFunda { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
        public string Textura { get; set; }
    
        public virtual MAPEO_PRODUCTO_TUNEL MAPEO_PRODUCTO_TUNEL { get; set; }
    }
}
