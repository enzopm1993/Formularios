using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.CloroAguaAutoclave
{
    public class ClsDCloroAguaAutoclave
    {

        public List<CC_CLORO_AGUA_AUTOCLAVE> ConsultaCloroAguaAutoclave(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var query = (from x in entities.CC_CLORO_AGUA_AUTOCLAVE_CONTROL
                            join y in entities.CC_CLORO_AGUA_AUTOCLAVE on x.IdCloroAguaAutoclaveControl equals y.IdCloroAguaAutoclaveControl
                            where x.Fecha == Fecha && y.EstadoRegistro == clsAtributos.EstadoRegistroActivo 
                            && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo
                            select y);
 
                return query.ToList();
            }
        }


        public void GuardarModificarCloroAguaAutoclave(CC_CLORO_AGUA_AUTOCLAVE model, DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    CC_CLORO_AGUA_AUTOCLAVE_CONTROL poControlReporte = entities.CC_CLORO_AGUA_AUTOCLAVE_CONTROL.FirstOrDefault(x => x.Fecha == Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    int idControl = 0;
                    if (poControlReporte != null)
                    {
                        idControl = poControlReporte.IdCloroAguaAutoclaveControl;
                    }
                    else
                    {
                        CC_CLORO_AGUA_AUTOCLAVE_CONTROL control = new CC_CLORO_AGUA_AUTOCLAVE_CONTROL();
                        control.Fecha = Fecha;
                        control.EstadoReporte = false;
                        control.FechaIngresoLog = model.FechaIngresoLog;
                        control.TerminalIngresoLog = model.TerminalIngresoLog;
                        control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                        control.EstadoReporte = false;
                        control.UsuarioIngresoLog = model.UsuarioIngresoLog;
                        entities.CC_CLORO_AGUA_AUTOCLAVE_CONTROL.Add(control);
                        entities.SaveChanges();
                        idControl = control.IdCloroAguaAutoclaveControl;

                    }
                    var poControl = entities.CC_CLORO_AGUA_AUTOCLAVE.FirstOrDefault(x => x.IdCloroAguaAutoclave == model.IdCloroAguaAutoclave);
                    if (poControl != null)
                    {
                        poControl.Observacion = model.Observacion;
                        poControl.Hora = model.Hora;
                        poControl.Parada = model.Parada;
                        poControl.Autoclave = model.Autoclave;
                        poControl.Producto = model.Producto;
                        poControl.TipoConserva = model.TipoConserva;
                        poControl.Cloro = model.Cloro;
                        poControl.Temperatura = model.Temperatura;
                        poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                        poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                        poControl.FechaModificacionLog = model.FechaIngresoLog;
                    }
                    else
                    {
                        model.IdCloroAguaAutoclaveControl = idControl;
                        entities.CC_CLORO_AGUA_AUTOCLAVE.Add(model);
                    }
                    entities.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public void EliminarCloroAguaAutoclave(CC_CLORO_AGUA_AUTOCLAVE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_CLORO_AGUA_AUTOCLAVE.FirstOrDefault(x => x.IdCloroAguaAutoclave == model.IdCloroAguaAutoclave);
                if (poControl != null)
                {
                    var poControl1 = entities.CC_CLORO_AGUA_AUTOCLAVE.Count(x => x.IdCloroAguaAutoclaveControl == poControl.IdCloroAguaAutoclaveControl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    var poControl2 = entities.CC_CLORO_AGUA_AUTOCLAVE_CONTROL.FirstOrDefault(x => x.IdCloroAguaAutoclaveControl == poControl.IdCloroAguaAutoclaveControl);
                    if (poControl2 != null && poControl1==1)
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
        public List<CC_CLORO_AGUA_AUTOCLAVE_CONTROL> ConsultaCloroAguaAutoclaveControl(DateTime FechaDesde, DateTime FechaHasta, bool Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_CLORO_AGUA_AUTOCLAVE_CONTROL.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                && x.EstadoReporte == Estado).ToList();
            }
        }

        public List<CC_CLORO_AGUA_AUTOCLAVE_CONTROL> ConsultaCloroAguaAutoclaveControl(DateTime FechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_CLORO_AGUA_AUTOCLAVE_CONTROL.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                               ).ToList();
            }
        }

        public List<CC_CLORO_AGUA_AUTOCLAVE_CONTROL> ConsultaCloroAguaAutoclaveControl(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_CLORO_AGUA_AUTOCLAVE_CONTROL.Where(x => x.Fecha == Fecha
                                                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }

        public List<CC_CLORO_AGUA_AUTOCLAVE_CONTROL> ConsultaCloroAguaAutoclaveControlPendiente()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_CLORO_AGUA_AUTOCLAVE_CONTROL.Where(x => !x.EstadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }
        public void Aprobar_ReporteCloroAguaAutoclave(CC_CLORO_AGUA_AUTOCLAVE_CONTROL controlCloro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CLORO_AGUA_AUTOCLAVE_CONTROL.FirstOrDefault(x => x.IdCloroAguaAutoclaveControl == controlCloro.IdCloroAguaAutoclaveControl || (x.Fecha == controlCloro.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
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