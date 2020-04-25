using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Net;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.TemperaturaTermoencogidoSellado;
using Rotativa;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class TemperaturaTermoencogidoSelladoController : Controller
    {
        clsDError clsDError = null;
        clsDEmpleado clsDEmpleado = null;
        clsDTemperaturaTermoencogidoSellado clsDTemperaturaTermoencogidoSellado = null;
        string[] lsUsuario;
        public ActionResult ControlTermoencogidoSellado()
        {
            try
            {
                ViewBag.dataTableJS = "1";
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
        public JsonResult ConsultarTermoencogidoSellado(DateTime fechaDesde, DateTime fechaHasta, int opcion)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();
                var poCloroCisterna = clsDTemperaturaTermoencogidoSellado.ConsultarTermoencogidoSellado(fechaDesde, fechaHasta,opcion).FirstOrDefault();
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
        public JsonResult GuardarModificarTermoencogidoSellado(CC_TEMPERATURA_TERMOENCOGIDO_SELLADO model)
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
                var valor = clsDTemperaturaTermoencogidoSellado.GuardarModificarTermoencogidoSellado(model);
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
                var probarEstadoReporte = clsDTemperaturaTermoencogidoSellado.ConsultarTermoencogidoSelladoDetalle(Convert.ToDateTime("01-01-2020"), Convert.ToDateTime("01-01-2020"), model.IdCabecera,0);
                if (probarEstadoReporte[0].EstadoReporte==false)
                {
                    var valor = clsDTemperaturaTermoencogidoSellado.GuardarModificarTermoencogidoSelladoDetalle(model);
                    if (valor == 0)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else return Json("1", JsonRequestBehavior.AllowGet);
                }
                else { return Json("2", JsonRequestBehavior.AllowGet); }
               
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
                var valor = clsDTemperaturaTermoencogidoSellado.EliminarTermoencogidoSelladoDetalle(model);
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

        //------------------------------------------------REPORTE-----------------------------------------------------
        public ActionResult ReporteTermoencogidoSellado()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
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

        public ActionResult ReporteTermoencogidoSelladoPartial(DateTime fechaDesde, DateTime fechaHasta, int id, int opcion)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();
                var poCloroCisterna = clsDTemperaturaTermoencogidoSellado.ConsultarTermoencogidoSelladoDetalle(fechaDesde, fechaHasta, id, opcion);
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

        public JsonResult ConsultarBandejaTermoencogidoSellado(DateTime fechaDesde, DateTime fechaHasta, int id, int opcion)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();
                var poCloroCisterna = clsDTemperaturaTermoencogidoSellado.ConsultarTermoencogidoSelladoDetalle(fechaDesde, fechaHasta, id, opcion);
                if (poCloroCisterna != null)
                {
                    List<sp_Control_Termoencogido_Sellado_Detalle> listaNueva = new List<sp_Control_Termoencogido_Sellado_Detalle>();
                    sp_Control_Termoencogido_Sellado_Detalle objNuevo;
                    var listaFecha = (from ssi in poCloroCisterna
                                      group ssi by new { ssi.Fecha, ssi.ObservacionCAB } into g
                                      select new { g.Key.Fecha }).ToList();//agrupo por fecha para poder saber el total de las filas 

                    foreach (var item in listaFecha)
                    {
                        objNuevo = new sp_Control_Termoencogido_Sellado_Detalle();
                        var stringLista = (from x in poCloroCisterna
                                           where x.Fecha == item.Fecha
                                           select new { x.Fecha, x.ObservacionCAB, x.EstadoReporte, x.UsuarioIngresoLog, x.Id, x.IdCabecera, x.HoraVerificacion,
                                           x.Temperatura, x.CorrectoSellado, x.Observacion, x.EstadoRegistroCAB}).First();
                        objNuevo.Fecha = stringLista.Fecha;
                        objNuevo.ObservacionCAB = stringLista.ObservacionCAB;
                        objNuevo.EstadoReporte = stringLista.EstadoReporte;
                        objNuevo.UsuarioIngresoLog = stringLista.UsuarioIngresoLog;
                        objNuevo.Id = stringLista.Id;
                        objNuevo.IdCabecera = stringLista.IdCabecera;
                        objNuevo.HoraVerificacion = stringLista.HoraVerificacion;
                        objNuevo.Temperatura = stringLista.Temperatura;
                        objNuevo.CorrectoSellado = stringLista.CorrectoSellado;
                        objNuevo.Observacion = stringLista.Observacion;
                        objNuevo.EstadoRegistroCAB = stringLista.EstadoRegistroCAB;
                        listaNueva.Add(objNuevo);
                    }
                    return Json(listaNueva, JsonRequestBehavior.AllowGet);

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

        //------------------------------------------------IMPRESION PDF-----------------------------------------------------
        public ActionResult PrintReport(DateTime filtroFechaDesde, DateTime filtroFechaHasta,int id, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    Response.Redirect(Url.Action("Login", "Login"));
                }
                clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();
                var consulta = clsDTemperaturaTermoencogidoSellado.ConsultarTermoencogidoSelladoDetalle(filtroFechaDesde, filtroFechaHasta, id, op);
                var headerPdf = Server.MapPath("~/Views/TemperaturaTermoencogidoSellado/HeaderPdf.html");//ARCHIVO HTML USADO EN LA CABECERA DEL PDF
                ViewBag.filtroFechaDesde = filtroFechaDesde;
                ViewBag.filtroFechaHasta = filtroFechaHasta;
                string customSwitches = string.Format("--header-html  \"{0}\" " +
                            "--header-font-size \"15\" ", headerPdf);
                return new ViewAsPdf("PdfReporteTermoencogidoSelladoPartial", consulta)
                {//METODO AL QUE SE HACE REFERENCIA ------------------, OBJETO 
                 // Establece la Cabecera y el Pie de página
                    CustomSwitches = customSwitches +
                    "--page-offset 0 --footer-center [page] --footer-font-size 10",
                    PageSize = Rotativa.Options.Size.A3,
                    PageMargins = new Rotativa.Options.Margins(25, 5, 10, 5),
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                };
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

        public ActionResult PdfReporteTermoencogidoSelladoPartial(DateTime fechaDesde, DateTime fechaHasta, int id, int opcion)
        {
            clsDTemperaturaTermoencogidoSellado = new clsDTemperaturaTermoencogidoSellado();
            var poCloroCisterna = clsDTemperaturaTermoencogidoSellado.ConsultarTermoencogidoSelladoDetalle(fechaDesde, fechaHasta, id, opcion);
            if (poCloroCisterna != null)
            {
                return PartialView(poCloroCisterna);

            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
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