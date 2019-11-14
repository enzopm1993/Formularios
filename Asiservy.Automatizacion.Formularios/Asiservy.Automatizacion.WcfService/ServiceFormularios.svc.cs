
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Asiservy.Automatizacion.Formularios.AccesoDatos;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.WcfService
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class ServiceFormularios : IServiceFormularios
    {
        public RespuestaGenerica GenerarSolicitudPermiso(string Identificacion, string CodigoMotivo, string Observacion, string UsuarioIngreso, string TerminalIngreso, DateTime FechaSalida, DateTime FechaRegreso)
        {
            try
            {
                if (string.IsNullOrEmpty(Identificacion) || string.IsNullOrEmpty(CodigoMotivo) || string.IsNullOrEmpty(UsuarioIngreso) || string.IsNullOrEmpty(TerminalIngreso))
                {
                    return new RespuestaGenerica { Respuesta = false, Mensaje = "Faltan Parametros" };
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
                return new RespuestaGenerica { Respuesta = true, Mensaje = mensaje };
            }
            catch (Exception ex)
            {
                return new RespuestaGenerica { Respuesta = false, Mensaje = ex.Message };
            }
        }
    }
}
