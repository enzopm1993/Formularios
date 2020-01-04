using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using System.Data.Entity.Validation;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class MoverPersonalController : Controller
    {
        clsDError clsDError = null;
        clsDGeneral clsDGeneral = null;
        string[] liststring;
        clsDEmpleado clsDEmpleado = null;
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        // GET: MoverPersonal
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult MoverPersonalDataLife()
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.CentroCostos = clsDGeneral.ConsultaCentroCostos();
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
                    UsuarioIngreso = liststring[0]
                });
                return RedirectToAction("Home", "Home");
            }
            return View();
        }
        public ActionResult MoverPersonalDataLifePartial(string psCentroCosto, string psRecurso, string psLinea, string psCargo)
        {
            try
            {
                List<spConsutaEmpleadosFiltroCambioPersonal> ListaEmpleados = new List<spConsutaEmpleadosFiltroCambioPersonal>();
                clsDEmpleado = new clsDEmpleado();
                ListaEmpleados = clsDEmpleado.ConsultaEmpleadosFiltroCambioPersonal(psLinea, psCentroCosto, psCargo, psRecurso, clsAtributos.TipoPrestar);
                return PartialView(ListaEmpleados);
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                liststring = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GuardarMoverPersonal(string Cedula, string CentroCostos, string Recurso, string Linea, string Cargo)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
              
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}