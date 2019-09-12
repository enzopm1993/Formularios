using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Seguridad
{
    public class Clasificador
    {
        public int IdClasificador { get; set; }
        
        public string Grupo { get; set; }
        [DisplayName("Nombre de Grupo")]
        public string GrupoNombre { get; set; }

        [Required(ErrorMessage ="CampoRequerido")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "CampoRequerido")]
        public string Descripcion { get; set; }
        public string EstadoRegistro { get; set; }       
        public Nullable<System.DateTime> FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
    }
}