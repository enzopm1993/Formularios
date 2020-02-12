using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.MapeoProductoTunel
{
    public class clsDMapeoProductoTunel
    {
        public List<MAPEO_PRODUCTO_TUNEL> ConsultaMapeoProductoTunel(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.MAPEO_PRODUCTO_TUNEL.Where(x=> x.Fecha==Fecha).ToList();
                return lista;
            }
        }
        public RespuestaGeneral GuardarModificarControl(MAPEO_PRODUCTO_TUNEL control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.MAPEO_PRODUCTO_TUNEL.FirstOrDefault(x => x.IdMapeoProductoTunel == control.IdMapeoProductoTunel);
                if (result != null)
                {
                    //result.FechaVencimiento = control.FechaVencimiento;
                    //result.CodigoProducto = control.CodigoProducto;
                    result.Observacion = control.Observacion;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    control.FechaIngresoLog = DateTime.Now;
                    entities.MAPEO_PRODUCTO_TUNEL.Add(control);
                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }

        public RespuestaGeneral EliminarProductoTerminado(MAPEO_PRODUCTO_TUNEL control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.MAPEO_PRODUCTO_TUNEL.FirstOrDefault(x => x.IdMapeoProductoTunel == control.IdMapeoProductoTunel);
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