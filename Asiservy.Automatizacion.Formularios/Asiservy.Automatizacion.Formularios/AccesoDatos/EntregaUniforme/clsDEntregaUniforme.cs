using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.EntregaUniforme
{
    public class clsDEntregaUniforme
    {
        public List<spConsultaUniformeEntregar> ConsultaEntregaUniforme(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaUniformeEntregar(Fecha).ToList();
                return lista;
            }
        }
        public RespuestaGeneral GuardarModificarControl(ENTREGA_UNIFORME control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var valida = entities.ENTREGA_UNIFORME.FirstOrDefault(x => x.Fecha == control.Fecha && x.Cedula == control.Cedula);
                if(valida!=null)
                {
                    return new RespuestaGeneral { Mensaje= "Ya se ha generado la entrega para este empleado", Respuesta = false };

                }

                var result = entities.ENTREGA_UNIFORME.FirstOrDefault(x => x.IdEntregaUniforme == control.IdEntregaUniforme
                                                                    || (x.Fecha == control.Fecha && x.Cedula == control.Cedula));
                if (result != null)
                {
                    result.EstadoEntrega = control.EstadoEntrega;
                    result.HoraEntregada = control.HoraEntregada;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    control.FechaIngresoLog = DateTime.Now;
                    entities.ENTREGA_UNIFORME.Add(control);
                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }

        public RespuestaGeneral EntregarUniforme(ENTREGA_UNIFORME control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.ENTREGA_UNIFORME.FirstOrDefault(x => x.IdEntregaUniforme == control.IdEntregaUniforme && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
     
                if (result != null)
                {
                    result.EstadoEntrega = control.EstadoEntrega;
                    result.HoraEntregada = control.HoraEntregada;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog; entities.SaveChanges();
                    return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
                }
                else
                {
                    return new RespuestaGeneral { Mensaje = "No se encontró empleado", Respuesta = false };

                }

            }
        }


        public RespuestaGeneral EliminarEntregaUniforme(ENTREGA_UNIFORME control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.ENTREGA_UNIFORME.FirstOrDefault(x => x.IdEntregaUniforme == control.IdEntregaUniforme);
                if (result != null)
                {
                    result.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                    entities.SaveChanges();
                }
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }
    }
}