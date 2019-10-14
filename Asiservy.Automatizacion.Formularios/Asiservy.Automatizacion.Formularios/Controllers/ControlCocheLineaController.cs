using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ControlCocheLinea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ControlCocheLineaController : Controller
    {
        string[] Usuario = null;
        clsDError clsDError = null;
        clsDControlCocheLinea clsDControlCocheLinea = null;
        clsDClasificador clsDClasificador = null;

        // GET: ControlCocheLinea
        [Authorize]
        public ActionResult ControlCocheLinea()
        {
            try
            {
                clsDClasificador = new clsDClasificador();
                ViewBag.Lineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador {Grupo= clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return RedirectToAction("Home", "Home");
            }

        }

      
        [HttpPost]
        public ActionResult ControlCocheLinea(CONTROL_COCHE_LINEA model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    clsDControlCocheLinea = new clsDControlCocheLinea();
                    Usuario = User.Identity.Name.Split('_');
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    model.UsuarioIngresoLog = Usuario[0];
                    model.FechaIngresoLog = DateTime.Now;
                    string respuesta = clsDControlCocheLinea.GuardarModificarControlCochePorLinea(model);
                    return Json(respuesta, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Faltan parametros", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }


       
        public ActionResult ControlCocheLineaPartial()
        {
            try
            {
                clsDControlCocheLinea = new clsDControlCocheLinea();
                var model = clsDControlCocheLinea.ConsultarControlCocheLinea(new Models.ControlCocheLinea.ControlCocheLineaViewModel { Fecha = DateTime.Now.Date});
                return PartialView(model);
            }
            catch (Exception ex)
            {

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        public ActionResult ReporteControlCocheLinea()
        {
            try
            {
                    return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return RedirectToAction("Home", "Home");
            }

        }

      
        public ActionResult ReporteControlCocheLineaPartial(DateTime Fecha)
        {
            try
            {
                clsDControlCocheLinea = new clsDControlCocheLinea();
                var model = clsDControlCocheLinea.ConsultaReporteControlCochePorLinea(Fecha);
                return PartialView(model);
            }
            catch (Exception ex)
            {

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
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