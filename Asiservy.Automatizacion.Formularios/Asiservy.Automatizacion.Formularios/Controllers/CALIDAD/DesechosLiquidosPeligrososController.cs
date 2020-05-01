using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ControlDesechosLiquidosPeligrosos;
using Rotativa;
using System.Data.Entity.Validation;
using System.Net;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class DesechosLiquidosPeligrososController : Controller
    {
        clsDError clsDError = null;
        clsDEmpleado clsDEmpleado = null;
        clsDDesechosLiquidosPeligrosos clsDDesechosLiquidosPeligrosos = null;
        string[] lsUsuario;
        public ActionResult DesechosLiquidosPeligrosos()
        {
            try
            {               
                ViewBag.dataTableJS = "1";
                ViewBag.FirmaPad = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
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

        public ActionResult DesechosLiquidosPeligrososPartial(int anioBusqueda, int mesBusqueda, int idDesechosLiquidos, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDDesechosLiquidosPeligrosos = new clsDDesechosLiquidosPeligrosos();
                var lista = clsDDesechosLiquidosPeligrosos.ConsultarDesechosLiquidos( anioBusqueda, mesBusqueda, idDesechosLiquidos, op);
                if (lista.Count() != 0)
                {
                    ViewBag.EstadoReporte = lista[0].EstadoReporte;
                    return PartialView(lista);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
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
        //-----------------------------------------------CONTROL---------------------------------------------------------------------
        public JsonResult ConsultarEstadoReporte(int anioBusqueda, int mesBusqueda, int idDesechosLiquidos, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDDesechosLiquidosPeligrosos = new clsDDesechosLiquidosPeligrosos();
                var lista = clsDDesechosLiquidosPeligrosos.ConsultarDesechosLiquidos(anioBusqueda, mesBusqueda, idDesechosLiquidos, op);
                
                if (lista != null)
                {
                    return Json(lista, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GuardarModificarDesechosLiquidos(CC_DESECHOS_LIQUIDOS_PELIGROSOS model, CC_DESECHOS_LIQUIDOS_PELIGROSOS_DETALLE modelDetalle, int siAprobar, int anioBusqueda, int mesBusqueda, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDDesechosLiquidosPeligrosos = new clsDDesechosLiquidosPeligrosos();
                //CUANDO EL USUARIO ABRE LA BANDEJA Y EL CONTROL, VALIDO SI EL ESTADO DEL REGISTRO ESTA EN TRUE NO LE PERMITO INGRESAR MAS REGISTROS
                var consultarEstadoReporte = clsDDesechosLiquidosPeligrosos.ConsultarDesechosLiquidos(anioBusqueda, mesBusqueda, model.IdDesechosLiquidos, op);
                bool estadoReporte = false;
                if (consultarEstadoReporte.Count != 0)
                {
                    estadoReporte = consultarEstadoReporte[0].EstadoReporte;
                    model.IdDesechosLiquidos = consultarEstadoReporte[0].IdDesechosLiquidos;
                    var existeDia = (from x in consultarEstadoReporte
                             where modelDetalle.FechaDIA == x.FechaDIA && modelDetalle.IdDesechosLiquidosDetalle== 0//VALIDO SI EXISTE UN DIA IGUAL AL QUE SE QUIERE INGRESAR && IdDesechosLiquidosDetalle  SE CONTROLA SI ES UN INGRESO NUEVO O ACTUALIZACIO
                                     select new {x.FechaDIA, x.EstadoReporte }).ToList();
                    if (existeDia.Count()!=0) {
                        return Json("3", JsonRequestBehavior.AllowGet);
                    }
                }
               
                if (estadoReporte == false)// EL REGISTRO SE ACTUALIZA SI NO ESTA APROBADO
                {
                    model.FechaIngresoLog = DateTime.Now;
                    model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    model.UsuarioIngresoLog = lsUsuario[0];
                    var valor = clsDDesechosLiquidosPeligrosos.GuardarModificarDesechosLiquidos(model, siAprobar);
                    modelDetalle.IdDesechosLiquidos = model.IdDesechosLiquidos;
                    var valorDetalle = GuardarModificarDesechosLiquidosDetalle(modelDetalle);//ENVIO A GUARDAR EL DETALLE
                    if (valor == 0)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else return Json("1", JsonRequestBehavior.AllowGet);
                }
                else return Json("2", JsonRequestBehavior.AllowGet);

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
        public JsonResult GuardarImagenFirma(string image, int idDesechosLiquidos)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                byte[] Firma = Convert.FromBase64String(image);
                CC_DESECHOS_LIQUIDOS_PELIGROSOS model = new CC_DESECHOS_LIQUIDOS_PELIGROSOS();
                clsDDesechosLiquidosPeligrosos = new clsDDesechosLiquidosPeligrosos();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.FirmaControl = Firma;
                model.IdDesechosLiquidos = idDesechosLiquidos;
                var valor = clsDDesechosLiquidosPeligrosos.GuardarImagenFirma(model, true);
                if (valor == 1)
                {
                    var imagenfirma = String.Format("data:image/png;base64,{0}", image);
                    return Json(imagenfirma, JsonRequestBehavior.AllowGet);
                }
                else return Json("0", JsonRequestBehavior.AllowGet);
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

        public JsonResult ConsultarImagenFirma(int idDesechosLiquidos)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDDesechosLiquidosPeligrosos = new clsDDesechosLiquidosPeligrosos();
                var lista = clsDDesechosLiquidosPeligrosos.ConsultarImagenFirma(idDesechosLiquidos, true);
                if (lista != null)
                {
                    if (lista[0].FirmaControl!=null)
                    {
                        var base64 = Convert.ToBase64String(lista[0].FirmaControl);
                        var imagenfirma = String.Format("data:image/png;base64,{0}", base64);
                        return Json(imagenfirma, JsonRequestBehavior.AllowGet);
                    } else return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GuardarModificarDesechosLiquidosDetalle(CC_DESECHOS_LIQUIDOS_PELIGROSOS_DETALLE model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor = clsDDesechosLiquidosPeligrosos.GuardarModificarDesechosLiquidosDetalle(model);

                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else return Json("1", JsonRequestBehavior.AllowGet);
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
        public JsonResult EliminarDesechosLiquidosDetalle(CC_DESECHOS_LIQUIDOS_PELIGROSOS_DETALLE model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDDesechosLiquidosPeligrosos = new clsDDesechosLiquidosPeligrosos();              
                    model.FechaIngresoLog = DateTime.Now;
                    model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    model.TerminalIngresoLog = Request.UserHostAddress;
                    model.UsuarioIngresoLog = lsUsuario[0];
                    var valor = clsDDesechosLiquidosPeligrosos.EliminarDesechosLiquidosDetalle(model);
                //VALIDO SI A LA CABECERA LE QUEDAN DETALLES CASO CONTRARIO LO ENVIO A ELIMINAR
                var consultarEstadoReporte = clsDDesechosLiquidosPeligrosos.ConsultarDesechosLiquidos(0, 0, model.IdDesechosLiquidos, 0);
                if (consultarEstadoReporte.Count()==0)
                {
                    CC_DESECHOS_LIQUIDOS_PELIGROSOS modelCab = new CC_DESECHOS_LIQUIDOS_PELIGROSOS();
                    modelCab.IdDesechosLiquidos = model.IdDesechosLiquidos;
                    modelCab.FechaIngresoLog = DateTime.Now;
                    modelCab.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    modelCab.TerminalIngresoLog = Request.UserHostAddress;
                    modelCab.UsuarioIngresoLog = lsUsuario[0];
                    var eliminarCabecera= clsDDesechosLiquidosPeligrosos.EliminarDesechosLiquidos(modelCab);
                }
                if (valor == 0)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    else return Json("1", JsonRequestBehavior.AllowGet);               
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
        //-----------------------------------------------BANDEJA---------------------------------------------------------------------
        public ActionResult BandejaDesechosLiquidosPeligrosos()
        {
            try
            {
                ViewBag.FirmaPad = "1";
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
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

        public ActionResult BandejaDesechosLiquidosPeligrososPartial(int anioBusqueda, int mesBusqueda, int idDesechosLiquidos, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDDesechosLiquidosPeligrosos = new clsDDesechosLiquidosPeligrosos();
                var tablaCabecera = clsDDesechosLiquidosPeligrosos.ConsultarDesechosLiquidos(anioBusqueda, mesBusqueda, idDesechosLiquidos, op);

                if (tablaCabecera.Count()!=0)
                {
                    return PartialView(tablaCabecera);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
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
        public JsonResult BandejaGuardarModificarDesechosLiquidos(CC_DESECHOS_LIQUIDOS_PELIGROSOS model, int siAprobar)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDDesechosLiquidosPeligrosos = new clsDDesechosLiquidosPeligrosos();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor = clsDDesechosLiquidosPeligrosos.GuardarModificarDesechosLiquidos(model, siAprobar);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else return Json("1", JsonRequestBehavior.AllowGet); ;
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

        public JsonResult BandejaConsultarDesechosLiquidosPeligrosos(int anioBusqueda, bool estadoReporte)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDDesechosLiquidosPeligrosos = new clsDDesechosLiquidosPeligrosos();
                var tablaCabecera = clsDDesechosLiquidosPeligrosos.ConsultarDesechosLiquidosCabecera(anioBusqueda, estadoReporte);
                if (tablaCabecera != null)
                {
                    return Json(tablaCabecera, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }
        //-----------------------------------------------REPORTE---------------------------------------------------------------------
        public ActionResult ReporteDesechosLiquidosPeligrosos()
        {
            try
            {
                ViewBag.FirmaPad = "1";
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
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

        public ActionResult ReporteDesechosLiquidosPeligrososPartial(int anioBusqueda, int mesBusqueda, int idDesechosLiquidos, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDDesechosLiquidosPeligrosos = new clsDDesechosLiquidosPeligrosos();
                var tablaCabecera = clsDDesechosLiquidosPeligrosos.ConsultarDesechosLiquidos(anioBusqueda, mesBusqueda, idDesechosLiquidos, op);
                if (tablaCabecera.Count() != 0)
                {
                    return PartialView(tablaCabecera);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
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
        public JsonResult BandejaGuardarImagenFirma(string image, int idDesechosLiquidos)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                byte[] Firma = Convert.FromBase64String(image);
                CC_DESECHOS_LIQUIDOS_PELIGROSOS model = new CC_DESECHOS_LIQUIDOS_PELIGROSOS();
                clsDDesechosLiquidosPeligrosos = new clsDDesechosLiquidosPeligrosos();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.FirmaAprobacion = Firma;
                model.IdDesechosLiquidos = idDesechosLiquidos;
                var valor = clsDDesechosLiquidosPeligrosos.GuardarImagenFirma(model, false);
                if (valor == 1)
                {
                    var imagenfirma = String.Format("data:image/png;base64,{0}", image);
                    return Json(imagenfirma, JsonRequestBehavior.AllowGet);
                }
                else return Json("0", JsonRequestBehavior.AllowGet);
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

        public JsonResult BandejaConsultarImagenFirma(int idDesechosLiquidos)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDDesechosLiquidosPeligrosos = new clsDDesechosLiquidosPeligrosos();
                var lista = clsDDesechosLiquidosPeligrosos.ConsultarImagenFirma(idDesechosLiquidos, false);
                if (lista != null)
                {
                    if (lista[0].FirmaAprobacion != null)
                    {
                        var base64 = Convert.ToBase64String(lista[0].FirmaAprobacion);
                        var imagenfirma = String.Format("data:image/png;base64,{0}", base64);
                        return Json(imagenfirma, JsonRequestBehavior.AllowGet);
                    }
                    else return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }


        //-----------------------------------------------IMPRESION PDF---------------------------------------------------------------------
        public ActionResult PrintReport(int anioBusqueda, int mesBusqueda, int idDesechosLiquidos, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    Response.Redirect(Url.Action("Login", "Login"));
                }
                clsDDesechosLiquidosPeligrosos = new clsDDesechosLiquidosPeligrosos();
                var tablaCabecera = clsDDesechosLiquidosPeligrosos.ConsultarDesechosLiquidos(anioBusqueda, mesBusqueda, idDesechosLiquidos, op);
                var headerPdf = Server.MapPath("~/Views/DesechosLiquidosPeligrosos/HeaderPdf.html");//ARCHIVO HTML USADO EN LA CABECERA DEL PDF
                ViewBag.filtroFechaDesde = anioBusqueda;
                ViewBag.filtroFechaHasta = mesBusqueda;
                string customSwitches = string.Format("--header-html  \"{0}\" " +
                            "--header-font-size \"15\" ", headerPdf);
                return new ViewAsPdf("PdfReporteDesechosLiquidosPeligrososPartial", tablaCabecera)
                {//METODO AL QUE SE HACE REFERENCIA ------------------, OBJETO 
                 // Establece la Cabecera y el Pie de página
                    CustomSwitches = customSwitches +
                    "--page-offset 0 --footer-center [page] --footer-font-size 10",
                    PageSize = Rotativa.Options.Size.A3,
                    PageMargins = new Rotativa.Options.Margins(25, 5, 10, 5),
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                };
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

        public ActionResult PdfReporteDesechosLiquidosPeligrososPartial(int anioBusqueda, int mesBusqueda, int idDesechosLiquidos, int op)
        {
            clsDDesechosLiquidosPeligrosos = new clsDDesechosLiquidosPeligrosos();
            var tablaCabecera = clsDDesechosLiquidosPeligrosos.ConsultarDesechosLiquidos(anioBusqueda, mesBusqueda, idDesechosLiquidos, op);
            if (tablaCabecera.Count() != 0)
            {
                return PartialView(tablaCabecera);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
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