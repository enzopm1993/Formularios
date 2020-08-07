using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;

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
                    .Select(x=> new CC_PROTOCOLO_MATERIA_PRIMA_AS(){
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
                        UsuarioModificacionLog=x.UsuarioModificacionLog
                       // CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS = x.CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS
                        
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
                var lista = entities.CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS.AsNoTracking().Include("CC_PROTOCOLO_MATERIA_PRIMA_SUBDETALLE_AS").Include("CC_PROTOCOLO_MATERIA_PRIMA_SUBDETALLE_APARIENCIA_AS")
                    .Where(x => x.IdProtocoloMateriaPrima == Id && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                var listaProtocolos = lista
                     .Select(x => new CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS
                     {
                         IdProtocoloMateriaPrima = x.IdProtocoloMateriaPrima,
                         IdProtocoloMateriaPrimaDetalle = x.IdProtocoloMateriaPrimaDetalle,
                         EstadoRegistro = x.EstadoRegistro,
                         FechaIngresoLog = x.FechaIngresoLog,
                         FechaModificacionLog = x.FechaModificacionLog,
                         TerminalIngresoLog = x.TerminalIngresoLog,
                         TerminalModificacionLog = x.TerminalModificacionLog,
                         UsuarioIngresoLog = x.UsuarioIngresoLog,
                         UsuarioModificacionLog = x.UsuarioModificacionLog,
                         CC_PROTOCOLO_MATERIA_PRIMA_SUBDETALLE_AS = x.CC_PROTOCOLO_MATERIA_PRIMA_SUBDETALLE_AS,
                         CC_PROTOCOLO_MATERIA_PRIMA_SUBDETALLE_APARIENCIA_AS=x.CC_PROTOCOLO_MATERIA_PRIMA_SUBDETALLE_APARIENCIA_AS
                     }).ToList();
                return listaProtocolos;
            }
        }

        public void ModificarParametroMateriaPrimaDetalle(CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS poControlReporte = entities.CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS.FirstOrDefault(x => x.IdProtocoloMateriaPrimaDetalle == model.IdProtocoloMateriaPrimaDetalle);
                    if (poControlReporte != null)
                    {
                        poControlReporte.TerminalModificacionLog = model.TerminalIngresoLog;
                        poControlReporte.UsuarioModificacionLog = model.UsuarioIngresoLog;
                        poControlReporte.FechaModificacionLog = model.FechaIngresoLog;
                    }                 

                    foreach(var d in model.CC_PROTOCOLO_MATERIA_PRIMA_SUBDETALLE_AS)
                    {
                        var modelDetalle = entities.CC_PROTOCOLO_MATERIA_PRIMA_SUBDETALLE_AS.FirstOrDefault(y => 
                        y.IdProtocoloMateriaPrimaDetalle==d.IdProtocoloMateriaPrimaDetalle
                        && y.IdParametroSensorial == d.IdParametroSensorial);
                        if (modelDetalle != null)
                        {
                            modelDetalle.IdCalificacion = d.IdCalificacion;
                            //modelDetalle.IdParametroSensorial = d.IdParametroSensorial;
                            modelDetalle.UsuarioModificacionLog = model.UsuarioModificacionLog;
                            modelDetalle.FechaModificacionLog = model.FechaModificacionLog;
                            modelDetalle.TerminalModificacionLog = model.TerminalModificacionLog;
                            modelDetalle.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                        }
                    }

                    foreach (var d in model.CC_PROTOCOLO_MATERIA_PRIMA_SUBDETALLE_APARIENCIA_AS)
                    {
                        var modelDetalle = entities.CC_PROTOCOLO_MATERIA_PRIMA_SUBDETALLE_APARIENCIA_AS.FirstOrDefault(y => 
                        y.IdProtocoloMateriaPrimaDetalle == d.IdProtocoloMateriaPrimaDetalle
                        && y.IdApariencia == d.IdApariencia);
                        if (modelDetalle != null)
                        {
                            modelDetalle.Valor = d.Valor;
                           modelDetalle.UsuarioModificacionLog = model.UsuarioModificacionLog;
                            modelDetalle.FechaModificacionLog = model.FechaModificacionLog;
                            modelDetalle.TerminalModificacionLog = model.TerminalModificacionLog;
                            modelDetalle.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                        }
                    }
                    entities.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public bool GuardarParametroMateriaPrimaDetalle(CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS model, int muestras)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                System.Data.Entity.Core.Objects.ObjectParameter myOutputParamBool = new System.Data.Entity.Core.Objects.ObjectParameter("PS_COD_ERROR", typeof(bool));
                //System.Data.Entity.Core.Objects.ObjectParameter myOutputParamString = new System.Data.Entity.Core.Objects.ObjectParameter("myOutputParamString", typeof(string));
                //System.Data.Entity.Core.Objects.ObjectParameter myOutputParamInt = new System.Data.Entity.Core.Objects.ObjectParameter("myOutputParamInt", typeof(Int32));
                var json = Json.Encode(model);

                entities.spGuardarProtocoloMateriaPrima(json, muestras, myOutputParamBool);
                bool myBool = Convert.ToBoolean(myOutputParamBool.Value);
                //string myString = Convert.ToString(myOutputParamString.Value);
                //int myInt = Convert.ToInt32(myOutputParamInt.Value);


                return myBool;

            }
        }

        public void EliminarParametroMateriaPrimaDetalle(CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS model, int[] detalles)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                foreach (var d in detalles) {
                    var poControl = entities.CC_PROTOCOLO_MATERIA_PRIMA_DETALLE_AS.FirstOrDefault(x => x.IdProtocoloMateriaPrimaDetalle == d);
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

        #endregion

        #region APROBACION - REPORTE



        public List<CC_PROTOCOLO_MATERIA_PRIMA_AS> ConsultaProtocoloMateriaPrima(DateTime FechaDesde, DateTime FechaHasta, bool Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_PROTOCOLO_MATERIA_PRIMA_AS.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                && x.EstadoReporte == Estado).ToList();
            }
        }

        //public List<spReporteAnalisisAguaCaldero> ConsultaProtocoloMateriaPrima(DateTime FechaDesde, DateTime FechaHasta)
        //{
        //    using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
        //    {
        //        return entities.spReporteAnalisisAguaCaldero(FechaDesde, FechaHasta).ToList();
        //    }
        //}

        

        public List<CC_PROTOCOLO_MATERIA_PRIMA_AS> ConsultaProtocoloMateriaPrimaPendiente()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_PROTOCOLO_MATERIA_PRIMA_AS.Where(x => !x.EstadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }
        public void Aprobar_Reporte(CC_PROTOCOLO_MATERIA_PRIMA_AS Control)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_PROTOCOLO_MATERIA_PRIMA_AS.FirstOrDefault(x => x.IdProtocoloMateriaPrima == Control.IdProtocoloMateriaPrima || (x.Fecha == Control.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
                if (model != null)
                {
                    model.EstadoReporte = Control.EstadoReporte;
                    model.AprobadoPor = Control.AprobadoPor;
                    model.FechaAprobacion = Control.FechaAprobacion;
                    model.FechaModificacionLog = Control.FechaIngresoLog;
                    model.TerminalModificacionLog = Control.TerminalIngresoLog;
                    model.UsuarioModificacionLog = Control.UsuarioIngresoLog;
                    db.SaveChanges();
                }

            }
        }
        #endregion
    }
}