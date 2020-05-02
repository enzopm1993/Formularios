using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.HigieneComedorCocina;
using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Rotativa;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    //public class HigieneComedorCocinaController : Controller
    //{
    //    clsDError clsDError = null;
    //    clsDEmpleado clsDEmpleado = null;
    //    clsDHigieneComedorCocina clsDHigieneComedorCocina = null;
    //    string[] lsUsuario;

    //    public ActionResult MantHigieneComedorCocina()
    //    {
    //        try
    //        {
    //            ViewBag.dataTableJS = "1";
    //            ViewBag.DateRangePicker = "1";
    //            ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
    //            return View();
    //        }
    //        catch (DbEntityValidationException e)
    //        {
    //            clsDError = new clsDError();
    //            lsUsuario = User.Identity.Name.Split('_');
    //            string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
    //                "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
    //            SetErrorMessage(Mensaje);
    //            return RedirectToAction("Home", "Home");
    //        }
    //        catch (Exception ex)
    //        {
    //            clsDError = new clsDError();
    //            lsUsuario = User.Identity.Name.Split('_');
    //            string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
    //                "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
    //            SetErrorMessage(Mensaje);
    //            return RedirectToAction("Home", "Home");
    //        }
    //    }

    //    public ActionResult MantHigieneComedorCocinaPartial()
    //    {
    //        try
    //        {
    //            lsUsuario = User.Identity.Name.Split('_');
    //            if (string.IsNullOrEmpty(lsUsuario[0]))
    //            {
    //                return Json("101", JsonRequestBehavior.AllowGet);
    //            }
    //            clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
    //            var lista = clsDHigieneComedorCocina.ConsultarAreaAuditoria();
    //            if (lista.Count() != 0)
    //            {                  
    //                return PartialView(lista);
    //            }
    //            else
    //            {
    //                return Json("0", JsonRequestBehavior.AllowGet);
    //            }
    //        }
    //        catch (DbEntityValidationException e)
    //        {
    //            clsDError = new clsDError();
    //            lsUsuario = User.Identity.Name.Split('_');
    //            string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
    //                "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
    //            SetErrorMessage(Mensaje);
    //            return RedirectToAction("Home", "Home");
    //        }
    //        catch (Exception ex)
    //        {
    //            clsDError = new clsDError();
    //            lsUsuario = User.Identity.Name.Split('_');
    //            string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
    //                "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
    //            SetErrorMessage(Mensaje);
    //            return RedirectToAction("Home", "Home");
    //        }
    //    }

    //    public JsonResult GuardarModificarAreaAuditoria(CC_HIGIENE_C_C_MANT_AREA_AUDITORIA model)
    //    {
    //        try
    //        {
    //            lsUsuario = User.Identity.Name.Split('_');
    //            if (string.IsNullOrEmpty(lsUsuario[0]))
    //            {
    //                return Json("101", JsonRequestBehavior.AllowGet);
    //            }
    //            clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
    //            model.FechaIngresoLog = DateTime.Now;
    //            model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
    //            model.TerminalIngresoLog = Request.UserHostAddress;
    //            model.UsuarioIngresoLog = lsUsuario[0];
    //            var valor = clsDHigieneComedorCocina.GuardarModificarAreaAuditoria(model);               
    //            if (valor == 0)
    //            {
    //                return Json("0", JsonRequestBehavior.AllowGet);
    //            }
    //            else return Json("1", JsonRequestBehavior.AllowGet);
    //        }
    //        catch (DbEntityValidationException e)
    //        {
    //            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //            clsDError = new clsDError();
    //            lsUsuario = User.Identity.Name.Split('_');
    //            string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
    //                "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
    //            return Json(Mensaje, JsonRequestBehavior.AllowGet);
    //        }
    //        catch (Exception ex)
    //        {
    //            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //            clsDError = new clsDError();
    //            lsUsuario = User.Identity.Name.Split('_');
    //            string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
    //                "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
    //            return Json(Mensaje, JsonRequestBehavior.AllowGet);
    //        }
    //    }

    //    public JsonResult EliminarAreaAuditoria(CC_HIGIENE_C_C_MANT_AREA_AUDITORIA model)
    //    {
    //        try
    //        {
    //            lsUsuario = User.Identity.Name.Split('_');
    //            if (string.IsNullOrEmpty(lsUsuario[0]))
    //            {
    //                return Json("101", JsonRequestBehavior.AllowGet);
    //            }
    //            clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
    //            model.FechaIngresoLog = DateTime.Now;
    //            model.TerminalIngresoLog = Request.UserHostAddress;
    //            model.UsuarioIngresoLog = lsUsuario[0];
    //            var valor = clsDHigieneComedorCocina.EliminarAreaAuditoria(model);
    //            if (valor == 0)
    //            {
    //                return Json("0", JsonRequestBehavior.AllowGet);
    //            }
    //            else return Json("1", JsonRequestBehavior.AllowGet);
    //        }
    //        catch (DbEntityValidationException e)
    //        {
    //            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //            clsDError = new clsDError();
    //            lsUsuario = User.Identity.Name.Split('_');
    //            string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
    //                "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
    //            return Json(Mensaje, JsonRequestBehavior.AllowGet);
    //        }
    //        catch (Exception ex)
    //        {
    //            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //            clsDError = new clsDError();
    //            lsUsuario = User.Identity.Name.Split('_');
    //            string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
    //                "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
    //            return Json(Mensaje, JsonRequestBehavior.AllowGet);
    //        }
    //    }

    //    protected void SetSuccessMessage(string message)
    //    {
    //        TempData["MensajeConfirmacion"] = message;
    //    }
    //    protected void SetErrorMessage(string message)
    //    {
    //        TempData["MensajeError"] = message;
    //    }
    //}
}