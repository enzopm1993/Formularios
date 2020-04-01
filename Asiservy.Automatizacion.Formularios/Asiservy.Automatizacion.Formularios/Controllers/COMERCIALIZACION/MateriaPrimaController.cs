using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers.COMERCIALIZACION
{
    public class MateriaPrimaController : Controller
    {
        // GET: MateriaPrima
        public ActionResult SOPMateriaPrima()
        {
            ViewBag.DateRangePicker = "1";
            ViewBag.dataTableJS = "1";
            ViewBag.Pivot = "1";
            ViewBag.JavaScrip = "COMERCIALIZACION/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
            return View();
        }
        public ActionResult ReporteIngresoMateriaPrima()
        {
            ViewBag.DateRangePicker = "1";
            ViewBag.Pivot = "1";
            ViewBag.dataTableJS = "1";
            ViewBag.JavaScrip = "COMERCIALIZACION/" + RouteData.Values["controller"] + "/" + RouteData.Values["action"];
            return View();
        }
    }
}