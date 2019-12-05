using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models
{
    public class ControlHorasMaquina
    {
        public int IdControlHoraMaquina { get; set; }
        public string OrdenFabricacion { get; set; }
        public string OrdenVenta { get; set; }
        public string Turno { get; set; }
        public string Cliente { get; set; }
        public string LineaNegocio { get; set; }
        public string CodigoProducto { get; set; }
        public string Producto { get; set; }
        public int PesoNeto { get; set; }
        public System.DateTime Fecha { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
        public List<CONTROL_HORA_MAQUINA_DETALLE> CONTROL_HORA_MAQUINA_DETALLE { get; set; }

    }

}