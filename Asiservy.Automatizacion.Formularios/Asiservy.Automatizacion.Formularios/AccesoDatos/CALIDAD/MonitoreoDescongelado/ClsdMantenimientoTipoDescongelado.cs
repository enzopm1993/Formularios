using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MonitoreoDescongelado
{
    public class ClsdMantenimientoTipoDescongelado
    {
        public List<CC_MANTENIMIENTO_TIPO_DESCONGELADO> ConsultaManteminetoTipoDescongelado()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_MANTENIMIENTO_TIPO_DESCONGELADO.ToList();
                return lista;
            }
        }

        public void GuardarModificarMantenimientoTipoDescongelado(CC_MANTENIMIENTO_TIPO_DESCONGELADO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_TIPO_DESCONGELADO.FirstOrDefault(x => x.CodTipoMonitoreo == model.CodTipoMonitoreo);
                if (poControl != null)
                {
                    poControl.Descripcion = model.Descripcion;
                    poControl.Abreviatura = model.Abreviatura;
                    poControl.EstadoRegistro = model.EstadoRegistro;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.CC_MANTENIMIENTO_TIPO_DESCONGELADO.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarMantenimientoTipoDescongelado(CC_MANTENIMIENTO_TIPO_DESCONGELADO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_TIPO_DESCONGELADO.FirstOrDefault(x => x.CodTipoMonitoreo == model.CodTipoMonitoreo);
                if (poControl != null)
                {
                    poControl.EstadoRegistro = model.EstadoRegistro;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                    entities.SaveChanges();
                }

            }
        }
    }
}