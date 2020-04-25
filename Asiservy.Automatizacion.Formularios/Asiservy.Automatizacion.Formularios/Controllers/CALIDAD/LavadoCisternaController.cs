using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Net;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ControlLavadoCisterna;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MantenimientoCisterna;
using Rotativa;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class LavadoCisternaController : Controller
    {
        clsDError clsDError = null;
        clsDEmpleado clsDEmpleado = null;
        clsDControlLavadoCisterna clsDControlLavadoCisterna = null;
        string[] lsUsuario;
        public ActionResult LavadoCisterna()
        {
            try
            {
                clsDMantenimientoCisterna c = new clsDMantenimientoCisterna();
                var codigoCisterna = c.ConsultarMantenimientoCisterna();
                var listaCisterna =  (from x in codigoCisterna
                                          where x.EstadoRegistro=="A"
                                          select new {x.IdCisterna, x.NDescripcion}).ToList();
                ViewBag.listaCisterna = listaCisterna;
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
        
        public ActionResult LavadoCisternaPartial(DateTime fechaDesde, DateTime fechaHasta, int idLavadoCisterna, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlLavadoCisterna = new clsDControlLavadoCisterna();
                var detalleTabla = clsDControlLavadoCisterna.ConsultarLavadoCisterna(fechaDesde, fechaHasta, idLavadoCisterna, op);

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

        //---------------------------------------------------CABECERA CONTROL---------------------------------------------------------------
        public JsonResult ConsultarLavadoCisterna(DateTime fechaDesde, DateTime fechaHasta, int idLavadoCisterna, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlLavadoCisterna = new clsDControlLavadoCisterna();
                var poCloroCisterna = clsDControlLavadoCisterna.ConsultarLavadoCisterna(fechaDesde, fechaHasta, idLavadoCisterna, op).FirstOrDefault();
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
        public JsonResult GuardarModificarLavadoCisterna(CC_LAVADO_CISTERNA model, string idMantCisterna, List<string> idIntermedia, int siAprobar,DateTime fechaDesde)
        {
            var splitIdMantCisterna = idMantCisterna.Split(';');          
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlLavadoCisterna = new clsDControlLavadoCisterna();
                var consultarEstadoReporte = clsDControlLavadoCisterna.ConsultarLavadoCisterna(fechaDesde, Convert.ToDateTime("01-01-2020"), model.IdLavadoCisterna,4);
                bool estadoReporte = false;
                if (consultarEstadoReporte.Count!=0) {
                    estadoReporte = consultarEstadoReporte[0].EstadoReporteCab;
                }
                if (estadoReporte == false)
                {
                    model.FechaIngresoLog = DateTime.Now;
                    model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    model.UsuarioIngresoLog = lsUsuario[0];
                    var valor = clsDControlLavadoCisterna.GuardarModificarLavadoCisterna(model, siAprobar);
                    //GUARDAR EN LA TABLA INTERMEDIA
                    int idCisterna = model.IdLavadoCisterna;
                    foreach (var item in splitIdMantCisterna)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            CC_INTERMEDIA_CTRL_MANT_CISTERNA modelIntermedia = new CC_INTERMEDIA_CTRL_MANT_CISTERNA();
                            modelIntermedia.IdCtrlLavadoCisterna = Convert.ToInt32(idCisterna);
                            modelIntermedia.IdMantCisterna = Convert.ToInt32(item);
                            modelIntermedia.FechaIngresoLog = DateTime.Now;
                            modelIntermedia.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                            modelIntermedia.TerminalIngresoLog = Request.UserHostAddress;
                            modelIntermedia.UsuarioIngresoLog = lsUsuario[0];
                            var guardar = clsDControlLavadoCisterna.GuardarModificarLavadoCisternaIntermedia(modelIntermedia);
                        }
                    }
                    //ELIMINO TODOS LOS REGISTROS DE LA TABLA INTERMEDIA QUE CORESPONDAN AL ITEM A ACTUALIZAR. SEGUN LA LISTA idIntermedia PARAMETRO DE ENTRADA
                    //HAGO ESTO YA QUE ES COMPLICADO ATUALIZAR UNO POR UNO, Y PEOR CUANDO EL USUARIO INGRESA 5 CISTERNAS Y LUEGO ACTUALIZA A 1. 
                    if (idIntermedia != null)
                    {
                        foreach (var item in idIntermedia)
                        {
                            CC_INTERMEDIA_CTRL_MANT_CISTERNA modelIntermedia = new CC_INTERMEDIA_CTRL_MANT_CISTERNA();
                            modelIntermedia.IdIntermedia = Convert.ToInt32(item);
                            modelIntermedia.FechaIngresoLog = DateTime.Now;
                            modelIntermedia.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                            modelIntermedia.TerminalIngresoLog = Request.UserHostAddress;
                            modelIntermedia.UsuarioIngresoLog = lsUsuario[0];
                            var guardar = clsDControlLavadoCisterna.EliminarLavadoCisternaIntermedia(modelIntermedia);
                        }
                    }
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
        public JsonResult EliminarLavadoCisterna(CC_LAVADO_CISTERNA model, DateTime fechaDesde)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlLavadoCisterna = new clsDControlLavadoCisterna();
                var consultarEstadoReporte = clsDControlLavadoCisterna.ConsultarLavadoCisterna(fechaDesde, Convert.ToDateTime("01-01-2020"), model.IdLavadoCisterna, 0);
                if (consultarEstadoReporte[0].EstadoReporteCab == false)
                {
                    model.FechaIngresoLog = DateTime.Now;
                    model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    model.UsuarioIngresoLog = lsUsuario[0];
                    var valor = clsDControlLavadoCisterna.EliminarLavadoCisterna(model);
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

        //---------------------------------------------------REPORTE---------------------------------------------------------------
        public ActionResult ReporteLavadoCisterna()
        {
            try
            {
                ViewBag.DateRangePicker = "1";
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

        public ActionResult ReporteLavadoCisternaPartial(DateTime fechaDesde, DateTime fechaHasta, int idLavadoCisterna, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlLavadoCisterna = new clsDControlLavadoCisterna();
                var tablaCabecera = clsDControlLavadoCisterna.ConsultarLavadoCisterna(fechaDesde, fechaHasta, idLavadoCisterna, op);

                if (tablaCabecera != null)
                {
                    return PartialView(tablaCabecera);
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

        //---------------------------------------------------BANDEJA---------------------------------------------------------------
        public ActionResult BandejaLavadoCisterna()
        {
            try
            {
                ViewBag.DateRangePicker = "1";
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

        public ActionResult BandejaLavadoCisternaPartial(DateTime fechaDesde, DateTime fechaHasta,int idLavadoCisterna, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlLavadoCisterna = new clsDControlLavadoCisterna();
                var tablaCabecera = clsDControlLavadoCisterna.ConsultarLavadoCisterna(fechaDesde, fechaHasta, idLavadoCisterna, op);

                if (tablaCabecera != null)
                {
                    sp_Control_Lavado_Cisterna cisterna;
                    List<sp_Control_Lavado_Cisterna> listaCisterna = new List<sp_Control_Lavado_Cisterna>();
                    var listaFiltrada = (from x in tablaCabecera
                                         group x by new { x.IdLavadoCisterna, x.Fecha } into f
                                         select new { f.Key.IdLavadoCisterna, f.Key.Fecha }).ToList();//agrupo por IdLavadoCisterna, fecha para poder saber el total de las filas 
                    foreach (var item in listaFiltrada)
                    {
                        cisterna = new sp_Control_Lavado_Cisterna();                       
                            var listaAgrupada = (from x in tablaCabecera
                                                 where item.IdLavadoCisterna== x.IdLavadoCisterna && item.Fecha==x.Fecha
                                                 select new { x.IdLavadoCisterna, x.Fecha, x.QuimUtilizados, x.Observacion, x.EstadoReporteCab
                                                 , x.NDescripcion, x.UsuarioIngresoLog}).First();
                        cisterna.IdLavadoCisterna = listaAgrupada.IdLavadoCisterna;
                        cisterna.Fecha = listaAgrupada.Fecha;
                        cisterna.QuimUtilizados = listaAgrupada.QuimUtilizados;
                        cisterna.Observacion = listaAgrupada.Observacion;
                        cisterna.EstadoReporteCab = listaAgrupada.EstadoReporteCab;
                        cisterna.NDescripcion = listaAgrupada.NDescripcion;
                        cisterna.UsuarioIngresoLog = listaAgrupada.UsuarioIngresoLog;
                        listaCisterna.Add(cisterna);
                    }
                    return PartialView(listaCisterna);
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

        [HttpPost]
        public JsonResult GuardarBandejaModificarLavadoCisterna(CC_LAVADO_CISTERNA model, int siAprobar)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlLavadoCisterna = new clsDControlLavadoCisterna();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor = clsDControlLavadoCisterna.GuardarModificarLavadoCisterna(model, siAprobar);
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

        public ActionResult BandejaLavadoCisternaAprobarPartial(DateTime fechaDesde, DateTime fechaHasta, int idLavadoCisterna, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlLavadoCisterna = new clsDControlLavadoCisterna();
                var tablaCabecera = clsDControlLavadoCisterna.ConsultarLavadoCisterna(fechaDesde, fechaHasta, idLavadoCisterna, op);

                if (tablaCabecera != null)
                {
                    return PartialView(tablaCabecera);
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

        //---------------------------------------------------IMPRESION PDF---------------------------------------------------------------
        public ActionResult PrintReport(DateTime filtroFechaDesde, DateTime filtroFechaHasta, int id, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    Response.Redirect(Url.Action("Login", "Login"));
                }
                clsDControlLavadoCisterna = new clsDControlLavadoCisterna();
                var consulta = clsDControlLavadoCisterna.ConsultarLavadoCisterna(filtroFechaDesde, filtroFechaHasta, id, op);
                var headerPdf = Server.MapPath("~/Views/LavadoCisterna/HeaderPdf.html");//ARCHIVO HTML USADO EN LA CABECERA DEL PDF
                ViewBag.filtroFechaDesde = filtroFechaDesde;
                ViewBag.filtroFechaHasta = filtroFechaHasta;
                string customSwitches = string.Format("--header-html  \"{0}\" " +
                            "--header-font-size \"15\" ", headerPdf);
                return new ViewAsPdf("PdfReporteLavadoCisternaPartial", consulta)
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

        public ActionResult PdfReporteLavadoCisternaPartial(DateTime fechaDesde, DateTime fechaHasta, int idLavadoCisterna, int op)
        {
            clsDControlLavadoCisterna = new clsDControlLavadoCisterna();
            var tablaCabecera = clsDControlLavadoCisterna.ConsultarLavadoCisterna(fechaDesde, fechaHasta, idLavadoCisterna, op);
            if (tablaCabecera != null)
            {
                return PartialView(tablaCabecera);
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