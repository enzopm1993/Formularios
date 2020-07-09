using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.CloroCisternaDescongelado;
using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ParametroCalidad;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class CloroCisternaDescongeladoController : Controller
    {
        ClsdParametroCalidad ClsdParametroCalidad { get; set; } = null;
        clsDPeriodo clsDPeriodo { get; set; } = null;
        clsDLogin clsLogin { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDCloroCisternaDescongelado clsDCloroCisternaDescongelado { get; set; } = null;
        string[] lsUsuario { get; set; }=null;
        public clsDReporte ClsDReporte { get; set; } = null;
        //-----------------------------------------------------VISTA DE INGRESO DE DATOS----------------------------------------------------------------
        public ActionResult CloroCisternaDescongelado()
        {
            try
            {              
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = "CALIDAD/"+RouteData.Values["controller"] + "/" + RouteData.Values["action"];                ViewBag.MascaraInput = "1";
                clsLogin = new clsDLogin();
                lsUsuario = User.Identity.Name.Split('_');
                var usuarioOpcion = clsLogin.ValidarPermisoOpcion(lsUsuario[1], "ReporteCloroCisternaDescongelado");
                if (usuarioOpcion)
                {
                    ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ReporteCloroCisternaDescongelado";
                }
                ClsdParametroCalidad = new ClsdParametroCalidad();
                var parametros = ClsdParametroCalidad.ConsultaManteminetoParametroCalidad("");
                ViewBag.ColorDentroRango = parametros.ColorDentroRango;
                ViewBag.ColorFueraRango = parametros.ColorFueraRango;
                
                clsDClasificador = new clsDClasificador();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();
                if (poTurno != null)
                {
                    ViewBag.Turno = poTurno;
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

        [HttpPost]
        public ActionResult EliminarCloroCisternaDescongelado(CC_CLORO_CISTERNA_DESCONGELADO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(model.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                clsDCloroCisternaDescongelado = new clsDCloroCisternaDescongelado();
                var estadoReporte = clsDCloroCisternaDescongelado.ConsultarEstadoReporte(model.IdCloroCisterna);
                if (!estadoReporte.EstadoReporte)
                {
                    var poCloroCisterna = clsDCloroCisternaDescongelado.Eliminar_ReporteCloroCisternaDescongelado(model);
                    if (poCloroCisterna == 1)
                    {
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                }
                else return Json("2", JsonRequestBehavior.AllowGet);


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

        public ActionResult ValidarCloroCisternaDescongelado(DateTime fecha, string turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDCloroCisternaDescongelado = new clsDCloroCisternaDescongelado();
                var poCloroCisterna=clsDCloroCisternaDescongelado.ConsultarCabeceraTurno(turno, fecha);
                if (poCloroCisterna != null)
                {
                    return Json(poCloroCisterna, JsonRequestBehavior.AllowGet);
                }
                else {
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

        [HttpPost]
        public ActionResult ControlCloroCisternaDescongelado(CC_CLORO_CISTERNA_DESCONGELADO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(model.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                clsDCloroCisternaDescongelado = new clsDCloroCisternaDescongelado();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;                
                model.UsuarioIngresoLog = lsUsuario[0];
                var estadoReporte = clsDCloroCisternaDescongelado.ConsultarEstadoReporte(model.IdCloroCisterna);
                if (!estadoReporte.EstadoReporte)
                {
                    ClsdParametroCalidad = new ClsdParametroCalidad();
                    var parametros=ClsdParametroCalidad.ConsultaManteminetoParametroCalidad("");
                    model.ParamMax = parametros.Maximo;
                    model.ParamMin = parametros.Minimo;
                    int result=clsDCloroCisternaDescongelado.GuardarModificar_ReporteCloroCisternaDescongelado(model);
                    if(result==0)return Json("0", JsonRequestBehavior.AllowGet);
                    else return Json("1", JsonRequestBehavior.AllowGet);
                }
                else return Json("2", JsonRequestBehavior.AllowGet);
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

        
        public ActionResult ControlCloroCisternaDescongeladoDetalle(CC_CLORO_CISTERNA_DESCONGELADO_DETALLE model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDCloroCisternaDescongelado = new clsDCloroCisternaDescongelado();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                var estadoReporte = clsDCloroCisternaDescongelado.ConsultarEstadoReporte(model.IdCloroCisternaCabecera);
                clsDPeriodo = new clsDPeriodo();
                if (estadoReporte!=null && !estadoReporte.EstadoReporte)
                {
                    bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.Fecha);
                    if (!periodo)
                    {
                        return Json("100", JsonRequestBehavior.AllowGet);
                    }
                //}
                
                
                //if (!estadoReporte.EstadoReporte)
                //{
                    clsDCloroCisternaDescongelado.GuardarModificar_ReporteCloroCisternaDescongeladoDetalle(model);
                    return Json("Registro Exitoso", JsonRequestBehavior.AllowGet);
                }else return Json("2", JsonRequestBehavior.AllowGet);
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

        public ActionResult ValidarCloroCisternaDescongeladoDetallePartial(DateTime fecha, int IdCloroCisterna)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDCloroCisternaDescongelado = new clsDCloroCisternaDescongelado();
                var poCloroCisterna = clsDCloroCisternaDescongelado.ConsultarDetalle(IdCloroCisterna);
                ClsdParametroCalidad = new ClsdParametroCalidad();
                var parametros = ClsdParametroCalidad.ConsultaManteminetoParametroCalidad(clsAtributos.CC_CodParametroCloroCisterna);
                ViewBag.ColorDentroRango = parametros.ColorDentroRango;
                ViewBag.ColorFueraRango = parametros.ColorFueraRango;
                ViewBag.ParamMax = parametros.Maximo;
                ViewBag.ParamMin = parametros.Minimo;
                if (poCloroCisterna != null && poCloroCisterna.Any())
                {
                    return PartialView(poCloroCisterna);
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

        [HttpPost]
        public ActionResult EliminarCloroCisternaDescongeladoDetalle(CC_CLORO_CISTERNA_DESCONGELADO_DETALLE model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDCloroCisternaDescongelado = new clsDCloroCisternaDescongelado();
                var estadoReporte = clsDCloroCisternaDescongelado.ConsultarEstadoReporte(model.IdCloroCisternaCabecera);
                if (estadoReporte!=null)
                {
                    clsDPeriodo = new clsDPeriodo();
                    bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.Fecha);
                    if (!periodo)
                    {
                        return Json("100", JsonRequestBehavior.AllowGet);
                    }
                    if (!estadoReporte.EstadoReporte)
                    {
                        var poCloroCisterna = clsDCloroCisternaDescongelado.Eliminar_ReporteCloroCisternaDescongeladoDetalle(model);
                        if (poCloroCisterna == 1)
                        {
                            return Json("1", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("0", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else return Json("2", JsonRequestBehavior.AllowGet);
                }
                return Json("10", JsonRequestBehavior.AllowGet);

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

        //-----------------------------------------------------VISTA DE BANDEJA DE APROBACION----------------------------------------------------------------
        public ActionResult BandejaCloroCisternaDescongelado()
        {
            try
            {
                ClsdParametroCalidad = new ClsdParametroCalidad();
                var parametros = ClsdParametroCalidad.ConsultaManteminetoParametroCalidad("");
                ViewBag.ColorDentroRango = parametros.ColorDentroRango;
                ViewBag.ColorFueraRango = parametros.ColorFueraRango;
                ViewBag.ParamMax = parametros.Maximo;
                ViewBag.ParamMin = parametros.Minimo;
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

        public ActionResult BandejaCloroCisternaDescongeladoPartial(DateTime fechaDesde, DateTime fechaHasta, bool estadoReporte)
        {           
            try
            {
                clsDCloroCisternaDescongelado = new clsDCloroCisternaDescongelado();
                clsDClasificador = new clsDClasificador();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();
                ViewBag.Turno = poTurno;
                var poCloroCisterna = clsDCloroCisternaDescongelado.ConsultarBadejaEstado(fechaDesde, fechaHasta, estadoReporte);                
                if (poCloroCisterna != null && poCloroCisterna.Any())
                {
                    return PartialView(poCloroCisterna);
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

        public ActionResult BandejaAprobarCloroCisternaDescongelado(int idCloroCisterna)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDCloroCisternaDescongelado = new clsDCloroCisternaDescongelado();
                var poCloroCisterna = clsDCloroCisternaDescongelado.ConsultarDetalle(idCloroCisterna);
                if (poCloroCisterna != null && poCloroCisterna.Any())
                {
                    return Json(poCloroCisterna, JsonRequestBehavior.AllowGet);
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

        public ActionResult AprobarBandejaControlCloro(CC_CLORO_CISTERNA_DESCONGELADO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(model.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                clsDCloroCisternaDescongelado = new clsDCloroCisternaDescongelado();
                model.AprobadoPor= lsUsuario[0];
                model.UsuarioIngresoLog = lsUsuario[0];
                clsDCloroCisternaDescongelado.Aprobar_ReporteCloroCisternaDescongelado(model);
                return Json("Cambio de ESTADO realizado  exitosamente", JsonRequestBehavior.AllowGet);
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
        //-----------------------------------------------------VISTA DE REPORTE----------------------------------------------------------------

        public ActionResult ReporteCloroCisternaDescongelado()
        {
            try
            {
                ClsdParametroCalidad = new ClsdParametroCalidad();
                var parametros = ClsdParametroCalidad.ConsultaManteminetoParametroCalidad(clsAtributos.CC_CodParametroCloroCisterna);
                ViewBag.ColorDentroRango = parametros.ColorDentroRango;
                ViewBag.ColorFueraRango = parametros.ColorFueraRango;
                ViewBag.ParamMax = parametros.Maximo;
                ViewBag.ParamMin = parametros.Minimo;
                ViewBag.DateRangePicker = "1";
                ViewBag.dataTableJS = "1";
                clsLogin = new clsDLogin();
                lsUsuario = User.Identity.Name.Split('_');
                var usuarioOpcion = clsLogin.ValidarPermisoOpcion(lsUsuario[1], "CloroCisternaDescongelado");
                if (usuarioOpcion)
                {
                    ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "CloroCisternaDescongelado";
                }
                ClsDReporte = new clsDReporte();
                var rep = ClsDReporte.ConsultaCodigoReporte(RouteData.Values["action"].ToString());
                if (rep != null)
                {
                    ViewBag.CodigoReporte = rep.Codigo;
                    ViewBag.VersionReporte = rep.UltimaVersion;
                    ViewBag.NombreReporte = rep.Nombre;
                }
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

        public ActionResult ReporteCloroCisternaDescongeladoPartial(DateTime fechaDesde, DateTime fechaHasta, int idCloroCisterna, int op)
        {
            try
            {
                ClsdParametroCalidad = new ClsdParametroCalidad();
                var parametros = ClsdParametroCalidad.ConsultaManteminetoParametroCalidad(clsAtributos.CC_CodParametroCloroCisterna);
                ViewBag.ColorDentroRango = parametros.ColorDentroRango;
                ViewBag.ColorFueraRango = parametros.ColorFueraRango;
                ViewBag.ParamMax = parametros.Maximo;
                ViewBag.ParamMin = parametros.Minimo;
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }                
                clsDCloroCisternaDescongelado = new clsDCloroCisternaDescongelado();
                var poCloroCisterna = clsDCloroCisternaDescongelado.ConsultarCloroCisternaRangoFecha(fechaDesde, fechaHasta, idCloroCisterna, op);
                if (poCloroCisterna.Count!=0)
                {
                    return PartialView(poCloroCisterna);
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

        public ActionResult ReporteCloroCisternaDescongeladoCabeceraPartial(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDClasificador = new clsDClasificador();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();
                ViewBag.Turno = poTurno;
                clsDCloroCisternaDescongelado = new clsDCloroCisternaDescongelado();
                var poCloroCisterna = clsDCloroCisternaDescongelado.ReporteConsultarcabecera(fechaDesde, fechaHasta);
                if (poCloroCisterna != null)
                {
                    return PartialView(poCloroCisterna);
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

        public JsonResult ConsultarEstadoReporte(long idControlCloro)
        {
            try
            {
                clsDCloroCisternaDescongelado = new clsDCloroCisternaDescongelado();
                var poCloroCisterna = clsDCloroCisternaDescongelado.ConsultarEstadoReporte(idControlCloro);
                if (poCloroCisterna != null)
                {
                    return Json(poCloroCisterna, JsonRequestBehavior.AllowGet);
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
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
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