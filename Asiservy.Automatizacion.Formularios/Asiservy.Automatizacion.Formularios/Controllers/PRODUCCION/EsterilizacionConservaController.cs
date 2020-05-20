using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.PRODUCCION.CocheAutoclave;
using Asiservy.Automatizacion.Formularios.AccesoDatos.PRODUCCION.EsterilizacionConserva;
using Asiservy.Automatizacion.Formularios.Models.Produccion.EsterilizacionConservas;
//using Asiservy.Automatizacion.Formularios.AccesoDatos.PRODUCCION.CocheAutoclave;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers.PRODUCCION
{
    public class EsterilizacionConservaController : Controller
    {
        string[] lsUsuario = null;
        clsDError clsDError = null;
        clsDEsterilizacionConserva clsDEsterilizacionConserva = null;
        clsDCcocheAutoclave clsDCcocheAutoclave = null;
        //clsDEmpleado clsDEmpleado = null;
        //clsDClasificador clsDClasificador = null;
        //clsDApiProduccion clsDApiProduccion = null;
        //clsDCcocheAutoclave clsDCcocheAutoclave = null;
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        // GET: EsterilizacionConserva
        [Authorize]
        public ActionResult ControlEsterilizacionConserva()
        {
            try
            {
                ViewBag.JavaScrip = "PRODUCCION/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                lsUsuario = User.Identity.Name.Split('_');

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
        public ActionResult ReporteControlEsterilizacionConserva()
        {
            try
            {
                ViewBag.JavaScrip = "PRODUCCION/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                lsUsuario = User.Identity.Name.Split('_');

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
        public ActionResult PartialReporteControl(DateTime Fecha, string Turno, string Linea)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                //clsDCcocheAutoclave = new clsDCcocheAutoclave();
                clsDEsterilizacionConserva = new clsDEsterilizacionConserva();
                //List<spConsultaCocheAutoclaveEsterilizacion> model = clsDCcocheAutoclave.ConsultaCocheAutoclaveEsterilizacion(Fecha, Turno, CabControl);
                //if (!model.Any())
                //{
                //    return Json("0", JsonRequestBehavior.AllowGet);
                //}
                List<spReporteEsterilizacionDetalle> detallereporte = clsDEsterilizacionConserva.ConsultarDetalleReporteControlEsterilizacion(Fecha, Turno, Linea);
                List<COCHE_AUTOCLAVE_DETALLE> DetalleCoches = null;
                List<TIPO_ESTERILIZACION_CONSERVA> TiposEsterilizacion = null;
                ReporteEsterilizacionViewModel Reporte = null;
                ViewBag.Registros = 0;
                if (detallereporte.Count != 0)
                {
                    DetalleCoches = clsDEsterilizacionConserva.ConsultarReporteDetallesCoches(detallereporte.Select(x=>x.IdCocheAutoclave).ToArray());
                    TiposEsterilizacion = clsDEsterilizacionConserva.ConsultarTiposReporteEsterilizado(detallereporte.Select(c => c.IdDetalleControlEsterilizacionConserva).ToArray());
                    Reporte = new ReporteEsterilizacionViewModel()
                    {
                        ListDetalleReporte = detallereporte,
                        ListDetalleCoches = DetalleCoches,
                        ListTipoEsterilizacion = TiposEsterilizacion
                    };
                    ViewBag.Registros = 1;
                }
                
                return PartialView(Reporte);
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
        public JsonResult GuardarModificarCabeceraEsterilizacion(CABECERA_CONTROL_ESTERILIZACION_CONSERVAS poControlEsterilizacion)
        {
            try
            {
                
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                poControlEsterilizacion.FechaIngresoLog = DateTime.Now;
                poControlEsterilizacion.UsuarioIngresoLog = lsUsuario[0];
                poControlEsterilizacion.TerminalIngresoLog = Request.UserHostAddress;
                poControlEsterilizacion.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado=null;
                clsDEsterilizacionConserva = new clsDEsterilizacionConserva();
                if (poControlEsterilizacion.IdCabControlEsterilizado == 0)
                {
                    resultado = clsDEsterilizacionConserva.GuardarCabEsterilizacionConserva(poControlEsterilizacion);
                }
                else
                {
                    resultado = clsDEsterilizacionConserva.ActualizarCabEsterilizacionConserva(poControlEsterilizacion);
                }
                
                //clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                //string resultado = clsDControlConsumoInsumo.GuardarPallet(pallet);
                return Json(resultado, JsonRequestBehavior.AllowGet);
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
        public JsonResult ConsultarCabeceraEsterilizacion(CABECERA_CONTROL_ESTERILIZACION_CONSERVAS poControlEsterilizacion)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                
                CABECERA_CONTROL_ESTERILIZACION_CONSERVAS resultado = null;
                clsDEsterilizacionConserva = new clsDEsterilizacionConserva();
                resultado = clsDEsterilizacionConserva.ConsultarCabeceraEsterilizacionConserva(poControlEsterilizacion);
                if (resultado != null) {
                    return Json(new { resultado.IdCabControlEsterilizado, resultado.Observacion, resultado.TipoLinea, resultado.Turno, resultado.Fecha }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public JsonResult EliminarCabControl(int idCabecera)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                CABECERA_CONTROL_ESTERILIZACION_CONSERVAS poCabControl = new CABECERA_CONTROL_ESTERILIZACION_CONSERVAS()
                {
                    IdCabControlEsterilizado = idCabecera,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                object[] Respuesta= null;
                clsDEsterilizacionConserva = new clsDEsterilizacionConserva();
                Respuesta = clsDEsterilizacionConserva.InactivarCabEsterilizacionConserva(poCabControl);
                return Json(Respuesta, JsonRequestBehavior.AllowGet);


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
        public JsonResult EliminarDetalleControl(int idDetalle)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                DETALLE_CONTROL_ESTERILIZACION_CONSERVA poDetControl = new DETALLE_CONTROL_ESTERILIZACION_CONSERVA()
                {
                    IdDetalleControlEsterilizacionConserva = idDetalle,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                object[] Respuesta = null;
                clsDEsterilizacionConserva = new clsDEsterilizacionConserva();
                Respuesta = clsDEsterilizacionConserva.InactivarDetalleEsterilizacionConserva(poDetControl);
                return Json(Respuesta, JsonRequestBehavior.AllowGet);


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
        [HttpGet]
        public ActionResult PartialCocheAutoclave(DateTime Fecha, string Turno,int CabControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDCcocheAutoclave = new clsDCcocheAutoclave();
                // clsDEmpleado = new clsDEmpleado();
                // var Empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                List<spConsultaCocheAutoclaveEsterilizacion> model = clsDCcocheAutoclave.ConsultaCocheAutoclaveEsterilizacion(Fecha, Turno,CabControl);
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
        public JsonResult GuardarModificarDetalleControl(DETALLE_CONTROL_ESTERILIZACION_CONSERVA poDetalleControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                poDetalleControl.FechaIngresoLog = DateTime.Now;
                poDetalleControl.UsuarioIngresoLog = lsUsuario[0];
                poDetalleControl.TerminalIngresoLog = Request.UserHostAddress;
                poDetalleControl.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                foreach (var item in poDetalleControl.TIPO_ESTERILIZACION_CONSERVA)
                {
                    item.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    item.FechaIngresoLog = DateTime.Now;
                    item.TerminalIngresoLog= Request.UserHostAddress;
                    item.UsuarioIngresoLog= lsUsuario[0];
                }
                object[] resultado = null;
                clsDEsterilizacionConserva = new clsDEsterilizacionConserva();
                if (poDetalleControl.IdDetalleControlEsterilizacionConserva == 0)
                {
                    resultado = clsDEsterilizacionConserva.GuardarDetalleEsterilizacion(poDetalleControl);

                }
                else
                {
                    resultado = clsDEsterilizacionConserva.ModificarDetalleEsterilizacion(poDetalleControl);

                }
               

                //clsDControlConsumoInsumo = new clsDControlConsumoInsumo();
                //string resultado = clsDControlConsumoInsumo.GuardarPallet(pallet);
                return Json(resultado, JsonRequestBehavior.AllowGet);
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
        public JsonResult ConsultarTipoEsterilizacion(int idDetalle, string Tipo)
        {
            //ConsultarTiposEsterilizacion
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
             
           
                clsDEsterilizacionConserva = new clsDEsterilizacionConserva();
                var resultado = clsDEsterilizacionConserva.ConsultarTiposEsterilizacion(idDetalle);
                decimal? Inicio=null; decimal? Medio=null; decimal? Final=null;
                if (Tipo=="Panel")
                {
                    Inicio = (from t in resultado
                              where t.Tipo == clsAtributos.Inicio
                              select t.Panel).FirstOrDefault();
                    Medio = (from t in resultado
                             where t.Tipo == clsAtributos.Medio
                             select t.Panel).FirstOrDefault();
                    Final = (from t in resultado
                             where t.Tipo == clsAtributos.Final
                             select t.Panel).FirstOrDefault();
                }
                if (Tipo == "Chart")
                {
                    Inicio = (from t in resultado
                              where t.Tipo == clsAtributos.Inicio
                              select t.Chart).FirstOrDefault();
                    Medio = (from t in resultado
                             where t.Tipo == clsAtributos.Medio
                             select t.Chart).FirstOrDefault();
                    Final = (from t in resultado
                             where t.Tipo == clsAtributos.Final
                             select t.Chart).FirstOrDefault();
                }
                if (Tipo == "Termometro")
                {
                    Inicio = (from t in resultado
                              where t.Tipo == clsAtributos.Inicio
                              select t.TermometroDigital).FirstOrDefault();
                    Medio = (from t in resultado
                             where t.Tipo == clsAtributos.Medio
                             select t.TermometroDigital).FirstOrDefault();
                    Final = (from t in resultado
                             where t.Tipo == clsAtributos.Final
                             select t.TermometroDigital).FirstOrDefault();
                }
                if (Tipo == "Presion")
                {
                    Inicio = (from t in resultado
                              where t.Tipo == clsAtributos.Inicio
                              select t.PresionManometro).FirstOrDefault();
                    Medio = (from t in resultado
                             where t.Tipo == clsAtributos.Medio
                             select t.PresionManometro).FirstOrDefault();
                    Final = (from t in resultado
                             where t.Tipo == clsAtributos.Final
                             select t.PresionManometro).FirstOrDefault();
                }
                if(Tipo== "HoraChequeo")
                {
                    DateTime? HInicio= (from t in resultado
                                      where t.Tipo == clsAtributos.Inicio
                                      select t.HoraChequeo).FirstOrDefault();
                    DateTime? HMedio = (from t in resultado
                                         where t.Tipo == clsAtributos.Medio
                                         select t.HoraChequeo).FirstOrDefault();
                    DateTime? HFinal = (from t in resultado
                                         where t.Tipo == clsAtributos.Final
                                         select t.HoraChequeo).FirstOrDefault();
                    return Json(new {Inicio=HInicio,Medio=HMedio,Final=HFinal }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var respuesta = new { Inicio, Medio, Final };

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
        public JsonResult ConsultarTipoEsterilizacionTodos(int idDetalle)
        {
            //ConsultarTiposEsterilizacion
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }


                clsDEsterilizacionConserva = new clsDEsterilizacionConserva();
                var resultado = clsDEsterilizacionConserva.ConsultarTiposEsterilizacion(idDetalle);
                var respuesta = (from t in resultado
                                 select new { t.Tipo, t.Panel, t.Chart, t.TermometroDigital, t.PresionManometro, t.HoraChequeo }).ToList();
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
        public JsonResult ConsultarCochesDet(int IdCabCoche)
        {
            //ConsultarTiposEsterilizacion
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDCcocheAutoclave = new clsDCcocheAutoclave();
                var respuesta = clsDCcocheAutoclave.ConsultaCocheAutoclaveDetalle(IdCabCoche);
                if (!respuesta.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
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
        public ActionResult PartialDetalleControl(int idCabecera)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                List<DetalleEsterilizacionConservaVieModel> resultado;
                clsDEsterilizacionConserva = new clsDEsterilizacionConserva();
                resultado = clsDEsterilizacionConserva.ConsultarDetalleEsterilizacion(idCabecera);
                if (resultado.Count==0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                return PartialView(resultado);
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
    }
}