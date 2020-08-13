using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.BLZ;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MonitoreoDescongelado;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class MonitoreoDescongeladoController : Controller
    {
        string[] lsUsuario { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        clsDMonitoreoDescongelado clsDMonitoreoDescongelado { get; set; } = null;
        clsDApiProduccion clsDApiProduccion { get; set; } = null;
        clsDReporte clsDReporte { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDPeriodo clsDPeriodo { get; set; } = null;
        clsDLogin clsDLogin { get; set; } = null;
        ClsdMantenimientoMuestraDescongelado ClsdMantenimientoMuestraDescongelado { get; set; } = null;
        ClsdMantenimientoTipoDescongelado ClsdMantenimientoTipoDescongelado { get; set; } = null;
        #region Control
        // GET: MonitoreoDescongelado
        [Authorize]
        public ActionResult MonitoreoDescongelado()
        {
            try
            {
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.DateTimePicker = "1";
                ViewBag.MascaraInput = "1";
                lsUsuario = User.Identity.Name.Split('_');
                clsDClasificador = new clsDClasificador();
                ClsdMantenimientoTipoDescongelado = new ClsdMantenimientoTipoDescongelado();
                ClsdMantenimientoMuestraDescongelado = new ClsdMantenimientoMuestraDescongelado();
                ViewBag.Muestra = ClsdMantenimientoMuestraDescongelado.ConsultaManteminetoMuestraDescongelado().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                ViewBag.Tipo = ClsdMantenimientoTipoDescongelado.ConsultaManteminetoTipoDescongelado().Where(x=> x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();
                ViewBag.Turno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);
                clsDLogin = new clsDLogin();
                if (!string.IsNullOrEmpty(lsUsuario[1]))
                {
                    var usuarioOpcion = clsDLogin.ValidarPermisoOpcion(lsUsuario[1], "ReporteMonitoreoDescongelado");
                    if (usuarioOpcion)
                    {
                        ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ReporteMonitoreoDescongelado";
                    }
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
        public ActionResult MonitoreoDescongeladoPartial(DateTime Fecha,DateTime FechaProduccion ,  string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDApiProduccion = new clsDApiProduccion();
                clsDMonitoreoDescongelado = new clsDMonitoreoDescongelado();
                ClsdMantenimientoTipoDescongelado = new ClsdMantenimientoTipoDescongelado();
                ViewBag.Tipo = ClsdMantenimientoTipoDescongelado.ConsultaManteminetoTipoDescongelado();

                var Control = clsDMonitoreoDescongelado.ConsultaMonitoreoDescongeladoControl(Fecha);
                if (Control != null)
                {
                    ViewBag.Observacion = Control.Observacion;
                }
                List<spConsultaMonitoreoDescongelado> Lista = clsDMonitoreoDescongelado.ConsultaMonitoreoDescongelado(Fecha).ToList();
                ViewBag.Control = Lista;
                List<RegistroDescongeladoEmparrilladoMP> model = clsDApiProduccion.ConsultaControlDescongeladoEmparrilladoMP(FechaProduccion);
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                return PartialView(model);
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


        public JsonResult MonitoreoDescongeladoDetallePartial(DateTime Fecha,string Tanque, string Lote, int Tipo)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDMonitoreoDescongelado = new clsDMonitoreoDescongelado();
               var model = clsDMonitoreoDescongelado.ConsultaMonitoreoDescongelado(Fecha,Tanque,Lote,Tipo);
                return model != null ? Json(model, JsonRequestBehavior.AllowGet) : Json("0",JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult MonitoreoDescongelado(CC_MONITOREO_DESCONGELADO control,List<CC_MONITOREO_DESCONGELADO_DETALLE> detalle)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDMonitoreoDescongelado = new clsDMonitoreoDescongelado();
                control.UsuarioIngresoLog = lsUsuario[0];
                control.FechaIngresoLog = DateTime.Now;
                control.TerminalIngresoLog = Request.UserHostAddress;
                control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(control.Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                var Control = clsDMonitoreoDescongelado.ConsultaMonitoreoDescongeladoControl(control.Fecha);
                if (Control!= null && Control.EstadoReporte)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }

                clsDMonitoreoDescongelado.GuardarModificarMonitoreoDescongelado(control,detalle);
                return Json("Registro Exitoso", JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult GuardarObservacion(CC_MONITOREO_DESCONGELADO_CONTROL control)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDMonitoreoDescongelado = new clsDMonitoreoDescongelado();
                control.UsuarioIngresoLog = lsUsuario[0];
                control.FechaIngresoLog = DateTime.Now;
                control.TerminalIngresoLog = Request.UserHostAddress;
                control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(control.Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                var Control = clsDMonitoreoDescongelado.ConsultaMonitoreoDescongeladoControl(control.Fecha);
                if (Control != null && Control.EstadoReporte)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                clsDMonitoreoDescongelado.GuardarObservacion(control);
                return Json("Registro Exitoso", JsonRequestBehavior.AllowGet);
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


        public JsonResult ValidaEstadoReporte(DateTime Fecha,string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDMonitoreoDescongelado = new clsDMonitoreoDescongelado();
                var control = clsDMonitoreoDescongelado.ConsultaMonitoreoDescongeladoControl(Fecha);
                if (control!=null)
                {
                    if (control.EstadoReporte)
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                       return Json(2, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult EliminarMonitoreoDescongelado(CC_MONITOREO_DESCONGELADO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model.IdMonitoreoDescongelado == 0)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                clsDMonitoreoDescongelado = new clsDMonitoreoDescongelado();
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(model.Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }

                var Control = clsDMonitoreoDescongelado.ConsultaMonitoreoDescongeladoControl(model.Fecha);
                if (Control != null && Control.EstadoReporte)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                clsDMonitoreoDescongelado.EliminarMonitoreoDescongelado(model);
                return Json("Registro Eliminado", JsonRequestBehavior.AllowGet);
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


        [HttpPost]
        public ActionResult EliminarMonitoreoDescongeladoControl(CC_MONITOREO_DESCONGELADO_CONTROL model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }               
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                clsDMonitoreoDescongelado = new clsDMonitoreoDescongelado();
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(model.Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }

                var Control = clsDMonitoreoDescongelado.ConsultaMonitoreoDescongeladoControl(model.Fecha);
                if (Control != null && Control.EstadoReporte)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                clsDMonitoreoDescongelado.EliminarMonitoreoDescongeladoControl(model);
                return Json("Registro Eliminado", JsonRequestBehavior.AllowGet);
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

        #region BANDEJA DE APORBACION
        //-----------------------------------------------------VISTA DE BANDEJA DE APROBACION----------------------------------------------------------------
        [Authorize]
        public ActionResult BandejaMonitoreoDescongelado()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
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

        public ActionResult BandejaMonitoreoDescongeladoPartial(DateTime? FechaDesde, DateTime? FechaHasta, bool Estado = false)
        {
            try
            {
                clsDMonitoreoDescongelado = new clsDMonitoreoDescongelado();
                List<CC_MONITOREO_DESCONGELADO_CONTROL> poControl = null;
                if (FechaDesde != null && FechaHasta != null)
                {
                    poControl = clsDMonitoreoDescongelado.ConsultaMonitoreoDescongeladoControl(FechaDesde.Value, FechaHasta.Value, Estado);
                }
                else
                {
                    poControl = clsDMonitoreoDescongelado.ConsultaMonitoreoDescongeladoControlPendiente();

                }
                if (poControl != null && poControl.Any())
                {
                    clsDClasificador = new clsDClasificador();
                    ViewBag.Turno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);
                    return PartialView(poControl);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
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
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult BandejaAprobarMonitoreoDescongelado(DateTime fecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDMonitoreoDescongelado = new clsDMonitoreoDescongelado();
                var poControl = clsDMonitoreoDescongelado.ConsultaMonitoreoDescongelado(fecha);
                if (poControl != null && poControl.Any())
                {
                    return Json(poControl, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
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

        public ActionResult AprobarBandejaControlCloro(CC_MONITOREO_DESCONGELADO_CONTROL model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDMonitoreoDescongelado = new clsDMonitoreoDescongelado();
                model.FechaAprobacion = DateTime.Now;
                model.AprobadoPor = lsUsuario[0];
                model.EstadoReporte = clsAtributos.EstadoReporteActivo;

                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(model.Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                clsDMonitoreoDescongelado.Aprobar_ReporteMonitoreoDescongelado(model);
                return Json("Aprobación Exitosa", JsonRequestBehavior.AllowGet);
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

        public ActionResult ReversarBandejaControl(CC_MONITOREO_DESCONGELADO_CONTROL model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDMonitoreoDescongelado = new clsDMonitoreoDescongelado();
                model.FechaAprobacion = null;
                model.AprobadoPor = null;
                model.EstadoReporte = clsAtributos.EstadoReportePendiente;

                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(model.Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                clsDMonitoreoDescongelado.Aprobar_ReporteMonitoreoDescongelado(model);
                return Json("Reporte reversado exitosamente", JsonRequestBehavior.AllowGet);
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


        #region REPORTE 
        [Authorize]
        public ActionResult ReporteMonitoreoDescongelado()
        {
            try
            {
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                clsDReporte = new clsDReporte();
                clsDLogin = new clsDLogin();
                lsUsuario = User.Identity.Name.Split('_');
                if (!string.IsNullOrEmpty(lsUsuario[1]))
                {
                    var usuarioOpcion = clsDLogin.ValidarPermisoOpcion(lsUsuario[1], "MonitoreoDescongelado");
                    if (usuarioOpcion)
                    {
                        ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "MonitoreoDescongelado";
                    }
                }
                var rep = clsDReporte.ConsultaCodigoReporte(RouteData.Values["action"].ToString());
                if (rep != null)
                {
                    ViewBag.CodigoReporte = rep.Codigo;
                    ViewBag.VersionReporte = rep.UltimaVersion;
                    ViewBag.NombreReporte = rep.Nombre;
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
        public ActionResult ReporteMonitoreoDescongeladoPartial(DateTime Fecha,string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDMonitoreoDescongelado = new clsDMonitoreoDescongelado();
                clsDClasificador = new clsDClasificador();
                ClsdMantenimientoTipoDescongelado = new ClsdMantenimientoTipoDescongelado();
                ClsdMantenimientoMuestraDescongelado = new ClsdMantenimientoMuestraDescongelado();
                ViewBag.Muestra = ClsdMantenimientoMuestraDescongelado.ConsultaManteminetoMuestraDescongelado().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                ViewBag.Tipo = ClsdMantenimientoTipoDescongelado.ConsultaManteminetoTipoDescongelado().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno, Turno).FirstOrDefault();
                if (poTurno != null)
                {
                    ViewBag.Turno = poTurno.Descripcion;
                }
                var model = clsDMonitoreoDescongelado.ConsultaMonitoreoDescongelado(Fecha).ToList();
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                return PartialView(model);
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

        public ActionResult ReporteMonitoreoDescongeladoControlPartial(DateTime FechaDesde, DateTime FechaHasta)
        {
            try
            {
                clsDMonitoreoDescongelado = new clsDMonitoreoDescongelado();
                List<CC_MONITOREO_DESCONGELADO_CONTROL> poControl = null;

                poControl = clsDMonitoreoDescongelado.ConsultaMonitoreoDescongeladoControl(FechaDesde, FechaHasta);


                if (poControl != null && poControl.Any())
                {
                    clsDClasificador = new clsDClasificador();
                    ViewBag.Turnos = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);
                    return PartialView(poControl);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
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
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
    }
}