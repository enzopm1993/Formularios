using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.AnalisisSensorial
{
    public class ClsdMantenimientoApariencia
    {

        public List<CC_MANTENIMIENTO_APARIENCIA_AS> ConsultaManteminetoApariencia()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_MANTENIMIENTO_APARIENCIA_AS.ToList();
                return lista;
            }
        }

        public void GuardarModificarMantenimientoApariencia(CC_MANTENIMIENTO_APARIENCIA_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_APARIENCIA_AS.FirstOrDefault(x => x.IdApariencia == model.IdApariencia);
                if (poControl != null)
                {
                    poControl.Descripcion = model.Descripcion.ToUpper();
                    poControl.Abreviatura = model.Abreviatura.ToUpper();
                    poControl.EstadoRegistro = model.EstadoRegistro;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    model.Descripcion = model.Descripcion.ToUpper();
                    model.Abreviatura = model.Abreviatura.ToUpper();
                    entities.CC_MANTENIMIENTO_APARIENCIA_AS.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarMantenimientoApariencia(CC_MANTENIMIENTO_APARIENCIA_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_APARIENCIA_AS.FirstOrDefault(x => x.IdApariencia == model.IdApariencia);
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