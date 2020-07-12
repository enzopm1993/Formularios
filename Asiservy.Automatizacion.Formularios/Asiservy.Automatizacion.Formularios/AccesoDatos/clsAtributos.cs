using System;
using System.Collections.Generic;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public static class clsAtributos
    {
        //CODIGO GRUPO PARA EL CONTROL DE ANALISIS QUIMICOS DE PRECOCCION
        public static string codPrecoccion { get; set; } = "044";
        //PARAMETROS PARA EL CONTROL DE CLORO EN CISTERNA
        public static string CC_CodParametroCloroCisterna { get; set; } = "Cod2";
        //USADO PARA EL REPORTE DE MATERIAL QUEBRADIZO DE CALIDAD
        public static List<string> MaterialQuebradizoVerificacion { get; set; } = new List<string>() { "Diario", "Semanal","Quincenal"};
        //ID DEL GRUPO DEL CLASIFICADOR PARA EL LAVADO Y DESINFECCION DE MANOS
        public static string IdCodigoLineaLavadoDesinfeccionManos { get; } = "037";
        //ESTADO DE REPORTE false=PENDIENTE; true=APROBADO
        public static bool EstadoReporteActivo { get; set; } = true;
        public static bool EstadoReportePendiente { get; set; } = false;
        //BASES DE DATOS
        public static string DesarrolloBD { get; } = "ASIS_DESARROLLO"; 
        public static string PreProduccionBD { get; } = "ASIS_PRE_PROD"; 
        public static string ProduccionBD { get; } = "ASISERVY_PROD";

        //DESCRIPCION DE BASES DE DATOS
        public static string BDProduccion { get; } = "SIAA";
        public static string BDPreProduccion { get; } = "Pruebas";
        public static string BDDesarrollo { get; } = "Desarrollo";

        //Mensajes de sistemas
        public static string MsjRegistroGuardado { get; } = "Registro Guardado Correctamente";
        public static string MsjRegistroError { get; } = "Registro no pudo ser Grabado";

        //Estados de registros
        public static string EstadoRegistroActivo { get; } = "A";
        public static string EstadoRegistroInactivo { get; } = "I";

        //Origen de solicitudes
        public static string SolicitudOrigenMedico { get; } = "M";
        public static string SolicitudOrigenGeneral { get; } = "G";

        //Estados de solicitudes
        public static string EstadoSolicitudTodos { get; } = "000";
        public static string EstadoSolicitudPendiente { get; } = "001";
        public static string EstadoSolicitudAprobado { get; } = "002";
        public static string EstadoSolicitudAnulado { get; } = "004";
        public static string EstadoSolicitudRevisado { get; } = "003";

      

        //Roles de usuario
        public static int RolSupervisorGeneral { get; } = 4;
        public static int RolSupervisorLinea { get; } = 2;
        public static int RolControladorLinea { get; } = 1;
        public static int RolControladorGeneral { get; } = 3;
        public static int RolGarita { get; } = 7;
        public static int RolGeneraPermisoCompartido { get; } = 17;
        public static int RolAprobacionSolicitud { get; } = 8;
        public static int RolRRHH { get; } = 9;
        public static int RolMedico { get; } = 6;
        public static int RolCamara { get; } = 26;
        public static int RolPouch { get; } = 40;
        public static int RolEnlatado { get; } = 27;
        public static int AsistenteProduccion { get; } = 16;
        public static int SeguridadIndustrial { get; } = 41;
        public static int RolEtiquetadoLata { get; } = 36;
        public static int RolEtiquetadoPouch { get; } = 37;
        public static int RolLimpiezaPouch { get; } = 38;
        public static int RolAutoclave { get; } = 25;
        public static int RolFrio { get; } = 39;
        public static int RolEvicerado { get; } = 13;
        public static int RolControlOC { get; } = 33;

       
        //Codigos de Lineas
        public static string CodLineaProduccion { get; } = "07";
        public static string CodLineaProduccionEmpaque { get; } = "45";
        public static string CodLineaProduccionRecuperadoControl { get; } = "47";


        //ClasificadorGenerico de Lineas
        public static string CodGrupoLineaProduccion { get; } = "002";
        public static string CodGrupoLineasAprobarSolicitudProduccion { get; } = "014";

        //ClasificadorGenerico de Grupo de Enfermedades
        public static string CodGrupoEnfermedadDiagnostico { get; } = "E";
        public static string CodGrupoEnfermedadGrupo { get; } = "G";
        public static string CodGrupoEnfermedadSubgrupo { get; } = "S";

        //Clasificador
        public static string CodigoClasificadorGrupo { get; } = "0";


        //Motivos de permiso medico
        public static string CodigoMotivoPermisoCitaMedica { get; } = "CM";
        public static string CodigoMotivoPermisoEnfermedadNP { get; } = "EN";

        //Motivos de permiso general
        public static string CodigoMotivoPermisoComisionServicio { get; } = "CS";

        //Tipos Cambio de personal de Área
        public static string TipoPrestar { get; } = "P";
        public static string TipoRegresar { get; } = "R";

        //Estados Asistencia
        public static string EstadoPresente { get; } = "1";
        public static string EstadoFalta { get; } = "3";
        public static string EstadoAtraso { get; } = "2";

        //Asistencia Origen donde se genera
        public static string Procesos { get; } = "PRO";
        public static string Prestado { get; } = "PRE";
        public static string General { get; } = "GNR";

        //Clasificador de colores de cuchillos
        public static string CodigoGrupoColorCuchillo { get; } = "003";
        public static string CodigoColorCuchilloBlanco { get; } = "B";
        public static string CodigoColorCuchilloRojo { get; } = "R";
        public static string CodigoColorCuchilloNegro { get; } = "N";
        //Clasificador de Estados de Asistencia
        public static string CodigoGrupoEstadoAsistencia { get; } = "004";

        //Clasificador de Estados de Control Cuchillo
        public static string CodigoGrupoEstadoControlCuchillo { get; } = "005";

        //Estados de Control de cuchillo
        public static string Entrada { get; } = "1";
        public static string IrAlmorzar { get; } = "2";
        public static string RegresoAlmuerzo { get; } = "3";
        public static string Salida { get; } = "4";


        //Classificador Tipo Control Huesos
        public static string CodigoGrupoTipoControlHuesos { get; } = "006";

        //Tipo Control Huesos
        public static int Hueso { get; } = 1;
        public static int Panza { get; } = 2;
        public static int PescadoPartido { get; } = 3;
        public static int Roto { get; } = 4;

        //Cargos 
        public static string CargoLimpiadora { get; } = "134";
        public static string CargoEnfundado { get; } = "129";
        public static string CargoEmpacado { get; } = "201";

        //Clasificador Tipos de limpieza pescado
        public static string CodigoGrupoTipoLimpiezaPescado { get; } = "008";

        //Clasificador Destinos de produccion
        public static string CodigoGrupoDestinoProduccion { get; } = "007";

        //Clasificador Grupo Especie pescado
        public static string CodigoGrupoEspeciePescado { get; } = "009";

        //Clasificador Grupo Talla de pescado
        public static string CodigoGrupoTallaPescado { get; } = "010";

        //Clasificador Grupo Marea
        public static string CodigoGrupoMarea { get; } = "016";

        //Clasificador Grupo Receta
        public static string CodigoGrupoRecetaRoceado { get; } = "017";

        //Clasificador Grupo Autoclave
        public static string CodigoGrupoAutoclave { get; } = "019";

        //Tallas de pescado
        public static string Talla35 { get; } = "001";
        public static string Talla1418 { get; } = "002";
        public static string Talla1934 { get; } = "003";
        public static string Talla13 { get; } = "004";

        //Especies pescado
        public static string yellowfin { get; } = "01";
        public static string skipjack { get; } = "02";
        public static string bigeye { get; } = "03";


        //Clasificador Nivel Usuario
        public static string CodigoGrupoNivelUsuario { get; } = "011";
        public static int NivelEmpleado { get; } = 5;
        public static int NivelSubJefe { get; } = 4;
        public static int NivelJefe { get; } = 3;
        public static int NivelSubGerencia { get; } = 2;
        public static int NivelGerencia { get; } = 1;

        //Clasificador Funda
        public static string CodigoGrupoFunda { get; } = "012";
        public static string FundaAurtion { get; } = "1";

        //ONLY CONTROL
        public static string keyLlaveAcceso { get; } = "Fy7VG+Fe5inU/sNNaHAnSA==";

        //swicth de servicios
        public static string CodigoGrupoSwitchServices { get; } = "013";
        public static string CodigoEnvioOnlyControl { get; } = "1";


        //PERIODOS
        public static string PeriodoBloqueado { get; } = "B";
        public static string PeriodoAbierto { get; } = "A";

        //PARAMETROS
        public static string TiempoSalidaGarita { get; } = "001";
        public static string ParaMensajeUrgente { get; } = "003";
        public static string ParaMensajeAviso { get; } = "002";


        //CLASIFICADOR COCINA
        public static string CodigoGrupoCocinas { get; } = "015";

        //CLASIFICADOR AUDITORIA
        public static string CodigoGrupoAuditoria { get; } = "018";

        //LINEA DE NEGOCIOS
        public static string LineaNegocioEnlatado { get; } = "ENLATADO";
        public static string LineaNegocioPouch { get; } = "POUCH";

        //PESO ENLATADO
        public static string GrupoCodPesoEnlatado { get; } = "020";
        public static string CodPesoEnlatadoFill { get; } = "1";
        public static string CodPesoEnlatadoNeto { get; } = "2";

        //LINEA ENLATADO
        public static string GrupoCodLineaEnlatado { get; } = "021";

        //TURNOS
        public static string GrupoCodTurno { get; } = "039";
        public static string TurnoUno { get; } = "1";
        public static string TurnoDos { get; } = "2";

        public static string BASE_URL_WS { get; } = "http://192.168.0.31:8870";
        //Estadp Aprobación Mover Personal en Nómina
        public static string EstadoPendienteMoverPersonalN { get; } = "001";
        public static string EstadoAprobadoMoverPersonalN { get; } = "002";

        //Clasificador Grupo de Motivos excluidos para solicitud de permiso
        public static string CodigoGrupoMotivosExcluidos { get; } = "022";

        //Clasificador de Grupo de Correos electronicos para solicitud aprobada.
        public static string CodigoGrupoCorreosElectronicosCopias { get; } = "023";


        //CLASIFICADOR DE CONSUMO DETALLE DANIADO
        public static string CodigoGrupoConsumoDaniadoLata { get; } = "024";
        public static string CodigoGrupoConsumoDaniadoPouch { get; } = "025";


        //CLASIFICADOR DE TIPO DE TIEMPOS MUERRTOS
        public static string CodigoGrupoTipoTiemposMuertos { get; } = "026";

        //CLASIFICADOR DE TIPO DE PROCEDENCIA CONSUMO INSUMO
        public static string CodigoGrupoProcedenciaConsumoInsumo { get; } = "027";

        //CLASIFICADOR DE CONSUMO PRODUCTOS TERMINADOS
        public static string CodigoGrupoConsumoProductoTerminado { get; } = "029";

        //CLASIFICADOR DE MATERIALES PRODUCTOS TERMINADOS
        public static string CodigoGrupoMaterialesProductoTerminado { get; } = "030";

        //CLASIFICADOR PROVEEDORES DE PALLET 
        public static string CodigoGrupoProveedorPallet { get; } = "028";

        //CLASIFICADOR DE LINEAS DE ENTREGA DE PRODUCTO TERMINADO
        public static string CodigoGrupoLineasEntregaProductoTerminado { get; } = "031";

        //TIPO  LINEA CONTROL  DE ESTERILIZACION DE CONSERVAS
        public static string TipoLineaLata { get; } = "L";
        public static string TipoLineaPouch { get; } = "P";

        
        //CODIGO GRUPO AREAS CALIDAD
        public static string CodGrupoAreasResidualCloro { get; } = "035";


        //CONTROL CONSERVAS TIEMPOS INICIO, MEDIO Y FINAL
        public static string Inicio { get; } = "I";
        public static string Medio { get; } = "M";
        public static string Final { get; } = "F";


        public static string UrlImagen { get; } = "~/ImagenSiaa/";


        //TIPO DE PRODUCTO_ ANALISIS DE PRODUCTO SEMIELABORADO

        public static string Lomo { get; } = "L";
        public static string Miga { get; } = "M";
        public static string Trozo { get; } = "T";

        
        //GRUPO CLASIFICADOR DE TIPO DE CONTROL EN EL MENU
        public static string CodGrupoTipoControl { get; } = "040";
        public static string CodTipoControlReporte { get; } = "R";


        //PARAMETROS DE CALIDAD
        public static string CodigoParametroCloroAguaAutoclave { get; } = "Cod1";


        //GRUPO CLASIFICADOR DE COLORES
        public static string CodGrupoColores { get; } = "041";

        //GRUPO CLASIFICADOR FORMULARIOS PARAMETROS CALIDAD
        public static string CodGrupoFormularios { get; } = "042";

        //GRUPO CLASIFICADOR TIPO PRODUCTO
        public static string CodGrupoTipoProducto { get; } = "043";

        //CÓDIGOS DE FORMULARIOS CLASIFICADOR
        public static string EvaluacionLomosMigasBandeja { get; } = "001";
    }
}