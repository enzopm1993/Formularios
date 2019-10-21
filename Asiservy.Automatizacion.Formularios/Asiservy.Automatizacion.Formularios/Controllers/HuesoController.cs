using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ControlHueso;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Empleado;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class HuesoController : Controller
    {
        string[] Usuario = null;
        clsDError clsDError = null;
        clsDEmpleado clsDEmpleado = null;
        clsDControlHueso clsDControlHueso = null;
        clsDClasificador clsDClasificador = null;
        clsDControlMiga clsDControlMiga = null;
        clsDGeneral clsDGeneral = null;
        clsDLogin clsDLogin = null;
        clsDApiOrdenFabricacion clsDApiOrdenFabricacion = null;
        #region CONTROL HUESOS
        [Authorize]
        public ActionResult ControlHueso()
        {
            try
            {

                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                clsDClasificador = new clsDClasificador();
                var TipoControlHueso = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodigoGrupoTipoControlHuesos, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                ViewBag.Linea = Empleado.LINEA;
                ViewBag.CodLinea = Empleado.CODIGOLINEA;
                ViewBag.TipoControlHueso = TipoControlHueso;
                ViewBag.OrdenesFabricacion = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaProduccion(DateTime.Now);

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
        public ActionResult ControlHuesoPartialCabecera(DateTime Fecha)
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDControlHueso = new clsDControlHueso();
                var model = clsDControlHueso.ConsultaControlHueso(Fecha);
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
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        public ActionResult ControlHuesoPartial(int id)
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDControlHueso = new clsDControlHueso();
                var model = clsDControlHueso.ConsultaControlHuesoDetalle(id);
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
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize]
        public JsonResult InactivarControlHueso(int id)
        {
            try
            {
                clsDControlHueso = new clsDControlHueso();
                Usuario = User.Identity.Name.Split('_');
                CONTROL_HUESO model = new CONTROL_HUESO
                {                    
                    IdControlHueso=id,
                    UsuarioIngresoLog = Usuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                clsDControlHueso.InactivarControlHueso(model);
                return Json("Registro Inactivado", JsonRequestBehavior.AllowGet);

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
        public ActionResult GenerarControlHueso(CONTROL_HUESO model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Linea)
                    || string.IsNullOrEmpty(model.Lote)
                    || model.Lote == "0"
                    || model.HoraInicio == TimeSpan.Parse("00:00")
                    || model.HoraFin == TimeSpan.Parse("00:00")
                    )
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json("Parametros Incompletos", JsonRequestBehavior.AllowGet);
                }
                if (model.HoraInicio > model.HoraFin)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json("Hora fin no puede ser mayor a la hora de inicio", JsonRequestBehavior.AllowGet);
                }

                Usuario = User.Identity.Name.Split('_');
                clsDControlHueso = new clsDControlHueso();
                int id = clsDControlHueso.GenerarControlHueso(new CONTROL_HUESO
                {
                    Linea = model.Linea,
                    TipoControlHueso = model.TipoControlHueso,
                    HoraInicio = model.HoraInicio,
                    HoraFin = model.HoraFin,
                    Lote = model.Lote,
                    Fecha = model.Fecha,
                    OrdenFabricacion = model.OrdenFabricacion,
                    Observacion = model.Observacion,
                    TotalPieza = model.TotalPieza,
                    EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                    UsuarioIngresoLog = Usuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                });
                // var listadoLimpiadoras = clsDControlHueso.ConsultaLimpiadorasControlHueso(dsLinea);
                //  clsDControlHueso.GenerarControlHuesoDetalle(listadoLimpiadoras, id, Usuario[0], Request.UserHostAddress);
                return Json(id, JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        public ActionResult ValidaControlHueso(string dsLinea, string Hora)
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDControlHueso = new clsDControlHueso();
                int id = clsDControlHueso.ValidaControlHueso(new CONTROL_HUESO
                {
                    Linea = dsLinea,
                    //Hora = TimeSpan.Parse(Hora)    
                });
                // var listadoLimpiadoras = clsDControlHueso.ConsultaLimpiadorasControlHueso(dsLinea);
                //  clsDControlHueso.GenerarControlHuesoDetalle(listadoLimpiadoras, id, Usuario[0], Request.UserHostAddress);
                return Json(id, JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        [HttpPost]
        public ActionResult GuardarControlHueso(CONTROL_HUESO_DETALLE detalle, decimal diMiga)
        {
            try
            {
                if (detalle == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json("Parametros Incompletos", JsonRequestBehavior.AllowGet);
                }

                Usuario = User.Identity.Name.Split('_');
                clsDControlHueso = new clsDControlHueso();
                clsDControlMiga = new clsDControlMiga();
                detalle.UsuarioIngresoLog = Usuario[0];
                detalle.TerminalIngresoLog = Request.UserHostAddress;
                this.clsDControlMiga.GuardarModificarControlMiga(new CONTROL_MIGA
                {
                    IdControlHuesoMiga = detalle.IdControlHuesoDetalle,
                    Miga = diMiga,
                    EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                    FechaIngresoLog = DateTime.Now,
                    UsuarioIngresoLog = Usuario[0],
                    TerminalIngresoLog = Request.UserHostAddress
                });
                var respuesta = clsDControlHueso.GuardarModificarControlHueso(detalle);

                return Json(respuesta, JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult ConsultarOrdenesFabricacion(DateTime Fecha)
        {
            try
            {
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                dynamic result= clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaProduccion(Fecha);
                List<OrdenFabricacion> Listado = new List<OrdenFabricacion>();
                foreach(var x in result)
                {
                    Listado.Add(new OrdenFabricacion { Orden = x.OrdenFabricacion });
                }

                return Json(Listado, JsonRequestBehavior.AllowGet);
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
        public JsonResult ConsultarLotesPorLinea(int Orden)
        {
            try
            {
                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                var Linea = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                dynamic result = clsDApiOrdenFabricacion.ConsultaLotesPorOrdenFabricacionLinea(Orden,Linea.CODIGOLINEA);
                List<ClasificadorGenerico> Listado = new List<ClasificadorGenerico>();
                foreach (var x in result)
                {
                    Listado.Add(new ClasificadorGenerico { descripcion = x.Lote });
                }

                return Json(Listado, JsonRequestBehavior.AllowGet);
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





        #region REPORTE CONTROL HUESO - MIGA
        [Authorize]
        public ActionResult ReporteControlAvanceDiario()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];


                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDClasificador = new clsDClasificador();
                clsDGeneral = new clsDGeneral();
                clsDLogin = new clsDLogin();

                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                ViewBag.LineaEmpleado = Empleado.CODIGOLINEA;                
                List<int?> roles = clsDLogin.ConsultaRolesUsuario(Usuario[1]);
                if (roles.FirstOrDefault(x=> x.Value ==clsAtributos.RolSupervisorGeneral || x.Value == clsAtributos.RolControladorGeneral) != null)
                {
                    ViewBag.SupervisorGeneral = clsAtributos.RolSupervisorGeneral;
                }
                ViewBag.ComboLineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador {Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro=clsAtributos.EstadoRegistroActivo }); 
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
        public ActionResult ReporteControlAvanceDiarioPartial(DateTime ddFecha,string dsLinea)
        {
            try
            {
               

                // Usuario = User.Identity.Name.Split('_');
                clsDControlHueso = new clsDControlHueso();
                var model = clsDControlHueso.ConsultaControlAvanceDiarioPorLinea(ddFecha.Date, dsLinea);
                if (!model.Any())
                    return Json("1",JsonRequestBehavior.AllowGet);
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
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize]
        public ActionResult ReporteAvanceDiarioPorLimpiadora()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];


                Usuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDClasificador = new clsDClasificador();
                clsDGeneral = new clsDGeneral();
                clsDLogin = new clsDLogin();

                var Empleado = clsDEmpleado.ConsultaEmpleado(Usuario[1]).FirstOrDefault();
                ViewBag.LineaEmpleado = Empleado.CODIGOLINEA;
                List<int?> roles = clsDLogin.ConsultaRolesUsuario(Usuario[1]);
                if (roles.FirstOrDefault(x => x.Value == clsAtributos.RolSupervisorGeneral) != null)
                {
                    ViewBag.SupervisorGeneral = clsAtributos.RolSupervisorGeneral;
                }
                ViewBag.ComboLineas = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
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
        public ActionResult ReporteAvanceDiarioPorLimpiadoraPartial(DateTime ddFecha, string dsLinea)
        {
            try
            {
                // Usuario = User.Identity.Name.Split('_');
                clsDControlHueso = new clsDControlHueso();
                var model = clsDControlHueso.ConsultaControlAvanceDiarioPorLimpiadora(ddFecha.Date, dsLinea);
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
                    UsuarioIngreso = Usuario[0]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion



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