using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Seguridad;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.Models;

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
                var errors = ModelState
               .Where(x => x.Value.Errors.Count > 0)
               .Select(x => new { x.Key, x.Value.Errors })
               .ToArray();
               
                if (string.IsNullOrEmpty(model.Nombre))
                {
                    ModelState.AddModelError("Nombre", "Campo Requerido");
                }
                
                if (string.IsNullOrEmpty(model.Clase))
                {
                    ModelState.AddModelError("Clase", "Campo Requerido");
                }
                if (model.Clase == "0" && model.Padre == null || model.Padre == 0)
                {
                    ModelState.AddModelError("Padre", "Campo Requerido");
                }
                if (string.IsNullOrEmpty(model.Formulario) && model.Clase=="0")
                {
                    ModelState.AddModelError("Formulario", "Campo Requerido");
                }
                ConsultaOpciones();
                if (!ModelState.IsValid)
                    return View(model);

                clsDopcion = new clsDOpcion();
                model.Clase = model.Clase == "0" ? "H" : "P";
                model.EstadoRegistro = model.EstadoRegistro == "true" ? clsAtributos.EstadoRegistroActivo : clsAtributos.EstadoRegistroInactivo;
                string[] Usuario = User.Identity.Name.Split('_');
                model.FechaCreacionLog = DateTime.Now;
                model.UsuarioCreacionLog = Usuario[0];
                model.TerminalCreacionLog = Request.UserHostAddress;
                var respuesta = clsDopcion.GuardarModificarOpcion(model);
                SetSuccessMessage(respuesta);
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
                var opciones = clsDopcion.ConsultarOpciones(null);
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
            var opciones = clsDopcion.ConsultarOpciones(new OPCION {EstadoRegistro=clsAtributos.EstadoRegistroActivo, Clase="P"}).Select(x => new { x.IdOpcion, x.Nombre });
            ViewBag.opciones = opciones;
            List<Clasificador> ClasificadorClase = new List<Clasificador>();
            ClasificadorClase.Add(new Clasificador { codigo = 0, descripcion = "Hijo" });
            ClasificadorClase.Add(new Clasificador { codigo = 1, descripcion = "Padre" });
            ViewBag.Clase = new SelectList(ClasificadorClase, "codigo", "descripcion");
        }

        #endregion

        [Authorize]
        public ActionResult Rol()
        {
            return View();
        }
        [Authorize]
        public ActionResult ConsultaRoles()
        {
            clsDOpcion Opciones = new clsDOpcion();
            var ListaRoles = Opciones.ConsultarRoles();
            return PartialView(ListaRoles);
        }
        [HttpPost]
        public ActionResult Rol(ROL poRol)
        {
            string[] liststring = User.Identity.Name.Split('_');
            try
            {
                if (string.IsNullOrEmpty(poRol.Descripcion))
                {
                    ModelState.AddModelError("ErrorDescripcion", "El Campo Descripción es obligatorio");
                    
                }
                if (string.IsNullOrEmpty(poRol.EstadoRegistro))
                {
                    ModelState.AddModelError("ErrorEstado", "El Campo Estado es obligatorio");

                }
                if (ModelState.IsValid)
                {
                    clsDOpcion poOpcion = new clsDOpcion();
                    string psMensaje = poOpcion.GuardarRol(poRol, liststring[0], Request.UserHostAddress);
                    SetSuccessMessage(psMensaje);
                }
                
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
                    UsuarioIngreso = liststring[0]
                });
                return RedirectToAction("Rol");
            }
            return View(poRol);
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