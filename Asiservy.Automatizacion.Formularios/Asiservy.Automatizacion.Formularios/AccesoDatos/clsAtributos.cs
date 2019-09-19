using System;
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
        public static int NivelEmpleado = 3;
        public static int NivelJefe = 2;
        public static int NivelJefatura = 1;
        public static int NivelGerencia = 0;

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


        //Motivos de permiso medico
        public static string CodigoMotivoPermisoCitaMedica = "LE";
        public static string CodigoMotivoPermisoEnfermedadNP = "EN";

        //Tipos Cambio de personal de Área
        public static string TipoPrestar = "P";
        public static string TipoRegresar = "R";

        //Estados Asistencia
        public static string EstadoPresente = "1";
        public static string EstadoFalta = "3";
        public static string EstadoAtraso = "2";


        //Clasificador de colores de cuchillos
        public static string CodigoGrupoColorCuchillo = "003";
        public static string CodigoColorCuchilloBlanco = "B";
        public static string CodigoColorCuchilloRojo = "R";
        public static string CodigoColorCuchilloNegro = "N";
        //Clasificador de Estados de Asistencia
        public static string CodigoGrupoEstadoAsistencia = "004";

        //Clasificador de Estados de Control Cuchillo
        public static string CodigoGrupoEstadoControlCuchillo = "005";

        //Estados de Control de cuchillo
        public static string Entrada = "1";
        public static string IrAlmorzar = "2";
        public static string RegresoAlmuerzo = "3";
        public static string Salida = "4";
    }
}