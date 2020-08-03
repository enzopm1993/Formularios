using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.AnalisisQuimicoProductoSemielaborado;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.Mantenimientos;
using Asiservy.Automatizacion.Formularios.Models;
using Asiservy.Automatizacion.Formularios.Models.CALIDAD;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class AnalisisQuimicoProductoSemielaboradoController : Controller
    {
        // GET: AnalisisQuimicoProductoSemielaborado
        string[] lsUsuario { get; set; } = null;
        public clsDLogin clsDLogin { get; private set; }
        clsDError clsDError { get; set; } = null;
        clsDApiOrdenFabricacion clsDApiOrdenFabricacion { get; set; } = null;
        ClsDAnalisisQuimicoProductoSemielaborado ClsDAnalisisQuimicoProductoSemielaborado { get; set; } = null;
        clsDReporte clsDReporte { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        ClsDParametrosLaboratorio ClsDParametrosLaboratorio { get; set; } = null;
        public clsDPeriodo clsDPeriodo { get; private set; }

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
            {//**
                lsUsuario = User.Identity.Name.Split('_');
                clsDLogin = new clsDLogin();
                if (!string.IsNullOrEmpty(lsUsuario[1]))
                {
                    var usuarioOpcion = clsDLogin.ValidarPermisoOpcion(lsUsuario[1], "ReporteAnalisisQuimicoProductoSemielaborado");
                    if (usuarioOpcion)
                    {
                        ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ReporteAnalisisQuimicoProductoSemielaborado";
                    }
                }
                //**
                clsDClasificador = new clsDClasificador();
                ClsDParametrosLaboratorio = new ClsDParametrosLaboratorio();
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                ViewBag.MascaraInput = "1";
                ViewBag.MaskedInput = "1";
                ViewBag.DxDevWeb = "1";
                ViewBag.Turno = new SelectList(clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno), "Codigo", "Descripcion");
                ViewBag.ParametrosControl = ClsDParametrosLaboratorio.ConsultarParametrosFormularios(clsAtributos.AnalisisQuimicoProductoSemielaborado).Where(x=>x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();

                ViewBag.Area= new SelectList(clsDClasificador.ConsultarClasificador(clsAtributos.CodGrupoAreaLaboratorio), "Codigo", "Descripcion");
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
        public JsonResult ConsultarParametroxArea(string IdArea)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDParametrosLaboratorio = new ClsDParametrosLaboratorio();
                var DRespuesta = ClsDParametrosLaboratorio.ConsultarParametrosxArea(IdArea);
                if (DRespuesta.Count > 0)
                {
                    var respuesta = (from d in DRespuesta
                             select new { d.IdParametro, d.NombreParametro,d.Mascara }).ToList();
                    return Json(respuesta, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
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
        [Authorize]
        public ActionResult ReporteAnalisisQuimicoProductoSemielaborado()
        {
            try
            {
                //**
                lsUsuario = User.Identity.Name.Split('_');
                clsDLogin = new clsDLogin();
                if (!string.IsNullOrEmpty(lsUsuario[1]))
                {
                    var usuarioOpcion = clsDLogin.ValidarPermisoOpcion(lsUsuario[1], "ControlAnalisisQuimicoProductoSemielaborado");
                    if (usuarioOpcion)
                    {
                        ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ControlAnalisisQuimicoProductoSemielaborado";
                    }
                }
                //**
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.JqueryRotate = "1";
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.DxDevWeb = "1";
                //lsUsuario = User.Identity.Name.Split('_');

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
        public JsonResult ConsultarCabecerasReporte(DateTime FechaDesde, DateTime FechaHasta)
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

                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                List<CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA> Respuesta = ClsDAnalisisQuimicoProductoSemielaborado.ConsultarCabReportes(FechaDesde, FechaHasta);
                if (Respuesta.Count == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {

                    string turno;
                    string Fecha;
                    string FechaIngresoLog;
                    string FechaModificacionLog;
                    string FechaAprobacion;
                    foreach (var item in Respuesta)
                    {
                        turno = (from t in ListaTurnos
                                 where t.Codigo == item.Turno
                                 select t.Descripcion).FirstOrDefault();
                        Fecha = item.Fecha.Value.ToString("yyyy-MM-dd");
                        FechaIngresoLog = item.FechaIngresoLog.ToString("yyyy-MM-dd HH:mm");
                        FechaModificacionLog = item.FechaModificacionLog == null ? "" : item.FechaModificacionLog.Value.ToString("yyyy-MM-dd HH:mm");
                        FechaAprobacion = item.FechaAprobacion == null ? "" : item.FechaAprobacion.Value.ToString("yyyy-MM-dd HH:mm");
                        DRespuesta.Add(new
                        {
                            item.AprobadoPor,
                            item.EstadoControl,
                            item.EstadoRegistro,
                            Fecha,
                            FechaAprobacion,
                            FechaIngresoLog,
                            FechaModificacionLog,
                            item.IdAnalisisQuimicoProductoSe,
                            item.Observacion,
                            item.TerminalIngresoLog,
                            item.TerminalModificacionLog,
                            item.UsuarioIngresoLog,
                            item.UsuarioModificacionLog,
                            turno
                        });
                    }
                    clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                    var ordenes = clsDApiOrdenFabricacion.ConsultaDatosLotePorRangoFecha(FechaDesde, FechaHasta);
                    //ordenes.FirstOrDefault().
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
        [Authorize]
        public ActionResult BandejaAnalisisQuimicoProductoSemielaborado()
        {
            try
            {
                ViewBag.DateTimePicker = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
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
        public ActionResult BandejaAprobadosAnalisisQuimicoProductoSemiElaboradoPartial(DateTime? FechaInicio, DateTime? FechaFin, bool EstadoControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                List<CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA> resultado;
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                resultado = ClsDAnalisisQuimicoProductoSemielaborado.ConsultarBandejaAnalisisQuimicoProductoSemielaborado(FechaInicio, FechaFin, EstadoControl);
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
        public JsonResult GuardarCabeceraControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA poCabeceraControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(poCabeceraControl.Fecha.Value))
                {
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
                resultado = ClsDAnalisisQuimicoProductoSemielaborado.ConsultarCabeceraControl(poCabControl.Fecha.Value,poCabControl.Turno);
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
        public JsonResult EliminarCabeceraControl(int IdCabecera,DateTime poFecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(poFecha))
                {
                    object[] respuesta = new object[3];
                    respuesta[0] = "444";
                    respuesta[1] = "No se pudo completar la acción, por que el periodo se encuentra cerrado";
                    respuesta[2] = poFecha;
                    return Json(respuesta, JsonRequestBehavior.AllowGet);
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
        public JsonResult GuardarDetalleControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE poDetalleControl,DateTime poFecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(poFecha))
                {
                    object[] respuesta = new object[3];
                    respuesta[0] = "444";
                    respuesta[1] = "No se pudo completar la acción, por que el periodo se encuentra cerrado";
                    respuesta[2] = poFecha;
                    return Json(respuesta, JsonRequestBehavior.AllowGet);
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
        //[HttpPost]
        //public JsonResult GuardarSubDetalleControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO poSubDetalleControl,int CabeceraControl,DateTime poFecha)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }
        //        clsDPeriodo = new clsDPeriodo();
        //        if (!clsDPeriodo.ValidaFechaPeriodo(poFecha))
        //        {
        //            object[] respuesta = new object[3];
        //            respuesta[0] = "444";
        //            respuesta[1] = "No se pudo completar la acción, por que el periodo se encuentra cerrado";
        //            respuesta[2] = poFecha;
        //            return Json(respuesta, JsonRequestBehavior.AllowGet);
        //        }
        //        foreach (var item in poSubDetalleControl.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_PARAMETROXTIPO)
        //        {
        //            item.TerminalIngresoLog = Request.UserHostAddress;
        //            item.UsuarioIngresoLog = lsUsuario[0];
        //            item.FechaIngresoLog = DateTime.Now;
        //            item.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
        //        }
        //        poSubDetalleControl.FechaIngresoLog = DateTime.Now;
        //        poSubDetalleControl.UsuarioIngresoLog = lsUsuario[0];
        //        poSubDetalleControl.TerminalIngresoLog = Request.UserHostAddress;
        //        poSubDetalleControl.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
        //        object[] resultado = null;
        //        ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
        //        if (poSubDetalleControl.IdTipoAnalisisQuimicoProductoSe == 0)
        //        {
        //            resultado = ClsDAnalisisQuimicoProductoSemielaborado.GuardarSubDetalleControl(poSubDetalleControl,CabeceraControl);
        //        }
        //        else
        //        {
        //            resultado = ClsDAnalisisQuimicoProductoSemielaborado.ActualizarSubDetalleControl(poSubDetalleControl,CabeceraControl);
        //        }

        //        //clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
        //        //string resultado = clsDControlConsumoInsumo.GuardarPallet(pallet);
       
        //        return Json(resultado, JsonRequestBehavior.AllowGet);
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
        public ActionResult PartialSubDetalleControl(int IdDetalleControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                List<SPSubdetalleAnalisisQuimicoProductoSe> resultado;
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                resultado = ClsDAnalisisQuimicoProductoSemielaborado.ConsultarSubDetalleControl(IdDetalleControl);
                var Areas = (from a in resultado
                             orderby a.IdClasificador
                             select new Area { CodArea = a.CodArea, AreaNombre = a.Area, Total = 0 }
                             ).ToList().GroupBy(x => x.CodArea).Select(g => g.First()).ToList();
                foreach (var itemarea in Areas)
                {
                    itemarea.Total = resultado.Where(x => x.CodArea == itemarea.CodArea).Select(x => new { x.ParametroLaboratorio, x.NombreParametro, x.CodArea }).Distinct().Count();
                }
                ViewBag.Areas = Areas;
                List<dynamic> Parametros = (from p in resultado
                                            orderby p.IdClasificador, p.ParametroLaboratorio
                                            select new { p.ParametroLaboratorio, p.CodArea, p.NombreParametro }).Distinct().ToList<dynamic>();
                List<TipoProducto> TipoProducto = (from t in resultado
                                                   select new TipoProducto { CodTipoProd=t.CodTipoProducto,TipoProductoNombre=t.TipoProducto})
                                              .ToList().GroupBy(x => new {x.CodTipoProd }).Select(g => g.First()).ToList();
                ViewBag.Parametros = Parametros;
                ViewBag.TipoProducto = TipoProducto;
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
        //public JsonResult EliminarSubDetalleControl(int IdSubdetalle, int IdCabecera, DateTime poFecha)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }
        //        clsDPeriodo = new clsDPeriodo();
        //        if (!clsDPeriodo.ValidaFechaPeriodo(poFecha))
        //        {
        //            object[] respuesta = new object[3];
        //            respuesta[0] = "444";
        //            respuesta[1] = "No se pudo completar la acción, por que el periodo se encuentra cerrado";
        //            respuesta[2] = poFecha;
        //            return Json(respuesta, JsonRequestBehavior.AllowGet);
        //        }
        //        CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO poSubDetalle = new CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO()
        //        {
        //            IdTipoAnalisisQuimicoProductoSe = IdSubdetalle,
        //            UsuarioIngresoLog = lsUsuario[0],
        //            FechaIngresoLog = DateTime.Now,
        //            TerminalIngresoLog = Request.UserHostAddress
        //        };
        //        object[] Respuesta = null;
        //        ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
        //        Respuesta = ClsDAnalisisQuimicoProductoSemielaborado.InactivarSubDetalleControl(poSubDetalle, IdCabecera);
        //        return Json(Respuesta, JsonRequestBehavior.AllowGet);
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
   
        public JsonResult EliminarDetalleControl(int IdDetalle, int IdCabecera,DateTime poFecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(poFecha))
                {
                    object[] respuesta = new object[3];
                    respuesta[0] = "444";
                    respuesta[1] = "No se pudo completar la acción, por que el periodo se encuentra cerrado";
                    respuesta[2] = poFecha;
                    return Json(respuesta, JsonRequestBehavior.AllowGet);
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
                    ViewBag.NombreReporte = "ANÁLISIS QUÍMICO PRODUCTO SEMIELABORADO";
                }
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                List<SPReporteAnalisisQuimicoProductoSe> resultado;
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                resultado = ClsDAnalisisQuimicoProductoSemielaborado.ConsultaReporte(IdCabecera);
                //var MinMaxItems = resultado.Where(x => x.CalcMinMax == true).GroupBy(x => x.ParametroLaboratorio).Select(g => g.First()).ToList();
                var Areas = (from a in resultado
                             orderby a.IdClasificador
                             select new Area { CodArea = a.CodArea, AreaNombre = a.Area, Total = 0 }
                             ).ToList().GroupBy(x => x.CodArea).Select(g => g.First()).ToList();
                foreach (var itemarea in Areas)
                {
                    itemarea.Total = resultado.Where(x => x.CodArea == itemarea.CodArea).Select(x => new { x.ParametroLaboratorio, x.NombreParametro, x.CodArea }).Distinct().Count();
                }
                ViewBag.Areas = Areas;
                List<dynamic> Parametros = (from p in resultado
                                            orderby p.IdClasificador, p.ParametroLaboratorio
                                            select new { p.ParametroLaboratorio, p.CodArea, p.NombreParametro, p.CalcMinMax }).Distinct().ToList<dynamic>();
                List<TipoProducto> TipoProducto = (from t in resultado

                                                   select new TipoProducto {TipoProductoNombre = t.TipoProducto, CodTipoProd=t.CodTipoProducto })
                                              .ToList().GroupBy(x => new { x.CodTipoProd }).Select(g => g.First()).ToList(); ;
                ViewBag.Parametros = Parametros;
                ViewBag.TipoProducto = TipoProducto;
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
        public JsonResult AprobarControl(int IdCabecera, DateTime Fecha,DateTime poFecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(poFecha))
                {
                    string respuesta = "444";

                    return Json(respuesta, JsonRequestBehavior.AllowGet);
                }
                //byte[] Firma = Convert.FromBase64String(imagen);
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                string Respuesta = ClsDAnalisisQuimicoProductoSemielaborado.AprobarControl(IdCabecera, lsUsuario[0], Request.UserHostAddress, Fecha);
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
        public JsonResult ReversarControl(int IdCabecera,DateTime poFecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(poFecha))
                {
                    string respuesta = "444";

                    return Json(respuesta, JsonRequestBehavior.AllowGet);
                }
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                string Respuesta = ClsDAnalisisQuimicoProductoSemielaborado.ReversarControl(IdCabecera, lsUsuario[0], Request.UserHostAddress);
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
        public JsonResult GuardarSubDetalle_ParamxSubdetalle(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_PARAMETROXDETALLE poParamxDetalle, DateTime? poFecha,int Idcabecera)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(poFecha.Value))
                {
                    string respuesta = "444";

                    return Json(respuesta, JsonRequestBehavior.AllowGet);
                }
                poParamxDetalle.FechaIngresoLog = DateTime.Now;
                poParamxDetalle.UsuarioIngresoLog = lsUsuario[0];
                poParamxDetalle.TerminalIngresoLog = Request.UserHostAddress;
                //poSubdetalle.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_PARAMETROXTIPO.FirstOrDefault().UsuarioIngresoLog = lsUsuario[0];
                //poSubdetalle.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_PARAMETROXTIPO.FirstOrDefault().TerminalIngresoLog = Request.UserHostAddress;
                //poSubdetalle.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_PARAMETROXTIPO.FirstOrDefault().FechaIngresoLog = DateTime.Now;
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                object[] Respuesta = null;
                if (poParamxDetalle.IdTipoxParametro == 0)
                {
                    Respuesta = ClsDAnalisisQuimicoProductoSemielaborado.GuardarSubdetalle_ParamxSubdetalle(poParamxDetalle, Idcabecera);
                }
                else
                {
                    Respuesta = ClsDAnalisisQuimicoProductoSemielaborado.ActualizarSubdetalle_ParamxSubdetalle(poParamxDetalle, Idcabecera);
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
        public JsonResult ConsultarSubDetalle_ParamxSubdetalle(int IdDetalle)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                var Respuesta = ClsDAnalisisQuimicoProductoSemielaborado.ConsultarParametrosSubdetalle(IdDetalle);
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
        public JsonResult EliminarParametroSubDetalle(int IdTipoxParametro, DateTime poFecha, int IdCabecera)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(poFecha))
                {
                    object[] respuesta = new object[3];
                    respuesta[0] = "444";
                    respuesta[1] = "No se pudo completar la acción, por que el periodo se encuentra cerrado";
                    respuesta[2] = poFecha;
                    return Json(respuesta, JsonRequestBehavior.AllowGet);
                }
                CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_PARAMETROXDETALLE poModelo = new CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_PARAMETROXDETALLE()
                {
                    IdTipoxParametro = IdTipoxParametro,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                object[] Respuesta = null;
                ClsDAnalisisQuimicoProductoSemielaborado = new ClsDAnalisisQuimicoProductoSemielaborado();
                Respuesta = ClsDAnalisisQuimicoProductoSemielaborado.InactivarParametroxSubdetalle(poModelo, IdCabecera);
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