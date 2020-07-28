using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ProyeccionProgramacion;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
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
        clsDError clsDError { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDProyeccionProgramacion clsDProyeccionProgramacion { get; set; } = null;
        clsDApiProduccion clsDApiProduccion { get; set; } = null;
        clsDGeneral clsDGeneral { get; set; } = null;
        clsDPeriodo clsDPeriodo { get; set; } = null;
        clsDApiOrdenFabricacion clsDApiOrdenFabricacion { get; set; } = null;
        clsDLogin clsDLogin { get; set; } = null;
        string[] lsUsuario { get; set; }

        #region Métodos
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }

        public JsonResult ConsultarOrdenesFabricacion(DateTime Fecha)
        {
            try
            {
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                dynamic result = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaProduccion(Fecha);
                List<OrdenFabricacion> Listado = new List<OrdenFabricacion>();
                if (result != null)
                {
                    foreach (var x in result)
                    {
                        Listado.Add(new OrdenFabricacion { Orden = x.OrdenFabricacion });
                    }
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
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
        #endregion

        #region Finaliza Proyeccion 
        [Authorize]
        public ActionResult FinalizaProyeccionProgramacionPreparacion()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
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
                if (!User.Identity.IsAuthenticated)
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
        #endregion

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
                if (!User.Identity.IsAuthenticated)
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
                if (!User.Identity.IsAuthenticated)
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
                if (!User.Identity.IsAuthenticated)
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

        public JsonResult ValidarProyeccionProgramacionEstado(DateTime Fecha, string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                int Estado = clsDProyeccionProgramacion.ValidarProyeccionProgramacionEstado(Fecha, Turno);
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
                if (!User.Identity.IsAuthenticated)
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
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(model.FechaProduccion))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
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

        public JsonResult EliminarProyeccionProgramacionDetalle(int id, DateTime Fecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
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

        #region METODOS
        [HttpPost]
        public JsonResult GuardarModificarProyeccionProgramacionDetalle(PROYECCION_PROGRAMACION_DETALLE model, int? proceso, DateTime FechaProduccion)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(FechaProduccion))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
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

                        clsDProyeccionProgramacion.GuardarModificarProyeccionProgramacionDetalle(model, 3); //Editar Proyección de la programación en preparación
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

        public JsonResult InactivarProyeccionProgramacionDetalle(int id,DateTime Fecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
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

        public JsonResult FinalizarIngresoProyeccionProgramacion(int id,int proceso, string Observacion,DateTime Fecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
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


        public JsonResult HabilitarIngresoProyeccionProgramacion(int id, int proceso, DateTime Fecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
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
        #endregion

        #region REPORTE
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

        
        public ActionResult ReporteProyeccionProgramacionPartial(DateTime Fecha, string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
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


                var modal = clsDProyeccionProgramacion.ConsultaProyeccionProgramacionReporte(Fecha, Turno);
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
        #endregion


        #region CAMBIA ESTADO LOTE 
        [Authorize]
        public ActionResult ProyeccionProgramacionEstadoLote()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDClasificador = new clsDClasificador();
                ViewBag.Turno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);
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


        public ActionResult ProyeccionProgramacionEstadoLotePartial(DateTime Fecha, string Turno )
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                var model = clsDProyeccionProgramacion.ConsultaProyeccionProgramacionReporte(Fecha,Turno);
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
        public JsonResult CerrarLote(PROYECCION_PROGRAMACION_DETALLE control, DateTime Fecha)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                if (!clsDPeriodo.ValidaFechaPeriodo(Fecha))
                {
                    return Json("800", JsonRequestBehavior.AllowGet);
                }
                lsUsuario = User.Identity.Name.Split('_');
                control.UsuarioIngresoLog = lsUsuario[0];
                control.TerminalIngresoLog = Request.UserHostAddress;
                control.FechaIngresoLog = DateTime.Now;
                
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                clsDProyeccionProgramacion.CerrarProyeccionProgramacion(control);
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
        #endregion


    }
}