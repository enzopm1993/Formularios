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
        clsDControlHueso clsDControlHueso = null;

        #region EMPLEADO ESFERO

        [Authorize]
        public ActionResult ControlEsfero()
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();               
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                ViewBag.Linea = Empleado.LINEA;

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
                return RedirectToAction("Home", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult ControlEsfero(spConsutaControlEsferos model,string dsTipo)
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleadoEsfero = new clsDEmpleadoEsfero();
              
                var respuesta = clsDEmpleadoEsfero.GuardarModificarControlEsfero(new CONTROL_ESFERO
                {
                    Cedula = model.Cedula,
                    HoraInicio = model.Hora,
                    UsuarioIngresoLog = Usuario[0],
                    TerminalIngresoLog = Request.UserHostAddress
                },dsTipo);

                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

               // SetErrorMessage(ex.Message);
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

        [Authorize]
        public ActionResult ControlEsferoPartial(string dsTipo)
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDEmpleadoEsfero = new clsDEmpleadoEsfero();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                List<spConsutaControlEsferos> model = new List<spConsutaControlEsferos>();
                if (Empleado != null)
                {
                   clsDEmpleadoEsfero.GenerarControlEmpleadoEsfero(Empleado.CODIGOLINEA, Usuario[0], Request.UserHostAddress);
                    model = clsDEmpleadoEsfero.ConsultaControlEsfero(Empleado.CODIGOLINEA, dsTipo);
                    
                }

                return PartialView(model);
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
                    UsuarioIngreso = Usuario[1]
                });
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }
        }

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
                SetSuccessMessage(Respuesta);
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
                return RedirectToAction("EmpleadoEsfero");
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
                    UsuarioIngreso = Usuario[0]
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

        #region CONTROL HUESOS
        [Authorize]
        public ActionResult ControlHueso()
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                ViewBag.Linea = Empleado.LINEA;
                ViewBag.CodLinea = Empleado.CODIGOLINEA;

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
                return RedirectToAction("Home","Home");
            }

        }

        [Authorize]
        public ActionResult ControlHuesoPartial(int id)
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDControlHueso = new clsDControlHueso();
                var model = clsDControlHueso.ConsultaControlHueso(id);
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
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        public ActionResult GenerarControlHueso(string dsLinea, string dsLote, string Hora)
        {
            try
            {
                if (string.IsNullOrEmpty(dsLinea) || string.IsNullOrEmpty(dsLote) || dsLote == "0" || string.IsNullOrEmpty(Hora))
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json("Parametros Incompletos", JsonRequestBehavior.AllowGet);
                }
                Usuario = User.Identity.Name.Split('_');
                clsDControlHueso = new clsDControlHueso();
                int id = clsDControlHueso.GenerarControlHueso(new CONTROL_HUESO {
                    Linea = dsLinea,
                    Hora = TimeSpan.Parse(Hora),
                    Lote = dsLote,
                    EstadoRegistro=clsAtributos.EstadoRegistroActivo,
                    UsuarioIngresoLog= Usuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                });
               // var listadoLimpiadoras = clsDControlHueso.ConsultaLimpiadorasControlHueso(dsLinea);
              //  clsDControlHueso.GenerarControlHuesoDetalle(listadoLimpiadoras, id, Usuario[0], Request.UserHostAddress);
                return Json(id,JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        public ActionResult ValidaControlHueso(string dsLinea, string Hora)
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDControlHueso = new clsDControlHueso();
                int id = clsDControlHueso.ValidaControlHueso(new CONTROL_HUESO
                {
                    Linea = dsLinea,
                    Hora = TimeSpan.Parse(Hora)    
                });
                // var listadoLimpiadoras = clsDControlHueso.ConsultaLimpiadorasControlHueso(dsLinea);
                //  clsDControlHueso.GenerarControlHuesoDetalle(listadoLimpiadoras, id, Usuario[0], Request.UserHostAddress);
                return Json(id, JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        public ActionResult GuardarControlHueso(CONTROL_HUESO_DETALLE detalle)
        {
            try
            {
                if (detalle== null)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json("Parametros Incompletos", JsonRequestBehavior.AllowGet);
                }

                Usuario = User.Identity.Name.Split('_');
                clsDControlHueso = new clsDControlHueso();
                detalle.UsuarioIngresoLog = Usuario[0];
                detalle.TerminalIngresoLog = Request.UserHostAddress;
                var respuesta = clsDControlHueso.GuardarModificarControlHueso(detalle);
              
                return Json(respuesta, JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = Usuario[0]
                });
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