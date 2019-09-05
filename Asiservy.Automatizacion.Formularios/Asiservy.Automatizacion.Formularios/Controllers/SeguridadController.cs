using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Seguridad;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class SeguridadController : Controller
    {
        clsDError clsDError = null;
        // GET: Seguridad
        [Authorize]
        public ActionResult Opcion()
        {
            return View();
        }

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