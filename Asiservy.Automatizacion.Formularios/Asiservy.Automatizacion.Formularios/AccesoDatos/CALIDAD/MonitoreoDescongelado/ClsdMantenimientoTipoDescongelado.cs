using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.CALIDAD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MonitoreoDescongelado
{
    public class ClsdMantenimientoTipoDescongelado
    {
        public List<MantenimientoTipoDescongeladoModel> ConsultaManteminetoTipoDescongelado()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = (from x in entities.CC_MANTENIMIENTO_TIPO_DESCONGELADO
                             select new MantenimientoTipoDescongeladoModel()
                             {
                                 Abreviatura = x.Abreviatura,
                                 Descripcion = x.Descripcion,
                                 Color = x.Color,
                                 TemperaturaAgua = x.TemperaturaAgua,
                                 EstadoRegistro = x.EstadoRegistro,
                                 FechaIngresoLog = x.FechaIngresoLog,
                                 FechaModificacionLog = x.FechaModificacionLog,
                                 IdTipoMonitoreo = x.IdTipoMonitoreo,
                                 TerminalIngresoLog = x.TerminalIngresoLog,
                                 TerminalModificacionLog = x.TerminalModificacionLog,
                                 UsuarioIngresoLog = x.UsuarioIngresoLog,
                                 UsuarioModificacionLog = x.UsuarioModificacionLog
                             }
                             );
                return lista.ToList();
            }
        }

        public void GuardarModificarMantenimientoTipoDescongelado(CC_MANTENIMIENTO_TIPO_DESCONGELADO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_TIPO_DESCONGELADO.FirstOrDefault(x => x.IdTipoMonitoreo == model.IdTipoMonitoreo);
                if (poControl != null)
                {
                    poControl.Descripcion = model.Descripcion.ToUpper();
                    poControl.Abreviatura = model.Abreviatura.ToUpper();
                    poControl.TemperaturaAgua = model.TemperaturaAgua;
                    poControl.Color = model.Color;
                    poControl.EstadoRegistro = model.EstadoRegistro;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    //Guid g = Guid.NewGuid();
                    
                    //model.CodTipoMonitoreo = entities.ret;
                    model.Descripcion = model.Descripcion.ToUpper();
                    model.Abreviatura = model.Abreviatura.ToUpper();
                    entities.CC_MANTENIMIENTO_TIPO_DESCONGELADO.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarMantenimientoTipoDescongelado(CC_MANTENIMIENTO_TIPO_DESCONGELADO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_TIPO_DESCONGELADO.FirstOrDefault(x => x.IdTipoMonitoreo == model.IdTipoMonitoreo);
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