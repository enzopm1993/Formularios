using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ClsdMantenimientoReactivo
{
    public class ClsdMantenimientoReactivo
    {
        public List<CC_MANTENIMIENTO_REACTIVO> ConsultaManteminetoReactivo()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_MANTENIMIENTO_REACTIVO.ToList();
                return lista;
            }
        }

        public void GuardarModificarMantenimientoReactivo(CC_MANTENIMIENTO_REACTIVO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_REACTIVO.FirstOrDefault(x => x.IdReactivo == model.IdReactivo);
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
                    entities.CC_MANTENIMIENTO_REACTIVO.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarMantenimientoReactivo(CC_MANTENIMIENTO_REACTIVO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_REACTIVO.FirstOrDefault(x => x.IdReactivo == model.IdReactivo);
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