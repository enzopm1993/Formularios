﻿using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.EntregaProductoTerminado;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;
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
        string[] lsUsuario { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        clsDEmpleado clsDEmpleado { get; set; } = null;
        clsDReporte clsDReporte { get; set; } = null;
        
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDLogin clsDLogin { get; set; } = null;
        clsDApiOrdenFabricacion clsDApiOrdenFabricacion { get; set; } = null;
        clsDApiProduccion clsDApiProduccion { get; set; } = null;
        clsDEntregaProductoTerminado clsDEntregaProductoTerminado { get; set; } = null;
        clsDPeriodo clsDPeriodo { get; set; } = null;
        
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
                ViewBag.MascaraInput = "1";
                lsUsuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                clsDClasificador = new clsDClasificador();
                clsDApiProduccion = new clsDApiProduccion();
                var Empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                ViewBag.Linea = Empleado.LINEA;
                ViewBag.CodLinea = Empleado.CODIGOLINEA;
                ViewBag.Daniado = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoConsumoProductoTerminado);
               // ViewBag.Material = clsDApiProduccion.ConsultaMaterialesOf()
                ViewBag.Turno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);

                clsDLogin = new clsDLogin();
                var rol = clsDLogin.ValidarUsuarioRol(lsUsuario[1], clsAtributos.RolEtiquetadoLata);
                if (rol)
                {
                     ViewBag.LineaNegocio = "ENLATADO";
                   }
                else
                {
                    ViewBag.LineaNegocio = "POUCH";
                 }

                if (!string.IsNullOrEmpty(lsUsuario[1]))
                {
                    var usuarioOpcion = clsDLogin.ValidarPermisoOpcion(lsUsuario[1], "ReporteEntregaProductoTerminado");
                    if (usuarioOpcion)
                    {
                        ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ReporteEntregaProductoTerminado";
                    }
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
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDEmpleado = new clsDEmpleado();
                lsUsuario = User.Identity.Name.Split('_');
                var Empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                var model = clsDEntregaProductoTerminado.ConsultaControlProductoTerminado(Fecha, LineaNegocio, Empleado.CODIGOLINEA);
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
                clsDPeriodo = new clsDPeriodo();
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                var empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();

                if (!clsDPeriodo.ValidaFechaPeriodo(model.FechaPaletizado))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                if (model.IdProductoTerminado>0 && clsDEntregaProductoTerminado.ConsultaControlProductoTerminado(model.IdProductoTerminado))
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }

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
                model.EstadoReporte = false;
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
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDPeriodo = new clsDPeriodo();
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null || model.IdProductoTerminado == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                if (!clsDPeriodo.ValidaFechaPeriodo(model.FechaPaletizado))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                if (model.IdProductoTerminado > 0 && clsDEntregaProductoTerminado.ConsultaControlProductoTerminado(model.IdProductoTerminado))
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }
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

        #region CONSUMO MATERIALES
        public ActionResult ControlConsumoMaterialPartial(int IdControl, int OrdenFabricacion)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                clsDLogin = new clsDLogin();
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
               
                var model = clsDEntregaProductoTerminado.ConsultaControlProductoTerminadoMateriales(IdControl, OrdenFabricacion);
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
        public ActionResult GuardarConsumoMaterial(PRODUCTO_TERMINADO_MATERIALES model, DateTime FechaPaletizado)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(FechaPaletizado))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                if (model.IdProductoTerminado > 0 && clsDEntregaProductoTerminado.ConsultaControlProductoTerminado(model.IdProductoTerminado))
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }

                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDEntregaProductoTerminado.GuardarModificarMateriales(model);
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
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
        public ActionResult EliminarConsumoMaterial(PRODUCTO_TERMINADO_MATERIALES model, DateTime FechaPaletizado)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(FechaPaletizado))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                if (model.IdProductoTerminado > 0 && clsDEntregaProductoTerminado.ConsultaControlProductoTerminado(model.IdProductoTerminado))
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }

                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDEntregaProductoTerminado.EliminarProductoTerminadoMateriales(model);
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
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

        #region CONSUMO INSUMO DETALLE
        public ActionResult EntregaProductoTerminadoDetallePartial(int IdControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDClasificador = new clsDClasificador();
                ViewBag.Turno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);

                var model = clsDEntregaProductoTerminado.ConsultaControlProductoTerminadoDetalle(IdControl);
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
        public ActionResult EntregaProductoTerminadoDetallePartial(PRODUCTO_TERMINADO_DETALLE model, DateTime FechaPaletizado)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(FechaPaletizado))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                if (model.IdProductoTerminado > 0 && clsDEntregaProductoTerminado.ConsultaControlProductoTerminado(model.IdProductoTerminado))
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Mensaje = clsDEntregaProductoTerminado.GuardarModificarDetalle(model);
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
        public ActionResult EliminarEntregaProductoTerminadoDetalle(PRODUCTO_TERMINADO_DETALLE model,DateTime FechaPaletizado)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null || model.IdProductoTerminadoDetalle == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(FechaPaletizado))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                if (model.IdProductoTerminado > 0 && clsDEntregaProductoTerminado.ConsultaControlProductoTerminado(model.IdProductoTerminado))
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Mensaje = clsDEntregaProductoTerminado.EliminarProductoTerminadoDetalle(model);
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


        #region CONSUMO DAÑADOS
        public ActionResult ControlConsumoDaniadoPartial(int IdControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                clsDLogin = new clsDLogin();
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                var model = clsDEntregaProductoTerminado.ConsultaControlProductoTerminadoDaniados(IdControl);
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
        public ActionResult GuardarConsumoDaniado(PRODUCTO_TERMINADO_DANIADOS model, DateTime FechaPaletizado)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(FechaPaletizado))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                if (model.IdProductoTerminado > 0 && clsDEntregaProductoTerminado.ConsultaControlProductoTerminado(model.IdProductoTerminado))
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDEntregaProductoTerminado.GuardarModificarDaniados(model);
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
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
        public ActionResult EliminarConsumoDaniado(PRODUCTO_TERMINADO_DANIADOS model, DateTime FechaPaletizado)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(FechaPaletizado))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                if (model.IdProductoTerminado > 0 && clsDEntregaProductoTerminado.ConsultaControlProductoTerminado(model.IdProductoTerminado))
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDEntregaProductoTerminado.EliminarProductoTerminadoDaniado(model);
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
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


        #region TIEMPOS MUERTOS
        public ActionResult ControlConsumoTiemposMuertos(int IdControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();

                var model = clsDEntregaProductoTerminado.ConsultaControlProductoTerminadoTiempoPara(IdControl);
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
        public ActionResult GuardarTiemposMuertos(PRODUCTO_TERMINADO_TIEMPO_PARADAS model, DateTime FechaPaletizado)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null || model.HoraInicio == null || model.HoraFin == null)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(FechaPaletizado))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                if (model.IdProductoTerminado > 0 && clsDEntregaProductoTerminado.ConsultaControlProductoTerminado(model.IdProductoTerminado))
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDEntregaProductoTerminado.GuardarModificarTiempoPara(model);
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
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
        public ActionResult EliminarTiemposMuertos(PRODUCTO_TERMINADO_TIEMPO_PARADAS model, DateTime FechaPaletizado)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(FechaPaletizado))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                if (model.IdProductoTerminado > 0 && clsDEntregaProductoTerminado.ConsultaControlProductoTerminado(model.IdProductoTerminado))
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Respuesta = clsDEntregaProductoTerminado.EliminarProductoTerminadoTiempoParada(model);
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
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


        #region BANDEJA DE APROBACION
        //-----------------------------------------------------VISTA DE BANDEJA DE APROBACION----------------------------------------------------------------
        [Authorize]
        public ActionResult BandejaControlCalidad()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
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

        public ActionResult BandejaControlCalidadPartial(DateTime? FechaDesde, DateTime? FechaHasta, bool Estado = false)
        {
            try
            {
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                List<spConsultaProductoTerminadoBandejaCC> poCloroCisterna = null;
                if (FechaDesde != null && FechaHasta != null)
                {
                    poCloroCisterna = clsDEntregaProductoTerminado.ConsultaControlProductoTerminadoBandejaCC(FechaDesde.Value, FechaHasta.Value, Estado);
                }
                else
                {
                    poCloroCisterna = clsDEntregaProductoTerminado.ConsultaControlProductoTerminadoBandejaCC(DateTime.Now, DateTime.Now, clsAtributos.EstadoReportePendiente);
                }
                if (poCloroCisterna != null && poCloroCisterna.Any())
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
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        //public ActionResult BandejaAprobarAnalisisAguaCalderos(DateTime fecha)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }
        //        clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
        //        var poCloroCisterna = clsDEntregaProductoTerminado.consu(fecha);
        //        if (poCloroCisterna != null && poCloroCisterna.Any())
        //        {
        //            return Json(poCloroCisterna, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("0", JsonRequestBehavior.AllowGet);
        //        }
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

        public ActionResult AprobarBandejaControl(PRODUCTO_TERMINADO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDPeriodo = new clsDPeriodo();
                model.FechaAprobacion = DateTime.Now;
                model.AprobadoPor = lsUsuario[0];
                model.EstadoReporte = clsAtributos.EstadoReporteActivo;

                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                if (!clsDPeriodo.ValidaFechaPeriodo(model.FechaPaletizado))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado.Aprobar_ReporteProductoTerminado(model);
                return Json("Aprobación Exitosa", JsonRequestBehavior.AllowGet);
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

        public ActionResult ReversarBandejaControl(PRODUCTO_TERMINADO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDPeriodo = new clsDPeriodo();
                model.FechaAprobacion = null;
                model.AprobadoPor = null;
                model.EstadoReporte = clsAtributos.EstadoReportePendiente;

                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                if (!clsDPeriodo.ValidaFechaPeriodo(model.FechaPaletizado))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado.Aprobar_ReporteProductoTerminado(model);
                return Json("Reporte reversado exitosamente", JsonRequestBehavior.AllowGet);
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

        #region REPORTE
        [Authorize]
        public ActionResult ReporteEntregaProductoTerminado()
        {
            try
            {
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.select2 = "1";
                clsDClasificador = new clsDClasificador();
                ViewBag.Lineas = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoLineasEntregaProductoTerminado);
                clsDLogin = new clsDLogin();
                lsUsuario = User.Identity.Name.Split('_');
                clsDReporte = new clsDReporte();
                var rep = clsDReporte.ConsultaCodigoReporte(RouteData.Values["action"].ToString());
                if (rep != null)
                {
                    ViewBag.CodigoReporte = rep.Codigo;
                    ViewBag.VersionReporte = rep.UltimaVersion;
                    ViewBag.NombreReporte = rep.Nombre;
                }
                if (!string.IsNullOrEmpty(lsUsuario[1]))
                {
                    var usuarioOpcion = clsDLogin.ValidarPermisoOpcion(lsUsuario[1], "EntregaProductoTerminado");
                    if (usuarioOpcion)
                    {
                        ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "EntregaProductoTerminado";
                    }
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

        public ActionResult ReporteEntregaProductoTerminadoPartial(DateTime FechaDesde, DateTime FechaHasta, string Linea)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDEmpleado = new clsDEmpleado();
                var model = clsDEntregaProductoTerminado.ReporteConsultaControlProductoTerminado(FechaDesde, FechaHasta, Linea);
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

        public ActionResult ReporteControlConsumoMaterialPartial(int IdControl, int OrdenFabricacion)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                clsDLogin = new clsDLogin();
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                var model = clsDEntregaProductoTerminado.ConsultaControlProductoTerminadoMateriales(IdControl, OrdenFabricacion);
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

        public ActionResult ReporteEntregaProductoTerminadoDetallePartial(int IdControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();
                clsDClasificador = new clsDClasificador();
                var model = clsDEntregaProductoTerminado.ConsultaControlProductoTerminadoDetalle(IdControl);
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                ViewBag.Turno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);

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

        public ActionResult ReporteControlConsumoTiemposMuertos(int IdControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEntregaProductoTerminado = new clsDEntregaProductoTerminado();

                var model = clsDEntregaProductoTerminado.ConsultaControlProductoTerminadoTiempoPara(IdControl);
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
        #endregion



        public JsonResult ConsultarBodegas(int OF)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (OF==0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                var result = clsDApiOrdenFabricacion.ConsultaCantidadesOrdenBodega(OF);
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


        public JsonResult ConsultarMateriales(int OF)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (OF == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                clsDApiProduccion = new clsDApiProduccion();
                var result = clsDApiProduccion.ConsultaMaterialesOf(OF);
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