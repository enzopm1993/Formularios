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

        public CC_MONITOREO_DESCONGELADO ConsultaMonitoreoDescongelado(DateTime Fecha, string Tanque, string Lote,string Tipo)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var model = entities.CC_MONITOREO_DESCONGELADO.Where(x=> 
                            x.Fecha==Fecha
                            &&x.Tanque == Tanque 
                            && x.Tipo == Tipo
                            && x.Lote == Lote
                            && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (model != null)
                {
                    CC_MONITOREO_DESCONGELADO control = new CC_MONITOREO_DESCONGELADO
                    {
                        Especie = model.Especie,
                        EstadoRegistro = model.EstadoRegistro,
                        Fecha = model.Fecha,
                        FechaIngresoLog = model.FechaIngresoLog,
                        FechaModificacionLog = model.FechaModificacionLog,
                        Hora = model.Hora,
                        IdMonitoreoDescongelado = model.IdMonitoreoDescongelado,
                        IdMonitoreoDescongeladoControl = model.IdMonitoreoDescongeladoControl,
                        Lote = model.Lote,
                        Muestra1 = model.Muestra1,
                        Muestra2 = model.Muestra2,
                        Muestra3 = model.Muestra3,
                        Talla = model.Talla,
                        Tanque = model.Tanque,
                        TemperaturaAgua = model.TemperaturaAgua,
                        TerminalIngresoLog = model.TerminalIngresoLog,
                        TerminalModificacionLog = model.TerminalModificacionLog,
                        Tipo = model.Tipo,
                        UsuarioIngresoLog = model.UsuarioIngresoLog,
                        UsuarioModificacionLog = model.UsuarioModificacionLog
                    };
                    return control;

                }
                else
                {
                    return null;
                }
            }
        }

        public void GuardarModificarMonitoreoDescongelado(CC_MONITOREO_DESCONGELADO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    CC_MONITOREO_DESCONGELADO_CONTROL poControlReporte = entities.CC_MONITOREO_DESCONGELADO_CONTROL.FirstOrDefault(x => x.Fecha == model.Fecha && x.EstadoRegistro== clsAtributos.EstadoRegistroActivo);
                    int idControl = 0;
                    if (poControlReporte != null)
                    {
                        idControl = poControlReporte.IdMonitoreoDescongeladoControl;
                    }
                    else
                    {
                        CC_MONITOREO_DESCONGELADO_CONTROL control = new CC_MONITOREO_DESCONGELADO_CONTROL();
                        control.Fecha = model.Fecha;
                        control.EstadoReporte = false;
                        control.FechaIngresoLog = model.FechaIngresoLog;
                        control.TerminalIngresoLog = model.TerminalIngresoLog;
                        control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                        control.EstadoReporte = false;
                        control.UsuarioIngresoLog = model.UsuarioIngresoLog;
                        entities.CC_MONITOREO_DESCONGELADO_CONTROL.Add(control);
                        entities.SaveChanges();
                        idControl = control.IdMonitoreoDescongeladoControl;

                    }
                    var poControl = entities.CC_MONITOREO_DESCONGELADO.FirstOrDefault(x => x.IdMonitoreoDescongelado == model.IdMonitoreoDescongelado);
                    if (poControl != null)
                    {
                        poControl.Hora = model.Hora;
                        poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                        poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                        poControl.FechaModificacionLog = model.FechaIngresoLog;
                    }
                    else
                    {
                        model.IdMonitoreoDescongeladoControl = idControl;
                        entities.CC_MONITOREO_DESCONGELADO.Add(model);
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
                    var poControl2 = entities.CC_MONITOREO_DESCONGELADO_CONTROL.FirstOrDefault(x => x.IdMonitoreoDescongeladoControl == poControl.IdMonitoreoDescongeladoControl);
                    if (poControl2 != null)
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

        public List<CC_MONITOREO_DESCONGELADO_CONTROL> ConsultaMonitoreoDescongeladoControl(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_MONITOREO_DESCONGELADO_CONTROL.Where(x => x.Fecha == Fecha
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
        public void Aprobar_ReporteMonitoreoDescongelado(CC_MONITOREO_DESCONGELADO_CONTROL controlCloro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_MONITOREO_DESCONGELADO_CONTROL.FirstOrDefault(x => x.IdMonitoreoDescongeladoControl == controlCloro.IdMonitoreoDescongeladoControl || (x.Fecha == controlCloro.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
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