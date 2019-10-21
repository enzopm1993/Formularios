using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models.Asistencia;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class AsistenciaController : Controller
    {

        clsDGeneral clsDGeneral = null;
        clsDEmpleado clsDEmpleado = null;
        clsDAsistencia clsDAsistencia = null;
        clsDCambioPersonal clsDCambioPersonal = null;
        string[] liststring;
        clsDError clsDError = null;
        clsDClasificador clsDClasificador = null;
        clsDCuchillo clsDCuchillo = null;
        clsApiUsuario clsApiUsuario=null;
        clsDSolicitudPermiso ClsDSolicitudPermiso = null;
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
        public JsonResult ConsultarExistenciaAsistenciaGeneral(string Turno)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                clsDAsistencia = new clsDAsistencia();
                int AsitenciaExiste = clsDAsistencia.ConsultarExistenciaAsistenciaGeneral(liststring[1], Turno);
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
        public JsonResult ConsultarExistenciaAsistencia(string Turno)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                clsDAsistencia = new clsDAsistencia();
                int AsitenciaExiste = clsDAsistencia.ConsultarExistenciaAsistencia(liststring[1],Turno);
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
        public JsonResult ConsultarExistenciaAsistenciaPrestados(string Turno)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                clsDAsistencia = new clsDAsistencia();
                int AsitenciaExiste = clsDAsistencia.ConsultarExistenciaAsistenciaPrestados(liststring[1], Turno);
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
                ViewBag.JavaScrip = RouteData.Values["controller"]+"/"+ RouteData.Values["action"];
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
        public ActionResult AsistenciaGeneralPartial(string CodLinea, int BanderaExiste, string turno)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                clsDCambioPersonal = new clsDCambioPersonal();
                clsDClasificador = new clsDClasificador();
                var EstadoAsistencia = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodigoGrupoEstadoAsistencia, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.EstadoAsistencia = EstadoAsistencia;

                clsDAsistencia = new clsDAsistencia();
                var AsistenciaViewModel = clsDAsistencia.ObtenerAsistenciaGeneralDiaria(CodLinea, BanderaExiste, liststring[1], Request.UserHostAddress, turno);
                clsApiUsuario = new clsApiUsuario();
                DateTime? pdUltimaMarcacion;
                foreach (var item in AsistenciaViewModel.ControlAsistencia)
                {
                    pdUltimaMarcacion = clsApiUsuario.ConsultarFechaBiometrico(item.Cedula);
                    //pdUltimaMarcacion =Convert.ToDateTime("2019-09-18 17:05:03.367");

                    if (item.Turno == "1")
                    {
                        if (pdUltimaMarcacion != null)
                        {
                            if ((pdUltimaMarcacion.Value.ToShortDateString() != DateTime.Now.ToShortDateString()))
                            {
                                item.Bloquear = 1;
                                item.Observacion += "No ha marcado en el biométrico";
                            }
                        }
                        else
                        {
                            item.Bloquear = 1;
                            item.Observacion += "No ha marcado en el biométrico";
                        }

                    }
                    if (item.Turno == "2")
                    {
                        if (pdUltimaMarcacion != null)
                        {
                            if (pdUltimaMarcacion.Value.ToShortDateString() != DateTime.Now.ToShortDateString())
                            {
                                item.Bloquear = 1;
                                item.Observacion += "No ha marcado en el biométrico";
                            }
                        }
                        else
                        {
                            item.Bloquear = 1;
                            item.Observacion += "No ha marcado en el biométrico";
                        }
                    }
                    //sp_ConsultaEmpleadosMovidos CambioPersonal = clsDCambioPersonal.ConsultarCambioPersonal(item.Cedula);

                    //if (CambioPersonal != null)
                    //{
                    //    item.Bloquear = 1;
                    //    item.Observacion += "El empleado fue movido a " + CambioPersonal.Linea;
                    //}
                    ClsDSolicitudPermiso = new clsDSolicitudPermiso();
                    string MotivoSolicitud = ClsDSolicitudPermiso.ConsultaMotivoPermisoxEmpleado(item.Cedula);
                    if (!string.IsNullOrEmpty(MotivoSolicitud))
                    {
                        item.Bloquear = 1;
                        item.Observacion += " Tiene permiso: " + MotivoSolicitud;
                    }


                }
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
        public ActionResult AsistenciaPrestadoPartial(string CodLinea, int BanderaExiste, string Turno)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                clsDCambioPersonal = new clsDCambioPersonal();
                clsDClasificador = new clsDClasificador();
                var EstadoAsistencia = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodigoGrupoEstadoAsistencia, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.EstadoAsistencia = EstadoAsistencia;

                clsDAsistencia = new clsDAsistencia();
                var AsistenciaViewModel = clsDAsistencia.ObtenerAsistenciaDiariaMovidos(CodLinea, BanderaExiste, liststring[1], Request.UserHostAddress,Turno);
                clsApiUsuario = new clsApiUsuario();
                DateTime? pdUltimaMarcacion;
                foreach (var item in AsistenciaViewModel.ControlAsistencia)
                {
                    pdUltimaMarcacion = clsApiUsuario.ConsultarFechaBiometrico(item.Cedula);
                    //pdUltimaMarcacion =Convert.ToDateTime("2019-09-18 17:05:03.367");

                    if (item.Turno == "1")
                    {
                        if (pdUltimaMarcacion != null)
                        {
                            if ((pdUltimaMarcacion.Value.ToShortDateString() != DateTime.Now.ToShortDateString()))
                            {
                                item.Bloquear = 1;
                                item.Observacion += "No ha marcado en el biométrico";
                            }
                        }
                        else
                        {
                            item.Bloquear = 1;
                            item.Observacion += "No ha marcado en el biométrico";
                        }

                    }
                    if (item.Turno == "2")
                    {
                        if (pdUltimaMarcacion != null)
                        {
                            if (pdUltimaMarcacion.Value.ToShortDateString() != DateTime.Now.ToShortDateString())
                            {
                                item.Bloquear = 1;
                                item.Observacion += "No ha marcado en el biométrico";
                            }
                        }
                        else
                        {
                            item.Bloquear = 1;
                            item.Observacion += "No ha marcado en el biométrico";
                        }
                    }
                    //sp_ConsultaEmpleadosMovidos CambioPersonal = clsDCambioPersonal.ConsultarCambioPersonal(item.Cedula);

                    //if (CambioPersonal != null)
                    //{
                    //    item.Bloquear = 1;
                    //    item.Observacion += "El empleado fue movido a " + CambioPersonal.Linea;
                    //}
                    ClsDSolicitudPermiso = new clsDSolicitudPermiso();
                    string MotivoSolicitud = ClsDSolicitudPermiso.ConsultaMotivoPermisoxEmpleado(item.Cedula);
                    if (!string.IsNullOrEmpty(MotivoSolicitud))
                    {
                        item.Bloquear = 1;
                        item.Observacion += " Tiene permiso: " + MotivoSolicitud;
                    }


                }
                //Control de Cuchillos
                clsDCuchillo = new clsDCuchillo();
                List<ControlCuchilloViewModel> ControlCuchillos = clsDCuchillo.ConsultaControlCuchillos();
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
        public ActionResult AsistenciaPartial(string CodLinea, int BanderaExiste, string Turno)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                clsDCambioPersonal = new clsDCambioPersonal();
                clsDClasificador = new clsDClasificador();
                var EstadoAsistencia = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodigoGrupoEstadoAsistencia, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.EstadoAsistencia = EstadoAsistencia;

                clsDAsistencia = new clsDAsistencia();
                var AsistenciaViewModel = clsDAsistencia.ObtenerAsistenciaDiaria(CodLinea, BanderaExiste, liststring[0], Request.UserHostAddress, Turno);
                clsApiUsuario = new clsApiUsuario();
                DateTime? pdUltimaMarcacion=null;
                ClsDSolicitudPermiso = new clsDSolicitudPermiso();
                var ListUltimamarcacionUsuarios = clsApiUsuario.ConsultarUltimaMarcacionxFecha(DateTime.Now);
                foreach (var item in AsistenciaViewModel.ControlAsistencia)
                {
                    //pdUltimaMarcacion = clsApiUsuario.ConsultarFechaBiometrico(item.Cedula);

                    pdUltimaMarcacion = ListUltimamarcacionUsuarios.Where(x => x.Cedula == item.Cedula).Select(x => x.UltimaMarcacion).FirstOrDefault();
                    if (item.Turno == "1")
                    {
                        if (pdUltimaMarcacion != null && pdUltimaMarcacion!= DateTime.MinValue)
                        {
                            if ((pdUltimaMarcacion.Value.ToShortDateString() != DateTime.Now.ToShortDateString()))
                            {
                                item.Bloquear = 1;
                                item.Observacion += "No ha marcado en el biométrico";
                            }
                        }
                        else
                        {
                            item.Bloquear = 1;
                            item.Observacion += "No ha marcado en el biométrico";
                        }
                        //if (pdUltimaMarcacion != null)
                        //{
                        //    if ((pdUltimaMarcacion.Value.ToShortDateString() != DateTime.Now.ToShortDateString()))
                        //    {
                        //        item.Bloquear = 1;
                        //        item.Observacion += "No ha marcado en el biométrico";
                        //    }
                        //}
                        //else
                        //{
                        //    item.Bloquear = 1;
                        //    item.Observacion += "No ha marcado en el biométrico";
                        //}

                    }
                    if (item.Turno == "2")
                    {
                        if (pdUltimaMarcacion != null)
                        {
                            if ((pdUltimaMarcacion.Value.ToShortDateString() != DateTime.Now.ToShortDateString()))
                            {
                                item.Bloquear = 1;
                                item.Observacion += "No ha marcado en el biométrico";
                            }
                        }
                        else
                        {
                            item.Bloquear = 1;
                            item.Observacion += "No ha marcado en el biométrico";
                        }
                        //if (pdUltimaMarcacion != null)
                        //{
                        //    if (pdUltimaMarcacion.Value.ToShortDateString() != DateTime.Now.ToShortDateString())
                        //    {
                        //        item.Bloquear = 1;
                        //        item.Observacion += "No ha marcado en el biométrico";
                        //    }
                        //}
                        //else
                        //{
                        //    item.Bloquear = 1;
                        //    item.Observacion += "No ha marcado en el biométrico";
                        //}
                    }

                    //sp_ConsultaEmpleadosMovidos CambioPersonal = clsDCambioPersonal.ConsultarCambioPersonal(item.Cedula);

                    //if (CambioPersonal != null)
                    //{
                    //    item.Bloquear = 1;
                    //    item.Observacion += "El empleado fue movido a " + CambioPersonal.Linea;
                    //}
                    //ClsDSolicitudPermiso = new clsDSolicitudPermiso();
                    //string MotivoSolicitud = ClsDSolicitudPermiso.ConsultaMotivoPermisoxEmpleado(item.Cedula);
                    //if (!string.IsNullOrEmpty(MotivoSolicitud))
                    //{
                    //    item.Bloquear = 1;
                    //    item.Observacion += " Tiene permiso: " + MotivoSolicitud;
                    //}


                }
                //Control de Cuchillos
                clsDCuchillo = new clsDCuchillo();
                List<ControlCuchilloViewModel> modelCuchillo = new List<ControlCuchilloViewModel>();
                modelCuchillo = clsDCuchillo.ConsultarEmpleadosCuchilloPorLinea(CodLinea, clsAtributos.Entrada,DateTime.Now);
                AsistenciaViewModel.ControlDeCuchillos = modelCuchillo;

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
        [HttpPost]
        public JsonResult ConsultarBiometrico(string cedula)
        {
            try
            {

                clsApiUsuario = new clsApiUsuario();
                var ultimaMarcacionBiometrico = clsApiUsuario.ConsultarFechaBiometrico(cedula);
                if (ultimaMarcacionBiometrico != null)
                {
                    if (Convert.ToDateTime(ultimaMarcacionBiometrico.Value.ToShortDateString()) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                

                //return Json(true, JsonRequestBehavior.AllowGet);
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
        public JsonResult GrabarAsistenciaEmpleado(string cedula, string nombre, TimeSpan Hora, string observacion, string estado)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                clsDAsistencia = new clsDAsistencia();
                string Resultado = clsDAsistencia.ActualizarAsistencia(new ASISTENCIA { Cedula = cedula, Hora = Hora, Observacion = observacion, EstadoAsistencia = estado, UsuarioModificacionLog = liststring[0], TerminalModificacionLog = Request.UserHostAddress, FechaModificacionLog = DateTime.Now });
                return Json(Resultado, JsonRequestBehavior.AllowGet);
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
        public JsonResult CambiarAsistenciaEmpleadoFalta(string cedula)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                clsDAsistencia = new clsDAsistencia();
                string Resultado = clsDAsistencia.ActualizarAsistencia(new ASISTENCIA { Cedula = cedula, EstadoAsistencia = clsAtributos.EstadoFalta, UsuarioModificacionLog = liststring[0], TerminalModificacionLog = Request.UserHostAddress, FechaModificacionLog = DateTime.Now });
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
            clsDGeneral = new clsDGeneral();
            ViewBag.Lineas = clsDGeneral.ConsultaLineas("0");
            return View();
        }
        #endregion
    
        #region EDITAR ASISTENCIA
        [Authorize]
        public ActionResult EditarAsistencia()
        {
            try
            {
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                clsDClasificador = new clsDClasificador();
                clsDEmpleado = new clsDEmpleado();
                this.ConsultaComboLineas();
                ViewBag.Estado = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador {Grupo=clsAtributos.CodigoGrupoEstadoAsistencia, EstadoRegistro=clsAtributos.EstadoRegistroActivo});

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
                return RedirectToAction("Home","Home");
            }
            
        }
        [Authorize]
        public ActionResult EditarAsistenciaPartial(string dsLinea, DateTime ddFecha)
        {
            try
            {
                clsDAsistencia = new clsDAsistencia();
                var model = clsDAsistencia.ConsultaControlAsistencia(dsLinea,ddFecha);
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
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
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
                return Json(respuesta,JsonRequestBehavior.AllowGet);
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
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ConsultaComboLineas();
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
               var model= clsDCambioPersonal.ConsultarBitacoraCambioPersonal(new Models.Asistencia.BitacoraCambioPersonalModelView {
                    CodLinea=dsLinea,
                    CodArea=dsArea,
                    CodCargo=dsCargo,
                    Cedula=dsCedula,
                    FechaDesde=ddFechaDesde,
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
        public ActionResult EmpleadosCambioPersonalPartial(string pslinea, string psarea, string pscargo, string tipo)
        {
            try
            {
                List<spConsutaEmpleadosFiltroCambioPersonal> ListaEmpleados = new List<spConsutaEmpleadosFiltroCambioPersonal>();
                clsDEmpleado = new clsDEmpleado();
                if (tipo == "prestar")
                {
                    ListaEmpleados = clsDEmpleado.ConsultaEmpleadosFiltroCambioPersonal(pslinea, psarea, pscargo, clsAtributos.TipoPrestar);

                }
                else
                {
                    ListaEmpleados = clsDEmpleado.ConsultaEmpleadosFiltroCambioPersonal(pslinea, psarea, pscargo, clsAtributos.TipoRegresar);
                    ViewBag.ADondeFuePrestado = clsDEmpleado.ConsultarDondeFueMovido(ListaEmpleados);
                }
                return PartialView(ListaEmpleados);
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

        [Authorize]
        public JsonResult MoverEmpleados(string[] dCedulas, string dlinea, string darea, string tipo)
        {
            try
            {
                List<CAMBIO_PERSONAL> pListCambioPersonal = new List<CAMBIO_PERSONAL>();
                List<BITACORA_CAMBIO_PERSONAL> pListBitacoraCambioPersonal = new List<BITACORA_CAMBIO_PERSONAL>();
                liststring = User.Identity.Name.Split('_');
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
                                CodArea = darea,
                                FechaIngresoLog = DateTime.Now,
                                UsuarioIngresoLog = liststring[0],
                                TerminalIngresoLog = Request.UserHostAddress,
                                EstadoRegistro = "A"
                            });
                            pListBitacoraCambioPersonal.Add(new BITACORA_CAMBIO_PERSONAL
                            {
                                Cedula = pscedulas,
                                Tipo = tipo == "prestar" ? "P" : "R",
                                CodLinea = dlinea,
                                CodArea = darea,
                                FechaIngresoLog = DateTime.Now,
                                UsuarioIngresoLog = liststring[0],
                                TerminalIngresoLog = Request.UserHostAddress,
                            });
                        }
                    }
                    clsDCambioPersonal = new clsDCambioPersonal();
                    psRespuesta = clsDCambioPersonal.GuardarCambioDePersonal(pListCambioPersonal, pListBitacoraCambioPersonal, tipo);
                    return Json(psRespuesta, JsonRequestBehavior.AllowGet);
                }
                return Json("Error, no se ha seleccionado ningún empleado", JsonRequestBehavior.AllowGet);

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

        #region CUCHILLO
        public JsonResult ConsultarCuchilloPorCedula(string cedula)
        {
            try
            {
                clsDCuchillo = new clsDCuchillo();
                var Respuesta = clsDCuchillo.ConsultarCOntrolCuchilloxCedula(cedula);
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public ActionResult GuardarControlCuchillo(string dsCedula, string dsColor,string dsNumero,string dsEstado,bool dbCheck, DateTime ddFecha, bool dbTipo=false)
        {
            try
            {
                if (string.IsNullOrEmpty(dsCedula) || string.IsNullOrEmpty(dsColor) || string.IsNullOrEmpty(dsNumero) || string.IsNullOrEmpty(dsEstado))
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json("Ningun Parametro debe estar vacio", JsonRequestBehavior.AllowGet);
                }
                clsDCuchillo = new clsDCuchillo();
                var poControlCuchillo = new CONTROL_CUCHILLO();
                liststring = User.Identity.Name.Split('_');
                poControlCuchillo.Cedula = dsCedula;
                poControlCuchillo.CuchilloBlanco = dsColor == "B" ? int.Parse(dsNumero) : 0;
                poControlCuchillo.CuchilloRojo = dsColor == "R" ? int.Parse(dsNumero) : 0;
                poControlCuchillo.CuchilloNegro = dsColor == "N" ? int.Parse(dsNumero) : 0;
                poControlCuchillo.Fecha = ddFecha;
                poControlCuchillo.EstadoCuchillo = dsEstado;
                poControlCuchillo.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                poControlCuchillo.FechaIngresoLog = DateTime.Now;
                poControlCuchillo.UsuarioIngresoLog = liststring[0];
                poControlCuchillo.TerminalIngresoLog = Request.UserHostAddress;
                if (dbTipo)
                {
                    poControlCuchillo.Tipo = "P";
                    clsDCuchillo.ActualizarControlCuchiillo(dsCedula, dsColor);
                }

                var respuesta = clsDCuchillo.GuardarModificarControlCuchillo(poControlCuchillo, dbCheck);
                if (respuesta != clsAtributos.MsjRegistroGuardado)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(respuesta, JsonRequestBehavior.AllowGet);

                }
                return Json(respuesta, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
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
        // GET: Asistencia/ControlCuchillo
        public ActionResult ControlCuchillo()
        {
            try
            {
                ViewBag.dataTableJS = "1"; 
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                clsDClasificador = new clsDClasificador();
               // clsDCuchillo = new clsDCuchillo();
                clsDEmpleado = new clsDEmpleado();
                liststring = User.Identity.Name.Split('_');
                var Empleado = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault();
               // List<ControlCuchilloViewModel> model = new List<ControlCuchilloViewModel>();
                
                var EstadosControlCuchillo = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador {
                    Grupo =clsAtributos.CodigoGrupoEstadoControlCuchillo,
                    EstadoRegistro =clsAtributos.EstadoRegistroActivo
                } );

                ViewBag.EstadosControlCuchillo = EstadosControlCuchillo;
                ViewBag.Linea = Empleado!=null?Empleado.LINEA:"";

                return View();
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
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
        // GET: Asistencia/ControlCuchillo
        public ActionResult ControlCuchilloPartial(string dsEstado, DateTime ddFecha)
        {
            try
            {
                clsDClasificador = new clsDClasificador();
                clsDCuchillo = new clsDCuchillo();
                clsDEmpleado = new clsDEmpleado();
                liststring = User.Identity.Name.Split('_');
                var Empleado = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault();
                List<ControlCuchilloViewModel> model = new List<ControlCuchilloViewModel>();
                if (Empleado != null && !string.IsNullOrEmpty(dsEstado))
                {
                    model = clsDCuchillo.ConsultarEmpleadosCuchilloPorLinea(Empleado.CODIGOLINEA, dsEstado, ddFecha);
                }               

                return PartialView(model);
            }
            catch (Exception ex)
            {               
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult Cuchillo()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                clsDClasificador = new clsDClasificador();
                var ColorCuchillos = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador {Grupo=clsAtributos.CodigoGrupoColorCuchillo, EstadoRegistro=clsAtributos.EstadoRegistroActivo });
                ViewBag.ColorCuchillos = ColorCuchillos;
                return View();

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
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
        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult Cuchillo(CUCHILLO model)
        {
            try
            {
                if (model.NumeroCuchillo == 0)
                {
                    ModelState.AddModelError("Numero Cuchillo", "Campo Requerido");
                    clsDClasificador = new clsDClasificador();
                    var ColorCuchillos = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodigoGrupoColorCuchillo, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                    ViewBag.ColorCuchillos = ColorCuchillos;
                    return View(model);
                }
                clsDCuchillo = new clsDCuchillo();
                liststring = User.Identity.Name.Split('_');
                model.EstadoRegistro = model.EstadoRegistro == "true" ? clsAtributos.EstadoRegistroActivo : clsAtributos.EstadoRegistroInactivo;
                model.FechaIngresoLog = DateTime.Now;
                model.UsuarioIngresoLog = liststring[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Respuesta = clsDCuchillo.GuardarModificarCuchillo(model);
                SetSuccessMessage(Respuesta);
                return RedirectToAction("Cuchillo");

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return RedirectToAction("Cuchillo");

            }
        }

        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult CuchilloPartial()
        {
            try
            {
                clsDCuchillo = new clsDCuchillo();
               
                var model = clsDCuchillo.ConsultarCuchillos(new CUCHILLO());
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
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult CuchilloEmpleado()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                ConsultarCombosEmpleadoCuchillo();
                return View();

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
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
        [HttpPost]
        public ActionResult CuchilloEmpleado(EmpleadoCuchilloViewModel model)
        {
            try
            {
                liststring = User.Identity.Name.Split('_');
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                if (!ModelState.IsValid)
                {

                    ConsultarCombosEmpleadoCuchillo();
                    return View(model);
                }
                else
                {
                    clsDCuchillo = new clsDCuchillo();
                    model.EstadoRegistro = model.EstadoRegistro == "true" ? clsAtributos.EstadoRegistroActivo : clsAtributos.EstadoRegistroInactivo;
                    model.FechaIngresoLog = DateTime.Now;
                    model.UsuarioIngresoLog = liststring[0];
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    
                    var respuesta = clsDCuchillo.GuardarModificarEmpleadoCuchillo(model);
                    SetSuccessMessage(respuesta);
                    return RedirectToAction("CuchilloEmpleado");
                }
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[0]
                });
                return RedirectToAction("CuchilloEmpleado");
            }
        }

        public void ConsultarCombosEmpleadoCuchillo()
        {
           
            clsDEmpleado = new clsDEmpleado();
            clsDCuchillo = new clsDCuchillo();
            liststring = User.Identity.Name.Split('_');
            var linea = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault();
            if (linea != null)
            {
                var Empleados = clsDEmpleado.ConsultaEmpleadosFiltro(linea.CODIGOLINEA, null, null);
                ViewBag.Empleados = Empleados;
                ViewBag.Linea = linea.LINEA;
                ViewBag.CodLinea = linea.CODIGOLINEA;
            }
            var poCuchillosBlancos = clsDCuchillo.ConsultarCuchillos(new CUCHILLO { ColorCuchillo = clsAtributos.CodigoColorCuchilloBlanco });
            var poCuchillosRojos = clsDCuchillo.ConsultarCuchillos(new CUCHILLO { ColorCuchillo = clsAtributos.CodigoColorCuchilloRojo });
            var poCuchillosNegros = clsDCuchillo.ConsultarCuchillos(new CUCHILLO { ColorCuchillo = clsAtributos.CodigoColorCuchilloNegro });
            ViewBag.CuchillosBlancos= poCuchillosBlancos;
            ViewBag.CuchillosRojos= poCuchillosRojos;
            ViewBag.CuchillosNegros= poCuchillosNegros;
        }

        [Authorize]
        public ActionResult CuchilloEmpleadoPartial()
        {
            try
            {
                clsDCuchillo = new clsDCuchillo();
                clsDEmpleado = new clsDEmpleado();
                liststring = User.Identity.Name.Split('_');
                var psLinea = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault().CODIGOLINEA;
                var model = clsDCuchillo.ConsultarEmpleadoCuchillo(new Models.Asistencia.EmpleadoCuchilloViewModel(),psLinea);
                return PartialView(model);

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
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

        public JsonResult ConsultaNumeroCuchillo(string dsColor)
        {
            try
            {
                clsDCuchillo = new clsDCuchillo();
                var poCuchillos = clsDCuchillo.ConsultarCuchillos(new CUCHILLO { ColorCuchillo = dsColor });
                return Json(poCuchillos, JsonRequestBehavior.AllowGet);
                
            }
            catch(Exception ex)
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
        [Authorize]
        // GET: Asistencia/ReporteControlCuchillo
        public ActionResult ReporteControlCuchillo()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                liststring = User.Identity.Name.Split('_');
                clsDClasificador = new clsDClasificador();
                clsDEmpleado = new clsDEmpleado();
                clsDLogin = new clsDLogin();
                ViewBag.Lineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador {Grupo =clsAtributos.CodGrupoLineaProduccion, EstadoRegistro=clsAtributos.EstadoRegistroActivo });
                var Empleado = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault();
                ViewBag.LineaEmpleado = Empleado.CODIGOLINEA;
                List<int?> roles = clsDLogin.ConsultaRolesUsuario(liststring[1]);
                if (roles.FirstOrDefault(x => x.Value == clsAtributos.RolSupervisorGeneral || x.Value == clsAtributos.RolControladorGeneral) != null)
                {
                    ViewBag.SupervisorGeneral = clsAtributos.RolSupervisorGeneral;
                }

                return View();

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
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
        // GET: Asistencia/ReporteControlCuchilloPartial
        public ActionResult ReporteControlCuchilloPartial(DateTime Fecha, string Linea)
        {
            try
            {
                clsDCuchillo = new clsDCuchillo();
                var model = clsDCuchillo.ConsultaControlCuchillo(Fecha,Linea);
                return PartialView(model);

            }
            catch (Exception ex)
            {
                //     SetErrorMessage(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
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
        public ActionResult PrestarCuchillo()
        {
            try
            {
               
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                //List<int?> CuchillosBlancos = db.sp_ObtenerCuchillosSobrantes(clsAtributos.CodigoColorCuchilloBlanco).ToList();
                //List<int?> CuchillosRojos = db.sp_ObtenerCuchillosSobrantes(clsAtributos.CodigoColorCuchilloRojo).ToList();
                //List<int?> CuchillosNegros = db.sp_ObtenerCuchillosSobrantes(clsAtributos.CodigoColorCuchilloNegro).ToList();
                //List<ControlDeAsistenciaPrestadosViewModel.Cuchillos> CuchillosBlancosSobrantes = new List<ControlDeAsistenciaPrestadosViewModel.Cuchillos>();
                //List<ControlDeAsistenciaPrestadosViewModel.Cuchillos> CuchillosRojosSobrantes = new List<ControlDeAsistenciaPrestadosViewModel.Cuchillos>();
                //List<ControlDeAsistenciaPrestadosViewModel.Cuchillos> CuchillosNegrosSobrantes = new List<ControlDeAsistenciaPrestadosViewModel.Cuchillos>();
                //foreach (var item in CuchillosBlancos)
                //{
                //    CuchillosBlancosSobrantes.Add(new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Id = item, Numero = item });
                //}
                clsDEmpleado = new clsDEmpleado();
                //clsDGeneral = new clsDGeneral();
                liststring = User.Identity.Name.Split('_');
                
                //ViewBag.Linea = clsDGeneral.ConsultarLineaUsuario(liststring[1]);
                ViewBag.CodLinea = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault().CODIGOLINEA;

                clsDCuchillo = new clsDCuchillo();
                var CuchillosBlancosSobrantes = clsDCuchillo.CuchillosSobrantes(clsAtributos.CodigoColorCuchilloBlanco).Select(x => new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Numero = x, Id = x }).ToList();
                ViewBag.CuchilloBlanco = new SelectList(CuchillosBlancosSobrantes, "Id", "Numero");
                var CuchillosRojosSobrantes = clsDCuchillo.CuchillosSobrantes(clsAtributos.CodigoColorCuchilloRojo).Select(x => new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Numero = x, Id = x }).ToList();
                ViewBag.CuchilloRojo = new SelectList(CuchillosRojosSobrantes, "Id", "Numero");
                var CuchillosNegrosSobrantes = clsDCuchillo.CuchillosSobrantes(clsAtributos.CodigoColorCuchilloNegro).Select(x => new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Numero = x, Id = x }).ToList();
                ViewBag.CuchilloNegro = new SelectList(CuchillosNegrosSobrantes, "Id", "Numero");
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                liststring = User.Identity.Name.Split('_');
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
        //[HttpPost]
        //public ActionResult PrestarCuchillo(CONTROL_CUCHILLO ControlCuchillo)
        //{
        //    try
        //    {
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}
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
