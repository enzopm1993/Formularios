using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public ActionResult Home()
        {
            var a = "Prueba commit";
            return View();
        }

        public ActionResult ViewPrueba()
        {
            var a = "Prueba commit";
            return View();
        }
    }
}