using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ProyeccionProgramacionController : Controller
    {
        // GET: ProyeccionProgramacion
        [Authorize]
        public ActionResult ProyeccionProgramacion()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [Authorize]
        public ActionResult ProyeccionProgramacionPartial()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [Authorize]
        public ActionResult ProyeccionProgramacionEditar()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}