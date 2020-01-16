using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ControlConsumoInsumo;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ControlConsumoInsumoController : Controller
    {
        string[] lsUsuario = null;
        clsDError clsDError = null;
        clsDEmpleado clsDEmpleado = null;
        clsDClasificador clsDClasificador = null;
       // clsDGeneral clsDGeneral = null;
        clsDLogin clsDLogin = null;
        clsDApiOrdenFabricacion clsDApiOrdenFabricacion = null;
        clsDControlConsumoInsumo clsDControlConsumoInsumo = null;


        #region CONTROL CONSUMOS INSUMOS
        // GET: ControlConsumoInsumo
        [Authorize]
        public ActionResult ControlConsumoInsumo()
        {
            try
            {
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                lsUsuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                clsDClasificador = new clsDClasificador();
                var Empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();

                ViewBag.Linea = Empleado.LINEA;
                ViewBag.CodLinea = Empleado.CODIGOLINEA;
                ViewBag.OrdenesFabricacion = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaProduccion(DateTime.Now);


                clsDLogin = new clsDLogin();
                var rol = clsDLogin.ValidarUsuarioRol(lsUsuario[1], clsAtributos.RolPouch);
                if (rol)
                {
                    ViewBag.Daniado = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoConsumoDaniadoPouch);
                    ViewBag.LineaNegocio = "POUCH";
                }
                else
                {
                    ViewBag.Daniado = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoConsumoDaniadoLata);
                    ViewBag.LineaNegocio = "ENLATADO";
                }
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


        public ActionResult ControlConsumoInsumoPartial(DateTime Fecha, string LineaNegocio, string Turno)
        {
            try
            {
              
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[1]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                
                var model = clsDControlConsumoInsumo.ConsultaControlConsumoInsumo(Fecha,LineaNegocio,Turno);
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult ControlConsumoInsumo(CONTROL_CONSUMO_INSUMO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[1]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if(model==null||model.OrdenFabricacion==0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Mensaje = clsDControlConsumoInsumo.GuardarModificarControl(model);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
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
        public ActionResult EliminarControlConsumoInsumo(CONTROL_CONSUMO_INSUMO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[1]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null || model.IdControlConsumoInsumos == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Mensaje = clsDControlConsumoInsumo.EliminarControlConsumoInsumo(model);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
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

        #region CONSUMO DETALLE ENLATADO
        public ActionResult ControlConsumoInsumoDetalleEnlatadoPartial(int IdControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[1]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();

                var model = clsDControlConsumoInsumo.ConsultaConsumoDetalleLata(IdControl);
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult GuardarConsumoInsumoDetalleEnlatado(CONSUMO_DETALLE_LATA model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[1]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDControlConsumoInsumo.GuardarModificarDetalleEnlatado(model);
                return Json(Respuesta,JsonRequestBehavior.AllowGet);
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
        public ActionResult EliminarConsumoDetalleLata(CONSUMO_DETALLE_LATA model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[1]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null || model.IdProcesoDetalleLata == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Mensaje = clsDControlConsumoInsumo.EliminarDetalleEnlatado(model);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
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

        #region CONSUMO DETALLE POUCH
        public ActionResult ControlConsumoInsumoDetallePouchPartial(int IdControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[1]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();

                var model = clsDControlConsumoInsumo.ConsultaConsumoDetallePouch(IdControl);
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult GuardarConsumoInsumoDetallePouch(CONSUMO_DETALLE_POUCH model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[1]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDControlConsumoInsumo.GuardarModificarDetallePouch(model);
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


        [HttpPost]
        public ActionResult EliminarConsumoDetallePouch(CONSUMO_DETALLE_POUCH model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[1]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null || model.IdProcesoDetallePouch == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Mensaje = clsDControlConsumoInsumo.EliminarDetallePouch(model);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
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

        #region CONSUMO DAÑADOS
        public ActionResult ControlConsumoDaniadoPartial(int IdControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[1]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();

                var model = clsDControlConsumoInsumo.ConsultaConsumoDaniado(IdControl);
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
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


        [HttpPost]
        public ActionResult GuardarConsumoDaniado(CONSUMO_DETALLE_DANIADO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[1]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDControlConsumoInsumo.GuardarModificarConsumoDaniado(model);
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

        [HttpPost]
        public ActionResult EliminarConsumoDaniado(CONSUMO_DETALLE_DANIADO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[1]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDControlConsumoInsumo.EliminarConsumoDaniado(model);
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