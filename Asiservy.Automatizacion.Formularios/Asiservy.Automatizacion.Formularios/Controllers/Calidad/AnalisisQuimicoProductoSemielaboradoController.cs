using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.AnalisisQuimicoProductoSemielaborado;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class AnalisisQuimicoProductoSemielaboradoController : Controller
    {
        // GET: AnalisisQuimicoProductoSemielaborado
        string[] lsUsuario { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        ClsDAnalisisQuimicoProductoSemielaborado ClsDAnalisisQuimicoProductoSemielaborado { get; set; } = null;
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        [Authorize]
        public ActionResult ControlAnalisisQuimicoProductoSemielaborado()
        {
            try
            {
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.JqueryRotate = "1";
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                ViewBag.MascaraInput = "1";
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
        public JsonResult GuardarCabeceraControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA poCabeceraControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                poCabeceraControl.FechaIngresoLog = DateTime.Now;
                poCabeceraControl.UsuarioIngresoLog = lsUsuario[0];
                poCabeceraControl.TerminalIngresoLog = Request.UserHostAddress;
                poCabeceraControl.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado = null;
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                if (poCabeceraControl.IdAnalisisQuimicoProductoSe == 0)
                {
                    resultado = ClsDAnalisisQuimicoProductoSemielaborado.GuardarCabeceraControl(poCabeceraControl);
                }
                else
                {
                    resultado = ClsDAnalisisQuimicoProductoSemielaborado.ActualizarCabeceraControl(poCabeceraControl);
                }

                //clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                //string resultado = clsDControlConsumoInsumo.GuardarPallet(pallet);
                return Json(resultado, JsonRequestBehavior.AllowGet);
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
        public JsonResult ConsultarCabeceraControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA poCabControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA resultado = null;
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                resultado = ClsDAnalisisQuimicoProductoSemielaborado.ConsultarCabeceraControl(poCabControl.Fecha.Value);
                if (resultado != null)
                {
                    return Json(new
                    {
                        resultado.IdAnalisisQuimicoProductoSe,
                        resultado.Fecha,
                        resultado.Observacion,
                        resultado.EstadoControl
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
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
        public JsonResult EliminarCabeceraControl(int IdCabecera)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA poCabecera = new CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA()
                {
                    IdAnalisisQuimicoProductoSe = IdCabecera,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                object[] Respuesta = null;
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                Respuesta = ClsDAnalisisQuimicoProductoSemielaborado.InactivarCabeceraControl(poCabecera);
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
        public JsonResult GuardarDetalleControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE poDetalleControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                poDetalleControl.FechaIngresoLog = DateTime.Now;
                poDetalleControl.UsuarioIngresoLog = lsUsuario[0];
                poDetalleControl.TerminalIngresoLog = Request.UserHostAddress;
                poDetalleControl.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado = null;
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                if (poDetalleControl.IdDetalleAnalisisQuimicoProductoSe == 0)
                {
                    resultado = ClsDAnalisisQuimicoProductoSemielaborado.GuardarDetalleControl(poDetalleControl);
                }
                else
                {
                    resultado = ClsDAnalisisQuimicoProductoSemielaborado.ActualizarDetalleControl(poDetalleControl);
                }

                //clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                //string resultado = clsDControlConsumoInsumo.GuardarPallet(pallet);
                return Json(resultado, JsonRequestBehavior.AllowGet);
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
        public ActionResult PartialDetalleControl(int IdCabeceraControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                List<CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE> resultado;
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                resultado = ClsDAnalisisQuimicoProductoSemielaborado.ConsultarDetalleControl(IdCabeceraControl);
                if (resultado.Count == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                return PartialView(resultado);
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
        public JsonResult GuardarSubDetalleControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO poSubDetalleControl,int CabeceraControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                poSubDetalleControl.FechaIngresoLog = DateTime.Now;
                poSubDetalleControl.UsuarioIngresoLog = lsUsuario[0];
                poSubDetalleControl.TerminalIngresoLog = Request.UserHostAddress;
                poSubDetalleControl.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado = null;
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                if (poSubDetalleControl.IdTipoAnalisisQuimicoProductoSe == 0)
                {
                    resultado = ClsDAnalisisQuimicoProductoSemielaborado.GuardarSubDetalleControl(poSubDetalleControl,CabeceraControl);
                }
                else
                {
                    resultado = ClsDAnalisisQuimicoProductoSemielaborado.ActualizarSubDetalleControl(poSubDetalleControl,CabeceraControl);
                }

                //clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                //string resultado = clsDControlConsumoInsumo.GuardarPallet(pallet);
                return Json(resultado, JsonRequestBehavior.AllowGet);
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
        public ActionResult PartialSubDetalleControl(int IdDetalleControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                List<CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO> resultado;
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                resultado = ClsDAnalisisQuimicoProductoSemielaborado.ConsultarSubDetalleControl(IdDetalleControl);
                if (resultado.Count == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                return PartialView(resultado);
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
        public JsonResult EliminarSubDetalleControl(int IdSubdetalle, int IdCabecera)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO poSubDetalle = new CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO()
                {
                    IdTipoAnalisisQuimicoProductoSe = IdSubdetalle,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                object[] Respuesta = null;
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                Respuesta = ClsDAnalisisQuimicoProductoSemielaborado.InactivarSubDetalleControl(poSubDetalle, IdCabecera);
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
        public JsonResult EliminarDetalleControl(int IdDetalle, int IdCabecera)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE poDetalle = new CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE()
                {
                    IdDetalleAnalisisQuimicoProductoSe = IdDetalle,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                object[] Respuesta = null;
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                Respuesta = ClsDAnalisisQuimicoProductoSemielaborado.InactivarSubDetalleControl(poDetalle, IdCabecera);
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
    }
}