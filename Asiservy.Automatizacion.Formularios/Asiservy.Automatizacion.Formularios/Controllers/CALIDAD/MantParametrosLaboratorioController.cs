using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.Mantenimientos;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity.Validation;
using System.Net;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class MantParametrosLaboratorioController : Controller
    {
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        ClsDParametrosLaboratorio ClsDParametrosLaboratorio { get; set; } = null;
        string[] lsUsuario { get; set; } = null;
        #region MANTENIMIENTO PARAMETROS DE LABORATORIO   
        public ActionResult MantParametrosLaboratorio()
        {
            try
            {
                ViewBag.MascaraInput = "1";
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDClasificador = new clsDClasificador();
                var precoccion =clsDClasificador.ConsultarClasificador(clsAtributos.codPrecoccion);
                ViewBag.CodigoFormulario = precoccion;
                var areaLaboratorio = clsDClasificador.ConsultarClasificador(clsAtributos.codArea);               
                ViewBag.AreaLaboratorio = areaLaboratorio;
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

        public ActionResult MantParametrosLaboratorioPartial()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDClasificador = new clsDClasificador();
                var precoccion = clsDClasificador.ConsultarClasificador(clsAtributos.codPrecoccion);
                ViewBag.CodigoFormulario = precoccion;
                ClsDParametrosLaboratorio = new ClsDParametrosLaboratorio();
                var lista = ClsDParametrosLaboratorio.ConsultarMantenimiento();
                if (lista != null)
                {
                    ViewBag.nose = lista;
                    return PartialView(null);
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

        public ActionResult GuardarModificarMantenimiento(CC_PARAMETROS_LABORATORIO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrWhiteSpace(model.NombreParametro))
                {
                    ClsDParametrosLaboratorio = new ClsDParametrosLaboratorio();
                    model.FechaIngresoLog = DateTime.Now;
                    model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    model.UsuarioIngresoLog = lsUsuario[0];
                    var valor = ClsDParametrosLaboratorio.GuardarModificarMantenimiento(model);
                    if (valor == 0)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else if (valor == 1) return Json("1", JsonRequestBehavior.AllowGet);
                    else if (valor == 2) return Json("2", JsonRequestBehavior.AllowGet);
                    else return Json("3", JsonRequestBehavior.AllowGet);
                }
                else return Json("4", JsonRequestBehavior.AllowGet);
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

        public ActionResult EliminarMantenimiento(CC_PARAMETROS_LABORATORIO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDParametrosLaboratorio = new ClsDParametrosLaboratorio();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor = ClsDParametrosLaboratorio.EliminarMantenimiento(model);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 1) return Json("1", JsonRequestBehavior.AllowGet);
                else return Json("2", JsonRequestBehavior.AllowGet);//EXISTRE REGISTRO ACTIVO CON EL MISMO NOMBRE
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