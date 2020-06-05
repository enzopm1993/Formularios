using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity.Validation;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.AccesoDatos.ProyeccionProgramacion;
using System.Globalization;
using Asiservy.Automatizacion.Formularios.Models;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia;
using System.Net;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Vacaciones;
using Newtonsoft.Json;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class HomeController : Controller
    {
       
        CultureInfo ci = new CultureInfo("Es-Es");
        clsDError clsDError = null;
        clsDSolicitudPermiso clsDSolicitudPermiso = null;
        clsDEmpleado clsDEmpleado = null;
        clsDParametro clsDParametro = null;
        clsDProyeccionProgramacion clsDProyeccionProgramacion = null;
        clsDAsistencia clsDAsistencia = null;
        clsDGeneral clsDGeneral = null;
        clsApiGeneral clsApiGeneral = null;
        ClsVacaciones clsVacaciones = null;
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
                ViewBag.dataTableJS = "1";
                ViewBag.JavaScrip = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
                ViewBag.Apexcharts = "1";
                clsDEmpleado = new clsDEmpleado();
                clsDSolicitudPermiso = new clsDSolicitudPermiso();
                clsVacaciones = new ClsVacaciones();
                clsDGeneral = new clsDGeneral();
                clsDLogin PsLogin = new clsDLogin();

                lsUsuario = User.Identity.Name.Split('_');
                string psrolid = lsUsuario[1];
                if (PsLogin.ValidarUsuarioRol(lsUsuario[1], clsAtributos.RolGarita))
                {
                    ViewBag.Garita = "1";
                }
                var resultado = PsLogin.ConsultarRolesDeUsuario(psrolid);
                Session.Timeout = 1480;
                Session["Padre"] = resultado[0];
                Session["Hijo"] = resultado[1];
                Session["Modulos"] = resultado[2];
                var Roles = PsLogin.ConsultaRolesUsuario(lsUsuario[1]);
                var Empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                if (Empleado != null)
                {
                    ViewBag.Nombre = Empleado.NOMBRES;
                }
                ViewBag.Vacaciones = JsonConvert.SerializeObject(clsVacaciones.ConsultarVacaciones(lsUsuario[1], "E").FirstOrDefault());
                ViewBag.Marcacion = clsDGeneral.ConsultarBiometricoxFecha(lsUsuario[1], DateTime.Now);
                Notificaciones(Roles);
                var BD = clsDGeneral.getDataBase();
                if (BD == clsAtributos.DesarrolloBD)
                {
                    Session["BaseDatos"] =clsAtributos.BDDesarrollo;
                }
                if (BD == clsAtributos.PreProduccionBD)
                {
                    Session["BaseDatos"] = clsAtributos.BDPreProduccion;
                }
                if (BD == clsAtributos.ProduccionBD)
                {
                    Session["BaseDatos"] = clsAtributos.BDProduccion;
                }
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

        [Authorize]
        public ActionResult HomeError(string Msg)
        {
            try
            {
                if(string.IsNullOrEmpty(Msg))
                {
                    Msg = "ha ocurrido un error, Comuniquese con sistemas!!";
                }
                ViewBag.Mensaje = Msg;
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

        public ActionResult ConsultaComunicados()
        {
            try
            {
                lsUsuario = User.Identity.Name.Split('_');
                if (string.IsNullOrEmpty(lsUsuario[0]))
                {
                    return Json("101", JsonRequestBehavior.AllowGet);
                }
                clsApiGeneral = new clsApiGeneral();
                var poComunicados = clsApiGeneral.ConsultaComunicados();
                if (!poComunicados.Any())
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                return PartialView(poComunicados);

            }
            catch (DbEntityValidationException e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
                lsUsuario = User.Identity.Name.Split('_');
                string Mensaje = clsDError.ControlError(lsUsuario[0], Request.UserHostAddress, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return Json(Mensaje, JsonRequestBehavior.AllowGet);
            }
        }

        public void Notificaciones(List<int?> Roles)
        {
            clsDParametro = new clsDParametro();
            clsDSolicitudPermiso = new clsDSolicitudPermiso();
            List<RespuestaGeneral> MensajesNotificaciones = new List<RespuestaGeneral>();

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
                var solicitudes = clsDSolicitudPermiso.ConsultaSolicitudesPermisoNotificaciones(clsAtributos.EstadoSolicitudPendiente, lsUsuario[1]);
                if (solicitudes>0)
                {
                     string enlace = "/SolicitudPermiso/BandejaAprobacion";
                    string Mensaje = "Tienes " + solicitudes + " solicitudes en su bandeja por aprobar";
                    MensajesNotificaciones.Add(new RespuestaGeneral
                    {
                        Mensaje = Mensaje,
                        Observacion = enlace
                    });

                }
            }

            if (Roles.Any(x => x.Value == clsAtributos.RolRRHH))
            {
                var solicitudes = clsDSolicitudPermiso.ConsultaSolicitudesPermisoNotificaciones(clsAtributos.EstadoSolicitudAprobado, lsUsuario[1]);
                if (solicitudes>0)
                {
                     string enlace = "/SolicitudPermiso/BandejaRRHH";
                    string Mensaje = "Tiene " + solicitudes + " solicitudes en su bandeja por revisar";
                    MensajesNotificaciones.Add(new RespuestaGeneral
                    {
                        Mensaje = Mensaje,
                        Observacion = enlace
                    });
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
                     string enlace = "/SolicitudPermiso/BandejaMedico";
                    string Mensaje = "Tiene " + solicitudes.Count + " solicitudes en su bandeja por revisar";
                    MensajesNotificaciones.Add(new RespuestaGeneral
                    {
                        Mensaje = Mensaje,
                        Observacion = enlace
                    });
                }
            }

            if (Roles.Any(x => x.Value == clsAtributos.RolGarita))
            {
                var solicitudes = clsDSolicitudPermiso.ConsultaSolicitudesPermisoReporte(null,null,clsAtributos.EstadoSolicitudAprobado, true, null, null).ToList();
                if (solicitudes.Any())
                {
                     string enlace = "/SolicitudPermiso/ReporteSolicitud";
                    string Mensaje = "Tiene " + solicitudes.Count + " solicitudes en su bandeja";
                    MensajesNotificaciones.Add(new RespuestaGeneral
                    {
                        Mensaje = Mensaje,
                        Observacion = enlace
                    });
                }
            }

            if (Roles.Any(x => x.Value == clsAtributos.RolControladorGeneral))
            {
                clsDProyeccionProgramacion = new clsDProyeccionProgramacion();
                clsDAsistencia = new clsDAsistencia();
                clsDEmpleado = new clsDEmpleado();
                var programaciones = clsDProyeccionProgramacion.ConsultaProyeccionProgramacion();
                if(programaciones!= null && programaciones.EditaProduccion)
                {
                    string dia = ci.DateTimeFormat.GetDayName(programaciones.FechaProduccion.DayOfWeek);
                    string enlace = "/ProyeccionProgramacion/EditarProyeccionProgramacionProduccion";
                    string Mensaje = "Tiene la proyección de la programación pendiente de finalizar del dia "+ dia +", "+ programaciones.FechaProduccion.ToString("dd-MM-yyyy");
                  
                    MensajesNotificaciones.Add(new RespuestaGeneral {
                        Mensaje = Mensaje,
                        Observacion = enlace
                    });
                }

                lsUsuario = User.Identity.Name.Split('_');
                var empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                if (empleado != null)
                {
                    var finalizarAsistencia = clsDAsistencia.ConsultaFaltantesFinalizarAsistencia(empleado.CODIGOLINEA, DateTime.Now.AddDays(-1));
                    if (finalizarAsistencia.Any())
                    {
                        foreach (var x in finalizarAsistencia)
                        {

                            string dia = ci.DateTimeFormat.GetDayName(x.FechaInicio.Value.DayOfWeek);
                            string enlace = "/Asistencia/FinalizarAsistencia";
                            string Mensaje = "No ha finalizado la Asistencia del: " + dia + ", " + x.FechaInicio.Value.ToString("dd-MM-yyyy");

                            MensajesNotificaciones.Add(new RespuestaGeneral
                            {
                                Mensaje = Mensaje,
                                Observacion = enlace
                            });

                        }
                    }
                }

            }

            if (Roles.Any(x => x.Value == clsAtributos.RolControladorLinea || x.Value == clsAtributos.RolEnlatado ||
            x.Value == clsAtributos.RolEtiquetadoLata || x.Value == clsAtributos.RolEtiquetadoPouch || x.Value == clsAtributos.RolLimpiezaPouch
            || x.Value == clsAtributos.RolLimpiezaPouch || x.Value == clsAtributos.RolAutoclave
            || x.Value == clsAtributos.RolFrio || x.Value == clsAtributos.RolEvicerado))
            {
                clsDAsistencia = new clsDAsistencia();
                clsDEmpleado = new clsDEmpleado();
                lsUsuario = User.Identity.Name.Split('_');
                var empleado = clsDEmpleado.ConsultaEmpleado(lsUsuario[1]).FirstOrDefault();
                if (empleado != null)
                {
                    var finalizarAsistencia = clsDAsistencia.ConsultaFaltantesFinalizarAsistencia(empleado.CODIGOLINEA, DateTime.Now.AddDays(-1));
                    var finalizarCantidadFecha = finalizarAsistencia.Select(x => x.FechaInicio).Distinct();
                    if (finalizarAsistencia.Any())
                    {
                        foreach (var x in finalizarCantidadFecha)
                        {
                            int cantidad = finalizarAsistencia.Count(y => y.FechaInicio == x.Value);
                            string dia = ci.DateTimeFormat.GetDayName(x.Value.DayOfWeek);
                            string enlace = "/Asistencia/AsistenciaFinalizar";
                            string Mensaje = "No ha finalizado la Asistencia del día: " + dia + ", " + x.Value.ToString("dd-MM-yyyy") + " Existen " + cantidad + " empleados sin finalizar";

                            MensajesNotificaciones.Add(new RespuestaGeneral
                            {
                                Mensaje = Mensaje,
                                Observacion = enlace
                            });

                        }
                    }
                }
            }

            if (Roles.Any(x => x.Value == clsAtributos.AsistenteProduccion))
            {
                clsDAsistencia = new clsDAsistencia();
                clsDEmpleado = new clsDEmpleado();
                clsDGeneral = new clsDGeneral();
                var finalizarAsistencia = clsDAsistencia.ConsultaFaltantesFinalizarAsistenciaTodos(DateTime.Now.AddDays(-1));
                var finalizarCantidadFecha = finalizarAsistencia.Select(x => new { Fecha=x.FechaInicio, Linea =x.CodLinea}).Distinct();
                if (finalizarAsistencia.Any())
                {
                    foreach (var x in finalizarCantidadFecha)
                    {
                        var linea = clsDGeneral.ConsultaLineas(x.Linea).FirstOrDefault();
                        int cantidad = finalizarAsistencia.Count(y => y.FechaInicio == x.Fecha && y.CodLinea==x.Linea);
                        string dia = ci.DateTimeFormat.GetDayName(x.Fecha.Value.DayOfWeek);
                        string Mensaje = "No ha finalizado la Asistencia "+linea.Descripcion+" del día: " + dia + ", " + x.Fecha.Value.ToString("dd-MM-yyyy") + " Existen " + cantidad + " empleados sin finalizar";

                        MensajesNotificaciones.Add(new RespuestaGeneral
                        {
                            Mensaje = Mensaje,
                        });

                    }
                }

            }

                if (MensajesNotificaciones.Any())
            {
                ViewBag.MensajesNotificaciones = MensajesNotificaciones;
            }

        }


       
    }
}