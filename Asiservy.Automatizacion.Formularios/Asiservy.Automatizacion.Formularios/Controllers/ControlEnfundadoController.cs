﻿using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ControlEnfundado;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ControlEnfundadoController : Controller
    {
        string[] Usuario = null;
        clsDError clsDError = null;
        clsDEmpleado clsDEmpleado = null;
        clsDClasificador clsDClasificador = null;
        clsDControlEnfundado clsDControlEnfundado = null;
        clsDApiProduccion clsDApiProduccion = null;

        // GET: ControlEnfundado
        [Authorize]
        public ActionResult ControlEnfundado()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDApiProduccion = new clsDApiProduccion();
                Usuario = User.Identity.Name.Split('_');
                clsDClasificador = new clsDClasificador();
                clsDEmpleado = new clsDEmpleado();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                var EspecificacionFunda = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo=clsAtributos.CodigoGrupoFunda, EstadoRegistro= clsAtributos.EstadoRegistroActivo});
                var Lotes = clsDApiProduccion.ConsultarLotesPorFecha(DateTime.Now);
                ViewBag.Lotes = Lotes;
                ViewBag.Linea = Empleado.LINEA;
                ViewBag.EspecificacionFunda = EspecificacionFunda;

                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return RedirectToAction("Home", "Home");
            }

        }

        [Authorize]
        public ActionResult ControlEnfundadoPartial(DateTime Fecha)
        {
            try
            {
                clsDControlEnfundado = new clsDControlEnfundado();
                var model = clsDControlEnfundado.ConsultaControlEnfundado(Fecha);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        public ActionResult ControlEnfundadoDetallePartial(int Id)
        {
            try
            {
                clsDControlEnfundado = new clsDControlEnfundado();
                var model = clsDControlEnfundado.ConsultaControlEnfundadoDetalle(Id);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        [HttpPost]
        public ActionResult GenerarControlEnfundado(CONTROL_ENFUNDADO model)
        {
            try
            {
               
                clsDControlEnfundado = new clsDControlEnfundado();
                Usuario = User.Identity.Name.Split('_');
                model.UsuarioIngresoLog = Usuario[0];
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                int id = clsDControlEnfundado.GenerarControlHueso(model);
                return Json(id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize]
        [HttpPost]
        public ActionResult GuardarControlEnfundado(CONTROL_ENFUNDADO_DETALLE detalle)
        {
            try
            {
                if (detalle == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json("Parametros Incompletos", JsonRequestBehavior.AllowGet);
                }

                Usuario = User.Identity.Name.Split('_');
                clsDControlEnfundado = new clsDControlEnfundado();
                detalle.UsuarioIngresoLog = Usuario[0];
                detalle.TerminalIngresoLog = Request.UserHostAddress;
                detalle.FechaIngresoLog = DateTime.Now;
                detalle.EstadoRegistro = clsAtributos.EstadoRegistroActivo;               
                var respuesta = clsDControlEnfundado.GuardarModificarControlEnfundado(detalle);

                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                //SetErrorMessage(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
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
            catch (Exception ex)
            {

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
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