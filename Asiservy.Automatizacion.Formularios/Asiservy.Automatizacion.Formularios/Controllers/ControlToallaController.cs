using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ControlToalla;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ControlToallaController : Controller
    {
        clsDError clsDError = null;
        clsDControlToalla ClsDControlToalla=null;
        clsDEmpleado clsDEmpleado = null;
        string[] liststring;
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        // GET: ControlToalla
        [Authorize]
        public ActionResult ControlToalla()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDEmpleado = new clsDEmpleado();
                liststring = User.Identity.Name.Split('_');
                ViewBag.Linea = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault().CODIGOLINEA;
                //ViewBag.Linea = "52";
                return View();
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
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            
        }
        [HttpPost]
        public ActionResult PartialControlToalla(DateTime? Fecha,string Linea, string Turno)
        {
            try
            {
                ClsDControlToalla = new clsDControlToalla();
                List<CONTROL_TOALLA> ListCabCOntrolToalla = ClsDControlToalla.ConsultarCabToalla(Fecha.Value,Linea, Turno);
                return PartialView(ListCabCOntrolToalla);
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
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult PartialDetalleToalla(int IdCabToalla)
        {
            try
            {
                ClsDControlToalla = new clsDControlToalla();
                List<spConsultaDetalleToalla> ListCabCOntrolToalla = ClsDControlToalla.ConsultarDetToalla(IdCabToalla);
                return PartialView(ListCabCOntrolToalla);
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
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GuardarControlToalla(int? id,string Turno,DateTime Fecha,TimeSpan Hora, string Linea,string Observacion,string estadoreg)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                ClsDControlToalla = new clsDControlToalla();
                string resultado = ClsDControlToalla.GuardarControlToallaCab(id,Turno,Fecha,Hora,Linea,Observacion,Request.UserHostAddress, liststring[0], estadoreg);
                return Json(resultado, JsonRequestBehavior.AllowGet);
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
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GuardarDetalleToalla(int IdDetalleToalla,int? NumToalla)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                ClsDControlToalla = new clsDControlToalla();
                string resultado = ClsDControlToalla.GuardarControlToallaDet(IdDetalleToalla,NumToalla,Request.UserHostAddress, liststring[0]);
                return Json(resultado, JsonRequestBehavior.AllowGet);
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
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult InactivarControlToalla(int IdCabToalla)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                ClsDControlToalla = new clsDControlToalla();
                string resultado = ClsDControlToalla.InactivarCOntrolToalla(IdCabToalla, Request.UserHostAddress, liststring[0]);
                return Json(resultado, JsonRequestBehavior.AllowGet);
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
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ReporteControlToalla()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                //clsDEmpleado = new clsDEmpleado();
                //liststring = User.Identity.Name.Split('_');
                //ViewBag.Linea = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault().CODIGOLINEA;
                //ViewBag.Linea = "52";
                return View();
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
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PartialReporteToalla(DateTime Fecha, string CodLinea, string Turno)
        {

            try
            {
                ClsDControlToalla = new clsDControlToalla();
                List<spReporteControlToalla> ListReporteToalla = ClsDControlToalla.ConsultarReporteControlToalla(Fecha,CodLinea,Turno);
                return PartialView(ListReporteToalla);
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
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}