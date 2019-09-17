using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class EmpleadoController : Controller
    {

        string[] liststring = null;
        clsDError clsDError = null;
        // GET: Empleado
        public ActionResult EmpleadoTurno()
        {
            
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                SetErrorMessage(ex.Message);
                liststring = User.Identity.Name.Split('_');
                clsDError = new clsDError();
                clsDError.GrabarError(new ERROR
                {
                    Controlador = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    Mensaje = ex.Message,
                    Observacion = "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(),
                    FechaIngreso = DateTime.Now,
                    TerminalIngreso = Request.UserHostAddress,
                    UsuarioIngreso = liststring[1]
                });
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
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