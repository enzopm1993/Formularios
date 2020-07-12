using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ResidualCloro
{
    public class clsDResidualCloro
    {
        public List<spConsultaResidualCloro> ConsultaResidualCloro(DateTime Fecha, string Area)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaResidualCloro(Fecha, Area).ToList();
                return lista;
            }
        }
        public RESIDUAL_CLORO_CONTROL ConsultaResidualCloroControl(DateTime Fecha, string Area, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.RESIDUAL_CLORO_CONTROL.Where(x=> x.Fecha== Fecha 
                && x.Area == Area
                && x.Turno == Turno
                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                return lista;
            }
        }


        public void GuardarModificarResidualCloroControl(RESIDUAL_CLORO model, DateTime Fecha,string Area, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    RESIDUAL_CLORO_CONTROL poControlReporte = entities.RESIDUAL_CLORO_CONTROL.FirstOrDefault(x => x.Fecha == Fecha
                                                                                                                                && x.Area== Area
                                                                                                                                && x.Turno == Turno
                                                                                                                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    int idControl = 0;
                    if (poControlReporte != null)
                    {
                        idControl = poControlReporte.IdResidualCloroControl;
                    }
                    else
                    {
                        RESIDUAL_CLORO_CONTROL control = new RESIDUAL_CLORO_CONTROL();
                        control.Fecha = Fecha;
                        control.Turno = Turno;
                        control.Area = Area;
                        control.EstadoReporte = false;
                        control.FechaIngresoLog = model.FechaIngresoLog;
                        control.TerminalIngresoLog = model.TerminalIngresoLog;
                        control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                        control.EstadoReporte = false;
                        control.UsuarioIngresoLog = model.UsuarioIngresoLog;
                        entities.RESIDUAL_CLORO_CONTROL.Add(control);
                        entities.SaveChanges();
                        idControl = control.IdResidualCloroControl;

                    }
                    var poControl = entities.RESIDUAL_CLORO.FirstOrDefault(x => x.IdResidualCloro == model.IdResidualCloro);
                    if (poControl != null)
                    {
                        poControl.Observacion = model.Observacion;
                        poControl.Hora = model.Hora;
                        poControl.Observacion = model.Observacion;
                        poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                        poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                        poControl.FechaModificacionLog = model.FechaIngresoLog;
                    }
                    else
                    {
                        model.IdResidualCloroControl = idControl;
                        entities.RESIDUAL_CLORO.Add(model);
                    }
                    entities.SaveChanges();
                    transaction.Commit();
                }
            }
        }


        public void GuardarModificarResidualCloro(RESIDUAL_CLORO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.RESIDUAL_CLORO.FirstOrDefault(x => x.IdResidualCloro == model.IdResidualCloro);
                if (poControl != null)
                {
                    poControl.Observacion = model.Observacion;
                    poControl.Hora = model.Hora;                  
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.RESIDUAL_CLORO.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarResidualCloro(RESIDUAL_CLORO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.RESIDUAL_CLORO.FirstOrDefault(x => x.IdResidualCloro == model.IdResidualCloro);
                if (poControl != null)
                {
                    var poControl1 = entities.RESIDUAL_CLORO.Count(x => x.IdResidualCloroControl == poControl.IdResidualCloroControl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    var poControl2 = entities.RESIDUAL_CLORO_CONTROL.FirstOrDefault(x => x.IdResidualCloroControl == poControl.IdResidualCloroControl);
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

        public List<spConsultaResidualCloroDetalle> ConsultaResidualCloroDetalle(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaResidualCloroDetalle(IdControl).ToList();
                return lista;
            }
        }

        public void GuardarModificarResidualCloroDetalle(RESIDUAL_CLORO_DETALLE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.RESIDUAL_CLORO_DETALLE.FirstOrDefault(x => x.IdResidualCloroDetalle == model.IdResidualCloroDetalle || (x.IdResidualCloro == model.IdResidualCloro && x.CodPeliduvio==model.CodPeliduvio && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
                if (poControl != null)
                {
                    poControl.CodPeliduvio = model.CodPeliduvio;
                    poControl.Cantidad = model.Cantidad;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.RESIDUAL_CLORO_DETALLE.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarResidualCloroDetalle(RESIDUAL_CLORO_DETALLE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.RESIDUAL_CLORO_DETALLE.FirstOrDefault(x => x.IdResidualCloroDetalle == model.IdResidualCloroDetalle);
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

        public List<spReporteResidualCloro> ConsultaReporteResidualCloro(DateTime Fecha, string Area)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spReporteResidualCloro(Fecha, Area).ToList();
                return lista;
            }
        }

        public List<RESIDUAL_CLORO_CONTROL> ConsultaResidualCloroControl(DateTime FechaDesde,DateTime FechaHasta, bool Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.RESIDUAL_CLORO_CONTROL.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                && x.EstadoReporte==Estado).ToList();
            }
        }

        public List<RESIDUAL_CLORO_CONTROL> ConsultaResidualCloroControl(DateTime FechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.RESIDUAL_CLORO_CONTROL.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                               ).ToList();
            }
        }

        public List<RESIDUAL_CLORO_CONTROL> ValidaResidualCloroControl(DateTime Fecha, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.RESIDUAL_CLORO_CONTROL.Where(x => x.Fecha == Fecha
                                                                && x.Turno ==Turno
                                                                && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();
            }
        }

        public List<RESIDUAL_CLORO_CONTROL> ConsultaResidualCloroControlPendiente()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.RESIDUAL_CLORO_CONTROL.Where(x => !x.EstadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }
        public void Aprobar_ReporteResidualCloro(RESIDUAL_CLORO_CONTROL controlCloro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.RESIDUAL_CLORO_CONTROL.FirstOrDefault(x => x.IdResidualCloroControl == controlCloro.IdResidualCloroControl || (x.Fecha == controlCloro.Fecha && x.Area == controlCloro.Area&& x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
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