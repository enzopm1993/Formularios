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

        #region BANDEJAS
        [Authorize]
        // GET: SolicitudPermiso
        public ActionResult BandejaAprobacion()
        {
            try
            {
                List<SolicitudPermisoViewModel> ListaSolicitud;
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                string[] psIdUsuario = User.Identity.Name.Split('_');
                ListaSolicitud = clsDSolicitudPermiso.ConsultaSolicitudesPermiso(clsAtributos.EstadoSolicitudPendiente, psIdUsuario[1]);
                return View(ListaSolicitud);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return View();
            }
        }

        [Authorize]
        public ActionResult BandejaRRHH()
        {
            try
            {
                List<SolicitudPermisoViewModel> ListaSolicitud;
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                ListaSolicitud = clsDSolicitudPermiso.ConsultaSolicitudesPermiso(clsAtributos.EstadoSolicitudAprobado, null);
                return View(ListaSolicitud);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
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
                            string[] psIdUsuario = User.Identity.Name.Split('_');
                            model.UsuarioModificacionLog = psIdUsuario[0];
                            model.TerminalModificacionLog = Request.UserHostAddress;

                            psRespuesta = clsDSolicitudPermiso.CambioEstadoSolicitud(model);
                        }
                    }
                    return Json(psRespuesta, JsonRequestBehavior.AllowGet);
                }
                return Json("Error, no se ha enviado ninguna solicitud", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
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
                    string[] psIdUsuario = User.Identity.Name.Split('_');
                    model.UsuarioModificacionLog = psIdUsuario[0] + "";
                    model.TerminalModificacionLog = Request.UserHostAddress;



                    string psRespuesta = clsDSolicitudPermiso.CambioEstadoSolicitud(model);
                    return Json(psRespuesta, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error, Numero de solicitud invalida", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public JsonResult FinalizarSolicitud(string[] diIdSolicitud)
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
                            model.EstadoSolicitud = clsAtributos.EstadoSolicitudRevisado;
                            model.FechaModificacionLog = DateTime.Now;
                            string[] psIdUsuario = User.Identity.Name.Split('_');
                            model.UsuarioModificacionLog = psIdUsuario[0];
                            model.TerminalModificacionLog = Request.UserHostAddress;



                            psRespuesta = clsDSolicitudPermiso.CambioEstadoSolicitud(model);
                        }
                    }
                    return Json(psRespuesta, JsonRequestBehavior.AllowGet);
                }
                return Json("Error, no se ha enviado ninguna solicitud", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public ActionResult SolicitudPermisoEdit(string dsSolicitud, string frm)
        {
            try
            {
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                SolicitudPermisoViewModel model = clsDSolicitudPermiso.ConsultaSolicitudPermiso(dsSolicitud);
                if (model.Origen == clsAtributos.SolicitudOrigenGeneral)
                {
                    ConsultaCombosGeneral();
                }
                else
                {
                    ConsultaCombosMedicos();

                }

                if (!string.IsNullOrEmpty(frm) && frm == "BandejaRRHH")
                    ViewBag.Justifica = "Justifica";

                return PartialView(model);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
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
                    poSolicitudPermiso.CodigoMotivo = doSolicitud.CodigoMotivo;
                    poSolicitudPermiso.FechaSalida = doSolicitud.FechaSalida ?? poSolicitudPermiso.FechaSalida;
                    poSolicitudPermiso.FechaRegreso = doSolicitud.FechaRegreso ?? poSolicitudPermiso.FechaRegreso;
                    poSolicitudPermiso.FechaModificacionLog = DateTime.Now;
                    string[] psIdUsuario = User.Identity.Name.Split('_');
                    poSolicitudPermiso.UsuarioModificacionLog = psIdUsuario[0] + "";
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
                else
                    return RedirectToAction("BandejaAprobacion");
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                if (!string.IsNullOrEmpty(frm) && frm == "BandejaRRHH")
                    return RedirectToAction("BandejaRRHH");
                else
                    return RedirectToAction("BandejaAprobacion");
            }

        }
        #endregion

        #region SOLICITUD PERMISO
        [Authorize]
        public ActionResult SolicitudPermiso()
        {
            try
            {   
                int piSupervisor = ValidarRolSupervisor();                 
                if(piSupervisor>0)
                    ViewBag.Supervisor = piSupervisor;
                else
                {
                    string[] psIdUsuario = User.Identity.Name.Split('_');
                    clsDEmpleado = new clsDEmpleado();
                    var Nombre = clsDEmpleado.ConsultaEmpleado(psIdUsuario[1]).FirstOrDefault().NOMBRES??"";
                    ViewBag.NombreEmpleado = Nombre;
                }
                ConsultaCombosGeneral();
                return View();
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return View();
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult SolicitudPermiso(SolicitudPermisoViewModel model)
        {
            try
            {
                //var errors = ModelState
                //.Where(x => x.Value.Errors.Count > 0)
                //.Select(x => new { x.Key, x.Value.Errors })
                //.ToArray();

                string psMensajeValidarFecha = string.Empty;
                psMensajeValidarFecha = ValidarFechas(model);
                if(!string.IsNullOrEmpty(psMensajeValidarFecha))
                {
                    int piSupervisor = ValidarRolSupervisor();
                    if (piSupervisor > 0)
                        ViewBag.Supervisor = piSupervisor;
                    ConsultaCombosGeneral();
                    ModelState.AddModelError("CustomError", psMensajeValidarFecha);
                    return View(model);
                }         

                if (ModelState.IsValid)
                {
                    SOLICITUD_PERMISO solicitudPermiso = new SOLICITUD_PERMISO();
                    clsDSolicitudPermiso = new clsDSolicitudPermiso();
                    clsDEmpleado = new clsDEmpleado();
                    string[] psIdUsuario = User.Identity.Name.Split('_');
                    var poEmpleado = clsDEmpleado.ConsultaEmpleado(psIdUsuario[1]).FirstOrDefault();
                    solicitudPermiso.CodigoLinea = poEmpleado.CODIGOLINEA;
                    solicitudPermiso.CodigoArea = poEmpleado.CODIGOAREA;
                    solicitudPermiso.CodigoCargo = poEmpleado.CODIGOCARGO;
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
                    solicitudPermiso.EstadoSolicitud = clsAtributos.EstadoSolicitudPendiente;
                    solicitudPermiso.FechaBiometrico = DateTime.Now;
                    solicitudPermiso.Origen = clsAtributos.SolicitudOrigenGeneral;
                    solicitudPermiso.CodigoDiagnostico = "";
                    solicitudPermiso.CodigoClasificador = 0;
                    solicitudPermiso.EstadoRegistro = clsAtributos.EstadoRegistroActivo;                    
                    solicitudPermiso.Nivel = clsDSolicitudPermiso.ConsultarNivelUsuario(psIdUsuario[1] + "");
                    solicitudPermiso.UsuarioIngresoLog = psIdUsuario[0];
                    solicitudPermiso.FechaIngresoLog = DateTime.Now;
                    solicitudPermiso.TerminalIngresoLog = Request.UserHostAddress;



                    string psRespuesta = clsDSolicitudPermiso.GuargarModificarSolicitud(solicitudPermiso);
                    SetSuccessMessage(string.Format(psRespuesta));
                }
                else
                {
                    ConsultaCombosGeneral();
                    return View(model);
                }

                return RedirectToAction("SolicitudPermiso");
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return RedirectToAction("SolicitudPermiso");
            }
        }

        [Authorize]
        public ActionResult ConsultarGrupoEnfermedades()
        {
            clsDSolicitudPermisoMedico pSPermisoMedico = new clsDSolicitudPermisoMedico();
            var ListGrupoEnfermedades = pSPermisoMedico.ConsultaGrupoEnfermedades("G","","");
            return PartialView(ListGrupoEnfermedades);
        }

        [Authorize]
        public ActionResult SolicitudPermisoDispensario()
        {
            try
            {
                ConsultaCombosMedicos();
                return View();
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return RedirectToAction("Home","Home");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SolicitudPermisoDispensario(SolicitudPermisoViewModel model)
        {
            try
            {
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

                string psMensajeValidarFecha = string.Empty;
                psMensajeValidarFecha = ValidarFechas(model);
                if (!string.IsNullOrEmpty(psMensajeValidarFecha))
                {
                    ConsultaCombosMedicos();
                    ModelState.AddModelError("CustomError", psMensajeValidarFecha);
                    return View(model);
                }

                if (ModelState.IsValid)
                {
                    SOLICITUD_PERMISO solicitudPermiso = new SOLICITUD_PERMISO();
                    clsDSolicitudPermiso = new clsDSolicitudPermiso();
                    var poEmpleado = clsDEmpleado.ConsultaEmpleado(model.Identificacion).FirstOrDefault();
                    solicitudPermiso.CodigoLinea = poEmpleado.CODIGOLINEA;
                    solicitudPermiso.CodigoArea = poEmpleado.CODIGOAREA;
                    solicitudPermiso.CodigoCargo = poEmpleado.CODIGOCARGO;
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
                    solicitudPermiso.FechaBiometrico = DateTime.Now;
                    solicitudPermiso.Origen = clsAtributos.SolicitudOrigenMedico;
                    solicitudPermiso.CodigoDiagnostico = model.CodigoDiagnostico;
                    solicitudPermiso.CodigoClasificador = int.Parse(model.CodigoClasificador);
                    solicitudPermiso.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    string[] psIdUsuario = User.Identity.Name.Split('_');
                    solicitudPermiso.Nivel = clsDSolicitudPermiso.ConsultarNivelUsuario(psIdUsuario[1] + "");

                    solicitudPermiso.FechaIngresoLog = DateTime.Now;
                    solicitudPermiso.UsuarioIngresoLog = psIdUsuario[0] + "";
                    solicitudPermiso.TerminalIngresoLog = Request.UserHostAddress;
                    string psRespuesta = clsDSolicitudPermiso.GuargarModificarSolicitud(solicitudPermiso);
                    SetSuccessMessage(string.Format(psRespuesta));
                }
                else
                {
                    ConsultaCombosMedicos();
                    return View(model);
                }

                return RedirectToAction("SolicitudPermisoDispensario");
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return RedirectToAction("SolicitudPermisoDispensario");
            }
        }
        #endregion 

        #region REPORTE DE SOLICITUD PERMISO
            
      
        [Authorize]
        public ActionResult ReporteSolicitud()
        {
            return View();
        }
        #endregion

        #region BITACORA SOLICITUD
        [Authorize]
        public ActionResult BitacoraSolicitud()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return View();

            }
        }

        [Authorize]
        public ActionResult BitacoraSolicitudPartial(string dsIdSolicitud, string dsCedula, DateTime? ddFechaDesde, DateTime? ddFechaHasta)
        {
            try
            {
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                if (string.IsNullOrEmpty(dsIdSolicitud) && string.IsNullOrEmpty(dsCedula))
                {
                    return Json(new { Failed = true, Mensaje = "Ingrese parametros de consulta" }, JsonRequestBehavior.AllowGet);
                }
                List<BitacoraSolicitud> model = clsDSolicitudPermiso.ConsultaBitacoraSolicitud(dsIdSolicitud, dsCedula, ddFechaDesde, ddFechaHasta);
                return PartialView(model);

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return Json(new { Failed = true, Mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region CONSULTAS

        public ActionResult ConsultaSolicitudes()
        {
            clsDSolicitudPermiso poSolicitudPermiso = new clsDSolicitudPermiso();
            var pListSolicitudPermiso = poSolicitudPermiso.ConsultaSolicitudesPermisoReporte(clsAtributos.EstadoSolicitudTodos);
            return PartialView(pListSolicitudPermiso);
        }
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


        public int ValidarRolSupervisor()
        {
            clsDLogin = new clsDLogin();
            string[] psIdUsuario = User.Identity.Name.Split('_');
            List<int?> roles = clsDLogin.ConsultaRolesUsuario(psIdUsuario[1]);
            int piSupervisor=0;
            if (roles.Any())
            {
                piSupervisor = roles.FirstOrDefault(x => x.Value == clsAtributos.RolSupervisro) ?? 0;
            }
            return piSupervisor;
        }

        public void ConsultaCombosGeneral()
        {
            clsDClasificador = new clsDClasificador();
            clsDSolicitudPermiso = new clsDSolicitudPermiso();
            clsDGeneral = new clsDGeneral();
            ViewBag.MotivosPermiso = clsDSolicitudPermiso.ConsultarMotivos("G");
            ViewBag.Lineas = clsDGeneral.ConsultaLineas();
            ViewBag.Areas = clsDGeneral.ConsultaAreas("0");
            ViewBag.Cargos = clsDGeneral.ConsultaCargos("0");
        }
        public void ConsultaCombosMedicos()
        {
            clsDClasificador = new clsDClasificador();
            clsDSolicitudPermiso = new clsDSolicitudPermiso();
            clsDGeneral = new clsDGeneral();
            clsApiUsuario = new clsApiUsuario();
            ViewBag.ClasificaroMedico = clsDClasificador.ConsultarClasificador("001", 0);
            ViewBag.MotivosPermiso = clsDSolicitudPermiso.ConsultarMotivos("M");
            ViewBag.Lineas = clsDGeneral.ConsultaLineas();
            ViewBag.Areas = clsDGeneral.ConsultaAreas("0");
            ViewBag.NombreMedico = clsApiUsuario.ConsultaListaUsuariosSap().FirstOrDefault(x => x.CEDULA == "1311401135").NOMBRES??"";

            ViewBag.Cargos = clsDGeneral.ConsultaCargos("0");
        }
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
   

        public ActionResult EmpleadoBuscar(string dsLinea, string dsArea, string dsCargo)
        {
            try
            {
                clsDEmpleado = new clsDEmpleado();
                List<spConsutaEmpleadosFiltro> lista = clsDEmpleado.ConsultaEmpleadosFiltro(dsLinea,dsArea,dsCargo);
                return PartialView(lista);

            }catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
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
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
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
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR {
                    Controlador =this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje=ex.Message,
                    Observacion= "Metodo: "+ this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso= DateTime.Now,
                    TerminalIngreso=Request.UserHostAddress,
                    UsuarioIngreso="sistemas"
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
         }

        #endregion

    }
}
