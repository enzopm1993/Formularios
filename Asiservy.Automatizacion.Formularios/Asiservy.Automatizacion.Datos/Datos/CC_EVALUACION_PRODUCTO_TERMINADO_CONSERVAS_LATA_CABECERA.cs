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
    
    public partial class CC_EVALUACION_PRODUCTO_TERMINADO_CONSERVAS_LATA_CABECERA
    {
        public int IdCabecera { get; set; }
        public System.DateTime FechaProduccion { get; set; }
        public System.DateTime FechaEvaluacion { get; set; }
        public string Fill { get; set; }
        public Nullable<decimal> Broth { get; set; }
        public Nullable<int> Agua { get; set; }
        public Nullable<int> Aceite { get; set; }
        public Nullable<int> PesoNeto { get; set; }
        public string EstadoControl { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    }
}