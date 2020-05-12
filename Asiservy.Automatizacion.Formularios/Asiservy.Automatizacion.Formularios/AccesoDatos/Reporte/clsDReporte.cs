﻿using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Reporte
{
    public class clsDReporte
    {

        public List<spConsultaReporteMaestro> ConsultaReporteMaestro()
        {
            using (Datos.Datos.ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaReporteMaestro().ToList();
                return lista;
            }
        }


        public spConsultaCodigoReporte ConsultaCodigoReporte(string Nombre)
        {
            using (Datos.Datos.ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaCodigoReporte(Nombre).FirstOrDefault();
                return lista;
            }
        }

        public void GuardarModificarReporteMaestro(REPORTE_MAESTRO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poReporte = entities.REPORTE_MAESTRO.FirstOrDefault(x => x.IdReporteMaestro == model.IdReporteMaestro);
                if (poReporte != null)
                {
                    poReporte.EstadoRegistro = model.EstadoRegistro;
                    poReporte.Nombre = model.Nombre;
                    poReporte.Codigo = model.Codigo;
                    poReporte.IdOpcion = model.IdOpcion;
                    poReporte.TerminalModificacionLog = model.TerminalIngresoLog;
                    poReporte.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poReporte.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.REPORTE_MAESTRO.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarReporteMaestro(REPORTE_MAESTRO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poReporte = entities.REPORTE_MAESTRO.FirstOrDefault(x => x.IdReporteMaestro == model.IdReporteMaestro);
                if (poReporte != null)
                {
                    poReporte.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    poReporte.TerminalModificacionLog = model.TerminalIngresoLog;
                    poReporte.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poReporte.FechaModificacionLog = model.FechaIngresoLog;
                    entities.SaveChanges();
                }

            }
        }


        public List<spConsultaReporteDetalle> ConsultaReporteDetalle(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaReporteDetalle(IdControl).ToList();
                return lista;
            }
        }

        public void ActualuzarUltimaVersion(int Id, string Version)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poReporte = entities.REPORTE_MAESTRO.FirstOrDefault(x => x.IdReporteMaestro == Id);
                if (poReporte != null)
                {
                    poReporte.UltimaVersion = Version;
                    entities.SaveChanges();
                }
            }
        }

        public void GuardarModificarReporteMaestroDetalle(REPORTE_DETALLE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poReporte = entities.REPORTE_DETALLE.FirstOrDefault(x => x.IdReporteDetalle == model.IdReporteDetalle);
                if (poReporte != null)
                {
                    poReporte.Version = model.Version;
                    if(!string.IsNullOrEmpty(model.Imagen))
                        poReporte.Imagen = model.Imagen;
                    poReporte.Rotacion = model.Rotacion;

                    poReporte.TerminalModificacionLog = model.TerminalIngresoLog;
                    poReporte.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poReporte.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.REPORTE_DETALLE.Add(model);
                }
                entities.SaveChanges();
            }
        }
        public void EliminarReporteMaestroDetalle(REPORTE_DETALLE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poReporte = entities.REPORTE_DETALLE.FirstOrDefault(x => x.IdReporteDetalle == model.IdReporteDetalle);
                if (poReporte != null)
                {
                    poReporte.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    poReporte.TerminalModificacionLog = model.TerminalIngresoLog;
                    poReporte.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poReporte.FechaModificacionLog = model.FechaIngresoLog;
                    entities.SaveChanges();
                }

            }
        }

    }
}