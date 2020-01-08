using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ControlToallaController : Controller
    {
        clsDError clsDError = null;
        // GET: ControlToalla
        [Authorize]
        public ActionResult ControlToalla()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
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
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}