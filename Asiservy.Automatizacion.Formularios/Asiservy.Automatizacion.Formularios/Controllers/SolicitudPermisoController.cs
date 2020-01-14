using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Datos.Datos;
using System.Net;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using System.Data.Entity.Validation;

namespace Asiservy.Automatizacion.Formularios.Controllers
{

    public class SolicitudPermisoController : Controller
    {
        clsDClasificador clsDClasificador = null;
        clsDSolicitudPermiso clsDSolicitudPermiso = null;
        clsDEmpleado clsDEmpleado = null;
        clsDGeneral clsDGeneral = null;
        clsDLogin clsDLogin = null;
        clsDError clsDError = null;
        clsApiUsuario clsApiUsuario = null;
        string[] lsUsuario;
        #region BANDEJAS
        [Authorize]
        // GET: SolicitudPermiso
        public ActionResult BandejaAprobacion()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];


                List<SolicitudPermisoViewModel> ListaSolicitud;
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                clsDGeneral = new clsDGeneral();

                lsUsuario = User.Identity.Name.Split('_');
                ViewBag.Linea = clsDGeneral.ConsultarLineaUsuario(lsUsuario[1]);
                ListaSolicitud = clsDSolicitudPermiso.ConsultaSolicitudesPermiso(clsAtributos.EstadoSolicitudPendiente, lsUsuario[1]);
                return View(ListaSolicitud);
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return View();
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return View();
            }
        }

        [Authorize]
        public ActionResult BandejaRRHH()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];


                List<SolicitudPermisoViewModel> ListaSolicitud;
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                ListaSolicitud = clsDSolicitudPermiso.ConsultaSolicitudesPermiso(clsAtributos.EstadoSolicitudAprobado, null);
                return View(ListaSolicitud);
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return View();
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return View();
            }
        }

        [Authorize]
        public JsonResult AprobarSolicitud(string[] diIdSolicitud)
        {
            try
            {
                string psRespuesta = string.Empty;
                if (diIdSolicitud != null && diIdSolicitud.Length > 0)
                {
                    foreach (var psIdSolicitud in diIdSolicitud)
                    {
                        if (!string.IsNullOrEmpty(psIdSolicitud))
                        {
                            clsDSolicitudPermiso = new clsDSolicitudPermiso();
                            SOLICITUD_PERMISO model = new SOLICITUD_PERMISO();
                            model.IdSolicitudPermiso = int.Parse(psIdSolicitud);
                            model.EstadoSolicitud = clsAtributos.EstadoSolicitudAprobado;
                            model.FechaModificacionLog = DateTime.Now;
                            lsUsuario = User.Identity.Name.Split('_');
                            model.UsuarioModificacionLog = lsUsuario[0];
                            model.TerminalModificacionLog = Request.UserHostAddress;

                            psRespuesta = clsDSolicitudPermiso.CambioEstadoSolicitud(model);
                        }
                          
                    }
                    return Json(psRespuesta, JsonRequestBehavior.AllowGet);
                }
                return Json("Error, no se ha enviado ninguna solicitud", JsonRequestBehavior.AllowGet);

            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public JsonResult AnularSolicitud(string diIdSolicitud, string dsObservacion)
        {
            try
            {
                if (!string.IsNullOrEmpty(diIdSolicitud))
                {
                    clsDSolicitudPermiso = new clsDSolicitudPermiso();
                    SOLICITUD_PERMISO model = new SOLICITUD_PERMISO();
                    model.IdSolicitudPermiso = int.Parse(diIdSolicitud);
                    model.Observacion = dsObservacion;
                    model.EstadoSolicitud = clsAtributos.EstadoSolicitudAnulado;
                    model.FechaModificacionLog = DateTime.Now;
                    lsUsuario = User.Identity.Name.Split('_');
                    model.UsuarioModificacionLog = lsUsuario[0] + "";
                    model.TerminalModificacionLog = Request.UserHostAddress;



                    string psRespuesta = clsDSolicitudPermiso.CambioEstadoSolicitud(model);
                    return Json(psRespuesta, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error, Numero de solicitud invalida", JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public JsonResult FinalizarSolicitud(string[] diIdSolicitud)
        {
            try
            {
                List<RespuestaGeneral> psRespuesta = new List<RespuestaGeneral>() ;
                if (diIdSolicitud != null && diIdSolicitud.Length > 0)
                {
                    foreach (var psIdSolicitud in diIdSolicitud)
                    {
                        if (!string.IsNullOrEmpty(psIdSolicitud))
                        {
                            clsDSolicitudPermiso = new clsDSolicitudPermiso();
                            clsDClasificador = new clsDClasificador();
                            SOLICITUD_PERMISO model = new SOLICITUD_PERMISO();
                            model.IdSolicitudPermiso = int.Parse(psIdSolicitud);
                            model.EstadoSolicitud = clsAtributos.EstadoSolicitudRevisado;
                            model.FechaModificacionLog = DateTime.Now;
                            lsUsuario = User.Identity.Name.Split('_');
                            model.UsuarioModificacionLog = lsUsuario[0];
                            model.TerminalModificacionLog = Request.UserHostAddress;
                            var envioOnly = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodigoGrupoSwitchServices, Codigo = clsAtributos.CodigoEnvioOnlyControl }).FirstOrDefault();
                            if (envioOnly.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                            {
                                psRespuesta = clsDSolicitudPermiso.EnviarSolicitudOnlyControl(model);
                            }
                            else
                            {
                                string mensaje = clsDSolicitudPermiso.CambioEstadoSolicitud(model);
                                psRespuesta.Add(new RespuestaGeneral { Mensaje = mensaje, Respuesta = true });
                            }
                        }
                    }
                    return Json(psRespuesta, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json("Error, no se ha enviado ninguna solicitud", JsonRequestBehavior.AllowGet);

            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public ActionResult SolicitudPermisoEdit(string dsSolicitud, string frm)
        {
            try
            {

                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                SolicitudPermisoViewModel model = clsDSolicitudPermiso.ConsultaSolicitudPermiso(dsSolicitud);
                if (model.Origen == clsAtributos.SolicitudOrigenGeneral && string.IsNullOrEmpty(frm))
                { 
                    ConsultaCombosMotivos(false);
                }
                else if (!string.IsNullOrEmpty(frm) && frm == "BandejaMedico")
                {
                    ConsultaCombosMedicos(true);
                    ViewBag.CodigosEnfermedad = clsDGeneral.ConsultaCodigosGrupoSubEnfermedad(clsAtributos.CodGrupoEnfermedadDiagnostico, "", "");
                    ViewBag.Medico = "1";
                    if (!string.IsNullOrEmpty(model.CodigoDiagnostico))
                    {
                        ViewBag.NombreDiagnoatico = (from d in ViewBag.CodigosEnfermedad as List<sp_GrupoEnfermedades>
                                                    where d.Codigo == model.CodigoDiagnostico
                                                    select d.Descripcion).FirstOrDefault();
                    }
                    
                }
                else if (!string.IsNullOrEmpty(frm) && frm == "BandejaRRHH")
                {
                    clsDClasificador = new clsDClasificador();
                    ConsultaCombosMotivos(true);
                    ViewBag.ClasificaroMedico = clsDClasificador.ConsultarClasificador("001", "0");
                    ViewBag.CodigosEnfermedad = clsDGeneral.ConsultaCodigosGrupoSubEnfermedad(clsAtributos.CodGrupoEnfermedadDiagnostico, "", "");
                    ViewBag.Medico = "1";
                    if (!string.IsNullOrEmpty(model.CodigoDiagnostico))
                    {
                        ViewBag.NombreDiagnoatico = (from d in ViewBag.CodigosEnfermedad as List<sp_GrupoEnfermedades>
                                                     where d.Codigo == model.CodigoDiagnostico
                                                     select d.Descripcion).FirstOrDefault();
                    }

                }
                else if (!string.IsNullOrEmpty(frm) && frm == "BandejaAprobacion")
                {
                    clsDClasificador = new clsDClasificador();
                    ConsultaCombosMotivos(true);
                    ViewBag.ClasificaroMedico = clsDClasificador.ConsultarClasificador("001", "0");
                    ViewBag.CodigosEnfermedad = clsDGeneral.ConsultaCodigosGrupoSubEnfermedad(clsAtributos.CodGrupoEnfermedadDiagnostico, "", "");
                    
                }
                else
                {
                    ConsultaCombosMedicos(false);
                    ViewBag.CodigosEnfermedad = clsDGeneral.ConsultaCodigosGrupoSubEnfermedad(clsAtributos.CodGrupoEnfermedadDiagnostico, "", model.CodigoDiagnostico);

                }

                if (!string.IsNullOrEmpty(frm) &&   frm == "BandejaRRHH")
                    ViewBag.Justifica = "Justifica";

                return PartialView(model);
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return PartialView();
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return PartialView();
            }         
            
        }


        [Authorize]
        [HttpPost]
        public ActionResult SolicitudPermisoEdit(SolicitudPermisoViewModel doSolicitud, string frm)
        {
            try
            {
                if (doSolicitud != null && doSolicitud.IdSolicitudPermiso > 0)
                {
                    clsDSolicitudPermiso = new clsDSolicitudPermiso();
                    SOLICITUD_PERMISO poSolicitudPermiso = new SOLICITUD_PERMISO();
                    poSolicitudPermiso.IdSolicitudPermiso = doSolicitud.IdSolicitudPermiso;
                    poSolicitudPermiso.Observacion = doSolicitud.Observacion;
                    if(!string.IsNullOrEmpty(doSolicitud.CodigoClasificador))
                        poSolicitudPermiso.CodigoClasificador = int.Parse(doSolicitud.CodigoClasificador);
                    if(!string.IsNullOrEmpty(doSolicitud.CodigoDiagnostico))
                        poSolicitudPermiso.CodigoDiagnostico = doSolicitud.CodigoDiagnostico;
                    poSolicitudPermiso.CodigoMotivo = doSolicitud.CodigoMotivo;
                    poSolicitudPermiso.NombreMedico = doSolicitud.NombreMedico;
                    poSolicitudPermiso.FechaSalida = doSolicitud.FechaSalida ?? poSolicitudPermiso.FechaSalida;
                    poSolicitudPermiso.FechaRegreso = doSolicitud.FechaRegreso ?? poSolicitudPermiso.FechaRegreso;
                    poSolicitudPermiso.FechaModificacionLog = DateTime.Now;
                    lsUsuario = User.Identity.Name.Split('_');
                    poSolicitudPermiso.UsuarioModificacionLog = lsUsuario[0] + "";
                    poSolicitudPermiso.TerminalModificacionLog = Request.UserHostAddress;

                    foreach (var detalle in doSolicitud.JustificaSolicitudes)
                    {
                        if (detalle.CodigoMotivo != null)
                        {
                            poSolicitudPermiso.JUSTICA_SOLICITUD.Add(detalle);
                        }
                    }
                    string Respuesta = clsDSolicitudPermiso.GuargarModificarSolicitud(poSolicitudPermiso);
                    SetSuccessMessage(Respuesta);
                }
                else
                {
                    SetErrorMessage("Solicitud Invalida");
                }
                if (!string.IsNullOrEmpty(frm) && frm == "BandejaRRHH")
                    return RedirectToAction("BandejaRRHH");
                else if(!string.IsNullOrEmpty(frm) && frm == "BandejaMedico")
                    return RedirectToAction("BandejaMedico");
                else
                    return RedirectToAction("BandejaAprobacion");
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                if (!string.IsNullOrEmpty(frm) && frm == "BandejaRRHH")
                    return RedirectToAction("BandejaRRHH");
                else
                    return RedirectToAction("BandejaAprobacion");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                if (!string.IsNullOrEmpty(frm) && frm == "BandejaRRHH")
                    return RedirectToAction("BandejaRRHH");
                else
                    return RedirectToAction("BandejaAprobacion");
            }           

        }
        #endregion

        #region BANDEJAMEDICO
        public ActionResult BandejaMedico()
        {
            try{
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                var model = clsDSolicitudPermiso.ConsultaSolicitudesPermiso(new SOLICITUD_PERMISO {
                    //FechaSalida = DateTime.Now.AddMonths(-1),
                    //FechaRegreso = DateTime.Now,
                    EstadoSolicitud= clsAtributos.EstadoSolicitudAprobado,
                    Origen = clsAtributos.SolicitudOrigenMedico,
                    ValidaMedico = true
                });

                return View(model);
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }           
        }
        #endregion

        #region SOLICITUD PERMISO

        
        [Authorize]
        public ActionResult SolicitudPermisoMasivo()
        {
            try
            {
                clsDEmpleado = new clsDEmpleado();
                clsDClasificador = new clsDClasificador();
                lsUsuario = User.Identity.Name.Split('_');
                ViewBag.dataTableJS2 = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                var poEmpleado= clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                ViewBag.Linea = poEmpleado.LINEA;
                if(poEmpleado.CODIGOLINEA == clsAtributos.CodLineaProduccion)
                {
                    ViewBag.Lineas = clsDClasificador.ConsultarClasificador(clsAtributos.CodGrupoLineasAprobarSolicitudProduccion, "0");
                    
                }
                ViewBag.CodLinea = poEmpleado.CODIGOLINEA;
                //ValidacionSolicitudPermiso();
                ConsultaCombosMotivos(false);
                return View();
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SolicitudPermisoMasivo(SolicitudPermisoViewModel model, List<string> Cedulas)
        {
            try
            {
                RespuestaGeneral respuestaGeneral = new RespuestaGeneral();
                clsDEmpleado = new clsDEmpleado();
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                SOLICITUD_PERMISO solicitudPermiso = new SOLICITUD_PERMISO();
                lsUsuario = User.Identity.Name.Split('_');
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                string psMensajeValidarFecha = string.Empty;
                psMensajeValidarFecha = ValidarFechas(model);
                if (!Cedulas.Any())
                {
                    respuestaGeneral.Codigo = 0;
                    respuestaGeneral.Mensaje = "Debe seleccionar al menos un empleado";
                    return Json(respuestaGeneral, JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrEmpty(psMensajeValidarFecha))
                {
                    respuestaGeneral.Codigo = 0;
                    respuestaGeneral.Mensaje = psMensajeValidarFecha;
                    return Json(respuestaGeneral, JsonRequestBehavior.AllowGet);
                }
                solicitudPermiso.CodigoLinea = model.CodigoLinea;
                solicitudPermiso.Observacion = model.Observacion;
                if (model.FechaSalidaEntrada == null)
                {
                    solicitudPermiso.FechaSalida = model.FechaSalida ?? DateTime.MinValue;
                    solicitudPermiso.FechaRegreso = model.FechaRegreso ?? DateTime.MinValue;
                }
                else
                {
                    solicitudPermiso.FechaSalida = new DateTime(
                        model.FechaSalidaEntrada.Value.Year
                        , model.FechaSalidaEntrada.Value.Month
                        , model.FechaSalidaEntrada.Value.Day
                        , model.HoraSalida.Value.Hour
                        , model.HoraSalida.Value.Minute
                        , model.HoraSalida.Value.Second
                        );

                    solicitudPermiso.FechaRegreso = new DateTime(
                        model.FechaSalidaEntrada.Value.Year
                        , model.FechaSalidaEntrada.Value.Month
                        , model.FechaSalidaEntrada.Value.Day
                        , model.HoraRegreso.Value.Hour
                        , model.HoraRegreso.Value.Minute
                        , model.HoraRegreso.Value.Second
                        );
                }
                solicitudPermiso.EstadoSolicitud = clsAtributos.EstadoSolicitudAprobado;
                //solicitudPermiso.FechaBiometrico = DateTime.Now;
                solicitudPermiso.Origen = clsAtributos.SolicitudOrigenGeneral;
                solicitudPermiso.CodigoDiagnostico = "";
                solicitudPermiso.CodigoClasificador = 0;
               
                solicitudPermiso.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                solicitudPermiso.UsuarioIngresoLog = lsUsuario[0];
                solicitudPermiso.FechaIngresoLog = DateTime.Now;
                solicitudPermiso.TerminalIngresoLog = Request.UserHostAddress;
                if (model.CodigoMotivo == clsAtributos.CodigoMotivoPermisoCitaMedica)
                {
                    solicitudPermiso.ValidaMedico = true;
                    solicitudPermiso.Origen = clsAtributos.SolicitudOrigenMedico;
                }
                solicitudPermiso.CodigoMotivo = model.CodigoMotivo;
                solicitudPermiso.Identificacion = lsUsuario[1];
                int Cantidad= clsDSolicitudPermiso.GenerarSolicitudPermisoMasivo(solicitudPermiso, Cedulas);
                respuestaGeneral.Codigo = 1;
                respuestaGeneral.Mensaje = Cantidad+" Solicitudes generadas con éxito.";
                return Json(respuestaGeneral, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult SolicitudPermiso()
        {
            try
            {
                clsDEmpleado = new clsDEmpleado();
                lsUsuario = User.Identity.Name.Split('_');
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ValidacionSolicitudPermiso();
                ConsultaCombosMotivos(false);
                return View();
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }           
        }
        public JsonResult ValidarMedicoSolicitud(string diIdSolicitud)
        {
            try
            {
                string psRespuesta = string.Empty;
                if (diIdSolicitud != null && diIdSolicitud.Length > 0)
                {                   
                    clsDSolicitudPermiso = new clsDSolicitudPermiso();
                    SOLICITUD_PERMISO model = new SOLICITUD_PERMISO();
                    model.IdSolicitudPermiso = int.Parse(diIdSolicitud);                           
                    model.FechaModificacionLog = DateTime.Now;
                    lsUsuario = User.Identity.Name.Split('_');
                    model.UsuarioModificacionLog = lsUsuario[0];
                    model.TerminalModificacionLog = Request.UserHostAddress;
                    model.ValidaMedico = false;
                    psRespuesta=clsDSolicitudPermiso.CambioEstadoSolicitud(model);
                    return Json(psRespuesta, JsonRequestBehavior.AllowGet);
                }
                return Json("Error, no se ha enviado ninguna solicitud", JsonRequestBehavior.AllowGet);

            }

            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SolicitudPermiso(SolicitudPermisoViewModel model)
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                //var errors = ModelState
                //.Where(x => x.Value.Errors.Count > 0)
                //.Select(x => new { x.Key, x.Value.Errors })
                //.ToArray();
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                string psMensajeValidarFecha = string.Empty;
                if (model.CodigoMotivo == clsAtributos.CodigoMotivoPermisoComisionServicio
                    && string.IsNullOrEmpty(model.Observacion))
                {
                    ConsultaCombosMotivos(false);
                    ValidacionSolicitudPermiso();
                    SetErrorMessage("La observación es obligatoria para este tipo de solicitud.");
                    return View(model);
                }
                psMensajeValidarFecha = ValidarFechas(model);
                if(!string.IsNullOrEmpty(psMensajeValidarFecha))
                {
                    //int piContrladorLinea = ValidarRolControladorLinea();
                    //if (piContrladorLinea > 0)
                    //    ViewBag.ControladorLinea = piContrladorLinea;
                    ConsultaCombosMotivos(false);
                    ValidacionSolicitudPermiso();
                    ModelState.AddModelError("CustomError", psMensajeValidarFecha);
                    return View(model);
                }         

                if (ModelState.IsValid)
                {
                    SOLICITUD_PERMISO solicitudPermiso = new SOLICITUD_PERMISO();
                    clsDSolicitudPermiso = new clsDSolicitudPermiso();
                    clsDEmpleado = new clsDEmpleado();
                    lsUsuario = User.Identity.Name.Split('_');
                    var poEmpleado = clsDEmpleado.ConsultaEmpleado(model.Identificacion).FirstOrDefault();
                    if(poEmpleado==null)
                    {
                        SetErrorMessage("No existe información del empleado.");
                        return RedirectToAction("SolicitudPermiso");
                    }
                    solicitudPermiso.CodigoLinea = poEmpleado.CODIGOLINEA;
                    solicitudPermiso.CodigoArea = poEmpleado.CODIGOAREA;
                    solicitudPermiso.CodigoRecurso = poEmpleado.CODIGORECURSO;
                    solicitudPermiso.CodigoCargo = poEmpleado.CODIGOCARGO;
                    solicitudPermiso.Identificacion = model.Identificacion;
                    solicitudPermiso.CodigoMotivo = model.CodigoMotivo;
                    solicitudPermiso.Observacion = model.Observacion==null?"":model.Observacion.ToUpper();

                    if (model.FechaSalidaEntrada == null)
                    {
                        solicitudPermiso.FechaSalida = model.FechaSalida ?? DateTime.MinValue;
                        solicitudPermiso.FechaRegreso = model.FechaRegreso ?? DateTime.MinValue;
                    }
                    else
                    {
                        solicitudPermiso.FechaSalida = new DateTime(
                            model.FechaSalidaEntrada.Value.Year
                            , model.FechaSalidaEntrada.Value.Month
                            , model.FechaSalidaEntrada.Value.Day
                            , model.HoraSalida.Value.Hour
                            , model.HoraSalida.Value.Minute
                            , model.HoraSalida.Value.Second
                            );

                        solicitudPermiso.FechaRegreso = new DateTime(
                            model.FechaSalidaEntrada.Value.Year
                            , model.FechaSalidaEntrada.Value.Month
                            , model.FechaSalidaEntrada.Value.Day
                            , model.HoraRegreso.Value.Hour
                            , model.HoraRegreso.Value.Minute
                            , model.HoraRegreso.Value.Second
                            );
                    }
                    solicitudPermiso.EstadoSolicitud = clsAtributos.EstadoSolicitudPendiente;
                    //solicitudPermiso.FechaBiometrico = DateTime.Now;
                    solicitudPermiso.Origen = clsAtributos.SolicitudOrigenGeneral;
                    solicitudPermiso.CodigoDiagnostico = "";
                    solicitudPermiso.CodigoClasificador = 0;
                    solicitudPermiso.EstadoRegistro = clsAtributos.EstadoRegistroActivo;                    
                    solicitudPermiso.Nivel = clsDSolicitudPermiso.ConsultarNivelUsuario(model.Identificacion);
                    solicitudPermiso.UsuarioIngresoLog = lsUsuario[0];
                    solicitudPermiso.FechaIngresoLog = DateTime.Now;
                    solicitudPermiso.TerminalIngresoLog = Request.UserHostAddress;
                    if(model.CodigoMotivo ==clsAtributos.CodigoMotivoPermisoCitaMedica)
                    {
                        solicitudPermiso.ValidaMedico = true;
                        solicitudPermiso.Origen = clsAtributos.SolicitudOrigenMedico;
                    }
                    var Motivo= clsDSolicitudPermiso.ConsultarMotivos(solicitudPermiso.CodigoMotivo).FirstOrDefault();

                    string psRespuesta = clsDSolicitudPermiso.GuargarModificarSolicitud(solicitudPermiso);                    
                    string mensajeCorreo=clsDGeneral.EnvioCorreo(poEmpleado, "Solicitud Permiso",
                        "Empleado: "+ poEmpleado.NOMBRES+ "\n</br>"
                        + "Motivo:" + Motivo.DescripcionMotivo+ "\n</br>"
                        + "Observación:" + solicitudPermiso.Observacion+ "\n</br>"
                        + "Fecha Salida: " + solicitudPermiso.FechaSalida+ "\n</br>" 
                        + "Fecha Regreso: " + solicitudPermiso.FechaRegreso+ "</br>\n Estado: Pendiente </br></br>",false);


                    SetSuccessMessage(string.Format(psRespuesta+" -- "+ mensajeCorreo));
                }
                else
                {
                    ValidacionSolicitudPermiso();
                    ConsultaCombosMotivos(false);
                    return View(model);
                }

                return RedirectToAction("SolicitudPermiso");
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("SolicitudPermiso");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("SolicitudPermiso");
            }            
        }


        public void ValidacionSolicitudPermiso()
        {
            clsDGeneral = new clsDGeneral();
            clsDEmpleado = new clsDEmpleado();
            clsDClasificador = new clsDClasificador();

            int piContrladorLinea = ValidarRolControladorLinea();
            int piPermisoCompartido = ValidarRolGeneraPermisoCompartido();
            
            lsUsuario = User.Identity.Name.Split('_');               
            var poEmpleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
            string Cedula = lsUsuario[1] + "";

            if (piPermisoCompartido > 0)
            {
                ViewBag.PermisoCompartido = piPermisoCompartido;
                ViewBag.Lineas = clsDGeneral.ConsultaLineas(poEmpleado.CODIGOLINEA);

            }else if (piContrladorLinea > 0)
            {
                ViewBag.ControladorLinea = piContrladorLinea;
                ViewBag.CodLinea = poEmpleado.CODIGOLINEA;
                ViewBag.Lineas = clsDClasificador.ConsultarClasificador(clsAtributos.CodGrupoLineaProduccion, "0");
            }
            else
            {
                ViewBag.NombreEmpleado = poEmpleado != null ? poEmpleado.NOMBRES : lsUsuario[0];
                ViewData["Identificacion"] = Cedula;
            }

        }

        [Authorize]
        public ActionResult ConsultarGrupoEnfermedades()
        {
            clsDSolicitudPermisoMedico pSPermisoMedico = new clsDSolicitudPermisoMedico();
            var ListGrupoEnfermedades = pSPermisoMedico.ConsultaGrupoEnfermedades(clsAtributos.CodGrupoEnfermedadDiagnostico, "",null);
            return PartialView(ListGrupoEnfermedades);
        }

        [Authorize]
        public ActionResult SolicitudPermisoDispensario()
        {
            try
            {
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ConsultaCombosMedicos(false);
                return View();
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }          
        }

        [Authorize]
        [HttpPost]
        public ActionResult SolicitudPermisoDispensario(SolicitudPermisoViewModel model)
        {
            try
            {
                //var errors = ModelState
                //.Where(x => x.Value.Errors.Count > 0)
                //.Select(x => new { x.Key, x.Value.Errors })
                //.ToArray();

                string psMensajeValidarFecha = string.Empty;
                psMensajeValidarFecha = ValidarFechas(model);
                if (!string.IsNullOrEmpty(psMensajeValidarFecha))
                {
                    ConsultaCombosMedicos(false);
                    ModelState.AddModelError("CustomError", psMensajeValidarFecha);
                    return View(model);
                }

                if (ModelState.IsValid)
                {
                    SOLICITUD_PERMISO solicitudPermiso = new SOLICITUD_PERMISO();
                    clsDEmpleado = new clsDEmpleado();
                    clsDSolicitudPermiso = new clsDSolicitudPermiso();
                    var poEmpleado = clsDEmpleado.ConsultaEmpleado(model.Identificacion).FirstOrDefault();
                    if (poEmpleado == null)
                    {
                        SetErrorMessage("No existe información del empleado.");
                        return RedirectToAction("SolicitudPermisoDispensario");
                    }
                    solicitudPermiso.CodigoLinea = poEmpleado.CODIGOLINEA;
                    solicitudPermiso.CodigoArea = poEmpleado.CODIGOAREA;
                    solicitudPermiso.CodigoCargo = poEmpleado.CODIGOCARGO;
                    solicitudPermiso.CodigoRecurso = poEmpleado.CODIGORECURSO;
                    solicitudPermiso.Identificacion = model.Identificacion;
                    solicitudPermiso.CodigoMotivo = model.CodigoMotivo;
                    solicitudPermiso.Observacion = model.Observacion;

                    if (model.FechaSalidaEntrada == null)
                    {
                        solicitudPermiso.FechaSalida = model.FechaSalida ?? DateTime.MinValue;
                        solicitudPermiso.FechaRegreso = model.FechaRegreso ?? DateTime.MinValue;
                    }
                    else
                    {
                        solicitudPermiso.FechaSalida = new DateTime(
                            model.FechaSalidaEntrada.Value.Year
                            , model.FechaSalidaEntrada.Value.Month
                            , model.FechaSalidaEntrada.Value.Day
                            , model.HoraSalida.Value.Hour
                            , model.HoraSalida.Value.Minute
                            , model.HoraSalida.Value.Second
                            );

                        solicitudPermiso.FechaRegreso = new DateTime(
                            model.FechaSalidaEntrada.Value.Year
                            , model.FechaSalidaEntrada.Value.Month
                            , model.FechaSalidaEntrada.Value.Day
                            , model.HoraRegreso.Value.Hour
                            , model.HoraRegreso.Value.Minute
                            , model.HoraRegreso.Value.Second
                            );
                    }
                    solicitudPermiso.EstadoSolicitud = clsAtributos.EstadoSolicitudAprobado;
                    //solicitudPermiso.FechaBiometrico = DateTime.Now;
                    solicitudPermiso.Origen = clsAtributos.SolicitudOrigenMedico;
                    solicitudPermiso.CodigoDiagnostico = model.CodigoDiagnostico;
                    solicitudPermiso.CodigoClasificador = int.Parse(model.CodigoClasificador);
                    solicitudPermiso.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    lsUsuario = User.Identity.Name.Split('_');
                    solicitudPermiso.Nivel = clsDSolicitudPermiso.ConsultarNivelUsuario(model.Identificacion + "");

                    solicitudPermiso.FechaIngresoLog = DateTime.Now;
                    solicitudPermiso.UsuarioIngresoLog = lsUsuario[0] + "";
                    solicitudPermiso.TerminalIngresoLog = Request.UserHostAddress;
                    string psRespuesta = clsDSolicitudPermiso.GuargarModificarSolicitud(solicitudPermiso);
                    SetSuccessMessage(string.Format(psRespuesta));
                }
                else
                {
                    ConsultaCombosMedicos(false);
                    return View(model);
                }

                return RedirectToAction("SolicitudPermisoDispensario");
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("SolicitudPermisoDispensario");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("SolicitudPermisoDispensario");
            }           
        }
        #endregion 

        #region REPORTE DE SOLICITUD PERMISO
            
      
        [Authorize]
        public ActionResult ReporteSolicitud()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];


                clsDGeneral = new clsDGeneral();
                ViewBag.Lineas = clsDGeneral.ConsultaLineas("0");
                ViewBag.Estados = clsDGeneral.ConsultarEstadosSolicitudSelect();
                int RolGarita = ValidarRolGarita();
                if (RolGarita > 0)
                    ViewBag.Garita = RolGarita;
                return View();
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }    
        }

       
        [HttpPost]
        public JsonResult MarcarSalidaSolicitudPermiso(int IdSolicitudPermiso, DateTime FechaBiometrico,DateTime FechaSalida)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                clsDParametro clsDParametro = new clsDParametro();
                var poTiempo=clsDParametro.ConsultaParametros(new PARAMETRO {Codigo=clsAtributos.TiempoSalidaGarita, EstadoRegistro=clsAtributos.EstadoRegistroActivo }).FirstOrDefault();
                int tiempo = poTiempo != null ? poTiempo.Valor : -9;     
                // Difference in days, hours, and minutes.
                TimeSpan ts = FechaBiometrico - FechaSalida;
                // Difference in days.
                int differenceInDays = ts.Days;
        
                if (differenceInDays < 0 || ts.Hours < 0 ||  ts.Minutes < tiempo)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }



                lsUsuario = User.Identity.Name.Split('_');

                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                SOLICITUD_PERMISO solicitud = new SOLICITUD_PERMISO();
                solicitud.IdSolicitudPermiso = IdSolicitudPermiso;
                solicitud.FechaBiometrico = FechaBiometrico;
                solicitud.UsuarioIngresoLog = lsUsuario[0];
                solicitud.FechaIngresoLog = DateTime.Now;
                solicitud.TerminalIngresoLog = Request.UserHostAddress;

                string Mensaje= clsDSolicitudPermiso.MarcarHoraSalidaSolicitudPermiso(solicitud);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ConsultaSolicitudes(string dsLinea, string dsArea, string dsEstado, bool dsGarita=false, DateTime? ddFechaDesde=null, DateTime? ddFechaHasta = null)
        {
            lsUsuario = User.Identity.Name.Split('_');
            if (string.IsNullOrEmpty(lsUsuario[0]))
            {
                return Json("101", JsonRequestBehavior.AllowGet);
            }
            clsDSolicitudPermiso poSolicitudPermiso = new clsDSolicitudPermiso();
            int RolGarita = ValidarRolGarita();
            if (RolGarita > 0)
                ViewBag.Garita = RolGarita;
            var pListSolicitudPermiso = poSolicitudPermiso.ConsultaSolicitudesPermisoReporte(dsLinea, dsArea, dsEstado, dsGarita, ddFechaDesde, ddFechaHasta);
            return PartialView(pListSolicitudPermiso);
        }

        [Authorize]
        public ActionResult SolicitudesRealizadas()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                //ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                
                lsUsuario = User.Identity.Name.Split('_');
                string Cedula = lsUsuario[1];

                var solicitudes = clsDSolicitudPermiso.ConsultarSolicitudesRealizadas(Cedula);

                return View(solicitudes);
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
        }
        #endregion

        #region BITACORA SOLICITUD
        [Authorize]
        public ActionResult BitacoraSolicitud()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                lsUsuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDClasificador = new clsDClasificador();
                clsDLogin = new clsDLogin();
                clsDGeneral = new clsDGeneral();
                if(clsDLogin.ValidarUsuarioRol(lsUsuario[1],clsAtributos.AsistenteProduccion))
                    ViewBag.Lineas = clsDClasificador.ConsultarClasificador(clsAtributos.CodGrupoLineasAprobarSolicitudProduccion, "0");
                if (clsDLogin.ValidarUsuarioRol(lsUsuario[1], clsAtributos.RolControladorGeneral) || clsDLogin.ValidarUsuarioRol(lsUsuario[1], clsAtributos.RolSupervisorGeneral))
                    ViewBag.Lineas = clsDClasificador.ConsultarClasificador(clsAtributos.CodGrupoLineasAprobarSolicitudProduccion, "0");
                if (clsDLogin.ValidarUsuarioRol(lsUsuario[1], clsAtributos.RolSupervisorLinea) || clsDLogin.ValidarUsuarioRol(lsUsuario[1], clsAtributos.RolControladorLinea))
                {
                    var empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                    ViewBag.Lineas = clsDClasificador.ConsultarClasificador(clsAtributos.CodGrupoLineaProduccion, empleado.CODIGOLINEA);
                }
                if(clsDLogin.ValidarUsuarioRol(lsUsuario[1], clsAtributos.RolRRHH))
                {
                    ViewBag.Lineas = clsDGeneral.ConsultaLineas("0");
                }else
                {
                    var empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                    ViewBag.Lineas = clsDGeneral.ConsultaLineas(empleado.CODIGOLINEA);
                }

                return View();
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }        
        }

        [Authorize]
        public ActionResult BitacoraSolicitudPartial(string dsIdSolicitud, string dsCedula, DateTime? ddFechaDesde, DateTime? ddFechaHasta)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                if (string.IsNullOrEmpty(dsIdSolicitud) && string.IsNullOrEmpty(dsCedula))
                {
                    return Json(new { Failed = true, Mensaje = "Ingrese parametros de consulta" }, JsonRequestBehavior.AllowGet);
                }
                List<BitacoraSolicitud> model = clsDSolicitudPermiso.ConsultaBitacoraSolicitud(dsIdSolicitud, dsCedula, ddFechaDesde, ddFechaHasta);
                return PartialView(model);

            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje1 = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(new { Failed = true, Mensaje = Mensaje1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje1 = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(new { Failed = true, Mensaje = Mensaje1 }, JsonRequestBehavior.AllowGet);
            }           
        }
        #endregion

        #region CONSULTAS

      
        public JsonResult ObtenerSubGrupoEnfermedades(string GrupoEnfermedad)
        {
            clsDGeneral = new clsDGeneral();
            List<sp_GrupoEnfermedades> pListSubGrupoEnfermedades = clsDGeneral.ConsultaCodigosGrupoSubEnfermedad("S", GrupoEnfermedad, "").ToList();
            return Json(pListSubGrupoEnfermedades, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ObtenerEnfermedades(string SubGrupoEnfermedad)
        {
            clsDGeneral = new clsDGeneral();
            List<sp_GrupoEnfermedades> pListEnfermedades = clsDGeneral.ConsultaCodigosGrupoSubEnfermedad("E", "", SubGrupoEnfermedad).ToList();
            return Json(pListEnfermedades, JsonRequestBehavior.AllowGet);
        }

        public string ValidarFechas(SolicitudPermisoViewModel model)
        {
            string psMensaje = string.Empty;
            if ((model.FechaSalida == null || model.FechaRegreso == null) && model.FechaSalidaEntrada == null)
            {
                psMensaje="Debe Ingresar un rango de horas o fechas";
                return psMensaje;
            }
            else if (model.FechaSalidaEntrada != null && (model.HoraRegreso == null || model.HoraSalida == null))
            {
                psMensaje="Debe Ingresar un rango de horas correcto";
                return psMensaje;
            }
            else if (model.FechaSalida != null && model.FechaRegreso != null && model.FechaSalida > model.FechaRegreso)
            {
                psMensaje="Fecha de salida no puede ser mayor a la de regreso";
                return psMensaje;
            }
            else if (model.HoraRegreso != null && model.HoraSalida != null && model.HoraSalida.Value.Hour > model.HoraRegreso.Value.Hour)
            {
                psMensaje="Hora de salida no puede ser mayor a la de regreso";
                return psMensaje;
            }

            return psMensaje;
        }


        public int ValidarRolControladorLinea()
        {
            clsDLogin = new clsDLogin();
            lsUsuario = User.Identity.Name.Split('_');
            List<int?> roles = clsDLogin.ConsultaRolesUsuario(lsUsuario[1]);
            int piContrladorLinea=0;
            if (roles.Any())
            {
                piContrladorLinea = roles.FirstOrDefault(x => x.Value == clsAtributos.RolControladorLinea) ?? 0;
            }
            return piContrladorLinea;
        }

        public int ValidarRolGeneraPermisoCompartido()
        {
            clsDLogin = new clsDLogin();
            lsUsuario = User.Identity.Name.Split('_');
            List<int?> roles = clsDLogin.ConsultaRolesUsuario(lsUsuario[1]);
            int piContrladorLinea = 0;
            if (roles.Any())
            {
                piContrladorLinea = roles.FirstOrDefault(x => x.Value == clsAtributos.RolGeneraPermisoCompartido) ?? 0;
            }
            return piContrladorLinea;
        }

        public int ValidarRolGarita()
        {
            clsDLogin = new clsDLogin();
            lsUsuario = User.Identity.Name.Split('_');
            List<int?> roles = clsDLogin.ConsultaRolesUsuario(lsUsuario[1]);
            int piGarita = 0;
            if (roles.Any())
            {
                piGarita = roles.FirstOrDefault(x => x.Value == clsAtributos.RolGarita) ?? 0;
            }
            return piGarita;
        }

        public void ConsultaCombosMotivos(bool RRHH)
        {
            clsDSolicitudPermiso = new clsDSolicitudPermiso();
            clsDGeneral = new clsDGeneral();
            clsDClasificador = new clsDClasificador();
            if (!RRHH)
            {
                List<string> Motivos = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoMotivosExcluidos, "0").Select(x=> x.Codigo).ToList();
                ViewBag.MotivosPermiso = clsDSolicitudPermiso.ConsultarMotivos(null).Where(x => !Motivos.Contains(x.CodigoMotivo));
            }
            else
                ViewBag.MotivosPermiso = clsDSolicitudPermiso.ConsultarMotivos(null);
           
        }
        public void ConsultaCombosMedicos(bool bandeja)
        {
            clsDClasificador = new clsDClasificador();
            clsDSolicitudPermiso = new clsDSolicitudPermiso();
            clsDGeneral = new clsDGeneral();
            clsApiUsuario = new clsApiUsuario();
            ViewBag.ClasificaroMedico = clsDClasificador.ConsultarClasificador("001", "0");
            if (bandeja)
            {
                ViewBag.MotivosPermiso = clsDSolicitudPermiso.ConsultarMotivos(null).Where(x=> x.CodigoMotivo== "CM");
            }
            else
            {
                ViewBag.MotivosPermiso = clsDSolicitudPermiso.ConsultarMotivos(null).Where(x => x.CodigoMotivo == "EN");
            }
            ViewBag.Lineas = clsDGeneral.ConsultaLineas("0");
            lsUsuario = User.Identity.Name.Split('_');
            ViewBag.NombreMedico = clsApiUsuario.ConsultaListaUsuariosSap().FirstOrDefault(x => x.Cedula == lsUsuario[1]).Nombre??"";
            //ViewBag.Areas = clsDGeneral.ConsultaAreas("0");
            //ViewBag.Cargos = clsDGeneral.ConsultaCargos("0");
        }
            protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
   

        public ActionResult EmpleadoBuscar(string dsLinea, string dsArea, string dsCargo,string formulario)
        {
            try
            {
                clsDEmpleado = new clsDEmpleado();
                List<spConsutaEmpleadosFiltro> lista = clsDEmpleado.ConsultaEmpleadosFiltro(dsLinea,dsArea,dsCargo);

                if (!string.IsNullOrEmpty(formulario))
                {
                    if (formulario == "PrestarCuchillo")
                    {
                        lista = clsDEmpleado.RetornaPersonalSinCuchilloAsignado(lista);
                    }
                }
                return PartialView(lista);

            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult ConsultaListadoAreas (string CodLinea)
        {
            try
            {
                clsDGeneral = new clsDGeneral();
            var areas = clsDGeneral.ConsultaAreas(CodLinea);
            return Json(areas, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ConsultaListadoCargos(string CodArea)
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                var areas = clsDGeneral.ConsultaCargos(CodArea);
                return Json(areas, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}
