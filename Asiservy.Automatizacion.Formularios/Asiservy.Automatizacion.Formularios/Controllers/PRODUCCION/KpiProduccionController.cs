using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.PRODUCCION.KpiEnvaseLata;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers.PRODUCCION
{
    public class KpiProduccionController : Controller
    {

        public ClsdKpiProduccion ClsdKpiProduccion { get; set; } = null;
        public clsDClasificador ClsDClasificador { get; set; } = null;
        public clsDEmpleado clsDEmpleado { get; set; } = null;
        public clsDError clsDError { get; set; } = null;
        public string[] lsUsuario { get; set; } = null;

        [Authorize]
        public ActionResult KpiEnvaseLata()
        {
            try
            {
                ViewBag.JavaScrip = "PRODUCCION/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                ViewBag.Apexcharts = "1";
                ViewBag.DateRangePicker = "1";
                ClsDClasificador = new clsDClasificador();
                ViewBag.Turnos = ClsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);
                lsUsuario = User.Identity.Name.Split('_');
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


        public JsonResult ConsultaKpiEnvaseLatas(DateTime FechaDesde, DateTime FechaHasta, string Turno, string Linea)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                lsUsuario = User.Identity.Name.Split('_');
                ClsdKpiProduccion = new ClsdKpiProduccion();
                clsDEmpleado = new clsDEmpleado();
                var model = ClsdKpiProduccion.ConsultaKpiEnvaseLatas(FechaDesde, FechaHasta,Turno, Linea);
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                return Json(model, JsonRequestBehavior.AllowGet);
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