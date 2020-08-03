using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.AnalisisSensorial
{
    public class ClsdProtocoloMateriaPrima
    {
        #region PROTOCOLO MATERIA PRIMA ANÁLISIS SENSORIAL
        public List<dynamic> ConsultaIntermedia()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_MANTENIMIENTO_INTERMEDIO_AS.AsNoTracking().Include("CC_MANTENIMIENTO_PARAMETRO_SENSORIAL_AS").AsNoTracking().Include("CC_MANTENIMIENTO_CALIFICACION_AS").AsNoTracking().ToList();
                List<dynamic> lista2 = (from x in lista
                                        select new
                                        {
                                            IdCalificacion = x.IdCalificacion,
                                            UsuarioModificacionLog = x.UsuarioModificacionLog,
                                            DescripcionCalificacion = x.CC_MANTENIMIENTO_CALIFICACION_AS.Descripcion,
                                            DescripcionParametroSensorial = x.CC_MANTENIMIENTO_PARAMETRO_SENSORIAL_AS.Descripcion,
                                            Descripcion = x.Descripcion,
                                            EstadoRegistro = x.EstadoRegistro,
                                            FechaIngresoLog = x.FechaIngresoLog,
                                            FechaModificacionLog = x.FechaModificacionLog,
                                            IdIntermedia = x.IdIntermedia,
                                            IdParametroSensorial = x.IdParametroSensorial,
                                            TerminalIngresoLog = x.TerminalIngresoLog,
                                            TerminalModificacionLog = x.TerminalModificacionLog,
                                            UsuarioIngresoLog = x.UsuarioIngresoLog
                                        }).ToList<dynamic>();

                return lista2;
            }
        }

        public List<CC_MANTENIMIENTO_INTERMEDIO_AS> ConsultaIntermedia2()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_MANTENIMIENTO_INTERMEDIO_AS.AsNoTracking().ToList();
                return lista;
            }
        }

        public void GuardarModificarIntermedia(CC_MANTENIMIENTO_INTERMEDIO_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_INTERMEDIO_AS.FirstOrDefault(x => x.IdCalificacion == model.IdCalificacion && x.IdParametroSensorial == model.IdParametroSensorial);
                if (poControl != null)
                {
                    poControl.Descripcion = model.Descripcion.ToUpper();
                    poControl.EstadoRegistro = model.EstadoRegistro;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    model.Descripcion = model.Descripcion.ToUpper();
                    entities.CC_MANTENIMIENTO_INTERMEDIO_AS.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarIntermedia(CC_MANTENIMIENTO_INTERMEDIO_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_INTERMEDIO_AS.FirstOrDefault(x => x.IdCalificacion == model.IdCalificacion && x.IdParametroSensorial == model.IdParametroSensorial);
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
        #endregion

    }
}