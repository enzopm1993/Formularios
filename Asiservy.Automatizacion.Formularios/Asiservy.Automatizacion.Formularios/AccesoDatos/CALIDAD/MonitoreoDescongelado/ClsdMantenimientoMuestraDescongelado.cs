using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.CALIDAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MonitoreoDescongelado
{
    public class ClsdMantenimientoMuestraDescongelado
    {
        public List<MantenimientoMuestraDescongeladoModel> ConsultaManteminetoMuestraDescongelado()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = (from x in  entities.CC_MANTENIMIENTO_MUESTRA_DESCONGELADO
                             select new MantenimientoMuestraDescongeladoModel(){
                                 Abreviatura= x.Abreviatura,
                                 Descripcion=x.Descripcion,
                                 EstadoRegistro = x.EstadoRegistro,
                                 FechaIngresoLog = x.FechaIngresoLog,
                                 FechaModificacionLog = x.FechaModificacionLog,
                                 IdMuestra = x.IdMuestra,
                                 TerminalIngresoLog= x.TerminalIngresoLog,
                                 TerminalModificacionLog = x.TerminalModificacionLog,
                                 UsuarioIngresoLog = x.UsuarioIngresoLog,
                                 UsuarioModificacionLog = x.UsuarioModificacionLog
                             }
                             );
                return lista.ToList();
            }
        }

        public void GuardarModificarMantenimientoMuestraDescongelado(CC_MANTENIMIENTO_MUESTRA_DESCONGELADO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_MUESTRA_DESCONGELADO.FirstOrDefault(x => x.IdMuestra == model.IdMuestra);
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
                   // var CodMuestra = entities.Database.SqlQuery<string>("select dbo.fn_RetornaCodigo ('2')").FirstOrDefault(); 
                    model.Descripcion = model.Descripcion.ToUpper();
                    model.Abreviatura = model.Abreviatura.ToUpper();
                    entities.CC_MANTENIMIENTO_MUESTRA_DESCONGELADO.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarMantenimientoMuestraDescongelado(CC_MANTENIMIENTO_MUESTRA_DESCONGELADO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_MUESTRA_DESCONGELADO.FirstOrDefault(x => x.IdMuestra == model.IdMuestra);
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