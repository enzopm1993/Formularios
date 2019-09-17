using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia;
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

        #region Métodos
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        public void ConsultaCombosGeneral()
        {
            clsDGeneral = new clsDGeneral();
            ViewBag.Lineas = clsDGeneral.ConsultaLineas();
            //ViewBag.Areas = clsDGeneral.ConsultaAreas("0");
            //ViewBag.Cargos = clsDGeneral.ConsultaCargos("0");
        }
        #endregion

        // GET: Asistencia
        [Authorize]
        public ActionResult Asistencia()
        {
            try
            {
                TimeSpan hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
                clsDEmpleado = new clsDEmpleado();
                clsDGeneral = new clsDGeneral();
                liststring = User.Identity.Name.Split('_');
                clsDAsistencia = new clsDAsistencia();
                int AsitenciaExiste = clsDAsistencia.ConsultarExistenciaAsistencia(liststring[1]);
                ViewBag.AsistenciaExiste = AsitenciaExiste;
                //var Asistencia = clsDAsistencia.ObtenerAsistenciaDiaria(liststring[1]);
                ViewBag.Linea = clsDGeneral.ConsultarLineaUsuario(liststring[1]);
                ViewBag.CodLinea = clsDEmpleado.ConsultaEmpleado(liststring[1]).FirstOrDefault().CODIGOLINEA;
                //Asistencia.ControlAsistencia.ForEach(a=>a.Hora= hora);

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
                    UsuarioIngreso = liststring[1]
                });
                return RedirectToAction("Home", "Home");
            }
            
        }
        [Authorize]
        public ActionResult AsistenciaPartial(string CodLinea, int BanderaExiste)
        {
            try
            {
                clsDClasificador = new clsDClasificador();
                var EstadoAsistencia = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodigoGrupoEstadoAsistencia, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.EstadoAsistencia = EstadoAsistencia;

                clsDAsistencia = new clsDAsistencia();
                var AsistenciaViewModel = clsDAsistencia.ObtenerAsistenciaDiaria(CodLinea, BanderaExiste);
                return PartialView(AsistenciaViewModel);
            }
            catch (Exception ex)
            {

               return  Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GrabarAsistenciaEmpleado(string cedula, string nombre,TimeSpan  Hora,string observacion)
        {
            try
            {
                clsDAsistencia = new clsDAsistencia();
                string Resultado = clsDAsistencia.ActualizarAsistencia(new ASISTENCIA { Cedula=cedula, Hora=Hora, Observacion=observacion, EstadoAsistencia=clsAtributos.EstadoPresente});
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Json("",JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult RptAsistencia()
        {
            return View();
        }

        [Authorize]
        public ActionResult EditarAsistencia()
        {
            return View();
        }

        #region Cambio_PersonaldeÁrea
        [Authorize]
        public ActionResult CambiarPersonalDeArea()
        {
            try
            {
                ConsultaCombosGeneral();
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        [Authorize]
        public ActionResult BitacoraCambioPersonal()
        {
            try
            {
                clsDGeneral = new clsDGeneral();
                ViewBag.Lineas = clsDGeneral.ConsultaLineas();
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
                    UsuarioIngreso = liststring[1]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region CUCHILLO
        [Authorize]
        public ActionResult GuardarControlCuchillo(string dsCedula, string dsColor,string dsNumero,string dsEstado,bool dbCheck)
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
                poControlCuchillo.Fecha = DateTime.Now;
                poControlCuchillo.EstadoCuchillo = dsEstado;
                poControlCuchillo.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                poControlCuchillo.FechaIngresoLog = DateTime.Now;
                poControlCuchillo.UsuarioIngresoLog = liststring[0];
                poControlCuchillo.TerminalIngresoLog = Request.UserHostAddress;

                var respuesta = clsDCuchillo.GuardarModificarControlCuchillo(poControlCuchillo, dbCheck);
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
                    UsuarioIngreso = liststring[1]
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
                    UsuarioIngreso = liststring[1]
                });
                return RedirectToAction("Home", "Home");
            }


        }
        [Authorize]
        // GET: Asistencia/ControlCuchillo
        public ActionResult ControlCuchilloPartial(string dsEstado)
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
                    model = clsDCuchillo.ConsultarEmpleadosCuchilloPorLinea(Empleado.CODIGOLINEA, dsEstado);
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
                    UsuarioIngreso = liststring[1]
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
                    UsuarioIngreso = liststring[1]
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
                    ModelState.AddModelError("NumeroCuchillo", "CampoRequerido");
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
                    UsuarioIngreso = liststring[1]
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
                    UsuarioIngreso = liststring[1]
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
                    UsuarioIngreso = liststring[1]
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
                    UsuarioIngreso = liststring[1]
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
                var model = clsDCuchillo.ConsultarEmpleadoCuchillo(new Models.Asistencia.EmpleadoCuchilloViewModel());
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
                    UsuarioIngreso = liststring[1]
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
                    UsuarioIngreso = liststring[1]
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
        public ActionResult EmpleadosCambioPersonalPartial(string pslinea, string psarea, string pscargo,string tipo)
        {
            try
            {
                List<spConsutaEmpleadosFiltro> ListaEmpleados=new List<spConsutaEmpleadosFiltro>();
                clsDEmpleado = new clsDEmpleado();
                if (tipo == "prestar")
                {
                    ListaEmpleados = clsDEmpleado.ConsultaEmpleadosFiltroCambioPersonal(pslinea, psarea, pscargo, clsAtributos.TipoPrestar);

                }
                else
                {
                    ListaEmpleados = clsDEmpleado.ConsultaEmpleadosFiltroCambioPersonal(pslinea, psarea, pscargo, clsAtributos.TipoRegresar);
                }
                return PartialView(ListaEmpleados);
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
                    UsuarioIngreso = liststring[1]
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
                            pListCambioPersonal.Add(new CAMBIO_PERSONAL {
                                Cedula = pscedulas,
                                CodLinea = dlinea,
                                CodArea = darea,
                                FechaIngresoLog = DateTime.Now,
                                UsuarioIngresoLog = liststring[0],
                                TerminalIngresoLog = Request.UserHostAddress,
                                EstadoRegistro = "A"
                            });
                            pListBitacoraCambioPersonal.Add(new BITACORA_CAMBIO_PERSONAL {
                            Cedula= pscedulas,
                            Tipo=tipo=="prestar"?"P":"R",
                            CodLinea= dlinea,
                            CodArea= darea,
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
                    UsuarioIngreso = liststring[1]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
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
                    UsuarioIngreso = liststring[1]
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
                    UsuarioIngreso = liststring[1]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
