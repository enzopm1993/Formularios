using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.LaboratorioAnalisisQuimico;
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
using Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ParametroCalidad;

namespace Asiservy.Automatizacion.Formularios.Controllers.CALIDAD
{
    public class LaboratorioAnalisisQuimicoController : Controller
    {
        ClsdParametroCalidad ClsdParametroCalidad { get; set; } = null;
        clsDPeriodo clsDPeriodo { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        clsDLogin clsDLogin { get; set; } = null;
        public clsDReporte ClsDReporte { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        ClsDLaboratorioAnalisisQuimico ClsDLaboratorioAnalisisQuimico { get; set; } = null;
        clsDApiProduccion clsDApiProduccion { get; set; } = null;
        string[] lsUsuario { get; set; } = null;

       
        #region CONTROL
        public ActionResult ControlAnalisisQuimico()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.JqueryRotate = "1";
                ViewBag.select2 = "1";
                ViewBag.usuarioAnalista = lsUsuario[0];
                clsDLogin = new clsDLogin();
                var usuarioOpcion = clsDLogin.ValidarPermisoOpcion(lsUsuario[1], "ReporteAnalisisQuimico");
                if (usuarioOpcion)
                {
                    ViewBag.Link = "../" + RouteData.Values["controller"] + "/" + "ReporteAnalisisQuimico";
                }
                clsDClasificador = new clsDClasificador();
                var poTurno = clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno).ToList();
                if (poTurno != null)
                {
                    ViewBag.Turno = poTurno;
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
        public ActionResult ControlAnalisisQuimicoPartial(DateTime fechaDesde)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');               
                if (!User.Identity.IsAuthenticated){
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();                
                clsDApiProduccion = new clsDApiProduccion();
                ClsdParametroCalidad = new ClsdParametroCalidad();
                var parametros = ClsdParametroCalidad.ConsultaManteminetoParametroCalidad(clsAtributos.CC_CodParametroCloroCisterna);
                var listaParadasCocinas = clsDApiProduccion.ParadasCocinasPorFecha(fechaDesde);
                if (listaParadasCocinas != null)
                {
                    return PartialView(listaParadasCocinas);
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
        public JsonResult GuardarModificarAnalisisQuimico(CC_ANALISIS_QUIMICO_PRECOCCION_CTRL model, int siAprobar)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(model.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                if (model.IdAnalisis != 0 && siAprobar == 0)
                {
                    var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(model.IdAnalisis, DateTime.MinValue);
                    if (estadoReporte.EstadoReporte)
                    {
                        return Json("4", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                    }
                }
                var valor = ClsDLaboratorioAnalisisQuimico.GuardarModificarAnalisisQuimico(model, siAprobar);
                if (valor == 0)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 1)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else if (valor == 2) { return Json("2", JsonRequestBehavior.AllowGet); }
                else return Json("3", JsonRequestBehavior.AllowGet);//ERROR DE FECHA
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
        public JsonResult EliminarAnalisisQuimico(CC_ANALISIS_QUIMICO_PRECOCCION_CTRL model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(model.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                model.FechaIngresoLog = DateTime.Now;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(model.IdAnalisis, DateTime.MinValue);
                if (!estadoReporte.EstadoReporte)
                {
                    var valor = ClsDLaboratorioAnalisisQuimico.EliminarAnalisisQuimico(model);
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
        public JsonResult ConsultarEstadoReporte(DateTime fechaControl, int idAnalisis = 0)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var lista = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(idAnalisis, fechaControl);
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
        public JsonResult ConsultarCabeceraTurno(DateTime fechaControl, string turno = null)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var lista = ClsDLaboratorioAnalisisQuimico.ConsultarCabeceraTurno(turno, fechaControl);
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
        public JsonResult GuardarModificarDetalle(CC_ANALISIS_QUIMICO_PRECOCCION_DET model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }               
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(model.IdAnalisis, DateTime.MinValue);
                if (estadoReporte==null)
                {
                    return Json("10", JsonRequestBehavior.AllowGet);//IdAnalisis NO ENCONTRADO
                }
                if (estadoReporte.EstadoReporte)
                {
                    return Json("3", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                var valor = 0;
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                valor = ClsDLaboratorioAnalisisQuimico.GuardarModificarDetalle(model);

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
        public JsonResult EliminarDetalle(CC_ANALISIS_QUIMICO_PRECOCCION_DET model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
               
                ClsDLaboratorioAnalisisQuimico = new ClsDLaboratorioAnalisisQuimico();
                var estadoReporte = ClsDLaboratorioAnalisisQuimico.ConsultarEstadoReporte(model.IdAnalisis, DateTime.MinValue);
                if (estadoReporte == null)
                {
                    return Json("3", JsonRequestBehavior.AllowGet);//IdAnalisis NO ENCONTRADO
                }
                if (estadoReporte.EstadoReporte)
                {
                    return Json("2", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
                }
                clsDPeriodo = new clsDPeriodo();
                bool periodo = clsDPeriodo.ValidaFechaPeriodo(estadoReporte.Fecha);
                if (!periodo)
                {
                    return Json("100", JsonRequestBehavior.AllowGet);
                }
                var valor = 0;
                model.FechaIngresoLog = DateTime.Now;
                model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.UsuarioIngresoLog = lsUsuario[0];
                valor = ClsDLaboratorioAnalisisQuimico.EliminarDetalle(model);

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
        //public JsonResult GuardarModificarAccionCorrectiva(CC_MATERIAL_QUEBRADIZO_ACCI_CORRECTIVA model, HttpPostedFileBase dataImg)
        //{
        //    try
        //    {
        //        lsUsuario = User.Identity.Name.Split('_');
        //         if (!User.Identity.IsAuthenticated){
    //                return Json("101", JsonRequestBehavior.AllowGet);
    //}

    //        ClsMaterialQuebradizo = new ClsMaterialQuebradizo();
    //        var estadoReporte = ClsMaterialQuebradizo.ConsultarEstadoReporte(model.IdMaterial, DateTime.MinValue);
    //        if (estadoReporte.EstadoReporte)
    //        {
    //            return Json("3", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
    //        }
    //        string path = string.Empty;
    //        string NombreImg = string.Empty;
    //        if (dataImg != null)
    //        {
    //            decimal mb = 1024 * 1024 * 5;//bytes to Mb; max 5Mb
    //            var supportedTypes = new[] { "jpg", "jpeg" };
    //            var fileExt = Path.GetExtension(dataImg.FileName).Substring(1);
    //            if (!supportedTypes.Contains(fileExt))
    //            {
    //                return Json("4", JsonRequestBehavior.AllowGet);//NO ES IMG
    //            }
    //            else if (dataImg.ContentLength > (mb))
    //            {
    //                return Json(dataImg.ContentLength, JsonRequestBehavior.AllowGet);//SOBREPASA EL LIMITE PERMITIDO dataImg.ContentLength=bytes convert to Mb
    //            }
    //            path = Server.MapPath(clsAtributos.UrlImagen + "MaterialQuebradizo/");
    //            if (!Directory.Exists(path))
    //            {
    //                Directory.CreateDirectory(path);
    //            }
    //            var date = DateTime.Now;
    //            long n = long.Parse(date.ToString("yyyyMMddHHmmss"));
    //            var ext2 = dataImg.FileName.Split('.');
    //            var cont = ext2.Length;
    //            NombreImg = "AccionCorrectiva" + n.ToString() + "." + ext2[cont - 1];
    //            model.RutaFoto = NombreImg;
    //        }
    //        model.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
    //        model.FechaIngresoLog = DateTime.Now;
    //        model.TerminalIngresoLog = Request.UserHostAddress;
    //        model.UsuarioIngresoLog = lsUsuario[0];
    //        var valor = ClsMaterialQuebradizo.GuardarModificarAccionCorrectiva(model);
    //        if (dataImg != null)
    //        {
    //            dataImg.SaveAs(path + Path.GetFileName(NombreImg));
    //        }
    //        if (valor == 0)
    //        {
    //            return Json("0", JsonRequestBehavior.AllowGet);
    //        }
    //        else
    //        {
    //            return Json("1", JsonRequestBehavior.AllowGet);
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
    //public ActionResult VerCrearImagenPartial(int idMaterial, int idArea, int op)
    //{
    //    try
    //    {
    //        lsUsuario = User.Identity.Name.Split('_');
    //        if (string.IsNullOrEmpty(lsUsuario[0]))
    //        {
    //            return Json("101", JsonRequestBehavior.AllowGet);
    //        }
    //        ClsMaterialQuebradizo = new ClsMaterialQuebradizo();
    //        var lista = ClsMaterialQuebradizo.ConsultarDetalle(idMaterial, op, idArea);
    //        if (lista.Count != 0)
    //        {
    //            ViewBag.Path = clsAtributos.UrlImagen.Replace("~", "..") + "MaterialQuebradizo/";
    //            return PartialView(lista);
    //        }
    //        else
    //        {
    //            return Json("0", JsonRequestBehavior.AllowGet);
    //        }
    //    }
    //    catch (DbEntityValidationException e)
    //    {
    //        clsDError = new clsDError();
    //        lsUsuario = User.Identity.Name.Split('_');
    //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
    //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
    //        SetErrorMessage(Mensaje);
    //        return RedirectToAction("Home", "Home");
    //    }
    //    catch (Exception ex)
    //    {
    //        clsDError = new clsDError();
    //        lsUsuario = User.Identity.Name.Split('_');
    //        string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
    //            "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
    //        SetErrorMessage(Mensaje);
    //        return RedirectToAction("Home", "Home");
    //    }
    //}
    //public JsonResult EliminarAccionCorrectiva(CC_MATERIAL_QUEBRADIZO_ACCI_CORRECTIVA model)
    //{
    //    try
    //    {
    //        lsUsuario = User.Identity.Name.Split('_');
    //        if (string.IsNullOrEmpty(lsUsuario[0]))
    //        {
    //            return Json("101", JsonRequestBehavior.AllowGet);
    //        }
    //        ClsMaterialQuebradizo = new ClsMaterialQuebradizo();
    //        var estadoReporte = ClsMaterialQuebradizo.ConsultarEstadoReporte(model.IdMaterial, DateTime.MinValue);
    //        if (estadoReporte.EstadoReporte)
    //        {
    //            return Json("2", JsonRequestBehavior.AllowGet);//REGISTRO APROBADO
    //        }
    //        var valor = 0;
    //        model.FechaIngresoLog = DateTime.Now;
    //        model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
    //        model.TerminalIngresoLog = Request.UserHostAddress;
    //        model.UsuarioIngresoLog = lsUsuario[0];
    //        valor = ClsMaterialQuebradizo.EliminarAccionCorrectiva(model);

    //        if (valor == 0)
    //        {
    //            return Json("0", JsonRequestBehavior.AllowGet);
    //        }
    //        else
    //        {
    //            return Json("1", JsonRequestBehavior.AllowGet);
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