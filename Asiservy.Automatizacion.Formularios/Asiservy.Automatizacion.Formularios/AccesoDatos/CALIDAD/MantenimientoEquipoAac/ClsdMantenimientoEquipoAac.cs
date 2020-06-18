using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MantenimientoEquipoAac
{
    public class ClsdMantenimientoEquipoAac
    {
        public List<CC_MANTENIMIENTO_EQUIPO_AAC> ConsultaManteminetoEquipoAac()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_MANTENIMIENTO_EQUIPO_AAC.ToList();
                return lista;
            }
        }

        public void GuardarModificarMantenimientoEquipoAac(CC_MANTENIMIENTO_EQUIPO_AAC model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_EQUIPO_AAC.FirstOrDefault(x => x.IdEquipo == model.IdEquipo);
                if (poControl != null)
                {
                    poControl.Descripcion = model.Descripcion;
                    poControl.Abreviatura = model.Abreviatura;
                    poControl.IdGrupo = model.IdGrupo;
                    poControl.EstadoRegistro = model.EstadoRegistro;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.CC_MANTENIMIENTO_EQUIPO_AAC.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarMantenimientoEquipoAac(CC_MANTENIMIENTO_EQUIPO_AAC model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_EQUIPO_AAC.FirstOrDefault(x => x.IdEquipo == model.IdEquipo);
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