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
    
    public partial class spConsultaProyeccionProgramacion
    {
        public int IdProyeccionProgramacionDetalle { get; set; }
        public int IdProyeccionProgramacion { get; set; }
        public string Lote { get; set; }
        public string OrdenFabricacion { get; set; }
        public Nullable<int> Toneladas { get; set; }
        public string Lineas { get; set; }
        public Nullable<System.TimeSpan> HoraProcesoInicio { get; set; }
        public Nullable<System.TimeSpan> HoraProcesoFin { get; set; }
        public string CodDestino { get; set; }
        public string Destino { get; set; }
        public string CodTipoLimpieza { get; set; }
        public string TipoLimpieza { get; set; }
        public string Especie { get; set; }
        public string Talla { get; set; }
        public Nullable<int> TemperaturaFinal { get; set; }
        public Nullable<System.TimeSpan> HoraCoccionInicio { get; set; }
        public Nullable<System.TimeSpan> HoraCoccionFin { get; set; }
        public Nullable<int> TotalCoches { get; set; }
        public string Cocina { get; set; }
        public Nullable<System.TimeSpan> HoraEviceradoInicio { get; set; }
        public Nullable<System.TimeSpan> HoraEviceradoFin { get; set; }
        public Nullable<System.TimeSpan> HoraDescongeladoInicio { get; set; }
        public Nullable<System.TimeSpan> HoraDescongeladoFin { get; set; }
        public Nullable<System.TimeSpan> Requerimiento { get; set; }
        public string Observacion { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    }
}
