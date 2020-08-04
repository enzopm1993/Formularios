using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ControlCocinamiento;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity.Validation;
using System.Net;
using System;
using System.Linq;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;
using System.Collections.Generic;
using System.Web;
using System.IO;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.Mantenimientos;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class ControlCocinamientoController : Controller
    {
        clsDPeriodo clsDPeriodo { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDLogin clsDLogin { get; set; } = null;
        public clsDReporte ClsDReporte { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        ClsDControlCocinamiento ClsDControlCocinamiento { get; set; } = null;
        clsDApiProduccion clsDApiProduccion { get; set; } = null;
        string[] lsUsuario { get; set; } = null;

        #region CONTROL
        [Authorize]
        public ActionResult ControlCocinamiento()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.JqueryRotate = "1";
                ViewBag.select2 = "1";
                ViewBag.MascaraInput = "1";
                ViewBag.usuarioAnalista = lsUsuario[0];
                clsDLogin = new clsDLogin();
                var usuarioOpcion = clsDLogin.ValidarPermisoOpcion(lsUsuario[1], "ReporteAnalisisQuimico");
                if (usuarioOpcion)
                {
                    ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ReporteAnalisisQuimico";
                }
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
        public ActionResult ControlCocinamientoPartial(DateTime fechaProduccion, DateTime fechaParadaCocina, int idAnalisis = 0, string turno = "1", int op = 1)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ViewBag.Turno = turno;
                ClsDControlCocinamiento = new ClsDControlCocinamiento();
               
                var listaParadasCocinas = clsDApiProduccion.ParadasCocinasPorFecha(fechaParadaCocina);
                if (listaParadasCocinas != null)
                {
                    return PartialView(listaParadasCocinas);
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
        //public JsonResult ConsultarDetalle(int idAnalisis)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (!User.Identity.IsAuthenticated)
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }
        //        ClsDControlCocinamiento = new ClsDControlCocinamiento();
        //        var listaDetalle = ClsDControlCocinamiento.GuardarModificarCocinamiento(null);
        //        if (listaDetalle.Any())
        //        {
        //            return Json(listaDetalle, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("0", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public ActionResult ControlElementoPartial(int idAnalisis, int idParametro = 0, int idAnalisisDetalle = 0)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (!User.Identity.IsAuthenticated)
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }
        //        ClsDParametrosLaboratorio = new ClsDParametrosLaboratorio();
        //        var parametros = ClsDParametrosLaboratorio.ConsultarParametrosFormularios(clsAtributos.codFormPrecoccion).Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
        //        ViewBag.listaParametros = parametros;
        //        ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
        //        var listaDetalle = ClsDLaboratorioAnalisisQuimico.ConsultarElemento(idAnalisis, idParametro, idAnalisisDetalle);

        //        if (listaDetalle.Any())
        //        {
        //            ViewBag.ListaElementos = listaDetalle;
        //            return PartialView(null);
        //        }
        //        else
        //        {
        //            return Json(0, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //}
        [HttpPost]
        public JsonResult GuardarModificarCocinamiento(CC_COCINAMIENTO_CTRL model, int siAprobar)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(model.FechaProduccion);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                ClsDControlCocinamiento = new ClsDControlCocinamiento();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                if (model.IdCocinamientoCtrl != 0 && siAprobar == 0)
                {
                    var estadoReporte = ClsDControlCocinamiento.ConsultarEstadoReporte(model.IdCocinamientoCtrl, DateTime.MinValue);
                    if (estadoReporte.EstadoReporte)
                    {
                        return Json("4", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                    }
                }
                var valor = ClsDControlCocinamiento.GuardarModificarCocinamiento(model, siAprobar);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 1)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 2) { return Json("2", JsonRequestBehavior.AllowGet); }
                else if (valor == 3) return Json("3", JsonRequestBehavior.AllowGet);//ERROR DE FECHA
                else return Json("5", JsonRequestBehavior.AllowGet);
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