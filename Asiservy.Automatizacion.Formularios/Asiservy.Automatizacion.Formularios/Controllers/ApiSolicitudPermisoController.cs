using Asiservy.Automatizacion.Formularios.Models;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity.Validation;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    public class ApiSolicitudPermisoController : ApiController
    {
        clsDError clsDError = null;

        [HttpPost]
        public IHttpActionResult GenerarSolicitudPermiso(string Identificacion, string CodigoMotivo, string Observacion, string UsuarioIngreso, string TerminalIngreso, DateTime FechaSalida, DateTime FechaRegreso)
        {
            try
            {
                RespuestaGeneral respuestaGeneral = new RespuestaGeneral();
                if (string.IsNullOrEmpty(Identificacion) || string.IsNullOrEmpty(CodigoMotivo) || string.IsNullOrEmpty(UsuarioIngreso) || string.IsNullOrEmpty(TerminalIngreso))
                {
                    //return new RespuestaGeneral { Respuesta = false, Mensaje = "Faltan Parametros" };
                    respuestaGeneral.Respuesta = false;
                    respuestaGeneral.Mensaje = "Faltan Parametros";
                    return Json(respuestaGeneral);

                }

                clsDSolicitudPermiso clsDSolicitudPermiso = new clsDSolicitudPermiso();
                clsDEmpleado clsDEmpleado = new clsDEmpleado();
                var poEmpleado = clsDEmpleado.ConsultaEmpleado(Identificacion).FirstOrDefault();

                SOLICITUD_PERMISO solicitud =
                new SOLICITUD_PERMISO
                {
                    IdSolicitudPermiso = 0,
                    CodigoLinea = poEmpleado.CODIGOLINEA,
                    CodigoArea = poEmpleado.CODIGOAREA,
                    CodigoCargo = poEmpleado.CODIGOCARGO,
                    CodigoRecurso = poEmpleado.CODIGORECURSO,
                    Identificacion = Identificacion,
                    CodigoMotivo = CodigoMotivo,
                    Observacion = Observacion,
                    FechaSalida = FechaSalida,
                    FechaRegreso = FechaRegreso,
                    Nivel = clsDSolicitudPermiso.ConsultarNivelUsuario(Identificacion),
                    FechaIngresoLog = DateTime.Now,
                    UsuarioIngresoLog = UsuarioIngreso,
                    TerminalIngresoLog = TerminalIngreso,
                    Origen = clsAtributos.SolicitudOrigenGeneral,
                    EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                    EstadoSolicitud = clsAtributos.EstadoSolicitudPendiente

                };

                var mensaje = clsDSolicitudPermiso.GuargarModificarSolicitud(solicitud);
                //return new RespuestaGeneral { Respuesta = true, Mensaje = mensaje };
                respuestaGeneral.Respuesta = true;
                respuestaGeneral.Mensaje = mensaje;
                return Json(respuestaGeneral);
            }

            catch (DbEntityValidationException e)
            {
                //Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
              
                string Mensaje = clsDError.ControlError(UsuarioIngreso, TerminalIngreso, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return InternalServerError(new Exception(Mensaje));
            }
            catch (Exception ex)
            {
               // Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
               
                string Mensaje = clsDError.ControlError(UsuarioIngreso, TerminalIngreso , this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return InternalServerError(new Exception(Mensaje));
            }

        }

    }
}
