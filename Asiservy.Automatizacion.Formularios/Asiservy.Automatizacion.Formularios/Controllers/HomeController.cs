using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public ActionResult Home()
        {
            //using (Asiservy.Automatizacion.Datos.Datos.ASIS_PRODEntities db = new Asiservy.Automatizacion.Datos.Datos.ASIS_PRODEntities())
            //{
            //    List<string> pListPadrestotal=new List<string>();
            //    List<string> pListHijostotal = new List<string>();
            //    string[] liststring = User.Identity.Name.Split('_');
            //    string psrolid = liststring[1];
            //    var piRol = (from r in db.USUARIO_ROL
            //                 where r.IdUsuario==psrolid
            //                 select r.IdRol).ToList();
            //    foreach (var item in piRol)
            //    {
            //        var pListPadres = (from r in db.OPCION_ROL
            //                           join rn in db.OPCION on r.IdOpcion equals rn.IdOpcion
            //                           where r.IdRol == item && rn.Clase == "P"
            //                           select rn.Nombre).ToList();
            //        pListPadrestotal.AddRange(pListPadres);
            //    }


            //    foreach (var item in piRol)
            //    {
            //        var pListHijos = (from r in db.OPCION_ROL
            //                          join rn in db.OPCION on r.IdOpcion equals rn.IdOpcion
            //                          where r.IdRol == item && rn.Clase == "H"
            //                          select rn.Formulario).ToList();
            //        pListHijostotal.AddRange(pListHijos);
            //    }


            //    Session["Padre"] = pListPadrestotal;
            //    Session["Hijo"] = pListHijostotal;


            //}
            string[] liststring = User.Identity.Name.Split('_');
            string psrolid = liststring[1];
            clsDLogin PsLogin = new clsDLogin();
            var resultado = PsLogin.ConsultarRolesDeUsuario(psrolid);
            Session["Padre"] = resultado[0];
            Session["Hijo"] = resultado[1];

            return View();
        }

        public ActionResult ViewPrueba()
        {
            return View();
        }      

    }
}