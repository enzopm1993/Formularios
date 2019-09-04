using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.Models;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class MensajeController : Controller
    {
        // GET: Mensaje
        public ActionResult Correcto(bool reload=false)
        {
            ViewBag.reload = reload;
            return PartialView();
        }

        public ActionResult Error(bool reload = false)
        {

            ViewBag.reload = reload;
            return PartialView();
        }

        public ActionResult Advertencia(bool reload = false)
        {

            ViewBag.reload = reload;
            return PartialView();
        }

    }
}