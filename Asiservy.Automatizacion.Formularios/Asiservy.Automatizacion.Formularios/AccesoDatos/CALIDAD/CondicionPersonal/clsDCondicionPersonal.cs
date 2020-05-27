﻿using Asiservy.Automatizacion.Datos.Datos;
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
     

        public void GuardarModificarCondicionPersonal(CC_CONDICION_PERSONAL model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControlReporte = entities.CC_CONDICION_PERSONAL_CONTROL.FirstOrDefault(x => x.Fecha == model.Fecha && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
                if (poControlReporte == null)
                {
                    CC_CONDICION_PERSONAL_CONTROL control = new CC_CONDICION_PERSONAL_CONTROL();
                    control.Fecha = model.Fecha;
                    control.FechaIngresoLog = model.FechaIngresoLog;
                    control.TerminalIngresoLog = model.TerminalIngresoLog;
                    control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    control.EstadoReporte = false;
                    control.UsuarioIngresoLog = model.UsuarioIngresoLog;
                    entities.CC_CONDICION_PERSONAL_CONTROL.Add(control);
                }
                var poControl = entities.CC_CONDICION_PERSONAL.FirstOrDefault(x => x.IdCondicionPersonal == model.IdCondicionPersonal);
                if (poControl != null)
                {
                    poControl.Observacion = model.Observacion;
                    poControl.Hora = model.Hora;
                    poControl.CodCondicion = model.CodCondicion;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.CC_CONDICION_PERSONAL.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarCondicionPersonal(CC_CONDICION_PERSONAL model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_CONDICION_PERSONAL.FirstOrDefault(x => x.IdCondicionPersonal == model.IdCondicionPersonal);
                var poControl2 = entities.CC_CONDICION_PERSONAL_CONTROL.FirstOrDefault(x => x.Fecha == model.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (poControl2 != null)
                {
                    poControl2.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    poControl2.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl2.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl2.FechaModificacionLog = model.FechaIngresoLog;

                }
                if (poControl != null)
                {
                    poControl.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                entities.SaveChanges();


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

        public List<CC_CONDICION_PERSONAL_CONTROL> ConsultaCondicionPersonalControl(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_CONDICION_PERSONAL_CONTROL.Where(x => x.Fecha == Fecha
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
        public void Aprobar_ReporteCondicionPersonal(CC_CONDICION_PERSONAL_CONTROL controlCloro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CONDICION_PERSONAL_CONTROL.FirstOrDefault(x => x.IdCondicionPersonalControl == controlCloro.IdCondicionPersonalControl || (x.Fecha == controlCloro.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
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