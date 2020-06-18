using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MantenimientoParametroAac
{
    public class ClsdMantenimientoParametroAac
    {
        public List<CC_MANTENIMIENTO_PARAMETRO_AAC> ConsultaManteminetoParametroAac()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_MANTENIMIENTO_PARAMETRO_AAC.ToList();
                return lista;
            }
        }

        public void GuardarModificarMantenimientoParametroAac(CC_MANTENIMIENTO_PARAMETRO_AAC model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_PARAMETRO_AAC.FirstOrDefault(x => x.IdParametro == model.IdParametro);
                if (poControl != null)
                {
                    poControl.Descripcion = model.Descripcion;
                    poControl.Abreviatura = model.Abreviatura;
                    poControl.MaximoPermitido = model.MaximoPermitido;
                    poControl.EstadoRegistro = model.EstadoRegistro;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.CC_MANTENIMIENTO_PARAMETRO_AAC.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarMantenimientoParametroAac(CC_MANTENIMIENTO_PARAMETRO_AAC model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_PARAMETRO_AAC.FirstOrDefault(x => x.IdParametro == model.IdParametro);
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