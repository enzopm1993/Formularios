using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia;
using Asiservy.Automatizacion.Formularios.Models;
using Asiservy.Automatizacion.Formularios.Models.Asistencia;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ControlCuchilloController : Controller
    {
        clsDCuchillo clsDCuchillo = null;
        clsDError clsDError = null;
        clsDClasificador clsDClasificador = null;
        clsDEmpleado clsDEmpleado = null;
        clsDGeneral clsDGeneral = null;
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
        public void ConsultaComboLineas()
        {
            clsDGeneral = new clsDGeneral();
            ViewBag.Lineas = clsDGeneral.ConsultaLineas("0");
            //ViewBag.Areas = clsDGeneral.ConsultaAreas("0");
            //ViewBag.Cargos = clsDGeneral.ConsultaCargos("0");
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

                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public ActionResult GuardarControlCuchillo(string dsCedula, string dsColor, string dsNumero, string dsEstado, bool dbCheck, DateTime ddFecha, string Observacion, bool dbTipo = false)
        {
            try
            {
                if (string.IsNullOrEmpty(dsCedula) || string.IsNullOrEmpty(dsColor) || string.IsNullOrEmpty(dsNumero) || string.IsNullOrEmpty(dsEstado))
                {
                    ClasificadorGenerico ClaRespuesta = new ClasificadorGenerico { codigo = 1, descripcion = "Ningun Parametro debe estar vacio" };

                    return Json(ClaRespuesta, JsonRequestBehavior.AllowGet);
                }
                clsDCuchillo = new clsDCuchillo();
                var poControlCuchillo = new CONTROL_CUCHILLO();
                lsUsuario = User.Identity.Name.Split('_');
                poControlCuchillo.Cedula = dsCedula;
                poControlCuchillo.CuchilloBlanco = dsColor == "B" ? int.Parse(dsNumero) : 0;
                poControlCuchillo.CuchilloRojo = dsColor == "R" ? int.Parse(dsNumero) : 0;
                poControlCuchillo.CuchilloNegro = dsColor == "N" ? int.Parse(dsNumero) : 0;
                poControlCuchillo.Fecha = ddFecha;
                poControlCuchillo.Observacion = Observacion;
                poControlCuchillo.EstadoCuchillo = dsEstado;
                poControlCuchillo.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                poControlCuchillo.FechaIngresoLog = DateTime.Now;
                poControlCuchillo.UsuarioIngresoLog = lsUsuario[0];
                poControlCuchillo.TerminalIngresoLog = Request.UserHostAddress;
                if (dbTipo)
                {
                    poControlCuchillo.Tipo = "P";
                    clsDCuchillo.ActualizarControlCuchiillo(dsCedula, dsColor);
                }

                var respuesta = clsDCuchillo.GuardarModificarControlCuchillo(poControlCuchillo, dbCheck);
                if (respuesta != clsAtributos.MsjRegistroGuardado)
                {
                    ClasificadorGenerico ClaRespuesta = new ClasificadorGenerico { codigo = 1, descripcion = respuesta };
                    return Json(ClaRespuesta, JsonRequestBehavior.AllowGet);

                }
                return Json(respuesta, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
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
                lsUsuario = User.Identity.Name.Split('_');
                var Empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                // List<ControlCuchilloViewModel> model = new List<ControlCuchilloViewModel>();

                var EstadosControlCuchillo = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador
                {
                    Grupo = clsAtributos.CodigoGrupoEstadoControlCuchillo,
                    EstadoRegistro = clsAtributos.EstadoRegistroActivo
                });

                ViewBag.EstadosControlCuchillo = EstadosControlCuchillo;
                ViewBag.Linea = Empleado != null ? Empleado.LINEA : "";

                return View();
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
                });
                return RedirectToAction("Home", "Home");
            }


        }
        [Authorize]
        // GET: Asistencia/ControlCuchillo
        public ActionResult ControlCuchilloPartial(string dsEstado, DateTime ddFecha, string Turno)
        {
            try
            {
                clsDClasificador = new clsDClasificador();
                clsDCuchillo = new clsDCuchillo();
                clsDEmpleado = new clsDEmpleado();
                lsUsuario = User.Identity.Name.Split('_');
                var Empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                List<ControlCuchilloViewModel> model = new List<ControlCuchilloViewModel>();
                if (Empleado != null && !string.IsNullOrEmpty(dsEstado))
                {
                    model = clsDCuchillo.ConsultarEmpleadosCuchilloPorLinea(Empleado.CODIGOLINEA, dsEstado, ddFecha, true, Turno);
                    ViewBag.ListadoCuchillosPrestado = clsDCuchillo.ConsultaControlCuchilloPrestado(ddFecha);


                }
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                return PartialView(model);
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
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
                var ColorCuchillos = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodigoGrupoColorCuchillo, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.ColorCuchillos = ColorCuchillos;
                return View();

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
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
                    return Json("1", JsonRequestBehavior.AllowGet);

                }
                clsDCuchillo = new clsDCuchillo();
                lsUsuario = User.Identity.Name.Split('_');
                model.EstadoRegistro = model.EstadoRegistro == "true" ? clsAtributos.EstadoRegistroActivo : clsAtributos.EstadoRegistroInactivo;
                model.FechaIngresoLog = DateTime.Now;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Respuesta = clsDCuchillo.GuardarModificarCuchillo(model);
                //  SetSuccessMessage(Respuesta);
                // return RedirectToAction("Cuchillo");
                return Json(Respuesta, JsonRequestBehavior.AllowGet);

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

       
        // GET: Asistencia/Cuchillo
        public ActionResult CuchilloPartial(string dsColor)
        {
            try
            {
                clsDCuchillo = new clsDCuchillo();

                var model = clsDCuchillo.ConsultarCuchillos(new CUCHILLO { ColorCuchillo = dsColor });
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

      
        // GET: Asistencia/Cuchillo
        public ActionResult CuchilloEmpleado()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                ConsultarCombosEmpleadoCuchillo();
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

       
        [HttpPost]
        public ActionResult CuchilloEmpleado(EmpleadoCuchilloViewModel model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (!ModelState.IsValid)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    clsDCuchillo = new clsDCuchillo();
                    model.EstadoRegistro = model.EstadoRegistro == "true" ? clsAtributos.EstadoRegistroActivo : clsAtributos.EstadoRegistroInactivo;
                    model.FechaIngresoLog = DateTime.Now;
                    model.UsuarioIngresoLog = lsUsuario[0];
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    RespuestaGeneral respuesta = new RespuestaGeneral();
                    respuesta = clsDCuchillo.GuardarModificarEmpleadoCuchillo(model);
                    return Json(respuesta, JsonRequestBehavior.AllowGet);
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

        public void ConsultarCombosEmpleadoCuchillo()
        {

            clsDEmpleado = new clsDEmpleado();
            clsDCuchillo = new clsDCuchillo();
            lsUsuario = User.Identity.Name.Split('_');
            var linea = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
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
            ViewBag.CuchillosBlancos = poCuchillosBlancos;
            ViewBag.CuchillosRojos = poCuchillosRojos;
            ViewBag.CuchillosNegros = poCuchillosNegros;
        }


        public ActionResult CuchilloEmpleadoPartial()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDCuchillo = new clsDCuchillo();
                clsDEmpleado = new clsDEmpleado();
                lsUsuario = User.Identity.Name.Split('_');
                var psLinea = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault().CODIGOLINEA;
                var model = clsDCuchillo.ConsultarEmpleadoCuchillo(new Models.Asistencia.EmpleadoCuchilloViewModel(), psLinea);
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

        public JsonResult ConsultaNumeroCuchillo(string dsColor)
        {
            try
            {
                clsDCuchillo = new clsDCuchillo();
                var poCuchillos = clsDCuchillo.ConsultarCuchillos(new CUCHILLO { ColorCuchillo = dsColor });
                return Json(poCuchillos, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                lsUsuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
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

                lsUsuario = User.Identity.Name.Split('_');
                clsDClasificador = new clsDClasificador();
                clsDEmpleado = new clsDEmpleado();
                clsDLogin = new clsDLogin();
                ViewBag.Lineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                var Empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                ViewBag.LineaEmpleado = Empleado.CODIGOLINEA;
                List<int?> roles = clsDLogin.ConsultaRolesUsuario(lsUsuario[1]);
                if (roles.FirstOrDefault(x => x.Value == clsAtributos.RolSupervisorGeneral || x.Value == clsAtributos.AsistenteProduccion || x.Value == clsAtributos.RolControladorGeneral) != null)
                {
                    ViewBag.SupervisorGeneral = clsAtributos.RolSupervisorGeneral;
                }

                return View();

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
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
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDCuchillo = new clsDCuchillo();
                var model = clsDCuchillo.ConsultaControlCuchillo(Fecha, Linea);
                return PartialView(model);

            }
            catch (Exception ex)
            {
                //     SetErrorMessage(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult PrestarCuchillo(DateTime Fecha)
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
                lsUsuario = User.Identity.Name.Split('_');

                //ViewBag.Linea = clsDGeneral.ConsultarLineaUsuario(lsUsuario[1]);
                ViewBag.CodLinea = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault().CODIGOLINEA;

                clsDCuchillo = new clsDCuchillo();
                var CuchillosBlancosSobrantes = clsDCuchillo.CuchillosSobrantes(clsAtributos.CodigoColorCuchilloBlanco, Fecha).Select(x => new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Numero = x, Id = x }).ToList();
                ViewBag.CuchilloBlanco = new SelectList(CuchillosBlancosSobrantes, "Id", "Numero");
                var CuchillosRojosSobrantes = clsDCuchillo.CuchillosSobrantes(clsAtributos.CodigoColorCuchilloRojo, Fecha).Select(x => new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Numero = x, Id = x }).ToList();
                ViewBag.CuchilloRojo = new SelectList(CuchillosRojosSobrantes, "Id", "Numero");
                var CuchillosNegrosSobrantes = clsDCuchillo.CuchillosSobrantes(clsAtributos.CodigoColorCuchilloNegro, Fecha).Select(x => new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Numero = x, Id = x }).ToList();
                ViewBag.CuchilloNegro = new SelectList(CuchillosNegrosSobrantes, "Id", "Numero");
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = lsUsuario[0]
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

        #region CUCHILLO EMPLEADO PRESTADO
        [Authorize]
        // GET: Asistencia/ControlCuchillo
        public ActionResult EmpleadoCuchilloPrestado()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDCuchillo = new clsDCuchillo();
                var CuchillosBlancosSobrantes = clsDCuchillo.CuchillosSobrantes(clsAtributos.CodigoColorCuchilloBlanco, DateTime.Now).Select(x => new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Numero = x, Id = x, Color = 'B' }).ToList();
                ViewBag.CuchilloBlanco = new SelectList(CuchillosBlancosSobrantes, "Id", "Numero");
                var CuchillosRojosSobrantes = clsDCuchillo.CuchillosSobrantes(clsAtributos.CodigoColorCuchilloRojo, DateTime.Now).Select(x => new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Numero = x, Id = x, Color = 'R' }).ToList();
                ViewBag.CuchilloRojo = new SelectList(CuchillosRojosSobrantes, "Id", "Numero");
                var CuchillosNegrosSobrantes = clsDCuchillo.CuchillosSobrantes(clsAtributos.CodigoColorCuchilloNegro, DateTime.Now).Select(x => new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Numero = x, Id = x, Color = 'N' }).ToList();
                ViewBag.CuchilloNegro = new SelectList(CuchillosNegrosSobrantes, "Id", "Numero");
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

        public ActionResult EmpleadoCuchilloPrestadoPartial(DateTime Fecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDCuchillo = new clsDCuchillo();
                clsDEmpleado = new clsDEmpleado();
                var empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                var model = clsDCuchillo.ConsultaCuchillosEmpleadoPrestadoPorLineaFecha(empleado.CODIGOLINEA, Fecha);
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
        public ActionResult EmpleadoCuchilloPrestado(EMPLEADO_CUCHILLO_PRESTADO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDCuchillo = new clsDCuchillo();
                clsDEmpleado = new clsDEmpleado();
                var empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                var modelEmpleado = clsDCuchillo.ConsultaEmpleadoPrestadoPorLineaFecha(empleado.CODIGOLINEA, model.Fecha).FirstOrDefault(x => x.CEDULA == model.Cedula);
                if (modelEmpleado == null)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.Linea = modelEmpleado.CODIGOLINEA;
                model.Cargo = modelEmpleado.CODIGOCARGO;

                if (!clsDCuchillo.ValidarCuchilloEmpleadoPrestado(model))
                {
                    return Json("1", JsonRequestBehavior.AllowGet);

                }
                clsDCuchillo.GuardarModificarEmpleadoCuchilloPrestado(model);
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



        public JsonResult ConsultaEmpleadosPrestado(DateTime Fecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDCuchillo = new clsDCuchillo();
                clsDEmpleado = new clsDEmpleado();
                var empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                var lista = clsDCuchillo.ConsultaEmpleadoPrestadoPorLineaFecha(empleado.CODIGOLINEA, Fecha);
                return Json(lista, JsonRequestBehavior.AllowGet);
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
        public JsonResult ConsultaCuchillosDisponibles(DateTime Fecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDCuchillo = new clsDCuchillo();
                var Listado = new List<ControlDeAsistenciaPrestadosViewModel.Cuchillos>();
                var CuchillosBlancosSobrantes = clsDCuchillo.CuchillosSobrantes(clsAtributos.CodigoColorCuchilloBlanco, DateTime.Now).Select(x => new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Numero = x, Id = x, Color = 'B' }).ToList();
                var CuchillosRojosSobrantes = clsDCuchillo.CuchillosSobrantes(clsAtributos.CodigoColorCuchilloRojo, DateTime.Now).Select(x => new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Numero = x, Id = x, Color = 'R' }).ToList();
                var CuchillosNegrosSobrantes = clsDCuchillo.CuchillosSobrantes(clsAtributos.CodigoColorCuchilloNegro, DateTime.Now).Select(x => new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Numero = x, Id = x, Color = 'N' }).ToList();
                Listado.AddRange(CuchillosBlancosSobrantes);
                Listado.AddRange(CuchillosRojosSobrantes);
                Listado.AddRange(CuchillosNegrosSobrantes);
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

    }
}