using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.TemperaturaInternaRefrigeradora
{
    public class ClsdTemperaturaInternaRefrigeradora
    {

        public List<CC_TEMPERATURA_INTERNA_REFRIGERADORA> ConsultaTemperaturaInternaRefrigeradora(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var query = (from x in entities.CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL
                             join y in entities.CC_TEMPERATURA_INTERNA_REFRIGERADORA on x.IdTemperaturaInternaRefrigeradoraControl equals y.IdTemperaturaInternaRefrigeradoraControl
                             where x.Fecha == Fecha && y.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                             && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                             select y);

                return query.ToList();
            }
        }


        public void GuardarModificarTemperaturaInternaRefrigeradora(CC_TEMPERATURA_INTERNA_REFRIGERADORA model, DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL poControlReporte = entities.CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL.FirstOrDefault(x => x.Fecha == Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    int idControl = 0;
                    if (poControlReporte != null)
                    {
                        idControl = poControlReporte.IdTemperaturaInternaRefrigeradoraControl;
                    }
                    else
                    {
                        CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL control = new CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL();
                        control.Fecha = Fecha;
                        control.EstadoReporte = false;
                        control.FechaIngresoLog = model.FechaIngresoLog;
                        control.TerminalIngresoLog = model.TerminalIngresoLog;
                        control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                        control.EstadoReporte = false;
                        control.UsuarioIngresoLog = model.UsuarioIngresoLog;
                        entities.CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL.Add(control);
                        entities.SaveChanges();
                        idControl = control.IdTemperaturaInternaRefrigeradoraControl;

                    }
                    var poControl = entities.CC_TEMPERATURA_INTERNA_REFRIGERADORA.FirstOrDefault(x => x.IdTemperaturaInternaRefrigeradora == model.IdTemperaturaInternaRefrigeradora);
                    if (poControl != null)
                    {
                        poControl.Observacion = model.Observacion;
                        poControl.Hora = model.Hora;
                        poControl.Temperatura = model.Temperatura;
                        poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                        poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                        poControl.FechaModificacionLog = model.FechaIngresoLog;
                    }
                    else
                    {
                        model.IdTemperaturaInternaRefrigeradoraControl = idControl;
                        entities.CC_TEMPERATURA_INTERNA_REFRIGERADORA.Add(model);
                    }
                    entities.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public void EliminarTemperaturaInternaRefrigeradora(CC_TEMPERATURA_INTERNA_REFRIGERADORA model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_TEMPERATURA_INTERNA_REFRIGERADORA.FirstOrDefault(x => x.IdTemperaturaInternaRefrigeradora == model.IdTemperaturaInternaRefrigeradora);
                if (poControl != null)
                {
                    var poControl1 = entities.CC_TEMPERATURA_INTERNA_REFRIGERADORA.Count(x => x.IdTemperaturaInternaRefrigeradoraControl == poControl.IdTemperaturaInternaRefrigeradoraControl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    var poControl2 = entities.CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL.FirstOrDefault(x => x.IdTemperaturaInternaRefrigeradoraControl == poControl.IdTemperaturaInternaRefrigeradoraControl);
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
        public List<CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL> ConsultaTemperaturaInternaRefrigeradoraControl(DateTime FechaDesde, DateTime FechaHasta, bool Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                && x.EstadoReporte == Estado).ToList();
            }
        }

        public List<spReporteTemperaturaInternaRefrigeradora> ConsultaTemperaturaInternaRefrigeradoraControl(DateTime FechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spReporteTemperaturaInternaRefrigeradora(FechaDesde,FechaHasta).ToList();
            }
        }

        public List<CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL> ConsultaTemperaturaInternaRefrigeradoraControl(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL.Where(x => x.Fecha == Fecha
                                                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }

        public List<CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL> ConsultaTemperaturaInternaRefrigeradoraControlPendiente()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL.Where(x => !x.EstadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }
        public void Aprobar_ReporteTemperaturaInternaRefrigeradora(CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL controlCloro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_TEMPERATURA_INTERNA_REFRIGERADORA_CONTROL.FirstOrDefault(x => x.IdTemperaturaInternaRefrigeradoraControl == controlCloro.IdTemperaturaInternaRefrigeradoraControl || (x.Fecha == controlCloro.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
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