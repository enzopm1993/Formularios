using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.LimpiezaDesinfeccionPlanta;
using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web;
using System.IO;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class LimpiezaDesinfeccionPlantaController : Controller
    {
        public clsDReporte ClsDReporte { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        clsDLimpiezaDesinfeccionPlanta clsDLimpiezaDesinfeccionPlanta { get; set; } = null;
        string[] lsUsuario;
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }

        public ActionResult MantAreaAuditoria()
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

        public ActionResult MantAreaAuditoriaPartial()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var lista = clsDLimpiezaDesinfeccionPlanta.ConsultarAreaAuditoria();
                if (lista.Count != 0)
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

        public ActionResult MantObjetos()
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

        public ActionResult MantObjetosPartial()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var lista = clsDLimpiezaDesinfeccionPlanta.ConsultarObjetos();
                if (lista.Count != 0)
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

        public ActionResult ConsultarObjetosActivos(string estadoRegistro)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var lista = clsDLimpiezaDesinfeccionPlanta.ConsultarObjetosActivos(estadoRegistro);
                if (lista.Count != 0)
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

        public JsonResult GuardarModificarAreaAuditoria(CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA model, List<int> listaIdObjetoAuditar, List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA> listaIdObjetoEliminar)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrWhiteSpace(model.NombreAuditoria))
                {
                    if (listaIdObjetoAuditar.Count == 0)//Id PARA INSERTAR EN LA TABLA INTERMEDIA
                    {
                        return Json("4", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                        //VALIDAR SI ALGUN ID DE LA LISTA HA SIDO INACTIVADO, ESTO SUCEDE AL ABRIR DOS PESTAÑAS AUDITORIA-OBJETO
                        foreach (var item in listaIdObjetoAuditar)
                        {
                            int activo = clsDLimpiezaDesinfeccionPlanta.ConsultarObjetosActivosID(item);
                            if (activo == 0)
                            {
                                return Json("5", JsonRequestBehavior.AllowGet);//CUANDO EXISTE UN ID EN ESTADO INACTIVO
                            }
                        }
                        model.FechaIngresoLog = DateTime.Now;
                        model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                        model.TerminalIngresoLog = Request.UserHostAddress;
                        model.UsuarioIngresoLog = lsUsuario[0];
                        var valor = clsDLimpiezaDesinfeccionPlanta.GuardarModificarAreaAuditoria(model);
                        CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA modelIntermedia = new CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA();
                        if (valor==6)
                        {
                            return Json("6", JsonRequestBehavior.AllowGet);
                        }
                        modelIntermedia.FechaIngresoLog = DateTime.Now;
                        modelIntermedia.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                        modelIntermedia.TerminalIngresoLog = Request.UserHostAddress;
                        modelIntermedia.UsuarioIngresoLog = lsUsuario[0];
                        modelIntermedia.IdAuditoria = model.IdAuditoria;                       

                        foreach (var id in listaIdObjetoAuditar)//GUARDAR INTERMEDIA
                        {
                            modelIntermedia.IdObjeto = id;
                            clsDLimpiezaDesinfeccionPlanta.GuardarModificarIntermedia(modelIntermedia);
                        }
                        modelIntermedia.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;//ELIMINAR INTERMEDIA LOS OBJETOS QUE SE LE QUITARON EL CHECK
                        if (listaIdObjetoEliminar != null)
                        {
                            foreach (var item in listaIdObjetoEliminar)
                            {
                                modelIntermedia.IdMantenimiento = item.IdMantenimiento;
                                clsDLimpiezaDesinfeccionPlanta.EliminarIntermedia(modelIntermedia);
                            }
                        }

                        if (valor == 0)
                        {
                            return Json("0", JsonRequestBehavior.AllowGet);
                        }
                        else if (valor == 1)
                        {
                            return Json("1", JsonRequestBehavior.AllowGet);
                        }
                        else if (valor == 2) return Json("2", JsonRequestBehavior.AllowGet);
                        else return Json("6", JsonRequestBehavior.AllowGet);
                    }
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

        public JsonResult EliminarAreaAuditoria(CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor = clsDLimpiezaDesinfeccionPlanta.EliminarAreaAuditoria(model);
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

        public JsonResult GuardarModificarObjeto(CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrWhiteSpace(model.NombreObjeto))
                {
                    clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                    model.FechaIngresoLog = DateTime.Now;
                    model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    model.UsuarioIngresoLog = lsUsuario[0];
                    var valor = clsDLimpiezaDesinfeccionPlanta.GuardarModificarObjeto(model);
                    if (valor == 0)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else if (valor == 1)
                    {
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                    else if (valor == 2) return Json("2", JsonRequestBehavior.AllowGet);
                    else return Json("3", JsonRequestBehavior.AllowGet);
                }
                else return Json("4", JsonRequestBehavior.AllowGet);
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

        public JsonResult EliminarObjeto(CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor = clsDLimpiezaDesinfeccionPlanta.EliminarObjeto(model);
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

        public ActionResult ConsultarIntermediaActivos(int idAuditoria)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var lista = clsDLimpiezaDesinfeccionPlanta.ConsultarIntermediaActivos(idAuditoria);
                if (lista.Count != 0)
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

        #region CONTROL
        //----------------------------------------------------CONTROL----------------------------------------------------------------------------------
        [Authorize]
        public ActionResult ControlLimpiezaDesinfeccionPlanta()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                ViewBag.DateRangePicker = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                ViewBag.JqueryRotate = "1";
                ViewBag.Inspector= lsUsuario[0];
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var lista = clsDLimpiezaDesinfeccionPlanta.ConsultarAreaAuditoriaActivos("A");
                ViewBag.ListaAreasAuditar = lista;
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

        public ActionResult LimpiezaDesinfeccionPlantaPartial(string estadoRegistro)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var lista = clsDLimpiezaDesinfeccionPlanta.ConsultarObjetosActivos(estadoRegistro);
                if (lista.Count != 0)
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

        public JsonResult ConsultarEstadoReporte(DateTime fechaControl, int idLimpiezaDesinfeccionPlanta=0)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var lista = clsDLimpiezaDesinfeccionPlanta.ConsultarEstadoReporte(idLimpiezaDesinfeccionPlanta, fechaControl);
                if (lista!= null)
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

        public JsonResult GuardarModificarLimpiezaCabecera(CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA model, int siAprobar)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                if (model.IdLimpiezaDesinfeccionPlanta!=0 && siAprobar==0)
                {
                    var estadoReporte = clsDLimpiezaDesinfeccionPlanta.ConsultarEstadoReporte(model.IdLimpiezaDesinfeccionPlanta,DateTime.MinValue);
                    if (estadoReporte.EstadoReporte)
                    {
                        return Json("4", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                    }
                }
                var valor = clsDLimpiezaDesinfeccionPlanta.GuardarModificarLimpiezaCabecera(model, siAprobar);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 1)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 2) { return Json("2", JsonRequestBehavior.AllowGet); }
                else return Json("3", JsonRequestBehavior.AllowGet);//ERROR DE FECHA
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

        public JsonResult EliminarLimpiezaCabecera(CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                var estadoReporte = clsDLimpiezaDesinfeccionPlanta.ConsultarEstadoReporte(model.IdLimpiezaDesinfeccionPlanta, DateTime.MinValue);
                if (!estadoReporte.EstadoReporte)
                {
                    var valor = clsDLimpiezaDesinfeccionPlanta.EliminarLimpiezaCabecera(model);
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

        public JsonResult ConsultarAreaAuditoriaActivos(string estadoRegistro)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var lista = clsDLimpiezaDesinfeccionPlanta.ConsultarAreaAuditoriaActivos(estadoRegistro);
                if (lista.Count != 0)
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
                return Json(Mensaje,JsonRequestBehavior.AllowGet);
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

        public ActionResult ConsultarIntermediaJoinObjetoPartial(int idAuditoria)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var lista = clsDLimpiezaDesinfeccionPlanta.ConsultarIntermediaJoinObjeto(idAuditoria);
                if (lista.Count != 0)
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

        public ActionResult ConsultarDetallePartial(int idLimpiezaDesinfeccionPlanta, int op, string turno= "TURNO_A", int idAuditoria=0)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var lista = clsDLimpiezaDesinfeccionPlanta.ConsultarJoinDetalle(idLimpiezaDesinfeccionPlanta, op, turno, idAuditoria);
                if (lista.Count != 0)
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

        public JsonResult GuardarModificarLimpiezaDetalle(List<CC_LIMPIEZA_DESINFECCION_PLANTA_DETALLE> listaDetalle, bool siActualizar=false, int idAuditoria=0)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
               
                if (listaDetalle.Any())
                {
                    var estadoReporte = clsDLimpiezaDesinfeccionPlanta.ConsultarEstadoReporte(listaDetalle[0].IdLimpiezaDesinfeccionPlanta, DateTime.MinValue);
                    if (estadoReporte.EstadoReporte)
                    {
                        return Json("4", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                    }
                    var validarHora = clsDLimpiezaDesinfeccionPlanta.ConsultarHoraAuditoria(listaDetalle[0].HoraAuditoria, listaDetalle[0].IdMantenimiento);
                    if (validarHora!=null && !siActualizar)
                    {
                        return Json("3", JsonRequestBehavior.AllowGet);
                    }
                    var valor=0;
                    foreach (var item in listaDetalle)
                    {
                        item.FechaIngresoLog = DateTime.Now;
                        item.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                        item.TerminalIngresoLog = Request.UserHostAddress;
                        item.UsuarioIngresoLog = lsUsuario[0];
                        valor = clsDLimpiezaDesinfeccionPlanta.GuardarModificarLimpiezaDetalle(item, idAuditoria);
                        if (valor==3)
                        {
                            return Json("3", JsonRequestBehavior.AllowGet);
                        }
                    }
                    
                    if (valor == 0)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                }else return Json("2", JsonRequestBehavior.AllowGet);

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

        public JsonResult GuardarModificarAccionCorrectiva(CC_LIMPIEZA_DESINFECCION_PLANTA_DETALLE model, HttpPostedFileBase dataImg)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
               
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var estadoReporte = clsDLimpiezaDesinfeccionPlanta.ConsultarEstadoReporte(model.IdLimpiezaDesinfeccionPlanta, DateTime.MinValue);
                if (estadoReporte.EstadoReporte)
                {
                    return Json("3", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                }
                string path = string.Empty;
                string NombreImg = string.Empty;
                if (dataImg != null)
                {
                    decimal mb = 1024 * 1024*5;//bytes to Mb; max 5Mb
                    var supportedTypes = new[] { "jpg", "jpeg" };
                    var fileExt = Path.GetExtension(dataImg.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        return Json("4", JsonRequestBehavior.AllowGet);//NO ES IMG
                    }
                    else if (dataImg.ContentLength> (mb))
                    {
                        return Json(dataImg.ContentLength, JsonRequestBehavior.AllowGet);//SOBREPASA EL LIMITE PERMITIDO dataImg.ContentLength=bytes convert to Mb
                    }
                    path = Server.MapPath("~/Content/Img/LimpiezaPlanta/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var date = DateTime.Now;
                    long n = long.Parse(date.ToString("yyyyMMddHHmmss"));
                    var ext2 = dataImg.FileName.Split('.');
                    var cont = ext2.Length;
                    NombreImg = "LimpiezaPlanta/LimpiezaPlanta" + n.ToString() + "." + ext2[cont - 1];
                    model.RutaFoto = NombreImg;
                }
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor = clsDLimpiezaDesinfeccionPlanta.GuardarModificarAccionCorrectiva(model);
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

        public JsonResult EliminarLimpiezaDetalle(List<CC_LIMPIEZA_DESINFECCION_PLANTA_DETALLE> model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var valor = 0;
                foreach (var item in model)
                {
                    item.FechaIngresoLog = DateTime.Now;
                    item.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    item.TerminalIngresoLog = Request.UserHostAddress;
                    item.UsuarioIngresoLog = lsUsuario[0];
                    valor = clsDLimpiezaDesinfeccionPlanta.EliminarLimpiezaDetalle(item);
                    
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

        #endregion

        #region BANDEJA
        //----------------------------------------------------BANDEJA DE APROBACION----------------------------------------------------------------------------------
        public ActionResult BandejaLimpiezaPlanta()
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

        public ActionResult BandejaLimpiezaPlantaPartial(bool estadoReporte, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var lista = clsDLimpiezaDesinfeccionPlanta.ConsultarBadejaEstado(fechaDesde, fechaHasta, estadoReporte);
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

        public ActionResult BandejaLimpiezaPlantaAprobarPartial(int idLimpiezaDesinfeccionPlanta, int op, string turno = "TURNO_A",int idAuditoria=0)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var lista = clsDLimpiezaDesinfeccionPlanta.ConsultarJoinDetalle(idLimpiezaDesinfeccionPlanta, op, turno, idAuditoria);
                if (lista.Count != 0)
                {
                    ViewBag.JqueryRotate = "1";
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
        #endregion

        #region REPORTE
        //----------------------------------------------------REPORTE----------------------------------------------------------------------------------
        public ActionResult ReporteLimpiezaPlanta()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.JqueryRotate = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
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

        public ActionResult ReporteLimpiezaPlantaPartial(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var lista = clsDLimpiezaDesinfeccionPlanta.ConsultarReporteRangoFecha(fechaDesde, fechaHasta);
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

        public ActionResult ReporteLimpiezaPlantaDetallePartial(int idLimpiezaDesinfeccionPlanta, int op, string turno = "TURNO_A", int idAuditoria=0)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDLimpiezaDesinfeccionPlanta = new clsDLimpiezaDesinfeccionPlanta();
                var lista = clsDLimpiezaDesinfeccionPlanta.ConsultarJoinDetalle(idLimpiezaDesinfeccionPlanta, op, turno, idAuditoria);
                if (lista.Count != 0)
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
        #endregion
    }
}
