using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ProyeccionProgramacion;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ProyeccionProgramacionController : Controller
    {
        clsDError clsDError = null;
        clsDClasificador clsDClasificador = null;
        clsDProyeccionProgramacion clsDProyeccionProgramacion = null;
        string[] liststring;

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
        // GET: ProyeccionProgramacion
        [Authorize]
        public ActionResult ProyeccionProgramacion()
        {
            try
            {
                clsDClasificador = new clsDClasificador();
                var ListLimpiezaPescado = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodigoGrupoTipoLimpiezaPescado, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                ViewBag.TipoLimpieza = new SelectList(ListLimpiezaPescado, "codigo", "descripcion");

                var ListDestinoProduccion = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodigoGrupoDestinoProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                ViewBag.Destino = new SelectList(ListDestinoProduccion, "codigo", "descripcion");

                var ListEspeciePescado = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodigoGrupoEspeciePescado, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                ViewBag.Especie = new SelectList(ListEspeciePescado, "codigo", "descripcion");

                var ListTallaPescado = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodigoGrupoTallaPescado, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                ViewBag.Talla = new SelectList(ListTallaPescado, "codigo", "descripcion");

                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                ViewBag.Proyeccion = clsDProyeccionProgramacion.ConsultarProyeccionProgramacion(null);
                return View();
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
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
                return RedirectToAction("Home", "Home");
            }
        }

        [HttpPost]
        public ActionResult ProyeccionProgramacionPartial(int IdProyeccionProgramacion,string Lote,DateTime? FechaProduccion,int? Toneladas,string Destino, string TipoLimpieza,string Observacion, string Especie, string Talla/*,string Lineas, TimeSpan HoraInicio, TimeSpan HoraFin*/)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                PROYECCION_PROGRAMACION ProyeccionProgramacion = null;
                //if (string.IsNullOrEmpty(Lineas))
                //{
                   ProyeccionProgramacion = new PROYECCION_PROGRAMACION()
                    {

                        IdProyeccionProgramacion = IdProyeccionProgramacion,
                        Lote = Lote,
                        FechaProduccion = FechaProduccion,
                        Toneladas = Toneladas,
                        Destino = Destino,
                        TipoLimpieza = TipoLimpieza,
                        Observacion = Observacion,
                        
                        FechaCreacionLog=DateTime.Now,
                        UsuarioCreacionLog=liststring[0],
                        TerminalCreacionLog= Request.UserHostAddress

                   };
                //}
                //else
                //{
                //    ProyeccionProgramacion = new PROYECCION_PROGRAMACION()
                //    {

                //        IdProyeccionProgramacion = IdProyeccionProgramacion,
                //        Lineas=Lineas,
                //        HoraInicio=HoraInicio,
                //        HoraFin=HoraFin

                //    };
                //}
               
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                var Respuesta = clsDProyeccionProgramacion.GuardarActualizarProyeccionProgramacion(ProyeccionProgramacion);
                return PartialView(Respuesta);
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
        [HttpPost]
        public ActionResult ProyeccionProgramacionEditarPartial(int IdProyeccionProgramacion,string Lineas, TimeSpan HoraInicio, TimeSpan HoraFin)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                PROYECCION_PROGRAMACION ProyeccionProgramacion = null;
                ProyeccionProgramacion = new PROYECCION_PROGRAMACION()
                    {

                        IdProyeccionProgramacion = IdProyeccionProgramacion,
                        Lineas = Lineas,
                        HoraInicio = HoraInicio,
                        HoraFin = HoraFin,
                        UsuarioCreacionLog = liststring[0],
                        TerminalCreacionLog = Request.UserHostAddress,
                        FechaCreacionLog=DateTime.Now
                };
               
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                var Respuesta = clsDProyeccionProgramacion.GuardarActualizarProyeccionProgramacion(ProyeccionProgramacion);
                return PartialView(Respuesta);
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
        [Authorize]
        public ActionResult ProyeccionProgramacionEditar()
        {
            try
            {
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                ViewBag.Proyeccion = clsDProyeccionProgramacion.ConsultarProyeccionProgramacion(null);
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
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
                return RedirectToAction("Home", "Home");
            }
        }
        public ActionResult ModalEditarProyeccion(int IdProyeccion)
        {
            try
            {
                clsDClasificador = new clsDClasificador();
                var ListLineas = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.IdProyeccion = IdProyeccion;
                ViewBag.Lineas = ListLineas;
                return PartialView();
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


        [Authorize]
        public ActionResult ReporteProyeccionProgramacion()
        {
            try
            {
                //clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                //var modal = clsDProyeccionProgramacion.ConsultarProyeccionProgramacion(null);
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
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
                return RedirectToAction("Home", "Home");
            }
        }
        [Authorize]
        public ActionResult ReporteProyeccionProgramacionPartial(DateTime Fecha)
        {
            try
            {
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                var modal = clsDProyeccionProgramacion.ConsultarProyeccionProgramacion(Fecha);
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


    }
}