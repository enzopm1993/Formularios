using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.TiempoExposicion
{
    public class ClsdTiempoExposicion
    {

        public List<CC_TIEMPO_EXPOSICION> ConsultaTiempoExposicion(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var query = entities.CC_TIEMPO_EXPOSICION.Where(x=> x.Fecha==Fecha).ToList();
                var Lista = query
                    .Select(x => new CC_TIEMPO_EXPOSICION()
                    {
                        Fecha = x.Fecha,
                        IdTiempoExposicion = x.IdTiempoExposicion,
                        AprobadoPor = x.AprobadoPor,
                        EstadoRegistro = x.EstadoRegistro,
                        EstadoReporte = x.EstadoReporte,
                        FechaAprobacion = x.FechaAprobacion,
                        FechaIngresoLog = x.FechaIngresoLog,
                        FechaModificacionLog = x.FechaModificacionLog,
                        Observacion = x.Observacion,
                        Pcc = x.Pcc,
                        TerminalIngresoLog = x.TerminalIngresoLog,
                        TerminalModificacionLog = x.TerminalModificacionLog,
                        UsuarioIngresoLog = x.UsuarioIngresoLog,
                        UsuarioModificacionLog = x.UsuarioModificacionLog
                    }).ToList();
                return Lista;
            }
        }

        public void GuardarModificarTiempoExposicion(CC_TIEMPO_EXPOSICION model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                CC_TIEMPO_EXPOSICION poControlReporte = entities.CC_TIEMPO_EXPOSICION.FirstOrDefault(x => x.Fecha == model.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (poControlReporte != null)
                {
                poControlReporte.Pcc = model.Pcc;
                poControlReporte.Observacion = model.Observacion;
                    poControlReporte.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControlReporte.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControlReporte.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.CC_TIEMPO_EXPOSICION.Add(model);
                }
                entities.SaveChanges();


            }
        }

        public void EliminarTiempoExposicion(CC_TIEMPO_EXPOSICION_DETALLE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_TIEMPO_EXPOSICION_DETALLE.FirstOrDefault(x => x.IdTiempoExposicionDetalle == model.IdTiempoExposicionDetalle);
                if (poControl != null)
                {
                    var poControl1 = entities.CC_TIEMPO_EXPOSICION_DETALLE.Count(x => x.IdTiempoExposicionDetalle == model.IdTiempoExposicionDetalle && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    var poControl2 = entities.CC_TIEMPO_EXPOSICION.FirstOrDefault(x => x.IdTiempoExposicion == model.IdTiempoExposicion);
                    if (poControl2 != null && poControl1 == 1)
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
        public List<CC_TIEMPO_EXPOSICION> ConsultaTiempoExposicionControl(DateTime FechaDesde, DateTime FechaHasta, bool Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_TIEMPO_EXPOSICION.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                && x.EstadoReporte == Estado).ToList();
            }
        }

        public CC_TIEMPO_EXPOSICION ValidaReporte(int Id)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_TIEMPO_EXPOSICION.FirstOrDefault(x => x.IdTiempoExposicion == Id
                                                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
            }
        }

        #region  DETALLE
        public List<CC_TIEMPO_EXPOSICION_DETALLE> ConsultaTiempoExposicionDetalle(int Id)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_TIEMPO_EXPOSICION_DETALLE.AsNoTracking()
                    .Where(x => x.IdTiempoExposicion == Id && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                var listaProtocolos = lista
                     .Select(x => new CC_TIEMPO_EXPOSICION_DETALLE
                     {
                         IdTiempoExposicion = x.IdTiempoExposicion,
                         IdTiempoExposicionDetalle = x.IdTiempoExposicionDetalle,
                         OrdenFabricacion=x.OrdenFabricacion,
                         Lote=x.Lote,
                         TemperaturaSala=x.TemperaturaSala,
                         EstadoRegistro = x.EstadoRegistro,
                         FechaIngresoLog = x.FechaIngresoLog,
                         FechaModificacionLog = x.FechaModificacionLog,
                         TerminalIngresoLog = x.TerminalIngresoLog,
                         TerminalModificacionLog = x.TerminalModificacionLog,
                         UsuarioIngresoLog = x.UsuarioIngresoLog,
                         UsuarioModificacionLog = x.UsuarioModificacionLog,
                     }).ToList();
                return listaProtocolos;
            }
        }

        public void GuardarModificarControlDetalle(CC_TIEMPO_EXPOSICION_DETALLE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                CC_TIEMPO_EXPOSICION_DETALLE poControlReporte = entities.CC_TIEMPO_EXPOSICION_DETALLE.FirstOrDefault(x => x.IdTiempoExposicionDetalle == model.IdTiempoExposicionDetalle);
                if (poControlReporte != null)
                {
                    poControlReporte.Lote = model.Lote;
                    poControlReporte.TemperaturaSala = model.TemperaturaSala;
                    poControlReporte.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControlReporte.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControlReporte.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.CC_TIEMPO_EXPOSICION_DETALLE.Add(model);
                }
                 entities.SaveChanges();
                   
            }
        }

       
        public void EliminarDetalle(CC_TIEMPO_EXPOSICION_DETALLE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_TIEMPO_EXPOSICION_DETALLE.FirstOrDefault(x => x.IdTiempoExposicionDetalle == model.IdTiempoExposicionDetalle);
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

        #endregion


        //public List<spReporteTiempoExposicion> ConsultaTiempoExposicionControl(DateTime FechaDesde, DateTime FechaHasta)
        //{
        //    using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
        //    {
        //        return entities.spReporteTiempoExposicion(FechaDesde, FechaHasta).ToList();
        //    }
        //}

        //public List<CC_TIEMPO_EXPOSICION> ConsultaTiempoExposicionControl(DateTime Fecha)
        //{
        //    using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
        //    {
        //        return entities.CC_TIEMPO_EXPOSICION.Where(x => x.Fecha == Fecha
        //                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
        //    }
        //}

        //public List<CC_TIEMPO_EXPOSICION> ConsultaTiempoExposicionControlPendiente()
        //{
        //    using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
        //    {
        //        return entities.CC_TIEMPO_EXPOSICION.Where(x => !x.EstadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
        //    }
        //}
        //public void Aprobar_ReporteTiempoExposicion(CC_TIEMPO_EXPOSICION controlCloro)
        //{
        //    using (ASIS_PRODEntities db = new ASIS_PRODEntities())
        //    {
        //        var model = db.CC_TIEMPO_EXPOSICION.FirstOrDefault(x => x.IdTiempoExposicions == controlCloro.IdTiempoExposicions || (x.Fecha == controlCloro.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
        //        if (model != null)
        //        {
        //            model.EstadoReporte = controlCloro.EstadoReporte;
        //            model.AprobadoPor = controlCloro.AprobadoPor;
        //            model.FechaAprobacion = controlCloro.FechaAprobacion;
        //            model.FechaModificacionLog = controlCloro.FechaIngresoLog;
        //            model.TerminalModificacionLog = controlCloro.TerminalIngresoLog;
        //            model.UsuarioModificacionLog = controlCloro.UsuarioIngresoLog;
        //            db.SaveChanges();
        //        }

        //    }
        //}
    }
}