using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class SeguridadController : Controller
    {
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
        
    }
}