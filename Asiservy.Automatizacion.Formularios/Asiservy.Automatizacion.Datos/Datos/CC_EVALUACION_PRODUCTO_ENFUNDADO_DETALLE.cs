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
    
    public partial class CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE
    {
        public int IdDetalleEvaluacionProductoEnfundado { get; set; }
        public Nullable<System.DateTime> Hora { get; set; }
        public string Empacador { get; set; }
        public string Lote { get; set; }
        public string buque { get; set; }
        public int Sabor { get; set; }
        public int Textura { get; set; }
        public int Color { get; set; }
        public int Olor { get; set; }
        public Nullable<int> Moretones { get; set; }
        public Nullable<int> HematomasProfundos { get; set; }
        public int Proteina { get; set; }
        public Nullable<decimal> Trozo { get; set; }
        public Nullable<decimal> Miga { get; set; }
        public Nullable<int> Venas { get; set; }
        public Nullable<int> Espinas { get; set; }
        public Nullable<int> Sangre { get; set; }
        public Nullable<int> Escamas { get; set; }
        public Nullable<int> Piel { get; set; }
        public Nullable<int> Otro { get; set; }
        public int IdCabeceraEvaluacionProductoEnfundado { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    
        public virtual CC_EVALUACION_PRODUCTO_ENFUNDADO CC_EVALUACION_PRODUCTO_ENFUNDADO { get; set; }
    }
}