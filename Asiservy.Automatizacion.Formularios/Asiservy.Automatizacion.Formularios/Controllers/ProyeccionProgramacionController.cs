﻿using Asiservy.Automatizacion.Datos.Datos;
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

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ProyeccionProgramacionController : Controller
    {
        clsDError clsDError = null;
        clsDClasificador clsDClasificador = null;
        clsDProyeccionProgramacion clsDProyeccionProgramacion = null;
        clsDApiProduccion clsDApiProduccion = null;
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

        // GET: ProyeccionProgramacion
        [Authorize]
        public ActionResult ProyeccionProgramacion()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];


                clsDClasificador = new clsDClasificador();
                var ListLimpiezaPescado = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodigoGrupoTipoLimpiezaPescado, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                ViewBag.TipoLimpieza = new SelectList(ListLimpiezaPescado, "codigo", "descripcion");

                var ListDestinoProduccion = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodigoGrupoDestinoProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                ViewBag.Destino = new SelectList(ListDestinoProduccion, "codigo", "descripcion");

                //var ListEspeciePescado = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodigoGrupoEspeciePescado, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                clsDApiProduccion = new clsDApiProduccion();
                ViewBag.Especie = clsDApiProduccion.ConsultarEspecies();
                //ViewBag.Especie = new SelectList(ListEspeciePescado, "codigo", "descripcion");


                //var ListTallaPescado = clsDClasificador.ConsultaClasificador(new Clasificador { Grupo = clsAtributos.CodigoGrupoTallaPescado, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                //ViewBag.Talla = new SelectList(ListTallaPescado, "codigo", "descripcion");

                ViewBag.Talla = clsDApiProduccion.ConsultarTallas(null);

                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
               // ViewBag.Proyeccion = clsDProyeccionProgramacion.ConsultarProyeccionProgramacion(null);
                //ViewBag.Proyeccion = clsDProyeccionProgramacion.ConsultarProyeccionProgramacion(Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString()));
                return View();
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home","Home");
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


        public JsonResult ValidarProyeccionProgramacion(DateTime Fecha)
        {
            try
            {
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                int idProyeccion = clsDProyeccionProgramacion.ValidarProyeccionProgramacion(Fecha);
                return Json(idProyeccion, JsonRequestBehavior.AllowGet);
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
        //public ActionResult ProyeccionProgramacionPartial(int? IdProyeccionProgramacion,string Lote,DateTime? FechaProduccion,int? Toneladas,string Destino, string TipoLimpieza,string Observacion, string Especie, string Talla/*,string Lineas, TimeSpan HoraInicio, TimeSpan HoraFin*/)
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
        //                Lote = Lote,
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
        //        if (!string.IsNullOrEmpty(Lote)){
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