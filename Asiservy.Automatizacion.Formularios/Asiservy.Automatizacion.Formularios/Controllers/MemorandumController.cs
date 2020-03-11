using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class MemorandumController : Controller
    {
        // GET: Memorandum
        public ActionResult Gestion()
        {
            ViewBag.Title = "Gestión de memorandum";
            ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];



            return View();
        }

        public ActionResult Generar()
        {
            return View();
        }

    }
}