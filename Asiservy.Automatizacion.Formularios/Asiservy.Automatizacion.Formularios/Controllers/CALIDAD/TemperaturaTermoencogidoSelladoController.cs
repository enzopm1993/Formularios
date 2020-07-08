using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Net;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.TemperaturaTermoencogidoSellado;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class TemperaturaTermoencogidoSelladoController : Controller
    {
        clsDLogin clsLogin { get; set; } = null;
        clsDPeriodo clsDPeriodo { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        public clsDReporte ClsDReporte { get; set; } = null;
        clsDTemperaturaTermoencogidoSellado clsDTemperaturaTermoencogidoSellado { get; set; } = null;
        string[] lsUsuario { get; set; }=null;
        public ActionResult ControlTermoencogidoSellado()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.MascaraInput = "1";
                ViewBag.DateTimePicker = "1";
                clsLogin = new clsDLogin();
                lsUsuario = User.Identity.Name.Split('_');
                var usuarioOpcion = clsLogin.ValidarPermisoOpcion(lsUsuario[1], "ReporteTermoencogidoSellado");
                if (usuarioOpcion)
                {
                    ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ReporteTermoencogidoSellado";
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

        //PartialView
        public ActionResult ControlTermoencogidoSelladoPartial(DateTime fechaDesde, DateTime fechaHasta, int idCabecera, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();
                var detalleTabla = clsDTemperaturaTermoencogidoSellado.ConsultarTermoencogidoSelladoDetalle(fechaDesde, fechaHasta, idCabecera, op);
                if (detalleTabla != null)
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

        //---------------------------------------------------CONTROL CABECERA-----------------------------------------------
        public JsonResult ConsultarTermoencogidoSellado(DateTime fechaControl, string turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();
                var poCloroCisterna = clsDTemperaturaTermoencogidoSellado.ConsultarCabeceraTurno(fechaControl, turno);
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

        [HttpPost]
        public JsonResult GuardarModificarTermoencogidoSellado(CC_TEMPERATURA_TERMOENCOGIDO_SELLADO model, bool siAprobar = false)
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
                clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                if (model.Id != 0 && !siAprobar)
                {
                    var estadoReporte = clsDTemperaturaTermoencogidoSellado.ConsultarEstadoReporte(model.Id);
                    if (estadoReporte.EstadoReporte)
                    {
                        return Json("4", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                    }
                }

                var valor = clsDTemperaturaTermoencogidoSellado.GuardarModificarTermoencogidoSellado(model, siAprobar);
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
                return Json("5", JsonRequestBehavior.AllowGet);//TURNO EXISTE
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
        public JsonResult EliminarTermoencogidoSellado(CC_TEMPERATURA_TERMOENCOGIDO_SELLADO model)
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
                clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor = clsDTemperaturaTermoencogidoSellado.EliminarTermoencogidoSellado(model);
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

        //------------------------------------------------ CONTROL DETALLE-----------------------------------------------------
        [HttpPost]
        public JsonResult GuardarModificarTermoencogidoSelladoDetalle(CC_TEMPERATURA_TERMOENCOGIDO_SELLADO_DETALLE model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
               
                clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                //PRUEBO SI EL ESTADOREPORTE ES APROBADO, SI ES APROBADO NO GUARDO EL DETALLE CASO CONTRARIO SI
                var consultarEstadoReporte = clsDTemperaturaTermoencogidoSellado.ConsultarEstadoReporte(model.IdCabecera);
                clsDPeriodo = new clsDPeriodo();
                if (consultarEstadoReporte != null)
                {
                    bool periodo = clsDPeriodo.ValidaFechaPeriodo(consultarEstadoReporte.Fecha);
                    if (!periodo)
                    {
                        return Json("100", JsonRequestBehavior.AllowGet);
                    }
                    if (!consultarEstadoReporte.EstadoReporte)
                    {
                        var valor = clsDTemperaturaTermoencogidoSellado.GuardarModificarTermoencogidoSelladoDetalle(model);
                        if (valor == 0)
                        {
                            return Json("0", JsonRequestBehavior.AllowGet);
                        }
                        else return Json("1", JsonRequestBehavior.AllowGet);
                    }
                    else { return Json("2", JsonRequestBehavior.AllowGet); }
                }else  { return Json("5", JsonRequestBehavior.AllowGet); }

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
        public ActionResult EliminarTermoencogidoSelladoDetalle(CC_TEMPERATURA_TERMOENCOGIDO_SELLADO_DETALLE model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                clsDPeriodo = new clsDPeriodo();
                var estadoReporte = clsDTemperaturaTermoencogidoSellado.ConsultarEstadoReporte(model.IdCabecera);
                if (estadoReporte!=null)
                {
                    bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.Fecha);
                    if (!periodo)
                    {
                        return Json("100", JsonRequestBehavior.AllowGet);
                    }
                    var valor = clsDTemperaturaTermoencogidoSellado.EliminarTermoencogidoSelladoDetalle(model);
                    if (valor == 0)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else return Json("1", JsonRequestBehavior.AllowGet);
                }
                else return Json("50", JsonRequestBehavior.AllowGet);

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

        //------------------------------------------------REPORTE-----------------------------------------------------
        public ActionResult ReporteTermoencogidoSellado()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsLogin = new clsDLogin();
                lsUsuario = User.Identity.Name.Split('_');
                var usuarioOpcion = clsLogin.ValidarPermisoOpcion(lsUsuario[1], "ControlTermoencogidoSellado");
                if (usuarioOpcion)
                {
                    ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ControlTermoencogidoSellado";
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

        public ActionResult ReporteTermoencogidoSelladoPartial(DateTime fechaDesde, DateTime fechaHasta, int id, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();
                var poCloroCisterna = clsDTemperaturaTermoencogidoSellado.ConsultarTermoencogidoSelladoDetalle(fechaDesde, fechaHasta, id, op);
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

        public ActionResult ReporteTermoencogidoSelladoCabeceraPartial(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDClasificador = new clsDClasificador();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();
                if (poTurno != null)
                {
                    ViewBag.Turno = poTurno;
                }
                clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();
                var poCloroCisterna = clsDTemperaturaTermoencogidoSellado.ReporteConsultarcabecera(fechaDesde, fechaHasta);
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

        //------------------------------------------------BANDEJA-----------------------------------------------------
        public ActionResult BandejaTermoencogidoSellado()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.DateTimePicker = "1";
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

        public JsonResult ConsultarBandejaTermoencogidoSellado(DateTime fechaDesde, DateTime fechaHasta, bool estadoReporte)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();

                var poCloroCisterna = clsDTemperaturaTermoencogidoSellado.ConsultarBadejaEstado(fechaDesde, fechaHasta, estadoReporte);
              
                    //List<sp_Control_Termoencogido_Sellado_Detalle> listaNueva = new List<sp_Control_Termoencogido_Sellado_Detalle>();
                    //sp_Control_Termoencogido_Sellado_Detalle objNuevo;
                    //var listaFecha = (from ssi in poCloroCisterna
                    //                  group ssi by new { ssi.Fecha, ssi.ObservacionCAB } into g
                    //                  select new { g.Key.Fecha }).ToList();//agrupo por fecha para poder saber el total de las filas 

                    //foreach (var item in listaFecha)
                    //{
                    //    objNuevo = new sp_Control_Termoencogido_Sellado_Detalle();
                    //    var stringLista = (from x in poCloroCisterna
                    //                       where x.Fecha == item.Fecha
                    //                       select new { x.Fecha, x.ObservacionCAB, x.EstadoReporte, x.UsuarioIngresoLog, x.Id, x.IdCabecera, x.HoraVerificacion,
                    //                       x.Temperatura, x.CorrectoSellado, x.Observacion, x.EstadoRegistroCAB}).First();
                    //    objNuevo.Fecha = stringLista.Fecha;
                    //    objNuevo.ObservacionCAB = stringLista.ObservacionCAB;
                    //    objNuevo.EstadoReporte = stringLista.EstadoReporte;
                    //    objNuevo.UsuarioIngresoLog = stringLista.UsuarioIngresoLog;
                    //    objNuevo.Id = stringLista.Id;
                    //    objNuevo.IdCabecera = stringLista.IdCabecera;
                    //    objNuevo.HoraVerificacion = stringLista.HoraVerificacion;
                    //    objNuevo.Temperatura = stringLista.Temperatura;
                    //    objNuevo.CorrectoSellado = stringLista.CorrectoSellado;
                    //    objNuevo.Observacion = stringLista.Observacion;
                    //    objNuevo.EstadoRegistroCAB = stringLista.EstadoRegistroCAB;
                    //    listaNueva.Add(objNuevo);
                    //}
                    clsDClasificador = new clsDClasificador();
                    var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();

                    List<dynamic> turno = new List<dynamic>();
                    string tr = "";
                    if (poCloroCisterna != null)
                    {
                        foreach (var item in poCloroCisterna)
                        {
                            tr = (from c in poTurno
                                  where c.Codigo == item.Turno
                                  select c.Descripcion).FirstOrDefault();
                            turno.Add(new { item.EstadoReporte, item.Fecha, item.Id, item.Observacion, item.Turno, item.UsuarioIngresoLog, item.UsuarioModificacionLog, tr });
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

        public ActionResult BandejaTermoencogidoSelladoPartial(DateTime fechaDesde, DateTime fechaHasta,int id, int opcion)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();
                var poCloroCisterna = clsDTemperaturaTermoencogidoSellado.ConsultarTermoencogidoSelladoDetalle(fechaDesde, fechaHasta,id, opcion);
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