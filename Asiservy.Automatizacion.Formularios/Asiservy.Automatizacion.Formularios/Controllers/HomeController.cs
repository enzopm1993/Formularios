using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity.Validation;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class HomeController : Controller
    {
        clsDError clsDError = null;
        clsDSolicitudPermiso clsDSolicitudPermiso = null;
        clsDEmpleado clsDEmpleado = null;
        clsDParametro clsDParametro = null;
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
            clsDParametro = new clsDParametro();
            List<string> MensajesNotificaciones = new List<string>();

            var MensajeUrgente = clsDParametro.ConsultaParametros(new PARAMETRO { Codigo = clsAtributos.ParaMensajeUrgente,
                EstadoRegistro = clsAtributos.EstadoRegistroActivo }).FirstOrDefault();
            if(MensajeUrgente != null)
            {
                ViewBag.MensajeUrgente = MensajeUrgente.Observacion;
            }

            var MensajeAviso = clsDParametro.ConsultaParametros(new PARAMETRO
            {
                Codigo = clsAtributos.ParaMensajeAviso,
                EstadoRegistro = clsAtributos.EstadoRegistroActivo
            }).FirstOrDefault();
            if (MensajeAviso != null)
            {
                ViewBag.MensajeAviso = MensajeAviso.Observacion;
            }



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
                    string Mensaje = "Tiene " + solicitudes.Count + " solicitudes en su bandeja por revisar";
                    //ViewBag.SolicitudPermiso = Mensaje;
                    MensajesNotificaciones.Add(Mensaje);
                }
            }

            if (Roles.Any(x => x.Value == clsAtributos.RolMedico))
            {
                var solicitudes = clsDSolicitudPermiso.ConsultaSolicitudesPermiso(new SOLICITUD_PERMISO
                {                   
                    EstadoSolicitud = clsAtributos.EstadoSolicitudAprobado,
                    Origen = clsAtributos.SolicitudOrigenMedico,
                    ValidaMedico = true
                });

                if (solicitudes.Any())
                {
                    string Mensaje = "Tiene " + solicitudes.Count + " solicitudes en su bandeja por revisar";
                    //ViewBag.SolicitudPermiso = Mensaje;
                    MensajesNotificaciones.Add(Mensaje);
                }
            }

            if (Roles.Any(x => x.Value == clsAtributos.RolGarita))
            {
                var solicitudes = clsDSolicitudPermiso.ConsultaSolicitudesPermisoReporte(null,null,clsAtributos.EstadoSolicitudAprobado, true, DateTime.Now.Date, DateTime.Now.Date).Where(x=> x.FechaBiometrico != null).ToList();
                if (solicitudes.Any())
                {
                    string Mensaje = "Tiene " + solicitudes.Count + " solicitudes en su bandeja";
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