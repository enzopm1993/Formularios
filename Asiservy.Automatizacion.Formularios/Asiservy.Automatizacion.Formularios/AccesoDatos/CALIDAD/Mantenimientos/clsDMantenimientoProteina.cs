using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.Mantenimientos
{
    public class clsDMantenimientoProteina
    {
        public List<CC_MANTENIMIENTO_PROTEINA> ConsultaManteminetoProteina()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_MANTENIMIENTO_PROTEINA.ToList();
                return lista;
            }
        }

        public void GuardarModificarMantenimientoProteina(CC_MANTENIMIENTO_PROTEINA model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_PROTEINA.FirstOrDefault(x => x.IdProteina == model.IdProteina);
                if (poControl != null)
                {
                    poControl.Descripcion = model.Descripcion;
                    // poControl.Hora = model.Hora;
                    poControl.EstadoRegistro = model.EstadoRegistro;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.CC_MANTENIMIENTO_PROTEINA.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarMantenimientoProteina(CC_MANTENIMIENTO_PROTEINA model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_PROTEINA.FirstOrDefault(x => x.IdProteina == model.IdProteina);
                if (poControl != null)
                {
                    poControl.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                    entities.SaveChanges();
                }

            }
        }

    }
}