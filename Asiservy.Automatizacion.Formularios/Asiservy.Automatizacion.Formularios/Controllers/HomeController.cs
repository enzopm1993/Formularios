using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity.Validation;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class HomeController : Controller
    {
        clsDError clsDError = null;
        clsDSolicitudPermiso clsDSolicitudPermiso = null;
        clsDEmpleado clsDEmpleado = null;
        string[] lsUsuario;
        protected void SetSuccessMessage(string message)
        {
            TempData["MensajeConfirmacion"] = message;
        }
        protected void SetErrorMessage(string message)
        {
            TempData["MensajeError"] = message;
        }
        // GET: Home
        [Authorize]
        public ActionResult Home()
        {

            try
            {
                clsDEmpleado = new clsDEmpleado();
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                 lsUsuario = User.Identity.Name.Split('_');
                string psrolid = lsUsuario[1];
                clsDLogin PsLogin = new clsDLogin();
                var resultado = PsLogin.ConsultarRolesDeUsuario(psrolid);
                Session["Padre"] = resultado[0];
                Session["Hijo"] = resultado[1];
                Session["Modulos"] = resultado[2];
                var Roles = PsLogin.ConsultaRolesUsuario(lsUsuario[1]);
                Notificaciones(Roles);


                return View();

            }
            catch (DbEntityValidationException e)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                SetErrorMessage(Mensaje);
                return View();
            }
            catch (Exception ex)
            {
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                SetErrorMessage(Mensaje);
                return View();
            }


        }

        public void Notificaciones(List<int?> Roles)
        {
            List<string> MensajesNotificaciones = new List<string>();
            if (Roles.Any(x => x.Value == clsAtributos.RolAprobacionSolicitud))
            {
                var solicitudes = clsDSolicitudPermiso.ConsultaSolicitudesPermiso(clsAtributos.EstadoSolicitudPendiente, lsUsuario[1]);
                if (solicitudes.Any())
                {
                    string Mensaje = "Tienes " + solicitudes.Count + " solicitudes en su bandeja por aprobar";
                    // ViewBag.SolicitudPermiso = Mensaje;
                    MensajesNotificaciones.Add(Mensaje);

                }
            }

            if (Roles.Any(x => x.Value == clsAtributos.RolRRHH))
            {
                var solicitudes = clsDSolicitudPermiso.ConsultaSolicitudesPermiso(clsAtributos.EstadoSolicitudAprobado, lsUsuario[1]);
                if (solicitudes.Any())
                {
                    string Mensaje = "Tienes " + solicitudes.Count + " solicitudes en su bandeja por revisar";
                    //ViewBag.SolicitudPermiso = Mensaje;
                    MensajesNotificaciones.Add(Mensaje);
                }
            }

            if (MensajesNotificaciones.Any())
            {
                ViewBag.MensajesNotificaciones = MensajesNotificaciones;
            }

        }


       
    }
}