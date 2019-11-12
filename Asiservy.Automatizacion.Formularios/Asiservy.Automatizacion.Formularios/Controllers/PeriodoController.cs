using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
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
    public class PeriodoController : Controller
    {
        clsDError clsDError = null;
        clsDPeriodo clsDPeriodo = null;
        string[] lsUsuario;
        [Authorize]
        public ActionResult Periodo()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                return View();

            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                //SetErrorMessage(Mensaje);
                return View();
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult Periodo(PERIODO model)
        {
            try
            {
                RespuestaGeneral Respuesta = new RespuestaGeneral();
                if (string.IsNullOrEmpty(model.Descripcion))
                {
                    Respuesta.Codigo = 0;
                    Respuesta.Mensaje = "Ingrese un Codigo";
                    return Json(Respuesta, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.Descripcion))
                {
                    Respuesta.Codigo = 0;
                    Respuesta.Mensaje = "Ingrese una Descripcion";
                    return Json(Respuesta, JsonRequestBehavior.AllowGet);
                }

               

                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                lsUsuario = User.Identity.Name.Split('_');
                clsDPeriodo = new clsDPeriodo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.FechaIngresoLog = DateTime.Now;
                // model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                string Mensaje = clsDPeriodo.GuardarModificarPeriodo(model);
                Respuesta.Codigo = 1;
                Respuesta.Mensaje = Mensaje;
                return Json(Respuesta, JsonRequestBehavior.AllowGet);

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
        public ActionResult PeriodoPartial()
        {
            try
            {
                clsDPeriodo = new clsDPeriodo();
                List<PERIODO> Periodos = new List<PERIODO>();
                Periodos = clsDPeriodo.ConsultaPeriodos(new PERIODO()).ToList();
                return PartialView(Periodos);
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