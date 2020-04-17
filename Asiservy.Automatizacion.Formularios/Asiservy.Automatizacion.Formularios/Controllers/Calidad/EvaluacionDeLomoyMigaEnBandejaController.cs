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
using System.IO;
using System.Text;

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
        clsDMantenimientoColor clsDMantenimientoColor = null;
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
        public ActionResult BandejaEvaluacionLomoMigaBandeja()
        {
            try
            {
                ViewBag.DateRangePicker = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                lsUsuario = User.Identity.Name.Split('_');
                string imagen = "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";
                byte[] data = Encoding.ASCII.GetBytes(imagen);
                //var base64 = Convert.ToBase64String(bytes);
                ViewBag.Base64String = "data:image/png;base64," + Convert.ToBase64String(data, 0, data.Length);
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

                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                lsUsuario = User.Identity.Name.Split('_');
                clsDMantenimientoOlor = new clsDMantenimientoOlor();
                clsDMantenimientoTextura = new clsDMantenimientoTextura();
                clsDMantenimientoSabor = new clsDMantenimientoSabor();
                clsDMantenimientoProteina = new clsDMantenimientoProteina();
                clsDMantenimientoColor = new clsDMantenimientoColor();
                clsDClasificador = new clsDClasificador();
                var ListaTiposLimpieza = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoTipoLimpiezaPescado).OrderBy(x=>x.Codigo);
                var Lineas = clsDClasificador.ConsultarClasificador(clsAtributos.CodGrupoLineaProduccion).OrderBy(x => x.Codigo);
                var Olor = clsDMantenimientoOlor.ConsultaManteminetoOlor().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                var Textura = clsDMantenimientoTextura.ConsultaManteminetoTextura().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                var Sabor = clsDMantenimientoSabor.ConsultaManteminetoSabor().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                var Proteina = clsDMantenimientoProteina.ConsultaManteminetoProteina().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                var Color = clsDMantenimientoColor.ConsultarMantenimientoColor().Where(x=>x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
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
        public ActionResult PartialReporteEvaluacionLomosMigasBandeja(DateTime Fecha)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                List<spReporteEvaluacionLomosMigasBandeja> resultado;
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                resultado = clsDEvaluacionDeLomosYMigasEnBandeja.ConsultarReporte(Fecha).OrderBy(x=>x.Hora).ToList();
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
        public ActionResult BandejaAprobadosEvaluacionDeLomoyMigaEnBandejaPartial(DateTime? FechaInicio, DateTime? FechaFin, bool EstadoControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                List<CabeceraEvaluacionLomosMigasViewModel> resultado = new List<CabeceraEvaluacionLomosMigasViewModel>();
                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                resultado = clsDEvaluacionDeLomosYMigasEnBandeja.ConsultarBandejaEvaluacionLomosyMiga(FechaInicio,FechaFin,EstadoControl);
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
        public JsonResult AprobarControl(int IdCabecera)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                

                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                string Respuesta = clsDEvaluacionDeLomosYMigasEnBandeja.AprobarControl(IdCabecera,Request.UserHostAddress, lsUsuario[0]);
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
        public JsonResult GuardarImagenFirma(string imagen,int IdCabecera,string Tipo)
        {
          
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }


                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                byte[] Firma = Convert.FromBase64String(imagen);
                var resultado = clsDEvaluacionDeLomosYMigasEnBandeja.GuardarImagenFirma(Firma,IdCabecera,Tipo,lsUsuario[0],Request.UserHostAddress);
                //var base64 = Convert.ToBase64String(Firma);
                //var imgSrc = String.Format("data:image/png;base64,{0}", base64);
                var imagenfirma= String.Format("data:image/png;base64,{0}", imagen);
                return Json(imagenfirma, JsonRequestBehavior.AllowGet);
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
        public JsonResult ConsultarFirma(int IdCabecera)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }


                clsDEvaluacionDeLomosYMigasEnBandeja = new clsDEvaluacionDeLomosYMigasEnBandeja();
                //byte[] Firma = Convert.FromBase64String(imagen);
                var resultado = clsDEvaluacionDeLomosYMigasEnBandeja.ConsultarFirmaControl(IdCabecera);
                //var base64 = Convert.ToBase64String(Firma);
                //var imgSrc = String.Format("data:image/png;base64,{0}", base64);
                //var imagenfirma = String.Format("data:image/png;base64,{0}", imagen);
                if (resultado != null)
                {
                    var base64 = Convert.ToBase64String(resultado);
                    var imagenfirma = String.Format("data:image/png;base64,{0}", base64);
                    return Json(imagenfirma, JsonRequestBehavior.AllowGet);
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
    }
}