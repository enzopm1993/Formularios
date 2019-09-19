using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Empleado;
using Asiservy.Automatizacion.Formularios.Models.Empleado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class EmpleadoController : Controller
    {

        string[] Usuario = null;
        clsDError clsDError = null;
        clsDEmpleado clsDEmpleado = null;
        clsDEmpleadoEsfero clsDEmpleadoEsfero = null;

        #region EMPLEADO ESFERO
        [Authorize]
        public ActionResult EmpleadoEsfero()
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                if(Empleado != null)
                {
                    var ListaEmpleados = clsDEmpleado.ConsultaEmpleadosFiltro(Empleado.CODIGOLINEA, null, null);
                    ViewBag.Empleados = ListaEmpleados;
                }

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
                    UsuarioIngreso = Usuario[1]
                });
                return RedirectToAction("Home","Home");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult EmpleadoEsfero(EmpleadoEsferoViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDEmpleadoEsfero = new clsDEmpleadoEsfero();
                model.EstadoRegistro = model.EstadoRegistro == "true" ? clsAtributos.EstadoRegistroActivo : clsAtributos.EstadoRegistroInactivo;
                model.UsuarioIngresoLog = Usuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Respuesta = clsDEmpleadoEsfero.GuardarMoficicarEsfero(model);
                return RedirectToAction("EmpleadoEsfero");
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
                    UsuarioIngreso = Usuario[1]
                });
                return RedirectToAction("Home", "Home");
            }
        }

        [Authorize]
        public ActionResult EmpleadoEsferoPartial()
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDEmpleadoEsfero = new clsDEmpleadoEsfero();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                List<EmpleadoEsferoViewModel> ListaEmpleadoEsfero = new List<EmpleadoEsferoViewModel>();
                if (Empleado != null)
                {
                    ListaEmpleadoEsfero = clsDEmpleadoEsfero.ConsultaEmpleadoEsfero(Empleado.CODIGOLINEA);
                 
                }

                return PartialView(ListaEmpleadoEsfero);
            }
            catch (Exception ex)
            {

                // SetErrorMessage(ex.Message);
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
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }
        }


        #endregion


        #region EMPLEADO TURNO
        // GET: Empleado
        [Authorize]
        public ActionResult EmpleadoTurno()
        {            
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                List<EmpleadoViewModel> model = new List<EmpleadoViewModel>();
                ViewBag.Linea = Empleado.LINEA;
                if (Empleado != null)
                {
                     model = clsDEmpleado.ConsultaEmpleadoTurno(Empleado.CODIGOLINEA);
                }
                return View(model);
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
                    UsuarioIngreso = Usuario[1]
                });
                return View();
            }
           
        }

        [Authorize]
        [HttpPost]
        public ActionResult EmpleadoTurno(List<EmpleadoViewModel> model)
        {
            try
            {
                if (!ModelState.IsValid) { return View(model); }

                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                //var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                // List<EmpleadoViewModel> model = new List<EmpleadoViewModel>();
                foreach(var x in model)
                {
                    x.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    x.FechaIngreso = DateTime.Now;
                    x.TerminalIngreso = Request.UserHostAddress;
                    x.UsuarioIngreso = Usuario[0];
                    clsDEmpleado.GuardarModificarEmpleadoTurno(x);
                }
                SetSuccessMessage(clsAtributos.MsjRegistroGuardado);
                return RedirectToAction("EmpleadoTurno");
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
                    UsuarioIngreso = Usuario[1]
                });
                return RedirectToAction("EmpleadoTurno");
            }

        }

        public ActionResult ReporteEmpleadoTurno()
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
                    UsuarioIngreso = Usuario[1]
                });
                return RedirectToAction("Home","Home");
            }
        }

        public ActionResult ReporteEmpleadoTurnoPartial(string dsTurno)
        {
            try
            {
                clsDEmpleado = new clsDEmpleado();
                Usuario = User.Identity.Name.Split('_');
                List<spConsutaReporteEmpleadosTurnos> model = new List<spConsutaReporteEmpleadosTurnos>();
                var empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                if (empleado != null)
                {
                    model = clsDEmpleado.ConsultaReporteEmpleadoTurno(empleado.CODIGOLINEA,dsTurno);
                }

                return PartialView(model);
            }
            catch (Exception ex)
            {

                //SetErrorMessage(ex.Message);
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
                    UsuarioIngreso = Usuario[1]
                });
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
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