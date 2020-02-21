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
using Asiservy.Automatizacion.Formularios.AccesoDatos.SolicitudPermiso;

namespace Asiservy.Automatizacion.Formularios.Controllers
{
    [RoutePrefix("api/Solicitud")]
    public class ApiSolicitudPermisoController : ApiController
    {
        clsDError clsDError = null;

        [HttpPost]
        [Route("Generar")]
        public IHttpActionResult PostGenerarSolicitudPermiso(ParamSolicitud parametros)
        {
            try
            {
                RespuestaGeneral respuestaGeneral = new RespuestaGeneral();
                if (string.IsNullOrEmpty(parametros.Identificacion) || string.IsNullOrEmpty(parametros.CodigoMotivo) || string.IsNullOrEmpty(parametros.UsuarioIngreso) || string.IsNullOrEmpty(parametros.TerminalIngreso))
                {
                    //return new RespuestaGeneral { Respuesta = false, Mensaje = "Faltan Parametros" };
                    respuestaGeneral.Respuesta = false;
                    respuestaGeneral.Mensaje = "Faltan Parametros";

                }
                else
                {
                    clsDSolicitudPermiso clsDSolicitudPermiso = new clsDSolicitudPermiso();
                    clsDEmpleado clsDEmpleado = new clsDEmpleado();
                    var poEmpleado = clsDEmpleado.ConsultaEmpleado(parametros.Identificacion).FirstOrDefault();

                    SOLICITUD_PERMISO solicitud =
                    new SOLICITUD_PERMISO
                    {
                        IdSolicitudPermiso = 0,
                        CodigoLinea = poEmpleado.CODIGOLINEA,
                        CodigoArea = poEmpleado.CODIGOAREA,
                        CodigoCargo = poEmpleado.CODIGOCARGO,
                        CodigoRecurso = poEmpleado.CODIGORECURSO,
                        Identificacion = parametros.Identificacion,
                        CodigoMotivo = parametros.CodigoMotivo,
                        Observacion = parametros.Observacion,
                        FechaSalida = parametros.FechaSalida,
                        FechaRegreso = parametros.FechaRegreso,
                        Nivel = clsDSolicitudPermiso.ConsultarNivelUsuario(parametros.Identificacion),
                        FechaIngresoLog = DateTime.Now,
                        UsuarioIngresoLog = parametros.UsuarioIngreso,
                        TerminalIngresoLog = parametros.TerminalIngreso,
                        Origen = clsAtributos.SolicitudOrigenGeneral,
                        EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                        EstadoSolicitud = clsAtributos.EstadoSolicitudPendiente

                    };

                    var mensaje = clsDSolicitudPermiso.GuargarModificarSolicitud(solicitud);
                    //return new RespuestaGeneral { Respuesta = true, Mensaje = mensaje };
                    respuestaGeneral.Respuesta = true;
                    respuestaGeneral.Mensaje = mensaje.Mensaje;
                  
                }
                return Json(respuestaGeneral);

            }

            catch (DbEntityValidationException e)
            {
                //Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
              
                string Mensaje = clsDError.ControlError(parametros.UsuarioIngreso, parametros.TerminalIngreso, this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), null, e);
                return InternalServerError(new Exception(Mensaje));
            }
            catch (Exception ex)
            {
               // Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                clsDError = new clsDError();
               
                string Mensaje = clsDError.ControlError(parametros.UsuarioIngreso, parametros.TerminalIngreso , this.ControllerContext.RouteData.Values["controller"].ToString(),
                    "Metodo: " + this.ControllerContext.RouteData.Values["action"].ToString(), ex, null);
                return InternalServerError(new Exception(Mensaje));
            }

        }

    }
}
