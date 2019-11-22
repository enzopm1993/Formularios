using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.App
{
    public class InfoCambioDatos
    {
        public int id { get; set; }
        public string username { get; set; }
        public string cedula { get; set; }
        public string direccion { get; set; }
        public string barrio { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string correo { get; set; }
        public bool cambia_direccion { get; set; }
        public bool cambia_barrio { get; set; }
        public bool cambia_telefono { get; set; }
        public bool cambia_celular { get; set; }
        public bool cambia_correo { get; set; }
        public string FechaSolicitud { get; set; }
        public string DEPARTAMENTO { get; set; }
    }
}