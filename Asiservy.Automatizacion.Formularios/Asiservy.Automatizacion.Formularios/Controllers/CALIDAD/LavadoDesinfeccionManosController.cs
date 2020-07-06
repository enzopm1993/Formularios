using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.LavadoDesinfeccionManos;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity.Validation;
using System.Net;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class LavadoDesinfeccionManosController : Controller
    {
        clsDPeriodo clsDPeriodo { get; set; } = null;
        clsDLogin clsLogin { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        public clsDReporte ClsDReporte { get; set; } = null;
        clsDLavadoDesinfeccionManos clsDLavadoDesinfeccionManos { get; set; } = null;
        string[] lsUsuario { get; set; } =null;
        //-----------------------------------------------------INICIALIZAR VISTA----------------------------------------------------------------
        public ActionResult ControlLavadoDesinfeccionManos()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDClasificador = new clsDClasificador();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();
                ViewBag.DateTimePicker = "1";
                clsLogin = new clsDLogin();
                var usuarioOpcion = clsLogin.ValidarPermisoOpcion(lsUsuario[1], "ReporteLavadoDesinfeccionManos");
                if (usuarioOpcion)
                {
                    ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ReporteLavadoDesinfeccionManos";
                }
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

        //VISTA USADA PARA ARMAR LA TABLA DEL INGRESO DEL DETALLE
        public ActionResult IngresoLavadoDesinfeccionManosDetallePartial()
        {
            try
            {
                clsDClasificador clasificador = new clsDClasificador();
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLavadoDesinfeccionManos = new clsDLavadoDesinfeccionManos();                
                var lineas = clasificador.ConsultarClasificador(clsAtributos.IdCodigoLineaLavadoDesinfeccionManos).ToList();
                ViewBag.Lineas = lineas;
                if (lineas != null)
                {
                    return PartialView(lineas); 
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
        //Esta vista sirve para mostrar el detalle ya que no puedo usar la vista IngresoLavadoDesinfeccionManosDetallePartial por que ya esta configurada para ingresar
        public ActionResult LavadoDesinfeccionManosDetallePartial(int IdDesinfeccionManos, int opcion)
        {
            try
            {
                clsDClasificador clasificador = new clsDClasificador();
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLavadoDesinfeccionManos = new clsDLavadoDesinfeccionManos();
                var detalleTabla = clsDLavadoDesinfeccionManos.ConsultarControlLavadoDesinfeccionManosDetalle(IdDesinfeccionManos, opcion);
                var cabeceraTable = clasificador.ConsultarClasificador(clsAtributos.IdCodigoLineaLavadoDesinfeccionManos).ToList();
                ViewBag.cabeceraTable = cabeceraTable;
                if (cabeceraTable != null)
                {
                    return PartialView(detalleTabla);
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
        //SE USA SOLO PARA EL VIEW DEL REPORTE
        public ActionResult ReporteDesinfeccionManosDetallePartial(DateTime fechaDesde, DateTime fechaHasta, int op, int idDesinfeccionManos=0)
        {
            try
            {
                clsDClasificador clasificador = new clsDClasificador();
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLavadoDesinfeccionManos = new clsDLavadoDesinfeccionManos();
                var detalleTabla = clsDLavadoDesinfeccionManos.ReporteControlLavadoDesinfeccion(fechaDesde, fechaHasta, idDesinfeccionManos, op);
                var cabeceraTable = clasificador.ConsultarClasificador(clsAtributos.IdCodigoLineaLavadoDesinfeccionManos).ToList();
                ViewBag.cabeceraTable = cabeceraTable;
                if (cabeceraTable != null)
                {
                    return PartialView(detalleTabla);
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

        public ActionResult ReporteDesinfeccionManosDetalleCabeceraPartial(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLavadoDesinfeccionManos = new clsDLavadoDesinfeccionManos();
                var detalleTabla = clsDLavadoDesinfeccionManos.ReporteConsultarcabecera(fechaDesde, fechaHasta);
                clsDClasificador = new clsDClasificador();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();
                if (poTurno != null)
                {
                    ViewBag.Turno = poTurno;
                }
                if (detalleTabla.Count != 0)
                {
                    return PartialView(detalleTabla);
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
        //-------------------------------------------------CONTROL LAVADO Y DESINFECTADO DE MANOS CABECERA-------------------------------------------
        public ActionResult ConsultarControlLavadoDesinfeccionManos(int turno, DateTime fechaControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLavadoDesinfeccionManos = new clsDLavadoDesinfeccionManos();
                var poCloroCisterna = clsDLavadoDesinfeccionManos.ConsultarCabeceraTurno(turno, fechaControl);
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
        
        public ActionResult GuardarModificarControlLavadoDesinfeccionManos(CC_CONTROL_LAVADO_DESINFECCION_MANOS model, bool siAprobar = false)
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
                clsDLavadoDesinfeccionManos = new clsDLavadoDesinfeccionManos();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];

                var valor = clsDLavadoDesinfeccionManos.GuardarModificarControlLavadoDesinfeccionManos(model, siAprobar);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 1) { return Json("1", JsonRequestBehavior.AllowGet); }
                else if (valor == 2) return Json("2", JsonRequestBehavior.AllowGet);
                else return Json("3", JsonRequestBehavior.AllowGet);
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

        public ActionResult EliminarControlLavadoDesinfeccionManos(CC_CONTROL_LAVADO_DESINFECCION_MANOS model)
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
                clsDLavadoDesinfeccionManos = new clsDLavadoDesinfeccionManos();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor = clsDLavadoDesinfeccionManos.EliminarControlLavadoDesinfeccionManos(model);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else return Json("1", JsonRequestBehavior.AllowGet); ;
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

        //-------------------------------------------------CONTROL LAVADO Y DESINFECTADO DE MANOS DETALLE---------------------------------------------        

        public JsonResult GuardarModificarControlLavadoDesinfeccionManosDetalle(List<CC_CONTROL_LAVADO_DESINFECCION_MANOS_DETALLE> model)
        {
            int valor=5;
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLavadoDesinfeccionManos = new clsDLavadoDesinfeccionManos();
                var obtenerPrimerRegistro = (from x in model
                             select new {x.IdDesinfeccionManos}).FirstOrDefault();
                var estadoReporte = clsDLavadoDesinfeccionManos.ConsultarEstadoReporte(obtenerPrimerRegistro.IdDesinfeccionManos);
                
                if (estadoReporte.EstadoReporte)
                {
                    return Json("2", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                foreach (var item in model)
                {
                    clsDLavadoDesinfeccionManos = new clsDLavadoDesinfeccionManos();
                    item.FechaIngresoLog = DateTime.Now;
                    item.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    item.TerminalIngresoLog = Request.UserHostAddress;
                    item.UsuarioIngresoLog = lsUsuario[0];
                    valor = clsDLavadoDesinfeccionManos.GuardarModificarControlLavadoDesinfeccionManosDetalle(item);
                }
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else return Json("1", JsonRequestBehavior.AllowGet);

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

        public ActionResult EliminarLavadoDesinfeccionManosDetalle(List<CC_CONTROL_LAVADO_DESINFECCION_MANOS_DETALLE> model)
        {
            int valor = 5;
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                clsDLavadoDesinfeccionManos = new clsDLavadoDesinfeccionManos();
                var estadoReporte = clsDLavadoDesinfeccionManos.ConsultarEstadoReporte(model[0].IdDesinfeccionManos);

                if (estadoReporte.EstadoReporte)
                {
                    return Json("2", JsonRequestBehavior.AllowGet);
                }
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                foreach (var item in model)
                {
                    clsDLavadoDesinfeccionManos = new clsDLavadoDesinfeccionManos();
                    item.FechaIngresoLog = DateTime.Now;
                    item.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    item.TerminalIngresoLog = Request.UserHostAddress;
                    item.UsuarioIngresoLog = lsUsuario[0];
                    valor = clsDLavadoDesinfeccionManos.EliminarLavadoDesinfeccionManosDetalle(item);                    
                }
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else{return Json("1", JsonRequestBehavior.AllowGet); }               
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

        //-------------------------------------------------REPORTE LAVADO Y DESINFECCION DE MANOS----------------------------------------------------
        public ActionResult ReporteLavadoDesinfeccionManos()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                ViewBag.DateRangePicker = "1";
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsLogin = new clsDLogin();
                var usuarioOpcion = clsLogin.ValidarPermisoOpcion(lsUsuario[1], "ControlLavadoDesinfeccionManos");
                if (usuarioOpcion)
                {
                    ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ControlLavadoDesinfeccionManos";
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

        //-------------------------------------------------BANDEJA LAVADO Y DESINFECCION DE MANOS----------------------------------------------------
        public ActionResult BandejaLavadoDesinfeccionManos()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.DateRangePicker = "1";
               
                return View();
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

        public JsonResult BandejaLavadoDesinfeccionManosJson(DateTime fechaDesde, DateTime fechaHasta, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLavadoDesinfeccionManos = new clsDLavadoDesinfeccionManos();
                var listObject = clsDLavadoDesinfeccionManos.ConsultarControlLavadoDesinfeccionManos(fechaDesde, fechaHasta, op);
                clsDClasificador = new clsDClasificador();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();
                
                List<dynamic> turno = new List<dynamic>();
                string tr = "";
                if (listObject != null)
                {
                    foreach (var item in listObject)
                    {                        
                        tr = (from c in poTurno
                              where c.Codigo == item.Turno
                              select c.Descripcion).FirstOrDefault();
                        turno.Add(new { item.EstadoReporte,item.Fecha,item.Hora,item.IdDesinfeccionManos,item.Observacion,item.Turno, item.UsuarioIngresoLog, item.UsuarioModificacionLog,tr});
                    }
                    return Json(turno, JsonRequestBehavior.AllowGet);

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

        public ActionResult BandejaLavadoDesinfeccionManosAprobarPartial(int IdDesinfeccionManos, int opcion)
        {
            try
            {
                clsDClasificador clasificador = new clsDClasificador();
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLavadoDesinfeccionManos = new clsDLavadoDesinfeccionManos();
                var detalleTabla = clsDLavadoDesinfeccionManos.ConsultarControlLavadoDesinfeccionManosDetalle(IdDesinfeccionManos, opcion);
                var cabeceraTable = clasificador.ConsultarClasificador(clsAtributos.IdCodigoLineaLavadoDesinfeccionManos).ToList();
                ViewBag.cabeceraTable = cabeceraTable;
                if (detalleTabla.Count != 0)
                {
                    return PartialView(detalleTabla);
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