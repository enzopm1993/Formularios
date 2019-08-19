using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class AsistenciaController : Controller
    {
        // GET: Asistencia
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult EditarAsistencia()
        {
            return View();
        }
        public ActionResult CambiarPersonalDeArea()
        {
            return View();
        }

        // GET: Asistencia/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult Cuchillo()
        {
            return View();
        }

        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult ReporteDistribucion()
        {
            return View();
        }
        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult PersonalNomina()
        {
            return View();
        }

    }
}
