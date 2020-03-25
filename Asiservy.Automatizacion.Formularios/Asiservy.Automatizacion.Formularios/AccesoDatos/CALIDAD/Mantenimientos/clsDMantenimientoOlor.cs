using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MantenimientoOlor
{
    public class clsDMantenimientoOlor
    {
        public List<CC_MANTENIMIENTO_OLOR> ConsultaManteminetoOlor()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_MANTENIMIENTO_OLOR.ToList();
                return lista;
            }
        }

        public void GuardarModificarMantenimientoOlor(CC_MANTENIMIENTO_OLOR model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_OLOR.FirstOrDefault(x => x.IdOlor == model.IdOlor);
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
                    entities.CC_MANTENIMIENTO_OLOR.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarMantenimientoOlor(CC_MANTENIMIENTO_OLOR model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_OLOR.FirstOrDefault(x => x.IdOlor == model.IdOlor);
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