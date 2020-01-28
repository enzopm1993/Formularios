using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Empleado;
using Asiservy.Automatizacion.Formularios.Models.Empleado;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class EmpleadoController : Controller
    {

        string[] Usuario = null;
        clsDError clsDError = null;
        clsDEmpleado clsDEmpleado = null;
        clsDEmpleadoEsfero clsDEmpleadoEsfero = null;
        clsDClasificador clsDClasificador = null;
        clsDLogin clsDLogin = null;
        clsDGeneral clsDGeneral = null;

        #region EMPLEADO ESFERO

        [Authorize]
        public ActionResult ControlEsfero()
        {
            try
            {
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                ViewBag.Linea = Empleado.LINEA;

                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return RedirectToAction("Home", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult ControlEsfero(spConsutaControlEsferos model, string dsTipo)
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleadoEsfero = new clsDEmpleadoEsfero();

                var respuesta = clsDEmpleadoEsfero.GuardarModificarControlEsfero(new CONTROL_ESFERO
                {
                    Cedula = model.Cedula,
                    HoraInicio = model.Hora,
                    UsuarioIngresoLog = Usuario[0],
                    TerminalIngresoLog = Request.UserHostAddress
                }, dsTipo);

                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                // SetErrorMessage(ex.Message);
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[1]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult ControlEsferoPartial(string dsTipo)
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDEmpleadoEsfero = new clsDEmpleadoEsfero();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                List<spConsutaControlEsferos> model = new List<spConsutaControlEsferos>();
                if (Empleado != null)
                {
                    clsDEmpleadoEsfero.GenerarControlEmpleadoEsfero(Empleado.CODIGOLINEA, Usuario[0], Request.UserHostAddress);
                    model = clsDEmpleadoEsfero.ConsultaControlEsfero(Empleado.CODIGOLINEA, dsTipo);

                }

                return PartialView(model);
            }
            catch (Exception ex)
            {

                // SetErrorMessage(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[1]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult EmpleadoEsfero()
        {
            try
            {

                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                if (Empleado != null)
                {
                    var ListaEmpleados = clsDEmpleado.ConsultaEmpleadosFiltro(Empleado.CODIGOLINEA, null, null);
                    ViewBag.Empleados = ListaEmpleados;
                }

                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[1]
                });
                return RedirectToAction("Home", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult EmpleadoEsfero(EmpleadoEsferoViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDEmpleadoEsfero = new clsDEmpleadoEsfero();
                model.EstadoRegistro = model.EstadoRegistro == "true" ? clsAtributos.EstadoRegistroActivo : clsAtributos.EstadoRegistroInactivo;
                model.UsuarioIngresoLog = Usuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                var Respuesta = clsDEmpleadoEsfero.GuardarMoficicarEsfero(model);
                SetSuccessMessage(Respuesta);
                return RedirectToAction("EmpleadoEsfero");
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[1]
                });
                return RedirectToAction("EmpleadoEsfero");
            }
        }

        [Authorize]
        public ActionResult EmpleadoEsferoPartial()
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDEmpleadoEsfero = new clsDEmpleadoEsfero();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                List<EmpleadoEsferoViewModel> ListaEmpleadoEsfero = new List<EmpleadoEsferoViewModel>();
                if (Empleado != null)
                {
                    ListaEmpleadoEsfero = clsDEmpleadoEsfero.ConsultaEmpleadoEsfero(Empleado.CODIGOLINEA);

                }

                return PartialView(ListaEmpleadoEsfero);
            }
            catch (Exception ex)
            {

                // SetErrorMessage(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion


        #region EMPLEADO TURNO
        // GET: Empleado
        [Authorize]
        public ActionResult EmpleadoTurno()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                Usuario = User.Identity.Name.Split('_');
                clsDLogin = new clsDLogin();
                clsDGeneral = new clsDGeneral();
                clsDEmpleado = new clsDEmpleado();
                clsDClasificador = new clsDClasificador();

                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                ViewBag.LineaEmpleado = Empleado.CODIGOLINEA;
                List<int?> roles = clsDLogin.ConsultaRolesUsuario(Usuario[1]);
                if (roles.FirstOrDefault(x => x.Value == clsAtributos.RolSupervisorGeneral || x.Value == clsAtributos.RolControladorGeneral) != null)
                {
                    ViewBag.Lineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineasAprobarSolicitudProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                }
                else if (roles.FirstOrDefault(x => x.Value == clsAtributos.RolSupervisorLinea || x.Value == clsAtributos.RolControladorLinea) != null)
                {
                    ViewBag.Lineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo, Codigo = Empleado.CODIGOLINEA });
                }
                else
                {
                    ViewBag.Lineas = clsDGeneral.ConsultaLineas(Empleado.CODIGOLINEA);
                }
                return View();
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                Usuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(Usuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                Usuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(Usuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return RedirectToAction("Home", "Home");
            }

        }

        public ActionResult EmpleadoTurnoPartial(string Linea, DateTime Fecha)
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                List<EmpleadoViewModel> model = new List<EmpleadoViewModel>();
                if (Empleado != null)
                {
                    model = clsDEmpleado.ConsultaEmpleadoTurno(Linea, Fecha);
                }
                return PartialView(model);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                Usuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(Usuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                Usuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(Usuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public ActionResult EmpleadoTurnoPartial(EmpleadoViewModel model)
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(Usuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (!ModelState.IsValid)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                clsDEmpleado = new clsDEmpleado();
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.FechaIngreso = DateTime.Now;
                model.TerminalIngreso = Request.UserHostAddress;
                model.UsuarioIngreso = Usuario[0];
                clsDEmpleado.GuardarModificarEmpleadoTurno(model);
                return Json("Registro Exitoso", JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                Usuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(Usuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                Usuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(Usuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ReporteEmpleadoTurno()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDGeneral = new clsDGeneral();
                ViewBag.Lineas = clsDGeneral.ConsultaLineas("0");
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[1]
                });
                return RedirectToAction("Home", "Home");
            }
        }

        public ActionResult ReporteEmpleadoTurnoPartial(string dsTurno, string dsLinea)
        {
            try
            {


                clsDEmpleado = new clsDEmpleado();
                Usuario = User.Identity.Name.Split('_');
                List<spConsutaReporteEmpleadosTurnos> model = new List<spConsutaReporteEmpleadosTurnos>();
                // var empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();

                model = clsDEmpleado.ConsultaReporteEmpleadoTurno(dsLinea, dsTurno);
                if (!model.Any())
                    return Json("0", JsonRequestBehavior.AllowGet);


                return PartialView(model);
            }
            catch (Exception ex)
            {

                //SetErrorMessage(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[1]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region EMPLEADO CARGO
        [Authorize]
        public ActionResult EmpleadoCargo()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDGeneral = new clsDGeneral();
                clsDLogin = new clsDLogin();
                //clsDClasificador = new clsDClasificador();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                ViewBag.Lineas = clsDGeneral.ConsultaLineas("0");
                ViewBag.Cargos = clsDGeneral.ConsultaCargos("0");
                ViewBag.Linea = Empleado.LINEA;
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return RedirectToAction("Home", "Home");
            }

        }
        [Authorize]
        [HttpPost]
        public ActionResult EmpleadoCargo(EmpleadoViewModel model)
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                //Servicio de actualizar datalife
                return Json("ok", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }

        }
        [Authorize]
        public ActionResult ReporteEmpleadoCargo()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDGeneral = new clsDGeneral();
                clsDLogin = new clsDLogin();
                clsDClasificador = new clsDClasificador();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                List<int?> roles = clsDLogin.ConsultaRolesUsuario(Usuario[1]);
                if (roles.FirstOrDefault(x => x.Value == clsAtributos.RolSupervisorGeneral || x.Value == clsAtributos.AsistenteProduccion || x.Value == clsAtributos.RolControladorGeneral) != null)
                {
                    ViewBag.SupervisorGeneral = clsAtributos.RolSupervisorGeneral;
                    var lineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineasAprobarSolicitudProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                    lineas.Add(new Models.Seguridad.Clasificador { Codigo = "T", Descripcion = "Todos" });
                    ViewBag.Lineas = lineas;
                }
                else if (roles.FirstOrDefault(x => x.Value == clsAtributos.RolSupervisorLinea || x.Value == clsAtributos.RolControladorLinea) != null)
                {
                    ViewBag.Lineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo, Codigo = Empleado.CODIGOLINEA });
                }
                else if (roles.FirstOrDefault(x => x.Value == clsAtributos.SeguridadIndustrial) != null)
                {
                    ViewBag.Lineas = clsDGeneral.ConsultaLineas("0");
                }
                else
                {
                    ViewBag.Lineas = clsDGeneral.ConsultaLineas(Empleado.CODIGOLINEA);
                }             
                ViewBag.Linea = Empleado.LINEA;
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return RedirectToAction("Home", "Home");
            }

        }

        [Authorize]
        public ActionResult ReporteEmpleadoCargoPartial(string Linea)
        {
            try
            {
                clsDEmpleado = new clsDEmpleado();
                var model = clsDEmpleado.ConsultaEmpleadoCargoLinea(Linea);
                if (!model.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);

                }
                return PartialView(model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion


        #region REPORTES

        [Authorize]
        public ActionResult ReportePersonalNominaPorLinea()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                var model = clsDEmpleado.ConsultaPersonalNominaPorLinea();
                return View(model);
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return RedirectToAction("Home", "Home");
            }
        }
        [Authorize]
        public ActionResult ReporteDistribucionPorLinea()
        {
            try
            {
                ViewBag.dataTableJS = "1";

                Usuario = User.Identity.Name.Split('_');
                clsDClasificador = new clsDClasificador();
                clsDEmpleado = new clsDEmpleado();
                clsDLogin = new clsDLogin();
                clsDGeneral = new clsDGeneral();
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                ViewBag.LineaEmpleado = Empleado.CODIGOLINEA;              
                
                List<int?> roles = clsDLogin.ConsultaRolesUsuario(Usuario[1]);
                if (roles.FirstOrDefault(x => x.Value == clsAtributos.RolSupervisorGeneral || x.Value == clsAtributos.RolControladorGeneral) != null)
                {
                    ViewBag.SupervisorGeneral = clsAtributos.RolSupervisorGeneral;
                    ViewBag.Lineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineasAprobarSolicitudProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                }
                else if (roles.FirstOrDefault(x => x.Value == clsAtributos.AsistenteProduccion) != null)
                {
                    ViewBag.SupervisorGeneral = clsAtributos.RolSupervisorGeneral;
                    ViewBag.Lineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineasAprobarSolicitudProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                }
                else if (roles.FirstOrDefault(x => x.Value == clsAtributos.RolSupervisorLinea || x.Value == clsAtributos.RolControladorLinea) != null)
                {
                    ViewBag.Lineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo, Codigo = Empleado.CODIGOLINEA });


                }
                else
                {
                    ViewBag.Lineas = clsDGeneral.ConsultaLineas(Empleado.CODIGOLINEA);


                }

                return View();


            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return RedirectToAction("Home", "Home");
            }
        }
       
        public ActionResult ReporteDistribucionPorLineaPartial(string Linea, DateTime Fecha,string Turno)
        {
            try
            {
                if (string.IsNullOrEmpty(Linea) || Fecha == null)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);

                }

                ViewBag.dataTableJS = "1";

                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                ViewBag.InicioJornada = clsDEmpleado.ConsultaFechaInicioJornada(Linea, Fecha, Turno);
                var model = clsDEmpleado.spConsultaDistribucionPorLinea(Linea, Fecha, Turno);
                return PartialView(model);
            }
            catch (Exception ex)
            {

                // SetErrorMessage(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ReportePersonalPresente()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];

                return View();


            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return RedirectToAction("Home", "Home");
            }
        }

        public ActionResult ReportePersonalPresentePartial( DateTime Fecha)
        {
            try
            {
                if (Fecha == null)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }

                ViewBag.dataTableJS = "1";

                clsDEmpleado = new clsDEmpleado();
                var model = clsDEmpleado.spConsultaPresentesPorAreaLinea(Fecha);

                return PartialView(model);
            }
            catch (Exception ex)
            {

                // SetErrorMessage(ex.Message);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Usuario = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        [Authorize]
        public ActionResult EmpleadosMasivoPartial(string Linea)
        {
            try
            {
                clsDEmpleado = new clsDEmpleado();
                var model = clsDEmpleado.ConsultaEmpleadosFiltro(Linea, "0", "0");
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
                Usuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(Usuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                Usuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(Usuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
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