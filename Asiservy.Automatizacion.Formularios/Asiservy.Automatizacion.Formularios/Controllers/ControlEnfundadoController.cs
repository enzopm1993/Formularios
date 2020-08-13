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
        string[] lsUsuario { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        clsDEmpleado clsDEmpleado { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDControlEnfundado clsDControlEnfundado { get; set; } = null;
        clsDApiProduccion clsDApiProduccion { get; set; } = null;
        clsDPeriodo clsDPeriodo { get; set; } = null;

        #region CONTROL
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
                ViewBag.Turno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);
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

       
        public ActionResult ControlEnfundadoPartial(DateTime Fecha, string Turno)
        {
            try
            {

                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlEnfundado = new clsDControlEnfundado();
                var model = clsDControlEnfundado.ConsultaControlEnfundado(Fecha,Turno);
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

      
        public ActionResult ControlEnfundadoDetallePartial(int Id)
        {
            try
            {

                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
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

       
        [HttpPost]
        public ActionResult GenerarControlEnfundado(CONTROL_ENFUNDADO model)
        {
            try
            {

                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlEnfundado = new clsDControlEnfundado();
                lsUsuario  = User.Identity.Name.Split('_');
                model.UsuarioIngresoLog = lsUsuario [0];
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(model.Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                int id = clsDControlEnfundado.GenerarControlEnfundado(model);
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
       


        [HttpPost]
        public ActionResult GuardarControlEnfundado(CONTROL_ENFUNDADO_DETALLE detalle, DateTime Fecha)
        {
            try
            {

                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

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
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
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


        public JsonResult InactivarControlEnfundado(int IdControl, DateTime Fecha)
        {
            try
            {

                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                RespuestaGeneral respuestaGeneral = new RespuestaGeneral();
                clsDControlEnfundado = new clsDControlEnfundado();
                lsUsuario  = User.Identity.Name.Split('_');
                if (IdControl>0){
                    CONTROL_ENFUNDADO model = new CONTROL_ENFUNDADO();
                    model.IdControlEnfundado = IdControl;
                    model.FechaModificacionLog = DateTime.Now;
                    model.UsuarioModificacionLog = lsUsuario [0];
                    model.TerminalModificacionLog = Request.UserHostAddress;
                    clsDPeriodo = new clsDPeriodo();
                    if (!clsDPeriodo.ValidaFechaPeriodo(Fecha))
                    {
                        return Json("800", JsonRequestBehavior.AllowGet);
                    }
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
        #endregion

        #region REPORTE 
        [Authorize]
        public ActionResult ReporteControlEnfundado()
        {
            try
            {
                clsDClasificador = new clsDClasificador();
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.Turno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);

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
        public ActionResult ReportePorEnfundadora(DateTime Fecha, string Turno)
        {
            try
            {
                RespuestaGeneral  respuestaGeneral = new RespuestaGeneral();
                clsDControlEnfundado = new clsDControlEnfundado();
                var model = clsDControlEnfundado.ReporteControlEnfundadoPorEnfundadora(Fecha, Turno);
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
        public ActionResult ReportePorHora(DateTime Fecha, string Turno)
        {
            try
            {
                RespuestaGeneral  respuestaGeneral = new RespuestaGeneral();
                clsDControlEnfundado = new clsDControlEnfundado();
                var model = clsDControlEnfundado.ReporteControlEnfundadoPorHora(Fecha, Turno);
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