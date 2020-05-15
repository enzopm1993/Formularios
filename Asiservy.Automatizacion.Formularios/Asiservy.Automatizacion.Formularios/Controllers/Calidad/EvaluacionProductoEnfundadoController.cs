using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.EvaluacionProductoEnfundado;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MantenimientoColor;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MantenimientoOlor;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.Mantenimientos;
using Asiservy.Automatizacion.Formularios.Models.CALIDAD;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class EvaluacionProductoEnfundadoController : Controller
    {
        string[] lsUsuario { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDEvaluacionProductoEnfundado clsDEvaluacionProductoEnfundado { get; set; } = null;
        clsDMantenimientoOlor clsDMantenimientoOlor { get; set; } = null;
        clsDMantenimientoTextura clsDMantenimientoTextura { get; set; } = null;
        clsDMantenimientoSabor clsDMantenimientoSabor { get; set; } = null;
        clsDMantenimientoProteina clsDMantenimientoProteina { get; set; } = null;
        clsDMantenimientoColor clsDMantenimientoColor { get; set; } = null;
        clsDEmpleado clsDEmpleado { get; set; } = null;
        // GET: EvaluacionProductoEnfundado
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        [Authorize]
        public ActionResult ControlEvaluacionProductoEnfundado()
        {
            try
            {
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.JqueryRotate = "1";
                ViewBag.MaskedInput = "1";
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                //ViewBag.FirmaPad = "1";
                clsDEmpleado = new clsDEmpleado();
                lsUsuario = User.Identity.Name.Split('_');
                clsDMantenimientoOlor = new clsDMantenimientoOlor();
                clsDMantenimientoTextura = new clsDMantenimientoTextura();
                clsDMantenimientoSabor = new clsDMantenimientoSabor();
                clsDMantenimientoProteina = new clsDMantenimientoProteina();
                clsDMantenimientoColor = new clsDMantenimientoColor();
                clsDClasificador = new clsDClasificador();
                var ListaTiposLimpieza = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoTipoLimpiezaPescado).OrderBy(x => x.Codigo);
                //var Lineas = clsDClasificador.ConsultarClasificador(clsAtributos.CodGrupoLineaProduccion).OrderBy(x => x.Codigo);
                ViewBag.Empacadores = new SelectList(clsDEmpleado.ConsultaEmpleadosFiltro(null, null, clsAtributos.CargoEmpacado),"CEDULA","NOMBRES");
                var Olor = clsDMantenimientoOlor.ConsultaManteminetoOlor().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                var Textura = clsDMantenimientoTextura.ConsultaManteminetoTextura().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                var Sabor = clsDMantenimientoSabor.ConsultaManteminetoSabor().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                var Proteina = clsDMantenimientoProteina.ConsultaManteminetoProteina().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                var Color = clsDMantenimientoColor.ConsultarMantenimientoColor().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                ViewBag.Olor = new SelectList(Olor, "IdOlor", "Descripcion");
                ViewBag.Textura = new SelectList(Textura, "IdTextura", "Descripcion");
                ViewBag.Sabor = new SelectList(Sabor, "IdSabor", "Descripcion");
                ViewBag.Color = new SelectList(Color, "IdColor", "Descripcion");
                ViewBag.Proteina = new SelectList(Proteina, "IdProteina", "Descripcion");
                ViewBag.NivelLimpieza = new SelectList(ListaTiposLimpieza, "Codigo", "Descripcion");
                //ViewBag.Lineas = new SelectList(Lineas, "Codigo", "Descripcion");
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
        public ActionResult ReporteEvaluacionProductoEnfundado()
        {
            try
            {

                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.dataTableJS = "1";
       
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
        public ActionResult PartialReporteEvaluacionProductoEnfundado(DateTime Fecha)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                List<spReporteEvaluacionProductoEnfundado> resultado;
                clsDEvaluacionProductoEnfundado = new clsDEvaluacionProductoEnfundado();
                resultado = clsDEvaluacionProductoEnfundado.ConsultarReporte(Fecha).OrderBy(x => x.Hora).ToList();
                if (resultado.Count == 0)
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
        [HttpPost]
        public JsonResult GuardarCabeceraControl(CC_EVALUACION_PRODUCTO_ENFUNDADO poCabeceraControl, HttpPostedFileBase dataImg, HttpPostedFileBase dataImg1, HttpPostedFileBase dataImg2, HttpPostedFileBase dataImg3)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                string path = string.Empty;
                string NombreImg = string.Empty;
                string NombreImg1 = string.Empty;
                string NombreImg2 = string.Empty;
                string NombreImg3 = string.Empty;
                if (dataImg != null)
                {
                    path = Server.MapPath("~/Content/Img/EvaluacionProductoEnfundado/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var date = DateTime.Now;
                    long n = long.Parse(date.ToString("yyyyMMddHHmmss"));
                    var ext2 = dataImg.FileName.Split('.');
                    var cont = ext2.Length;
                    NombreImg = "EvaluacionProductoEnfundado/EvaluacionProductoEnfundado" + n.ToString() +"1"+ "." + ext2[cont - 1];
                    poCabeceraControl.ImagenCodigo = NombreImg;
                }
                if (dataImg1 != null)
                {
                    path = Server.MapPath("~/Content/Img/EvaluacionProductoEnfundado/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var date = DateTime.Now;
                    long n = long.Parse(date.ToString("yyyyMMddHHmmss"));
                    var ext2 = dataImg1.FileName.Split('.');
                    var cont = ext2.Length;
                    NombreImg1 = "EvaluacionProductoEnfundado/EvaluacionProductoEnfundado" + n.ToString() + "2" + "." + ext2[cont - 1];
                    poCabeceraControl.ImagenProducto1 = NombreImg1;
                }
                if (dataImg2 != null)
                {
                    path = Server.MapPath("~/Content/Img/EvaluacionProductoEnfundado/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var date = DateTime.Now;
                    long n = long.Parse(date.ToString("yyyyMMddHHmmss"));
                    var ext2 = dataImg2.FileName.Split('.');
                    var cont = ext2.Length;
                    NombreImg2 = "EvaluacionProductoEnfundado/EvaluacionProductoEnfundado" + n.ToString() + "3" + "." + ext2[cont - 1];
                    poCabeceraControl.ImagenProducto2 = NombreImg2;
                }
                if (dataImg3 != null)
                {
                    path = Server.MapPath("~/Content/Img/EvaluacionProductoEnfundado/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var date = DateTime.Now;
                    long n = long.Parse(date.ToString("yyyyMMddHHmmss"));
                    var ext2 = dataImg3.FileName.Split('.');
                    var cont = ext2.Length;
                    NombreImg3 = "EvaluacionProductoEnfundado/EvaluacionProductoEnfundado" + n.ToString() + "4"+ "." + ext2[cont - 1];
                    poCabeceraControl.ImagenProducto3 = NombreImg3;
                }
                poCabeceraControl.FechaIngresoLog = DateTime.Now;
                poCabeceraControl.UsuarioIngresoLog = lsUsuario[0];
                poCabeceraControl.TerminalIngresoLog = Request.UserHostAddress;
                poCabeceraControl.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                object[] resultado = null;
                clsDEvaluacionProductoEnfundado = new clsDEvaluacionProductoEnfundado();
                if (poCabeceraControl.IdEvaluacionProductoEnfundado == 0)
                {
                    resultado = clsDEvaluacionProductoEnfundado.GuardarCabeceraControl(poCabeceraControl);
                }
                else
                {
                    resultado = clsDEvaluacionProductoEnfundado.ActualizarCabeceraControl(poCabeceraControl);
                }
                if (dataImg != null)
                {
                    dataImg.SaveAs(path + Path.GetFileName(NombreImg));
                }
                if (dataImg1 != null)
                {
                    dataImg1.SaveAs(path + Path.GetFileName(NombreImg1));
                }
                if (dataImg2 != null)
                {
                    dataImg2.SaveAs(path + Path.GetFileName(NombreImg2));
                }
                if (dataImg3 != null)
                {
                    dataImg3.SaveAs(path + Path.GetFileName(NombreImg3));
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
        public JsonResult ConsultarCabeceraControl(CC_EVALUACION_PRODUCTO_ENFUNDADO poCabControl)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                CC_EVALUACION_PRODUCTO_ENFUNDADO resultado = null;
                clsDEvaluacionProductoEnfundado = new clsDEvaluacionProductoEnfundado();
                resultado = clsDEvaluacionProductoEnfundado.ConsultarCabeceraControl(poCabControl.FechaProduccion.Value);
                if (resultado != null)
                {
                    return Json(new
                    {
                        resultado.IdEvaluacionProductoEnfundado,
                        resultado.FechaProduccion,
                        resultado.Cliente,
                        resultado.Marca,
                        resultado.Proveedor,
                        resultado.Batch,
                        resultado.Lote,
                        resultado.Lomo,
                        resultado.Miga,
                        resultado.Trozo,
                        resultado.Destino,
                        resultado.NivelLimpieza,
                        resultado.OrdenFabricacion,
                        resultado.Observacion,
                        resultado.EstadoControl,
                        resultado.ImagenCodigo,
                        resultado.ImagenProducto1,
                        resultado.ImagenProducto2,
                        resultado.ImagenProducto3,
                        resultado.RotacionImagenCod,
                        resultado.RotacionImagenProd1,
                        resultado.RotacionImagenProd2,
                        resultado.RotacionImagenProd3
                    }, JsonRequestBehavior.AllowGet);
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
        public JsonResult EliminarCabeceraControl(int IdCabecera)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                CC_EVALUACION_PRODUCTO_ENFUNDADO poCabecera = new CC_EVALUACION_PRODUCTO_ENFUNDADO()
                {
                    IdEvaluacionProductoEnfundado = IdCabecera,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                object[] Respuesta = null;
                clsDEvaluacionProductoEnfundado = new clsDEvaluacionProductoEnfundado();
                Respuesta = clsDEvaluacionProductoEnfundado.InactivarCabeceraControl(poCabecera);
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
        public JsonResult GuardarDetalleControl(CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE poDetalleControl)
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
                object[] resultado = null;
                clsDEvaluacionProductoEnfundado = new clsDEvaluacionProductoEnfundado();
                if (poDetalleControl.IdDetalleEvaluacionProductoEnfundado == 0)
                {
                    resultado = clsDEvaluacionProductoEnfundado.GuardarDetalleControl(poDetalleControl);
                }
                else
                {
                    resultado = clsDEvaluacionProductoEnfundado.ActualizarDetalleControl(poDetalleControl);
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
        public JsonResult EliminarDetalleControl(int IdDetalle)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE poDetControl = new CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE()
                {
                    IdDetalleEvaluacionProductoEnfundado = IdDetalle,
                    UsuarioIngresoLog = lsUsuario[0],
                    FechaIngresoLog = DateTime.Now,
                    TerminalIngresoLog = Request.UserHostAddress
                };
                object[] Respuesta = null;
                clsDEvaluacionProductoEnfundado = new clsDEvaluacionProductoEnfundado();
                Respuesta = clsDEvaluacionProductoEnfundado.InactivarDetalle(poDetControl);
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
        public ActionResult PartialDetalleControl(int IdCabeceraControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                List<DetalleEvaluacionProductoEnfundadoViewModel> resultado;
                clsDEvaluacionProductoEnfundado = new clsDEvaluacionProductoEnfundado();
                resultado = clsDEvaluacionProductoEnfundado.ConsultarDetalleControl(IdCabeceraControl);
                if (resultado.Count == 0)
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
        [HttpPost]
        //public JsonResult GuardarImagenFirma(string imagen, int IdCabecera, string Tipo)
        //{

        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }


        //        clsDEvaluacionProductoEnfundado = new clsDEvaluacionProductoEnfundado();
        //        byte[] Firma = Convert.FromBase64String(imagen);
        //        var resultado = clsDEvaluacionProductoEnfundado.GuardarImagenFirma(Firma, IdCabecera, Tipo, lsUsuario[0], Request.UserHostAddress);
                
        //        var imagenfirma = String.Format("data:image/png;base64,{0}", imagen);
        //        return Json(imagenfirma, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }

        //}
        //public JsonResult ConsultarFirma(int IdCabecera)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }


        //        clsDEvaluacionProductoEnfundado = new clsDEvaluacionProductoEnfundado();
        //        var resultado = clsDEvaluacionProductoEnfundado.ConsultarFirmaControl(IdCabecera);

        //        if (resultado != null)
        //        {
        //            var base64 = Convert.ToBase64String(resultado);
        //            var imagenfirma = String.Format("data:image/png;base64,{0}", base64);
        //            return Json(imagenfirma, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("0", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [Authorize]
        public ActionResult BandejaEvaluacionProductoEnfundado()
        {
            try
            {
                ViewBag.DateRangePicker = "1";
                ViewBag.FirmaPad = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
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
        public ActionResult BandejaAprobadosEvaluacionProductoEnfundadoPartial(DateTime? FechaInicio, DateTime? FechaFin, bool EstadoControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                List<CabeceraEvaluacionProductoEnfundadoViewModel> resultado = new List<CabeceraEvaluacionProductoEnfundadoViewModel>();
                clsDEvaluacionProductoEnfundado = new clsDEvaluacionProductoEnfundado();
                resultado = clsDEvaluacionProductoEnfundado.ConsultarBandejaEvaluacionLomosyMiga(FechaInicio, FechaFin, EstadoControl);
                if (resultado.Count == 0)
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
        public ActionResult PartialDetalleBandeja(int IdCabeceraControl)
        {
            try
            {

                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                List<DetalleEvaluacionProductoEnfundadoViewModel> resultado;
                clsDEvaluacionProductoEnfundado = new clsDEvaluacionProductoEnfundado();
                resultado = clsDEvaluacionProductoEnfundado.ConsultarDetalleControl(IdCabeceraControl);
                if (resultado.Count == 0)
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
        [HttpPost]
        public JsonResult AprobarControl(int IdCabecera, string imagen)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }

                byte[] Firma = Convert.FromBase64String(imagen);
                clsDEvaluacionProductoEnfundado = new clsDEvaluacionProductoEnfundado();
                string Respuesta = clsDEvaluacionProductoEnfundado.AprobarControl(IdCabecera, Request.UserHostAddress, lsUsuario[0], Firma);
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
        //public JsonResult ConsultarFirmaAprobacion(int IdCabecera)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //        if (string.IsNullOrEmpty(lsUsuario[0]))
        //        {
        //            return Json("101", JsonRequestBehavior.AllowGet);
        //        }


        //        clsDEvaluacionProductoEnfundado = new clsDEvaluacionProductoEnfundado();
        //        var resultado = clsDEvaluacionProductoEnfundado.ConsultarFirmaAprobacion(IdCabecera);

        //        if (resultado[0] != null)
        //        {
        //            var base64 = Convert.ToBase64String(resultado[0] as byte[]);
        //            var imagenfirma = String.Format("data:image/png;base64,{0}", base64);
        //            return Json(imagenfirma, JsonRequestBehavior.AllowGet);
        //        }
        //        else if (Convert.ToBoolean(resultado[1]) && (resultado[0] == null))
        //        {
        //            return Json("000", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("0", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        clsDError = new clsDError();
        //        lsUsuario = User.Identity.Name.Split('_');
        //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
        //        return Json(Mensaje, JsonRequestBehavior.AllowGet);
        //    }
        //}
   
    }
}