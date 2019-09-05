﻿using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDSolicitudPermiso
    {
        ASIS_PRODEntities entities = null;
        public string CambioEstadoSolicitud(SOLICITUD_PERMISO doSolicitud)
        {
            string psMensaje = "No se pudo cambiar de estado a la solicitud";
            entities = new ASIS_PRODEntities();
            var poSolicitud = entities.SOLICITUD_PERMISO.FirstOrDefault(x => x.IdSolicitudPermiso == doSolicitud.IdSolicitudPermiso);
            if (poSolicitud != null)
            {
                poSolicitud.Observacion += doSolicitud.Observacion ?? "";
                poSolicitud.EstadoSolicitud = doSolicitud.EstadoSolicitud ?? poSolicitud.EstadoSolicitud;
                poSolicitud.FechaModificacionLog = doSolicitud.FechaModificacionLog;
                poSolicitud.TerminalModificacionLog = doSolicitud.TerminalModificacionLog;
                poSolicitud.UsuarioModificacionLog = doSolicitud.UsuarioModificacionLog;
                psMensaje = "Registro Actualizado Correctamente";

                BITACORA_SOLICITUD poBitacora = new BITACORA_SOLICITUD();
                poBitacora.IdSolicitud = poSolicitud.IdSolicitudPermiso;
                poBitacora.Cedula = poSolicitud.Identificacion;
                poBitacora.Observacion = poSolicitud.Observacion;
                poBitacora.EstadoSolicitud = poSolicitud.EstadoSolicitud;
                poBitacora.FechaIngresoLog = DateTime.Now;
                poBitacora.UsuarioIngresoLog = doSolicitud.UsuarioModificacionLog;
                poBitacora.TerminalIngresoLog = doSolicitud.TerminalModificacionLog;
                entities.BITACORA_SOLICITUD.Add(poBitacora);
            }
            

            entities.SaveChanges();
            return psMensaje;
        }
        
        public string GuargarModificarSolicitud(SOLICITUD_PERMISO doSolicitud)
        {
            string psMensaje = string.Empty;
            BITACORA_SOLICITUD poBitacora = new BITACORA_SOLICITUD();

            using (ASIS_PRODEntities entities = new ASIS_PRODEntities()) {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    var poSolicitud = entities.SOLICITUD_PERMISO.FirstOrDefault(x => x.IdSolicitudPermiso == doSolicitud.IdSolicitudPermiso);
                    if (poSolicitud != null)
                    {
                        poSolicitud.CodigoMotivo = doSolicitud.CodigoMotivo ?? poSolicitud.CodigoMotivo;
                        poSolicitud.Observacion = doSolicitud.Observacion ?? "";
                        poSolicitud.FechaSalida = doSolicitud.FechaSalida;
                        poSolicitud.FechaRegreso = doSolicitud.FechaRegreso;
                        poSolicitud.FechaModificacionLog = doSolicitud.FechaModificacionLog;
                        poSolicitud.TerminalModificacionLog = doSolicitud.TerminalModificacionLog;
                        poSolicitud.UsuarioModificacionLog = doSolicitud.UsuarioModificacionLog;
                        psMensaje = "Registro Actualizado Correctamente";

                        poBitacora.IdSolicitud = poSolicitud.IdSolicitudPermiso;
                        poBitacora.Cedula = poSolicitud.Identificacion;
                        poBitacora.Observacion = poSolicitud.Observacion;
                        poBitacora.EstadoSolicitud = poSolicitud.EstadoSolicitud;
                        poBitacora.FechaSalida = poSolicitud.FechaSalida;
                        poBitacora.FechaRegreso = poSolicitud.FechaRegreso;
                    }
                    else
                    {
                        entities.SOLICITUD_PERMISO.Add(doSolicitud);
                        psMensaje = "Registro Guardado Correctamente";


                        poBitacora.IdSolicitud = doSolicitud.IdSolicitudPermiso;
                        poBitacora.Cedula = doSolicitud.Identificacion;
                        poBitacora.Observacion = doSolicitud.Observacion;
                        poBitacora.EstadoSolicitud = doSolicitud.EstadoSolicitud;
                        poBitacora.FechaSalida = doSolicitud.FechaSalida;
                        poBitacora.FechaRegreso = doSolicitud.FechaRegreso;

                    }

                    foreach (var detalle in doSolicitud.JUSTICA_SOLICITUD)
                    {
                        var poDetalle = entities.JUSTICA_SOLICITUD.Where(x =>
                        x.IdSolicitudPermiso == detalle.IdSolicitudPermiso
                        && x.IdJustificaSolicitud == detalle.IdJustificaSolicitud).FirstOrDefault();

                        if (poDetalle != null)
                        {
                            poDetalle.CodigoMotivo = detalle.CodigoMotivo;
                            poDetalle.FechaSalida = detalle.FechaSalida ?? poDetalle.FechaSalida;
                            poDetalle.FechaRegreso = detalle.FechaRegreso ?? poDetalle.FechaRegreso;
                            poDetalle.FechaModificacionLog = DateTime.Now;
                            poDetalle.UsuarioModificacionLog = doSolicitud.UsuarioModificacionLog;
                            poDetalle.TerminalModificacionLog = doSolicitud.TerminalModificacionLog;
                        }
                        else
                        {
                            detalle.IdSolicitudPermiso = doSolicitud.IdSolicitudPermiso;
                            detalle.FechaIngresoLog = DateTime.Now;
                            detalle.UsuarioIngresoLog = doSolicitud.UsuarioModificacionLog;
                            detalle.TerminalIngresoLog = doSolicitud.TerminalModificacionLog;
                            entities.JUSTICA_SOLICITUD.Add(detalle);
                        }

                    }


                    poBitacora.FechaIngresoLog = DateTime.Now;
                    poBitacora.UsuarioIngresoLog = doSolicitud.UsuarioIngresoLog ?? doSolicitud.UsuarioModificacionLog;
                    poBitacora.TerminalIngresoLog = doSolicitud.TerminalIngresoLog ?? doSolicitud.TerminalModificacionLog;
                    entities.SaveChanges();

                    var psIdentificacion = doSolicitud.Identificacion ?? poSolicitud.Identificacion;
                    var sol = entities.SOLICITUD_PERMISO.Where(x => x.Identificacion == psIdentificacion).OrderByDescending(x => x.FechaIngresoLog).FirstOrDefault();
                    poBitacora.IdSolicitud = sol.IdSolicitudPermiso;
                    entities.BITACORA_SOLICITUD.Add(poBitacora);
                    entities.SaveChanges();
                    transaction.Commit();
                }
            }
            return psMensaje;

        }

        public List<spConsutaMotivosPermiso> ConsultarMotivos(string tipo)
        {
            entities = new ASIS_PRODEntities();

           var lista= entities.spConsutaMotivosPermiso(tipo).ToList();


            return lista;
        }
        public List<SolicitudPermisoViewModel> ConsultaSolicitudesPermisoReporte(string dsEstadoSolcitud)
        {
            entities = new ASIS_PRODEntities();
            List<SolicitudPermisoViewModel> ListaSolicitudesPermiso = new List<SolicitudPermisoViewModel>();
            List<SOLICITUD_PERMISO> Lista = new List<SOLICITUD_PERMISO>();
            
            if (dsEstadoSolcitud == clsAtributos.EstadoSolicitudTodos)
            {
                Lista = entities.SOLICITUD_PERMISO.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
            else
            {
                Lista = entities.SOLICITUD_PERMISO.Where(x => x.EstadoSolicitud == dsEstadoSolcitud && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
            foreach (var x in Lista)
            {
                //List<JUSTICA_SOLICITUD> ListadoJustificaciones = new List<JUSTICA_SOLICITUD>();
                //var detalle = entities.JUSTICA_SOLICITUD.Where(y => y.IdSolicitudPermiso == x.IdSolicitudPermiso).ToList();
                //foreach (var d in detalle)
                //{
                //    ListadoJustificaciones.Add(new JUSTICA_SOLICITUD
                //    {
                //        IdJustificaSolicitud = d.IdJustificaSolicitud,
                //        IdSolicitudPermiso = x.IdSolicitudPermiso,
                //        CodigoMotivo = d.CodigoMotivo,
                //        FechaSalida = d.FechaSalida,
                //        FechaRegreso = d.FechaRegreso,
                //        UsuarioIngresoLog = d.UsuarioIngresoLog,
                //        FechaIngresoLog = d.FechaIngresoLog,
                //        TerminalIngresoLog = d.TerminalIngresoLog,
                //        UsuarioModificacionLog = d.UsuarioModificacionLog,
                //        TerminalModificacionLog = d.TerminalModificacionLog,
                //        FechaModificacionLog = d.FechaModificacionLog
                //    });
                //}

                //var poMotivoPermiso = entities.spConsutaMotivosPermiso("0").FirstOrDefault(m => m.CodigoMotivo == x.CodigoMotivo);
                var poEmpleado = entities.spConsutaEmpleados(x.Identificacion).FirstOrDefault();
                string DescripcionEstadosSolicitud = (from e in entities.ESTADO_SOLICITUD
                                                      where e.Estado == x.EstadoSolicitud
                                                      select e.Descripcion).FirstOrDefault();
                ListaSolicitudesPermiso.Add(new SolicitudPermisoViewModel
                {
                    IdSolicitudPermiso = x.IdSolicitudPermiso,
                    //CodigoLinea = x.CodigoLinea,
                    DescripcionLinea = poEmpleado != null ? poEmpleado.LINEA : "",
                    //CodigoArea = x.CodigoArea,
                    DescripcionArea = poEmpleado != null ? poEmpleado.AREA : "",
                    //CodigoCargo = x.CodigoCargo,
                    //DescripcionCargo = poEmpleado != null ? poEmpleado.CARGO : "",
                    //Identificacion = x.Identificacion,
                    NombreEmpleado = poEmpleado != null ? poEmpleado.NOMBRES : "",
                    //CodigoMotivo = x.CodigoMotivo,
                    //DescripcionMotivo = poMotivoPermiso != null ? poMotivoPermiso.Descripcion : "",
                    //Observacion = x.Observacion,
                    //FechaSalida = x.FechaSalida,
                    //FechaRegreso = x.FechaRegreso,
                    EstadoSolicitud = x.EstadoSolicitud,
                    DescripcionEstadoSolicitud = DescripcionEstadosSolicitud,
                    FechaBiometrico = x.FechaBiometrico,
                    //Origen = x.Origen,
                    //CodigoDiagnostico = x.CodigoDiagnostico,
                    //FechaIngresoLog = x.FechaIngresoLog,
                    //UsuarioIngresoLog = x.UsuarioIngresoLog,
                    //TerminalIngresoLog = x.TerminalIngresoLog,
                    //UsuarioModificacionLog = x.UsuarioModificacionLog,
                    //FechaModificacionLog = x.FechaModificacionLog,
                    //TerminalModificacionLog = x.TerminalModificacionLog
                });
            }
            return ListaSolicitudesPermiso;
        }
        public List<SolicitudPermisoViewModel> ConsultaSolicitudesPermiso(string dsEstadoSolcitud, string dsIdUsuario)
        {
            entities = new ASIS_PRODEntities();

            List<SolicitudPermisoViewModel> ListaSolicitudesPermiso = new List<SolicitudPermisoViewModel>();
                                                                                               
            List<SOLICITUD_PERMISO> Lista = new List<SOLICITUD_PERMISO>();
            if (dsEstadoSolcitud == clsAtributos.EstadoSolicitudTodos)
            {
                Lista = entities.SOLICITUD_PERMISO.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
            else if(dsEstadoSolcitud==clsAtributos.EstadoSolicitudPendiente)
            {
                if (string.IsNullOrEmpty(dsIdUsuario))
                {
                    Lista = entities.SOLICITUD_PERMISO.Where(x =>
                    x.EstadoSolicitud == dsEstadoSolcitud &&
                    x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                }
                else { 
                    var nivelesUser = entities.NIVEL_USUARIO.FirstOrDefault(x => x.IdUsuario == dsIdUsuario);
                    if (nivelesUser != null)
                    {
                        var nivelesAprobacion = entities.NIVEL_APROBACION.Where(x => x.NivelBase == nivelesUser.Nivel).Select(x => x.NivelAprobar).ToList();
                        Lista = entities.SOLICITUD_PERMISO.Where(x => 
                        x.EstadoSolicitud == dsEstadoSolcitud && 
                        x.EstadoRegistro == clsAtributos.EstadoRegistroActivo &&
                        nivelesAprobacion.Contains(x.Nivel)).ToList();
                    }
                }
            }
            else
            {
                Lista = entities.SOLICITUD_PERMISO.Where(x => x.EstadoSolicitud == dsEstadoSolcitud && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }

            foreach (var x in Lista)
            {           
                var poMotivoPermiso = entities.spConsutaMotivosPermiso("0").FirstOrDefault(m => m.CodigoMotivo == x.CodigoMotivo);
                var poEmpleado = entities.spConsutaEmpleados(x.Identificacion).FirstOrDefault();
                ListaSolicitudesPermiso.Add(new SolicitudPermisoViewModel
                {
                    IdSolicitudPermiso = x.IdSolicitudPermiso,
                    CodigoLinea = x.CodigoLinea,
                    DescripcionLinea = poEmpleado != null ? poEmpleado.LINEA : "",
                    CodigoArea = x.CodigoArea,
                    DescripcionArea = poEmpleado != null ? poEmpleado.AREA : "",
                    CodigoCargo = x.CodigoCargo,
                    DescripcionCargo = poEmpleado != null ? poEmpleado.CARGO : "",
                    Identificacion = x.Identificacion,
                    NombreEmpleado = poEmpleado != null ? poEmpleado.NOMBRES : "",
                    CodigoMotivo = x.CodigoMotivo,
                    DescripcionMotivo = poMotivoPermiso != null ? poMotivoPermiso.Descripcion : "",
                    Observacion = x.Observacion,
                    FechaSalida = x.FechaSalida,
                    FechaRegreso = x.FechaRegreso,
                    EstadoSolicitud = x.EstadoSolicitud,
                    FechaBiometrico = x.FechaBiometrico,
                    Origen = x.Origen,
                    CodigoDiagnostico = x.CodigoDiagnostico,
                    FechaIngresoLog = x.FechaIngresoLog,
                    UsuarioIngresoLog = x.UsuarioIngresoLog,
                    TerminalIngresoLog = x.TerminalIngresoLog,
                    UsuarioModificacionLog = x.UsuarioModificacionLog,
                    FechaModificacionLog = x.FechaModificacionLog,
                    TerminalModificacionLog = x.TerminalModificacionLog
                });
            }
            
            return ListaSolicitudesPermiso;
        }
        public SolicitudPermisoViewModel ConsultaSolicitudPermiso(string dsSolicitud)
        {
            entities = new ASIS_PRODEntities();
            SolicitudPermisoViewModel ListaSolicitudesPermiso;
            int id = int.Parse(dsSolicitud);
            var lista = entities.SOLICITUD_PERMISO.FirstOrDefault(x => x.IdSolicitudPermiso == id && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);


            List<JUSTICA_SOLICITUD> ListadoJustificaciones =  new List<JUSTICA_SOLICITUD>();
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

            for (int i = detalle.Count; i < 5; i++)
                ListadoJustificaciones.Add(new JUSTICA_SOLICITUD());


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
                Origen = lista.Origen,
                DescripcionOrigen = lista.Origen == clsAtributos.SolicitudOrigenGeneral ? "General" : "Médico",
                CodigoDiagnostico = lista.CodigoDiagnostico,
                FechaIngresoLog = lista.FechaIngresoLog,
                UsuarioIngresoLog = lista.UsuarioIngresoLog,
                TerminalIngresoLog = lista.TerminalIngresoLog,
                UsuarioModificacionLog = lista.UsuarioModificacionLog,
                FechaModificacionLog = lista.FechaModificacionLog,
                TerminalModificacionLog = lista.TerminalModificacionLog,
                JustificaSolicitudes=ListadoJustificaciones
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
                piNivel = poNivelUsuario.Nivel??clsAtributos.NivelEmpleado;
            }
            return piNivel;
        }

        public List<BitacoraSolicitud> ConsultaBitacoraSolicitud(string dsIdSolicitud, string dsCedula, DateTime? ddFechaDesde, DateTime? ddFechaHasta)
        {
            IQueryable<BitacoraSolicitud> ListaBitacora = null;
            using(var context = new ASIS_PRODEntities())
            {

                var query = (from bitacora in context.BITACORA_SOLICITUD
                             join estado in context.ESTADO_SOLICITUD on bitacora.EstadoSolicitud equals estado.Estado
                             select new BitacoraSolicitud() {
                                 idBitacoraSolicitud=bitacora.IdBitacoraSolicitud,
                                 idSolicitud=bitacora.IdSolicitud,
                                 Cedula=bitacora.Cedula,
                                 CodEstadoSolicitud=bitacora.EstadoSolicitud,
                                 EstadoSolicitud=estado.Descripcion,
                                 FechaRegreso=bitacora.FechaSalida,
                                 FechaSalida=bitacora.FechaSalida,
                                 Observacion=bitacora.Observacion,
                                 FechaIngresoLog=bitacora.FechaIngresoLog,
                                 TerminalIngresoLog=bitacora.TerminalIngresoLog,
                                 UsuarioIngresoLog=bitacora.UsuarioIngresoLog
                             });
                if (!string.IsNullOrEmpty(dsIdSolicitud))
                {
                    int id = int.Parse(dsIdSolicitud);
                    ListaBitacora = query.Where(x => x.idSolicitud == id).OrderByDescending(x=>x.FechaIngresoLog);
                }
                else if (!string.IsNullOrEmpty(dsCedula))
                {
                    ListaBitacora = query.Where(x => x.Cedula == dsCedula).OrderByDescending(x => x.FechaIngresoLog);
                }
                if (ddFechaDesde != null && ddFechaHasta != null)
                {
                    ddFechaHasta = ddFechaHasta.Value.AddDays(1);
                    ddFechaHasta = ddFechaHasta.Value.Date;
                    ddFechaDesde = ddFechaDesde.Value.Date;
                    ListaBitacora = ListaBitacora.Where(x => x.FechaIngresoLog >= ddFechaDesde && x.FechaIngresoLog <= ddFechaHasta);
                }
                return ListaBitacora.ToList();
            }
        }
    }
} 