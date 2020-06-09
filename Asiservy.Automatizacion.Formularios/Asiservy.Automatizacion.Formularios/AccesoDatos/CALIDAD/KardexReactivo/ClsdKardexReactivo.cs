using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.KardexReactivo
{
    public class ClsdKardexReactivo
    {
        public List<CC_KARDEX_REACTIVO> ConsultaVerificacionPotenciometro(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var query = entities.CC_KARDEX_REACTIVO.Where(x => x.Fecha == Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();

                return query;
            }
        }


        public void GuardarModificarVerificacionPotenciometro(CC_KARDEX_REACTIVO model, List<CC_KARDEX_REACTIVO_DETALLE> detalle)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    CC_KARDEX_REACTIVO poControlReporte = entities.CC_KARDEX_REACTIVO.FirstOrDefault(x => x.Fecha == model.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);

                    if (poControlReporte != null)
                    {
                        poControlReporte.TerminalModificacionLog = model.TerminalIngresoLog;
                        poControlReporte.UsuarioModificacionLog = model.UsuarioIngresoLog;
                        poControlReporte.FechaModificacionLog = model.FechaIngresoLog;
                   }
                    else
                    {
                        entities.CC_KARDEX_REACTIVO.Add(model);
                    }

                    foreach(var x in detalle)
                    {
                        var modelDetalle = entities.CC_KARDEX_REACTIVO_DETALLE.FirstOrDefault(y=> y.IdKardexReactivo == poControlReporte.IdKardexReactivo
                                           && y.IdKardexReactivoDetalle == x.IdKardexReactivoDetalle);

                        if (modelDetalle != null)
                        {
                            modelDetalle.Valor = x.Valor;
                        }
                        else
                        {
                            entities.CC_KARDEX_REACTIVO_DETALLE.Add(x);
                        }
                    }

                    entities.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public void EliminarVerificacionPotenciometro(CC_KARDEX_REACTIVO model)
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
        public List<CC_KARDEX_REACTIVO> ConsultaVerificacionPotenciometroControl(DateTime FechaDesde, DateTime FechaHasta, bool Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_KARDEX_REACTIVO.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                && x.EstadoReporte == Estado).ToList();
            }
        }

        public List<CC_KARDEX_REACTIVO> ConsultaVerificacionPotenciometroControl(DateTime FechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_KARDEX_REACTIVO.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                               ).ToList();
            }
        }

        public List<CC_KARDEX_REACTIVO> ConsultaVerificacionPotenciometroControl(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_KARDEX_REACTIVO.Where(x => x.Fecha == Fecha
                                                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }

        public List<CC_KARDEX_REACTIVO> ConsultaVerificacionPotenciometroControlPendiente()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_KARDEX_REACTIVO.Where(x => !x.EstadoReporte).ToList();
            }
        }
        public void Aprobar_ReporteVerificacionPotenciometro(CC_KARDEX_REACTIVO controlCloro)
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