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
        public ActionResult EmpleadoBuscar()
        {
            using(Asiservy.Automatizacion.Datos.Datos.ASIS_PRODEntities db=new Datos.Datos.ASIS_PRODEntities())
            {
                List<Empleado> Empleados = new List<Empleado>();
                var pListEmpleados= db.spConsutaEmpleadosFiltro(0, 0, 0).ToList();
                foreach (var item in pListEmpleados)
                {
                    Empleados.Add(new Empleado { Cedula = item.CEDULA, Nombre = item.NOMBRES });
                }
               
          
                return PartialView("EmpleadoBuscar", Empleados);
            }
            
        }
    }
}