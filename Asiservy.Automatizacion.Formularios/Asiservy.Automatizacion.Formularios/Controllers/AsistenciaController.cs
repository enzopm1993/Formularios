using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.Models;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class AsistenciaController : Controller
    {
        // GET: Asistencia
        [Authorize]
        public ActionResult Asistencia()
        {
            return View();
        }

        [Authorize]
        public ActionResult RptAsistencia()
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
        

        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult Cuchillo()
        {
            return View();
        }
        [Authorize]
        // GET: Asistencia/Cuchillo
        public ActionResult RptCuchillo()
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
        public ActionResult ConsultarEmpleado()
        {
            List<Empleado> Empleados = new List<Empleado>
            {
                new Empleado { Cedula = "0940203406", Nombre = "Juan Maldonado" },
                new Empleado { Cedula = "1188888456", Nombre = "Pedro Suarez" },
                new Empleado { Cedula = "2723626161", Nombre = "Alejandro Sánchez" },
                new Empleado { Cedula = "3635261617", Nombre = "María Perez" },
                new Empleado { Cedula = "1188888456", Nombre = "Andrea Bejarano" },
                new Empleado { Cedula = "2345789123", Nombre = "Juan Peña" },
            };
            return PartialView("Empleados",Empleados);

        }
        

    }
}
