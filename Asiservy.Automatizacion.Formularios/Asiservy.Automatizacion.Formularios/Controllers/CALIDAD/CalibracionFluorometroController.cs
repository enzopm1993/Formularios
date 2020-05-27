using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.CalibracionFluorometro;
using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Data.Entity.Validation;
using System.Net;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;
using System.Collections.Generic;
using System.Text;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class CalibracionFluorometroController : Controller
    {
        clsDError clsDError { get; set; } = null;
        ClsDCalibracionFluorometro ClsDCalibracionFluorometro { get; set; } = null;
        string[] lsUsuario { get; set; } = null;
        public clsDReporte ClsDReporte { get; set; } = null;
        public ActionResult MantenimientoEstandar()
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

        public ActionResult MantenimientoEstandarPartial()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDCalibracionFluorometro = new ClsDCalibracionFluorometro();
                var listaEstandar = ClsDCalibracionFluorometro.ListarEstandar();
                if (listaEstandar.Count != 0)
                {
                    return PartialView(listaEstandar);
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

        public JsonResult GuardarModificarEstandar(CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrWhiteSpace(model.NombEstandar))
                {
                    ClsDCalibracionFluorometro = new ClsDCalibracionFluorometro();
                    model.FechaIngresoLog = DateTime.Now;
                    model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    model.UsuarioIngresoLog = lsUsuario[0];
                    var valor = ClsDCalibracionFluorometro.GuardarModificarEstandar(model);
                    if (valor == 0)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else if (valor == 1)
                    {
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                    else return Json("2", JsonRequestBehavior.AllowGet);
                }
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

        public JsonResult EliminarEstandar(CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
               
                    ClsDCalibracionFluorometro = new ClsDCalibracionFluorometro();
                    model.FechaIngresoLog = DateTime.Now;
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    model.UsuarioIngresoLog = lsUsuario[0];
                    var valor = ClsDCalibracionFluorometro.EliminarEstandar(model);
                    if (valor == 0)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else if (valor == 1)
                    {
                        return Json("1", JsonRequestBehavior.AllowGet);
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

        public ActionResult CalibracionFluorometro()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.MascaraInput = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ClsDCalibracionFluorometro = new ClsDCalibracionFluorometro();                
                ViewBag.ListaEstandar = ClsDCalibracionFluorometro.ListarEstandar(1);
                lsUsuario = User.Identity.Name.Split('_');
                ViewBag.Analista = lsUsuario[0];
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

        public JsonResult ConsultarCalibracionFluorometroJson(int idCalibracionFluor = 0,DateTime? fecha=null)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDCalibracionFluorometro = new ClsDCalibracionFluorometro();
                var calibracionFluorIdFecha = ClsDCalibracionFluorometro.ConsultarCalibracionFluorIdFecha(idCalibracionFluor, fecha);
                if (calibracionFluorIdFecha != null)
                {
                    CC_CALIBRACION_FLUOROMETRO_CTRL cab = new CC_CALIBRACION_FLUOROMETRO_CTRL();
                    cab.IdCalibracionFluor = calibracionFluorIdFecha.IdCalibracionFluor;
                    cab.FechaHora = calibracionFluorIdFecha.FechaHora;
                    cab.CoeficienteDeterminacion = calibracionFluorIdFecha.CoeficienteDeterminacion;
                    cab.EstadoReporte = calibracionFluorIdFecha.EstadoReporte;
                    cab.FechaAprobado = calibracionFluorIdFecha.FechaAprobado;
                    cab.FechaAprobado = calibracionFluorIdFecha.FechaAprobado;
                    cab.UsuarioIngresoLog = calibracionFluorIdFecha.UsuarioIngresoLog;
                    cab.FechaIngresoLog = calibracionFluorIdFecha.FechaIngresoLog;
                    return Json(cab, JsonRequestBehavior.AllowGet);
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

        public ActionResult CalibracionFluorometroPartial(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDCalibracionFluorometro = new ClsDCalibracionFluorometro();
                var rangoFecha = ClsDCalibracionFluorometro.ConsultarFluorRangoFecha(fechaDesde,fechaHasta);
                //if (rangoFecha != null)
                //{
                    return PartialView(rangoFecha);
                //}
                //else
                //{
                //    return Json("0", JsonRequestBehavior.AllowGet);
                //}
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

        public JsonResult GuardarModificarCalibracionFluor(CC_CALIBRACION_FLUOROMETRO_CTRL model, bool siAprobar, List<CC_CALIBRACION_FLUOROMETRO_DET> detalle)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDCalibracionFluorometro = new ClsDCalibracionFluorometro();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                if (detalle.Count == 0)
                {
                    return Json("6", JsonRequestBehavior.AllowGet);//SIN DETALLE
                }
                if (siAprobar)
                {
                    var estadoReporte = ClsDCalibracionFluorometro.ConsultarCalibracionFluorIdFecha(model.IdCalibracionFluor);
                    if (estadoReporte.EstadoReporte)
                    {
                        return Json("5", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                    }
                }
                else
                {
                    var validarFechaExiste = ClsDCalibracionFluorometro.ConsultarCalibracionFluorIdFecha(0, model.FechaHora);
                    if (validarFechaExiste != null)
                    {
                        return Json("4", JsonRequestBehavior.AllowGet);//FECHA EXISTE
                    }
                }
                var valor = ClsDCalibracionFluorometro.GuardarModificarCalibracionFluor(model, siAprobar);
                foreach (var item in detalle)
                {
                    item.IdCalibracionFluor = model.IdCalibracionFluor;
                    item.FechaIngresoLog = DateTime.Now;
                    item.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    item.TerminalIngresoLog = Request.UserHostAddress;
                    item.UsuarioIngresoLog = lsUsuario[0];
                    ClsDCalibracionFluorometro.GuardarModificarCalibracionFluorDetalle(item);
                }
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 1)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 2) { return Json("2", JsonRequestBehavior.AllowGet); }//APROBAR
                else
                {
                    return Json("3", JsonRequestBehavior.AllowGet);//ERROR DE FECHA/HORA
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

        public JsonResult EliminarCalibracionFluor(CC_CALIBRACION_FLUOROMETRO_CTRL model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDCalibracionFluorometro = new ClsDCalibracionFluorometro();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor = ClsDCalibracionFluorometro.EliminarCalibracionFluor(model);
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
        //public JsonResult EliminarHigieneControl(CC_CALIBRACION_FLUOROMETRO_CTRL model)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }
        //        ClsDCalibracionFluorometro = new ClsDCalibracionFluorometro();
        //        model.FechaIngresoLog = DateTime.Now;
        //        model.TerminalIngresoLog = Request.UserHostAddress;
        //        model.UsuarioIngresoLog = lsUsuario[0];
        //        model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
        //        var valor = ClsDCalibracionFluorometro.EliminarCalibracionFluor(model);
        //        if (valor == 0)
        //        {
        //            return Json("0", JsonRequestBehavior.AllowGet);
        //        }
        //        else return Json("1", JsonRequestBehavior.AllowGet);
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

        //public JsonResult GuardarModificarCalibracionFluorDetalle(CC_CALIBRACION_FLUOROMETRO_DET model)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }
        //        if (model.Fecha != DateTime.MinValue && model.Hora != DateTime.MinValue && model.ValorEstandar >= 0)
        //        {
        //            ClsDCalibracionFluorometro = new ClsDCalibracionFluorometro();
        //            model.FechaIngresoLog = DateTime.Now;
        //            model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
        //            model.TerminalIngresoLog = Request.UserHostAddress;
        //            model.UsuarioIngresoLog = lsUsuario[0];
        //            var valor = ClsDCalibracionFluorometro.GuardarModificarCalibracionFluorDetalle(model);
        //            if (valor == 0)
        //            {
        //                return Json("0", JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                return Json("1", JsonRequestBehavior.AllowGet);
        //            }
        //        }else return Json("2", JsonRequestBehavior.AllowGet);

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

        //public JsonResult EliminarHigieneControlDetalle(CC_CALIBRACION_FLUOROMETRO_DET model)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }
        //        ClsDCalibracionFluorometro = new ClsDCalibracionFluorometro();
        //        model.FechaIngresoLog = DateTime.Now;
        //        model.TerminalIngresoLog = Request.UserHostAddress;
        //        model.UsuarioIngresoLog = lsUsuario[0];
        //        model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
        //        var valor = ClsDCalibracionFluorometro.EliminarCalibracionFluorDetalle(model);
        //        if (valor == 0)
        //        {
        //            return Json("0", JsonRequestBehavior.AllowGet);
        //        }
        //        else return Json("1", JsonRequestBehavior.AllowGet);
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