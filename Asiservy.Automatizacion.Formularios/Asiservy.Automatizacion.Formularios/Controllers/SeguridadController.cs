using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class SeguridadController : Controller
    {
        clsDOpcion clsDopcion = null;
        clsDError clsDError = null;
        // GET: Seguridad
        #region OPCION
        [Authorize]
        public ActionResult Opcion()
        {
            try
            {
                ConsultaOpciones();
                return View();

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return RedirectToAction("Home","Home");
            }
        }
        [HttpPost]
        [Authorize]
        public ActionResult Opcion(OPCION model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Nombre))
                {
                    ModelState.AddModelError("Nombre", "Campo Requerido");
                    return View(model);
                }
                if (string.IsNullOrEmpty(model.Formulario))
                {
                    ModelState.AddModelError("Formulario", "Campo Requerido");
                    return View(model);
                }
                if (string.IsNullOrEmpty(model.Clase))
                {
                    ModelState.AddModelError("Clase", "Campo Requerido");
                    return View(model);
                }
                if (model.Clase == "H" && model.Padre>0)
                {
                    ModelState.AddModelError("Padre", "Campo Requerido");
                    return View(model);
                }
                clsDopcion = new clsDOpcion();
                var respuesta = clsDopcion.GuardarModificarOpcion(model);
                SetSuccessMessage(respuesta);
                ConsultaOpciones();
                return View();

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return View();
            }
        }

        [Authorize]
        public ActionResult OpcionPartial()
        {
            try
            {
                clsDopcion = new clsDOpcion();
                var opciones = clsDopcion.ConsultarOpciones();
                return PartialView(opciones);

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = "sistemas"
                });
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }
        }


        public void ConsultaOpciones()
        {
            clsDopcion = new clsDOpcion();
            var opciones = clsDopcion.ConsultarOpciones().Select(x => new { x.IdOpcion, x.Nombre });
            ViewBag.opciones = opciones;
        }

        #endregion

        [Authorize]
        public ActionResult Rol()
        {
            return View();
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