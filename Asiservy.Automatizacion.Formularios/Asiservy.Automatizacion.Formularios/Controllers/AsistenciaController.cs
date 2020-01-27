using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models;
using Asiservy.Automatizacion.Formularios.Models.Asistencia;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class AsistenciaController : Controller
    {
        clsDPeriodo ClsDPeriodo = null;
        clsDGeneral clsDGeneral = null;
        clsDEmpleado clsDEmpleado = null;
        clsDAsistencia clsDAsistencia = null;
        clsDCambioPersonal clsDCambioPersonal = null;
        string[] liststring;
        clsDError clsDError = null;
        clsDClasificador clsDClasificador = null;
        clsDCuchillo clsDCuchillo = null;
        //clsApiUsuario clsApiUsuario=null;
        //clsDSolicitudPermiso ClsDSolicitudPermiso = null;
        clsDLogin clsDLogin = null;
        #region Métodos
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        public void ConsultaComboLineas()
        {
            clsDGeneral = new clsDGeneral();
            ViewBag.Lineas = clsDGeneral.ConsultaLineas("0");
            //ViewBag.Areas = clsDGeneral.ConsultaAreas("0");
            //ViewBag.Cargos = clsDGeneral.ConsultaCargos("0");
        }
        #endregion

        #region Asistencia
        // GET: Asistencia
        [Authorize]
        public ActionResult AsistenciaFinalizarPartial(string CodLinea, DateTime Fecha, string Turno)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                //clsDCambioPersonal = new clsDCambioPersonal();
                //clsDClasificador = new clsDClasificador();
                //var EstadoAsistencia = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodigoGrupoEstadoAsistencia, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                //ViewBag.EstadoAsistencia = EstadoAsistencia;

                //clsDAsistencia = new clsDAsistencia();
                //var AsistenciaViewModel = clsDAsistencia.ObtenerAsistenciaGeneralDiaria(CodLinea, BanderaExiste, liststring[1], Request.UserHostAddress, turno, Fecha);
                clsDAsistencia = new clsDAsistencia();
                List<spConsultaAsistenciaFinalizar> ConultaAsistenciaFinalizar = clsDAsistencia.ConsultarAsistenciaFinalizar(Fecha, CodLinea, Turno);
                ViewBag.NRegistros = ConultaAsistenciaFinalizar.Count;
                return PartialView(ConultaAsistenciaFinalizar);
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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public ActionResult AsistenciaFinalizar()
        {
            try
            {
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                //clsApiUsuario = new clsApiUsuario();
                //var respuestaapi = clsApiUsuario.ConsultarUltimaMarcacionxFecha(DateTime.Now);
                TimeSpan hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
                clsDEmpleado = new clsDEmpleado();
                clsDGeneral = new clsDGeneral();
                liststring = User.Identity.Name.Split('_');

                ViewBag.Linea = clsDGeneral.ConsultarLineaUsuario(liststring[1]);
                ViewBag.CodLinea = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault().CODIGOLINEA;



                return View(/*Asistencia*/);
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
                    UsuarioIngreso = liststring[0]
                });
                return RedirectToAction("Home", "Home");
            }
        }
        public JsonResult VerificarPrestados(string CodLinea, DateTime? Fecha, TimeSpan? Hora)
        {
            try
            {
                clsDAsistencia = new clsDAsistencia();
                var respuesta = clsDAsistencia.ConsultaPrestadosxLinea(CodLinea, Fecha, Hora);
                if (respuesta.Count > 0)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

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
        public JsonResult VerificarMovidosaLinea(string CodLinea, DateTime? Fecha, TimeSpan? Hora)
        {
            try
            {
                clsDAsistencia = new clsDAsistencia();
                var respuesta = clsDAsistencia.ConsultaPersonalMovidoaLinea(CodLinea, Fecha, Hora);
                if (respuesta.Count > 0)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

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
       
        public ActionResult ModalPrestados(DateTime Fecha, TimeSpan Hora)
        {
            try
            {
                ClsDPeriodo = new clsDPeriodo();
                if (!ClsDPeriodo.ValidaFechaPeriodo(Fecha.Add(Hora)))
                {
                    return Json("888", JsonRequestBehavior.AllowGet);
                }
                
                clsDAsistencia = new clsDAsistencia();
                clsDEmpleado = new clsDEmpleado();
                liststring = User.Identity.Name.Split('_');
                string CodLinea = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault().CODIGOLINEA;
                var respuesta = clsDAsistencia.ConsultaPrestadosxLinea(CodLinea, Fecha, Hora);
                ViewBag.ListaEmpleadosPres = respuesta;
                if (respuesta.Count > 0)
                {
                    ViewBag.Prestado = "true";
                }
                else
                {
                    ViewBag.Prestado = "false";
                }
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
        public ActionResult ModalMovidosaMiLinea(DateTime Fecha, TimeSpan Hora)
        {
            try
            {
                //clsDAsistencia = new clsDAsistencia();
                //var respuesta = clsDAsistencia.ConsultaPrestadosxLinea(CodLinea, Fecha, Hora);
                //if (respuesta.Count > 0)
                //{
                //    return Json(true, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    return Json(false, JsonRequestBehavior.AllowGet);
                //}
                //ViewBag.Linea = CodLinea;
                //ViewBag.bandera = bandera;
                clsDAsistencia = new clsDAsistencia();
                clsDEmpleado = new clsDEmpleado();
                liststring = User.Identity.Name.Split('_');
                string CodLinea = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault().CODIGOLINEA;
                var respuesta = clsDAsistencia.ConsultaPersonalMovidoaLineaModal(CodLinea, Fecha, Hora);
                ViewBag.ListaEmpleadosPres = respuesta;
                if (respuesta.Count > 0)
                {
                    ViewBag.Prestado = "true";
                }
                else
                {
                    ViewBag.Prestado = "false";
                }
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
        public ActionResult ModalObservacion()
        {
            try
            {
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
                    UsuarioIngreso = liststring[1]
                });
                return RedirectToAction("Home", "Home");
            }
        }
        public ActionResult ModalHora()
        {
            try
            {
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
                    UsuarioIngreso = liststring[1]
                });
                return RedirectToAction("Home", "Home");
            }
        }
        [Authorize]
        public JsonResult ConsultarExistenciaAsistenciaGeneral(string Turno, DateTime Fecha)
        {
            try
            {
                if (Fecha > DateTime.Now)
                {
                    return Json("333", JsonRequestBehavior.AllowGet);
                }
                liststring = User.Identity.Name.Split('_');
                clsDAsistencia = new clsDAsistencia();
                int AsitenciaExiste = clsDAsistencia.ConsultarExistenciaAsistenciaGeneral(liststring[1], Turno, Fecha);
                return Json(AsitenciaExiste, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }


        }
        [Authorize]
        public JsonResult ConsultarExistenciaAsistencia(string Turno, DateTime Fecha)
        {
            try
            {
                if (Fecha > DateTime.Now)
                {
                    return Json("333",JsonRequestBehavior.AllowGet);
                }
                liststring = User.Identity.Name.Split('_');
                clsDAsistencia = new clsDAsistencia();
                int AsitenciaExiste = clsDAsistencia.ConsultarExistenciaAsistencia(liststring[1], Turno, Fecha);
                return Json(AsitenciaExiste, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize]
        public JsonResult ConsultarExistenciaAsistenciaPrestados(string Turno, DateTime Fecha)
        {
            try
            {
                if (Fecha > DateTime.Now)
                {
                    return Json("333", JsonRequestBehavior.AllowGet);
                }
                liststring = User.Identity.Name.Split('_');
                clsDAsistencia = new clsDAsistencia();
                int AsitenciaExiste = clsDAsistencia.ConsultarExistenciaAsistenciaPrestados(liststring[1], Turno, Fecha);
                return Json(AsitenciaExiste, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize]
        public ActionResult Asistencia()
        {
            try
            {
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                //clsApiUsuario = new clsApiUsuario();
                //var respuestaapi = clsApiUsuario.ConsultarUltimaMarcacionxFecha(DateTime.Now);
                TimeSpan hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
                clsDEmpleado = new clsDEmpleado();
                clsDGeneral = new clsDGeneral();
                liststring = User.Identity.Name.Split('_');
                //clsDAsistencia = new clsDAsistencia();
                //int AsitenciaExiste = clsDAsistencia.ConsultarExistenciaAsistencia(liststring[1]);
                //ViewBag.AsistenciaExiste = AsitenciaExiste;
                ////var Asistencia = clsDAsistencia.ObtenerAsistenciaDiaria(liststring[1]);
                ViewBag.Linea = clsDGeneral.ConsultarLineaUsuario(liststring[1]);
                ViewBag.CodLinea = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault().CODIGOLINEA;
                ////Asistencia.ControlAsistencia.ForEach(a=>a.Hora= hora);

                //**
                //clsDAsistencia = new clsDAsistencia();
                //ViewData["Empleados"] = clsDAsistencia.ConsultaPrestadosxLinea(ViewBag.CodLinea);

                //**
                return View(/*Asistencia*/);
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
                    UsuarioIngreso = liststring[0]
                });
                return RedirectToAction("Home", "Home");
            }

        }
        [Authorize]
        public ActionResult AsistenciaGeneral()
        {
            try
            {
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                TimeSpan hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
                clsDEmpleado = new clsDEmpleado();
                clsDGeneral = new clsDGeneral();
                liststring = User.Identity.Name.Split('_');
                //clsDAsistencia = new clsDAsistencia();
                //int AsitenciaExiste = clsDAsistencia.ConsultarExistenciaAsistencia(liststring[1],"1");
                //ViewBag.AsistenciaExiste = AsitenciaExiste;
                ////var Asistencia = clsDAsistencia.ObtenerAsistenciaDiaria(liststring[1]);
                ViewBag.Linea = clsDGeneral.ConsultarLineaUsuario(liststring[1]);
                ViewBag.CodLinea = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault().CODIGOLINEA;

                //**
                //clsDAsistencia = new clsDAsistencia();
                //ViewData["Empleados"] = clsDAsistencia.ConsultaPrestadosxLinea(ViewBag.CodLinea);

                //**
                return View();
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
                    UsuarioIngreso = liststring[0]
                });
                return RedirectToAction("Home", "Home");
            }

        }
        [HttpPost]
        public ActionResult AsistenciaGeneralPartial(string CodLinea, int BanderaExiste, string turno, DateTime Fecha, TimeSpan HoraServidor)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                clsDCambioPersonal = new clsDCambioPersonal();
                clsDClasificador = new clsDClasificador();
                var EstadoAsistencia = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodigoGrupoEstadoAsistencia, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.EstadoAsistencia = EstadoAsistencia;

                clsDAsistencia = new clsDAsistencia();
                
                //falta modificar el script para enbviar fecha y hora
                var AsistenciaViewModel = clsDAsistencia.ObtenerAsistenciaGeneralDiaria(CodLinea, BanderaExiste, liststring[0], Request.UserHostAddress, turno, Fecha, HoraServidor);
                //**
                ViewBag.HoraControlAsistencia = clsDAsistencia.ConsultarControlAsistencia(Fecha, CodLinea, turno, clsAtributos.General).Hora;
                //**
                return PartialView(AsistenciaViewModel);
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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public ActionResult AsistenciaPrestado()
        {

            try
            {
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                clsDEmpleado = new clsDEmpleado();
                clsDGeneral = new clsDGeneral();
                liststring = User.Identity.Name.Split('_');
                //clsDAsistencia = new clsDAsistencia();
                //int AsitenciaExiste = clsDAsistencia.ConsultarExistenciaAsistenciaPrestados(liststring[1]);
                //ViewBag.AsistenciaExiste = AsitenciaExiste;
                ViewBag.Linea = clsDGeneral.ConsultarLineaUsuario(liststring[1]);
                ViewBag.CodLinea = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault().CODIGOLINEA;
                ///Asistencia.ControlAsistencia.ForEach(a=>a.Hora= hora);
                return View();
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
                    UsuarioIngreso = liststring[1]
                });
                return RedirectToAction("Home", "Home");
            }
        }
        [Authorize]
        public ActionResult AsistenciaPrestadoPartial(string CodLinea, int BanderaExiste, string Turno, DateTime Fecha, TimeSpan Hora)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                clsDCambioPersonal = new clsDCambioPersonal();
                clsDClasificador = new clsDClasificador();
                var EstadoAsistencia = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodigoGrupoEstadoAsistencia, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.EstadoAsistencia = EstadoAsistencia;

                clsDAsistencia = new clsDAsistencia();
                ControlDeAsistenciaPrestadosViewModel AsistenciaViewModel = clsDAsistencia.ObtenerAsistenciaDiariaMovidos(CodLinea, BanderaExiste, liststring[0], Request.UserHostAddress, Turno, Fecha, Hora);

                //**
                ViewBag.HoraControlAsistencia = clsDAsistencia.ConsultarControlAsistencia(Fecha, CodLinea, Turno,clsAtributos.Prestado).Hora;
                //**
                //Control de Cuchillos
                clsDCuchillo = new clsDCuchillo();
                List<ControlCuchilloViewModel> ControlCuchillos = clsDCuchillo.ConsultaControlCuchillos(Fecha);
                //List<ControlCuchilloViewModel> modelCuchillo = new List<ControlCuchilloViewModel>();
                //modelCuchillo = clsDCuchillo.ConsultarEmpleadosCuchilloPorLinea(CodLinea, clsAtributos.Entrada);
                AsistenciaViewModel.ControlDeCuchillos = ControlCuchillos;

                //**
                return PartialView(AsistenciaViewModel);
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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public ActionResult AsistenciaPartial(string CodLinea, int BanderaExiste, string Turno, DateTime Fecha, TimeSpan? HoraServidor)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                clsDCambioPersonal = new clsDCambioPersonal();
                clsDClasificador = new clsDClasificador();
                var EstadoAsistencia = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodigoGrupoEstadoAsistencia, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.EstadoAsistencia = EstadoAsistencia;

                clsDAsistencia = new clsDAsistencia();
                ControlDeAsistenciaViewModel AsistenciaViewModel = clsDAsistencia.ObtenerAsistenciaDiaria(CodLinea, BanderaExiste, liststring[0], Request.UserHostAddress, Turno, Fecha, HoraServidor.Value);

                //**
                ViewBag.HoraControlAsistencia = clsDAsistencia.ConsultarControlAsistencia(Fecha, CodLinea, Turno,clsAtributos.Procesos).Hora;
                //**
                //Control de Cuchillos
                clsDCuchillo = new clsDCuchillo();
                List<ControlCuchilloViewModel> modelCuchillo = new List<ControlCuchilloViewModel>();
                modelCuchillo = clsDCuchillo.ConsultarEmpleadosCuchilloPorLinea(CodLinea, clsAtributos.Entrada, Fecha, false);
                AsistenciaViewModel.ControlDeCuchillos = modelCuchillo;

                //**
                return PartialView(AsistenciaViewModel);
            }
            catch (Exception ex)
            {
                //SetErrorMessage(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GuardarSalidaAsistencia(string Cedula, DateTime Fecha, TimeSpan? Hora, string Tipo, int IdMovimiento, string Turno, string CodLinea)
        {
            try
            {
                clsDAsistencia = new clsDAsistencia();
                var resultado = clsDAsistencia.GuardarAsistenciaSalida(Cedula, Fecha, Hora.Value, Tipo, IdMovimiento, Turno, CodLinea);
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ConsultarBiometrico(string cedula)
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                var resultado = clsDGeneral.ConsultarSiMarcoBiometrico(cedula);
                return Json(resultado, JsonRequestBehavior.AllowGet);

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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GrabarAsistenciaEmpleado(string cedula, string nombre, TimeSpan Hora, string observacion, string estado, DateTime Fecha, string CentroCostos, string Recurso, string Linea, string Cargo, string Turno)
        {
            try
            {
                ClsDPeriodo = new clsDPeriodo();
                if (!ClsDPeriodo.ValidaFechaPeriodo(Fecha))
                {
                    return Json("888", JsonRequestBehavior.AllowGet);
                }
                liststring = User.Identity.Name.Split('_');
                clsDAsistencia = new clsDAsistencia();
                string Resultado = clsDAsistencia.ActualizarAsistencia(new ASISTENCIA
                {
                    Cedula = cedula,
                    Hora = Hora,
                    Observacion = observacion,
                    EstadoAsistencia = estado,
                    UsuarioModificacionLog = liststring[0],
                    TerminalModificacionLog = Request.UserHostAddress,
                    FechaModificacionLog = DateTime.Now,
                    Fecha = Fecha,
                    CentroCostos = CentroCostos,
                    Recurso = Recurso,
                    Linea = Linea,
                    Cargo = Cargo,
                    Turno = Turno,
                    EstadoRegistro = clsAtributos.EstadoRegistroActivo
                });
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                liststring = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult ReiniciarHoraSalida(string cedula, DateTime Fecha)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                clsDAsistencia = new clsDAsistencia();
                string Resultado = clsDAsistencia.ActualizarAsistencia(new ASISTENCIA { Cedula = cedula, UsuarioModificacionLog = liststring[0], TerminalModificacionLog = Request.UserHostAddress, FechaModificacionLog = DateTime.Now, Fecha = Fecha });
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                liststring = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult CambiarAsistenciaEmpleadoFalta(string cedula, DateTime Fecha)
        {
            try
            {
                ClsDPeriodo = new clsDPeriodo();
                if (!ClsDPeriodo.ValidaFechaPeriodo(Fecha))
                {
                    return Json("888", JsonRequestBehavior.AllowGet);
                }
                liststring = User.Identity.Name.Split('_');
                clsDAsistencia = new clsDAsistencia();
                string Resultado = clsDAsistencia.ActualizarAsistencia(new ASISTENCIA { Cedula = cedula, EstadoAsistencia = clsAtributos.EstadoFalta, UsuarioModificacionLog = liststring[0], TerminalModificacionLog = Request.UserHostAddress, FechaModificacionLog = DateTime.Now, Fecha = Fecha });
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                liststring = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult RptAsistencia()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                clsDGeneral = new clsDGeneral();
                ViewBag.Lineas = new SelectList(clsDGeneral.ConsultaLineas("0"), "codigo", "descripcion");

                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                //Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                liststring = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return RedirectToAction("Home", "Home");
            }
        }
        public ActionResult RptAsistenciaPartial(DateTime FechaInicio, DateTime FechaFin, string Linea, string Turno)
        {
            try
            {

                ViewBag.FechaInicio = FechaInicio;
                ViewBag.FechaFinal = FechaFin;
                clsDAsistencia = new clsDAsistencia();
                var resultado = clsDAsistencia.ConsultarRptAsistencia(FechaInicio, FechaFin, Linea, Turno);
                List<EmpleadoRpt> Empleado = new List<EmpleadoRpt>();

                foreach (var item in resultado)
                {
                    Empleado.Add(new EmpleadoRpt { Cedula = item.Cedula, Nombre = item.NOMBRES });
                }
                ViewBag.Empleados = Empleado.Distinct().OrderBy(z => z.Nombre);
                return PartialView(resultado);
            }
            catch (Exception ex)
            {

                // SetErrorMessage(ex.Message);
                liststring = User.Identity.Name.Split('_');
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region EDITAR ASISTENCIA
        [Authorize]
        public ActionResult EditarAsistencia()
        {
            try
            {
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                clsDClasificador = new clsDClasificador();
                clsDEmpleado = new clsDEmpleado();
                this.ConsultaComboLineas();
                ViewBag.Estado = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodigoGrupoEstadoAsistencia, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                return View();
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                //Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                liststring = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return RedirectToAction("Home", "Home");
            }

        }
        [Authorize]
        public ActionResult EditarAsistenciaPartial(string dsLinea, DateTime ddFecha)
        {
            try
            {
                clsDAsistencia = new clsDAsistencia();
                var model = clsDAsistencia.ConsultaControlAsistencia(dsLinea, ddFecha);
                if (!model.Any())
                    return Json("0", JsonRequestBehavior.AllowGet);

                return PartialView(model);
            }
            catch (Exception ex)
            {
                // SetErrorMessage(ex.Message);
                liststring = User.Identity.Name.Split('_');
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        [HttpPost]
        public ActionResult ModificarAsistencia(ASISTENCIA model)
        {
            try
            {
                if (model != null && model.IdAsistencia == 0) return Json("No se puedo actualizar el registro", JsonRequestBehavior.AllowGet);
                liststring = User.Identity.Name.Split('_');
                clsDAsistencia = new clsDAsistencia();
                model.UsuarioCreacionLog = liststring[0];
                model.TerminalCreacionLog = Request.UserHostAddress;
                var respuesta = clsDAsistencia.ModificarAsistencia(model);
                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // SetErrorMessage(ex.Message);
                liststring = User.Identity.Name.Split('_');
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }



        #endregion

        #region Cambio_PersonaldeÁrea
        
        [Authorize]
        public ActionResult CambiarPersonalDeArea()
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.Select2 = "1";
                ViewBag.CentroCostos = clsDGeneral.ConsultaCentroCostos();
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
                    UsuarioIngreso = liststring[0]
                });
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        [Authorize]
        public ActionResult BitacoraCambioPersonal()
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                ViewBag.Lineas = clsDGeneral.ConsultaLineas("0");
                return View();

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
                    UsuarioIngreso = liststring[0]
                });
                return RedirectToAction("Home", "Home");
            }
        }

        [Authorize]
        public ActionResult BitacoraCambioPersonalPartial(string dsLinea, string dsArea, string dsCargo, string dsCedula, DateTime ddFechaDesde, DateTime ddFechaHasta)
        {
            try
            {
                clsDCambioPersonal = new clsDCambioPersonal();
                var model = clsDCambioPersonal.ConsultarBitacoraCambioPersonal(new Models.Asistencia.BitacoraCambioPersonalModelView
                {
                    CodLinea = dsLinea,
                    CodArea = dsArea,
                    CodCargo = dsCargo,
                    Cedula = dsCedula,
                    FechaDesde = ddFechaDesde,
                    FechaHasta = ddFechaHasta
                });
                return PartialView(model);

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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult ReporteCambioPersonal()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                clsDGeneral = new clsDGeneral();
                var Lineas = clsDGeneral.ConsultaLineas("0");
                ViewBag.Lineas = new SelectList(Lineas, "codigo", "descripcion");
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                liststring = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return RedirectToAction("Home", "Home");
            }
        }
        public ActionResult ReporteCambioPersonalPartial(string CodLinea, DateTime FechaInicio, DateTime FechaFin)
        {
            try
            {
                ViewBag.dataTableJS = "1";

                clsDCambioPersonal = new clsDCambioPersonal();
                List<spReporteCambioPersonal> Resultado = clsDCambioPersonal.ReporteCambioPersonal(CodLinea, FechaInicio, FechaFin);
                return PartialView(Resultado);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                liststring = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
       
        public ActionResult EmpleadosCambioPersonalPartial(string psCentroCosto, string psRecurso, string psLinea, string psCargo, string tipo)
        {
            try
            {
                List<spConsutaEmpleadosFiltroCambioPersonal> ListaEmpleados = new List<spConsutaEmpleadosFiltroCambioPersonal>();
                clsDEmpleado = new clsDEmpleado();
                if (tipo == "prestar")
                {
                    ListaEmpleados = clsDEmpleado.ConsultaEmpleadosFiltroCambioPersonal(psLinea, psCentroCosto, psCargo, psRecurso, clsAtributos.TipoPrestar);
                    TempData["ListaEmpleados"] = ListaEmpleados;
                }
                else
                {
                    ListaEmpleados = clsDEmpleado.ConsultaEmpleadosFiltroCambioPersonal(psLinea, psCentroCosto, psCargo, psRecurso, clsAtributos.TipoRegresar);
                    TempData["ListaEmpleados"] = ListaEmpleados;
                    ViewBag.ADondeFuePrestado = clsDEmpleado.ConsultarDondeFueMovido(ListaEmpleados);
                }
                return PartialView(ListaEmpleados);
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                liststring = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult InactivarEmpleadoCambioPersonal(string[] dCedulas)
        {
            try
            {
                if (dCedulas.ToList().Contains("horaswitch"))
                {

                    List<string> array = dCedulas.ToList();
                    array.Remove("horaswitch");
                    dCedulas = array.ToArray();

                }

                List<CAMBIO_PERSONAL> pListCambioPersonal = new List<CAMBIO_PERSONAL>();
                //List<BITACORA_CAMBIO_PERSONAL> pListBitacoraCambioPersonal = new List<BITACORA_CAMBIO_PERSONAL>();
                liststring = User.Identity.Name.Split('_');
                string psRespuesta = string.Empty;
                clsDAsistencia = new clsDAsistencia();
                if (dCedulas != null && dCedulas.Length > 0)
                {
                    psRespuesta = clsDAsistencia.InactivarEmpleadosCambioPersonal(dCedulas);
                    //foreach (var pscedulas in dCedulas)
                    //{
                    //    if (!string.IsNullOrEmpty(pscedulas))
                    //    {

                    //pListCambioPersonal.Add(new CAMBIO_PERSONAL
                    //{
                    //    Cedula = pscedulas,
                    //    CodLinea = dlinea,
                    //    CentroCosto = darea,
                    //    Recurso = drecurso,
                    //    CodCargo = dcargo,
                    //    Fecha = dfecha,
                    //    HoraInicio = dhora,
                    //    Vigente = true,
                    //    FechaIngresoLog = DateTime.Now,
                    //    UsuarioIngresoLog = liststring[0],
                    //    TerminalIngresoLog = Request.UserHostAddress,
                    //    EstadoRegistro = "A"
                    //});
                    //pListBitacoraCambioPersonal.Add(new BITACORA_CAMBIO_PERSONAL
                    //{
                    //    Cedula = pscedulas,
                    //    Tipo = tipo == "prestar" ? "P" : "R",
                    //    CodLinea = dlinea,
                    //    CodArea = darea,
                    //    FechaIngresoLog = DateTime.Now,
                    //    UsuarioIngresoLog = liststring[0],
                    //    TerminalIngresoLog = Request.UserHostAddress,
                    //});
                    //    }
                    //}
                    //clsDCambioPersonal = new clsDCambioPersonal();
                    //ClsDPeriodo = new clsDPeriodo();
                    //if (ClsDPeriodo.ValidaFechaPeriodo(dfecha))
                    //{
                    //    psRespuesta = clsDCambioPersonal.GuardarCambioDePersonal(pListCambioPersonal/*, pListBitacoraCambioPersonal*/, tipo);
                    //}
                    //else
                    //{
                    //    psRespuesta = "No se pudo completar la acción, por que el periodo se encuentra cerrado";
                    //}
                    return Json(psRespuesta, JsonRequestBehavior.AllowGet);
                }
                return Json("Error, no se ha seleccionado ningún empleado", JsonRequestBehavior.AllowGet);

            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult MoverEmpleados(string[] dCedulas, string dlinea, string darea, string drecurso, string dcargo, DateTime dfecha, TimeSpan? dhora, string tipo)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(liststring[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                List<spConsutaEmpleadosFiltroCambioPersonal> ListEmpleados = TempData["ListaEmpleados"] as List<spConsutaEmpleadosFiltroCambioPersonal>;
                if (dCedulas.ToList().Contains("horaswitch"))
                {

                    List<string> array = dCedulas.ToList();
                    array.Remove("horaswitch");
                    dCedulas = array.ToArray();

                }

                List<CAMBIO_PERSONAL> pListCambioPersonal = new List<CAMBIO_PERSONAL>();
                //List<BITACORA_CAMBIO_PERSONAL> pListBitacoraCambioPersonal = new List<BITACORA_CAMBIO_PERSONAL>();
                
                string psRespuesta = string.Empty;
                if (dCedulas != null && dCedulas.Length > 0)
                {
                    foreach (var pscedulas in dCedulas)
                    {
                        if (!string.IsNullOrEmpty(pscedulas))
                        {
                            pListCambioPersonal.Add(new CAMBIO_PERSONAL
                            {
                                Cedula = pscedulas,
                                CodLinea = dlinea,
                                CentroCosto = darea,
                                Recurso = drecurso,
                                CodCargo = dcargo,
                                Fecha = dfecha,
                                HoraInicio = dhora,
                                Vigente = true,
                                FechaIngresoLog = DateTime.Now,
                                UsuarioIngresoLog = liststring[0],
                                TerminalIngresoLog = Request.UserHostAddress,
                                EstadoRegistro = "A",
                                CentroCostoOrigen=ListEmpleados.Where(x=>x.CEDULA==pscedulas).FirstOrDefault().CODIGOAREA,
                                CodLineaOrigen=ListEmpleados.Where(x => x.CEDULA == pscedulas).FirstOrDefault().CODIGOLINEA,
                                RecursoOrigen= ListEmpleados.Where(x => x.CEDULA == pscedulas).FirstOrDefault().RECURSO,
                                CodCargoOrigen= ListEmpleados.Where(x => x.CEDULA == pscedulas).FirstOrDefault().CODIGOCARGO
                            });
                            //pListBitacoraCambioPersonal.Add(new BITACORA_CAMBIO_PERSONAL
                            //{
                            //    Cedula = pscedulas,
                            //    Tipo = tipo == "prestar" ? "P" : "R",
                            //    CodLinea = dlinea,
                            //    CodArea = darea,
                            //    FechaIngresoLog = DateTime.Now,
                            //    UsuarioIngresoLog = liststring[0],
                            //    TerminalIngresoLog = Request.UserHostAddress,
                            //});
                        }
                    }
                    clsDCambioPersonal = new clsDCambioPersonal();
                    ClsDPeriodo = new clsDPeriodo();
                    if (ClsDPeriodo.ValidaFechaPeriodo(dfecha))
                    {
                        psRespuesta = clsDCambioPersonal.GuardarCambioDePersonal(pListCambioPersonal/*, pListBitacoraCambioPersonal*/, tipo);
                    }
                    else
                    {
                        psRespuesta = "No se pudo completar la acción, por que el periodo se encuentra cerrado";
                    }
                    return Json(psRespuesta, JsonRequestBehavior.AllowGet);
                }
                return Json("Error, no se ha seleccionado ningún empleado", JsonRequestBehavior.AllowGet);

            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(liststring[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion



        [Authorize]
        // GET: Asistencia/RptCuchillo
        public ActionResult RptCuchillo()
        {
            return View();
        }

        [Authorize]
        // GET: Asistencia/ReporteDistribucion
        public ActionResult ReporteDistribucion()
        {
            return View();
        }
        [Authorize]
        // GET: Asistencia/PersonalNomina
        public ActionResult PersonalNomina()
        {
            return View();
        }


        #region METODOS GENERICOS
        public JsonResult ConsultaListadoCargosxRecursoyLinea(string CodRecurso, string CodLinea)
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                var recursos = clsDGeneral.ConsultaCargosxRecursoyLinea(CodRecurso, CodLinea);
                return Json(recursos, JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ConsultaListadoLineas(string CodCentroCostos, string CodRecurso)
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                var recursos = clsDGeneral.ConsultaLineasxCCyRecurso(CodCentroCostos, CodRecurso);
                return Json(recursos, JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ConsultaListadoRecursos(string CodCentroCostos)
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                var recursos = clsDGeneral.ConsultaRecursos(CodCentroCostos);
                return Json(recursos, JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ConsultaListadoAreas(string CodLinea)
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                var areas = clsDGeneral.ConsultaAreas(CodLinea);
                return Json(areas, JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ConsultaListadoCargos(string CodArea)
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                var areas = clsDGeneral.ConsultaCargos(CodArea);
                return Json(areas, JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
