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


            //ViewBag.NombreUsuario = User.Identity.Name.ToString()+ " |";
            return View();
        }

        public ActionResult ViewPrueba()
        {
            return View();
        }      

    }
}