using Asiservy.Automatizacion.Formularios.AccesoDatos;
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
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MantenimientoColor;
using Asiservy.Automatizacion.Formularios.Models.CALIDAD;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;
using System.IO;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class EvaluacionDeLomoyMigaEnBandejaController : Controller
    {
        // GET: EvaluacionDeLomoyMigaEnBandeja
        private clsDReporte clsDReporte { get;set;}=null;
        private string[] lsUsuario { get; set; } = null;
        private clsDError clsDError { get; set; } = null;
        private clsDMantenimientoOlor clsDMantenimientoOlor { get; set; } = null;
        private clsDMantenimientoTextura clsDMantenimientoTextura { get; set; } = null;
        private clsDMantenimientoSabor clsDMantenimientoSabor { get; set; } = null;
        private clsDMantenimientoProteina clsDMantenimientoProteina { get; set; } = null;
        private clsDMantenimientoColor clsDMantenimientoColor { get; set; } = null;
        private clsDClasificador clsDClasificador { get; set; } = null;
        private ClsDMantenimientoMoreton ClsDMantenimientoMoreton { get; set; } = null;
        private clsDEvaluacionDeLomosYMigasEnBandeja clsDEvaluacionDeLomosYMigasEnBandeja { get; set; } = null;
        clsDApiOrdenFabricacion clsDApiOrdenFabricacion { get; set; } = null;
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        [Authorize]
        public ActionResult BandejaEvaluacionLomoMigaBandeja()
        {
            try
            {
                ViewBag.DateTimePicker = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.JqueryRotate = "1";
                //ViewBag.FirmaPad = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
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
        [Authorize]
        public ActionResult ControlEvaluacionLomoMigaBandeja()
        {
            try
            {
                ViewBag.MaskedInput = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.JqueryRotate = "1";
                //ViewBag.FirmaPad = "1";
                lsUsuario = User.Identity.Name.Split('_');
                clsDMantenimientoOlor = new clsDMantenimientoOlor();
                clsDMantenimientoTextura = new clsDMantenimientoTextura();
                clsDMantenimientoSabor = new clsDMantenimientoSabor();
                clsDMantenimientoProteina = new clsDMantenimientoProteina();
                clsDMantenimientoColor = new clsDMantenimientoColor();
                clsDClasificador = new clsDClasificador();
                ClsDMantenimientoMoreton = new ClsDMantenimientoMoreton();
                var ListaTiposLimpieza = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoTipoLimpiezaPescado).OrderBy(x=>x.Codigo);
                var Lineas = clsDClasificador.ConsultarClasificador(clsAtributos.CodGrupoLineaProduccion).OrderBy(x => x.Codigo);
                var Olor = clsDMantenimientoOlor.ConsultaManteminetoOlor().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                var Textura = clsDMantenimientoTextura.ConsultaManteminetoTextura().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                var Sabor = clsDMantenimientoSabor.ConsultaManteminetoSabor().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                var Proteina = clsDMantenimientoProteina.ConsultaManteminetoProteina().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                var Color = clsDMantenimientoColor.ConsultarMantenimientoColor().Where(x=>x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
                var Moretones = ClsDMantenimientoMoreton.ConsultaManteminetoMoreton();
                ViewBag.Turno = new SelectList(clsDClasificador.ConsultarClasificador(clsAtributos.Turnos), "Codigo", "Descripcion");
                ViewBag.Moreton = new SelectList(Moretones, "IdMoreton", "Descripcion");
                ViewBag.Olor = new SelectList(Olor, "IdOlor", "Descripcion");
                ViewBag.Textura = new SelectList(Textura, "IdTextura", "Descripcion");
                ViewBag.Sabor = new SelectList(Sabor, "IdSabor", "Descripcion");
                ViewBag.Color = new SelectList(Color, "IdColor", "Descripcion");
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
        [Authorize]
        public ActionResult ReporteEvaluacionLomoMigaBandeja()
        {
            try
            {
                
                ViewBag.DateRangePicker = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.JqueryRotate = "1";
                ViewBag.select2 = "1";
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
                resultado = clsDEvaluacionDeLomosYMigasEnBandeja.ConsultarCabeceraControl(poCabControl.FechaProduccion.Value,poCabControl.OrdenFabricacion.Value,poCabControl.Turno);
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
        public JsonResult EliminarFotoDetalle(int IdFoto)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                CC_EVALUACION_LOMO_MIGA_FOTO poFotoLomosyMigas = new CC_EVALUACION_LOMO_MIGA_FOTO()
                {
                    IdFotoEvaluacioLomosyMigas = IdFoto,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                object[] Respuesta = null;
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                Respuesta = clsDEvaluacionDeLomosYMigasEnBandeja.InactivarFotoDetalle(poFotoLomosyMigas);
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
        public ActionResult PartialReporteFotos(int IdCabecera)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                List<ReporteFotosEvaluacionLomosyMigasViewModel> resultado;
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                resultado = clsDEvaluacionDeLomosYMigasEnBandeja.ConsultarFotosControl(IdCabecera);
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
        public ActionResult PartialDetalleBandeja(int IdCabeceraControl)
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
        public ActionResult PartialReporteEvaluacionLomosMigasBandeja(int IdEvaluacionDeLomosYMigasEnBandejas)
        {
            try
            {
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
                    ViewBag.NombreReporte = "EVALUACIÓN DE LOMOS Y MIGAS EN BANDEJAS";
                }

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                List<spReporteEvaluacionLomosMigasBandeja> resultado;
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                resultado = clsDEvaluacionDeLomosYMigasEnBandeja.ConsultarReporte(IdEvaluacionDeLomosYMigasEnBandejas).OrderBy(x=>x.Hora).ToList();
                if (resultado.Count == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    clsDApiOrdenFabricacion clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                    ViewBag.Cliente = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaConsumoInsumo(resultado.FirstOrDefault().OrdenFabricacion.ToString()).FirstOrDefault().CLIENTE;
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
        public ActionResult BandejaAprobadosEvaluacionDeLomoyMigaEnBandejaPartial(DateTime? FechaInicio, DateTime? FechaFin, bool EstadoControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                List<CabeceraEvaluacionLomosMigasViewModel> resultado;
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                resultado = clsDEvaluacionDeLomosYMigasEnBandeja.ConsultarBandejaEvaluacionLomosyMiga(FechaInicio,FechaFin,EstadoControl);
                if (resultado.Count==0)
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
        public JsonResult AprobarControl(int IdCabecera,DateTime Fecha/*, string imagen*/)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                //byte[] Firma = Convert.FromBase64String(imagen);
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                string Respuesta = clsDEvaluacionDeLomosYMigasEnBandeja.AprobarControl(IdCabecera, lsUsuario[0],Request.UserHostAddress,Fecha/*,Firma*/);
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

                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                string Respuesta = clsDEvaluacionDeLomosYMigasEnBandeja.ReversarControl(IdCabecera, lsUsuario[0], Request.UserHostAddress);
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
        public JsonResult ConsultarCabecerasReporte(DateTime FechaDesde, DateTime FechaHasta)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                //byte[] Firma = Convert.FromBase64String(imagen);
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                List<CabeceraEvaluacionLomosMigasViewModel> Respuesta = clsDEvaluacionDeLomosYMigasEnBandeja.ConsultarCabReportes(FechaDesde,FechaHasta);
                if (Respuesta.Count == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                    var ordenes = clsDApiOrdenFabricacion.ConsultaDatosLotePorRangoFecha(FechaDesde,FechaHasta);
                    //ordenes.FirstOrDefault().
                }
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
        public JsonResult GuardarFotoDetalle(HttpPostedFileBase dataImg, CC_EVALUACION_LOMO_MIGA_FOTO control)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                object resultado = new object[3];
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                string path = string.Empty;
                string NombreImg = string.Empty;
                if (dataImg != null)
                {
                    path = Server.MapPath(clsAtributos.UrlImagen +"EvaluacionDeLomosYMigasEnBandeja/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var date = DateTime.Now;
                    long n = long.Parse(date.ToString("yyyyMMddHHmmss"));
                    var ext2 = dataImg.FileName.Split('.');
                    var cont = ext2.Length;
                    NombreImg = "EvaluacionDeLomosYMigasEnBandeja/EvaluacionDeLomosYMigasEnBandeja" + n.ToString() + "." + ext2[cont - 1];
                    control.Imagen = NombreImg;
                }


                control.UsuarioIngresoLog = lsUsuario[0];
                control.FechaIngresoLog = DateTime.Now;
                control.TerminalIngresoLog = Request.UserHostAddress;
                control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                if (control.IdFotoEvaluacioLomosyMigas == 0)
                {
                    resultado=clsDEvaluacionDeLomosYMigasEnBandeja.GuardarDetalleFoto(control);
                }
                else
                {
                    resultado=clsDEvaluacionDeLomosYMigasEnBandeja.ModificarDetalleFoto(control);
                }
                
                if (dataImg != null)
                {
                    dataImg.SaveAs(path + Path.GetFileName(NombreImg));
                }
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
        public ActionResult DetalleFotoPartial(int IdDetalle)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

               
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                var resultado = clsDEvaluacionDeLomosYMigasEnBandeja.ConsultarFotosDetalle(IdDetalle);
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
        //[HttpPost]
        //public JsonResult GuardarImagenFirma(string imagen,int IdCabecera,string Tipo)
        //{

        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }


        //        clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
        //        byte[] Firma = Convert.FromBase64String(imagen);
        //        var resultado = clsDEvaluacionDeLomosYMigasEnBandeja.GuardarImagenFirma(Firma,IdCabecera,Tipo,lsUsuario[0],Request.UserHostAddress);
        //        //var base64 = Convert.ToBase64String(Firma);
        //        //var imgSrc = String.Format("data:image/png;base64,{0}", base64);
        //        var imagenfirma= String.Format("data:image/png;base64,{0}", imagen);
        //        return Json(imagenfirma, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }

        //}
        //public JsonResult ConsultarFirma(int IdCabecera)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }


        //        clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
        //        var resultado = clsDEvaluacionDeLomosYMigasEnBandeja.ConsultarFirmaControl(IdCabecera);

        //        if (resultado != null)
        //        {
        //            var base64 = Convert.ToBase64String(resultado);
        //            var imagenfirma = String.Format("data:image/png;base64,{0}", base64);
        //            return Json(imagenfirma, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("0", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public JsonResult ConsultarFirmaAprobacion(int IdCabecera)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }


        //        clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
        //        var resultado = clsDEvaluacionDeLomosYMigasEnBandeja.ConsultarFirmaAprobacion(IdCabecera);

        //        if (resultado[0] != null)
        //        {
        //            var base64 = Convert.ToBase64String(resultado[0] as byte[]);
        //            var imagenfirma = String.Format("data:image/png;base64,{0}", base64);
        //            return Json(imagenfirma, JsonRequestBehavior.AllowGet);
        //        }
        //        else if (Convert.ToBoolean(resultado[1])&&(resultado[0] ==null)){
        //            return Json("000", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("0", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}