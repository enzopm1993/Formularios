﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public static class clsAtributos
    {
        //Mensajes de sistemas
        public static string MsjRegistroGuardado = "Registro Guardado Correctamente";
        public static string MsjRegistroError = "Registro no pudo ser Grabado";

        //Estados de registros
        public static string EstadoRegistroActivo = "A";
        public static string EstadoRegistroInactivo = "I";

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

        //Roles de usuario
        public static int RolSupervisor = 5;
        public static int RolGarita = 10;

        //Codigos de Lineas
        public static string CodLineaProduccion = "07";

        //ClasificadorGenerico de Lineas
        public static string CodGrupoLineaProduccion = "002";

        //ClasificadorGenerico de Grupo de Enfermedades
        public static string CodGrupoEnfermedadDiagnostico = "E";
        public static string CodGrupoEnfermedadGrupo = "G";
        public static string CodGrupoEnfermedadSubgrupo = "S";

        //Clasificador
        public static string CodigoClasificadorGrupo = "0";

    }
}