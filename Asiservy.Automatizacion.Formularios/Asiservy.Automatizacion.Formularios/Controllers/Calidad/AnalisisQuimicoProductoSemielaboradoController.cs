using Asiservy.Automatizacion.Formularios.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class AnalisisQuimicoProductoSemielaboradoController : Controller
    {
        // GET: AnalisisQuimicoProductoSemielaborado
        string[] lsUsuario { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        [Authorize]
        public ActionResult ControlAnalisisQuimicoProductoSemielaborado()
        {
            try
            {
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.JqueryRotate = "1";
                ViewBag.MaskedInput = "1";
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                
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
    }
}