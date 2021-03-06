﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ControlCuchillosPreparacion;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity.Validation;
using System.Net;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;
using System.Linq;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class ControlCuchillosPreparacionController : Controller
    {
        clsDPeriodo clsDPeriodo { get; set; } = null;
        clsDLogin clsLogin { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        public clsDReporte ClsDReporte { get; set; } = null;
        clsDControlCuchillosPreparacion clsDControlCuchillosPreparacion { get; set; } = null;
        string[] lsUsuario { get; set; }=null;
        //-----------------------------------------------------VISTA DE INGRESO DE DATOS DE CUCHILLO----------------------------------------------------------------
        [Authorize]
        public ActionResult MantenimientoCuchilloPreparacion()
        {
            try
            {
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
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

        public ActionResult ConsultarCuchilloPreparacionPartial(string codigoCuchillo, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                var poCloroCisterna = clsDControlCuchillosPreparacion.ConsultarCuchilloPreparacion(codigoCuchillo, op);
                if (poCloroCisterna != null)
                {
                    return PartialView(poCloroCisterna);
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

        public JsonResult ConsultarCuchilloPreparacion(string codigoCuchillo, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                var poCloroCisterna = clsDControlCuchillosPreparacion.ConsultarCuchilloPreparacion(codigoCuchillo, op);
                if (poCloroCisterna != null)
                {                               
                    return Json(poCloroCisterna, JsonRequestBehavior.AllowGet);
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

        public ActionResult GuardarModificarCuchilloPreparacion(CC_CUCHILLOS_PREPARACION model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;

                model.UsuarioIngresoLog = lsUsuario[0];
                clsDControlCuchillosPreparacion.GuardarModificarCuchilloPreparacion(model);
                return Json("Registro Exitoso", JsonRequestBehavior.AllowGet);
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

        //-----------------------------------------------------CONTROL CUCHILLO PREPARACION----------------------------------------------------------------------
        [Authorize]
        public ActionResult ControlCuchilloPreparacion(string codigoCuchillo="", int op=0)
        {
            try
            {
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.DateTimePicker = "1";
                clsLogin = new clsDLogin();
                lsUsuario = User.Identity.Name.Split('_');
                var usuarioOpcion = clsLogin.ValidarPermisoOpcion(lsUsuario[1], "ReporteControlCuchilloPreparacion");
                if (usuarioOpcion)
                {
                    ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ReporteControlCuchilloPreparacion";
                }
               
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();                
                var listaEmpleadoLinea = clsDControlCuchillosPreparacion.ConsultaMovimientoPersonalDiario(DateTime.Now, "46");
                ViewBag.listaEmpleadoLinea = listaEmpleadoLinea;
                var poCloroCisterna = clsDControlCuchillosPreparacion.ConsultarCuchilloPreparacion(codigoCuchillo, op);
                ViewBag.Cuchillos = poCloroCisterna;
                clsDClasificador = new clsDClasificador();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();
                if (poTurno != null)
                {
                    ViewBag.Turno = poTurno;
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

        public JsonResult ConsultarCabecera(DateTime fecha, int turno, int idControlCuchillo=0)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                var poCloroCisterna = clsDControlCuchillosPreparacion.ConsultarCabecera(fecha,idControlCuchillo, turno);
                if (poCloroCisterna != null)
                {
                    return Json(poCloroCisterna, JsonRequestBehavior.AllowGet);
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

        public JsonResult GuardarModificarControlCuchilloPreparacion(CC_CUCHILLOS_PREPARACION_CTRL model,int turno, bool siAprobar=false)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(model.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.UsuarioIngresoLog = lsUsuario[0];
                var validarEstadoReporte = clsDControlCuchillosPreparacion.ConsultarCabecera(DateTime.MinValue, model.IdControlCuchillo, turno);
                if (validarEstadoReporte != null && validarEstadoReporte.EstadoReporte)
                {
                    return Json("3", JsonRequestBehavior.AllowGet);//CONTROL APROBADO
                }                
               
                var valor = clsDControlCuchillosPreparacion.GuardarModificarControlCuchilloPreparacion(model, siAprobar);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 1) { return Json("1", JsonRequestBehavior.AllowGet); }
                else if (valor == 2) return Json("2", JsonRequestBehavior.AllowGet);
                else return Json("4", JsonRequestBehavior.AllowGet);// TURNO REPETIDO
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
        //-----------------------------------------------------CONTROL CUCHILLO PREPARACION----------------------------------------------------------------------
        public ActionResult ConsultarHoraPartial(int idControlCuchillo)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                var hora = clsDControlCuchillosPreparacion.ConsultarHora(idControlCuchillo);
                if (hora != null)
                {
                    return PartialView(hora);
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
        public JsonResult GuardarModificarHora(CC_CUCHILLOS_PREPARACION_HORA model, int idControlCuchillo, int turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.UsuarioIngresoLog = lsUsuario[0];
                var consultarEstado = clsDControlCuchillosPreparacion.ConsultarCabecera(DateTime.MinValue, idControlCuchillo, turno);
                if (consultarEstado != null)
                {
                    clsDPeriodo = new clsDPeriodo();
                    bool periodo = clsDPeriodo.ValidaFechaPeriodo(consultarEstado.Fecha);
                    if (!periodo)
                    {
                        return Json("100", JsonRequestBehavior.AllowGet);
                    }
                    if (consultarEstado.EstadoReporte)
                    {
                        return Json("2", JsonRequestBehavior.AllowGet);
                    }
                    var valor = clsDControlCuchillosPreparacion.GuardarModificarHora(model);
                    if (valor == 0)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else return Json("1", JsonRequestBehavior.AllowGet);
                }
                else return Json("10", JsonRequestBehavior.AllowGet);
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
        public ActionResult EliminarrHora(CC_CUCHILLOS_PREPARACION_HORA model, int idControlCuchillo, int turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                model.UsuarioIngresoLog = lsUsuario[0];
                var consultarEstado = clsDControlCuchillosPreparacion.ConsultarCabecera(DateTime.MinValue, idControlCuchillo, turno);
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(consultarEstado.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                if (consultarEstado.EstadoReporte)
                {
                    return Json("2", JsonRequestBehavior.AllowGet);
                }
                var valor = clsDControlCuchillosPreparacion.EliminarHora(model);
                if (valor == 1)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
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
        public ActionResult EliminarControlCuchilloPreparacion(CC_CUCHILLOS_PREPARACION_CTRL model, int turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(model.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                var validarEstadoReporte = clsDControlCuchillosPreparacion.ConsultarCabecera(DateTime.Now, model.IdControlCuchillo, turno);
                if (!validarEstadoReporte.EstadoReporte)
                {
                    var poCloroCisterna = clsDControlCuchillosPreparacion.EliminarControlCuchilloPreparacion(model);
                    if (poCloroCisterna == 1)
                    {
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                }
                else return Json("2", JsonRequestBehavior.AllowGet);

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

        //-----------------------------------------------------CONTROL CUCHILLO PREPARACION DETALLE----------------------------------------------------------------------
        public JsonResult ConsultarControlCuchilloDetalle(int idHora, int op, DateTime fecha, string codLinea, string turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                var poCloroCisterna = clsDControlCuchillosPreparacion.ConsultarDetalle(idHora,op);
                if (poCloroCisterna != null)
                {
                    if (fecha>DateTime.Now)
                    {
                        fecha = DateTime.Now;
                    }
                    var listaEmpleadoLinea = clsDControlCuchillosPreparacion.ConsultaMovimientoPersonalDiario(fecha, codLinea);
                    List<string> listaCedula = new List<string>();
                    foreach (var itemCuchillo in poCloroCisterna)
                    {
                        listaCedula.Add(itemCuchillo.CedulaEmpleado);
                        foreach (var itemEmpleado in listaEmpleadoLinea)
                        {
                            if (itemCuchillo.CedulaEmpleado == itemEmpleado.Cedula)
                            {
                                itemCuchillo.CedulaEmpleado += "-" + itemEmpleado.NOMBRES;
                            }
                        }

                    }
                    return Json(poCloroCisterna, JsonRequestBehavior.AllowGet);
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

        public JsonResult GuardarModificarControlCuchilloDetalle(CC_CUCHILLOS_PREPARACION_CTRL_DET model, int idControlCuchillo, int turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.UsuarioIngresoLog = lsUsuario[0];
                var validarEstadoReporte = clsDControlCuchillosPreparacion.ConsultarCabecera(DateTime.MinValue, idControlCuchillo, turno);
                clsDPeriodo = new clsDPeriodo();
                if (validarEstadoReporte!=null && !validarEstadoReporte.EstadoReporte)
                {
                    bool periodo = clsDPeriodo.ValidaFechaPeriodo(validarEstadoReporte.Fecha);
                    if (!periodo)
                    {
                        return Json("100", JsonRequestBehavior.AllowGet);
                    }                
                    var valor = clsDControlCuchillosPreparacion.GuardarModificarControlCuchilloDetalle(model);
                    if (valor == 0)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else return Json("1", JsonRequestBehavior.AllowGet);
                }
                else return Json("2", JsonRequestBehavior.AllowGet);
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
        public ActionResult EliminarControlCuchilloDetalle(CC_CUCHILLOS_PREPARACION_CTRL_DET model, int idControlCuchillo, int turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                var validarEstadoReporte = clsDControlCuchillosPreparacion.ConsultarCabecera(DateTime.MinValue, idControlCuchillo, turno);
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(validarEstadoReporte.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                if (!validarEstadoReporte.EstadoReporte)
                    {
                        var poCloroCisterna = clsDControlCuchillosPreparacion.EliminarControlCuchilloDetalle(model);
                        if(poCloroCisterna == 1)
                        {
                            return Json("1", JsonRequestBehavior.AllowGet);
                        }
                    else return Json("0", JsonRequestBehavior.AllowGet);
                }
                    else return Json("2", JsonRequestBehavior.AllowGet);               

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

        ////------------------------------------REPORTE CONTROL CUCHILLO PREPARACION DETALLE----------------------------------------------
        [Authorize]
        public ActionResult ReporteControlCuchilloPreparacion()
        {
            try
            {
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.DateRangePicker = "1";
                ViewBag.dataTableJS = "1";
                clsLogin = new clsDLogin();
                lsUsuario = User.Identity.Name.Split('_');
                var usuarioOpcion = clsLogin.ValidarPermisoOpcion(lsUsuario[1], "ControlCuchilloPreparacion");
                if (usuarioOpcion)
                {
                    ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ControlCuchilloPreparacion";
                }
                ClsDReporte = new clsDReporte();
                var rep = ClsDReporte.ConsultaCodigoReporte(RouteData.Values["action"].ToString());
                if (rep != null)
                {
                    ViewBag.CodigoReporte = rep.Codigo;
                    ViewBag.VersionReporte = rep.UltimaVersion;
                    ViewBag.NombreReporte = rep.Nombre;
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

        public ActionResult ReporteControlCuchilloPreparacionPartial(int idControlCuchillo, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                var poCloroCisterna = clsDControlCuchillosPreparacion.ConsultarBandeja(idControlCuchillo, op);
                if (poCloroCisterna.Count != 0)
                {
                    //clsDEmpleado empleado = new clsDEmpleado();
                    var listaEmpleadoLinea = clsDControlCuchillosPreparacion.ConsultaMovimientoPersonalDiario(poCloroCisterna[0].Fecha, "46");
                    foreach (var itemCuchillo in poCloroCisterna)
                    {
                        foreach (var itemEmpleado in listaEmpleadoLinea)
                        {
                            if (itemCuchillo.CedulaEmpleado == itemEmpleado.Cedula)
                            {
                                itemCuchillo.CedulaEmpleado += "-" + itemEmpleado.NOMBRES;
                            }
                        }
                    }
                    return PartialView(poCloroCisterna);
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

        public ActionResult ReporteControlCuchilloPreparacionCabeceraPartial(DateTime fechaDesde, DateTime fechaHasta,int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                clsDClasificador = new clsDClasificador();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();
                if (poTurno != null)
                {
                    ViewBag.Turno = poTurno;
                }
                var poCloroCisterna = clsDControlCuchillosPreparacion.ConsultarcabeceraFechaID(fechaDesde, fechaHasta, false,op);
                if (poCloroCisterna != null)
                {
                    return PartialView(poCloroCisterna);
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

        //-------------------------------------------BANDEJA  CUCHILLO PREPARACION------------------------------------------------------
        [Authorize]
        public ActionResult BandejaCuchilloPreparacion()
        {
            try
            {
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.DateRangePicker = "1";
                ViewBag.dataTableJS = "1";
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

        public ActionResult BandejaCuchilloPreparacionPartial(DateTime fechaDesde, DateTime fechaHasta, bool estado, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                var cabeceraTabla = clsDControlCuchillosPreparacion.ConsultarcabeceraFechaID(fechaDesde, fechaHasta, estado,op);
                clsDClasificador = new clsDClasificador();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();
                if (poTurno != null)
                {
                    ViewBag.Turno = poTurno;
                }
                if (cabeceraTabla != null)
                {
                    return PartialView(cabeceraTabla);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AprobarPendiente(CC_CUCHILLOS_PREPARACION_CTRL model, bool siAprobar = false)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(model.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.UsuarioIngresoLog = lsUsuario[0];               
                    var valor = clsDControlCuchillosPreparacion.GuardarModificarControlCuchilloPreparacion(model, siAprobar);
                    if (valor == 0)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else if (valor == 1) { return Json("1", JsonRequestBehavior.AllowGet); }
                    else return Json("2", JsonRequestBehavior.AllowGet);                
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