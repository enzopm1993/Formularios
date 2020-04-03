using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ProyeccionProgramacion;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
using Asiservy.Automatizacion.Formularios.Models.ProyeccionProgramacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using Asiservy.Automatizacion.Formularios.Models;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ProyeccionProgramacionController : Controller
    {
        clsDError clsDError = null;
        clsDClasificador clsDClasificador = null;
        clsDProyeccionProgramacion clsDProyeccionProgramacion = null;
        clsDApiProduccion clsDApiProduccion = null;
        clsDGeneral clsDGeneral = null;
        clsDApiOrdenFabricacion clsDApiOrdenFabricacion = null;
        clsDLogin clsDLogin = null;
        string[] lsUsuario;

        #region Métodos
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        #endregion


        [Authorize]
        public ActionResult FinalizaProyeccionProgramacionPreparacion()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                //var model = clsDProyeccionProgramacion.ConsultaProyeccionProgramacion
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

        public ActionResult FinalizaProyeccionProgramacionPreparacionPartial(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                var model = clsDProyeccionProgramacion.ConsultaProyeccionProgramacion(fechaDesde, fechaHasta);
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

        #region EDITAR PREPARACION
        [Authorize]
        public ActionResult EditarProyeccionProgramacionPreparacion()
        {
            try
            {
                clsDClasificador = new clsDClasificador();
                var ListRecetas = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoRecetaRoceado, "0");
                ViewBag.Receta = ListRecetas;
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                TimeSpan tiempo1 = new TimeSpan(0, 0, 0);
                TimeSpan tiempo2 = new TimeSpan(24, 0, 0);
                TimeSpan incremento = new TimeSpan(0, 15, 0);
                List<string> Horas = new List<string>();
                while (tiempo1 < tiempo2)
                {
                    string Hora;
                    string Minuto;
                    if (tiempo1.Hours < 10)
                        Hora = "0" + tiempo1.Hours;
                    else
                        Hora = tiempo1.Hours + "";
                    if (tiempo1.Minutes < 10)
                        Minuto = "0" + tiempo1.Minutes;
                    else
                        Minuto = tiempo1.Minutes + "";
                    Horas.Add(Hora + ":" + Minuto);
                    tiempo1 = tiempo1.Add(incremento);
                }
                ViewBag.horas = Horas;

                ViewBag.Cocinas = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoCocinas, "0");

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

        public JsonResult ValidarProyeccionProgramacionPreparacion(DateTime Fecha, string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                RespuestaGeneral respuesta = new RespuestaGeneral();
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                int idProyeccion = clsDProyeccionProgramacion.ValidarProyeccionProgramacion(Fecha, Turno);
                if (idProyeccion > 0)
                {
                    var pro = clsDProyeccionProgramacion.ConsultaProyeccionProgramacion(idProyeccion);
                    if (pro.Finaliza)
                    {
                        respuesta.Codigo = 4;
                        respuesta.Mensaje = "Proyección se encuentra cerrada";
                        respuesta.Observacion = idProyeccion + "";
                    }
                    else if (pro.IngresoPreparacion)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "Control se encuentra en ingreso de preparación";
                        respuesta.Observacion = idProyeccion + "";
                    }
                    else if (pro.EditaProduccion)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "Control se encuentra en producción";
                        respuesta.Observacion = idProyeccion + "";
                    }
                    else if (!pro.EditarPreparacion)
                    {
                        respuesta.Codigo = 3;
                        respuesta.Mensaje = "Control se encuentra en finalizada";
                        respuesta.Observacion = idProyeccion + "";
                    }
                    else
                    {
                        respuesta.Codigo = 1;
                        respuesta.Observacion = idProyeccion + "";
                    }
                }
                else
                {
                    respuesta.Codigo = 0;
                    respuesta.Mensaje = "No existen registros";
                }
                return Json(respuesta, JsonRequestBehavior.AllowGet);
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

        #region EDITAR PRODUCCION
        [Authorize]
        public ActionResult EditarProyeccionProgramacionProduccion()
        {
            try
            {
                clsDClasificador = new clsDClasificador();
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                var ListLineas = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.Lineas = ListLineas;
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
        public JsonResult ValidarProyeccionProgramacionProduccion(DateTime Fecha, string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                RespuestaGeneral respuesta = new RespuestaGeneral();
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                int idProyeccion = clsDProyeccionProgramacion.ValidarProyeccionProgramacion(Fecha, Turno);
                if (idProyeccion > 0)
                {
                    var pro = clsDProyeccionProgramacion.ConsultaProyeccionProgramacion(idProyeccion);
                    if (pro.Finaliza)
                    {
                        respuesta.Codigo = 4;
                        respuesta.Mensaje = "Proyección se encuentra cerrada";
                        respuesta.Observacion = idProyeccion + "";
                    }
                    else if (pro.IngresoPreparacion)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "Proyección esta siendo ingresado en preparación";
                        respuesta.Observacion = idProyeccion + "";
                    }
                    else if (pro.EditarPreparacion)
                    {
                        respuesta.Codigo = 3;
                        respuesta.Mensaje = "Proyección esta siendo editado por preparación";
                        respuesta.Observacion = idProyeccion + "";
                    }
                    else if (pro.EditaProduccion)
                    {
                        respuesta.Codigo = 1;
                        respuesta.Observacion = idProyeccion + "";
                    }
                    else
                    {
                        respuesta.Codigo = 4;
                        respuesta.Mensaje = "Proyección se encuentra finalizada";
                        respuesta.Observacion = idProyeccion + "";
                    }
                }
                else
                {
                    respuesta.Codigo = 0;
                    respuesta.Mensaje = "No existen registros";
                }
                return Json(respuesta, JsonRequestBehavior.AllowGet);
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

        #region INGRESO PROGRAMACION
        // GET: ProyeccionProgramacion
        [Authorize]
        public ActionResult ProyeccionProgramacion()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.Select2 = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDClasificador = new clsDClasificador();
                clsDApiProduccion = new clsDApiProduccion();
                //clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();

                var ListLimpiezaPescado = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodigoGrupoTipoLimpiezaPescado, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.TipoLimpieza = new SelectList(ListLimpiezaPescado, "codigo", "descripcion");
                var ListDestinoProduccion = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodigoGrupoDestinoProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                var ListMareas = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoMarea, "0");
                ViewBag.Marea = ListMareas;
                ViewBag.Destino = new SelectList(ListDestinoProduccion, "codigo", "descripcion");
                ViewBag.Especie = clsDApiProduccion.ConsultarEspecies();
                ViewBag.Talla = clsDApiProduccion.ConsultarTallas(null);
                ViewBag.OrdenFabricacion = clsDApiProduccion.ConsultarLotesPorFecha(DateTime.Now);
                ViewBag.Barco = clsDApiProduccion.ConsultarBarcos();

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


        public ActionResult ProyeccionProgramacionDetallePartial(int IdProgramacion, int? proceso)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                var model = clsDProyeccionProgramacion.ConsultaProyeccionProgramacionDetalle(IdProgramacion);
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                if (proceso != null && proceso == 2)
                {
                    ViewBag.EditaProduccion = "2";
                }
                if (proceso != null && proceso == 3)
                {
                    clsDGeneral = new clsDGeneral();
                    var ListadoPreparacion = clsDGeneral.ConsultarMantenimientoPreparacion();
                    ViewBag.EditaPreparacion = "3";
                    ViewBag.ListadoEnfriado = ListadoPreparacion.Select(x => new { x.Talla, x.HorasDescongelado });
                    ViewBag.ListadoCoccion = ListadoPreparacion.Select(x => new { x.Talla, x.HorasCoccion });



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

        public JsonResult ValidarProyeccionProgramacionEstado(DateTime Fecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                RespuestaGeneral respuesta = new RespuestaGeneral();
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                int Estado = clsDProyeccionProgramacion.ValidarProyeccionProgramacionEstado(Fecha);
                return Json(Estado, JsonRequestBehavior.AllowGet);
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
        public JsonResult ValidarProyeccionProgramacion(DateTime Fecha,string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                RespuestaGeneral respuesta = new RespuestaGeneral();
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                int idProyeccion = clsDProyeccionProgramacion.ValidarProyeccionProgramacion(Fecha, Turno);
                if(idProyeccion>0)
                {
                    var pro = clsDProyeccionProgramacion.ConsultaProyeccionProgramacion(idProyeccion);
                    if (pro.Finaliza)
                    {
                        respuesta.Codigo = 4; 
                        respuesta.Mensaje = "Proyección se encuentra cerrada";
                        respuesta.Observacion = idProyeccion + "";          
                    }
                    else if (pro.IngresoPreparacion)
                    {
                        respuesta.Codigo = 1;
                        respuesta.Observacion = idProyeccion + "";
                    }
                    else if (pro.EditaProduccion)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "Proyección se encuentra en producción";
                        respuesta.Observacion= idProyeccion + "";
                    }
                    else if (pro.EditarPreparacion)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje = "Proyección está siendo editado en preparación";
                        respuesta.Observacion = idProyeccion + "";
                    }
                    else
                    {
                        respuesta.Codigo = 4;
                        respuesta.Mensaje = "Proyección se encuentra finalizada";
                        respuesta.Observacion = idProyeccion + "";
                    }
                }
                else
                {
                    respuesta.Codigo = 0;
                    respuesta.Mensaje = "No existen registros";
                }
                return Json(respuesta, JsonRequestBehavior.AllowGet);
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

        public JsonResult GenerarProyeccionProgramacion(PROYECCION_PROGRAMACION model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.FechaIngresoLog = DateTime.Now;
                model.IngresoPreparacion = true; 
                model.EditarPreparacion = false;
                model.EditaProduccion = false;
                model.Finaliza = false;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                var Genera= clsDProyeccionProgramacion.GenerarProyeccionProgramacion(model);
                return Json(Genera, JsonRequestBehavior.AllowGet);
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

        public JsonResult EliminarProyeccionProgramacionDetalle(int id)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                PROYECCION_PROGRAMACION_DETALLE model= new PROYECCION_PROGRAMACION_DETALLE();
                model.IdProyeccionProgramacionDetalle = id;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                string Lote =clsDProyeccionProgramacion.EliminarProyeccionProgramacionDetalle(model);
                return Json("Se elimino correctamente el lote: "+ Lote, JsonRequestBehavior.AllowGet);
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


        [HttpPost]
        public JsonResult GuardarModificarProyeccionProgramacionDetalle(PROYECCION_PROGRAMACION_DETALLE model, int? proceso)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                if (proceso != null && proceso == 2)
                {
                    clsDProyeccionProgramacion.GuardarModificarProyeccionProgramacionDetalle(model, 2); //editar desde producción
                }
                else if (proceso != null && proceso == 3)
                {
                    RespuestaGeneral respuesta = new RespuestaGeneral();
                    if (model.HoraCoccionInicio > model.HoraCoccionFin)
                    {
                        respuesta.Codigo = 2;
                        respuesta.Mensaje= "Fecha de inicio de cocción no puede ser menor a la fecha fin";
                        return Json(respuesta, JsonRequestBehavior.AllowGet);
                    }
                    if (model.HoraEviceradoInicio > model.HoraEviceradoFin)
                    {
                        respuesta.Codigo = 3;
                        respuesta.Mensaje= "Fecha de inicio de evicerado no puede ser menor a la fecha fin";
                        return Json(respuesta, JsonRequestBehavior.AllowGet);
                    }
                    if (model.HoraDescongeladoInicio > model.HoraDescongeladoFin)
                    {
                        respuesta.Codigo = 4;
                        respuesta.Mensaje= "Fecha de inicio de descongelado no puede ser menor a la fecha fin";
                        return Json(respuesta, JsonRequestBehavior.AllowGet);
                    }


                    //string mensaje = clsDProyeccionProgramacion.validarCocinas(model);
                    //if (string.IsNullOrEmpty(mensaje))
                    //{
                        clsDProyeccionProgramacion.GuardarModificarProyeccionProgramacionDetalle(model, 3); //Editar Proyección de la programación en preparación
                    //}
                    //else
                    //{
                       
                    //    respuesta.Codigo = 1;
                    //    string[] resp = mensaje.Split('-');
                    //    respuesta.Mensaje ="Cocina: "+ resp[1] + ", está siendo utilizado por el lote: "+ resp[0];
                    //    return Json(respuesta, JsonRequestBehavior.AllowGet);
                    //}

                }
                else
                {
                    clsDProyeccionProgramacion.GuardarModificarProyeccionProgramacionDetalle(model, 1); //Ingreso de la programación
                }
                return Json("Registro guardado correctamente", JsonRequestBehavior.AllowGet);
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

        public JsonResult InactivarProyeccionProgramacionDetalle(int id)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                PROYECCION_PROGRAMACION model = new PROYECCION_PROGRAMACION();
                model.IdProyeccionProgramacion = id;               
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                clsDProyeccionProgramacion.InactivarProyeccionProgramacion(model);
                return Json("Registro ha sido eliminado con éxito.", JsonRequestBehavior.AllowGet);
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

        public JsonResult FinalizarIngresoProyeccionProgramacion(int id,int proceso, string Observacion)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                if (proceso == 1)
                {
                    clsDProyeccionProgramacion.EditarProyeccionProgramacion(id,false,true,false,null,lsUsuario[0],Request.UserHostAddress);
                }
                if (proceso == 2)
                {
                    clsDProyeccionProgramacion.EditarProyeccionProgramacion(id,null,false,true,null,lsUsuario[0],Request.UserHostAddress);
                }
                if (proceso == 3)
                {
                    clsDProyeccionProgramacion.EditarProyeccionProgramacion(id, null, null, false, null, lsUsuario[0], Request.UserHostAddress);
                }
                if (proceso == 4)
                {
                    clsDProyeccionProgramacion.EditarProyeccionProgramacion(id, null, null, false, true, lsUsuario[0], Request.UserHostAddress,Observacion);
                }
                return Json("Registro modificado con éxito.", JsonRequestBehavior.AllowGet);
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


        public JsonResult HabilitarIngresoProyeccionProgramacion(int id, int proceso)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                if (proceso == 1)
                {
                    clsDProyeccionProgramacion.EditarProyeccionProgramacion(id, true, false, false, null, lsUsuario[0], Request.UserHostAddress);
                }
                if (proceso == 2)
                {
                    clsDProyeccionProgramacion.EditarProyeccionProgramacion(id, null, true, false, null, lsUsuario[0], Request.UserHostAddress);
                }
                if (proceso == 3)
                {
                    clsDProyeccionProgramacion.EditarProyeccionProgramacion(id, null, null, true, null, lsUsuario[0], Request.UserHostAddress);
                }
                if (proceso == 4)
                {
                    clsDProyeccionProgramacion.EditarProyeccionProgramacion(id, null, null, true, false, lsUsuario[0], Request.UserHostAddress);
                }

                return Json("Registro modificado con éxito.", JsonRequestBehavior.AllowGet);
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


        [Authorize]
        public ActionResult ReporteProyeccionProgramacion()
        {
            try
            {
                ViewBag.dataTableJS = "1";
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

        
        public ActionResult ReporteProyeccionProgramacionPartial(DateTime Fecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                clsDLogin = new clsDLogin();
                var rolCamara = clsDLogin.ValidarUsuarioRol(lsUsuario[1],clsAtributos.RolCamara);
                if (rolCamara)
                {
                    ViewBag.RolCamara = rolCamara;
                }

                var rolAsistenteProduccion = clsDLogin.ValidarUsuarioRol(lsUsuario[1], clsAtributos.AsistenteProduccion);
                if (rolAsistenteProduccion)
                {
                    ViewBag.rolAsistenteProduccion = rolAsistenteProduccion;
                }


                var modal = clsDProyeccionProgramacion.ConsultaProyeccionProgramacionReporte(Fecha);
                return PartialView(modal);
            }
            catch (Exception ex)
            {

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult ConsultarOrdenesFabricacion(DateTime Fecha)
        {
            try
            {
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                dynamic result = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaProduccion(Fecha);
                List<OrdenFabricacion> Listado = new List<OrdenFabricacion>();
                foreach (var x in result)
                {
                    Listado.Add(new OrdenFabricacion { Orden = x.OrdenFabricacion });
                }
                return Json(Listado, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult ProyeccionProgramacionPartial(int? IdProyeccionProgramacion,string OfLote,DateTime? FechaProduccion,int? Toneladas,string Destino, string TipoLimpieza,string Observacion, string Especie, string Talla/*,string Lineas, TimeSpan HoraInicio, TimeSpan HoraFin*/)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        PROYECCION_PROGRAMACION ProyeccionProgramacion = null;
        //        //if (string.IsNullOrEmpty(Lineas))
        //        //{
        //           ProyeccionProgramacion = new PROYECCION_PROGRAMACION()
        //            {

        //                IdProyeccionProgramacion = Convert.ToInt32(IdProyeccionProgramacion),
        //                OfLote = OfLote,
        //                FechaProduccion = FechaProduccion,
        //                Toneladas = Toneladas,
        //                Destino = Destino,
        //                TipoLimpieza = TipoLimpieza,
        //                Observacion = Observacion,
        //                Especie=Especie,
        //                Talla=Talla,
        //                FechaCreacionLog=DateTime.Now,
        //                UsuarioCreacionLog=lsUsuario[0],
        //                TerminalCreacionLog= Request.UserHostAddress

        //           };
        //        //}
        //        //else
        //        //{
        //        //    ProyeccionProgramacion = new PROYECCION_PROGRAMACION()
        //        //    {

        //        //        IdProyeccionProgramacion = IdProyeccionProgramacion,
        //        //        Lineas=Lineas,
        //        //        HoraInicio=HoraInicio,
        //        //        HoraFin=HoraFin

        //        //    };
        //        //}
        //        List<ProyeccionProgramacionViewModel> Respuesta=new List<ProyeccionProgramacionViewModel>();
        //        clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
        //        if (!string.IsNullOrEmpty(OfLote)){
        //            Respuesta = clsDProyeccionProgramacion.GuardarActualizarProyeccionProgramacion(ProyeccionProgramacion);

        //        }
        //        else
        //        {
        //            Respuesta = clsDProyeccionProgramacion.ConsultarProyeccionProgramacion(FechaProduccion);
        //        }
        //        //var Respuesta = clsDProyeccionProgramacion.GuardarActualizarProyeccionProgramacion(ProyeccionProgramacion);
        //        return PartialView(Respuesta);
        //    }
        //    catch (Exception ex)
        //    {

        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        clsDError.GrabarError(new ERROR
        //        {
        //            Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            Mensaje = ex.Message,
        //            Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
        //            FechaIngreso = DateTime.Now,
        //            TerminalIngreso = Request.UserHostAddress,
        //            UsuarioIngreso = "sistemas"
        //        });
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //[HttpPost]
        //public ActionResult ProyeccionProgramacionEditPartial(DateTime Fecha)
        //{
        //    try
        //    {
        //        clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
        //        var resultado = clsDProyeccionProgramacion.ConsultarProyeccionProgramacion(Fecha);
        //        return PartialView(resultado);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        clsDError.GrabarError(new ERROR
        //        {
        //            Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            Mensaje = ex.Message,
        //            Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
        //            FechaIngreso = DateTime.Now,
        //            TerminalIngreso = Request.UserHostAddress,
        //            UsuarioIngreso = "sistemas"
        //        });
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //[HttpPost]
        //public ActionResult ProyeccionProgramacionEditarPartial(int IdProyeccionProgramacion,string Lineas, TimeSpan HoraInicio, TimeSpan HoraFin,string Observacion, DateTime FechaProduccion)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        PROYECCION_PROGRAMACION ProyeccionProgramacion = null;
        //        ProyeccionProgramacion = new PROYECCION_PROGRAMACION()
        //            {

        //                IdProyeccionProgramacion = IdProyeccionProgramacion,
        //                Lineas = Lineas,
        //                HoraInicio = HoraInicio,
        //                HoraFin = HoraFin,
        //                Observacion=Observacion,
        //                UsuarioCreacionLog = lsUsuario[0],
        //                TerminalCreacionLog = Request.UserHostAddress,
        //                FechaCreacionLog=DateTime.Now,
        //                FechaProduccion=FechaProduccion
        //        };

        //        clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
        //        var Respuesta = clsDProyeccionProgramacion.GuardarActualizarProyeccionProgramacion(ProyeccionProgramacion);
        //        return PartialView(Respuesta);
        //    }
        //    catch (Exception ex)
        //    {

        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        clsDError.GrabarError(new ERROR
        //        {
        //            Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            Mensaje = ex.Message,
        //            Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
        //            FechaIngreso = DateTime.Now,
        //            TerminalIngreso = Request.UserHostAddress,
        //            UsuarioIngreso = "sistemas"
        //        });
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //[Authorize]
        //public ActionResult ProyeccionProgramacionEditar()
        //{
        //    try
        //    {
        //        ViewBag.dataTableJS = "1";
        //        ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

        //        clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
        //        ViewBag.Proyeccion = clsDProyeccionProgramacion.ConsultarProyeccionProgramacion(null);
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {

        //        SetErrorMessage(ex.Message);
        //        clsDError = new clsDError();
        //        clsDError.GrabarError(new ERROR
        //        {
        //            Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            Mensaje = ex.Message,
        //            Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
        //            FechaIngreso = DateTime.Now,
        //            TerminalIngreso = Request.UserHostAddress,
        //            UsuarioIngreso = "sistemas"
        //        });
        //        return RedirectToAction("Home", "Home");
        //    }
        //}
        //public ActionResult ModalEditarProyeccion(int IdProyeccion,string Observacion,string Lineas,TimeSpan? HoraInicio,TimeSpan? HoraFin)
        //{
        //    try
        //    {
        //        clsDClasificador = new clsDClasificador();
        //        var ListLineas = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
        //        ViewBag.IdProyeccion = IdProyeccion;
        //        ViewBag.Lineas = ListLineas;
        //        string[] LineasProduccion = Lineas.Split(',');
        //        ViewBag.LineasSelec = LineasProduccion.ToList();
        //        var a= LineasProduccion.ToList();
        //        var b=a[0];
        //        ViewBag.Obsercacion = Observacion;
        //        ViewBag.HoraInicio = HoraInicio;
        //        ViewBag.HoraFin = HoraFin;
        //        return PartialView();
        //    }
        //    catch (Exception ex)
        //    {

        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        clsDError.GrabarError(new ERROR
        //        {
        //            Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            Mensaje = ex.Message,
        //            Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
        //            FechaIngreso = DateTime.Now,
        //            TerminalIngreso = Request.UserHostAddress,
        //            UsuarioIngreso = "sistemas"
        //        });
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}


        //[Authorize]
        //public ActionResult ReporteProyeccionProgramacion()
        //{
        //    try
        //    {
        //        ViewBag.dataTableJS = "1";
        //        ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];


        //        //clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
        //        //var modal = clsDProyeccionProgramacion.ConsultarProyeccionProgramacion(null);
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {

        //        SetErrorMessage(ex.Message);
        //        clsDError = new clsDError();
        //        clsDError.GrabarError(new ERROR
        //        {
        //            Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            Mensaje = ex.Message,
        //            Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
        //            FechaIngreso = DateTime.Now,
        //            TerminalIngreso = Request.UserHostAddress,
        //            UsuarioIngreso = "sistemas"
        //        });
        //        return RedirectToAction("Home", "Home");
        //    }
        //}
        //[Authorize]
        //public ActionResult ReporteProyeccionProgramacionPartial(DateTime Fecha)
        //{
        //    try
        //    {
        //        clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
        //        var modal = clsDProyeccionProgramacion.ConsultarProyeccionProgramacion(Fecha);
        //        return PartialView(modal);
        //    }
        //    catch (Exception ex)
        //    {

        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        clsDError.GrabarError(new ERROR
        //        {
        //            Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            Mensaje = ex.Message,
        //            Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
        //            FechaIngreso = DateTime.Now,
        //            TerminalIngreso = Request.UserHostAddress,
        //            UsuarioIngreso = "sistemas"
        //        });
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}


    }
}