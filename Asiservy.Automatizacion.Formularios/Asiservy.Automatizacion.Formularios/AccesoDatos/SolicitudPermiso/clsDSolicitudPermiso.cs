using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Xml;
using System.Xml.Serialization;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDSolicitudPermiso
    {
        ASIS_PRODEntities entities = null;
        clsDEmpleado clsDEmpleado = null;
        clsApiUsuario clsApiUsuario = null;
        clsDGeneral clsDGeneral = null;
        public string CambioEstadoSolicitud(SOLICITUD_PERMISO doSolicitud)
        {
            clsDGeneral = new clsDGeneral();
            clsDEmpleado = new clsDEmpleado();
            string psMensaje = "No se pudo cambiar de estado a la solicitud";
            string mensajeCorreo = string.Empty;
            entities = new ASIS_PRODEntities();
            var poSolicitud = entities.SOLICITUD_PERMISO.FirstOrDefault(x => x.IdSolicitudPermiso == doSolicitud.IdSolicitudPermiso);
            if (poSolicitud != null)
            {
                poSolicitud.Observacion += doSolicitud.Observacion ?? "";
                poSolicitud.EstadoSolicitud = doSolicitud.EstadoSolicitud ?? poSolicitud.EstadoSolicitud;
                poSolicitud.FechaModificacionLog = doSolicitud.FechaModificacionLog;
                poSolicitud.TerminalModificacionLog = doSolicitud.TerminalModificacionLog;
                poSolicitud.UsuarioModificacionLog = doSolicitud.UsuarioModificacionLog;
                poSolicitud.ValidaMedico = doSolicitud.ValidaMedico??poSolicitud.ValidaMedico;
                if (doSolicitud.EnvioOnlyControl == true){
                    poSolicitud.EnvioOnlyControl = doSolicitud.EnvioOnlyControl;
                }
                psMensaje = "Registro Actualizado Correctamente";

                BITACORA_SOLICITUD poBitacora = new BITACORA_SOLICITUD();
                poBitacora.IdSolicitud = poSolicitud.IdSolicitudPermiso;
                poBitacora.Cedula = poSolicitud.Identificacion;
                poBitacora.Observacion = poSolicitud.Observacion;
                poBitacora.EstadoSolicitud = poSolicitud.EstadoSolicitud;
                poBitacora.FechaIngresoLog = DateTime.Now;
                poBitacora.CambioEstado = true;
                poBitacora.UsuarioIngresoLog = doSolicitud.UsuarioModificacionLog;
                poBitacora.TerminalIngresoLog = doSolicitud.TerminalModificacionLog;
                entities.BITACORA_SOLICITUD.Add(poBitacora);  
            entities.SaveChanges();

                bool RRHH = false;
                string EstadoSolictud = string.Empty;
                if (poSolicitud.EstadoSolicitud == clsAtributos.EstadoSolicitudAprobado)
                {
                    EstadoSolictud = "Aprobado";
                    RRHH = true;
                }
                else if (poSolicitud.EstadoSolicitud == clsAtributos.EstadoSolicitudAnulado)
                {
                    EstadoSolictud = "Anulado";
                }
                else if (poSolicitud.EstadoSolicitud == clsAtributos.EstadoSolicitudRevisado)
                {
                    EstadoSolictud = "Revisado";
                }
                var poEmpleado = clsDEmpleado.ConsultaEmpleado(poSolicitud.Identificacion).FirstOrDefault();      
                var Motivo = ConsultarMotivos(poSolicitud.CodigoMotivo).FirstOrDefault();
                String MensajeBody = "Empleado: " + poEmpleado.NOMBRES + "\n</br>"
                      + " Motivo:" + Motivo.DescripcionMotivo + "\n</br>"
                       + "Observación:" + poSolicitud.Observacion + "\n</br>"
                      + "Fecha Salida: " + poSolicitud.FechaSalida + "\n</br>"
                      + "Fecha Regreso: " + poSolicitud.FechaRegreso + "</br>\n Estado: " + EstadoSolictud + " </br>"
                      + "Realizado por: " + doSolicitud.UsuarioModificacionLog + "</br>";
                if(poSolicitud.EstadoSolicitud == clsAtributos.EstadoSolicitudAnulado)
                {
                    MensajeBody = MensajeBody + "Motivo: " + poSolicitud.Observacion+ "</br></br>";
                }
                else
                {
                    MensajeBody = MensajeBody + "</br>";
                }
               
                mensajeCorreo = clsDGeneral.EnvioCorreo(poEmpleado, "Solicitud Permiso",
                MensajeBody, RRHH);
            }

            return psMensaje+"--"+ mensajeCorreo;
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
                        poSolicitud.CodigoClasificador = doSolicitud.CodigoClasificador;
                        poSolicitud.CodigoDiagnostico = doSolicitud.CodigoDiagnostico;
                        poSolicitud.NombreMedico = doSolicitud.NombreMedico;
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

                    poBitacora.CambioEstado = false;
                    poBitacora.CodigoMotivo = doSolicitud.CodigoMotivo;
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

        public List<Motivo> ConsultarMotivos(string Codmotivo)
        {
            entities = new ASIS_PRODEntities();

            //var lista= entities.spConsutaMotivosPermiso(tipo).ToList();
            var client = new RestClient("http://192.168.0.31:8870");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            RestRequest request = null;
            if (!string.IsNullOrEmpty(Codmotivo))
                 request = new RestRequest("/api/Motivos/"+ Codmotivo, Method.GET);
            else
                 request = new RestRequest("/api/Motivos", Method.GET);

            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string
            List<Motivo> ListaMotivo = JsonConvert.DeserializeObject<List<Motivo>>(content);


            return ListaMotivo;
        }

        public List<SolicitudPermisoViewModel> ConsultaSolicitudesPermisoReporte(string dsLinea, string dsArea, string dsEstado, bool dbGarita=false, DateTime? FechaDesde = null, DateTime? FechaHasta = null)
        {
            entities = new ASIS_PRODEntities();
            clsApiUsuario = new clsApiUsuario();
            List<SolicitudPermisoViewModel> ListaSolicitudesPermiso = new List<SolicitudPermisoViewModel>();
           // IEnumerable<SOLICITUD_PERMISO> Lista;

            var ListadoSolicitudes = entities.spConsultaSolcitudesPermisos(dsLinea, dsArea, dsEstado, dbGarita, FechaDesde, FechaHasta).ToList();
            var motivos = ConsultarMotivos(null).ToList();
            foreach (var x in ListadoSolicitudes)
            {
                var DescripcionMotivo = motivos.FirstOrDefault(y => y.CodigoMotivo == x.CodigoMotivo);
                ListaSolicitudesPermiso.Add(new SolicitudPermisoViewModel
                {
                IdSolicitudPermiso = x.IdSolicitudPermiso,
                //CodigoLinea = x.CodigoLinea,
                DescripcionLinea = x.Linea,
                //CodigoArea = x.CodigoArea,
                DescripcionArea = x.Area,
                //CodigoCargo = x.CodigoCargo,
                //DescripcionCargo = poEmpleado != null ? poEmpleado.CARGO : "",
                //Identificacion = x.Identificacion,
                NombreEmpleado = x.Nombre,
                //CodigoMotivo = x.CodigoMotivo,
                DescripcionMotivo = DescripcionMotivo!=null ? DescripcionMotivo.DescripcionMotivo : "",
                Observacion = x.Observacion,
                FechaSalida = x.FechaSalida,
                FechaRegreso = x.FechaRegreso,
                EstadoSolicitud = x.CodEstadoSolicitud,
                DescripcionEstadoSolicitud = x.EstadoSolcitud,
                FechaBiometrico = x.FechaBiometrico,
                //Origen = x.Origen,
                //CodigoDiagnostico = x.CodigoDiagnostico,
                FechaIngresoLog = x.FechaIngresoLog,
                UsuarioIngresoLog = x.UsuarioIngresoLog,
                TerminalIngresoLog = x.TerminalIngresoLog,
                UsuarioModificacionLog = x.UsuarioModificacionLog,
                FechaModificacionLog = x.FechaModificacionLog,
                TerminalModificacionLog = x.TerminalModificacionLog
            });
        }
            //if (dsEstado == clsAtributos.EstadoSolicitudTodos)
            //{
            //    Lista = entities.SOLICITUD_PERMISO.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
            //}
            //else
            //{
            //    if(dbGarita)
            //    {
            //        //2020-01-08 -> se agrega el estado de solicitud revisado para que garita pueda visualizar las solicitude que RRHH ya las pone en ese estado.
            //        Lista = entities.SOLICITUD_PERMISO.Where(x => (x.EstadoSolicitud == dsEstado || x.EstadoSolicitud ==clsAtributos.EstadoSolicitudRevisado) && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);

            //    }
            //    else
            //    {
            //        Lista = entities.SOLICITUD_PERMISO.Where(x => x.EstadoSolicitud == dsEstado && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);

            //    }
            //}
            //if (!string.IsNullOrEmpty(dsLinea))
            //{
            //    Lista = Lista.Where(x => x.CodigoLinea == dsLinea);
            //}
            //if (!string.IsNullOrEmpty(dsArea))
            //{
            //    Lista = Lista.Where(x => x.CodigoArea == dsArea);
            //}
            //if (dbGarita)
            //{
            //    Lista = Lista.Where(x => x.FechaSalida.Date == DateTime.Now.Date);
            //    Lista = Lista.Where(x => x.FechaBiometrico==null);
            //}
            //if (FechaDesde != null)
            //{
            //    var FechaFin = FechaHasta != null ? FechaHasta.Value.AddDays(1) : DateTime.Now.AddDays(1);
            //    Lista = Lista.Where(x => x.FechaIngresoLog.Date >= FechaDesde && x.FechaIngresoLog.Date<= FechaFin);

            //}

            //foreach (var x in Lista.ToList())
            //{
            //    var fechaBiometrico = entities.spConsultaUltimaMarcacionBiometrico(x.Identificacion).FirstOrDefault();
            //    var poEmpleado = entities.spConsutaEmpleados(x.Identificacion).FirstOrDefault();
            //    var poMotivos = ConsultarMotivos(x.CodigoMotivo).FirstOrDefault();
            //    string DescripcionEstadosSolicitud = (from e in entities.ESTADO_SOLICITUD
            //                                          where e.Estado == x.EstadoSolicitud
            //                                          select e.Descripcion).FirstOrDefault();
            //    ListaSolicitudesPermiso.Add(new SolicitudPermisoViewModel
            //    {
            //        IdSolicitudPermiso = x.IdSolicitudPermiso,
            //        //CodigoLinea = x.CodigoLinea,
            //        DescripcionLinea = poEmpleado != null ? poEmpleado.LINEA : "",
            //        //CodigoArea = x.CodigoArea,
            //        DescripcionArea = poEmpleado != null ? poEmpleado.AREA : "",
            //        //CodigoCargo = x.CodigoCargo,
            //        //DescripcionCargo = poEmpleado != null ? poEmpleado.CARGO : "",
            //        //Identificacion = x.Identificacion,
            //        NombreEmpleado = poEmpleado != null ? poEmpleado.NOMBRES : "",
            //        //CodigoMotivo = x.CodigoMotivo,
            //       DescripcionMotivo = poMotivos != null ? poMotivos.DescripcionMotivo : "",
            //        Observacion = x.Observacion,
            //        FechaSalida = x.FechaSalida,
            //        FechaRegreso = x.FechaRegreso,
            //        EstadoSolicitud = x.EstadoSolicitud,
            //        DescripcionEstadoSolicitud = DescripcionEstadosSolicitud,
            //        FechaBiometrico = dbGarita?fechaBiometrico.Marcacion:x.FechaBiometrico,
            //        //Origen = x.Origen,
            //        //CodigoDiagnostico = x.CodigoDiagnostico,
            //        FechaIngresoLog = x.FechaIngresoLog,
            //        UsuarioIngresoLog = x.UsuarioIngresoLog,
            //        TerminalIngresoLog = x.TerminalIngresoLog,
            //        UsuarioModificacionLog = x.UsuarioModificacionLog,
            //        FechaModificacionLog = x.FechaModificacionLog,
            //        TerminalModificacionLog = x.TerminalModificacionLog
            //    });
            //}
            return ListaSolicitudesPermiso;
        }

        public string MarcarHoraSalidaSolicitudPermiso(SOLICITUD_PERMISO model)
        {
            using(ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var Solicitud= entities.SOLICITUD_PERMISO.FirstOrDefault(x => x.IdSolicitudPermiso == model.IdSolicitudPermiso);
                if (Solicitud != null)
                {
                    Solicitud.FechaBiometrico = model.FechaBiometrico ?? DateTime.Now;
                    Solicitud.FechaModificacionLog = DateTime.Now;
                    Solicitud.TerminalModificacionLog = model.TerminalIngresoLog;
                    Solicitud.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    BITACORA_SOLICITUD bitacora = new BITACORA_SOLICITUD {
                        IdSolicitud = Solicitud.IdSolicitudPermiso,
                        EstadoSolicitud =Solicitud.EstadoSolicitud,
                        Observacion = "Marcación de salida",
                        FechaIngresoLog = DateTime.Now,
                        FechaSalida= Solicitud.FechaSalida,
                        CambioEstado= false,
                        CodigoMotivo = Solicitud.CodigoMotivo,
                        FechaRegreso = Solicitud.FechaRegreso,
                        TerminalIngresoLog = model.TerminalIngresoLog,
                        UsuarioIngresoLog =model.UsuarioIngresoLog,
                        Cedula= Solicitud.Identificacion

                    };
                    entities.BITACORA_SOLICITUD.Add(bitacora);

                    
                    if (Solicitud.Origen == "M")
                    {
                        //INGRESAR MARCACIÓN DE SALIDA DEL BIOMÉTRICO
                        StatusOnlyControl resultOnlyControl;
                        using (OnlyControlService.wsrvTcontrolSoapClient service = new OnlyControlService.wsrvTcontrolSoapClient())
                        {
                            string content = service.InsertaMarcacion(clsAtributos.keyLlaveAcceso, Solicitud.Identificacion, DateTime.Now, "SALIDA", "192.168.31.2");
                            resultOnlyControl = JsonConvert.DeserializeObject<StatusOnlyControl>(content);
                        }

                    }


                }
                entities.SaveChanges();
                return clsAtributos.MsjRegistroGuardado;
            }
        }
     

        public List<SolicitudPermisoViewModel> ConsultaSolicitudesPermisosRRHH()
        {
            entities = new ASIS_PRODEntities();
            List<SolicitudPermisoViewModel> ListaSolicitudesPermiso = new List<SolicitudPermisoViewModel>();
            // IEnumerable<SOLICITUD_PERMISO> Lista;

            var ListadoSolicitudes = entities.spConsultaSolcitudesPermisos(null, null, clsAtributos.EstadoSolicitudAprobado, false, null, null).ToList();
            var motivos = ConsultarMotivos(null).ToList();
            foreach (var x in ListadoSolicitudes)
            {
                var DescripcionMotivo = motivos.FirstOrDefault(y => y.CodigoMotivo == x.CodigoMotivo);
                ListaSolicitudesPermiso.Add(new SolicitudPermisoViewModel
                {
                    IdSolicitudPermiso = x.IdSolicitudPermiso,
                    //CodigoLinea = x.CodigoLinea,
                    DescripcionLinea = x.Linea,
                    //CodigoArea = x.CodigoArea,
                    DescripcionArea = x.Area,
                    //CodigoCargo = x.CodigoCargo,
                    //DescripcionCargo = poEmpleado != null ? poEmpleado.CARGO : "",
                    //Identificacion = x.Identificacion,
                    NombreEmpleado = x.Nombre,
                    //CodigoMotivo = x.CodigoMotivo,
                    DescripcionMotivo = DescripcionMotivo != null ? DescripcionMotivo.DescripcionMotivo : "",
                    Observacion = x.Observacion,
                    FechaSalida = x.FechaSalida,
                    FechaRegreso = x.FechaRegreso,
                    EstadoSolicitud = x.CodEstadoSolicitud,
                    DescripcionEstadoSolicitud = x.EstadoSolcitud,
                    FechaBiometrico = x.FechaBiometrico,
                    Origen = x.Origen,
                    //CodigoDiagnostico = x.CodigoDiagnostico,
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

        public List<SolicitudPermisoViewModel> ConsultaSolicitudesPendiente(string dsIdUsuario)
        {
            entities = new ASIS_PRODEntities();
            clsApiUsuario = new clsApiUsuario();
            List<SolicitudPermisoViewModel> ListaSolicitudesPermiso = new List<SolicitudPermisoViewModel>();
            var ListaPreliminar = entities.spConsultaSolicitudesPendientesAprobar(dsIdUsuario).ToList();
            var ListaMotivoPermiso = this.ConsultarMotivos(null).ToList();
            foreach (var x in ListaPreliminar)
            {
                var poMotivoPermiso = ListaMotivoPermiso.FirstOrDefault(y => y.CodigoMotivo == x.CodigoMotivo);
                  ListaSolicitudesPermiso.Add(new SolicitudPermisoViewModel
                {

                    IdSolicitudPermiso = x.IdSolicitudPermiso??0,
                    CodigoLinea = x.CodigoLinea,
                    DescripcionLinea = x.Linea,
                    CodigoArea = x.CodigoArea,
                    DescripcionArea = x.Area,
                    CodigoCargo = x.CodigoCargo,
                  //  DescripcionCargo = x.ca,
                    Identificacion = x.Identificacion,
                    NombreEmpleado = x.Nombre,
                    CodigoMotivo = x.CodigoMotivo,
                    DescripcionMotivo = poMotivoPermiso != null ? poMotivoPermiso.DescripcionMotivo : "",
                    Observacion = x.Observacion,
                    FechaSalida = x.FechaSalida,
                    FechaRegreso = x.FechaRegreso,
                    EstadoSolicitud = x.EstadoSolcitud,
                    FechaBiometrico = x.FechaBiometrico,
                    //Origen = x.,
                    //CodigoDiagnostico = x.c,
                   // ValidaMedico = x.ValidaMedico,
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
                            x.Grupo == clsAtributos.CodGrupoLineasAprobarSolicitudProduccion
                            && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                            && x.Codigo!="0"
                            )).ToList();
                            if(LineasPertenece != null)
                            {
                                foreach (var x in LineasPertenece)
                                    ListaLineas.Add(x.Codigo+"");
                                //ListaLineas.Add(clsAtributos.CodLineaProduccionEmpaque);
                                //ListaLineas.Add(clsAtributos.CodLineaProduccionRecuperadoControl);
                            }
                        }
                        if(NivelUsuario != null) {
                            var nivelesAprobacion = entities.NIVEL_APROBACION.Where(x =>
                              x.NivelBase == NivelUsuario.Nivel).Select(x => x.NivelAprobar).ToList();

                            //VALIDAMOS QUE SEAN JEFES O EMPLEADOS
                            if (NivelUsuario.Nivel != clsAtributos.NivelGerencia && NivelUsuario.Nivel != clsAtributos.NivelSubGerencia)
                            {
                               
                                ListaPreliminar = entities.SOLICITUD_PERMISO.Where(x =>
                                x.EstadoSolicitud == dsEstadoSolcitud &&
                                x.EstadoRegistro == clsAtributos.EstadoRegistroActivo &&
                                nivelesAprobacion.Contains(x.Nivel)
                                && ListaLineas.Contains(x.CodigoLinea)).ToList();
                            }
                            //VALIDAMOS QUE SEA GERENCIA
                            else if (NivelUsuario.Nivel != clsAtributos.NivelSubGerencia)
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

            var ListaMotivoPermiso = this.ConsultarMotivos(null).ToList();


            foreach (var x in ListaPreliminar)
            {
                var poMotivoPermiso = ListaMotivoPermiso.FirstOrDefault(y => y.CodigoMotivo == x.CodigoMotivo);
                var poEmpleado = entities.spConsutaEmpleados(x.Identificacion).FirstOrDefault();
                var Biometrico = entities.spConsultaUltimaMarcacionBiometrico(x.Identificacion).FirstOrDefault();
              //  var fechaBiometrico = entities.spConsultaUltimaMarcacionBiometrico(x.Identificacion).FirstOrDefault();
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
                    DescripcionMotivo = poMotivoPermiso != null ? poMotivoPermiso.DescripcionMotivo : "",
                    Observacion = x.Observacion,
                    FechaSalida = x.FechaSalida,
                    FechaRegreso = x.FechaRegreso,
                    EstadoSolicitud = x.EstadoSolicitud,
                    FechaBiometrico = Biometrico!= null ?Biometrico.Marcacion:null,
                    Origen = x.Origen,
                    CodigoDiagnostico = x.CodigoDiagnostico,
                    ValidaMedico = x.ValidaMedico,
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

        public int ConsultaSolicitudesPermisoNotificaciones(string dsEstadoSolcitud, string dsIdUsuario)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                clsApiUsuario = new clsApiUsuario();
                List<SolicitudPermisoViewModel> ListaSolicitudesPermiso = new List<SolicitudPermisoViewModel>();
                var ListaPreliminar = entities.spConsultaSolicitudesPendientesAprobar(dsIdUsuario).ToList();
                return ListaPreliminar.Count;
            }
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
                    EnvioOnlyControl = d.EnvioOnlyControl,
                    UsuarioIngresoLog = d.UsuarioIngresoLog,
                    FechaIngresoLog = d.FechaIngresoLog,
                    TerminalIngresoLog = d.TerminalIngresoLog,
                    UsuarioModificacionLog = d.UsuarioModificacionLog,
                    TerminalModificacionLog = d.TerminalModificacionLog,
                    FechaModificacionLog = d.FechaModificacionLog
                });
            }

            for (int i = detalle.Count; i < 5; i++)
                ListadoJustificaciones.Add(new JUSTICA_SOLICITUD { CodigoMotivo="0"});


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
                CodigoClasificador =lista.CodigoClasificador+"",
                 NombreMedico=lista.NombreMedico,
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

        public string ConsultaMotivoPermisoxEmpleado(string cedula)
        {
            string poMotivoPermiso=string.Empty;
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                string CodMotivo = db.sp_ConsultaMotivoSolicitudPermisoAsistencia(cedula).FirstOrDefault();
                if (!string.IsNullOrEmpty(CodMotivo))
                {
                     poMotivoPermiso = ConsultarMotivos(CodMotivo).FirstOrDefault().DescripcionMotivo;
                }
               
                return poMotivoPermiso;
            }
        }

        public List<SolicitudPermisoViewModel> ConsultaSolicitudesPermiso(SOLICITUD_PERMISO filtros)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<SolicitudPermisoViewModel> ListaSolicitudesPermiso= new List<SolicitudPermisoViewModel>();
                IEnumerable<SOLICITUD_PERMISO> ListaSolicitudes = entities.SOLICITUD_PERMISO;                

                if(filtros !=null)
                {
                    if (!string.IsNullOrEmpty(filtros.Identificacion))
                    {
                        ListaSolicitudes = ListaSolicitudes.Where(x => x.Identificacion == filtros.Identificacion);
                    }
                    if (!string.IsNullOrEmpty(filtros.EstadoRegistro))
                    {
                        ListaSolicitudes = ListaSolicitudes.Where(x => x.EstadoRegistro == filtros.EstadoRegistro);
                    }
                    if (!string.IsNullOrEmpty(filtros.EstadoSolicitud))
                    {
                        ListaSolicitudes = ListaSolicitudes.Where(x => x.EstadoSolicitud == filtros.EstadoSolicitud);
                    }
                    if (!string.IsNullOrEmpty(filtros.Origen))
                    {
                        ListaSolicitudes = ListaSolicitudes.Where(x => x.Origen == filtros.Origen);
                    }
                    if (filtros.ValidaMedico!=null && filtros.ValidaMedico==true)
                    {
                        ListaSolicitudes = ListaSolicitudes.Where(x => x.ValidaMedico == filtros.ValidaMedico);
                    }
                    //ListaSolicitudes = entities.SOLICITUD_PERMISO.Where(x => x.FechaSalida >=  filtros.FechaSalida);                  
                    //ListaSolicitudes = entities.SOLICITUD_PERMISO.Where(x => x.FechaSalida <= filtros.FechaRegreso);                   
                }
                var ListaRecorrer = ListaSolicitudes.ToList();
                foreach (var lista in ListaRecorrer)
                {
                    //var poMotivoPermiso = entities.spConsutaMotivosPermiso("0").FirstOrDefault(m => m.CodigoMotivo == lista.CodigoMotivo);
                    var poMotivoPermiso = ConsultarMotivos(lista.CodigoMotivo).FirstOrDefault();
                    //ConsultarMotivos(codmot)
                    var poEmpleado = entities.spConsutaEmpleados(lista.Identificacion).FirstOrDefault();
                    ListaSolicitudesPermiso.Add( new SolicitudPermisoViewModel
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
                        DescripcionMotivo = poMotivoPermiso != null ? poMotivoPermiso.DescripcionMotivo : "",
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
                        TerminalModificacionLog = lista.TerminalModificacionLog
                       
                    });
                }
                return ListaSolicitudesPermiso;
            }
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
            List<BitacoraSolicitud> ListaBitacoraFinal = null;
            clsDEmpleado = new clsDEmpleado();
            clsDGeneral = new clsDGeneral();
            //var motivos = clsDGeneral.
           

            using (var context = new ASIS_PRODEntities())
            {

                var query = (from bitacora in context.BITACORA_SOLICITUD
                             join estado in context.ESTADO_SOLICITUD on bitacora.EstadoSolicitud equals estado.Estado
                             select new BitacoraSolicitud() {
                                 idBitacoraSolicitud=bitacora.IdBitacoraSolicitud,
                                 idSolicitud=bitacora.IdSolicitud,
                                 Cedula=bitacora.Cedula,
                                 CodMotivo=bitacora.CodigoMotivo,
                                 CodEstadoSolicitud=bitacora.EstadoSolicitud,
                                 EstadoSolicitud=estado.Descripcion,
                                 FechaRegreso=bitacora.FechaRegreso,
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
                ListaBitacoraFinal = ListaBitacora.ToList();
                foreach (var x in ListaBitacoraFinal)
                {
                    var empleado = clsDEmpleado.ConsultaEmpleado(x.Cedula).FirstOrDefault();
                    var linea = clsDGeneral.ConsultaLineas(empleado.CODIGOLINEA).FirstOrDefault();
                    x.Nombres = empleado.NOMBRES;
                    x.Linea = linea.Descripcion;                    
                }


                return ListaBitacoraFinal.ToList();
            }
        }

        public List<RespuestaGeneral> EnviarSolicitudOnlyControl(SOLICITUD_PERMISO doSolicitud)
        {
            List<RespuestaGeneral> Respuestas = new List<RespuestaGeneral>();
            entities = new ASIS_PRODEntities();
            var poSolicitud = entities.SOLICITUD_PERMISO.FirstOrDefault(x => x.IdSolicitudPermiso == doSolicitud.IdSolicitudPermiso);
            var OnlyControl = entities.spConsultaCodigoOnlyControl(poSolicitud.Identificacion).FirstOrDefault();
            List<SOLICITUD_PERMISO> ListadoSolicitudes = new List<SOLICITUD_PERMISO>();
            //ListadoSolicitudes.Add(new SOLICITUD_PERMISO {
            //    FechaRegreso = poSolicitud.FechaRegreso,
            //    FechaSalida = poSolicitud.FechaSalida,
            //    CodigoMotivo = poSolicitud.CodigoMotivo,
            //    IdSolicitudPermiso = poSolicitud.IdSolicitudPermiso,
            //    Observacion = poSolicitud.Observacion
            //});
            foreach(var x in poSolicitud.JUSTICA_SOLICITUD)
            {
                if (x.FechaSalida != null && x.FechaRegreso != null && x.EnvioOnlyControl != true)
                {
                    ListadoSolicitudes.Add(new SOLICITUD_PERMISO
                    {
                        FechaRegreso = x.FechaRegreso??poSolicitud.FechaRegreso,
                        FechaSalida = x.FechaSalida??poSolicitud.FechaSalida,
                        CodigoMotivo = x.CodigoMotivo,
                        IdSolicitudPermiso = x.IdJustificaSolicitud,
                        Observacion = poSolicitud.Observacion
                    });
                }
            }

            StatusOnlyControl resultOnlyControl;
            foreach (var x in ListadoSolicitudes)
            {
                bool envioTodoDia = false;
                TimeSpan Tiempo = x.FechaRegreso - x.FechaSalida;
                if(x.FechaSalida==x.FechaRegreso || Tiempo.Days >= 1)
                {
                    envioTodoDia = true;
                }
                //if () { }
                using (OnlyControlService.wsrvTcontrolSoapClient service = new OnlyControlService.wsrvTcontrolSoapClient())
                {
                    string content = service.insertarPermiso(x.FechaSalida.Date, x.FechaRegreso.Date, OnlyControl.Codigo, x.CodigoMotivo, envioTodoDia, x.FechaSalida, x.FechaRegreso, 1, x.Observacion, clsAtributos.keyLlaveAcceso);
                    //var prueba = content.codigo;
                    resultOnlyControl = JsonConvert.DeserializeObject<StatusOnlyControl>(content);
                }
                if (resultOnlyControl.codigo == "0")
                {
                    var JustificaSolicitud = entities.JUSTICA_SOLICITUD.FirstOrDefault(y=> y.IdJustificaSolicitud==x.IdSolicitudPermiso);
                    JustificaSolicitud.EnvioOnlyControl = true;
                    entities.SaveChanges();
                    Respuestas.Add(new RespuestaGeneral { Mensaje = "Secundaria: ("+x.IdSolicitudPermiso+")->"+resultOnlyControl.mensaje, Respuesta = true, Observacion = "" });
                }
                else
                {
                    Respuestas.Add(new RespuestaGeneral { Mensaje = "Secundaria: (" + x.IdSolicitudPermiso + ")->" + resultOnlyControl.mensaje, Respuesta = false, Observacion = "" });
                }
            }

            if(!Respuestas.Any(x=> x.Respuesta==false))
            {
                bool envioTodoDia = false;
                TimeSpan Tiempo = poSolicitud.FechaRegreso - poSolicitud.FechaSalida;
                if (poSolicitud.FechaSalida == poSolicitud.FechaRegreso || Tiempo.Days >= 1)
                {
                    envioTodoDia = true;
                }
                using (OnlyControlService.wsrvTcontrolSoapClient service = new OnlyControlService.wsrvTcontrolSoapClient())
                {
                    string content = service.insertarPermiso(poSolicitud.FechaSalida.Date, poSolicitud.FechaRegreso.Date, OnlyControl.Codigo, poSolicitud.CodigoMotivo, envioTodoDia, poSolicitud.FechaSalida, poSolicitud.FechaRegreso, 1, poSolicitud.Observacion, clsAtributos.keyLlaveAcceso);
                    //var prueba = content.codigo;
                    resultOnlyControl = JsonConvert.DeserializeObject<StatusOnlyControl>(content);
                }
                if (resultOnlyControl.codigo == "0")
                {
                    doSolicitud.EnvioOnlyControl = true;
                    CambioEstadoSolicitud(doSolicitud);
                  //  entities.SaveChanges();
                    Respuestas.Add(new RespuestaGeneral { Mensaje = "Principal: (" + poSolicitud.IdSolicitudPermiso + ")-> " + resultOnlyControl.mensaje, Respuesta = true, Observacion = "" });
                }
                else
                {
                    Respuestas.Add(new RespuestaGeneral { Mensaje = "Principal: (" + poSolicitud.IdSolicitudPermiso + ")-> " + resultOnlyControl.mensaje, Respuesta = false, Observacion = "" });
                } 
            }
            else{
                Respuestas.Add(new RespuestaGeneral { Mensaje = "Principal: (" + poSolicitud.IdSolicitudPermiso + ")-> " + "Algunas solicitudes secundarias no se enviaron", Respuesta = false, Observacion = "" });
            }


            return Respuestas;

        }

        public List<SP_PKI_SOLICITUDES> ConsultaPKI_General()
        {
            using(ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
               return  db.SP_PKI_SOLICITUDES().ToList();
            }
        }

        public int GenerarSolicitudPermisoMasivo(SOLICITUD_PERMISO model, List<string> Cedulas)
        {
            int Cantidad = 0;
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    string Cedula = model.Identificacion;
                    int Nivel = model.Nivel??clsAtributos.NivelEmpleado;
                    clsDEmpleado = new clsDEmpleado();                  
                    
                    foreach(var x in Cedulas)
                    {
                        var poEmpleados = clsDEmpleado.ConsultaEmpleado(x).FirstOrDefault();
                        SOLICITUD_PERMISO sol = model;
                        sol.Nivel = ConsultarNivelUsuario(x);
                        sol.CodigoArea = poEmpleados.CODIGOAREA;
                        sol.CodigoRecurso = poEmpleados.CODIGORECURSO;
                        sol.CodigoCargo = poEmpleados.CODIGOCARGO;
                        sol.Identificacion = x;
                        if (sol.Nivel == clsAtributos.NivelEmpleado)
                        {
                            sol.EstadoSolicitud = clsAtributos.EstadoSolicitudAprobado;                            
                        }
                        else
                        {
                            sol.EstadoSolicitud = clsAtributos.EstadoSolicitudPendiente;                           
                        }
                        entities.SOLICITUD_PERMISO.Add(sol);

                        BITACORA_SOLICITUD poBitacora = new BITACORA_SOLICITUD();
                        poBitacora.IdSolicitud = sol.IdSolicitudPermiso;
                        poBitacora.Cedula = sol.Identificacion;
                        poBitacora.CodigoMotivo = sol.CodigoMotivo;
                        poBitacora.CambioEstado = true;
                        poBitacora.Observacion = sol.Observacion;
                        poBitacora.EstadoSolicitud = sol.EstadoSolicitud;
                        poBitacora.FechaIngresoLog = DateTime.Now;
                        poBitacora.UsuarioIngresoLog = sol.UsuarioIngresoLog;
                        poBitacora.TerminalIngresoLog = sol.TerminalIngresoLog;
                        entities.BITACORA_SOLICITUD.Add(poBitacora);
                        entities.SaveChanges();
                        Cantidad++;
                    }
                    
                    transaction.Commit();
                    return Cantidad;
                }
            }
        }

        public List<SolicitudPermisoViewModel> ConsultarSolicitudesRealizadas(string cedula)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                

                var listSolicitudes = entities.sp_SolicitudesRealizadas(cedula).ToList();


                List<SolicitudPermisoViewModel> listaRetorna = new List<SolicitudPermisoViewModel>();

                foreach (var itemBase in listSolicitudes)
                {                   

                    listaRetorna.Add(new SolicitudPermisoViewModel {
                        IdSolicitudPermiso = itemBase.ID
                        ,DescripcionMotivo = itemBase.MOTIVO
                        ,Observacion = itemBase.OBSERVACION
                        ,FechaSalida = itemBase.FECHA_DESDE
                        ,FechaRegreso = itemBase.FECHA_HASTA
                        ,DescripcionEstadoSolicitud = itemBase.ESTADO
                        ,UsuarioIngresoLog = itemBase.USUARIO_CREA
                        ,FechaIngresoLog = itemBase.FECHA_CREACION
                    });

                }

                return listaRetorna;
            }
        }
    }
} 