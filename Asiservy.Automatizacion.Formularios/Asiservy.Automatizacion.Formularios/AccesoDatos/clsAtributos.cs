using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public static class clsAtributos
    {
        public static string EstadoRegistroActivo = "1";
        public static string EstadoRegistroInactivo = "0";

        public static string SolicitudOrigenMedico = "M";
        public static string SolicitudOrigenGeneral = "G";

        public static string EstadoSolicitudTodos = "000";
        public static string EstadoSolicitudPendiente = "001";
        public static string EstadoSolicitudAprobado = "002";
        public static string EstadoSolicitudAnulado = "004";
        public static string EstadoSolicitudRevisado = "003";
    }
}