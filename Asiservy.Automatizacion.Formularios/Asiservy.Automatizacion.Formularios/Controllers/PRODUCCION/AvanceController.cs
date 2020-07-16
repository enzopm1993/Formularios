using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ControlHueso;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Empleado;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ProyeccionProgramacion;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class AvanceController : Controller
    {
        string[] lsUsuario { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        clsDEmpleado clsDEmpleado { get; set; } = null;
        clsDControlHueso clsDControlHueso { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDControlMiga clsDControlMiga { get; set; } = null;
        clsDGeneral clsDGeneral { get; set; } = null;
        clsDLogin clsDLogin { get; set; } = null;
        clsDProyeccionProgramacion clsDProyeccionProgramacion { get; set; } = null;
        clsDApiOrdenFabricacion clsDApiOrdenFabricacion { get; set; } = null;
        clsDApiProduccion clsDApiProduccion { get; set; } = null;

        #region CONTROL HUESOS
        [Authorize]
        public ActionResult ControlHueso()
        {
            try
            {

                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                lsUsuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                clsDClasificador = new clsDClasificador();
                var TipoControlHueso = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodigoGrupoTipoControlHuesos, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                var Empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();

                ViewBag.Linea = Empleado.LINEA;
                ViewBag.CodLinea = Empleado.CODIGOLINEA;
                ViewBag.TipoControlHueso = TipoControlHueso;
                ViewBag.OrdenesFabricacion = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaProduccion(DateTime.Now);

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

       
        public ActionResult ControlHuesoPartialCabecera(DateTime Fecha, string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlHueso = new clsDControlHueso();
                clsDEmpleado = new clsDEmpleado();
                var linea = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
            
                var model = clsDControlHueso.ConsultaControlHueso(Fecha, linea.CODIGOLINEA,Turno);
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

       
        public ActionResult ControlHuesoPartial(int id)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlHueso = new clsDControlHueso();
                var model = clsDControlHueso.ConsultaControlHuesoDetalle(id);
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
        
        public JsonResult InactivarControlHueso(int id)
        {
            try
            {
                clsDControlHueso = new clsDControlHueso();
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                CONTROL_HUESO model = new CONTROL_HUESO
                {                    
                    IdControlHueso=id,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                clsDControlHueso.InactivarControlHueso(model);
                return Json("Registro Inactivado", JsonRequestBehavior.AllowGet);

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
        public ActionResult GenerarControlHueso(CONTROL_HUESO model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.Linea)
                    || string.IsNullOrEmpty(model.Lote)
                    || model.Lote == "0"
                    || model.OrdenFabricacion<1
                    || model.Limpieza == "0"
                    || string.IsNullOrEmpty(model.Limpieza)                   
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

                lsUsuario = User.Identity.Name.Split('_');
                clsDControlHueso = new clsDControlHueso();
                int id = clsDControlHueso.GenerarControlHueso(new CONTROL_HUESO
                {
                    Turno = model.Turno,
                    Linea = model.Linea,
                    TipoControlHueso = model.TipoControlHueso,
                    HoraInicio = model.HoraInicio,
                    HoraFin = model.HoraFin,
                    Lote = model.Lote,
                    Fecha = model.Fecha,
                    OrdenFabricacion = model.OrdenFabricacion,
                    Observacion = model.Observacion,
                    TotalPieza = model.TotalPieza,
                    TotalLimpiadoras = model.TotalLimpiadoras,
                    Limpieza = model.Limpieza,
                    EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                });
                // var listadoLimpiadoras = clsDControlHueso.ConsultaLimpiadorasControlHueso(dsLinea);
                //  clsDControlHueso.GenerarControlHuesoDetalle(listadoLimpiadoras, id, lsUsuario[0], Request.UserHostAddress);
                return Json(id, JsonRequestBehavior.AllowGet);
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

  
        public ActionResult ValidaControlHueso(string dsLinea, string Hora)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlHueso = new clsDControlHueso();
                int id = clsDControlHueso.ValidaControlHueso(new CONTROL_HUESO
                {
                    Linea = dsLinea,
                     
                });              
                return Json(id, JsonRequestBehavior.AllowGet);
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
        public ActionResult GuardarControlHueso(CONTROL_HUESO_DETALLE detalle, decimal diMiga)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                if (detalle == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json("Parametros Incompletos", JsonRequestBehavior.AllowGet);
                }

              
                clsDControlHueso = new clsDControlHueso();
                clsDControlMiga = new clsDControlMiga();
                detalle.UsuarioIngresoLog = lsUsuario[0];
                detalle.TerminalIngresoLog = Request.UserHostAddress;
                this.clsDControlMiga.GuardarModificarControlMiga(new CONTROL_MIGA
                {
                    IdControlHuesoMiga = detalle.IdControlHuesoDetalle,
                    Miga = diMiga,
                    EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                    FechaIngresoLog = DateTime.Now,
                    UsuarioIngresoLog = lsUsuario[0],
                    TerminalIngresoLog = Request.UserHostAddress
                });
                var respuesta = clsDControlHueso.GuardarModificarControlHueso(detalle);

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

        public JsonResult ModificarControl(CONTROL_HUESO model)
        {
            try
            {
                clsDControlHueso = new clsDControlHueso();
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                RespuestaGeneral respuesta = new RespuestaGeneral();
                if (model.IdControlHueso == 0 ||
                    model.HoraInicio == null || 
                    model.HoraFin == null|| string.IsNullOrEmpty(model.Limpieza))
                {
                    return Json("0", JsonRequestBehavior.AllowGet);

                }
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                respuesta = clsDControlHueso.GuardarModificarControl(model);
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


        public JsonResult ConsultarOrdenesFabricacion(DateTime Fecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                dynamic result= clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaProduccion(Fecha);
                List<OrdenFabricacion> Listado = new List<OrdenFabricacion>();
                foreach(var x in result)
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
        public JsonResult ConsultarLotesPorLinea(int Orden)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDEmpleado = new clsDEmpleado();
                var Linea = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                dynamic result = clsDApiOrdenFabricacion.ConsultaLotesPorOrdenFabricacionLinea(Orden,Linea.CODIGOLINEA);
                List<ClasificadorGenerico> Listado = new List<ClasificadorGenerico>();
                foreach (var x in result)
                {
                    Listado.Add(new ClasificadorGenerico { descripcion = x.Lote });
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

        #region EFICIENCIA AVANCE KILOS/HORA
        [Authorize]
        public ActionResult EficienciaAvanceKiloHora()
        {
            try
            {
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                clsDApiProduccion = new clsDApiProduccion();
                ViewBag.Tallas=clsDApiProduccion.ConsultarTallas(null);
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

        [Authorize]
        [HttpPost]
        public ActionResult EficienciaAvanceKiloHora(AVANCE_KILOS_HORA model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlHueso = new clsDControlHueso();
                model.FechaIngresoLog = DateTime.Now;
                model.UsuarioIngresoLog = lsUsuario[1];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                clsDControlHueso.GuardarModificarEficienciaAvanceKilosHora(model);
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

        public ActionResult EficienciaAvanceKiloHoraPartial()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlHueso = new clsDControlHueso();
                var model = clsDControlHueso.ConsultaEficienciaAvanceKilosHora();
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

        #region REPORTE CONTROL HUESO - MIGA
        [Authorize]
        public ActionResult ReporteControlAvanceDiario()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.DateRangePicker = "1";


                lsUsuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDClasificador = new clsDClasificador();
                clsDGeneral = new clsDGeneral();
                clsDLogin = new clsDLogin();

                var Empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                ViewBag.LineaEmpleado = Empleado.CODIGOLINEA;                
                List<int?> roles = clsDLogin.ConsultaRolesUsuario(lsUsuario[1]);
                if (roles.FirstOrDefault(x => x.Value == clsAtributos.RolSupervisorGeneral || x.Value == clsAtributos.AsistenteProduccion || x.Value == clsAtributos.RolControladorGeneral) != null)
                {
                    ViewBag.SupervisorGeneral = clsAtributos.RolSupervisorGeneral;
                    ViewBag.Linea = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                }              
                else
                {
                    ViewBag.Linea = clsDGeneral.ConsultaLineas(Empleado.CODIGOLINEA);
                }
              
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
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
                return RedirectToAction("Home", "Home");
            }

        }

        
        public ActionResult ReporteControlAvanceDiarioPartial(DateTime ddFechaDesde, DateTime ddFechaHasta, string Turno, string dsLinea)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }                            
                clsDControlHueso = new clsDControlHueso();
                var model = clsDControlHueso.ConsultaControlAvanceDiarioPorLinea(ddFechaDesde.Date,ddFechaHasta.Date,Turno, dsLinea);
                if (!model.Any())
                    return Json("1",JsonRequestBehavior.AllowGet);
                return PartialView(model);
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
        public ActionResult ReporteAvanceDiarioPorLimpiadora()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.Apexcharts = "1";

                lsUsuario = User.Identity.Name.Split('_');
                clsDEmpleado = new clsDEmpleado();
                clsDClasificador = new clsDClasificador();
                clsDGeneral = new clsDGeneral();
                clsDLogin = new clsDLogin();

                var Empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                ViewBag.LineaEmpleado = Empleado.CODIGOLINEA;
                List<int?> roles = clsDLogin.ConsultaRolesUsuario(lsUsuario[1]);
                if (roles.FirstOrDefault(x => x.Value == clsAtributos.RolSupervisorGeneral || x.Value == clsAtributos.AsistenteProduccion || x.Value == clsAtributos.RolControladorGeneral) != null)
                {
                    ViewBag.SupervisorGeneral = clsAtributos.RolSupervisorGeneral;
                    ViewBag.Linea = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                }
                else
                {
                    ViewBag.Linea = clsDGeneral.ConsultaLineas(Empleado.CODIGOLINEA);
                }
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
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
                return RedirectToAction("Home", "Home");
            }

        }

        
        public ActionResult ReporteAvanceDiarioPorLimpiadoraPartial(DateTime ddFecha, string dsLinea, string dsTurno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlHueso = new clsDControlHueso();
                var model = clsDControlHueso.ConsultaControlAvanceDiarioPorLimpiadora(ddFecha.Date, dsLinea, dsTurno);
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
        public JsonResult ConsultaKpiAvanceLimpiadora(DateTime Fecha, string Cedula)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDControlHueso = new clsDControlHueso();
                var model = clsDControlHueso.ConsultaKpiAvanceDiarioPorLimpiadora(Fecha, Cedula);
                return Json(model,JsonRequestBehavior.AllowGet);
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
        public ActionResult ReporteControlAvanceDiarioGeneral()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.Apexcharts = "1";
                ViewBag.Handsontable = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDClasificador = new clsDClasificador();
                ViewBag.Linea = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });

                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
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
                return RedirectToAction("Home", "Home");
            }

        }

  
        public ActionResult ReporteControlAvanceDiarioGeneralPartial(DateTime ddFecha, string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                clsDControlHueso = new clsDControlHueso();
                var model = clsDControlHueso.ConsultaControlAvanceDiario(ddFecha.Date,Turno);
                if (!model.Any())
                    return Json("1", JsonRequestBehavior.AllowGet);
                return PartialView(model);
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

        public ActionResult ConsultaControlAvanceDiarioGeneral(DateTime ddFecha,string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                clsDControlHueso = new clsDControlHueso();
                var model = clsDControlHueso.ConsultaControlAvanceDiario(ddFecha.Date,Turno);
                return Json(model,JsonRequestBehavior.AllowGet);
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


        #endregion


        #region REPORTE DE RENDIMIENTOS


        [Authorize]
        public ActionResult ReporteRendimientoLote()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.Apexcharts = "1";
                ViewBag.Handsontable = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDClasificador = new clsDClasificador();
                ViewBag.Linea = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.Turno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
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
                return RedirectToAction("Home", "Home");
            }

        }


        public ActionResult ReporteRendimientoLotePartial(DateTime ddFecha, string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                // lsUsuario = User.Identity.Name.Split('_');s
                clsDControlHueso = new clsDControlHueso();
                var model = clsDControlHueso.ConsultaReporteRendimientoPorLte(ddFecha.Date, Turno);
                if (!model.Any())
                { return Json("1", JsonRequestBehavior.AllowGet); }
                ViewBag.Model = model;
                return PartialView(model);
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
        public ActionResult ReporteRendimientoLinea()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.Apexcharts = "1";
                ViewBag.Handsontable = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDClasificador = new clsDClasificador();
                // ViewBag.Linea = clsDClasificador.ConsultaClasificador(new Models.Seguridad.Clasificador { Grupo = clsAtributos.CodGrupoLineaProduccion, EstadoRegistro = clsAtributos.EstadoRegistroActivo });
                ViewBag.Turno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno);
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
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
                return RedirectToAction("Home", "Home");
            }

        }


        public ActionResult ReporteRendimientoLineaPartial(DateTime ddFecha, string Turno)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                clsDClasificador = new clsDClasificador();
                clsDControlHueso = new clsDControlHueso();
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                var model = clsDControlHueso.ConsultaReporteRendimientoPorLinea(ddFecha.Date, Turno);
                if (!model.Any())
                { return Json("1", JsonRequestBehavior.AllowGet); }
                ViewBag.Toneladas = clsDProyeccionProgramacion.ConsultaProyeccionProgramacionReporte(ddFecha.Date, Turno).Sum(x=> x.Toneladas);
                ViewBag.Lineas = clsDClasificador.ConsultarClasificador(clsAtributos.CodGrupoLineaProduccion);
                return PartialView(model);
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
        public ActionResult ReporteEficienciaRendimiento()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.Apexcharts = "1";
                ViewBag.Handsontable = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                clsDControlHueso = new clsDControlHueso();
                ViewData["modelEficiencia"] = clsDControlHueso.ConsultaEficienciaAvanceKilosHora();
                ViewData["modelRendimiento"] = clsDControlHueso.ConsultaRendimientos();
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
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
                return RedirectToAction("Home", "Home");
            }

        }


        public ActionResult ReporteEficienciaRendimientoPartial()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                clsDControlHueso = new clsDControlHueso();
                var model = clsDControlHueso.ConsultaRendimientos();

                return PartialView(model);
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