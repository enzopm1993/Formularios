using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.PRODUCCION.EntradaySalidaDeMaterialesEnAreasDeProceso;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;
using Asiservy.Automatizacion.Formularios.Models.Produccion.EntradaYSalidaDeMateriales;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class EntradaySalidaDeMaterialesEnAreasDeProcesoController : Controller
    {
        string[] lsUsuario { get; set; }
        clsDPeriodo clsDPeriodo { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        clsDGeneral clsDGeneral { get; set; } = null;
        clsDEmpleado clsDEmpleado { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        ClsDEntradaSalidaMateriales ClsDEntradaSalidaMateriales { get; set; }
        clsDReporte clsDReporte { get; set; }
        #region Métodos
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        #endregion
        // GET: EntradaySalidaDeMaterialesEnAreasDeProceso
     
        #region MANTENIMIENTO DE MATERIAL
        [Authorize]
        public ActionResult MantenimientoMaterial()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = "PRODUCCION/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                ViewBag.Material = ClsDEntradaSalidaMateriales.ConsultaMaterial();
                return View();
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return View();
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return View();
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult MantenimientoMaterial(ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.FechaIngresoLog = DateTime.Now;
                if (model.IdMaterial == 0)
                {
                    ClsDEntradaSalidaMateriales.GuardarMaterial(model);
                }
                else
                {
                    ClsDEntradaSalidaMateriales.ModificarMaterial(model);
                }
                
                return Json("Registro exitoso", JsonRequestBehavior.AllowGet);
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


        public ActionResult MantenimientoMaterialPartial()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                var model = ClsDEntradaSalidaMateriales.ConsultaMaterial();
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


        #endregion
        #region Control
        [Authorize]
        public ActionResult ControlEntradaySalidaDeMateriales()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                clsDClasificador = new clsDClasificador();
                ViewBag.JavaScrip = "PRODUCCION/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.JqueryRotate = "1";
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                ViewBag.MascaraInput = "1";
                ViewBag.MaskedInput = "1";
                ViewBag.Turno = new SelectList(clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno), "Codigo", "Descripcion");
                clsDEmpleado = new clsDEmpleado();
                ViewBag.Linea = clsDEmpleado.ConsultarEmpleadoxCedula(lsUsuario[1]).LINEA;
                ViewBag.NombreLinea = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault().LINEA;
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                ViewBag.Producto = new SelectList(ClsDEntradaSalidaMateriales.ConsultaMaterial(), "IdMaterial", "Nombre");
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
        public JsonResult GuardarCabeceraControl(ENTRADA_SALIDA_MATERIAL_CABECERA poCabeceraControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(poCabeceraControl.Fecha)){
                    object[] respuesta = new object[3];
                    respuesta[0] = "444";
                    respuesta[1] = "No se pudo completar la acción, por que el periodo se encuentra cerrado";
                    respuesta[2] = poCabeceraControl;
                    return Json(respuesta, JsonRequestBehavior.AllowGet);
                }
                poCabeceraControl.FechaIngresoLog = DateTime.Now;
                poCabeceraControl.UsuarioIngresoLog = lsUsuario[0];
                poCabeceraControl.TerminalIngresoLog = Request.UserHostAddress;
                poCabeceraControl.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado = null;
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                if (poCabeceraControl.IdControlEntradaSalidaMateriales == 0)
                {
                    resultado = ClsDEntradaSalidaMateriales.GuardarCabeceraControl(poCabeceraControl);
                }
                else
                {
                    resultado = ClsDEntradaSalidaMateriales.ActualizarCabeceraControl(poCabeceraControl);
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
        public JsonResult ConsultarCabeceraControl(ENTRADA_SALIDA_MATERIAL_CABECERA poCabControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ENTRADA_SALIDA_MATERIAL_CABECERA resultado = null;
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                resultado = ClsDEntradaSalidaMateriales.ConsultarCabeceraControl(poCabControl.Fecha, poCabControl.Turno);
                if (resultado != null)
                {
                    return Json(new
                    {
                        resultado.IdControlEntradaSalidaMateriales,
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
        public JsonResult EliminarCabeceraControl(int IdCabecera, DateTime Fechaco)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(Fechaco))
                {
                    object[] respuesta = new object[3];
                    respuesta[0] = "444";
                    respuesta[1] = "No se pudo completar la acción, por que el periodo se encuentra cerrado";
                    respuesta[2] = IdCabecera;
                    return Json(respuesta, JsonRequestBehavior.AllowGet);
                }
                ENTRADA_SALIDA_MATERIAL_CABECERA poCabecera = new ENTRADA_SALIDA_MATERIAL_CABECERA()
                {
                    IdControlEntradaSalidaMateriales = IdCabecera,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                object[] Respuesta = null;
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                Respuesta = ClsDEntradaSalidaMateriales.InactivarCabeceraControl(poCabecera);
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
        public JsonResult GuardarDetalleControl(ENTRADA_SALIDA_MATERIAL_DETALLE poDetalleControl,DateTime Fechaco)
        {
            try
            {
                clsDPeriodo = new clsDPeriodo();
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (!clsDPeriodo.ValidaFechaPeriodo(Fechaco))
                {
                    object[] respuesta = new object[3];
                    respuesta[0] = "444";
                    respuesta[1] = "No se pudo completar la acción, por que el periodo se encuentra cerrado";
                    respuesta[2] = poDetalleControl;
                    return Json(respuesta, JsonRequestBehavior.AllowGet);
                }
                poDetalleControl.FechaIngresoLog = DateTime.Now;
                poDetalleControl.UsuarioIngresoLog = lsUsuario[0];
                poDetalleControl.TerminalIngresoLog = Request.UserHostAddress;
                poDetalleControl.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado = null;
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                if (poDetalleControl.IdDetalleEntradaSalidaMateriales == 0)
                {
                    resultado = ClsDEntradaSalidaMateriales.GuardarDetalleControl(poDetalleControl);
                }
                else
                {
                    resultado = ClsDEntradaSalidaMateriales.ActualizarDetalleControl(poDetalleControl);
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

                List<EntradaSalidaMaterialDetalleViewModel> resultado;
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                resultado = ClsDEntradaSalidaMateriales.ConsultarDetalleControl(IdCabeceraControl);
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
        public JsonResult GuardarSubDetalleControl(ENTRADA_SALIDA_MATERIAL_SUBDETALLE poSubDetalleControl, int CabeceraControl,DateTime Fechaco)
        {
            try
            {
                clsDPeriodo = new clsDPeriodo();
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (!clsDPeriodo.ValidaFechaPeriodo(Fechaco))
                {
                    object[] respuesta = new object[3];
                    respuesta[0] = "444";
                    respuesta[1] = "No se pudo completar la acción, por que el periodo se encuentra cerrado";
                    respuesta[2] = poSubDetalleControl;
                    return Json(respuesta, JsonRequestBehavior.AllowGet);
                }
                poSubDetalleControl.FechaIngresoLog = DateTime.Now;
                poSubDetalleControl.UsuarioIngresoLog = lsUsuario[0];
                poSubDetalleControl.TerminalIngresoLog = Request.UserHostAddress;
                poSubDetalleControl.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado = null;
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                if (poSubDetalleControl.IdSubDetalleEntradaSalidaMaterial == 0)
                {
                    resultado = ClsDEntradaSalidaMateriales.GuardarSubDetalleControl(poSubDetalleControl, CabeceraControl);
                }
                else
                {
                    resultado = ClsDEntradaSalidaMateriales.ActualizarSubDetalleControl(poSubDetalleControl, CabeceraControl);
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

                List<ENTRADA_SALIDA_MATERIAL_SUBDETALLE> resultado;
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                resultado = ClsDEntradaSalidaMateriales.ConsultarSubDetalleControl(IdDetalleControl);
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
        public JsonResult EliminarSubDetalleControl(int IdSubdetalle, int IdCabecera, DateTime Fechaco)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(Fechaco))
                {
                    object[] respuesta = new object[3];
                    respuesta[0] = "444";
                    respuesta[1] = "No se pudo completar la acción, por que el periodo se encuentra cerrado";
                    respuesta[2] = IdSubdetalle;
                    return Json(respuesta, JsonRequestBehavior.AllowGet);
                }
                ENTRADA_SALIDA_MATERIAL_SUBDETALLE poSubDetalle = new ENTRADA_SALIDA_MATERIAL_SUBDETALLE()
                {
                    IdSubDetalleEntradaSalidaMaterial = IdSubdetalle,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                object[] Respuesta = null;
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                Respuesta = ClsDEntradaSalidaMateriales.InactivarSubDetalleControl(poSubDetalle, IdCabecera);
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
        public JsonResult EliminarDetalleControl(int IdDetalle, int IdCabecera, DateTime Fechaco)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(Fechaco))
                {
                    object[] respuesta = new object[3];
                    respuesta[0] = "444";
                    respuesta[1] = "No se pudo completar la acción, por que el periodo se encuentra cerrado";
                    respuesta[2] = IdDetalle;
                    return Json(respuesta, JsonRequestBehavior.AllowGet);
                }
                ENTRADA_SALIDA_MATERIAL_DETALLE poDetalle = new ENTRADA_SALIDA_MATERIAL_DETALLE()
                {
                    IdDetalleEntradaSalidaMateriales = IdDetalle,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                object[] Respuesta = null;
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                Respuesta = ClsDEntradaSalidaMateriales.InactivarDetalleControl(poDetalle, IdCabecera);
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
        public ActionResult ReporteEntradaySalidaDeMateriales()
        {
            try
            {

                ViewBag.JavaScrip = "PRODUCCION/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.JqueryRotate = "1";
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                lsUsuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                ViewBag.Linea = clsDEmpleado.ConsultarEmpleadoxCedula(lsUsuario[1]).LINEA;
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
        public JsonResult ConsultarCabecerasReporte(DateTime FechaDesde, DateTime FechaHasta,string CodLinea)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                
                List<dynamic> DRespuesta = new List<dynamic>();
                clsDClasificador = new clsDClasificador();
                List<CLASIFICADOR> ListaTurnos = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);
                clsDGeneral = new clsDGeneral();
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                List<ENTRADA_SALIDA_MATERIAL_CABECERA> Respuesta = ClsDEntradaSalidaMateriales.ConsultarCabReportes(FechaDesde, FechaHasta, CodLinea);
                if (Respuesta.Count == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string Linea;
                    string turno;
                    
                    foreach (var item in Respuesta)
                    {
                        turno = (from t in ListaTurnos
                                 where t.Codigo == item.Turno
                                 select t.Descripcion).FirstOrDefault();
                        Linea  = clsDGeneral.ConsultaLineas(item.Linea).FirstOrDefault().Descripcion;
                        DRespuesta.Add(new
                        {
                            item.AprobadoPor,
                            item.EstadoControl,
                            item.EstadoRegistro,
                            item.Fecha,
                            item.FechaAprobacion,
                            item.FechaIngresoLog,
                            item.FechaModificacionLog,
                            item.IdControlEntradaSalidaMateriales,
                            item.Observacion,
                            item.TerminalIngresoLog,
                            item.TerminalModificacionLog,
                            item.UsuarioIngresoLog,
                            item.UsuarioModificacionLog,
                            turno,
                            Linea
                        });
                    }
                 
                }
                return Json(DRespuesta, JsonRequestBehavior.AllowGet);
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
        public ActionResult PartialReporteControl(int IdCabecera)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                clsDReporte = new clsDReporte();
                var rep = clsDReporte.ConsultaCodigoReporte(RouteData.Values["action"].ToString());
                if (rep != null)
                {
                    ViewBag.CodigoReporte = rep.Codigo;
                    ViewBag.VersionReporte = rep.UltimaVersion;
                    ViewBag.NombreReporte = rep.Nombre;
                }
                else
                {
                    ViewBag.CodigoReporte = "AS-RG-CC-21";
                    ViewBag.VersionReporte = "V 10.0";
                    ViewBag.NombreReporte = "CONTROL DE ENTRADA Y SALIDA DE MATERIALES EN ÁREAS DE PROCESO";
                }
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                List<spReporteEntradaSalidaMaterialesProduccion> resultado;
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                resultado = ClsDEntradaSalidaMateriales.ConsultaReporte(IdCabecera);
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
        [Authorize]
        public ActionResult BandejaEntradaySalidaDeMateriales()
        {
            try
            {
                ViewBag.DateTimePicker = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.JavaScrip = "PRODUCCION/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                lsUsuario = User.Identity.Name.Split('_');
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
        public ActionResult BandejaAprobadosEntradaySalidaDeMaterialesPartial(DateTime? FechaInicio, DateTime? FechaFin, bool EstadoControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                List<EntradaSalidaMaterialViewModel> resultado;
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                resultado = ClsDEntradaSalidaMateriales.ConsultarBandejaEntradaySalidaDeMateriales(FechaInicio, FechaFin, EstadoControl);
                clsDClasificador = new clsDClasificador();
                ViewBag.Turnos = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);
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
        public JsonResult AprobarControl(int IdCabecera, DateTime Fecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                //byte[] Firma = Convert.FromBase64String(imagen);
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                string Respuesta = ClsDEntradaSalidaMateriales.AprobarControl(IdCabecera, lsUsuario[0], Request.UserHostAddress, Fecha);
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
        public JsonResult ReversarControl(int IdCabecera)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                string Respuesta = ClsDEntradaSalidaMateriales.ReversarControl(IdCabecera, lsUsuario[0], Request.UserHostAddress);
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
    }
}