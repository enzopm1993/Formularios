using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.AnalisisAguaCaldero
{
    public class ClsdAnalisisAguaCaldero
    {
        public List<spConsultaAnalisisAguaCaldero> ConsultaAnalisisAguaCaldero(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var query = entities.spConsultaAnalisisAguaCaldero(Fecha).ToList();
                return query;
            }
        }


        public void GuardarModificarAnalisisAguaCaldero(CC_ANALISIS_AGUA_CALDEROS model,CC_ANALISIS_AGUA_CALDEROS_DETALLE detalle)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    CC_ANALISIS_AGUA_CALDEROS poControlReporte = entities.CC_ANALISIS_AGUA_CALDEROS.FirstOrDefault(x => x.Fecha == model.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    var IControl = 0;
                    if (poControlReporte != null)
                    {
                        poControlReporte.TerminalModificacionLog = model.TerminalIngresoLog;
                        poControlReporte.UsuarioModificacionLog = model.UsuarioIngresoLog;
                        poControlReporte.FechaModificacionLog = model.FechaIngresoLog;
                        IControl = poControlReporte.IdAnalisisAguaCalderos;
                    }
                    else
                    {
                        entities.CC_ANALISIS_AGUA_CALDEROS.Add(model);
                        entities.SaveChanges();
                        IControl = model.IdAnalisisAguaCalderos;
                    }

                   
                    var modelDetalle = entities.CC_ANALISIS_AGUA_CALDEROS_DETALLE.FirstOrDefault(y => y.IdAnalisisAguaCalderos == IControl
                                        && y.IdParametro == detalle.IdParametro && y.IdEquipo == detalle.IdEquipo && y.EstadoRegistro == clsAtributos.EstadoRegistroActivo);

                    if (modelDetalle != null)
                    {

                        detalle.UsuarioModificacionLog = model.UsuarioModificacionLog;
                        detalle.FechaModificacionLog = model.FechaModificacionLog;
                        detalle.TerminalModificacionLog = model.TerminalModificacionLog;
                        detalle.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                        modelDetalle.Valor = detalle.Valor;
                    }
                    else
                    {
                        detalle.IdAnalisisAguaCalderos = IControl;
                        detalle.UsuarioIngresoLog = model.UsuarioIngresoLog;
                        detalle.FechaIngresoLog = model.FechaIngresoLog;
                        detalle.TerminalIngresoLog = model.TerminalIngresoLog;
                        detalle.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                        entities.CC_ANALISIS_AGUA_CALDEROS_DETALLE.Add(detalle);
                    }
                   

                    entities.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public void EliminarAnalisisAguaCaldero(CC_ANALISIS_AGUA_CALDEROS_DETALLE  model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_ANALISIS_AGUA_CALDEROS_DETALLE.FirstOrDefault(x => x.IdAnalisisAguaCalderosDetalle == model.IdAnalisisAguaCalderosDetalle);
                if (poControl != null)
                {
                    var poControl1 = entities.CC_ANALISIS_AGUA_CALDEROS_DETALLE.Count(x => x.IdAnalisisAguaCalderos == model.IdAnalisisAguaCalderos && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    var poControl2 = entities.CC_ANALISIS_AGUA_CALDEROS.FirstOrDefault(x => x.IdAnalisisAguaCalderos == model.IdAnalisisAguaCalderos);
                    if (poControl2 != null  && poControl1==1)
                    {
                        poControl2.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                        poControl2.TerminalModificacionLog = model.TerminalIngresoLog;
                        poControl2.UsuarioModificacionLog = model.UsuarioIngresoLog;
                        poControl2.FechaModificacionLog = model.FechaIngresoLog;
                    }

                    poControl.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                    entities.SaveChanges();
                }

               


            }
        }
        public List<CC_ANALISIS_AGUA_CALDEROS> ConsultaAnalisisAguaCalderoControl(DateTime FechaDesde, DateTime FechaHasta, bool Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_ANALISIS_AGUA_CALDEROS.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                && x.EstadoReporte == Estado).ToList();
            }
        }

        public List<spReporteAnalisisAguaCaldero> ConsultaAnalisisAguaCalderoControl(DateTime FechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spReporteAnalisisAguaCaldero(FechaDesde, FechaHasta).ToList();
            }
        }

        public List<CC_ANALISIS_AGUA_CALDEROS> ConsultaAnalisisAguaCalderoControl(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_ANALISIS_AGUA_CALDEROS.Where(x => x.Fecha == Fecha
                                                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }

        public List<CC_ANALISIS_AGUA_CALDEROS> ConsultaAnalisisAguaCalderoControlPendiente()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_ANALISIS_AGUA_CALDEROS.Where(x => !x.EstadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }
        public void Aprobar_ReporteAnalisisAguaCaldero(CC_ANALISIS_AGUA_CALDEROS controlCloro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_ANALISIS_AGUA_CALDEROS.FirstOrDefault(x => x.IdAnalisisAguaCalderos == controlCloro.IdAnalisisAguaCalderos || (x.Fecha == controlCloro.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
                if (model != null)
                {
                    model.EstadoReporte = controlCloro.EstadoReporte;
                    model.AprobadoPor = controlCloro.AprobadoPor;
                    model.FechaAprobacion = controlCloro.FechaAprobacion;
                    model.FechaModificacionLog = controlCloro.FechaIngresoLog;
                    model.TerminalModificacionLog = controlCloro.TerminalIngresoLog;
                    model.UsuarioModificacionLog = controlCloro.UsuarioIngresoLog;
                    db.SaveChanges();
                }

            }
        }
    }
}