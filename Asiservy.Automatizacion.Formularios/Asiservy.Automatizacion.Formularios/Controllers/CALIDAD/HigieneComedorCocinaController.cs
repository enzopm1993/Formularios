using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.HigieneComedorCocina;
using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Rotativa;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class HigieneComedorCocinaController : Controller
    {
        clsDError clsDError = null;
        clsDHigieneComedorCocina clsDHigieneComedorCocina = null;
        string[] lsUsuario;

        public ActionResult MantHigieneComedorCocina()
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

        public ActionResult MantHigieneComedorCocinaPartial()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
                var lista = clsDHigieneComedorCocina.ConsultaHigieneMant();
                if (lista.Count() != 0)
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

        public JsonResult GuardarModificarMantHigieneComedorCocina(CC_HIGIENE_COMEDOR_COCINA_MANT model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrWhiteSpace(model.Nombre) || !string.IsNullOrWhiteSpace(model.Categoria))
                {
                    clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
                    model.FechaIngresoLog = DateTime.Now;
                    model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    model.UsuarioIngresoLog = lsUsuario[0];
                    var valor = clsDHigieneComedorCocina.GuardarModificarMant(model);
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

        public JsonResult EliminarMantHigieneComedorCocina(CC_HIGIENE_COMEDOR_COCINA_MANT model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor = clsDHigieneComedorCocina.EliminarMant(model);
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
        //------------------------------------------------CONTROL------------------------------------------------------------------------
        public ActionResult HigieneComedorCocina()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.FirmaPad = "1";
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

        public JsonResult ConsultarHigieneControl(DateTime fecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
                var lista = clsDHigieneComedorCocina.ConsultarHigieneControl(fecha);
                if (lista.Count() != 0)
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

        public ActionResult HigieneComedorCocinaDetallePartial(int idControlHigiene, DateTime fechaDesde, DateTime fechaHasta, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
                var lista = clsDHigieneComedorCocina.ConsultaHigieneSP(idControlHigiene, fechaDesde, fechaHasta, op);
                if (lista.Count() != 0)
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

        //public JsonResult HigieneComedorCocinaDetalle(int idControlHigiene, DateTime fechaDesde, DateTime fechaHasta, int op)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }
        //        clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
        //        var lista = clsDHigieneComedorCocina.ConsultaHigieneSP(idControlHigiene, fechaDesde, fechaHasta, op);
        //        if (lista.Count() != 0)
        //        {
        //            return Json(lista, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("0", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
        //        SetErrorMessage(Mensaje);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
        //        SetErrorMessage(Mensaje);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult ConsultaHigieneMantActivos(string estadoRegistro)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }
        //        clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
        //        var lista = clsDHigieneComedorCocina.ConsultaHigieneMantActivos(estadoRegistro);
        //        if (lista.Count() != 0)
        //        {
        //            return Json(lista,JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("0", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
        //        SetErrorMessage(Mensaje);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
        //        SetErrorMessage(Mensaje);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public ActionResult ConsultaHigieneMantActivosPartial(string estadoRegistro)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
                var lista = clsDHigieneComedorCocina.ConsultaHigieneMantActivos(estadoRegistro);
                if (lista.Count() != 0)
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

        public JsonResult GuardarModificarHigieneControl(CC_HIGIENE_COMEDOR_COCINA_CTRL model, int siAprobar)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model.Fecha!=null  || model.Hora!=null)
                {
                    clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
                    model.FechaIngresoLog = DateTime.Now;
                    model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    model.UsuarioIngresoLog = lsUsuario[0];
                    var valor = clsDHigieneComedorCocina.GuardarModificarHigieneControl(model, siAprobar);
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

        public JsonResult EliminarHigieneControl(CC_HIGIENE_COMEDOR_COCINA_CTRL model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                var valor = clsDHigieneComedorCocina.EliminarHigieneControl(model);
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

        public JsonResult GuardarModificarHigieneControlDetalle(List<CC_HIGIENE_COMEDOR_COCINA_CTRL_DET> listaControlDetalle)
        {
            try
            {
                int p = 0;
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
                CC_HIGIENE_COMEDOR_COCINA_CTRL_DET model;
                foreach (var item in listaControlDetalle)
                {
                    model = new CC_HIGIENE_COMEDOR_COCINA_CTRL_DET();
                    model.FechaIngresoLog = DateTime.Now;
                    model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    model.UsuarioIngresoLog = lsUsuario[0];
                    model.AccionCorrectiva = item.AccionCorrectiva;
                    model.IdControlHigiene = item.IdControlHigiene;
                    model.IdMantenimiento = item.IdMantenimiento;
                    model.LimpiezaEstado = item.LimpiezaEstado;
                    model.Observacion = item.Observacion;
                    model.IdControlDetalle = item.IdControlDetalle;
                    var valor = clsDHigieneComedorCocina.GuardarModificarControlDetalle(model);
                    p = valor;
                }
               
                if (p == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else if (p == 1)
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

        //public JsonResult EliminarHigieneControlDetalle(CC_HIGIENE_COMEDOR_COCINA_CTRL_DET model)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }
        //        clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
        //        model.FechaIngresoLog = DateTime.Now;
        //        model.TerminalIngresoLog = Request.UserHostAddress;
        //        model.UsuarioIngresoLog = lsUsuario[0];
        //        model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
        //        var valor = clsDHigieneComedorCocina.EliminarControlDetalle(model);
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

        [HttpPost]
        public JsonResult GuardarImagenFirma(string image, int idControlHigiene)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                byte[] Firma = Convert.FromBase64String(image);
                CC_HIGIENE_COMEDOR_COCINA_CTRL model = new CC_HIGIENE_COMEDOR_COCINA_CTRL();
                clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.FirmaControl = Firma;
                model.IdControlHigiene = idControlHigiene;
                var valor = clsDHigieneComedorCocina.GuardarImagenFirma(model, true);
                if (valor == 1)
                {
                    var imagenfirma = String.Format("data:image/png;base64,{0}", image);
                    return Json(imagenfirma, JsonRequestBehavior.AllowGet);
                }
                else return Json("0", JsonRequestBehavior.AllowGet);
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

        public JsonResult ConsultarImagenFirma(int idControlHigiene)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDHigieneComedorCocina = new clsDHigieneComedorCocina();
                var lista = clsDHigieneComedorCocina.ConsultarImagenFirma(idControlHigiene, true);
                if (lista != null)
                {
                    if (lista[0].FirmaControl != null)
                    {
                        var base64 = Convert.ToBase64String(lista[0].FirmaControl);
                        var imagenfirma = String.Format("data:image/png;base64,{0}", base64);
                        return Json(imagenfirma, JsonRequestBehavior.AllowGet);
                    }
                    else return Json("0", JsonRequestBehavior.AllowGet);
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