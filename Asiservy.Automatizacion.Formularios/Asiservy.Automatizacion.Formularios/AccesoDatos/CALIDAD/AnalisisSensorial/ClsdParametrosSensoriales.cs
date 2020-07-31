using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.AnalisisSensorial
{
    public class ClsdParametrosSensoriales
    {
        #region MANTENIMIENTO INTERMEDIA
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
                var poControl = entities.CC_MANTENIMIENTO_INTERMEDIO_AS.FirstOrDefault(x => x.IdCalificacion == model.IdCalificacion && x.IdParametroSensorial== model.IdParametroSensorial);
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


        #region MANTENIMIENTO DE PARAMETROS SENSORIALES
        public List<CC_MANTENIMIENTO_PARAMETRO_SENSORIAL_AS> ConsultaParametroSensorial()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {

                var lista = (from x in entities.CC_MANTENIMIENTO_PARAMETRO_SENSORIAL_AS.AsNoTracking()
                             select x).ToList();
                lista= lista.Select(x=> new CC_MANTENIMIENTO_PARAMETRO_SENSORIAL_AS()
                             {
                                 Abreviatura = x.Abreviatura,
                                 Descripcion= x.Descripcion,
                                 EstadoRegistro= x.EstadoRegistro,
                                 FechaIngresoLog= x.FechaIngresoLog,
                                 FechaModificacionLog=x.FechaModificacionLog,
                                 IdParametroSensorial=x.IdParametroSensorial,
                                 TerminalIngresoLog=x.TerminalIngresoLog,
                                 TerminalModificacionLog=x.TerminalModificacionLog,
                                 UsuarioIngresoLog=x.UsuarioIngresoLog,
                                 UsuarioModificacionLog=x.UsuarioModificacionLog
                             }).ToList();
                return lista;
            }
        }

        public void GuardarModificarParametroSensorial(CC_MANTENIMIENTO_PARAMETRO_SENSORIAL_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_PARAMETRO_SENSORIAL_AS.FirstOrDefault(x => x.IdParametroSensorial == model.IdParametroSensorial);
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
                    entities.CC_MANTENIMIENTO_PARAMETRO_SENSORIAL_AS.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarParametroSensorial(CC_MANTENIMIENTO_PARAMETRO_SENSORIAL_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_PARAMETRO_SENSORIAL_AS.FirstOrDefault(x => x.IdParametroSensorial == model.IdParametroSensorial);
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


        #region MANTENIMIENTO DE CALIFICACION
        public List<CC_MANTENIMIENTO_CALIFICACION_AS> ConsultaMantemientoCalificacion()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = (from x in entities.CC_MANTENIMIENTO_CALIFICACION_AS
                             select x).ToList();

                lista = lista.Select(x=> new CC_MANTENIMIENTO_CALIFICACION_AS()
                             {
                                 Abreviatura = x.Abreviatura,
                                 Descripcion = x.Descripcion,
                                 EstadoRegistro = x.EstadoRegistro,
                                 FechaIngresoLog = x.FechaIngresoLog,
                                 FechaModificacionLog = x.FechaModificacionLog,
                                 IdCalificacion = x.IdCalificacion,
                                 TerminalIngresoLog = x.TerminalIngresoLog,
                                 TerminalModificacionLog = x.TerminalModificacionLog,
                                 UsuarioIngresoLog = x.UsuarioIngresoLog,
                                 UsuarioModificacionLog = x.UsuarioModificacionLog
                             })
                             .ToList();
                return lista;
            }
        }

        public void GuardarModificarMantenimientoCalificacion(CC_MANTENIMIENTO_CALIFICACION_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_CALIFICACION_AS.FirstOrDefault(x => x.IdCalificacion == model.IdCalificacion);
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
                    entities.CC_MANTENIMIENTO_CALIFICACION_AS.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarMantenimientoCalificacion(CC_MANTENIMIENTO_CALIFICACION_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_CALIFICACION_AS.FirstOrDefault(x => x.IdCalificacion == model.IdCalificacion);
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