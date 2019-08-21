using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiservy.Automatizacion.Formularios.Controllers
{

    public class SolicitudPermisoController : Controller
    {       

        [Authorize]
        // GET: SolicitudPermiso
        public ActionResult BandejaAprobacion()
        {
            List<Solicitud> solicitud = new List<Solicitud>();

            solicitud.Add(new Solicitud { codigo = "1", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Santiago Jose" });
            solicitud.Add(new Solicitud { codigo = "2", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Morales Victor" });
            solicitud.Add(new Solicitud { codigo = "3", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Cazares Julio" });
            solicitud.Add(new Solicitud { codigo = "4", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Vera Jose" });
            solicitud.Add(new Solicitud { codigo = "5", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Chavez Jorge" });
            solicitud.Add(new Solicitud {codigo="6",fecha="16/08/2019" ,Motivo="Vacaciones",Area="Proceso",Empleado="Santiago Emilio" });


            return View(solicitud);
        }
        [Authorize]
        public ActionResult SolicitudPermiso()
        {
            return View();
        }
        public ActionResult SolicitudPermisoDispensario()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BandejaProduccion(string buscar, string SelectPermiso)
        {
            List<Solicitud> solicitud = new List<Solicitud>();

            solicitud.Add(new Solicitud { codigo = "1", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Santiago Jose" });
            solicitud.Add(new Solicitud { codigo = "2", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Morales Victor" });
            solicitud.Add(new Solicitud { codigo = "3", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Cazares Julio" });
            solicitud.Add(new Solicitud { codigo = "4", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Vera Jose" });
            solicitud.Add(new Solicitud { codigo = "5", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Chavez Jorge" });
            solicitud.Add(new Solicitud { codigo = "6", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Santiago Emilio" });
            if(SelectPermiso=="1")
                return View(solicitud.Where(x=>x.codigo.Contains(buscar)).ToList());
            if(SelectPermiso=="2")
                return View(solicitud.Where(x => x.Area.Contains(buscar)).ToList());
            if (SelectPermiso == "3")
                return View(solicitud.Where(x => x.Empleado.Contains(buscar)).ToList());

            return View(solicitud.ToList());


        }


        [Authorize]
        public ActionResult BandejaRRHH()
        {
            List<Solicitud> solicitud = new List<Solicitud>();

            solicitud.Add(new Solicitud { codigo = "1", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Santiago Jose" });
            solicitud.Add(new Solicitud { codigo = "2", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Morales Victor" });
            solicitud.Add(new Solicitud { codigo = "3", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Cazares Julio" });
            solicitud.Add(new Solicitud { codigo = "4", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Vera Jose" });
            solicitud.Add(new Solicitud { codigo = "5", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Chavez Jorge" });
            solicitud.Add(new Solicitud { codigo = "6", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Santiago Emilio" });


            return View(solicitud);
        }

        [Authorize]
        [HttpPost]
        public ActionResult BandejaRRHH(string buscar, string SelectPermiso)
        {
            List<Solicitud> solicitud = new List<Solicitud>();

            solicitud.Add(new Solicitud { codigo = "1", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Santiago Jose" });
            solicitud.Add(new Solicitud { codigo = "2", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Morales Victor" });
            solicitud.Add(new Solicitud { codigo = "3", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Cazares Julio" });
            solicitud.Add(new Solicitud { codigo = "4", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Vera Jose" });
            solicitud.Add(new Solicitud { codigo = "5", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Chavez Jorge" });
            solicitud.Add(new Solicitud { codigo = "6", fecha = "16/08/2019", Motivo = "Vacaciones", Area = "Proceso", Empleado = "Santiago Emilio" });
            if (SelectPermiso == "1")
                return View(solicitud.Where(x => x.codigo.Contains(buscar)).ToList());
            if (SelectPermiso == "2")
                return View(solicitud.Where(x => x.Area.Contains(buscar)).ToList());
            if (SelectPermiso == "3")
                return View(solicitud.Where(x => x.Empleado.Contains(buscar)).ToList());

            return View(solicitud.ToList());
        }
        
    }
}
