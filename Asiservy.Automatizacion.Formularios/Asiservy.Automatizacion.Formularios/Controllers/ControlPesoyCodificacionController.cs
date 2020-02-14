using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ControlPesoyCodificacionLomosyMigas;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.Models.ControlPesoyCodificacion;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ControlPesoyCodificacionController : Controller
    {
        string[] lsUsuario;
        clsDError clsDError = null;
        //clsDGeneral clsDGeneral = null;
        //clsDEmpleado clsDEmpleado = null;
        clsDClasificador clsDClasificador = null;
        clsDApiOrdenFabricacion clsDApiOrdenFabricacion = null;
        //clsDLogin clsDLogin = null;
        ClsdControlPesoCodificacionLomosyMigas ClsdControlPesoCodificacionLomosyMigas = null;
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
        // GET: ControlPesoyCodificacion
        [Authorize]
        public ActionResult ControlPesoCodificacionLomosyMigas()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                return View();
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return View();
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return View();
            }
        }
        [HttpPost]
        public JsonResult GuardarCabeceraControl(CABECERA_CONTROL_PESO_CODIFICACION CabeceraControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                CabeceraControl.FechaCreacionLog = DateTime.Now;
                CabeceraControl.UsuarioCreacionLog = lsUsuario[0];
                CabeceraControl.TerminalCreacionLog = Request.UserHostAddress;
                CabeceraControl.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado;
                ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                if (CabeceraControl.IdCabeceraControlPesoYCodificacion==0)
                    resultado = ClsdControlPesoCodificacionLomosyMigas.GuardarCabeceraControl(CabeceraControl);
                else
                    resultado = ClsdControlPesoCodificacionLomosyMigas.ActualizarCabeceraControl(CabeceraControl);
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

        public JsonResult ConsultarCabeceraControl(CABECERA_CONTROL_PESO_CODIFICACION CabeceraControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
             
                ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                var resultado = ClsdControlPesoCodificacionLomosyMigas.ConsultarCabControl(CabeceraControl);
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
        public JsonResult GuardarHorasControl(DETALLE_CONTROL_PESO_CODIFICACION HoraControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                HoraControl.FechaCreacionLog = DateTime.Now;
                HoraControl.UsuarioCreacionLog = lsUsuario[0];
                HoraControl.TerminalCreacionLog = Request.UserHostAddress;
                HoraControl.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado;
                ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                if (HoraControl.IdDetalleControlPeso==0)
                {
                     resultado = ClsdControlPesoCodificacionLomosyMigas.GuardarHorasControl(HoraControl);
                }
                else
                {
                     resultado = ClsdControlPesoCodificacionLomosyMigas.ActualizarHorasControl(HoraControl);
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
        public JsonResult GuardarMuestrasPorHora(DETALLE_HORAS_CONTROL_PESO_CODIFICACION Muestra)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                Muestra.FechaCreacionLog = DateTime.Now;
                Muestra.UsuarioCreacionLog = lsUsuario[0];
                Muestra.TerminalCreacionLog = Request.UserHostAddress;
                Muestra.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado;
                ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                if (Muestra.IdDetalleHorasControlPesoCodificacion == 0)
                {
                    resultado = ClsdControlPesoCodificacionLomosyMigas.GuardarMuestrasPorHora(Muestra);
                }
                else
                {
                    resultado = ClsdControlPesoCodificacionLomosyMigas.ActualizarMuestrasPorHora(Muestra);
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
        public ActionResult CargarPartialControlDetalles(int? IdCabeceraCOntrol,DateTime Fecha)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDClasificador = new clsDClasificador();
                
                ViewBag.Usos= clsDClasificador.ConsultarClasificador("032");
                //clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                //dynamic result = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaProduccion(Fecha);
                //List<OrdenFabricacion> Listado = new List<OrdenFabricacion>();
                //foreach (var x in result)
                //{
                //    Listado.Add(new OrdenFabricacion { Orden = x.OrdenFabricacion });
                //}
                //ViewBag.Ordenesfab = Listado;
                //clsDControlConsumoInsumo = new clsDControlConsumoInsumo();

                //var model = clsDControlConsumoInsumo.ConsultaControlConsumoInsumo(Fecha, LineaNegocio, Turno);
                //if (!model.Any())
                //{
                //    return Json("0", JsonRequestBehavior.AllowGet);
                //}
                return PartialView();
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
        public ActionResult CargarHorasControl(int IdCabeceraCOntrol)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsdControlPesoCodificacionLomosyMigas ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                List<DETALLE_CONTROL_PESO_CODIFICACION> respuesta = ClsdControlPesoCodificacionLomosyMigas.ConsultarHorasControl(IdCabeceraCOntrol).OrderByDescending(x=>x.IdDetalleControlPeso).ToList();
                if (respuesta.Count==0)
                {
                    respuesta = null;
                }
                //clsDControlConsumoInsumo = new clsDControlConsumoInsumo();

                //var model = clsDControlConsumoInsumo.ConsultaControlConsumoInsumo(Fecha, LineaNegocio, Turno);
                //if (!model.Any())
                //{
                //    return Json("0", JsonRequestBehavior.AllowGet);
                //}
                return PartialView(respuesta);
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

        public ActionResult PartialConsultarMuestrasPorHora(int IdDetalleControlPesoCodificacion)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsdControlPesoCodificacionLomosyMigas ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                List<DETALLE_HORAS_CONTROL_PESO_CODIFICACION> respuesta = ClsdControlPesoCodificacionLomosyMigas.ConsultarMuestrasPorHora(IdDetalleControlPesoCodificacion).OrderByDescending(x => x.IdDetalleControlPesoCodificacion).ToList();
                if (respuesta.Count == 0)
                {
                    respuesta = null;
                }
                //clsDControlConsumoInsumo = new clsDControlConsumoInsumo();

                //var model = clsDControlConsumoInsumo.ConsultaControlConsumoInsumo(Fecha, LineaNegocio, Turno);
                //if (!model.Any())
                //{
                //    return Json("0", JsonRequestBehavior.AllowGet);
                //}
                return PartialView(respuesta);
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
        public ActionResult PartialConsultarUsos(int IdCabeceraControlPesoCodificacion)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsdControlPesoCodificacionLomosyMigas ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                List<ControlUsosViewModel> respuesta = ClsdControlPesoCodificacionLomosyMigas.ConsultarUsosControl(IdCabeceraControlPesoCodificacion).OrderByDescending(x => x.IdDescripcionUso).ToList();
                if (respuesta.Count == 0)
                {
                    respuesta = null;
                }
                //clsDControlConsumoInsumo = new clsDControlConsumoInsumo();

                //var model = clsDControlConsumoInsumo.ConsultaControlConsumoInsumo(Fecha, LineaNegocio, Turno);
                //if (!model.Any())
                //{
                //    return Json("0", JsonRequestBehavior.AllowGet);
                //}
                return PartialView(respuesta);
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
        public JsonResult InactivarHora(DETALLE_CONTROL_PESO_CODIFICACION ControlHora)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ControlHora.FechaCreacionLog = DateTime.Now;
                ControlHora.UsuarioCreacionLog = lsUsuario[0];
                ControlHora.TerminalCreacionLog = Request.UserHostAddress;
                ControlHora.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado;
                ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                resultado = ClsdControlPesoCodificacionLomosyMigas.InactivarHora(ControlHora);
               

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
        public JsonResult InactivarMuestra(DETALLE_HORAS_CONTROL_PESO_CODIFICACION Muestra)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                Muestra.FechaCreacionLog = DateTime.Now;
                Muestra.UsuarioCreacionLog = lsUsuario[0];
                Muestra.TerminalCreacionLog = Request.UserHostAddress;
                Muestra.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado;
                ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                resultado = ClsdControlPesoCodificacionLomosyMigas.InactivarMuestra(Muestra);


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
        public JsonResult GuardarUso(DETALLE_USO_CONTROL_PESO_CODIFICACION UsoCOntrol)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                UsoCOntrol.FechaCreacionLog = DateTime.Now;
                UsoCOntrol.UsuarioCreacionLog = lsUsuario[0];
                UsoCOntrol.TerminalCreacionLog = Request.UserHostAddress;
                UsoCOntrol.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado;
                ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                if (UsoCOntrol.IdDescripcionUso == 0)
                {
                    resultado = ClsdControlPesoCodificacionLomosyMigas.GuardarUsoControl(UsoCOntrol);
                }
                else
                {
                    resultado = ClsdControlPesoCodificacionLomosyMigas.ActualizarUsosControl(UsoCOntrol);
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
        public JsonResult InactivarUso(DETALLE_USO_CONTROL_PESO_CODIFICACION ControlUso)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ControlUso.FechaCreacionLog = DateTime.Now;
                ControlUso.UsuarioCreacionLog = lsUsuario[0];
                ControlUso.TerminalCreacionLog = Request.UserHostAddress;
                ControlUso.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado;
                ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                resultado = ClsdControlPesoCodificacionLomosyMigas.InactivarUso(ControlUso);


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
        public JsonResult GuardarLote(DETALLE_LOTE_CONTROL_PESO_CODIFICACION LoteControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                LoteControl.FechaCreacionLog = DateTime.Now;
                LoteControl.UsuarioCreacionLog = lsUsuario[0];
                LoteControl.TerminalCreacionLog = Request.UserHostAddress;
                LoteControl.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado;
                ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                if (LoteControl.IdDetalleLote == 0)
                {
                    resultado = ClsdControlPesoCodificacionLomosyMigas.GuardarLoteControl(LoteControl);
                }
                else
                {
                    resultado = ClsdControlPesoCodificacionLomosyMigas.ActualizarLoteControl(LoteControl);
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
        public ActionResult PartialConsultarLotes(int IdCabeceraControlPesoCodificacion)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                List<DETALLE_LOTE_CONTROL_PESO_CODIFICACION> respuesta = ClsdControlPesoCodificacionLomosyMigas.ConsultarLotesControl(IdCabeceraControlPesoCodificacion).OrderByDescending(x => x.IdDetalleLote).ToList();
                if (respuesta.Count == 0)
                {
                    respuesta = null;
                }
                //clsDControlConsumoInsumo = new clsDControlConsumoInsumo();

                //var model = clsDControlConsumoInsumo.ConsultaControlConsumoInsumo(Fecha, LineaNegocio, Turno);
                //if (!model.Any())
                //{
                //    return Json("0", JsonRequestBehavior.AllowGet);
                //}
                return PartialView(respuesta);
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
        public JsonResult InactivarLote(DETALLE_LOTE_CONTROL_PESO_CODIFICACION ControlLote)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ControlLote.FechaCreacionLog = DateTime.Now;
                ControlLote.UsuarioCreacionLog = lsUsuario[0];
                ControlLote.TerminalCreacionLog = Request.UserHostAddress;
                ControlLote.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado;
                ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                resultado = ClsdControlPesoCodificacionLomosyMigas.InactivarLote(ControlLote);


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
        public JsonResult InactivarControl(CABECERA_CONTROL_PESO_CODIFICACION CabControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                CabControl.FechaCreacionLog = DateTime.Now;
                CabControl.UsuarioCreacionLog = lsUsuario[0];
                CabControl.TerminalCreacionLog = Request.UserHostAddress;
                CabControl.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado;
                ClsdControlPesoCodificacionLomosyMigas = new ClsdControlPesoCodificacionLomosyMigas();
                resultado = ClsdControlPesoCodificacionLomosyMigas.InactivarCabControl(CabControl);


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
    }
}