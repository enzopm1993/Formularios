using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.CondicionPersonal
{
    public class clsDCondicionPersonal
    {
        public List<MANTENIMIENTO_CONDICION> ConsultaManteminetoCondicion()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.MANTENIMIENTO_CONDICION.ToList();
                return lista;
            }
        }

        public void GuardarModificarMantenimientoCondicion(MANTENIMIENTO_CONDICION model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.MANTENIMIENTO_CONDICION.FirstOrDefault(x => x.IdMantenimientoCondicion == model.IdMantenimientoCondicion);
                if (poControl != null)
                {
                    poControl.Descripcion = model.Descripcion;
                    // poControl.Hora = model.Hora;
                    poControl.EstadoRegistro = model.EstadoRegistro;

                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.MANTENIMIENTO_CONDICION.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarMantenimientoCondicion(MANTENIMIENTO_CONDICION model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.MANTENIMIENTO_CONDICION.FirstOrDefault(x => x.IdMantenimientoCondicion == model.IdMantenimientoCondicion);
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


        public List<spConsultaCondicionesPersonal> ConsultaCondicionPersonal(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaCondicionesPersonal(Fecha).ToList();
                return lista;
            }
        }
     

        public void GuardarModificarCondicionPersonal(CC_CONDICION_PERSONAL model,DateTime Fecha, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    CC_CONDICION_PERSONAL_CONTROL poControlReporte = entities.CC_CONDICION_PERSONAL_CONTROL.FirstOrDefault(x => x.Fecha == Fecha
                                                                                                                                && x.Turno == Turno
                                                                                                                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    int idControl = 0;
                    if (poControlReporte != null)
                    {
                        idControl = poControlReporte.IdCondicionPersonalControl;
                    }
                    else
                    {
                        CC_CONDICION_PERSONAL_CONTROL control = new CC_CONDICION_PERSONAL_CONTROL();
                        control.Fecha = Fecha;
                        control.Turno = Turno;
                        control.EstadoReporte = false;
                        control.FechaIngresoLog = model.FechaIngresoLog;
                        control.TerminalIngresoLog = model.TerminalIngresoLog;
                        control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                        control.EstadoReporte = false;
                        control.UsuarioIngresoLog = model.UsuarioIngresoLog;
                        entities.CC_CONDICION_PERSONAL_CONTROL.Add(control);
                        entities.SaveChanges();
                        idControl = control.IdCondicionPersonalControl;

                    }
                    var poControl = entities.CC_CONDICION_PERSONAL.FirstOrDefault(x => x.IdCondicionPersonal == model.IdCondicionPersonal);
                    if (poControl != null)
                    {
                        poControl.Observacion = model.Observacion;
                        poControl.Hora = model.Hora;
                        poControl.Cedula = model.Cedula;
                        poControl.CodCondicion = model.CodCondicion;
                        poControl.Observacion = model.Observacion;
                        poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                        poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                        poControl.FechaModificacionLog = model.FechaIngresoLog;
                    }
                    else
                    {
                        model.IdCondicionPersonalControl = idControl;
                        entities.CC_CONDICION_PERSONAL.Add(model);
                    }
                    entities.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public void EliminarCondicionPersonal(CC_CONDICION_PERSONAL model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_CONDICION_PERSONAL.FirstOrDefault(x => x.IdCondicionPersonal == model.IdCondicionPersonal);
                if (poControl != null)
                {
                    var poControl1 = entities.CC_CONDICION_PERSONAL.Count(x => x.IdCondicionPersonalControl == poControl.IdCondicionPersonalControl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    var poControl2 = entities.CC_CONDICION_PERSONAL_CONTROL.FirstOrDefault(x => x.IdCondicionPersonalControl == poControl.IdCondicionPersonalControl);
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
        public List<CC_CONDICION_PERSONAL_CONTROL> ConsultaCondicionPersonalControl(DateTime FechaDesde,DateTime FechaHasta, bool Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_CONDICION_PERSONAL_CONTROL.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                && x.EstadoReporte==Estado).ToList();
            }
        }

        public List<CC_CONDICION_PERSONAL_CONTROL> ConsultaCondicionPersonalControl(DateTime FechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_CONDICION_PERSONAL_CONTROL.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                               ).ToList();
            }
        }

        public List<CC_CONDICION_PERSONAL_CONTROL> ConsultaCondicionPersonalControl(DateTime Fecha, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_CONDICION_PERSONAL_CONTROL.Where(x => x.Fecha == Fecha
                                                                && x.Turno==Turno
                                                                && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();
            }
        }

        public List<CC_CONDICION_PERSONAL_CONTROL> ConsultaCondicionPersonalControlPendiente()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_CONDICION_PERSONAL_CONTROL.Where(x => !x.EstadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }
        public void Aprobar_ReporteCondicionPersonal(CC_CONDICION_PERSONAL_CONTROL control)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CONDICION_PERSONAL_CONTROL.FirstOrDefault(x => x.IdCondicionPersonalControl == control.IdCondicionPersonalControl || (x.Fecha == control.Fecha&& x.Turno == control.Turno && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
                if (model != null)
                {
                    model.EstadoReporte = control.EstadoReporte;
                    model.AprobadoPor = control.AprobadoPor;
                    model.FechaAprobacion = control.FechaAprobacion;
                    model.FechaModificacionLog = control.FechaIngresoLog;
                    model.TerminalModificacionLog = control.TerminalIngresoLog;
                    model.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    db.SaveChanges();
                }
               
            }
        }
    }
}