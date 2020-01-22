using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ControlConsumoInsumo;
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
    public class ControlConsumoInsumoController : Controller
    {
        string[] lsUsuario = null;
        clsDError clsDError = null;
        clsDEmpleado clsDEmpleado = null;
        clsDClasificador clsDClasificador = null;
       // clsDGeneral clsDGeneral = null;
        clsDLogin clsDLogin = null;
        clsDApiOrdenFabricacion clsDApiOrdenFabricacion = null;
        clsDControlConsumoInsumo clsDControlConsumoInsumo = null;
        clsDApiProduccion clsDApiProduccion = null;

        #region CONTROL CONSUMOS INSUMOS
        // GET: ControlConsumoInsumo
        [Authorize]
        public ActionResult ControlConsumoInsumo()
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
                ViewBag.TipoTiemposMuertos = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoTipoTiemposMuertos);
              

                clsDLogin = new clsDLogin();
                var rol = clsDLogin.ValidarUsuarioRol(lsUsuario[1], clsAtributos.RolPouch);
                if (rol)
                {
                    ViewBag.Daniado = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoConsumoDaniadoPouch);
                    ViewBag.LineaNegocio = "POUCH";
                    ViewBag.OrdenesFabricacion = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaAutoclave(DateTime.Now).Where(x=> x.LineaNegocio==clsAtributos.LineaNegocioPouch).ToList();
                }
                else
                {
                    ViewBag.Daniado = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoConsumoDaniadoLata);
                    ViewBag.LineaNegocio = "ENLATADO";
                    ViewBag.OrdenesFabricacion = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaAutoclave(DateTime.Now).Where(x => x.LineaNegocio == clsAtributos.LineaNegocioEnlatado).ToList();

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


        public ActionResult ControlConsumoInsumoPartial(DateTime Fecha, string LineaNegocio, string Turno)
        {
            try
            {
              
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                
                var model = clsDControlConsumoInsumo.ConsultaControlConsumoInsumo(Fecha,LineaNegocio,Turno);
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
        public ActionResult ControlConsumoInsumo(CONTROL_CONSUMO_INSUMO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if(model==null||model.OrdenFabricacion==0||model.HoraInicio==null||model.PesoNeto==null||model.PesoEscrundido==null)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                OrdenFabricacionConsumoInsumo result = new OrdenFabricacionConsumoInsumo();
                result = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaConsumoInsumo(model.OrdenFabricacion+"").FirstOrDefault();
                if (result == null)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);

                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.CodigoProducto = result.CODIGO_PRODUCTO;
                model.CodigoMaterial = result.CODIGO_MATERIAL;
                model.Producto = result.NOMBRE_ADICIONAL;
                model.Cliente = string.IsNullOrEmpty(result.CLIENTE_CORTO)?result.CLIENTE:result.CLIENTE_CORTO;
                model.Envase = result.ENVASE;
                model.Tapa = result.TAPA??"";
                model.Destino = result.DESTINO;
                model.Marca = result.MARCA;
                

                var Mensaje = clsDControlConsumoInsumo.GuardarModificarControl(model);
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
        public ActionResult EliminarControlConsumoInsumo(CONTROL_CONSUMO_INSUMO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null || model.IdControlConsumoInsumos == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Mensaje = clsDControlConsumoInsumo.EliminarControlConsumoInsumo(model);
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

        #region CONSUMO DETALLE ENLATADO
        public ActionResult ControlConsumoInsumoDetalleEnlatadoPartial(int IdControl,string Tipo)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();

                var model = clsDControlConsumoInsumo.ConsultaConsumoDetalleLata(IdControl).Where(x=> x.Tipo == Tipo);
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
        public ActionResult GuardarConsumoInsumoDetalleEnlatado(CONSUMO_DETALLE_LATA model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDControlConsumoInsumo.GuardarModificarDetalleEnlatado(model);
                return Json(Respuesta,JsonRequestBehavior.AllowGet);
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
        public ActionResult EliminarConsumoDetalleLata(CONSUMO_DETALLE_LATA model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null || model.IdProcesoDetalleLata == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Mensaje = clsDControlConsumoInsumo.EliminarDetalleEnlatado(model);
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

        #region CONSUMO DETALLE POUCH
        public ActionResult ControlConsumoInsumoDetallePouchPartial(int IdControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();

                var model = clsDControlConsumoInsumo.ConsultaConsumoDetallePouch(IdControl);
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
        public ActionResult GuardarConsumoInsumoDetallePouch(CONSUMO_DETALLE_POUCH model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDControlConsumoInsumo.GuardarModificarDetallePouch(model);
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
        public ActionResult EliminarConsumoDetallePouch(CONSUMO_DETALLE_POUCH model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null || model.IdProcesoDetallePouch == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Mensaje = clsDControlConsumoInsumo.EliminarDetallePouch(model);
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
        public ActionResult ControlConsumoDaniadoPartial(int IdControl,string Tipo)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                clsDLogin = new clsDLogin();
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();

                var model = clsDControlConsumoInsumo.ConsultaConsumoDaniado(IdControl, Tipo);
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                var rol = clsDLogin.ValidarUsuarioRol(lsUsuario[1], clsAtributos.RolEnlatado);
                if (rol)
                {
                    ViewBag.LineaNegocio = "ENLATADO";
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
        public ActionResult GuardarConsumoDaniado(CONSUMO_DETALLE_DANIADO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDControlConsumoInsumo.GuardarModificarConsumoDaniado(model);
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
        public ActionResult EliminarConsumoDaniado(CONSUMO_DETALLE_DANIADO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDControlConsumoInsumo.EliminarConsumoDaniado(model);
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


        #region ADITIVOS
        public ActionResult ControlConsumoAditivos(int IdControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                clsDApiProduccion = new clsDApiProduccion();
                var model = clsDControlConsumoInsumo.ConsultaConsumoDetalleAditivo(IdControl);
                var Aditivos = clsDApiProduccion.ConsultaAditivos();
                foreach (var x in model)
                {
                    x.DescripcionAditivo = Aditivos.FirstOrDefault(y=>y.CodigoInsumo==x.Aditivo).Descripcion;
                }
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
        public ActionResult GuardarAditivos(CONSUMO_DETALLE_ADITIVO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null ||  string.IsNullOrEmpty(model.Aditivo))
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDControlConsumoInsumo.GuardarModificarAditivo(model);
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
        public ActionResult EliminarAditivos(CONSUMO_DETALLE_ADITIVO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Respuesta = clsDControlConsumoInsumo.EliminarAditivo(model);
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

        public JsonResult ConsultarAditivos()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDApiProduccion = new clsDApiProduccion();
                var Aditivos = clsDApiProduccion.ConsultaAditivos()
                               .GroupBy(x => new  { x.CodigoInsumo, x.Descripcion })
                               .Select(x=> new InsumosProduccion {CodigoInsumo=x.Key.CodigoInsumo,Descripcion=x.Key.Descripcion })
                                .Distinct().ToList();
               // var prueba = Aditivos.GroupBy(x=>x.CodigoInsumo).Distinct().ToList();
                return Json(Aditivos, JsonRequestBehavior.AllowGet);
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

        public JsonResult ConsultarAditivosProveedores(string CodigoInsumo)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDApiProduccion = new clsDApiProduccion();
                var Proveedores = clsDApiProduccion.ConsultaAditivos()
                                .Where(x=> x.CodigoInsumo==CodigoInsumo)
                               .Select(x => new { Proveedor = x.Proveedor })
                                .Distinct().ToList();
                return Json(Proveedores, JsonRequestBehavior.AllowGet);
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
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();

                var model = clsDControlConsumoInsumo.ConsultaConsumoDetalleTiempoMuerto(IdControl);
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
        public ActionResult GuardarTiemposMuertos(CONSUMO_TIEMPO_MUERTO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (model == null || model.HoraPara == null || string.IsNullOrEmpty(model.Tipo))
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                var Respuesta = clsDControlConsumoInsumo.GuardarModificarTiempoMuerto(model);
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
        public ActionResult EliminarTiemposMuertos(CONSUMO_TIEMPO_MUERTO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Respuesta = clsDControlConsumoInsumo.EliminarTiempoMuerto(model);
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

        #region REPORTE DE ENLATADO
        public ActionResult ReporteControlEnlatado()
        {
            try
            {
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
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

        public ActionResult ReporteControlEnlatadoPartial()
        {
            try
            {
                return PartialView();
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



        public ActionResult ImprimirReporteEnlatado()
        {
            var report = new Rotativa.ActionAsPdf("ReporteControlEnlatadoPartial");
            return report;

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
                    result = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaAutoclave(Fecha).Where(x => x.LineaNegocio == clsAtributos.LineaNegocioEnlatado).ToList();
                }
                else
                {
                    result = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaAutoclave(Fecha).Where(x => x.LineaNegocio == clsAtributos.LineaNegocioPouch).ToList();

                }
                if (!result.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);

                }
                //else
                //{
                //    clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                //    var ordenesUsadas = clsDControlConsumoInsumo.ConsultaControlConsumoInsumo(Fecha, "0").Select(x => x.OrdenFabricacion).ToList();
                //    result = result.Where(x => !ordenesUsadas.Contains(x.ORDEN_FABRICACION)).ToList();
                //}                
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
                if (result==null)
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