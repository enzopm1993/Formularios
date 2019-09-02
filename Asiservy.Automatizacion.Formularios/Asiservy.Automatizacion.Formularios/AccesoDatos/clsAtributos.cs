using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public static class clsAtributos
    {
        //Estados de registros
        public static string EstadoRegistroActivo = "1";
        public static string EstadoRegistroInactivo = "0";

        //Origen de solicitudes
        public static string SolicitudOrigenMedico = "M";
        public static string SolicitudOrigenGeneral = "G";

        //Estados de solicitudes
        public static string EstadoSolicitudTodos = "000";
        public static string EstadoSolicitudPendiente = "001";
        public static string EstadoSolicitudAprobado = "002";
        public static string EstadoSolicitudAnulado = "004";
        public static string EstadoSolicitudRevisado = "003";

        //Niveles de usuario
        public static int NivelEmpleado =3;
        public static int NivelJefe =2;
        public static int NivelJefatura =1;
        public static int NivelGerencia =0;

    }
}