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
        // GET: Seguridad
        [Authorize]
        public ActionResult Opcion()
        {
            try
            {
                clsDopcion = new clsDOpcion();
                var opciones = clsDopcion.ConsultarOpciones().Select(x=>x.Nombre);
                ViewBag.opciones=opciones;

                return View();

            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return RedirectToAction("Home","Home");
            }
        }
        
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
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }
        }

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