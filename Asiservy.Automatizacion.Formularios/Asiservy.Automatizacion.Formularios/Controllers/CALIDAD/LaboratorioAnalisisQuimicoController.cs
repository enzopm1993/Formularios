﻿//using System.Web.Mvc;
//using Asiservy.Automatizacion.Formularios.AccesoDatos;
//using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.LaboratorioAnalisisQuimico;
//using Asiservy.Automatizacion.Datos.Datos;
//using System.Data.Entity.Validation;
//using System.Net;
//using System;
//using System.Linq;
//using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;
//using System.Collections.Generic;
//using System.Web;
//using System.IO;
//using Asiservy.Automatizacion.Formularios.AccesoDatos.General;

//namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
//{
//    public class LaboratorioAnalisisQuimicoController : Controller
//    {       
//        public clsDReporte ClsDReporte { get; set; } = null;
//        clsDError clsDError { get; set; } = null;
//        ClsDLaboratorioAnalisisQuimico ClsDLaboratorioAnalisisQuimico { get; set; } = null;
//        clsDApiOrdenFabricacion clsDApiOrdenFabricacion { get; set; } = null;
//        string[] lsUsuario { get; set; } = null;

//        #region MANTENIMIENTO LABORATORIO ANALISIS QUIMICOS PRECOCCION   
//        public ActionResult MantenimientoTurno()
//        {
//            try
//            {
//                ViewBag.dataTableJS = "1";
//                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];              
//                return View();
//            }
//            catch (DbEntityValidationException e)
//            {
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//                SetErrorMessage(Mensaje);
//                return RedirectToAction("Home", "Home");
//            }
//            catch (Exception ex)
//            {
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//                SetErrorMessage(Mensaje);
//                return RedirectToAction("Home", "Home");
//            }
//        }
//        public ActionResult MantenimientoTurnoPartial()
//        {
//            try
//            {
//                lsUsuario = User.Identity.Name.Split('_');
//                if (string.IsNullOrEmpty(lsUsuario[0]))
//                {
//                    return Json("101", JsonRequestBehavior.AllowGet);
//                }
//                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
//                var lista = ClsDLaboratorioAnalisisQuimico.ConsultarMantenimiento();
//                if (lista != null)
//                {
//                    return PartialView(lista);
//                }
//                else
//                {
//                    return Json("0", JsonRequestBehavior.AllowGet);
//                }
//            }
//            catch (DbEntityValidationException e)
//            {
//                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//        }
//        public ActionResult GuardarModificarMantenimiento(CC_ANALISIS_QUIMICO_PRECOCCION_TURNO model)
//        {
//            try
//            {
//                lsUsuario = User.Identity.Name.Split('_');
//                if (string.IsNullOrEmpty(lsUsuario[0]))
//                {
//                    return Json("101", JsonRequestBehavior.AllowGet);
//                }
//                if (!string.IsNullOrWhiteSpace(model.Nombre))
//                {
//                    ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
//                    model.FechaIngresoLog = DateTime.Now;
//                    model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
//                    model.TerminalIngresoLog = Request.UserHostAddress;
//                    model.UsuarioIngresoLog = lsUsuario[0];
//                    var valor = ClsDLaboratorioAnalisisQuimico.GuardarModificarMantenimiento(model);
//                    if (valor == 0)
//                    {
//                        return Json("0", JsonRequestBehavior.AllowGet);
//                    }
//                    else if (valor == 1) return Json("1", JsonRequestBehavior.AllowGet);
//                    else if (valor == 2) return Json("2", JsonRequestBehavior.AllowGet);
//                    else return Json("3", JsonRequestBehavior.AllowGet);
//                }
//                else return Json("4", JsonRequestBehavior.AllowGet);
//            }
//            catch (DbEntityValidationException e)
//            {
//                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//        }
//        public ActionResult EliminarMantenimiento(CC_ANALISIS_QUIMICO_PRECOCCION_TURNO model)
//        {
//            try
//            {
//                lsUsuario = User.Identity.Name.Split('_');
//                if (string.IsNullOrEmpty(lsUsuario[0]))
//                {
//                    return Json("101", JsonRequestBehavior.AllowGet);
//                }
//                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
//                model.FechaIngresoLog = DateTime.Now;
//                model.TerminalIngresoLog = Request.UserHostAddress;
//                model.UsuarioIngresoLog = lsUsuario[0];
//                var valor = ClsDLaboratorioAnalisisQuimico.EliminarMantenimiento(model);
//                if (valor == 0)
//                {
//                    return Json("0", JsonRequestBehavior.AllowGet);
//                }
//                else if (valor == 1) return Json("1", JsonRequestBehavior.AllowGet);
//                else return Json("2", JsonRequestBehavior.AllowGet);//EXISTRE REGISTRO ACTIVO CON EL MISMO NOMBRE
//            }
//            catch (DbEntityValidationException e)
//            {
//                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//        }
//        #endregion

//        #region CONTROL
//        public ActionResult ControlAnalisisQuimico()
//        {
//            try
//            {
//                lsUsuario = User.Identity.Name.Split('_');
//                ViewBag.dataTableJS = "1";
//                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
//                ViewBag.JqueryRotate = "1";
//                ViewBag.usuarioAnalista= lsUsuario[0];
//                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
//                ViewBag.Turno = ClsDLaboratorioAnalisisQuimico.ConsultarMantenimiento(clsAtributos.EstadoRegistroActivo);
//                return View();
//            }
//            catch (DbEntityValidationException e)
//            {
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//                SetErrorMessage(Mensaje);
//                return RedirectToAction("Home", "Home");
//            }
//            catch (Exception ex)
//            {
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//                SetErrorMessage(Mensaje);
//                return RedirectToAction("Home", "Home");
//            }
//        }
//        public ActionResult ControlAnalisisQuimicoPartial(DateTime fechaDesde)
//        {
//            try
//            {
//                lsUsuario = User.Identity.Name.Split('_');
//                if (string.IsNullOrEmpty(lsUsuario[0]))
//                {
//                    return Json("101", JsonRequestBehavior.AllowGet);
//                }
//                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
//                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
//                var listaOrdenFabricacion = clsDApiOrdenFabricacion.ConsultaDatosLotePorRangoFecha(fechaDesde, fechaDesde);
//                ViewBag.listaOrdenFabricacion = listaOrdenFabricacion;
//                //var listaDetalleCocinador = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte();

//                if (listaOrdenFabricacion.Count != 0)
//                {
//                    return PartialView(listaOrdenFabricacion);
//                }
//                else
//                {
//                    return Json("No existen registros", JsonRequestBehavior.AllowGet);
//                }
//            }
//            catch (DbEntityValidationException e)
//            {
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//                SetErrorMessage(Mensaje);
//                return RedirectToAction("Home", "Home");
//            }
//            catch (Exception ex)
//            {
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//                SetErrorMessage(Mensaje);
//                return RedirectToAction("Home", "Home");
//            }
//        }
//        //public ActionResult ConsultarDetallePartial(int idMaterial, int op)
//        //{
//        //    try
//        //    {
//        //        lsUsuario = User.Identity.Name.Split('_');
//        //        if (string.IsNullOrEmpty(lsUsuario[0]))
//        //        {
//        //            return Json("101", JsonRequestBehavior.AllowGet);
//        //        }
//        //        ClsMaterialQuebradizo = new ClsMaterialQuebradizo();
//        //        var lista = ClsMaterialQuebradizo.ConsultarDetalle(idMaterial, op);
//        //        if (lista.Count != 0)
//        //        {
//        //            return PartialView(lista);
//        //        }
//        //        else
//        //        {
//        //            return Json("0", JsonRequestBehavior.AllowGet);
//        //        }
//        //    }
//        //    catch (DbEntityValidationException e)
//        //    {
//        //        clsDError = new clsDError();
//        //        lsUsuario = User.Identity.Name.Split('_');
//        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//        //        SetErrorMessage(Mensaje);
//        //        return RedirectToAction("Home", "Home");
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        clsDError = new clsDError();
//        //        lsUsuario = User.Identity.Name.Split('_');
//        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//        //        SetErrorMessage(Mensaje);
//        //        return RedirectToAction("Home", "Home");
//        //    }
//        //}
//        public JsonResult GuardarModificarAnalisisQuimico(CC_ANALISIS_QUIMICO_PRECOCCION_CTRL model, int siAprobar)
//        {
//            try
//            {
//                lsUsuario = User.Identity.Name.Split('_');
//                if (string.IsNullOrEmpty(lsUsuario[0]))
//                {
//                    return Json("101", JsonRequestBehavior.AllowGet);
//                }
//                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
//                model.FechaIngresoLog = DateTime.Now;
//                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
//                model.TerminalIngresoLog = Request.UserHostAddress;
//                model.UsuarioIngresoLog = lsUsuario[0];
//                if (model.IdAnalisis != 0 && siAprobar == 0)
//                {
//                    var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(model.IdAnalisis, DateTime.MinValue);
//                    if (estadoReporte.EstadoReporte)
//                    {
//                        return Json("4", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
//                    }
//                }
//                var valor = ClsDLaboratorioAnalisisQuimico.GuardarModificarAnalisisQuimico(model, siAprobar);
//                if (valor == 0)
//                {
//                    return Json("0", JsonRequestBehavior.AllowGet);
//                }
//                else if (valor == 1)
//                {
//                    return Json("1", JsonRequestBehavior.AllowGet);
//                }
//                else if (valor == 2) { return Json("2", JsonRequestBehavior.AllowGet); }
//                else return Json("3", JsonRequestBehavior.AllowGet);//ERROR DE FECHA
//            }
//            catch (DbEntityValidationException e)
//            {
//                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//        }
//        public JsonResult EliminarAnalisisQuimico(CC_ANALISIS_QUIMICO_PRECOCCION_CTRL model)
//        {
//            try
//            {
//                lsUsuario = User.Identity.Name.Split('_');
//                if (string.IsNullOrEmpty(lsUsuario[0]))
//                {
//                    return Json("101", JsonRequestBehavior.AllowGet);
//                }
//                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
//                model.FechaIngresoLog = DateTime.Now;
//                model.TerminalIngresoLog = Request.UserHostAddress;
//                model.UsuarioIngresoLog = lsUsuario[0];
//                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
//                var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(model.IdAnalisis, DateTime.MinValue);
//                if (!estadoReporte.EstadoReporte)
//                {
//                    var valor = ClsDLaboratorioAnalisisQuimico.EliminarAnalisisQuimico(model);
//                    if (valor == 0)
//                    {
//                        return Json("0", JsonRequestBehavior.AllowGet);
//                    }
//                    else return Json("1", JsonRequestBehavior.AllowGet);
//                }
//                else return Json("2", JsonRequestBehavior.AllowGet);
//            }
//            catch (DbEntityValidationException e)
//            {
//                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//        }
//        public JsonResult ConsultarEstadoReporte(DateTime fechaControl, int idAnalisis = 0)
//        {
//            try
//            {
//                lsUsuario = User.Identity.Name.Split('_');
//                if (string.IsNullOrEmpty(lsUsuario[0]))
//                {
//                    return Json("101", JsonRequestBehavior.AllowGet);
//                }
//                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
//                var lista = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(idAnalisis, fechaControl);
//                if (lista != null)
//                {
//                    return Json(lista, JsonRequestBehavior.AllowGet);
//                }
//                else
//                {
//                    return Json("0", JsonRequestBehavior.AllowGet);
//                }
//            }
//            catch (DbEntityValidationException e)
//            {
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//                SetErrorMessage(Mensaje);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//                SetErrorMessage(Mensaje);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//        }
//        public JsonResult GuardarModificarDetalle(CC_ANALISIS_QUIMICO_PRECOCCION_DET model)
//        {
//            try
//            {
//                lsUsuario = User.Identity.Name.Split('_');
//                if (string.IsNullOrEmpty(lsUsuario[0]))
//                {
//                    return Json("101", JsonRequestBehavior.AllowGet);
//                }
//                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();

//                var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(model.IdAnalisis, DateTime.MinValue);
//                if (estadoReporte.EstadoReporte)
//                {
//                    return Json("3", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
//                }
//                var valor = 0;

//                model.FechaIngresoLog = DateTime.Now;
//                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
//                model.TerminalIngresoLog = Request.UserHostAddress;
//                model.UsuarioIngresoLog = lsUsuario[0];
//                valor = ClsDLaboratorioAnalisisQuimico.GuardarModificarDetalle(model);

//                if (valor == 0)
//                {
//                    return Json("0", JsonRequestBehavior.AllowGet);
//                }
//                else
//                {
//                    return Json("1", JsonRequestBehavior.AllowGet);
//                }

//            }
//            catch (DbEntityValidationException e)
//            {
//                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//        }
//        public JsonResult EliminarDetalle(CC_ANALISIS_QUIMICO_PRECOCCION_DET model)
//        {
//            try
//            {
//                lsUsuario = User.Identity.Name.Split('_');
//                if (string.IsNullOrEmpty(lsUsuario[0]))
//                {
//                    return Json("101", JsonRequestBehavior.AllowGet);
//                }
//                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
//                var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(model.IdAnalisis, DateTime.MinValue);
//                if (estadoReporte == null)
//                {
//                    return Json("3", JsonRequestBehavior.AllowGet);//IDMANTERIAL NO ENCONTRADO
//                }
//                if (estadoReporte.EstadoReporte)
//                {
//                    return Json("2", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
//                }
//                var valor = 0;

//                model.FechaIngresoLog = DateTime.Now;
//                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
//                model.TerminalIngresoLog = Request.UserHostAddress;
//                model.UsuarioIngresoLog = lsUsuario[0];
//                valor = ClsDLaboratorioAnalisisQuimico.EliminarDetalle(model);

//                if (valor == 0)
//                {
//                    return Json("0", JsonRequestBehavior.AllowGet);
//                }
//                else
//                {
//                    return Json("1", JsonRequestBehavior.AllowGet);
//                }
//            }
//            catch (DbEntityValidationException e)
//            {
//                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                clsDError = new clsDError();
//                lsUsuario = User.Identity.Name.Split('_');
//                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//                return Json(Mensaje, JsonRequestBehavior.AllowGet);
//            }
//        }
//        //public JsonResult GuardarModificarAccionCorrectiva(CC_MATERIAL_QUEBRADIZO_ACCI_CORRECTIVA model, HttpPostedFileBase dataImg)
//        //{
//        //    try
//        //    {
//        //        lsUsuario = User.Identity.Name.Split('_');
//        //        if (string.IsNullOrEmpty(lsUsuario[0]))
//        //        {
//        //            return Json("101", JsonRequestBehavior.AllowGet);
//        //        }

//        //        ClsMaterialQuebradizo = new ClsMaterialQuebradizo();
//        //        var estadoReporte = ClsMaterialQuebradizo.ConsultarEstadoReporte(model.IdMaterial, DateTime.MinValue);
//        //        if (estadoReporte.EstadoReporte)
//        //        {
//        //            return Json("3", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
//        //        }
//        //        string path = string.Empty;
//        //        string NombreImg = string.Empty;
//        //        if (dataImg != null)
//        //        {
//        //            decimal mb = 1024 * 1024 * 5;//bytes to Mb; max 5Mb
//        //            var supportedTypes = new[] { "jpg", "jpeg" };
//        //            var fileExt = Path.GetExtension(dataImg.FileName).Substring(1);
//        //            if (!supportedTypes.Contains(fileExt))
//        //            {
//        //                return Json("4", JsonRequestBehavior.AllowGet);//NO ES IMG
//        //            }
//        //            else if (dataImg.ContentLength > (mb))
//        //            {
//        //                return Json(dataImg.ContentLength, JsonRequestBehavior.AllowGet);//SOBREPASA EL LIMITE PERMITIDO dataImg.ContentLength=bytes convert to Mb
//        //            }
//        //            path = Server.MapPath(clsAtributos.UrlImagen + "MaterialQuebradizo/");
//        //            if (!Directory.Exists(path))
//        //            {
//        //                Directory.CreateDirectory(path);
//        //            }
//        //            var date = DateTime.Now;
//        //            long n = long.Parse(date.ToString("yyyyMMddHHmmss"));
//        //            var ext2 = dataImg.FileName.Split('.');
//        //            var cont = ext2.Length;
//        //            NombreImg = "AccionCorrectiva" + n.ToString() + "." + ext2[cont - 1];
//        //            model.RutaFoto = NombreImg;
//        //        }
//        //        model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
//        //        model.FechaIngresoLog = DateTime.Now;
//        //        model.TerminalIngresoLog = Request.UserHostAddress;
//        //        model.UsuarioIngresoLog = lsUsuario[0];
//        //        var valor = ClsMaterialQuebradizo.GuardarModificarAccionCorrectiva(model);
//        //        if (dataImg != null)
//        //        {
//        //            dataImg.SaveAs(path + Path.GetFileName(NombreImg));
//        //        }
//        //        if (valor == 0)
//        //        {
//        //            return Json("0", JsonRequestBehavior.AllowGet);
//        //        }
//        //        else
//        //        {
//        //            return Json("1", JsonRequestBehavior.AllowGet);
//        //        }
//        //    }
//        //    catch (DbEntityValidationException e)
//        //    {
//        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//        //        clsDError = new clsDError();
//        //        lsUsuario = User.Identity.Name.Split('_');
//        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//        //        clsDError = new clsDError();
//        //        lsUsuario = User.Identity.Name.Split('_');
//        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
//        //    }
//        //}
//        //public ActionResult VerCrearImagenPartial(int idMaterial, int idArea, int op)
//        //{
//        //    try
//        //    {
//        //        lsUsuario = User.Identity.Name.Split('_');
//        //        if (string.IsNullOrEmpty(lsUsuario[0]))
//        //        {
//        //            return Json("101", JsonRequestBehavior.AllowGet);
//        //        }
//        //        ClsMaterialQuebradizo = new ClsMaterialQuebradizo();
//        //        var lista = ClsMaterialQuebradizo.ConsultarDetalle(idMaterial, op, idArea);
//        //        if (lista.Count != 0)
//        //        {
//        //            ViewBag.Path = clsAtributos.UrlImagen.Replace("~", "..") + "MaterialQuebradizo/";
//        //            return PartialView(lista);
//        //        }
//        //        else
//        //        {
//        //            return Json("0", JsonRequestBehavior.AllowGet);
//        //        }
//        //    }
//        //    catch (DbEntityValidationException e)
//        //    {
//        //        clsDError = new clsDError();
//        //        lsUsuario = User.Identity.Name.Split('_');
//        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//        //        SetErrorMessage(Mensaje);
//        //        return RedirectToAction("Home", "Home");
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        clsDError = new clsDError();
//        //        lsUsuario = User.Identity.Name.Split('_');
//        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//        //        SetErrorMessage(Mensaje);
//        //        return RedirectToAction("Home", "Home");
//        //    }
//        //}
//        //public JsonResult EliminarAccionCorrectiva(CC_MATERIAL_QUEBRADIZO_ACCI_CORRECTIVA model)
//        //{
//        //    try
//        //    {
//        //        lsUsuario = User.Identity.Name.Split('_');
//        //        if (string.IsNullOrEmpty(lsUsuario[0]))
//        //        {
//        //            return Json("101", JsonRequestBehavior.AllowGet);
//        //        }
//        //        ClsMaterialQuebradizo = new ClsMaterialQuebradizo();
//        //        var estadoReporte = ClsMaterialQuebradizo.ConsultarEstadoReporte(model.IdMaterial, DateTime.MinValue);
//        //        if (estadoReporte.EstadoReporte)
//        //        {
//        //            return Json("2", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
//        //        }
//        //        var valor = 0;
//        //        model.FechaIngresoLog = DateTime.Now;
//        //        model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
//        //        model.TerminalIngresoLog = Request.UserHostAddress;
//        //        model.UsuarioIngresoLog = lsUsuario[0];
//        //        valor = ClsMaterialQuebradizo.EliminarAccionCorrectiva(model);

//        //        if (valor == 0)
//        //        {
//        //            return Json("0", JsonRequestBehavior.AllowGet);
//        //        }
//        //        else
//        //        {
//        //            return Json("1", JsonRequestBehavior.AllowGet);
//        //        }
//        //    }
//        //    catch (DbEntityValidationException e)
//        //    {
//        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//        //        clsDError = new clsDError();
//        //        lsUsuario = User.Identity.Name.Split('_');
//        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
//        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//        //        clsDError = new clsDError();
//        //        lsUsuario = User.Identity.Name.Split('_');
//        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
//        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
//        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
//        //    }
//        //}
//        #endregion
//        protected void SetSuccessMessage(string message)
//        {
//            TempData["MensajeConfirmacion"] = message;
//        }
//        protected void SetErrorMessage(string message)
//        {
//            TempData["MensajeError"] = message;
//        }
//    }
//}