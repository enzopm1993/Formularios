﻿using Asiservy.Automatizacion.Datos.Datos;
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
        public RESIDUAL_CLORO_CONTROL ConsultaResidualCloroControl(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.RESIDUAL_CLORO_CONTROL.Where(x=> x.Fecha==Fecha).FirstOrDefault();
                return lista;
            }
        }


        public void GuardarModificarResidualCloroControl(RESIDUAL_CLORO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                RESIDUAL_CLORO_CONTROL control = new RESIDUAL_CLORO_CONTROL();
                var poControl = entities.RESIDUAL_CLORO_CONTROL.FirstOrDefault(x => x.Fecha == model.Fecha);
                if (poControl == null)
                {
                    control.Fecha = model.Fecha;
                    control.EstadoReporte = clsAtributos.EstadoReportePendiente;
                    control.FechaIngresoLog = model.FechaIngresoLog;
                    control.TerminalIngresoLog = model.TerminalIngresoLog;
                    control.UsuarioIngresoLog = model.UsuarioIngresoLog;
                    control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    entities.RESIDUAL_CLORO_CONTROL.Add(control);
                    entities.SaveChanges();
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


    }
}