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
    
    public partial class CC_OPERATIVIDAD_METAL_DETALLE
    {
        public int IdOperatividadMetalDetalle { get; set; }
        public int IdOperatividadMetal { get; set; }
        public System.DateTime Hora { get; set; }
        public bool Ferroso { get; set; }
        public bool NoFerroso { get; set; }
        public bool AceroInoxidable { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    
        public virtual CC_OPERATIVIDAD_METAL CC_OPERATIVIDAD_METAL { get; set; }
    }
}
