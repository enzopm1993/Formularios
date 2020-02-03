using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }

        }
        [HttpPost]
        public ActionResult PartialControlToalla(DateTime? Fecha,string Linea, string Turno)
        {
            try
            {
                ClsDControlToalla = new clsDControlToalla();
                List<CONTROL_TOALLA> ListCabCOntrolToalla = ClsDControlToalla.ConsultarCabToalla(Fecha.Value,Linea, Turno).OrderByDescending(X=>X.Hora).ToList();
                return PartialView(ListCabCOntrolToalla);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
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
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GuardarControlToalla(int? id,string Turno,DateTime Fecha,DateTime Hora, string Linea,string Observacion,string estadoreg)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                ClsDControlToalla = new clsDControlToalla();
                string resultado = ClsDControlToalla.GuardarControlToallaCab(id,Turno,Fecha,Hora,Linea,Observacion,Request.UserHostAddress, liststring[0], estadoreg);
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
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
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
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
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
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
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
        }
        public ActionResult PartialReporteToalla(DateTime Fecha, string CodLinea, string Turno)
        {

            try
            {
                ClsDControlToalla = new clsDControlToalla();
                List<spReporteControlToalla> ListReporteToalla = ClsDControlToalla.ConsultarReporteControlToalla(Fecha,CodLinea,Turno);
                var ListNombres= ListReporteToalla.Select(x => new  {x.CEDULA, x.NOMBRES }).Distinct();
                List<spReporteControlToalla> LstNombres = new List<spReporteControlToalla>();
                List<spReporteControlToalla> lstHoras = new List<spReporteControlToalla>();
                foreach (var item in ListNombres)
                {
                    LstNombres.Add(new spReporteControlToalla {CEDULA=item.CEDULA, NOMBRES=item.NOMBRES });
                }
                ViewBag.Nombres = LstNombres.OrderBy(x=>x.NOMBRES).ToList(); /*ListReporteToalla.Select(x => new {x.CEDULA, x.NOMBRES}).Distinct();*/
                var Horas = ListReporteToalla.Select(x => new  { x.HORA }).Distinct();
                foreach (var item in Horas)
                {
                    lstHoras.Add(new spReporteControlToalla {HORA=item.HORA });
                }
                ViewBag.Horas = lstHoras;
                ViewBag.ContHoras = lstHoras.Count();
               
                return PartialView(ListReporteToalla);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }
    }
}