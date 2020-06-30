using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.PRODUCCION.EntradaySalidaDeMaterialesEnAreasDeProceso;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class EntradaySalidaDeMaterialesEnAreasDeProcesoController : Controller
    {
        string[] lsUsuario { get; set; }
        clsDError clsDError { get; set; } = null;
        clsDGeneral clsDGeneral { get; set; } = null;
        clsDClasificador clsDClasificador { get; set; } = null;
        ClsDEntradaSalidaMateriales ClsDEntradaSalidaMateriales { get; set; }
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
        // GET: EntradaySalidaDeMaterialesEnAreasDeProceso
     
        #region MANTENIMIENTO DE MATERIAL
        [Authorize]
        public ActionResult MantenimientoMaterial()
        {
            try
            {
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = "PRODUCCION/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                ViewBag.Material = ClsDEntradaSalidaMateriales.ConsultaMaterialQuebradizo();
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
        [Authorize]
        [HttpPost]
        public ActionResult MantenimientoMaterial(ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL model)
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                model.UsuarioIngresoLog = lsUsuario[0];
                model.TerminalIngresoLog = Request.UserHostAddress;
                model.FechaIngresoLog = DateTime.Now;
                if (model.IdMaterial == 0)
                {
                    ClsDEntradaSalidaMateriales.GuardarMaterial(model);
                }
                else
                {
                    ClsDEntradaSalidaMateriales.ModificarMaterial(model);
                }
                
                return Json("Registro exitoso", JsonRequestBehavior.AllowGet);
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


        public ActionResult MantenimientoMaterialPartial()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                ClsDEntradaSalidaMateriales = new ClsDEntradaSalidaMateriales();
                var model = ClsDEntradaSalidaMateriales.ConsultaMaterialQuebradizo();
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
        #region Control
        [Authorize]
        public ActionResult ControlEntradaySalidaDeMateriales()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                clsDClasificador = new clsDClasificador();
                ViewBag.JavaScrip = "CALIDAD/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.JqueryRotate = "1";
                ViewBag.dataTableJS = "1";
                ViewBag.select2 = "1";
                ViewBag.MascaraInput = "1";
                ViewBag.MaskedInput = "1";
                ViewBag.Turno = new SelectList(clsDClasificador.ConsultarClasificador(clsAtributos.GrupoCodTurno), "Codigo", "Descripcion");
                ViewBag.Linea = clsDGeneral.ConsultarLineaUsuario(lsUsuario[0]);
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
        #endregion
    }
}