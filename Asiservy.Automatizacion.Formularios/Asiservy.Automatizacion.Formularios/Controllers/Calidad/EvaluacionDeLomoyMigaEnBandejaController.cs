﻿using Asiservy.Automatizacion.Formularios.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.EvaluacionDeLomosyMigasEnBandeja;
using System.Net;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MantenimientoOlor;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.Mantenimientos;
using Asiservy.Automatizacion.Formularios.Models.Calidad;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class EvaluacionDeLomoyMigaEnBandejaController : Controller
    {
        // GET: EvaluacionDeLomoyMigaEnBandeja
        string[] lsUsuario = null;
        clsDError clsDError = null;
        clsDMantenimientoOlor clsDMantenimientoOlor = null;
        clsDMantenimientoTextura clsDMantenimientoTextura = null;
        clsDMantenimientoSabor clsDMantenimientoSabor = null;
        clsDMantenimientoProteina clsDMantenimientoProteina = null;
        clsDClasificador clsDClasificador = null;
        clsDEvaluacionDeLomosYMigasEnBandeja clsDEvaluacionDeLomosYMigasEnBandeja = null;
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        [Authorize]
        public ActionResult ControlEvaluacionLomoMigaBandeja()
        {
            try
            {

                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                lsUsuario = User.Identity.Name.Split('_');
                clsDMantenimientoOlor = new clsDMantenimientoOlor();
                clsDMantenimientoTextura = new clsDMantenimientoTextura();
                clsDMantenimientoSabor = new clsDMantenimientoSabor();
                clsDMantenimientoProteina = new clsDMantenimientoProteina();
                clsDClasificador = new clsDClasificador();
                var ListaTiposLimpieza = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoTipoLimpiezaPescado).OrderBy(x=>x.Codigo);
                var Lineas = clsDClasificador.ConsultarClasificador(clsAtributos.CodGrupoLineaProduccion).OrderBy(x => x.Codigo);
                var Olor = clsDMantenimientoOlor.ConsultaManteminetoOlor();
                var Textura = clsDMantenimientoTextura.ConsultaManteminetoTextura();
                var Sabor = clsDMantenimientoSabor.ConsultaManteminetoSabor();
                var Proteina = clsDMantenimientoProteina.ConsultaManteminetoProteina();
                ViewBag.Olor = new SelectList(Olor, "IdOlor", "Descripcion");
                ViewBag.Textura = new SelectList(Textura, "IdTextura", "Descripcion");
                ViewBag.Sabor = new SelectList(Sabor, "IdSabor", "Descripcion");
                ViewBag.Proteina = new SelectList(Proteina, "IdProteina", "Descripcion");
                ViewBag.NivelLimpieza =new SelectList(ListaTiposLimpieza, "Codigo","Descripcion");
                ViewBag.Lineas = new SelectList(Lineas, "Codigo", "Descripcion");
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
        public JsonResult GuardarCabeceraControl(CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA poCabeceraControl)
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
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                if (poCabeceraControl.IdEvaluacionDeLomosYMigasEnBandejas == 0)
                {
                    resultado = clsDEvaluacionDeLomosYMigasEnBandeja.GuardarCabeceraControl(poCabeceraControl);
                }
                else
                {
                    resultado = clsDEvaluacionDeLomosYMigasEnBandeja.ActualizarCabeceraControl(poCabeceraControl);
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
        public JsonResult ConsultarCabeceraControl(CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA poCabControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA resultado = null;
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                resultado = clsDEvaluacionDeLomosYMigasEnBandeja.ConsultarCabeceraControl(poCabControl.FechaProduccion.Value);
                if (resultado != null)
                {
                    return Json(new
                    {
                        resultado.IdEvaluacionDeLomosYMigasEnBandejas,
                        resultado.FechaProduccion,
                        resultado.Cliente,
                        resultado.Lomo,
                        resultado.Miga,
                        resultado.Empaque,

                        resultado.Enlatado,
                        resultado.Pouch,
                        resultado.NivelLimpieza,
                        resultado.OrdenFabricacion,
                        resultado.Observacion
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
                CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA poCabecera = new CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA()
                {
                    IdEvaluacionDeLomosYMigasEnBandejas = IdCabecera,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                object[] Respuesta = null;
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                Respuesta = clsDEvaluacionDeLomosYMigasEnBandeja.InactivarCabeceraControl(poCabecera);
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
        public JsonResult GuardarDetalleControl(CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE poDetalleControl)
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
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                if (poDetalleControl.IdDetalleEvaluacionLomoyMigasEnBandeja == 0)
                {
                    resultado = clsDEvaluacionDeLomosYMigasEnBandeja.GuardarDetalleControl(poDetalleControl);
                }
                else
                {
                    resultado = clsDEvaluacionDeLomosYMigasEnBandeja.ActualizarDetalleControl(poDetalleControl);
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
        public JsonResult EliminarDetalleControl(int IdDetalle)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE poDetControl = new CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE()
                {
                    IdDetalleEvaluacionLomoyMigasEnBandeja = IdDetalle,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                object[] Respuesta = null;
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                Respuesta = clsDEvaluacionDeLomosYMigasEnBandeja.InactivarDetalle(poDetControl);
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
        public ActionResult PartialDetalleControl(int IdCabeceraControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                List<DetalleEvaluacionLomosMIgasBandejaViewModel> resultado;
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                resultado = clsDEvaluacionDeLomosYMigasEnBandeja.ConsultarDetalleControl(IdCabeceraControl);
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
    }
}