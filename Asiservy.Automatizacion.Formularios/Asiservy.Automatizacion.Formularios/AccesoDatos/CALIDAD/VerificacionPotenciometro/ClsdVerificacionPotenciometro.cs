using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.VerificacionPotenciometro
{
    public class ClsdVerificacionPotenciometro
    {
        public List<CC_VERIFICACION_POTENCIOMETRO> ConsultaVerificacionPotenciometro(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var query = entities.CC_VERIFICACION_POTENCIOMETRO.Where(x=> x.Fecha==Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();

                return query;
            }
        }


        public void GuardarModificarVerificacionPotenciometro(CC_VERIFICACION_POTENCIOMETRO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    CC_VERIFICACION_POTENCIOMETRO poControlReporte = entities.CC_VERIFICACION_POTENCIOMETRO.FirstOrDefault(x => x.Fecha == model.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    if (poControlReporte != null)
                    {
                        poControlReporte.NaCI1 = model.NaCI1;
                        poControlReporte.NaCI2 = model.NaCI2;
                        poControlReporte.NaCI3 = model.NaCI3;
                        poControlReporte.Observacion = model.Observacion;
                        poControlReporte.TerminalModificacionLog = model.TerminalIngresoLog;
                        poControlReporte.UsuarioModificacionLog = model.UsuarioIngresoLog;
                        poControlReporte.FechaModificacionLog = model.FechaIngresoLog;
                        poControlReporte.Modelo = model.Modelo;
                        poControlReporte.Serie = model.Serie;
                    }
                    else
                    {
                        entities.CC_VERIFICACION_POTENCIOMETRO.Add(model);
                    }
                   
                    entities.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public void EliminarVerificacionPotenciometro(CC_VERIFICACION_POTENCIOMETRO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_VERIFICACION_POTENCIOMETRO.FirstOrDefault(x => x.Fecha == model.Fecha && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
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
        public List<CC_VERIFICACION_POTENCIOMETRO> ConsultaVerificacionPotenciometroControl(DateTime FechaDesde, DateTime FechaHasta, bool Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_VERIFICACION_POTENCIOMETRO.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                && x.EstadoReporte == Estado).ToList();
            }
        }

        public List<CC_VERIFICACION_POTENCIOMETRO> ConsultaVerificacionPotenciometroControl(DateTime FechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_VERIFICACION_POTENCIOMETRO.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                         && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                               ).ToList();
            }
        }

        public List<CC_VERIFICACION_POTENCIOMETRO> ConsultaVerificacionPotenciometroControl(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_VERIFICACION_POTENCIOMETRO.Where(x => x.Fecha == Fecha
                                                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }

        public List<CC_VERIFICACION_POTENCIOMETRO> ConsultaVerificacionPotenciometroControlPendiente()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_VERIFICACION_POTENCIOMETRO.Where(x => !x.EstadoReporte).ToList();
            }
        }
        public void Aprobar_ReporteVerificacionPotenciometro(CC_VERIFICACION_POTENCIOMETRO controlCloro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_VERIFICACION_POTENCIOMETRO.FirstOrDefault(x => x.IdVerificacionPotenciometroControl == controlCloro.IdVerificacionPotenciometroControl || (x.Fecha == controlCloro.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
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