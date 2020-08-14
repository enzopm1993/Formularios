using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ControlCocinamiento;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity.Validation;
using System.Net;
using System;
using System.Linq;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte;
using System.Collections.Generic;
using System.Web;
using System.IO;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.Mantenimientos;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class TomaMuestra
    {
        public string Nombre { get; set; } = null;
        public string Id { get; set; } = null;
    }
    public class ControlCocinamientoController : Controller
    {
        ClsDMantenimientoPCC ClsDMantenimientoPCC { get; set; } = null;
        TomaMuestra TomaMuestra { get; set; } = null;
         clsDPeriodo clsDPeriodo { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDLogin clsDLogin { get; set; } = null;
        public clsDReporte ClsDReporte { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        ClsDControlCocinamiento ClsDControlCocinamiento { get; set; } = null;
        clsDApiProduccion clsDApiProduccion { get; set; } = null;
        string[] lsUsuario { get; set; } = null;
        List<Temperatura> listaTemperatura { get; set; }=null;
        Temperatura temperatura { get; set; }=null;
        ListaTemperatura subDetalle { get; set; }=null;
        List<ListaTemperatura> listObjTemp { get; set; }=null;
        List<ListaImagenes> listaObjImagenes { get; set; } = null;
        ListaImagenes objImagenes { get; set; } = null;
        #region CONTROL
        [Authorize]
        public ActionResult ControlCocinamiento()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.Apexcharts = "1";
                ViewBag.JqueryRotate = "1";
                ViewBag.select2 = "1";
                ViewBag.MascaraInput = "1";
                ViewBag.DxDevWeb = "1";
                TomaMuestra TomaMuestra;
                List<TomaMuestra> listaTomaMuestra = new List<TomaMuestra>();
                int con = 0;
                foreach (var item in clsAtributos.TomaMuestraCocinamiento)
                {
                    con++;
                    TomaMuestra = new TomaMuestra();
                    TomaMuestra.Id = con.ToString();
                    TomaMuestra.Nombre = item;
                    listaTomaMuestra.Add(TomaMuestra);
                }
                ViewBag.TomaMuestra = listaTomaMuestra;
                ClsDMantenimientoPCC = new ClsDMantenimientoPCC();
                ViewBag.PCC = ClsDMantenimientoPCC.ConsultarRegistroActivos();
                clsDLogin = new clsDLogin();
                var usuarioOpcion = clsDLogin.ValidarPermisoOpcion(lsUsuario[1], "ReporteAnalisisQuimico");
                if (usuarioOpcion)
                {
                    ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ReporteAnalisisQuimico";
                }

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
        public JsonResult JsonControlCocinamiento(DateTime fechaAsignada, DateTime fechaProduccion, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDControlCocinamiento = new ClsDControlCocinamiento();
                clsDApiProduccion = new clsDApiProduccion();
                var barcosAsignados = ClsDControlCocinamiento.ConsultarBarcoFecha(fechaAsignada);
                var listaParadasCocinas = clsDApiProduccion.ParadasCocinasPorFecha(fechaAsignada);
                var listaDetalleDia = ClsDControlCocinamiento.ConsultarDetDiaSinImagenes(fechaProduccion, op);
                if (listaParadasCocinas != null)
                {
                    listaTemperatura = new List<Temperatura>();
                    foreach (var item in listaParadasCocinas)
                    {
                        var barco = (from x in barcosAsignados
                                     where x.Lote == item.LOTE && x.OrdenFabricacion == item.ORDEN.ToString() && x.Nombre != item.BARCO
                                     select x.Nombre).FirstOrDefault();
                        temperatura = new Temperatura();
                        temperatura.BarcoAsignado = barco;
                        temperatura.Lote = item.LOTE;
                        temperatura.OrdenFabricacion = item.ORDEN;
                        temperatura.Parada = item.PARADA;
                        temperatura.Talla = item.TALLA;
                        temperatura.Coche = item.COCHES;
                        temperatura.Barco = item.BARCO;
                        temperatura.Especie = item.ESPECIE;
                        temperatura.Cocina = item.COCINA;
                        listObjTemp = new List<ListaTemperatura>();
                        foreach (var subItem in listaDetalleDia)
                        {
                            subDetalle = new ListaTemperatura();
                            if (item.LOTE==subItem.Lote && item.ORDEN==subItem.OrdenFabricacion) {
                                temperatura.IdCocinamientoCtrl = subItem.IdCocinamientoCtrl;
                                temperatura.IdCocinamientoDet = subItem.IdCocinamientoDet;
                                subDetalle.IdCocinamientoSubDet = subItem.IdCocinamientoSubDet;
                                subDetalle.IdCocinamientoSubDet = subItem.IdCocinamientoSubDet;
                                subDetalle.NumCoche = subItem.NumCoche;
                                subDetalle.Temperatura = subItem.Temperatura;
                                subDetalle.TomaMuestra = subItem.TomaMuestra;
                                listObjTemp.Add(subDetalle);
                            }
                        }
                        temperatura.listaTemperatura = listObjTemp;
                        listaTemperatura.Add(temperatura);
                    }
                    return Json(listaTemperatura, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
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
        public JsonResult ConsultarEstadoReporte(int idCocinamientoCtrl = 0)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDControlCocinamiento = new ClsDControlCocinamiento();
                var lista = ClsDControlCocinamiento.ConsultarEstadoReporte(idCocinamientoCtrl, DateTime.MinValue);
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
        public JsonResult ConsultarCabecera(DateTime fechaProduccion)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDControlCocinamiento = new ClsDControlCocinamiento();
                var lista = ClsDControlCocinamiento.ConsultarCabecera(fechaProduccion);
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
        public JsonResult GuardarModificarCocinamiento(CC_COCINAMIENTO_CTRL model, int siAprobar)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(model.FechaProduccion);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                ClsDControlCocinamiento = new ClsDControlCocinamiento();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                if (model.IdCocinamientoCtrl != 0 && siAprobar == 0)
                {
                    var estadoReporte = ClsDControlCocinamiento.ConsultarEstadoReporte(model.IdCocinamientoCtrl, DateTime.MinValue);
                    if (estadoReporte.EstadoReporte)
                    {
                        return Json("4", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                    }
                }
                var valor = ClsDControlCocinamiento.GuardarModificarCocinamiento(model, siAprobar);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 1)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 2) { return Json("2", JsonRequestBehavior.AllowGet); }
                else if (valor == 3) return Json("3", JsonRequestBehavior.AllowGet);//ERROR DE FECHA
                else return Json("5", JsonRequestBehavior.AllowGet);
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
        public JsonResult EliminarCocinamiento(CC_COCINAMIENTO_CTRL model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(model.FechaProduccion);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                ClsDControlCocinamiento = new ClsDControlCocinamiento();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                var estadoReporte = ClsDControlCocinamiento.ConsultarEstadoReporte(model.IdCocinamientoCtrl, DateTime.MinValue);
                if (!estadoReporte.EstadoReporte)
                {
                    var valor = ClsDControlCocinamiento.EliminarCocinamiento(model);
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
        public JsonResult GuardarModificarSubDetalle(CC_COCINAMIENTO_SUBDET subDetalle, int IdCocinamientoCtrl, CC_COCINAMIENTO_DET detalle)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDControlCocinamiento = new ClsDControlCocinamiento();
                var estadoReporte = ClsDControlCocinamiento.ConsultarEstadoReporte(detalle.IdCocinamientoCtrl, DateTime.MinValue);
                if (estadoReporte == null)
                {
                    return Json("10", JsonRequestBehavior.AllowGet);//IdCocinamientoCtrl NO ENCONTRADO
                }
                if (estadoReporte.EstadoReporte)
                {
                    return Json("3", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.FechaProduccion);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                var existeDetalle = ClsDControlCocinamiento.ConsultarDetalleExiste(subDetalle.IdCocinamientoDet);
                if (existeDetalle == null)
                {
                    detalle.FechaIngresoLog = DateTime.Now;
                    detalle.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    detalle.TerminalIngresoLog = Request.UserHostAddress;
                    detalle.UsuarioIngresoLog = lsUsuario[0];
                    ClsDControlCocinamiento.GuardarModificarDetalle(detalle);
                    subDetalle.IdCocinamientoDet = detalle.IdCocinamientoDet;
                }
                var valor = 0;
                subDetalle.FechaIngresoLog = DateTime.Now;
                subDetalle.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                subDetalle.TerminalIngresoLog = Request.UserHostAddress;
                subDetalle.UsuarioIngresoLog = lsUsuario[0];
                valor = ClsDControlCocinamiento.GuardarModificarSubDetalle(subDetalle);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
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
        public JsonResult EliminarSubDetalle(CC_COCINAMIENTO_SUBDET subDetalle, int idCocinamientoCtrl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDControlCocinamiento = new ClsDControlCocinamiento();
                var estadoReporte = ClsDControlCocinamiento.ConsultarEstadoReporte(idCocinamientoCtrl, DateTime.MinValue);
                if (estadoReporte == null)
                {
                    return Json("3", JsonRequestBehavior.AllowGet);//IdCocinamientoCtrl NO ENCONTRADO
                }
                if (estadoReporte.EstadoReporte)
                {
                    return Json("2", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.FechaProduccion);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                var valor = 0;
                subDetalle.FechaIngresoLog = DateTime.Now;
                subDetalle.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                subDetalle.TerminalIngresoLog = Request.UserHostAddress;
                subDetalle.UsuarioIngresoLog = lsUsuario[0];
                valor = ClsDControlCocinamiento.EliminarSubDetalle(subDetalle);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
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
        public JsonResult JsonSubDetalle(int idCocinamientoCtrl, int idCocinamientoDet = 0)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDControlCocinamiento = new ClsDControlCocinamiento();                
                var listaSubDetalle = ClsDControlCocinamiento.ConsultarSubDetalle(idCocinamientoCtrl, idCocinamientoDet);
                if (listaSubDetalle.Any())
                {
                    return Json(listaSubDetalle, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
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
        public JsonResult JsonCargarDetalle(int idCocinamientoCtrl, string lote, int ordenFabricacion)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDControlCocinamiento = new ClsDControlCocinamiento();
                var objDetalle = ClsDControlCocinamiento.ConsultarDetalle(idCocinamientoCtrl, lote, ordenFabricacion);
                if (objDetalle!=null)
                {
                    return Json(objDetalle, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
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
        public JsonResult GuardarImagen(CC_COCINAMIENTO_IMAGEN model, HttpPostedFileBase dataImg, CC_COCINAMIENTO_DET detalle)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                ClsDControlCocinamiento = new ClsDControlCocinamiento();
                var estadoReporte = ClsDControlCocinamiento.ConsultarEstadoReporte(detalle.IdCocinamientoCtrl, DateTime.MinValue);
                if (estadoReporte.EstadoReporte)
                {
                    return Json("3", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.FechaProduccion);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                string path = string.Empty;
                string NombreImg = string.Empty;
                if (dataImg != null)
                {
                    decimal mb = 1024 * 1024 * 5;//bytes to Mb; max 5Mb
                    var supportedTypes = new[] { "jpg", "jpeg", "PNG", "png" };
                    var fileExt = Path.GetExtension(dataImg.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        return Json("4", JsonRequestBehavior.AllowGet);//NO ES IMG
                    }
                    else if (dataImg.ContentLength > (mb))
                    {
                        return Json(dataImg.ContentLength, JsonRequestBehavior.AllowGet);//SOBREPASA EL LIMITE PERMITIDO dataImg.ContentLength=bytes convert to Mb
                    }
                    path = Server.MapPath(clsAtributos.UrlImagen + this.ControllerContext.RouteData.Values["controller"].ToString() + "/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var date = DateTime.Now;
                    long n = long.Parse(date.ToString("yyyyMMddHHmmss"));
                    var ext2 = dataImg.FileName.Split('.');
                    var cont = ext2.Length;
                    NombreImg = "fotoLaboratorio" + n.ToString() + "." + ext2[cont - 1];
                    model.RutaImagen = NombreImg;
                }
                var existeDetalle = ClsDControlCocinamiento.ConsultarDetalleExiste(model.IdCocinamientoDet);
                if (existeDetalle == null)
                {
                    detalle.FechaIngresoLog = DateTime.Now;
                    detalle.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    detalle.TerminalIngresoLog = Request.UserHostAddress;
                    detalle.UsuarioIngresoLog = lsUsuario[0];
                    ClsDControlCocinamiento.GuardarModificarDetalle(detalle);
                    model.IdCocinamientoDet = detalle.IdCocinamientoDet;
                }
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                var valor = ClsDControlCocinamiento.GuardarModificarFoto(model);
                if (dataImg != null)
                {
                    dataImg.SaveAs(path + Path.GetFileName(NombreImg));
                }
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
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
        public ActionResult VerCrearImagenPartial(int idCocinamientoDet)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDControlCocinamiento = new ClsDControlCocinamiento();
                var lista = ClsDControlCocinamiento.ConsultarImagen(idCocinamientoDet);
                if (lista.Count != 0)
                {
                    ViewBag.Path = clsAtributos.UrlImagen.Replace("~", "..") + this.ControllerContext.RouteData.Values["controller"].ToString() + "/";
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
        public JsonResult EliminarImagen(CC_COCINAMIENTO_IMAGEN model, int idCocinamientoCtrl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDControlCocinamiento = new ClsDControlCocinamiento();
                var estadoReporte = ClsDControlCocinamiento.ConsultarEstadoReporte(idCocinamientoCtrl, DateTime.MinValue);
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.FechaProduccion);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                if (estadoReporte.EstadoReporte)
                {
                    return Json("2", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                }
                var valor = 0;
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                valor = ClsDControlCocinamiento.EliminarImagen(model);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
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
        #endregion

        #region BANDEJA
        [Authorize]
        public ActionResult BandejaCocinamiento()
        {
            try
            {
                ViewBag.DxDevWeb = "1";
                ViewBag.Apexcharts = "1";
                ViewBag.dataTableJS = "1";
                ViewBag.DateRangePicker = "1";
                ViewBag.JqueryRotate = "1";
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

        public ActionResult BandejaCocinamientoPartial(bool estadoReporte, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDControlCocinamiento = new ClsDControlCocinamiento();
                ClsDMantenimientoPCC = new ClsDMantenimientoPCC();
                ViewBag.PCC = ClsDMantenimientoPCC.ConsultarRegistroActivos();
                var lista = ClsDControlCocinamiento.ConsultarBadejaEstado(fechaDesde, fechaHasta, estadoReporte);
                clsDClasificador = new clsDClasificador();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();
                if (poTurno != null)
                {
                    ViewBag.Turno = poTurno;
                }
                if (lista.Any())
                {
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
        public JsonResult JsonBandejaCocinamientoAprobacion(DateTime fechaProduccion, DateTime fechaAsignada, int op)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(fechaProduccion);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                ClsDControlCocinamiento = new ClsDControlCocinamiento();
                clsDApiProduccion = new clsDApiProduccion();
                var listaParadasCocinas = clsDApiProduccion.ParadasCocinasPorFecha(fechaAsignada);
                
                var barcosAsignados = ClsDControlCocinamiento.ConsultarBarcoFecha(fechaAsignada);
                var listaDetalleDia = ClsDControlCocinamiento.ConsultarDetalleDia(fechaProduccion, op);
                if (listaDetalleDia.Count != 0)
                {
                    string path= clsAtributos.UrlImagen.Replace("~", "..") + this.ControllerContext.RouteData.Values["controller"].ToString() + "/";
                    listaTemperatura = new List<Temperatura>();
                    var listaGroupSubDet = (from x in listaDetalleDia
                                            where x.IdCocinamientoSubDet!=null
                                            group x by new { x.IdCocinamientoSubDet } into s                                            
                                            select new { s.Key.IdCocinamientoSubDet });
                    var listaGroupImages = (from x in listaDetalleDia
                                            where x.IdImagen!=null
                                            group x by new { x.IdImagen } into s
                                            select new { s.Key.IdImagen });
                    foreach (var item in listaParadasCocinas)
                    {
                        var barco = (from x in barcosAsignados
                                     where x.Lote == item.LOTE && x.OrdenFabricacion == item.ORDEN.ToString() && x.Nombre!=item.BARCO
                                     select x.Nombre).FirstOrDefault();                        
                        temperatura = new Temperatura();
                        temperatura.BarcoAsignado = barco;
                        temperatura.Lote = item.LOTE;
                        temperatura.OrdenFabricacion = item.ORDEN;
                        temperatura.Parada = item.PARADA;
                        temperatura.Talla = item.TALLA;
                        temperatura.Coche = item.COCHES;
                        temperatura.Barco = item.BARCO;
                        temperatura.Especie = item.ESPECIE;
                        temperatura.Cocina = item.COCINA;
                        listObjTemp = new List<ListaTemperatura>();
                        listaObjImagenes = new List<ListaImagenes>();

                        foreach (var subItem in listaGroupSubDet)
                        {
                            subDetalle = new ListaTemperatura();
                            var objSubDetalle = (from x in listaDetalleDia
                                                 where x.IdCocinamientoSubDet == subItem.IdCocinamientoSubDet
                                                 select x).FirstOrDefault();
                            if (item.LOTE == objSubDetalle.Lote && item.ORDEN == objSubDetalle.OrdenFabricacion)
                            {
                                temperatura.IdCocinamientoCtrl = objSubDetalle.IdCocinamientoCtrl;
                                temperatura.IdCocinamientoDet = objSubDetalle.IdCocinamientoDet;

                                subDetalle.IdCocinamientoSubDet = objSubDetalle.IdCocinamientoSubDet;
                                subDetalle.NumCoche = objSubDetalle.NumCoche;
                                subDetalle.Temperatura = objSubDetalle.Temperatura;
                                subDetalle.TomaMuestra = objSubDetalle.TomaMuestra;
                                listObjTemp.Add(subDetalle);
                            }
                        }
                        foreach (var subItem in listaGroupImages)
                        {
                            objImagenes = new ListaImagenes();
                            var objImg = (from x in listaDetalleDia
                                          where x.IdImagen == subItem.IdImagen
                                          select x).FirstOrDefault();
                            if (item.LOTE == objImg.Lote && item.ORDEN == objImg.OrdenFabricacion)
                            {
                                temperatura.IdCocinamientoCtrl = objImg.IdCocinamientoCtrl;
                                temperatura.IdCocinamientoDet = objImg.IdCocinamientoDet;

                                objImagenes.IdImagen = objImg.IdImagen;
                                objImagenes.ObservacionImagen = objImg.ObservacionImagen;
                                objImagenes.RutaImagen = path + objImg.RutaImagen;
                                objImagenes.Rotation = objImg.Rotation;
                                listaObjImagenes.Add(objImagenes);
                            }
                        }
                        temperatura.listaTemperatura = listObjTemp;
                        temperatura.listaImagenes = listaObjImagenes;
                        listaTemperatura.Add(temperatura);
                    }
                    return Json(listaTemperatura, JsonRequestBehavior.AllowGet);
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