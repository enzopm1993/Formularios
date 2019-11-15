using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ControlEnfundado;
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
    public class ControlEnfundadoController : Controller
    {
        string[] lsUsuario  = null;
        clsDError clsDError = null;
        clsDEmpleado clsDEmpleado = null;
        clsDClasificador clsDClasificador = null;
        clsDControlEnfundado clsDControlEnfundado = null;
        clsDApiProduccion clsDApiProduccion = null;

        // GET: ControlEnfundado
        [Authorize]
        public ActionResult ControlEnfundado()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDApiProduccion = new clsDApiProduccion();
                lsUsuario  = User.Identity.Name.Split('_');
                clsDClasificador = new clsDClasificador();
                clsDEmpleado = new clsDEmpleado();
                var Empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario [1]).FirstOrDefault();
                var EspecificacionFunda = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo=clsAtributos.CodigoGrupoFunda, EstadoRegistro= clsAtributos.EstadoRegistroActivo});
                var Lotes = clsDApiProduccion.ConsultarLotesPorFecha(DateTime.Now);
                ViewBag.Lotes = Lotes;
                ViewBag.Linea = Empleado.LINEA;
                ViewBag.EspecificacionFunda = EspecificacionFunda;

                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                lsUsuario  = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario [0]
                });
                return RedirectToAction("Home", "Home");
            }

        }

        [Authorize]
        public ActionResult ControlEnfundadoPartial(DateTime Fecha)
        {
            try
            {
                clsDControlEnfundado = new clsDControlEnfundado();
                var model = clsDControlEnfundado.ConsultaControlEnfundado(Fecha);
                return PartialView(model);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        public ActionResult ControlEnfundadoDetallePartial(int Id)
        {
            try
            {
                clsDControlEnfundado = new clsDControlEnfundado();
                var model = clsDControlEnfundado.ConsultaControlEnfundadoDetalle(Id);
                return PartialView(model);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        [HttpPost]
        public ActionResult GenerarControlEnfundado(CONTROL_ENFUNDADO model)
        {
            try
            {
               
                clsDControlEnfundado = new clsDControlEnfundado();
                lsUsuario  = User.Identity.Name.Split('_');
                model.UsuarioIngresoLog = lsUsuario [0];
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                int id = clsDControlEnfundado.GenerarControlHueso(model);
                return Json(id, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize]
        [HttpPost]
        public ActionResult GuardarControlEnfundado(CONTROL_ENFUNDADO_DETALLE detalle)
        {
            try
            {
                if (detalle == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json("Parametros Incompletos", JsonRequestBehavior.AllowGet);
                }

                lsUsuario  = User.Identity.Name.Split('_');
                clsDControlEnfundado = new clsDControlEnfundado();
                detalle.UsuarioIngresoLog = lsUsuario [0];
                detalle.TerminalIngresoLog = Request.UserHostAddress;
                detalle.FechaIngresoLog = DateTime.Now;
                detalle.EstadoRegistro = clsAtributos.EstadoRegistroActivo;               
                var respuesta = clsDControlEnfundado.GuardarModificarControlEnfundado(detalle);

                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult ConsultarLotes (DateTime Fecha)
        {
            try
            {
                clsDApiProduccion = new clsDApiProduccion();
                dynamic Lotes = clsDApiProduccion.ConsultarLotesPorFecha(Fecha);
                List<ClasificadorGenerico> ListadoLotes = new List<ClasificadorGenerico>();
                foreach(var x in Lotes)
                {
                    ListadoLotes.Add(new ClasificadorGenerico {descripcion = x.Lote });
                }
                return Json(ListadoLotes, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult InactivarControlEnfundado(int IdControl)
        {
            try
            {
                RespuestaGeneral respuestaGeneral = new RespuestaGeneral();
                clsDControlEnfundado = new clsDControlEnfundado();
                lsUsuario  = User.Identity.Name.Split('_');
                if (IdControl>0){
                    CONTROL_ENFUNDADO model = new CONTROL_ENFUNDADO();
                    model.IdControlEnfundado = IdControl;
                    model.FechaModificacionLog = DateTime.Now;
                    model.UsuarioModificacionLog = lsUsuario [0];
                    model.TerminalModificacionLog = Request.UserHostAddress;

                    clsDControlEnfundado.InactivarControlEnfundado(model);
                    respuestaGeneral.Codigo = 1;
                    respuestaGeneral.Mensaje = "Registro Elimminado con Éxito";
                    return Json(respuestaGeneral, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    respuestaGeneral.Codigo = 0;
                    respuestaGeneral.Mensaje = "No ha seleccionado ningun Control de Enfundado";
                    return Json(respuestaGeneral, JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }


        [Authorize]
        public ActionResult ReporteControlEnfundado()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                return View();
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return View();
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return View();
            }
        }

        [Authorize]
        public ActionResult ReportePorEnfundadora(DateTime Fecha)
        {
            try
            {
                RespuestaGeneral  respuestaGeneral = new RespuestaGeneral();
                clsDControlEnfundado = new clsDControlEnfundado();
                var model = clsDControlEnfundado.ReporteControlEnfundadoPorEnfundadora(Fecha);
                if (!model.Any())
                {
                    respuestaGeneral.Codigo = 0;
                    respuestaGeneral.Mensaje = "No Existen Registros";
                    return Json(respuestaGeneral, JsonRequestBehavior.AllowGet);
                }
                return PartialView(model);

            }
           catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario  = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario [0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult ReportePorHora(DateTime Fecha)
        {
            try
            {
                RespuestaGeneral  respuestaGeneral = new RespuestaGeneral();
                clsDControlEnfundado = new clsDControlEnfundado();
                var model = clsDControlEnfundado.ReporteControlEnfundadoPorHora(Fecha);
                if(!model.Any())
                {
                    respuestaGeneral.Codigo = 0;
                    respuestaGeneral.Mensaje = "No Existen Registros";
                    return Json(respuestaGeneral, JsonRequestBehavior.AllowGet);
                }
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