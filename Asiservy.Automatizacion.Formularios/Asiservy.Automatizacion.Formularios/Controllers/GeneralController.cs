using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
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
    public class GeneralController : Controller
    {
        string[] lsUsuario { get; set; } = null;
        clsDError clsDError { get; set; } = null;
        clsDEmpleado clsDEmpleado { get; set; } = null;
        clsDApiOrdenFabricacion clsDApiOrdenFabricacion { get; set; } = null;
        // GET: General

        //PRDOCUTO TERMINADO
        public JsonResult ConsultarOFNivel1(DateTime Fecha)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                var ordenes = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaProductoTerminado(Fecha);
                return Json(ordenes, JsonRequestBehavior.AllowGet);
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

        /// <summary>
        /// PARA EL CONTROL DE AVANCE EN LA SALA DE PROCESO
        /// </summary>
        /// <param name="Orden"></param>
        /// <returns></returns>
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
                dynamic result = clsDApiOrdenFabricacion.ConsultaLotesPorOrdenFabricacionLinea(Orden, Linea.CODIGOLINEA);
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

        /// <summary>
        /// CONSULTA DE LOS LOTES POR UNA ORDEN DE FABRICACION
        /// </summary>
        /// <param name="Orden"></param>
        /// <returns></returns>
        public JsonResult ConsultarLotesPorOf(int Orden)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                dynamic result = clsDApiOrdenFabricacion.ConsultaLotesPorOrdenFabricacion(Orden);
                List<ClasificadorGenerico> Listado = new List<ClasificadorGenerico>();
                foreach (var x in result)
                {
                    Listado.Add(new ClasificadorGenerico { descripcion = x.Lote, Especie=x.Especie, Barco=x.Barco });
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

        /// <summary>
        /// PRODUCTO SEMI ELABORADO
        /// </summary>
        public JsonResult ConsultaOFNivel2(DateTime Fecha, string Orden)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                List<OrdenFabricacionAutoclave> result = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaAutoclave(Fecha);
                if (!result.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);

                }
                if (!string.IsNullOrEmpty(Orden))
                {
                    result = result.Where(x => x.ORDEN_FABRICACION == Orden).ToList();
                }
                return Json(result, JsonRequestBehavior.AllowGet);
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

        //PRODUCTOS CONGELADOS -> SOLO OF
        public JsonResult ConsultaSoloOFNivel3(DateTime Fecha)
        {
            try
            {
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                dynamic result = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaProduccion(Fecha);
                List<OrdenFabricacion> Listado = new List<OrdenFabricacion>();
                foreach (var x in result)
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

        //PRODUCTO CONGELADO LOMOS - DATOS OF
        public JsonResult ConsultarDatosOFNivel3(DateTime Fecha)
        {
            try
            {
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                dynamic result = clsDApiOrdenFabricacion.ConsultaOFNivel3(Fecha);
                List<OrdenFabricacion> Listado = new List<OrdenFabricacion>();
                foreach (var x in result)
                {
                    Listado.Add(new OrdenFabricacion { Orden = x.ORDEN_FABRICACION });
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


        public JsonResult ConsultarDatosOrdenFabricacion(string Orden)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(Orden))
                {
                    return Json("0", JsonRequestBehavior.AllowGet);

                }
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                OrdenFabricacionConsumoInsumo result = new OrdenFabricacionConsumoInsumo();
                result = clsDApiOrdenFabricacion.ConsultaOrdenFabricacionPorFechaConsumoInsumo(Orden).FirstOrDefault();
                if (result == null)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);

                }
                return Json(result, JsonRequestBehavior.AllowGet);
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


        public ActionResult EmpleadoBuscar(string dsLinea, string dsArea, string dsCargo)
        {
            try
            {
                clsDEmpleado = new clsDEmpleado();
                List<spConsutaEmpleadosFiltro> lista = clsDEmpleado.ConsultaEmpleadosFiltro(dsLinea, dsArea, dsCargo);
                return PartialView(lista);

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