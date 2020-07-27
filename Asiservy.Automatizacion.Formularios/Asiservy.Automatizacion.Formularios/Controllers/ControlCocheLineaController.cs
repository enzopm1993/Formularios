using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ControlCocheLinea;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ProyeccionProgramacion;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ControlCocheLineaController : Controller
    {
        string[] lsUsuario { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        clsDControlCocheLinea clsDControlCocheLinea { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDApiProduccion clsDApiProduccion { get; set; } = null;
        clsDPeriodo clsDPeriodo { get; set; } = null;
        clsDProyeccionProgramacion clsDProyeccionProgramacion { get; set; } = null;

        // GET: ControlCocheLinea
        [Authorize]
        public ActionResult ControlCocheLinea()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDClasificador = new clsDClasificador();
                ViewBag.Lineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador {Grupo= clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.Turno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);

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

      
        [HttpPost]
        public ActionResult ControlCocheLinea(CONTROL_COCHE_LINEA model)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(model.Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                if (ModelState.IsValid)
                {
                    clsDControlCocheLinea = new clsDControlCocheLinea();
                    
                    lsUsuario = User.Identity.Name.Split('_');
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    model.UsuarioIngresoLog = lsUsuario[0];
                    model.FechaIngresoLog = DateTime.Now;
                    model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    string respuesta = clsDControlCocheLinea.GuardarModificarControlCochePorLinea(model);
                    return Json(respuesta, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Faltan parametros", JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult EliminarControl(CONTROL_COCHE_LINEA model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                clsDControlCocheLinea = new clsDControlCocheLinea();
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(model.Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                clsDControlCocheLinea.EliminarControl(model);
                return Json("Registro Eliminado", JsonRequestBehavior.AllowGet);
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


        public ActionResult ControlCocheLineaPartial(DateTime Fecha, string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                clsDControlCocheLinea = new clsDControlCocheLinea();
                var model = clsDControlCocheLinea.ConsultarControlCocheLinea(new Models.ControlCocheLinea.ControlCocheLineaViewModel { Fecha = Fecha,Turno=Turno });
                return PartialView(model);
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

        public JsonResult ConsultaLotes(DateTime Fecha, string Turno)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                var model = clsDProyeccionProgramacion.ConsultaProyeccionProgramacionReporte(Fecha, Turno);
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                return Json(model,JsonRequestBehavior.AllowGet);
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






        [Authorize]
        public ActionResult ReporteControlCocheLinea()
        {
            try
            {

                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDControlCocheLinea = new clsDControlCocheLinea();
                clsDClasificador = new clsDClasificador();
                ViewBag.Turno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);


                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                lsUsuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
                });
                return RedirectToAction("Home", "Home");
            }

        }

      
        public ActionResult ReporteControlCocheLineaPartial(DateTime Fecha, string Turno)
        {
            try
            { 

                clsDControlCocheLinea = new clsDControlCocheLinea();
                clsDClasificador = new clsDClasificador();
                ViewBag.Lineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                var model = clsDControlCocheLinea.ConsultaReporteControlCochePorLinea(Fecha,Turno);
                return PartialView(model);
            }
            catch (Exception ex)
            {

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                lsUsuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize]
        public ActionResult ReporteCochesPorDias()
        {
            try
            {
                ViewBag.Apexcharts = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDClasificador = new clsDClasificador();
                var ListLineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                ViewBag.LineasJson = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ListLineas);

                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                lsUsuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
                });
                return RedirectToAction("Home", "Home");
            }

        }
        [HttpGet]
        public JsonResult ObtenerCochesPorLineaDiario(DateTime fechaIni, DateTime fechaFin)
        {
            
            try
            {
                clsDControlCocheLinea = new clsDControlCocheLinea();

                var respuesta = clsDControlCocheLinea.ConsultarCochesPorLineaDiario(fechaIni, fechaFin);

                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
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