using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MonitoreoDescongelado
{
    public class clsDMonitoreoDescongelado
    {
        public List<spConsultaMonitoreoDescongelado> ConsultaMonitoreoDescongelado(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaMonitoreoDescongelado(Fecha).ToList();
                return lista;
            }
        }

        public List<spConsultaMonitoreoDescongeladoDetalle> ConsultaMonitoreoDescongelado(DateTime Fecha, string Tanque, string Lote,int Tipo, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<spConsultaMonitoreoDescongeladoDetalle> consulta = null;
                var model = (from x in entities.CC_MONITOREO_DESCONGELADO
                             join y in entities.CC_MONITOREO_DESCONGELADO_CONTROL on x.IdMonitoreoDescongeladoControl equals y.IdMonitoreoDescongeladoControl
                             where x.Fecha == Fecha
                             && x.Tanque == Tanque
                             && x.IdTipoMonitoreo == Tipo
                             && x.Lote == Lote
                             && y.Turno == Turno
                             && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                             select x
                            ).FirstOrDefault();
                if (model != null)
                {
                    consulta=  entities.spConsultaMonitoreoDescongeladoDetalle(model.IdMonitoreoDescongelado).ToList();
                }
                return consulta;
            }
        }

        public void GuardarModificarMonitoreoDescongelado(CC_MONITOREO_DESCONGELADO model, List<CC_MONITOREO_DESCONGELADO_DETALLE> detalle, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    CC_MONITOREO_DESCONGELADO_CONTROL poControlReporte = entities.CC_MONITOREO_DESCONGELADO_CONTROL.FirstOrDefault(x => x.Fecha == model.Fecha 
                    && x.Turno==Turno 
                    && x.EstadoRegistro== clsAtributos.EstadoRegistroActivo);
                    int idControl = 0;
                    int idCabecera = 0;
                    if (poControlReporte != null)
                    {
                        idControl = poControlReporte.IdMonitoreoDescongeladoControl;
                    }
                    else
                    {
                        CC_MONITOREO_DESCONGELADO_CONTROL control = new CC_MONITOREO_DESCONGELADO_CONTROL();
                        control.Fecha = model.Fecha;
                        control.Turno = Turno;
                        control.EstadoReporte = false;
                        control.FechaIngresoLog = model.FechaIngresoLog;
                        control.TerminalIngresoLog = model.TerminalIngresoLog;
                        control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                        control.UsuarioIngresoLog = model.UsuarioIngresoLog;
                        entities.CC_MONITOREO_DESCONGELADO_CONTROL.Add(control);
                        entities.SaveChanges();
                        idControl = control.IdMonitoreoDescongeladoControl;

                    }
                    var poControl = entities.CC_MONITOREO_DESCONGELADO.FirstOrDefault(x => x.IdMonitoreoDescongelado == model.IdMonitoreoDescongelado);
                    if (poControl != null)
                    {
                        poControl.Hora = model.Hora;
                        //poControl.Muestra1 = model.Muestra1;
                        //poControl.Muestra2 = model.Muestra2;
                        poControl.TemperaturaAgua = model.TemperaturaAgua;
                        //poControl.Muestra3 = model.Muestra3;
                        poControl.Observacion = !string.IsNullOrEmpty(model.Observacion)? model.Observacion.ToUpper():model.Observacion;
                        poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                        poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                        poControl.FechaModificacionLog = model.FechaIngresoLog;
                        idCabecera = poControl.IdMonitoreoDescongelado;
                    }
                    else
                    {
                        model.Observacion = !string.IsNullOrEmpty(model.Observacion) ? model.Observacion.ToUpper() : model.Observacion;
                        model.IdMonitoreoDescongeladoControl = idControl;
                        entities.CC_MONITOREO_DESCONGELADO.Add(model);
                        entities.SaveChanges();
                        idCabecera = model.IdMonitoreoDescongelado;
                    }
                    foreach(var d in detalle)
                    {
                        //var poMuestra = entities.CC_MANTENIMIENTO_MUESTRA_DESCONGELADO.FirstOrDefault(x=> x.IdMuestra = d.cod);
                        var poControlDetalle = entities.CC_MONITOREO_DESCONGELADO_DETALLE.FirstOrDefault(x => x.IdMonitoreoDescongelado == idCabecera && x.IdMuestra == d.IdMuestra && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
                        if (poControlDetalle != null)
                        {
                            poControlDetalle.Cantidad = d.Cantidad;
                            poControlDetalle.TerminalModificacionLog = model.TerminalIngresoLog;
                            poControlDetalle.UsuarioModificacionLog = model.UsuarioIngresoLog;
                            poControlDetalle.FechaModificacionLog = model.FechaIngresoLog;
                        }
                        else
                        {
                            d.IdMonitoreoDescongelado = idCabecera;
                            d.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                            d.UsuarioIngresoLog = model.UsuarioIngresoLog;
                            d.FechaIngresoLog = model.FechaIngresoLog;
                            d.TerminalIngresoLog = model.TerminalIngresoLog;
                            entities.CC_MONITOREO_DESCONGELADO_DETALLE.Add(d);
                        }
                    }



                    entities.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public void EliminarMonitoreoDescongelado(CC_MONITOREO_DESCONGELADO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MONITOREO_DESCONGELADO.FirstOrDefault(x => x.IdMonitoreoDescongelado == model.IdMonitoreoDescongelado);
                if (poControl != null)
                {
                     var poControl1 = entities.CC_MONITOREO_DESCONGELADO.Count(x => x.IdMonitoreoDescongeladoControl == poControl.IdMonitoreoDescongeladoControl && x.EstadoRegistro== clsAtributos.EstadoRegistroActivo);
                    var poControl2 = entities.CC_MONITOREO_DESCONGELADO_CONTROL.FirstOrDefault(x => x.IdMonitoreoDescongeladoControl == poControl.IdMonitoreoDescongeladoControl);
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
        public List<CC_MONITOREO_DESCONGELADO_CONTROL> ConsultaMonitoreoDescongeladoControl(DateTime FechaDesde, DateTime FechaHasta, bool Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_MONITOREO_DESCONGELADO_CONTROL.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                && x.EstadoReporte == Estado).ToList();
            }
        }

        public List<CC_MONITOREO_DESCONGELADO_CONTROL> ConsultaMonitoreoDescongeladoControl(DateTime FechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_MONITOREO_DESCONGELADO_CONTROL.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                               ).ToList();
            }
        }

        public List<CC_MONITOREO_DESCONGELADO_CONTROL> ConsultaMonitoreoDescongeladoControl(DateTime Fecha, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_MONITOREO_DESCONGELADO_CONTROL.Where(x => x.Fecha == Fecha
                                                                && x.Turno == Turno
                                                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }

        public List<CC_MONITOREO_DESCONGELADO_CONTROL> ConsultaMonitoreoDescongeladoControlPendiente()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_MONITOREO_DESCONGELADO_CONTROL.Where(x => !x.EstadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }
        public void Aprobar_ReporteMonitoreoDescongelado(CC_MONITOREO_DESCONGELADO_CONTROL controlCloro, string Turno)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_MONITOREO_DESCONGELADO_CONTROL.FirstOrDefault(x => x.IdMonitoreoDescongeladoControl == controlCloro.IdMonitoreoDescongeladoControl || (x.Fecha == controlCloro.Fecha&&x.Turno==Turno && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
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