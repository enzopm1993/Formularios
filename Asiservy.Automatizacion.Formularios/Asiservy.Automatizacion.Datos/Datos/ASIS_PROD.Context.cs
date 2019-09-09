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
    
        public virtual ObjectResult<spConsutaEmpleados> spConsutaEmpleados(string cedula)
        {
            var cedulaParameter = cedula != null ?
                new ObjectParameter("cedula", cedula) :
                new ObjectParameter("cedula", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsutaEmpleados>("spConsutaEmpleados", cedulaParameter);
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
    
        public virtual ObjectResult<spConsultaOpcionesPorRol> spConsultaOpcionesPorRol(Nullable<int> iDROL)
        {
            var iDROLParameter = iDROL.HasValue ?
                new ObjectParameter("IDROL", iDROL) :
                new ObjectParameter("IDROL", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spConsultaOpcionesPorRol>("spConsultaOpcionesPorRol", iDROLParameter);
        }
    }
}
