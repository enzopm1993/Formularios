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

      

        //Roles de usuario
        public static int RolSupervisorGeneral = 4;
        public static int RolSupervisorLinea = 2;
        public static int RolControladorLinea = 1;
        public static int RolControladorGeneral = 3;
        public static int RolGarita = 7;
        public static int RolGeneraPermisoCompartido = 17;
        public static int RolAprobacionSolicitud=8;
        public static int RolRRHH = 9;
        public static int RolMedico = 6;
        public static int AsistenteProduccion = 16;

        //Codigos de Lineas
        public static string CodLineaProduccion = "07";
        public static string CodLineaProduccionEmpaque = "45";
        public static string CodLineaProduccionRecuperadoControl = "47";


        //ClasificadorGenerico de Lineas
        public static string CodGrupoLineaProduccion = "002";
        public static string CodGrupoLineasAprobarSolicitudProduccion = "014";

        //ClasificadorGenerico de Grupo de Enfermedades
        public static string CodGrupoEnfermedadDiagnostico = "E";
        public static string CodGrupoEnfermedadGrupo = "G";
        public static string CodGrupoEnfermedadSubgrupo = "S";

        //Clasificador
        public static string CodigoClasificadorGrupo = "0";


        //Motivos de permiso medico
        public static string CodigoMotivoPermisoCitaMedica = "CM";
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


        //Classificador Tipo Control Huesos
        public static string CodigoGrupoTipoControlHuesos = "006";

        //Tipo Control Huesos
        public static int Hueso = 1;
        public static int Panza = 2;
        public static int PescadoPartido = 3;
        public static int Roto = 4;

        //Cargos 
        public static string CargoLimpiadora = "134";
        public static string CargoEnfundado = "129";
        public static string CargoEmpacado = "201";

        //Clasificador Tipos de limpieza pescado
        public static string CodigoGrupoTipoLimpiezaPescado = "008";

        //Clasificador Destinos de produccion
        public static string CodigoGrupoDestinoProduccion = "007";

        //Clasificador Grupo Especie pescado
        public static string CodigoGrupoEspeciePescado= "009";

        //Clasificador Grupo Talla de pescado
        public static string CodigoGrupoTallaPescado = "010";

        //Clasificador Grupo Marea
        public static string CodigoGrupoMarea = "016";

        //Clasificador Grupo Receta
        public static string CodigoGrupoRecetaRoceado = "017";

        //Tallas de pescado
        public static string Talla35="001";
        public static string Talla1418 = "002";
        public static string Talla1934 = "003";
        public static string Talla13 = "004";

        //Especies pescado
        public static string yellowfin = "01";
        public static string skipjack = "02";
        public static string bigeye = "03";


        //Clasificador Nivel Usuario
        public static string CodigoGrupoNivelUsuario = "011";
        public static int NivelEmpleado = 3;
        public static int NivelJefe = 2;
        public static int NivelJefatura = 1;
        public static int NivelGerencia = 0;

        //Clasificador Funda
        public static string CodigoGrupoFunda = "012";
        public static string FundaAurtion = "1";

        //ONLY CONTROL
        public static string keyLlaveAcceso = "Fy7VG+Fe5inU/sNNaHAnSA==";

        //swicth de servicios
        public static string CodigoGrupoSwitchServices = "013";
        public static string CodigoEnvioOnlyControl = "1";


        //PERIODOS
        public static string PeriodoBloqueado = "B";
        public static string PeriodoAbierto = "A";

        //PARAMETROS
        public static string TiempoSalidaGarita = "001";
        public static string ParaMensajeUrgente = "003";
        public static string ParaMensajeAviso = "002";


        //CLASIFICADOR COCINA
        public static string CodigoGrupoCocinas = "015";

        //CLASIFICADOR AUDITORIA
        public static string CodigoGrupoAuditoria = "018";


        public static string BASE_URL_WS = "http://192.168.0.31:8870";

    }
}