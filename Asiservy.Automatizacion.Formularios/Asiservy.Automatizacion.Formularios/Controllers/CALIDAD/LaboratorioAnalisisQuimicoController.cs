using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.LaboratorioAnalisisQuimico;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity.Validation;
using System.Net;
using System;
using System.Linq;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;
using System.Collections.Generic;
using System.Web;
using System.IO;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.Mantenimientos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ProyeccionProgramacion;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class LaboratorioAnalisisQuimicoController : Controller
    {
        ClsDParametrosLaboratorio ClsDParametrosLaboratorio { get; set; } = null;
        clsDPeriodo clsDPeriodo { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDLogin clsDLogin { get; set; } = null;
        public clsDReporte ClsDReporte { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        ClsDLaboratorioAnalisisQuimico ClsDLaboratorioAnalisisQuimico { get; set; } = null;
        clsDApiProduccion clsDApiProduccion { get; set; } = null;
        string[] lsUsuario { get; set; } = null;

       
        #region CONTROL
        [Authorize]
        public ActionResult ControlAnalisisQuimico()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.JqueryRotate = "1";
                ViewBag.select2 = "1";
                ViewBag.MascaraInput = "1";
                ViewBag.usuarioAnalista = lsUsuario[0];
                ClsDParametrosLaboratorio = new ClsDParametrosLaboratorio();
                var parametros = ClsDParametrosLaboratorio.ConsultarParametrosFormularios(clsAtributos.codFormPrecoccion).Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
               
                ViewBag.listaParametros = parametros;
                clsDLogin = new clsDLogin();
                var usuarioOpcion = clsDLogin.ValidarPermisoOpcion(lsUsuario[1], "ReporteAnalisisQuimico");
                if (usuarioOpcion)
                {
                    ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ReporteAnalisisQuimico";
                }
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
        public ActionResult ControlAnalisisQuimicoPartial(DateTime fechaProduccion, DateTime fechaParadaCocina, int idAnalisis=0, string turno="1", int op=1)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');           
                if (!User.Identity.IsAuthenticated){
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ViewBag.Turno = turno;
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                ClsDParametrosLaboratorio = new ClsDParametrosLaboratorio();
                clsDApiProduccion = new clsDApiProduccion();
                var parametros = ClsDParametrosLaboratorio.ConsultarParametrosFormularios(clsAtributos.codFormPrecoccion).Where(x=> x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
                ViewBag.listaParametros = parametros ;
                ViewBag.ConsultarDetalleDia = ClsDLaboratorioAnalisisQuimico.ConsultarDetalleDia(fechaProduccion, turno, op);                
                ViewBag.BarcosAsignados = ClsDLaboratorioAnalisisQuimico.ConsultarBarcoFecha(fechaParadaCocina);
                var listaParadasCocinas = clsDApiProduccion.ParadasCocinasPorFecha(fechaParadaCocina);
                if (listaParadasCocinas != null)
                {
                    return PartialView(listaParadasCocinas);
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
        public JsonResult ConsultarDetalle(int idAnalisis)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var listaDetalle = ClsDLaboratorioAnalisisQuimico.ConsultarDetalle(idAnalisis);
                if (listaDetalle.Any())
                {
                    return Json(listaDetalle, JsonRequestBehavior.AllowGet);
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
        public ActionResult ControlElementoPartial(int idAnalisis, int idParametro=0, int idAnalisisDetalle=0)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDParametrosLaboratorio = new ClsDParametrosLaboratorio();
                var parametros = ClsDParametrosLaboratorio.ConsultarParametrosFormularios(clsAtributos.codFormPrecoccion).Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                ViewBag.listaParametros = parametros;
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var listaDetalle = ClsDLaboratorioAnalisisQuimico.ConsultarElemento(idAnalisis, idParametro, idAnalisisDetalle);
                
                if (listaDetalle.Any())
                {
                    ViewBag.ListaElementos = listaDetalle;
                    return PartialView(null);
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
        [HttpPost]
        public JsonResult GuardarModificarAnalisisQuimico(CC_ANALISIS_QUIMICO_PRECOCCION_CTRL model, int siAprobar)
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
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                if (model.IdAnalisis != 0 && siAprobar == 0)
                {
                    var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(model.IdAnalisis, DateTime.MinValue);
                    if (estadoReporte.EstadoReporte)
                    {
                        return Json("4", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                    }
                }
                var valor = ClsDLaboratorioAnalisisQuimico.GuardarModificarAnalisisQuimico(model, siAprobar);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 1)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 2) { return Json("2", JsonRequestBehavior.AllowGet); }
                else if (valor == 3) return Json("3", JsonRequestBehavior.AllowGet);//ERROR DE FECHA
                else return Json("5", JsonRequestBehavior.AllowGet);
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
        public JsonResult EliminarAnalisisQuimico(CC_ANALISIS_QUIMICO_PRECOCCION_CTRL model)
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
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(model.IdAnalisis, DateTime.MinValue);
                if (!estadoReporte.EstadoReporte)
                {
                    var valor = ClsDLaboratorioAnalisisQuimico.EliminarAnalisisQuimico(model);
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
        public JsonResult ConsultarEstadoReporte(int idAnalisis=0)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var lista = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(idAnalisis, DateTime.MinValue);
                if (lista != null)
                {
                    return Json(lista, JsonRequestBehavior.AllowGet);
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
        public JsonResult ConsultarCabeceraTurno(DateTime fechaControl, string turno = null)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var lista = ClsDLaboratorioAnalisisQuimico.ConsultarCabeceraTurno(turno, fechaControl);
                if (lista != null)
                {                    
                    return Json(lista, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public JsonResult GuardarModificarDetalle(CC_ANALISIS_QUIMICO_PRECOCCION_DET model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }               
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(model.IdAnalisis,DateTime.MinValue);
                if (estadoReporte==null)
                {
                    return Json("10", JsonRequestBehavior.AllowGet);//IdAnalisis NO ENCONTRADO
                }
                if (estadoReporte.EstadoReporte)
                {
                    return Json("3", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                var valor = 0;
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                valor = ClsDLaboratorioAnalisisQuimico.GuardarModificarDetalle(model);

                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
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
        public JsonResult GuardarModificarElemento(CC_ANALISIS_QUIMICO_PRECOCCION_ELEMENTOS model,int idAnalisis, CC_ANALISIS_QUIMICO_PRECOCCION_DET detalle)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
               
                var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(idAnalisis, DateTime.MinValue);
                if (estadoReporte == null)
                {
                    return Json("10", JsonRequestBehavior.AllowGet);//IdAnalisis NO ENCONTRADO
                }
                if (estadoReporte.EstadoReporte)
                {
                    return Json("3", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                var existeDetalle = ClsDLaboratorioAnalisisQuimico.ConsultarDetalleExiste(model.IdAnalisisDetalle);
                if (existeDetalle == null)
                {
                    detalle.FechaIngresoLog = DateTime.Now;
                    detalle.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    detalle.TerminalIngresoLog = Request.UserHostAddress;
                    detalle.UsuarioIngresoLog = lsUsuario[0];
                    ClsDLaboratorioAnalisisQuimico.GuardarModificarDetalle(detalle);
                    model.IdAnalisisDetalle = detalle.IdAnalisisDetalle;
                }
                var valor = 0;
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                valor = ClsDLaboratorioAnalisisQuimico.GuardarModificarElemento(model);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
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
        public JsonResult EliminarDetalle(CC_ANALISIS_QUIMICO_PRECOCCION_DET model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
               
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(model.IdAnalisis, DateTime.MinValue);
                if (estadoReporte == null)
                {
                    return Json("3", JsonRequestBehavior.AllowGet);//IdAnalisis NO ENCONTRADO
                }
                if (estadoReporte.EstadoReporte)
                {
                    return Json("2", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                var valor = 0;
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                valor = ClsDLaboratorioAnalisisQuimico.EliminarDetalle(model);

                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
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
        public JsonResult EliminarElemento(CC_ANALISIS_QUIMICO_PRECOCCION_ELEMENTOS model, int idAnalisis)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(idAnalisis, DateTime.MinValue);
                if (estadoReporte == null)
                {
                    return Json("3", JsonRequestBehavior.AllowGet);//IdAnalisis NO ENCONTRADO
                }
                if (estadoReporte.EstadoReporte)
                {
                    return Json("2", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                var valor = 0;
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                valor = ClsDLaboratorioAnalisisQuimico.EliminarElemento(model);

                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
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
        public JsonResult GuardarFoto(CC_ANALISIS_QUIMICO_PRECOCCION_FOTO model, HttpPostedFileBase dataImg, CC_ANALISIS_QUIMICO_PRECOCCION_DET detalle)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(detalle.IdAnalisis, DateTime.MinValue);
                if (estadoReporte.EstadoReporte)
                {
                    return Json("3", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                string path = string.Empty;
                string NombreImg = string.Empty;
                if (dataImg != null)
                {
                    decimal mb = 1024 * 1024 * 5;//bytes to Mb; max 5Mb
                    var supportedTypes = new[] { "jpg", "jpeg", "PNG","png" };
                    var fileExt = Path.GetExtension(dataImg.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        return Json("4", JsonRequestBehavior.AllowGet);//NO ES IMG
                    }
                    else if (dataImg.ContentLength > (mb))
                    {
                        return Json(dataImg.ContentLength, JsonRequestBehavior.AllowGet);//SOBREPASA EL LIMITE PERMITIDO dataImg.ContentLength=bytes convert to Mb
                    }
                    path = Server.MapPath(clsAtributos.UrlImagen + this.ControllerContext.RouteData.Values["controller"].ToString() + "/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var date = DateTime.Now;
                    long n = long.Parse(date.ToString("yyyyMMddHHmmss"));
                    var ext2 = dataImg.FileName.Split('.');
                    var cont = ext2.Length;
                    NombreImg = "fotoLaboratorio" + n.ToString() + "." + ext2[cont - 1];
                    model.RutaFoto = NombreImg;
                }
                var existeDetalle = ClsDLaboratorioAnalisisQuimico.ConsultarDetalleExiste(model.IdAnalisisDetalle);
                if (existeDetalle == null)
                {
                    detalle.FechaIngresoLog = DateTime.Now;
                    detalle.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    detalle.TerminalIngresoLog = Request.UserHostAddress;
                    detalle.UsuarioIngresoLog = lsUsuario[0];
                    ClsDLaboratorioAnalisisQuimico.GuardarModificarDetalle(detalle);
                    model.IdAnalisisDetalle = detalle.IdAnalisisDetalle;
                }
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor = ClsDLaboratorioAnalisisQuimico.GuardarModificarFoto(model);
                if (dataImg != null)
                {
                    dataImg.SaveAs(path + Path.GetFileName(NombreImg));
                }
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
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
        public ActionResult VerCrearImagenPartial(int idAnalisisDetalle)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var lista = ClsDLaboratorioAnalisisQuimico.ConsultarImagen(idAnalisisDetalle);
                if (lista.Count != 0)
                {
                    ViewBag.Path = clsAtributos.UrlImagen.Replace("~", "..") + this.ControllerContext.RouteData.Values["controller"].ToString()+"/";
                    return PartialView(lista);
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
        [HttpPost]
        public JsonResult EliminarImagen(CC_ANALISIS_QUIMICO_PRECOCCION_FOTO model, int idAnalisis)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(idAnalisis, DateTime.MinValue);
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                if (estadoReporte.EstadoReporte)
                {
                    return Json("2", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                }                               
                var valor = 0;
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                valor = ClsDLaboratorioAnalisisQuimico.EliminarImagen(model);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
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
        #endregion

        #region BANDEJA
        [Authorize]
        public ActionResult BandejaAnalisisQuimico()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.JqueryRotate = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
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

        public ActionResult BandejaAnalisisQuimicoPartial(bool estadoReporte, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var lista = ClsDLaboratorioAnalisisQuimico.ConsultarBadejaEstado(fechaDesde, fechaHasta, estadoReporte);
                clsDClasificador = new clsDClasificador();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();
                if (poTurno != null)
                {
                    ViewBag.Turno = poTurno;
                }
                if (lista.Any())
                {
                    return PartialView(lista);
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
        
        public ActionResult BandejaAnalisisQuimicoAprobarPartial(DateTime fechaControl, DateTime fechaAsignada, string turno, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(fechaControl);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                clsDApiProduccion = new clsDApiProduccion();
                ViewBag.ListaParadasCocinas = clsDApiProduccion.ParadasCocinasPorFecha(fechaAsignada);
                ViewBag.Turno = turno;
                ViewBag.BarcosAsignados = ClsDLaboratorioAnalisisQuimico.ConsultarBarcoFecha(fechaAsignada);                
                var lista = ClsDLaboratorioAnalisisQuimico.ConsultarDetalleDia(fechaControl, turno, op);
                if (lista.Count != 0)
                {
                    ViewBag.Path = clsAtributos.UrlImagen.Replace("~", "..") + this.ControllerContext.RouteData.Values["controller"].ToString()+"/";
                    return PartialView(lista);
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
        #endregion

        #region REPORTE
        [Authorize]
        public ActionResult ReporteAnalisisQuimico()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.JqueryRotate = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDLogin = new clsDLogin();
                lsUsuario = User.Identity.Name.Split('_');
                var usuarioOpcion = clsDLogin.ValidarPermisoOpcion(lsUsuario[1], "ControlAnalisisQuimico");
                if (usuarioOpcion)
                {
                    ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ControlAnalisisQuimico";
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
        public ActionResult ReporteAnalisisQuimicoPartial(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var lista = ClsDLaboratorioAnalisisQuimico.ConsultarReporteRangoFecha(fechaDesde, fechaHasta);
                clsDClasificador = new clsDClasificador();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();
                if (poTurno != null)
                {
                    ViewBag.Turno = poTurno;
                }
                if (lista.Any())
                {
                    return PartialView(lista);
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
        public ActionResult ReporteAnalisisQuimicoDetallePartial(DateTime fechaControl, DateTime fechaAsignada, string turno, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var lista = ClsDLaboratorioAnalisisQuimico.ConsultarDetalleDia(fechaControl, turno, op);
                if (lista.Count != 0)
                {
                    clsDApiProduccion = new clsDApiProduccion();
                    ViewBag.ListaParadasCocinas = clsDApiProduccion.ParadasCocinasPorFecha(fechaAsignada);
                    ViewBag.Turno = turno;
                    ViewBag.BarcosAsignados = ClsDLaboratorioAnalisisQuimico.ConsultarBarcoFecha(fechaAsignada);
                    ViewBag.Path = clsAtributos.UrlImagen.Replace("~", "..")+ this.ControllerContext.RouteData.Values["controller"].ToString() + "/";
                    return PartialView(lista);
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