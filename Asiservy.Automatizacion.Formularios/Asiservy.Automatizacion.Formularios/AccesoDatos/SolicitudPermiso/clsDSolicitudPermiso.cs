using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
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
        clsDEmpleado clsDEmpleado = null;
        clsApiUsuario clsApiUsuario = null;
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
        public List<SolicitudPermisoViewModel> ConsultaSolicitudesPermisoReporte(string dsLinea, string dsArea, string dsEstado, bool dbGarita=false)
        {
            entities = new ASIS_PRODEntities();
            clsApiUsuario = new clsApiUsuario();
            List<SolicitudPermisoViewModel> ListaSolicitudesPermiso = new List<SolicitudPermisoViewModel>();
            IEnumerable<SOLICITUD_PERMISO> Lista;
            
            if (dsEstado == clsAtributos.EstadoSolicitudTodos)
            {
                Lista = entities.SOLICITUD_PERMISO.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
            }
            else
            {
                Lista = entities.SOLICITUD_PERMISO.Where(x => x.EstadoSolicitud == dsEstado && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
            }
            if (!string.IsNullOrEmpty(dsLinea))
            {
                Lista = Lista.Where(x => x.CodigoLinea == dsLinea);
            }
            if (!string.IsNullOrEmpty(dsArea))
            {
                Lista = Lista.Where(x => x.CodigoArea == dsArea);
            }
            if (dbGarita)
            {
                Lista = Lista.Where(x => x.FechaSalida.Date == DateTime.Now.Date);
                Lista = Lista.Where(x => x.FechaBiometrico==null);
            }
            foreach (var x in Lista.ToList())
            {
                var fechaBiometrico = clsApiUsuario.ConsultarFechaBiometrico(x.Identificacion);
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
                    FechaBiometrico = fechaBiometrico,
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

        public string MarcarHoraSalidaSolicitudPermiso(int IdSolicitudPermiso, DateTime? FechaBiometrico)
        {
            using(ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var Solicitud= entities.SOLICITUD_PERMISO.FirstOrDefault(x => x.IdSolicitudPermiso == IdSolicitudPermiso);
                if (Solicitud != null)
                {
                    Solicitud.FechaBiometrico = FechaBiometrico??DateTime.Now;
                }
                entities.SaveChanges();
                return clsAtributos.MsjRegistroGuardado;
            }
        }
     
        public List<SolicitudPermisoViewModel> ConsultaSolicitudesPermiso(string dsEstadoSolcitud, string dsIdUsuario)
        {
            entities = new ASIS_PRODEntities();
            clsApiUsuario = new clsApiUsuario();
            List<SolicitudPermisoViewModel> ListaSolicitudesPermiso = new List<SolicitudPermisoViewModel>();
                                                                                               
            List<SOLICITUD_PERMISO> ListaPreliminar = new List<SOLICITUD_PERMISO>();
            //Validacion de estados
            if (dsEstadoSolcitud == clsAtributos.EstadoSolicitudTodos)
            {
                ListaPreliminar = entities.SOLICITUD_PERMISO.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
            else if(dsEstadoSolcitud==clsAtributos.EstadoSolicitudPendiente)
            {
                //SI NO VIENE LA CEDULA ENVIAMOS TODOS
                if (string.IsNullOrEmpty(dsIdUsuario))
                {
                    ListaPreliminar = entities.SOLICITUD_PERMISO.Where(x =>
                    x.EstadoSolicitud == dsEstadoSolcitud &&
                    x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                }
                else {
                    //CONSULTAMOS LOS NIVELES DE USUARIOS ASIGNADOS PARA ESE USUARIO
                    var NivelUsuario = entities.NIVEL_USUARIO.FirstOrDefault(x => x.IdUsuario == dsIdUsuario);
                    clsDEmpleado = new clsDEmpleado();
                    //CONSULTAMOS LA LINEA A LA QUE PERTENECE EL USUARIO
                    var Linea = clsDEmpleado.ConsultaEmpleado(dsIdUsuario).FirstOrDefault();
                    if (Linea != null)
                    {
                        List<string> ListaLineas = new List<string>();
                        ListaLineas.Add(Linea.CODIGOLINEA);
                        //SI LA LINEA ES DE PRODUCCION VAMOS A CONSULTAR EL CLASIFICADOR DE TODAS LAS LINEAS QUE LE PERTENECEN A ESTE.
                        if (Linea.CODIGOLINEA == clsAtributos.CodLineaProduccion)
                        {
                            var LineasPertenece = (entities.CLASIFICADOR.Where(x=> 
                            x.Grupo == clsAtributos.CodGrupoLineaProduccion
                            && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                            )).ToList();
                            if(LineasPertenece != null)
                            {
                                foreach (var x in LineasPertenece)
                                    ListaLineas.Add(x.Codigo+"");
                            }
                        }
                        if(NivelUsuario != null) {
                            var nivelesAprobacion = entities.NIVEL_APROBACION.Where(x =>
                              x.NivelBase == NivelUsuario.Nivel).Select(x => x.NivelAprobar).ToList();

                            //VALIDAMOS QUE SEAN JEFES O EMPLEADOS
                            if (NivelUsuario.Nivel != clsAtributos.NivelGerencia && NivelUsuario.Nivel != clsAtributos.NivelJefatura)
                            {
                               
                                ListaPreliminar = entities.SOLICITUD_PERMISO.Where(x =>
                                x.EstadoSolicitud == dsEstadoSolcitud &&
                                x.EstadoRegistro == clsAtributos.EstadoRegistroActivo &&
                                nivelesAprobacion.Contains(x.Nivel)
                                && ListaLineas.Contains(x.CodigoLinea)).ToList();
                            }
                            //VALIDAMOS QUE SEA GERENCIA
                            else if (NivelUsuario.Nivel != clsAtributos.NivelJefatura)
                            {

                                ListaPreliminar = entities.SOLICITUD_PERMISO.Where(x =>
                                x.EstadoSolicitud == dsEstadoSolcitud &&
                                x.EstadoRegistro == clsAtributos.EstadoRegistroActivo &&
                                nivelesAprobacion.Contains(x.Nivel)).ToList();
                            }
                            //VALIDAMOS QUE SEA SUBGERENCIA
                            else
                            {        
                                //NIVEL DE JEFATURA- VALIDAMOS LOS QUE SON SOLO SUS EMPLEADOS 
                                foreach(var n in nivelesAprobacion)
                                {
                                    List<SOLICITUD_PERMISO> Sol = null;
                                    if (n.Value == clsAtributos.NivelEmpleado)
                                    {
                                        Sol = entities.SOLICITUD_PERMISO.Where(x =>
                                         x.EstadoSolicitud == dsEstadoSolcitud &&
                                         x.EstadoRegistro == clsAtributos.EstadoRegistroActivo &&
                                         //nivelesAprobacion.Contains(x.Nivel) &&
                                         ListaLineas.Contains(x.CodigoLinea) && x.Nivel == clsAtributos.NivelEmpleado
                                         ).ToList();
                                    }
                                    else
                                    {
                                        Sol = entities.SOLICITUD_PERMISO.Where(x =>
                                         x.EstadoSolicitud == dsEstadoSolcitud &&
                                         x.EstadoRegistro == clsAtributos.EstadoRegistroActivo &&
                                         //nivelesAprobacion.Contains(x.Nivel) &&
                                         x.Nivel == n.Value
                                         ).ToList();
                                    }
                                    if(Sol!=null)
                                        ListaPreliminar.AddRange(Sol);
                                }
                                
                            }
                        }
                    }
                }
            }
            else
            {
                ListaPreliminar = entities.SOLICITUD_PERMISO.Where(x => x.EstadoSolicitud == dsEstadoSolcitud && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }

            foreach (var x in ListaPreliminar)
            {           
                var poMotivoPermiso = entities.spConsutaMotivosPermiso("0").FirstOrDefault(m => m.CodigoMotivo == x.CodigoMotivo);
                var poEmpleado = entities.spConsutaEmpleados(x.Identificacion).FirstOrDefault();
                var fechaBiometrico = clsApiUsuario.ConsultarFechaBiometrico(x.Identificacion);
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
                    FechaBiometrico = fechaBiometrico,
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
            int piNivel = clsAtributos.NivelEmpleado;
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