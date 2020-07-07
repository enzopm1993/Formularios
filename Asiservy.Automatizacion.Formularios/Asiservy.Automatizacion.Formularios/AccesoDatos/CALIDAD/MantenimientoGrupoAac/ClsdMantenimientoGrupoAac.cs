using Asiservy.Automatizacion.Datos.Datos;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MantenimientoGrupoAac
{
    public class ClsdMantenimientoGrupoAac
    {
        public List<CC_MANTENIMIENTO_GRUPO_AAC> ConsultaManteminetoGrupo()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_MANTENIMIENTO_GRUPO_AAC.ToList();
                return lista;
            }
        }

        public void GuardarModificarMantenimientoGrupo(CC_MANTENIMIENTO_GRUPO_AAC model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_GRUPO_AAC.FirstOrDefault(x => x.IdGrupo == model.IdGrupo);
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
                    entities.CC_MANTENIMIENTO_GRUPO_AAC.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarMantenimientoGrupo(CC_MANTENIMIENTO_GRUPO_AAC model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_GRUPO_AAC.FirstOrDefault(x => x.IdGrupo == model.IdGrupo);
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