using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.AnalisisAguaTanquero
{
    public class ClsdAnalisisAguaTanquero
    {

        public List<CC_ANALISIS_AGUA_TANQUERO> ConsultaAnalisisAguaTanquero(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var query = (from x in entities.CC_ANALISIS_AGUA_TANQUERO_CONTROL
                             join y in entities.CC_ANALISIS_AGUA_TANQUERO on x.IdAnalisisAguaTanqueroControl equals y.IdAnalisisAguaTanqueroControl
                             where x.Fecha == Fecha && y.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                             && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                             select y);

                return query.ToList();
            }
        }


        public void GuardarModificarAnalisisAguaTanquero(CC_ANALISIS_AGUA_TANQUERO model, DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    CC_ANALISIS_AGUA_TANQUERO_CONTROL poControlReporte = entities.CC_ANALISIS_AGUA_TANQUERO_CONTROL.FirstOrDefault(x => x.Fecha == Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    int idControl = 0;
                    if (poControlReporte != null)
                    {
                        idControl = poControlReporte.IdAnalisisAguaTanqueroControl;
                    }
                    else
                    {
                        CC_ANALISIS_AGUA_TANQUERO_CONTROL control = new CC_ANALISIS_AGUA_TANQUERO_CONTROL();
                        control.Fecha = Fecha;
                        control.EstadoReporte = false;
                        control.FechaIngresoLog = model.FechaIngresoLog;
                        control.TerminalIngresoLog = model.TerminalIngresoLog;
                        control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                        control.EstadoReporte = false;
                        control.UsuarioIngresoLog = model.UsuarioIngresoLog;
                        entities.CC_ANALISIS_AGUA_TANQUERO_CONTROL.Add(control);
                        entities.SaveChanges();
                        idControl = control.IdAnalisisAguaTanqueroControl;

                    }
                    var poControl = entities.CC_ANALISIS_AGUA_TANQUERO.FirstOrDefault(x => x.IdAnalisisAguaTanquero == model.IdAnalisisAguaTanquero);
                    if (poControl != null)
                    {
                        poControl.Observacion = string.IsNullOrEmpty(model.Observacion)? model.Observacion : model.Observacion.ToUpper();
                        poControl.Hora = model.Hora;
                        poControl.Placa = model.Placa.ToUpper();
                        poControl.Std = model.Std;
                        poControl.Dureza = model.Dureza;
                        poControl.Destino = model.Destino.ToUpper();
                        poControl.Ph = model.Ph;
                        poControl.Olor = model.Olor;
                        poControl.Color = model.Color;
                        poControl.Sabor = model.Sabor;
                        poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                        poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                        poControl.FechaModificacionLog = model.FechaIngresoLog;
                    }
                    else
                    {
                        model.Destino = model.Destino.ToUpper();
                        model.Placa = model.Placa.ToUpper();
                        model.Observacion = string.IsNullOrEmpty(model.Observacion) ? model.Observacion : model.Observacion.ToUpper();
                        model.IdAnalisisAguaTanqueroControl = idControl;
                        entities.CC_ANALISIS_AGUA_TANQUERO.Add(model);
                    }
                    entities.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public void EliminarAnalisisAguaTanquero(CC_ANALISIS_AGUA_TANQUERO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_ANALISIS_AGUA_TANQUERO.FirstOrDefault(x => x.IdAnalisisAguaTanquero == model.IdAnalisisAguaTanquero);
                if (poControl != null)
                {

                    var poControl1 = entities.CC_ANALISIS_AGUA_TANQUERO.Count(x => x.IdAnalisisAguaTanqueroControl == poControl.IdAnalisisAguaTanqueroControl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    var poControl2 = entities.CC_ANALISIS_AGUA_TANQUERO_CONTROL.FirstOrDefault(x => x.IdAnalisisAguaTanqueroControl == poControl.IdAnalisisAguaTanqueroControl);
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
        public List<CC_ANALISIS_AGUA_TANQUERO_CONTROL> ConsultaAnalisisAguaTanqueroControl(DateTime FechaDesde, DateTime FechaHasta, bool Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_ANALISIS_AGUA_TANQUERO_CONTROL.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                && x.EstadoReporte == Estado).ToList();
            }
        }

        public List<CC_ANALISIS_AGUA_TANQUERO_CONTROL> ConsultaAnalisisAguaTanqueroControl(DateTime FechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_ANALISIS_AGUA_TANQUERO_CONTROL.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                               ).ToList();
            }
        }

        public List<CC_ANALISIS_AGUA_TANQUERO_CONTROL> ConsultaAnalisisAguaTanqueroControl(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_ANALISIS_AGUA_TANQUERO_CONTROL.Where(x => x.Fecha == Fecha
                                                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }

        public List<CC_ANALISIS_AGUA_TANQUERO_CONTROL> ConsultaAnalisisAguaTanqueroControlPendiente()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_ANALISIS_AGUA_TANQUERO_CONTROL.Where(x => !x.EstadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }
        public void Aprobar_ReporteAnalisisAguaTanquero(CC_ANALISIS_AGUA_TANQUERO_CONTROL controlCloro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_ANALISIS_AGUA_TANQUERO_CONTROL.FirstOrDefault(x => x.IdAnalisisAguaTanqueroControl == controlCloro.IdAnalisisAguaTanqueroControl || (x.Fecha == controlCloro.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
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