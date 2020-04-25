using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ControlCuchillosPreparacion;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity.Validation;
using System.Net;
using Rotativa;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class ControlCuchillosPreparacionController : Controller
    {
        clsDError clsDError = null;
        clsDEmpleado clsDEmpleado = null;
        clsDControlCuchillosPreparacion clsDControlCuchillosPreparacion = null;
        string[] lsUsuario;
        //-----------------------------------------------------VISTA DE INGRESO DE DATOS DE CUCHILLO----------------------------------------------------------------
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

        public JsonResult ConsultarCuchilloPreparacion(string CodigoCuchillo, int opcion)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                var poCloroCisterna = clsDControlCuchillosPreparacion.ConsultarCuchilloPreparacion(CodigoCuchillo, opcion);
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
                if (string.IsNullOrEmpty(lsUsuario[0]))
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
        public ActionResult ControlCuchilloPreparacion()
        {
            try
            {
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                clsDEmpleado empleado = new clsDEmpleado();
                var listaEmpleadoLinea = empleado.ConsultaEmpleadosFiltro("46","0","0");
                ViewBag.listaEmpleadoLinea = listaEmpleadoLinea;
                var poCloroCisterna = clsDControlCuchillosPreparacion.ConsultarCuchilloPreparacion("", 2);
                ViewBag.Cuchillos = poCloroCisterna;
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

        public JsonResult ConsultarControlCuchilloPreparacion(DateTime fecha, int IdControlCuchillo, int opcion)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                var poCloroCisterna = clsDControlCuchillosPreparacion.ConsultarControlCuchilloPreparacion(fecha, fecha, IdControlCuchillo, opcion);
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

        public JsonResult GuardarModificarControlCuchilloPreparacion(CC_CONTROL_CUCHILLOS_PREPARACION model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor=clsDControlCuchillosPreparacion.GuardarModificarControlCuchilloPreparacion(model);
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
        public ActionResult EliminarControlCuchilloPreparacion(CC_CONTROL_CUCHILLOS_PREPARACION model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
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
        public JsonResult ConsultarControlCuchilloDetalle(int idCuchilloPreparacion, int idControlCuchillo, int idControlCuchilloDetalle, int opcion)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                var poCloroCisterna = clsDControlCuchillosPreparacion.ConsultarControlCuchilloDetalle(idCuchilloPreparacion, idControlCuchillo, idControlCuchilloDetalle, opcion);
                if (poCloroCisterna != null)
                {
                    clsDEmpleado empleado = new clsDEmpleado();
                    var listaEmpleadoLinea = empleado.ConsultaEmpleadosFiltro("46", "0", "0");
                    List<string> listaCedula = new List<string>();
                    foreach (var itemCuchillo in poCloroCisterna) {
                        listaCedula.Add(itemCuchillo.CedulaEmpleado);
                        foreach (var itemEmpleado in listaEmpleadoLinea)
                        {
                            if (itemCuchillo.CedulaEmpleado==itemEmpleado.CEDULA) {                                
                                itemCuchillo.CedulaEmpleado+="-"+itemEmpleado.NOMBRES;
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

        public JsonResult GuardarModificarControlCuchilloDetalle(CC_CONTROL_CUCHILLOS_PREPARACION_DETALLE model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                model.FechaIngresoLog = DateTime.Now;           
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor = clsDControlCuchillosPreparacion.GuardarModificarControlCuchilloDetalle(model);
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
        public ActionResult EliminarControlCuchilloDetalle(CC_CONTROL_CUCHILLOS_PREPARACION_DETALLE model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                var poCloroCisterna = clsDControlCuchillosPreparacion.EliminarControlCuchilloDetalle(model);
                if (poCloroCisterna == 1)
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

        //------------------------------------REPORTE CONTROL CUCHILLO PREPARACION DETALLE----------------------------------------------
        public ActionResult ReporteControlCuchilloPreparacion()
        {
            try
            {
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.DateRangePicker = "1";
                ViewBag.dataTableJS = "1";
                ViewBag.Pivot = "1";
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

        public ActionResult ReporteControlCuchilloPreparacionPartial(DateTime filtroFechaDesde, DateTime filtroFechaHasta, int opcion)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();

                var poCloroCisterna = clsDControlCuchillosPreparacion.ReporteControlCuchilloPreparacion(filtroFechaDesde, filtroFechaHasta, opcion);
                if (poCloroCisterna != null)
                {
                    clsDEmpleado empleado = new clsDEmpleado();
                    var listaEmpleadoLinea = empleado.ConsultaEmpleadosFiltro("46", "0", "0");
                    foreach (var itemCuchillo in poCloroCisterna)
                    {
                        foreach (var itemEmpleado in listaEmpleadoLinea)
                        {
                            if (itemCuchillo.CedulaEmpleado == itemEmpleado.CEDULA)
                            {
                                itemCuchillo.CedulaEmpleado += "-" + itemEmpleado.NOMBRES;
                            }
                        }             }
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

        public ActionResult BandejaCuchilloPreparacionPartial(DateTime fechaDesde, DateTime fechaHasta,int idControlCuchillo, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
                var cabeceraTabla = clsDControlCuchillosPreparacion.ConsultarControlCuchilloPreparacion(fechaDesde, fechaHasta, idControlCuchillo, op);
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

        //-------------------------------------------IMPRIMIR PDF------------------------------------------------------
        public ActionResult PrintReport(DateTime filtroFechaDesde, DateTime filtroFechaHasta, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    Response.Redirect(Url.Action("Login", "Login"));
                }
                var headerPdf = Server.MapPath("~/Views/ControlCuchillosPreparacion/HeaderPdf.html");//ARCHIVO HTML USADO EN LA CABECERA DEL PDF
                ViewBag.filtroFechaDesde = filtroFechaDesde;
                ViewBag.filtroFechaHasta = filtroFechaHasta;
                string customSwitches = string.Format("--header-html  \"{0}\" " +
                            "--header-font-size \"15\" ", headerPdf);
                return new ViewAsPdf("PdfControlCuchilloPreparacionPartial", model(filtroFechaDesde, filtroFechaHasta, op))
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

        public ActionResult PdfControlCuchilloPreparacionPartial(DateTime filtroFechaDesde, DateTime filtroFechaHasta, int op)//VISTA MOSTRAR PDF
        {
            clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
            var poCloroCisterna = clsDControlCuchillosPreparacion.ReporteControlCuchilloPreparacion(filtroFechaDesde, filtroFechaHasta, op);
            if (poCloroCisterna != null)
            {
                clsDEmpleado empleado = new clsDEmpleado();
                var listaEmpleadoLinea = empleado.ConsultaEmpleadosFiltro("46", "0", "0");
                foreach (var itemCuchillo in poCloroCisterna)
                {
                    foreach (var itemEmpleado in listaEmpleadoLinea)
                    {
                        if (itemCuchillo.CedulaEmpleado == itemEmpleado.CEDULA)
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

        public List<sp_Reporte_Control_Cuchillos_Preparacion> model(DateTime filtroFechaDesde, DateTime filtroFechaHasta, int op)
        {//ESTE MEDOTO CONCATENA LA CEDULA CON EL NOMBRE DEL EMPLEADO PARA LUEGO SER MOSTRADO EN EL REPORTE.
            clsDControlCuchillosPreparacion = new clsDControlCuchillosPreparacion();
            var poCloroCisterna = clsDControlCuchillosPreparacion.ReporteControlCuchilloPreparacion(filtroFechaDesde, filtroFechaHasta, op);
            if (poCloroCisterna != null)
            {
                clsDEmpleado empleado = new clsDEmpleado();
                var listaEmpleadoLinea = empleado.ConsultaEmpleadosFiltro("46", "0", "0");
                foreach (var itemCuchillo in poCloroCisterna)
                {
                    foreach (var itemEmpleado in listaEmpleadoLinea)
                    {
                        if (itemCuchillo.CedulaEmpleado == itemEmpleado.CEDULA)
                        {
                            itemCuchillo.CedulaEmpleado += "-" + itemEmpleado.NOMBRES;
                        }
                    }
                }                
            }
            return poCloroCisterna;
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