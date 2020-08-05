using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.AnalisisSensorial
{
    public class ClsdProtocoloMateriaPrima
    {
        #region PROTOCOLO MATERIA PRIMA ANÁLISIS SENSORIAL
       
        public List<CC_PROTOCOLO_MATERIA_PRIMA_AS> ConsultaProtocoloMateriaPrima(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_PROTOCOLO_MATERIA_PRIMA_AS.AsNoTracking()
                    .Where(x => x.Fecha == Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
               var listaProtocolos= lista
                    .Select(x=> new CC_PROTOCOLO_MATERIA_PRIMA_AS{
                        Fecha=x.Fecha,
                        IdProtocoloMateriaPrima = x.IdProtocoloMateriaPrima,
                        AprobadoPor=x.AprobadoPor,
                        CodigoProtocolo=x.CodigoProtocolo,
                        EstadoRegistro=x.EstadoRegistro,
                        EstadoReporte=x.EstadoReporte,
                        FechaAprobacion=x.FechaAprobacion,
                        FechaDescarga=x.FechaDescarga,
                        FechaEvaluacion=x.FechaEvaluacion,
                        FechaIngresoLog=x.FechaIngresoLog,
                        FechaModificacionLog=x.FechaModificacionLog,
                        Lote=x.Lote,
                        LoteDescarga=x.LoteDescarga,
                        Observacion=x.Observacion,
                        OrdenFabricacion=x.OrdenFabricacion,
                        Pcc=x.Pcc,
                        TerminalIngresoLog=x.TerminalIngresoLog,
                        TerminalModificacionLog=x.TerminalModificacionLog,
                        UsuarioIngresoLog=x.UsuarioIngresoLog,
                        UsuarioModificacionLog=x.UsuarioModificacionLog,
                        CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS = x.CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS
                }).ToList();
                return listaProtocolos;
            }
        }

        public void GuardarModificarParamtroMateriaPrima(CC_PROTOCOLO_MATERIA_PRIMA_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_PROTOCOLO_MATERIA_PRIMA_AS.FirstOrDefault(x => x.IdProtocoloMateriaPrima == model.IdProtocoloMateriaPrima );
                if (poControl != null)
                {
                    poControl.FechaEvaluacion = model.FechaEvaluacion;
                    poControl.FechaDescarga = model.FechaDescarga;
                    poControl.Lote = model.Lote;
                    poControl.LoteDescarga = model.LoteDescarga;
                    poControl.CodigoProtocolo = model.CodigoProtocolo;
                    poControl.Pcc = model.Pcc;
                    poControl.Observacion = model.Observacion != null ? model.Observacion.ToUpper() : model.Observacion;
                    poControl.EstadoRegistro = model.EstadoRegistro;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    model.Observacion = model.Observacion!=null? model.Observacion.ToUpper():model.Observacion;
                    entities.CC_PROTOCOLO_MATERIA_PRIMA_AS.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarParametroMateriaPrima(CC_PROTOCOLO_MATERIA_PRIMA_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_PROTOCOLO_MATERIA_PRIMA_AS.FirstOrDefault(x => x.IdProtocoloMateriaPrima == model.IdProtocoloMateriaPrima );
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

        public CC_PROTOCOLO_MATERIA_PRIMA_AS ConsultaProtocoloMateriaPrima(int Id)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_PROTOCOLO_MATERIA_PRIMA_AS.FirstOrDefault(x => x.IdProtocoloMateriaPrima == Id
                                                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
            }
        }
        #endregion




        #region PROTOCOLO DE MATERIA PRIMA DETALLE
        public List<CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS> ConsultaProtocoloMateriaPrimaDetalle(int Id)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS.AsNoTracking().Include("CC_PROTOCOLO_MATERIA_PRIMA_SUBDETALLE_AS")
                    .Where(x => x.IdProtocoloMateriaPrima == Id && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                var listaProtocolos = lista
                     .Select(x => new CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS
                     {
                         IdProtocoloMateriaPrima = x.IdProtocoloMateriaPrima,
                         
                         EstadoRegistro = x.EstadoRegistro,
                         FechaIngresoLog = x.FechaIngresoLog,
                         FechaModificacionLog = x.FechaModificacionLog,
                         TerminalIngresoLog = x.TerminalIngresoLog,
                         TerminalModificacionLog = x.TerminalModificacionLog,
                         UsuarioIngresoLog = x.UsuarioIngresoLog,
                         UsuarioModificacionLog = x.UsuarioModificacionLog
                     }).ToList();
                return listaProtocolos;
            }
        }

        public void GuardarModificarParamtroMateriaPrimaDetalle(CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS.FirstOrDefault(x => x.IdProtocoloMateriaPrimaDetalle == model.IdProtocoloMateriaPrimaDetalle);
                if (poControl != null)
                {
                    poControl.EstadoRegistro = model.EstadoRegistro;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarParametroMateriaPrimaDetalle(CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS.FirstOrDefault(x => x.IdProtocoloMateriaPrimaDetalle == model.IdProtocoloMateriaPrimaDetalle);
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
    }
}