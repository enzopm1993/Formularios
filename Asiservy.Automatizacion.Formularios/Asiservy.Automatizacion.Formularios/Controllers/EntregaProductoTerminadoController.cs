﻿using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.EntregaProductoTerminado;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class EntregaProductoTerminadoController : Controller
    {
        string[] lsUsuario = null;
        clsDError clsDError = null;
        clsDEmpleado clsDEmpleado = null;
        clsDClasificador clsDClasificador = null;
        // clsDGeneral clsDGeneral = null;
        clsDLogin clsDLogin = null;
        clsDApiOrdenFabricacion clsDApiOrdenFabricacion = null;
        clsDEntregaProductoTerminado clsDEntregaProductoTerminado = null;
        clsDApiProduccion clsDApiProduccion = null;

        #region ENTREGA DE PRODUCTO TERMINADO
        // GET: EntregaProductoTerminado
        [Authorize]
        public ActionResult EntregaProductoTerminado()
        {
            try
            {
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                lsUsuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                clsDClasificador = new clsDClasificador();
                var Empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                ViewBag.Linea = Empleado.LINEA;
                ViewBag.CodLinea = Empleado.CODIGOLINEA;
        
                clsDLogin = new clsDLogin();
                var rol = clsDLogin.ValidarUsuarioRol(lsUsuario[1], clsAtributos.RolEtiquetadoLata);
                if (rol)
                {
                    //ViewBag.Daniado = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoConsumoDaniadoPouch);
                    ViewBag.LineaNegocio = "ENLATADO";
                    ViewBag.OrdenesFabricacion = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaAutoclave(DateTime.Now).Where(x => x.LineaNegocio == clsAtributos.LineaNegocioEnlatado).ToList();
                }
                else
                {
                    // ViewBag.Daniado = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoConsumoDaniadoLata);
                    ViewBag.LineaNegocio = "POUCH";
                    ViewBag.OrdenesFabricacion = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaAutoclave(DateTime.Now).Where(x => x.LineaNegocio == clsAtributos.LineaNegocioPouch).ToList();
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
        public ActionResult EntregaProductoTerminadoPartial(DateTime Fecha, string LineaNegocio)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();

                var model = clsDEntregaProductoTerminado.ConsultaControlProductoTerminado(Fecha, LineaNegocio);
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                return PartialView(model);
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
        public ActionResult EntregaProductoTerminado(PRODUCTO_TERMINADO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null || model.OrdenFabricacion == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                clsDEmpleado = new clsDEmpleado();
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                var empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                var result = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaConsumoInsumo(model.OrdenFabricacion+"").FirstOrDefault();
                 if(result==null)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);

                }
                model.Cliente = string.IsNullOrEmpty(result.CLIENTE_CORTO) ? result.CLIENTE : result.CLIENTE_CORTO;
                model.Producto = result.NOMBRE_PRODUCTO;
                model.CodigoSap = int.Parse(result.CODIGO_MATERIAL);
                model.OrdenVenta = string.IsNullOrEmpty(result.PEDIDO_VENTA)?0:int.Parse(result.PEDIDO_VENTA);
                //model.Etiqueta
                //model.LineaNegocio= result.
                model.Linea = empleado.CODIGOLINEA;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;     
                var Mensaje = clsDEntregaProductoTerminado.GuardarModificarControl(model);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
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
        public ActionResult EliminarEntregaProductoTerminado(PRODUCTO_TERMINADO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null || model.IdProductoTerminado == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Mensaje = clsDEntregaProductoTerminado.EliminarProductoTerminado(model);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
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



        public JsonResult ConsultarOrdenesFabricacion(DateTime Fecha, string Linea)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(Linea))
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                List<OrdenFabricacionAutoclave> result = new List<OrdenFabricacionAutoclave>();
                if (Linea == clsAtributos.LineaNegocioEnlatado)
                {
                    result = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaProductoTerminado(Fecha).Where(x => x.LineaNegocio == clsAtributos.LineaNegocioEnlatado).ToList();
                }
                else
                {
                    result = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaProductoTerminado(Fecha).Where(x => x.LineaNegocio == clsAtributos.LineaNegocioPouch).ToList();
                }
                if (!result.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);

                }                      
                return Json(result, JsonRequestBehavior.AllowGet);
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

        public JsonResult ConsultarDatosOrdenFabricacion(string Orden)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(Orden))
                {
                    return Json("0", JsonRequestBehavior.AllowGet);

                }
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                OrdenFabricacionConsumoInsumo result = new OrdenFabricacionConsumoInsumo();
                result = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaConsumoInsumo(Orden).FirstOrDefault();
                if (result == null)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);

                }
                return Json(result, JsonRequestBehavior.AllowGet);
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