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
    
    public partial class PROYECCION_PROGRAMACION
    {
        public int IdProyeccionProgramacion { get; set; }
        public string Lote { get; set; }
        public string OrdenFabricacion { get; set; }
        public Nullable<int> Toneladas { get; set; }
        public string Lineas { get; set; }
        public Nullable<System.TimeSpan> HoraInicio { get; set; }
        public Nullable<System.TimeSpan> HoraFin { get; set; }
        public string Destino { get; set; }
        public string TipoLimpieza { get; set; }
        public Nullable<System.DateTime> FechaProduccion { get; set; }
        public string Especie { get; set; }
        public string Talla { get; set; }
        public string Observacion { get; set; }
        public string EstadoRegistro { get; set; }
        public Nullable<System.DateTime> FechaCreacionLog { get; set; }
        public string UsuarioCreacionLog { get; set; }
        public string TerminalCreacionLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    }
}
