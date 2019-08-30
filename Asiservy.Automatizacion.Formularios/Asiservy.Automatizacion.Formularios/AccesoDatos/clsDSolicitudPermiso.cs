using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDSolicitudPermiso
    {
        ASIS_PRODEntities entities = null;

        public string GuargarModificarSolicitud(SOLICITUD_PERMISO doSolicitud)
        {
          
            string psMensaje = string.Empty;
            entities = new ASIS_PRODEntities();
            var poSolicitud = entities.SOLICITUD_PERMISO.FirstOrDefault(x => x.IdSolicitudPermiso == doSolicitud.IdSolicitudPermiso);
            if (poSolicitud != null)
            {
                poSolicitud.Observacion += doSolicitud.Observacion;
                poSolicitud.EstadoSolicitud = doSolicitud.EstadoSolicitud;
                poSolicitud.FechaModificacionLog = doSolicitud.FechaModificacionLog;
                poSolicitud.TerminalModificacionLog = doSolicitud.TerminalModificacionLog;
                poSolicitud.UsuarioModificacionLog = doSolicitud.UsuarioModificacionLog;
                psMensaje = "Registro Actualizado Correctamente";
            }
            else
            {
                entities.SOLICITUD_PERMISO.Add(doSolicitud);
                psMensaje = "Registro Guardado Correctamente";
            }
            entities.SaveChanges();
            return psMensaje;
           
        }

        public List<spConsutaMotivosPermiso> ConsultarMotivos(string tipo)
        {
            entities = new ASIS_PRODEntities();

           var lista= entities.spConsutaMotivosPermiso(tipo).ToList();


            return lista;
        }
        public List<SolicitudPermisoViewModel> ConsultaSolicitudesPermiso(string dsEstadoSolcitud)
        {
            entities = new ASIS_PRODEntities();
            List<SolicitudPermisoViewModel> ListaSolicitudesPermiso = new List<SolicitudPermisoViewModel>();
            var lista = entities.SOLICITUD_PERMISO.Where(x=> x.EstadoSolicitud== dsEstadoSolcitud && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
            foreach(var x in lista)
            {
                List<JUSTICA_SOLICITUD> ListadoJustificaciones = new List<JUSTICA_SOLICITUD>();
                var detalle = entities.JUSTICA_SOLICITUD.Where(y => y.IdSolicitudPermiso == x.IdSolicitudPermiso).ToList();
                foreach(var d in detalle)
                {
                    ListadoJustificaciones.Add(new JUSTICA_SOLICITUD
                    {
                        IdJustificaSolicitud=d.IdJustificaSolicitud,
                        IdSolicitudPermiso=x.IdSolicitudPermiso,
                        CodigoMotivo=d.CodigoMotivo,
                        FechaSalida=d.FechaSalida,
                        FechaRegreso=d.FechaRegreso,
                        UsuarioIngresoLog=d.UsuarioIngresoLog,
                        FechaIngresoLog=d.FechaIngresoLog,
                        TerminalIngresoLog=d.TerminalIngresoLog,
                        UsuarioModificacionLog=d.UsuarioModificacionLog,
                        TerminalModificacionLog=d.TerminalModificacionLog,
                        FechaModificacionLog=d.FechaModificacionLog
                    });
                }
                
                var poMotivoPermiso = entities.spConsutaMotivosPermiso("0").FirstOrDefault(m => m.CodigoMotivo == x.CodigoMotivo);
                var poEmpleado = entities.spConsutaEmpleados(x.Identificacion).FirstOrDefault();
                ListaSolicitudesPermiso.Add(new SolicitudPermisoViewModel
                {
                    IdSolicitudPermiso = x.IdSolicitudPermiso,
                    CodigoLinea = x.CodigoLinea,
                    DescripcionLinea= poEmpleado!=null? poEmpleado.LINEA:"",
                    CodigoArea = x.CodigoArea,
                    DescripcionArea= poEmpleado != null ? poEmpleado.AREA : "",
                    CodigoCargo = x.CodigoCargo,
                    DescripcionCargo= poEmpleado != null ? poEmpleado.CARGO : "",
                    Identificacion = x.Identificacion,
                    NombreEmpleado= poEmpleado!=null? poEmpleado.NOMBRES:"",
                    CodigoMotivo = x.CodigoMotivo,
                    DescripcionMotivo= poMotivoPermiso != null ? poMotivoPermiso.Descripcion : "",
                    Observacion = x.Observacion,
                    FechaSalida = x.FechaSalida,
                    FechaRegreso = x.FechaRegreso,
                    EstadoSolicitud=x.EstadoSolicitud,
                    FechaBiometrico=x.FechaBiometrico,
                    Origen=char.Parse(x.Origen),
                    CodigoDiagnostico=x.CodigoDiagnostico,
                    FechaIngresoLog=x.FechaIngresoLog,
                    UsuarioIngresoLog=x.UsuarioIngresoLog,
                    TerminalIngresoLog=x.TerminalIngresoLog,
                    UsuarioModificacionLog=x.UsuarioModificacionLog,
                    FechaModificacionLog=x.FechaModificacionLog,
                    TerminalModificacionLog=x.TerminalModificacionLog
                });
            }
            return ListaSolicitudesPermiso;
        }
        public SolicitudPermisoViewModel ConsultaSolicitudPermiso(string dsSolicitud)
        {
            entities = new ASIS_PRODEntities();
            SolicitudPermisoViewModel ListaSolicitudesPermiso ;
            int id = int.Parse(dsSolicitud);
            var lista = entities.SOLICITUD_PERMISO.FirstOrDefault(x => x.IdSolicitudPermiso == id && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
            

                List<JUSTICA_SOLICITUD> ListadoJustificaciones = new List<JUSTICA_SOLICITUD>();
                var detalle = entities.JUSTICA_SOLICITUD.Where(y => y.IdSolicitudPermiso == lista.IdSolicitudPermiso).ToList();
                foreach (var d in detalle)
                {
                    ListadoJustificaciones.Add(new JUSTICA_SOLICITUD
                    {
                        IdJustificaSolicitud = d.IdJustificaSolicitud,
                        IdSolicitudPermiso = lista.IdSolicitudPermiso,
                        CodigoMotivo = d.CodigoMotivo,
                        FechaSalida = d.FechaSalida,
                        FechaRegreso = d.FechaRegreso,
                        UsuarioIngresoLog = d.UsuarioIngresoLog,
                        FechaIngresoLog = d.FechaIngresoLog,
                        TerminalIngresoLog = d.TerminalIngresoLog,
                        UsuarioModificacionLog = d.UsuarioModificacionLog,
                        TerminalModificacionLog = d.TerminalModificacionLog,
                        FechaModificacionLog = d.FechaModificacionLog
                    });
                }

                var poMotivoPermiso = entities.spConsutaMotivosPermiso("0").FirstOrDefault(m => m.CodigoMotivo == lista.CodigoMotivo);
                var poEmpleado = entities.spConsutaEmpleados(lista.Identificacion).FirstOrDefault();
            ListaSolicitudesPermiso = new SolicitudPermisoViewModel
            {
                IdSolicitudPermiso = lista.IdSolicitudPermiso,
                CodigoLinea = lista.CodigoLinea,
                DescripcionLinea = poEmpleado != null ? poEmpleado.LINEA : "",
                CodigoArea = lista.CodigoArea,
                DescripcionArea = poEmpleado != null ? poEmpleado.AREA : "",
                CodigoCargo = lista.CodigoCargo,
                DescripcionCargo = poEmpleado != null ? poEmpleado.CARGO : "",
                Identificacion = lista.Identificacion,
                NombreEmpleado = poEmpleado != null ? poEmpleado.NOMBRES : "",
                CodigoMotivo = lista.CodigoMotivo,
                DescripcionMotivo = poMotivoPermiso != null ? poMotivoPermiso.Descripcion : "",
                Observacion = lista.Observacion,
                FechaSalida = lista.FechaSalida,
                FechaRegreso = lista.FechaRegreso,
                EstadoSolicitud = lista.EstadoSolicitud,
                FechaBiometrico = lista.FechaBiometrico,
                Origen = char.Parse(lista.Origen),
                CodigoDiagnostico = lista.CodigoDiagnostico,
                FechaIngresoLog = lista.FechaIngresoLog,
                UsuarioIngresoLog = lista.UsuarioIngresoLog,
                TerminalIngresoLog = lista.TerminalIngresoLog,
                UsuarioModificacionLog = lista.UsuarioModificacionLog,
                FechaModificacionLog = lista.FechaModificacionLog,
                TerminalModificacionLog = lista.TerminalModificacionLog
            };
           
            return ListaSolicitudesPermiso;
        }
        public int ConsultarNivelUsuario(string dsUsuario)
        {
            int piNivel = 0;
            entities = new ASIS_PRODEntities();
            var poNivelUsuario= entities.NIVEL_USUARIO.FirstOrDefault(x => x.IdUsuario == dsUsuario);
            if(poNivelUsuario!=null)
            {
                piNivel = poNivelUsuario.Nivel??0;
            }
            return piNivel;
        }



    }
} 