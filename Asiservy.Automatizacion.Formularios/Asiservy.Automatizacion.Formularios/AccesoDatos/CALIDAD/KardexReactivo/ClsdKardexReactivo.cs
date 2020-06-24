using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.KardexReactivo
{
    public class ClsdKardexReactivo
    {
        public List<spConsultaKardexReactivo> ConsultaKardexReactivo(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var query = entities.spConsultaKardexReactivo(Fecha).ToList();
                return query;
            }
        }


        public void GuardarModificarKardexReactivo(CC_KARDEX_REACTIVO model, List<CC_KARDEX_REACTIVO_DETALLE> detalle)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    CC_KARDEX_REACTIVO poControlReporte = entities.CC_KARDEX_REACTIVO.FirstOrDefault(x => x.Fecha == model.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    var idReactivo = 0;
                    if (poControlReporte != null)
                    {
                        poControlReporte.TerminalModificacionLog = model.TerminalIngresoLog;
                        poControlReporte.UsuarioModificacionLog = model.UsuarioIngresoLog;
                        poControlReporte.FechaModificacionLog = model.FechaIngresoLog;
                        idReactivo = poControlReporte.IdKardexReactivo;
                   }
                    else
                    {
                        entities.CC_KARDEX_REACTIVO.Add(model);
                        entities.SaveChanges();
                        idReactivo = model.IdKardexReactivo;
                    }

                    foreach(var x in detalle)
                    {
                        var modelDetalle = entities.CC_KARDEX_REACTIVO_DETALLE.FirstOrDefault(y=> y.IdKardexReactivo == idReactivo
                                           && y.IdReactivo == x.IdReactivo && y.EstadoRegistro == clsAtributos.EstadoRegistroActivo);

                        if (modelDetalle != null)
                        {

                            x.UsuarioModificacionLog = model.UsuarioModificacionLog;
                            x.FechaModificacionLog = model.FechaModificacionLog;
                            x.TerminalModificacionLog = model.TerminalModificacionLog;
                            x.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                            modelDetalle.Valor = x.Valor;
                        }
                        else
                        {
                            x.IdKardexReactivo = idReactivo;
                            x.UsuarioIngresoLog = model.UsuarioIngresoLog;
                            x.FechaIngresoLog = model.FechaIngresoLog;
                            x.TerminalIngresoLog = model.TerminalIngresoLog;
                            x.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                            entities.CC_KARDEX_REACTIVO_DETALLE.Add(x);
                        }
                    }

                    entities.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public void EliminarKardexReactivo(CC_KARDEX_REACTIVO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_KARDEX_REACTIVO.FirstOrDefault(x => x.Fecha == model.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
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
        public List<CC_KARDEX_REACTIVO> ConsultaKardexReactivoControl(DateTime FechaDesde, DateTime FechaHasta, bool Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_KARDEX_REACTIVO.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                && x.EstadoReporte == Estado).ToList();
            }
        }

        public List<spReporteKardexReactivo> ConsultaKardexReactivoControl(DateTime FechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spReporteKardexReactivo(FechaDesde,FechaHasta).ToList();
            }
        }

        public List<CC_KARDEX_REACTIVO> ConsultaKardexReactivoControl(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_KARDEX_REACTIVO.Where(x => x.Fecha == Fecha
                                                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }

        public List<CC_KARDEX_REACTIVO> ConsultaKardexReactivoControlPendiente()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_KARDEX_REACTIVO.Where(x => !x.EstadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }
        public void Aprobar_ReporteKardexReactivo(CC_KARDEX_REACTIVO controlCloro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_KARDEX_REACTIVO.FirstOrDefault(x => x.IdKardexReactivo == controlCloro.IdKardexReactivo || (x.Fecha == controlCloro.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
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