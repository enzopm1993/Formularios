using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class AsistenciaController : Controller
    {

        clsDGeneral clsDGeneral = null;

        #region Métodos
        public void ConsultaCombosGeneral()
        {
            clsDGeneral = new clsDGeneral();
            ViewBag.Lineas = clsDGeneral.ConsultaLineas();
            //ViewBag.Areas = clsDGeneral.ConsultaAreas("0");
            //ViewBag.Cargos = clsDGeneral.ConsultaCargos("0");
        }
        #endregion

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

        #region Cambio_PersonaldeÁrea
        [Authorize]
        public ActionResult CambiarPersonalDeArea()
        {
            try
            {
                ConsultaCombosGeneral();
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        [Authorize]
        public ActionResult BitacoraCambioPersonal()
        {
            return View();
        }
        #endregion


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
        public ActionResult Empleados()
        {
            
            List<Empleado> Empleados = new List<Empleado>
            {
                new Empleado { Cedula = "0940203406", Nombre = "Juan Maldonado", Area="Procesos", Cargo="Limpiador" },
                new Empleado { Cedula = "1188888456", Nombre = "Pedro Suarez", Area="Procesos", Cargo="Despellejador" },
                new Empleado { Cedula = "2723626161", Nombre = "Alejandro Sánchez", Area="Procesos", Cargo="Limpiador" },
                new Empleado { Cedula = "3635261617", Nombre = "María Perez", Area="Procesos", Cargo="Limpiador" },
                new Empleado { Cedula = "1188888456", Nombre = "Andrea Bejarano", Area="Procesos", Cargo="Despellejador" },
                new Empleado { Cedula = "2345789123", Nombre = "Juan Peña", Area="Procesos", Cargo="Despellejador" },
            };
            return PartialView("Empleados",Empleados);

        }
       

    }
}
