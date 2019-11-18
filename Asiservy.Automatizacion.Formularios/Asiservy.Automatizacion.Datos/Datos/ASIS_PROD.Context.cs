﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ASIS_PRODEntities : DbContext
    {
        public ASIS_PRODEntities()
            : base("name=ASIS_PRODEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<JUSTICA_SOLICITUD> JUSTICA_SOLICITUD { get; set; }
        public virtual DbSet<NIVEL_APROBACION> NIVEL_APROBACION { get; set; }
        public virtual DbSet<NIVEL_USUARIO> NIVEL_USUARIO { get; set; }
        public virtual DbSet<OPCION_ROL> OPCION_ROL { get; set; }
        public virtual DbSet<ROL> ROL { get; set; }
        public virtual DbSet<SOLICITUD_PERMISO> SOLICITUD_PERMISO { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<USUARIO_ROL> USUARIO_ROL { get; set; }
        public virtual DbSet<ESTADO_SOLICITUD> ESTADO_SOLICITUD { get; set; }
        public virtual DbSet<BITACORA_SOLICITUD> BITACORA_SOLICITUD { get; set; }
        public virtual DbSet<ERROR> ERROR { get; set; }
        public virtual DbSet<CLASIFICADOR> CLASIFICADOR { get; set; }
        public virtual DbSet<OPCION> OPCION { get; set; }
        public virtual DbSet<BITACORA_CAMBIO_PERSONAL> BITACORA_CAMBIO_PERSONAL { get; set; }
        public virtual DbSet<ASISTENCIA> ASISTENCIA { get; set; }
        public virtual DbSet<CUCHILLO> CUCHILLO { get; set; }
        public virtual DbSet<CONTROL_CUCHILLO> CONTROL_CUCHILLO { get; set; }
        public virtual DbSet<EMPLEADO_CUCHILLO> EMPLEADO_CUCHILLO { get; set; }
        public virtual DbSet<BITACORA_EMPLEADO_TURNO> BITACORA_EMPLEADO_TURNO { get; set; }
        public virtual DbSet<EMPLEADO_TURNO> EMPLEADO_TURNO { get; set; }
        public virtual DbSet<EMPLEADO_ESFERO> EMPLEADO_ESFERO { get; set; }
        public virtual DbSet<CONTROL_ESFERO> CONTROL_ESFERO { get; set; }
        public virtual DbSet<CONTROL_HUESO> CONTROL_HUESO { get; set; }
        public virtual DbSet<CONTROL_HUESO_DETALLE> CONTROL_HUESO_DETALLE { get; set; }
        public virtual DbSet<CONTROL_MIGA> CONTROL_MIGA { get; set; }
        public virtual DbSet<CONTROL_AUDITORIASANGRE> CONTROL_AUDITORIASANGRE { get; set; }
        public virtual DbSet<CONTROL_COCHE_LINEA> CONTROL_COCHE_LINEA { get; set; }
        public virtual DbSet<CONTROL_ENFUNDADO> CONTROL_ENFUNDADO { get; set; }
        public virtual DbSet<CONTROL_ENFUNDADO_DETALLE> CONTROL_ENFUNDADO_DETALLE { get; set; }
        public virtual DbSet<MODULO> MODULO { get; set; }
        public virtual DbSet<BITACORA_MOVER_EMPLEADO> BITACORA_MOVER_EMPLEADO { get; set; }
        public virtual DbSet<PROYECCION_PROGRAMACION> PROYECCION_PROGRAMACION { get; set; }
        public virtual DbSet<PARAMETRO> PARAMETRO { get; set; }
        public virtual DbSet<CAMBIO_PERSONAL> CAMBIO_PERSONAL { get; set; }
        public virtual DbSet<PERIODO> PERIODO { get; set; }
    
        public virtual ObjectResult<spConsultaCodigosEnfermedad> spConsultaCodigosEnfermedad(string codigo)
        {
            var codigoParameter = codigo != null ?
                new ObjectParameter("codigo", codigo) :
                new ObjectParameter("codigo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaCodigosEnfermedad>("spConsultaCodigosEnfermedad", codigoParameter);
        }
    
        public virtual ObjectResult<spConsultaCargos> spConsultaCargos(string codigo)
        {
            var codigoParameter = codigo != null ?
                new ObjectParameter("codigo", codigo) :
                new ObjectParameter("codigo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaCargos>("spConsultaCargos", codigoParameter);
        }
    
        public virtual ObjectResult<string> spConsultarUsuario(string usuario, string clave)
        {
            var usuarioParameter = usuario != null ?
                new ObjectParameter("usuario", usuario) :
                new ObjectParameter("usuario", typeof(string));
    
            var claveParameter = clave != null ?
                new ObjectParameter("Clave", clave) :
                new ObjectParameter("Clave", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("spConsultarUsuario", usuarioParameter, claveParameter);
        }
    
        public virtual ObjectResult<spConsutaMotivosPermiso> spConsutaMotivosPermiso(string dsCodigoMotivo)
        {
            var dsCodigoMotivoParameter = dsCodigoMotivo != null ?
                new ObjectParameter("dsCodigoMotivo", dsCodigoMotivo) :
                new ObjectParameter("dsCodigoMotivo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsutaMotivosPermiso>("spConsutaMotivosPermiso", dsCodigoMotivoParameter);
        }
    
        public virtual ObjectResult<spConsultaLinea> spConsultaLinea(string codigo)
        {
            var codigoParameter = codigo != null ?
                new ObjectParameter("codigo", codigo) :
                new ObjectParameter("codigo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaLinea>("spConsultaLinea", codigoParameter);
        }
    
        public virtual int sp_ReporteSolicitudes()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_ReporteSolicitudes");
        }
    
        public virtual ObjectResult<spConsultaArea> spConsultaArea(string codigo)
        {
            var codigoParameter = codigo != null ?
                new ObjectParameter("codigo", codigo) :
                new ObjectParameter("codigo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaArea>("spConsultaArea", codigoParameter);
        }
    
        public virtual ObjectResult<sp_GrupoEnfermedades> sp_GrupoEnfermedades(string tipo, string codigoGrupoEnfermedad, string codigoSubGrupoEnfermedad)
        {
            var tipoParameter = tipo != null ?
                new ObjectParameter("tipo", tipo) :
                new ObjectParameter("tipo", typeof(string));
    
            var codigoGrupoEnfermedadParameter = codigoGrupoEnfermedad != null ?
                new ObjectParameter("CodigoGrupoEnfermedad", codigoGrupoEnfermedad) :
                new ObjectParameter("CodigoGrupoEnfermedad", typeof(string));
    
            var codigoSubGrupoEnfermedadParameter = codigoSubGrupoEnfermedad != null ?
                new ObjectParameter("CodigoSubGrupoEnfermedad", codigoSubGrupoEnfermedad) :
                new ObjectParameter("CodigoSubGrupoEnfermedad", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GrupoEnfermedades>("sp_GrupoEnfermedades", tipoParameter, codigoGrupoEnfermedadParameter, codigoSubGrupoEnfermedadParameter);
        }
    
        public virtual ObjectResult<sp_ConsultaEmpleadosMovidos> sp_ConsultaEmpleadosMovidos(string cedula)
        {
            var cedulaParameter = cedula != null ?
                new ObjectParameter("Cedula", cedula) :
                new ObjectParameter("Cedula", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_ConsultaEmpleadosMovidos>("sp_ConsultaEmpleadosMovidos", cedulaParameter);
        }
    
        public virtual ObjectResult<string> sp_ConsultaMotivoSolicitudPermisoAsistencia(string cedula)
        {
            var cedulaParameter = cedula != null ?
                new ObjectParameter("cedula", cedula) :
                new ObjectParameter("cedula", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_ConsultaMotivoSolicitudPermisoAsistencia", cedulaParameter);
        }
    
        public virtual ObjectResult<spConsutaEmpleadosTurnos> spConsutaEmpleadosTurnos(string linea)
        {
            var lineaParameter = linea != null ?
                new ObjectParameter("linea", linea) :
                new ObjectParameter("linea", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsutaEmpleadosTurnos>("spConsutaEmpleadosTurnos", lineaParameter);
        }
    
        public virtual ObjectResult<spConsutaReporteEmpleadosTurnos> spConsutaReporteEmpleadosTurnos(string linea, string turno)
        {
            var lineaParameter = linea != null ?
                new ObjectParameter("linea", linea) :
                new ObjectParameter("linea", typeof(string));
    
            var turnoParameter = turno != null ?
                new ObjectParameter("turno", turno) :
                new ObjectParameter("turno", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsutaReporteEmpleadosTurnos>("spConsutaReporteEmpleadosTurnos", lineaParameter, turnoParameter);
        }
    
        public virtual ObjectResult<spConsutaEmpleadoEsfero> spConsutaEmpleadoEsfero(string linea)
        {
            var lineaParameter = linea != null ?
                new ObjectParameter("linea", linea) :
                new ObjectParameter("linea", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsutaEmpleadoEsfero>("spConsutaEmpleadoEsfero", lineaParameter);
        }
    
        public virtual ObjectResult<spConsutaControlEsferos> spConsutaControlEsferos(string linea, Nullable<System.DateTime> fecha, string tipo)
        {
            var lineaParameter = linea != null ?
                new ObjectParameter("linea", linea) :
                new ObjectParameter("linea", typeof(string));
    
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("fecha", fecha) :
                new ObjectParameter("fecha", typeof(System.DateTime));
    
            var tipoParameter = tipo != null ?
                new ObjectParameter("tipo", tipo) :
                new ObjectParameter("tipo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsutaControlEsferos>("spConsutaControlEsferos", lineaParameter, fechaParameter, tipoParameter);
        }
    
        public virtual ObjectResult<spConsultaControlAsistencia> spConsultaControlAsistencia(string linea, Nullable<System.DateTime> fecha)
        {
            var lineaParameter = linea != null ?
                new ObjectParameter("Linea", linea) :
                new ObjectParameter("Linea", typeof(string));
    
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("Fecha", fecha) :
                new ObjectParameter("Fecha", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaControlAsistencia>("spConsultaControlAsistencia", lineaParameter, fechaParameter);
        }
    
        public virtual ObjectResult<sp_ConsultaAsistenciaDiariaPersonalMovido> sp_ConsultaAsistenciaDiariaPersonalMovido(string codLinea, Nullable<int> turno, Nullable<System.DateTime> fecha)
        {
            var codLineaParameter = codLinea != null ?
                new ObjectParameter("codLinea", codLinea) :
                new ObjectParameter("codLinea", typeof(string));
    
            var turnoParameter = turno.HasValue ?
                new ObjectParameter("Turno", turno) :
                new ObjectParameter("Turno", typeof(int));
    
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("Fecha", fecha) :
                new ObjectParameter("Fecha", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_ConsultaAsistenciaDiariaPersonalMovido>("sp_ConsultaAsistenciaDiariaPersonalMovido", codLineaParameter, turnoParameter, fechaParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> sp_ObtenerCuchillosSobrantes(string colorCuchillo)
        {
            var colorCuchilloParameter = colorCuchillo != null ?
                new ObjectParameter("ColorCuchillo", colorCuchillo) :
                new ObjectParameter("ColorCuchillo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("sp_ObtenerCuchillosSobrantes", colorCuchilloParameter);
        }
    
        public virtual ObjectResult<spConsultarEmpleadosxTurno> spConsultarEmpleadosxTurno(string codLinea, string turno)
        {
            var codLineaParameter = codLinea != null ?
                new ObjectParameter("CodLinea", codLinea) :
                new ObjectParameter("CodLinea", typeof(string));
    
            var turnoParameter = turno != null ?
                new ObjectParameter("Turno", turno) :
                new ObjectParameter("Turno", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultarEmpleadosxTurno>("spConsultarEmpleadosxTurno", codLineaParameter, turnoParameter);
        }
    
        public virtual ObjectResult<spConsultaControlHueso> spConsultaControlHueso(Nullable<System.DateTime> fecha)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("fecha", fecha) :
                new ObjectParameter("fecha", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaControlHueso>("spConsultaControlHueso", fechaParameter);
        }
    
        public virtual ObjectResult<spConsultarCambioPersonalxLineaxTurno> spConsultarCambioPersonalxLineaxTurno(string linea, string turno)
        {
            var lineaParameter = linea != null ?
                new ObjectParameter("Linea", linea) :
                new ObjectParameter("Linea", typeof(string));
    
            var turnoParameter = turno != null ?
                new ObjectParameter("Turno", turno) :
                new ObjectParameter("Turno", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultarCambioPersonalxLineaxTurno>("spConsultarCambioPersonalxLineaxTurno", lineaParameter, turnoParameter);
        }
    
        public virtual ObjectResult<spConsultaControlHuesoDetalle> spConsultaControlHuesoDetalle(Nullable<int> idControlHueso)
        {
            var idControlHuesoParameter = idControlHueso.HasValue ?
                new ObjectParameter("IdControlHueso", idControlHueso) :
                new ObjectParameter("IdControlHueso", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaControlHuesoDetalle>("spConsultaControlHuesoDetalle", idControlHuesoParameter);
        }
    
        public virtual ObjectResult<spConsultaControlAvanceDiarioPorLinea> spConsultaControlAvanceDiarioPorLinea(Nullable<System.DateTime> fecha, string linea)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("fecha", fecha) :
                new ObjectParameter("fecha", typeof(System.DateTime));
    
            var lineaParameter = linea != null ?
                new ObjectParameter("linea", linea) :
                new ObjectParameter("linea", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaControlAvanceDiarioPorLinea>("spConsultaControlAvanceDiarioPorLinea", fechaParameter, lineaParameter);
        }
    
        public virtual ObjectResult<spConsultaAvanceDiarioPorLimpiadora> spConsultaAvanceDiarioPorLimpiadora(Nullable<System.DateTime> fecha, string linea)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("fecha", fecha) :
                new ObjectParameter("fecha", typeof(System.DateTime));
    
            var lineaParameter = linea != null ?
                new ObjectParameter("linea", linea) :
                new ObjectParameter("linea", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaAvanceDiarioPorLimpiadora>("spConsultaAvanceDiarioPorLimpiadora", fechaParameter, lineaParameter);
        }
    
        public virtual ObjectResult<spConsultaPersonalNominaPorLinea> spConsultaPersonalNominaPorLinea()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaPersonalNominaPorLinea>("spConsultaPersonalNominaPorLinea");
        }
    
        public virtual ObjectResult<spConsultaReporteControlCuchillo> spConsultaReporteControlCuchillo(Nullable<System.DateTime> fecha, string linea)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("fecha", fecha) :
                new ObjectParameter("fecha", typeof(System.DateTime));
    
            var lineaParameter = linea != null ?
                new ObjectParameter("linea", linea) :
                new ObjectParameter("linea", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaReporteControlCuchillo>("spConsultaReporteControlCuchillo", fechaParameter, lineaParameter);
        }
    
        public virtual ObjectResult<spConsultarCaambioPersonalxCedula> spConsultarCaambioPersonalxCedula(string cedula)
        {
            var cedulaParameter = cedula != null ?
                new ObjectParameter("Cedula", cedula) :
                new ObjectParameter("Cedula", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultarCaambioPersonalxCedula>("spConsultarCaambioPersonalxCedula", cedulaParameter);
        }
    
        public virtual ObjectResult<spReporteCambioPersonal> spReporteCambioPersonal(string codLinea, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var codLineaParameter = codLinea != null ?
                new ObjectParameter("CodLinea", codLinea) :
                new ObjectParameter("CodLinea", typeof(string));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("fechaFin", fechaFin) :
                new ObjectParameter("fechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spReporteCambioPersonal>("spReporteCambioPersonal", codLineaParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<spConsultaReporteControlCochePorLineas> spConsultaReporteControlCochePorLineas(Nullable<System.DateTime> fecha)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("fecha", fecha) :
                new ObjectParameter("fecha", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaReporteControlCochePorLineas>("spConsultaReporteControlCochePorLineas", fechaParameter);
        }
    
        public virtual ObjectResult<spConsultaDistribucionPorLinea> spConsultaDistribucionPorLinea(Nullable<System.DateTime> fecha, string linea)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("fecha", fecha) :
                new ObjectParameter("fecha", typeof(System.DateTime));
    
            var lineaParameter = linea != null ?
                new ObjectParameter("linea", linea) :
                new ObjectParameter("linea", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaDistribucionPorLinea>("spConsultaDistribucionPorLinea", fechaParameter, lineaParameter);
        }
    
        public virtual ObjectResult<spConsultaControlEnfundado> spConsultaControlEnfundado(Nullable<System.DateTime> fecha)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("fecha", fecha) :
                new ObjectParameter("fecha", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaControlEnfundado>("spConsultaControlEnfundado", fechaParameter);
        }
    
        public virtual ObjectResult<spConsultaControlEnfundadoDetalle> spConsultaControlEnfundadoDetalle(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaControlEnfundadoDetalle>("spConsultaControlEnfundadoDetalle", idParameter);
        }
    
        public virtual ObjectResult<spReporteAsistencia> spReporteAsistencia(Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin, string turno, string linea)
        {
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            var turnoParameter = turno != null ?
                new ObjectParameter("Turno", turno) :
                new ObjectParameter("Turno", typeof(string));
    
            var lineaParameter = linea != null ?
                new ObjectParameter("Linea", linea) :
                new ObjectParameter("Linea", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spReporteAsistencia>("spReporteAsistencia", fechaInicioParameter, fechaFinParameter, turnoParameter, lineaParameter);
        }
    
        public virtual ObjectResult<spReporteAuditoriaSangre> spReporteAuditoriaSangre(string codLinea, Nullable<System.DateTime> fECHA)
        {
            var codLineaParameter = codLinea != null ?
                new ObjectParameter("CodLinea", codLinea) :
                new ObjectParameter("CodLinea", typeof(string));
    
            var fECHAParameter = fECHA.HasValue ?
                new ObjectParameter("FECHA", fECHA) :
                new ObjectParameter("FECHA", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spReporteAuditoriaSangre>("spReporteAuditoriaSangre", codLineaParameter, fECHAParameter);
        }
    
        public virtual ObjectResult<sp_ConsultaAsistenciaDiaria> sp_ConsultaAsistenciaDiaria(string codLinea, Nullable<int> turno, Nullable<System.DateTime> fecha)
        {
            var codLineaParameter = codLinea != null ?
                new ObjectParameter("codLinea", codLinea) :
                new ObjectParameter("codLinea", typeof(string));
    
            var turnoParameter = turno.HasValue ?
                new ObjectParameter("Turno", turno) :
                new ObjectParameter("Turno", typeof(int));
    
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("Fecha", fecha) :
                new ObjectParameter("Fecha", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_ConsultaAsistenciaDiaria>("sp_ConsultaAsistenciaDiaria", codLineaParameter, turnoParameter, fechaParameter);
        }
    
        public virtual ObjectResult<spConsultaLimpiadorasControlHueso> spConsultaLimpiadorasControlHueso(string linea, Nullable<System.DateTime> fecha)
        {
            var lineaParameter = linea != null ?
                new ObjectParameter("linea", linea) :
                new ObjectParameter("linea", typeof(string));
    
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("fecha", fecha) :
                new ObjectParameter("fecha", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaLimpiadorasControlHueso>("spConsultaLimpiadorasControlHueso", lineaParameter, fechaParameter);
        }
    
        public virtual ObjectResult<spConsultaUltimaMarcacionBiometrico> spConsultaUltimaMarcacionBiometrico(string cedula)
        {
            var cedulaParameter = cedula != null ?
                new ObjectParameter("cedula", cedula) :
                new ObjectParameter("cedula", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaUltimaMarcacionBiometrico>("spConsultaUltimaMarcacionBiometrico", cedulaParameter);
        }
    
        public virtual ObjectResult<sp_ConsultaAsistenciaGeneralDiaria> sp_ConsultaAsistenciaGeneralDiaria(string codLinea, Nullable<int> turno, Nullable<System.DateTime> fecha)
        {
            var codLineaParameter = codLinea != null ?
                new ObjectParameter("codLinea", codLinea) :
                new ObjectParameter("codLinea", typeof(string));
    
            var turnoParameter = turno.HasValue ?
                new ObjectParameter("Turno", turno) :
                new ObjectParameter("Turno", typeof(int));
    
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("Fecha", fecha) :
                new ObjectParameter("Fecha", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_ConsultaAsistenciaGeneralDiaria>("sp_ConsultaAsistenciaGeneralDiaria", codLineaParameter, turnoParameter, fechaParameter);
        }
    
        public virtual ObjectResult<spConsultaEmpleadoCargoPorLinea> spConsultaEmpleadoCargoPorLinea(string linea)
        {
            var lineaParameter = linea != null ?
                new ObjectParameter("Linea", linea) :
                new ObjectParameter("Linea", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaEmpleadoCargoPorLinea>("spConsultaEmpleadoCargoPorLinea", lineaParameter);
        }
    
        public virtual ObjectResult<spConsultaOpcionesPorRol> spConsultaOpcionesPorRol(Nullable<int> iDROL)
        {
            var iDROLParameter = iDROL.HasValue ?
                new ObjectParameter("IDROL", iDROL) :
                new ObjectParameter("IDROL", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaOpcionesPorRol>("spConsultaOpcionesPorRol", iDROLParameter);
        }
    
        public virtual ObjectResult<spConsultaCuchilloEmpleado> spConsultaCuchilloEmpleado(string cedula, string linea)
        {
            var cedulaParameter = cedula != null ?
                new ObjectParameter("cedula", cedula) :
                new ObjectParameter("cedula", typeof(string));
    
            var lineaParameter = linea != null ?
                new ObjectParameter("linea", linea) :
                new ObjectParameter("linea", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaCuchilloEmpleado>("spConsultaCuchilloEmpleado", cedulaParameter, lineaParameter);
        }
    
        public virtual ObjectResult<spConsutaEmpleadosCuchillos> spConsutaEmpleadosCuchillos(string linea, string estado, Nullable<System.DateTime> fecha, Nullable<bool> control)
        {
            var lineaParameter = linea != null ?
                new ObjectParameter("linea", linea) :
                new ObjectParameter("linea", typeof(string));
    
            var estadoParameter = estado != null ?
                new ObjectParameter("estado", estado) :
                new ObjectParameter("estado", typeof(string));
    
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("fecha", fecha) :
                new ObjectParameter("fecha", typeof(System.DateTime));
    
            var controlParameter = control.HasValue ?
                new ObjectParameter("control", control) :
                new ObjectParameter("control", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsutaEmpleadosCuchillos>("spConsutaEmpleadosCuchillos", lineaParameter, estadoParameter, fechaParameter, controlParameter);
        }
    
        public virtual ObjectResult<spConsultaPersonalADondeFueronMovidos> spConsultaPersonalADondeFueronMovidos(string lINEA)
        {
            var lINEAParameter = lINEA != null ?
                new ObjectParameter("LINEA", lINEA) :
                new ObjectParameter("LINEA", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaPersonalADondeFueronMovidos>("spConsultaPersonalADondeFueronMovidos", lINEAParameter);
        }
    
        public virtual ObjectResult<spConsultarAuditoriaSangreDiaria> spConsultarAuditoriaSangreDiaria(Nullable<System.DateTime> fecha)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("Fecha", fecha) :
                new ObjectParameter("Fecha", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultarAuditoriaSangreDiaria>("spConsultarAuditoriaSangreDiaria", fechaParameter);
        }
    
        public virtual ObjectResult<spConsultaCodigoOnlyControl> spConsultaCodigoOnlyControl(string cedula)
        {
            var cedulaParameter = cedula != null ?
                new ObjectParameter("cedula", cedula) :
                new ObjectParameter("cedula", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaCodigoOnlyControl>("spConsultaCodigoOnlyControl", cedulaParameter);
        }
    
        public virtual ObjectResult<spConsultaOpcionModulo> spConsultaOpcionModulo()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaOpcionModulo>("spConsultaOpcionModulo");
        }
    
        public virtual ObjectResult<SP_PKI_SOLICITUDES> SP_PKI_SOLICITUDES()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_PKI_SOLICITUDES>("SP_PKI_SOLICITUDES");
        }
    
        public virtual ObjectResult<spConsutaEmpleados> spConsutaEmpleados(string cedula)
        {
            var cedulaParameter = cedula != null ?
                new ObjectParameter("cedula", cedula) :
                new ObjectParameter("cedula", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsutaEmpleados>("spConsutaEmpleados", cedulaParameter);
        }
    
        public virtual ObjectResult<spConsultaCambioPersonalFecha> spConsultaCambioPersonalFecha(Nullable<System.DateTime> fecha, Nullable<System.TimeSpan> hora)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("Fecha", fecha) :
                new ObjectParameter("Fecha", typeof(System.DateTime));
    
            var horaParameter = hora.HasValue ?
                new ObjectParameter("Hora", hora) :
                new ObjectParameter("Hora", typeof(System.TimeSpan));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaCambioPersonalFecha>("spConsultaCambioPersonalFecha", fechaParameter, horaParameter);
        }
    
        public virtual ObjectResult<spConsultaCargosXRecursoLinea> spConsultaCargosXRecursoLinea(string recurso, string linea)
        {
            var recursoParameter = recurso != null ?
                new ObjectParameter("Recurso", recurso) :
                new ObjectParameter("Recurso", typeof(string));
    
            var lineaParameter = linea != null ?
                new ObjectParameter("Linea", linea) :
                new ObjectParameter("Linea", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaCargosXRecursoLinea>("spConsultaCargosXRecursoLinea", recursoParameter, lineaParameter);
        }
    
        public virtual ObjectResult<spConsultaCentroCostos> spConsultaCentroCostos()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaCentroCostos>("spConsultaCentroCostos");
        }
    
        public virtual ObjectResult<spConsultaLineaXRecursoyCentroCosto> spConsultaLineaXRecursoyCentroCosto(string recurso, string centroCostos)
        {
            var recursoParameter = recurso != null ?
                new ObjectParameter("recurso", recurso) :
                new ObjectParameter("recurso", typeof(string));
    
            var centroCostosParameter = centroCostos != null ?
                new ObjectParameter("CentroCostos", centroCostos) :
                new ObjectParameter("CentroCostos", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaLineaXRecursoyCentroCosto>("spConsultaLineaXRecursoyCentroCosto", recursoParameter, centroCostosParameter);
        }
    
        public virtual ObjectResult<spConsultaRecurso> spConsultaRecurso(string centroCostos)
        {
            var centroCostosParameter = centroCostos != null ?
                new ObjectParameter("CentroCostos", centroCostos) :
                new ObjectParameter("CentroCostos", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaRecurso>("spConsultaRecurso", centroCostosParameter);
        }
    
        public virtual ObjectResult<spConsutaEmpleadosFiltro> spConsutaEmpleadosFiltro(string area, string linea, string cargo)
        {
            var areaParameter = area != null ?
                new ObjectParameter("Area", area) :
                new ObjectParameter("Area", typeof(string));
    
            var lineaParameter = linea != null ?
                new ObjectParameter("Linea", linea) :
                new ObjectParameter("Linea", typeof(string));
    
            var cargoParameter = cargo != null ?
                new ObjectParameter("Cargo", cargo) :
                new ObjectParameter("Cargo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsutaEmpleadosFiltro>("spConsutaEmpleadosFiltro", areaParameter, lineaParameter, cargoParameter);
        }
    
        public virtual ObjectResult<spConsutaEmpleadosFiltroCambioPersonal> spConsutaEmpleadosFiltroCambioPersonal(string area, string linea, string cargo, string recurso, string tipo)
        {
            var areaParameter = area != null ?
                new ObjectParameter("Area", area) :
                new ObjectParameter("Area", typeof(string));
    
            var lineaParameter = linea != null ?
                new ObjectParameter("Linea", linea) :
                new ObjectParameter("Linea", typeof(string));
    
            var cargoParameter = cargo != null ?
                new ObjectParameter("Cargo", cargo) :
                new ObjectParameter("Cargo", typeof(string));
    
            var recursoParameter = recurso != null ?
                new ObjectParameter("Recurso", recurso) :
                new ObjectParameter("Recurso", typeof(string));
    
            var tipoParameter = tipo != null ?
                new ObjectParameter("Tipo", tipo) :
                new ObjectParameter("Tipo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsutaEmpleadosFiltroCambioPersonal>("spConsutaEmpleadosFiltroCambioPersonal", areaParameter, lineaParameter, cargoParameter, recursoParameter, tipoParameter);
        }
    
        public virtual ObjectResult<spReporteControlEnfundadoPorEnfundadora> spReporteControlEnfundadoPorEnfundadora(Nullable<System.DateTime> fecha)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("Fecha", fecha) :
                new ObjectParameter("Fecha", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spReporteControlEnfundadoPorEnfundadora>("spReporteControlEnfundadoPorEnfundadora", fechaParameter);
        }
    
        public virtual ObjectResult<spReporteControlEnfundadoPorHora> spReporteControlEnfundadoPorHora(Nullable<System.DateTime> fecha)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("Fecha", fecha) :
                new ObjectParameter("Fecha", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spReporteControlEnfundadoPorHora>("spReporteControlEnfundadoPorHora", fechaParameter);
        }
    }
}
