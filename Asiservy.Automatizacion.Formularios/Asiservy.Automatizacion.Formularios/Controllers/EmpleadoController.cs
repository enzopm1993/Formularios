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
        clsDClasificador clsDClasificador = null;
        clsDLogin clsDLogin = null;
        clsDGeneral clsDGeneral = null;

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
                    UsuarioIngreso = Usuario[0]
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
                ViewBag.dataTableJS = "1";

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
                ViewBag.dataTableJS = "1";

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


#region REPORTES

        [Authorize]
        public ActionResult ReportePersonalNominaPorLinea()
        {
            try
            {
                ViewBag.dataTableJS = "1";

                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                var model = clsDEmpleado.ConsultaPersonalNominaPorLinea();      
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
                return RedirectToAction("Home", "Home");
            }
        }
        [Authorize]
        public ActionResult ReporteDistribucionPorLinea()
        {
            try
            {
                ViewBag.dataTableJS = "1";

                Usuario = User.Identity.Name.Split('_');
                clsDClasificador = new clsDClasificador();
                clsDEmpleado = new clsDEmpleado();
                clsDLogin = new clsDLogin();
                clsDGeneral = new clsDGeneral();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                ViewBag.LineaEmpleado = Empleado.CODIGOLINEA;
                List<int?> roles = clsDLogin.ConsultaRolesUsuario(Usuario[1]);
                if (roles.FirstOrDefault(x => x.Value == clsAtributos.RolSupervisorGeneral|| x.Value == clsAtributos.RolControladorGeneral) != null)
                {
                    ViewBag.SupervisorGeneral = clsAtributos.RolSupervisorGeneral;
                    ViewBag.Lineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                    
                } else if (roles.FirstOrDefault(x => x.Value == clsAtributos.RolSupervisorLinea || x.Value == clsAtributos.RolControladorLinea) != null)
                {
                    ViewBag.Lineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo, Codigo = Empleado.CODIGOLINEA });
                

                }
                else
                {
                    ViewBag.Lineas = clsDGeneral.ConsultaLineas(Empleado.CODIGOLINEA);
                  

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
                    UsuarioIngreso = Usuario[0]
                });
                return RedirectToAction("Home", "Home");
            }
        }
        [Authorize]
        public ActionResult ReporteDistribucionPorLineaPartial(string Linea)
        {
            try
            {
                ViewBag.dataTableJS = "1";

                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                var model = clsDEmpleado.spConsultaDistribucionPorLinea(Linea);
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