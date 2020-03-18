using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.CondicionPersonal
{
    public class clsDCondicionPersonal
    {
        public List<MANTENIMIENTO_CONDICION> ConsultaManteminetoCondicion()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.MANTENIMIENTO_CONDICION.ToList();
                return lista;
            }
        }

        public void GuardarModificarMantenimientoCondicion(MANTENIMIENTO_CONDICION model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.MANTENIMIENTO_CONDICION.FirstOrDefault(x => x.IdMantenimientoCondicion == model.IdMantenimientoCondicion);
                if (poControl != null)
                {
                    poControl.Descripcion = model.Descripcion;
                   // poControl.Hora = model.Hora;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.MANTENIMIENTO_CONDICION.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarMantenimientoCondicion(MANTENIMIENTO_CONDICION model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.MANTENIMIENTO_CONDICION.FirstOrDefault(x => x.IdMantenimientoCondicion == model.IdMantenimientoCondicion);
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