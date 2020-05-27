using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.OperatividadMetal
{
    public class clsDOperatividadMetal
    {
        public List<CC_OPERATIVIDAD_METAL> ConsultaOperatividadMetal(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_OPERATIVIDAD_METAL.Where(x =>
                            x.Fecha == Fecha
                            && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                return lista;
            }
        }

        public bool ValidaOpertatividadMetal(CC_OPERATIVIDAD_METAL model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_OPERATIVIDAD_METAL.FirstOrDefault(x => x.IdOperatividadMetal == model.IdOperatividadMetal
                                || (x.Fecha == model.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));

                if (poControl != null && poControl.EstadoReporte==clsAtributos.EstadoReporteActivo)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

            public void GuardarModificarOperatividadMetal(CC_OPERATIVIDAD_METAL model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_OPERATIVIDAD_METAL.FirstOrDefault(x => x.IdOperatividadMetal == model.IdOperatividadMetal);
                if (poControl != null)
                {
                    poControl.AceroInoxidable= model.AceroInoxidable;
                    poControl.DetectorMetal= model.DetectorMetal;
                    poControl.Ferroso= model.Ferroso;
                    poControl.Lomos= model.Lomos;
                    poControl.Latas = model.Latas;
                    poControl.NoFerroso= model.NoFerroso;
                    poControl.Observacion= model.Observacion;
                    poControl.Pcc= model.Pcc;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.CC_OPERATIVIDAD_METAL.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarOperatividadMetal(CC_OPERATIVIDAD_METAL model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_OPERATIVIDAD_METAL.FirstOrDefault(x => x.IdOperatividadMetal == model.IdOperatividadMetal);
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


        public List<CC_OPERATIVIDAD_METAL_DETALLE> ConsultaOperatividadMetalDetalle(int idControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_OPERATIVIDAD_METAL_DETALLE.Where(x =>
                            x.IdOperatividadMetal == idControl
                            && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                return lista;
            }
        }


        public void GuardarModificarOperatividadMetalDetalle(CC_OPERATIVIDAD_METAL_DETALLE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_OPERATIVIDAD_METAL_DETALLE.FirstOrDefault(x =>x.IdOperatividadMetal==model.IdOperatividadMetal && x.IdOperatividadMetalDetalle == model.IdOperatividadMetalDetalle);
                if (poControl != null)
                {
                    poControl.AceroInoxidable = model.AceroInoxidable;
                    poControl.Ferroso = model.Ferroso;
                    poControl.NoFerroso = model.NoFerroso;
                    poControl.Observacion = model.Observacion;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.CC_OPERATIVIDAD_METAL_DETALLE.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarOperatividadMetalDetalle(CC_OPERATIVIDAD_METAL_DETALLE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_OPERATIVIDAD_METAL_DETALLE.FirstOrDefault(x => x.IdOperatividadMetalDetalle == model.IdOperatividadMetalDetalle);
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



        public List<CC_OPERATIVIDAD_DETECTOR_METAL> ConsultaOperatividadMetalDetector(int idControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_OPERATIVIDAD_DETECTOR_METAL.Where(x =>
                            x.IdOperatividadMetal == idControl
                            && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                return lista;
            }
        }


        public void GuardarModificarOperatividadMetalDetector(CC_OPERATIVIDAD_DETECTOR_METAL model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_OPERATIVIDAD_DETECTOR_METAL.FirstOrDefault(x => x.IdOperatividadDetectorMetal == model.IdOperatividadDetectorMetal);
                if (poControl != null)
                {
                    poControl.Novedad = model.Novedad;
                    if(!string.IsNullOrEmpty(model.Imagen))
                        poControl.Imagen = model.Imagen;
                    poControl.Rotacion = model.Rotacion;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.CC_OPERATIVIDAD_DETECTOR_METAL.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarOperatividadMetalDetector(CC_OPERATIVIDAD_DETECTOR_METAL model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_OPERATIVIDAD_DETECTOR_METAL.FirstOrDefault(x => x.IdOperatividadDetectorMetal == model.IdOperatividadDetectorMetal);
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

        public List<CC_OPERATIVIDAD_METAL> ConsultaOperatividadMetalControl(DateTime FechaDesde, DateTime FechaHasta, bool Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_OPERATIVIDAD_METAL.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                && x.EstadoReporte == Estado
                                                                && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();
            }
        }

        public List<CC_OPERATIVIDAD_METAL> ConsultaOperatividadMetalControl(DateTime FechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_OPERATIVIDAD_METAL.Where(x => x.Fecha >= FechaDesde
                                                                         && x.Fecha <= FechaHasta
                                                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }


        public List<CC_OPERATIVIDAD_METAL> ConsultaOperatividadMetalControlPendiente()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_OPERATIVIDAD_METAL.Where(x => !x.EstadoReporte && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();
            }
        }

        public void Aprobar_ReporteOperatividadMetal(CC_OPERATIVIDAD_METAL Control)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_OPERATIVIDAD_METAL.FirstOrDefault(x => x.IdOperatividadMetal == Control.IdOperatividadMetal);
                if (model != null)
                {
                    model.EstadoReporte = Control.EstadoReporte;
                    model.AprobadoPor = Control.AprobadoPor;
                    model.FechaAprobacion = Control.FechaAprobacion;
                    model.FechaModificacionLog = Control.FechaIngresoLog;
                    model.TerminalModificacionLog = Control.TerminalIngresoLog;
                    model.UsuarioModificacionLog = Control.UsuarioIngresoLog;
                    db.SaveChanges();
                }
                //else
                //{
                //    db.CC_CONDICION_PERSONAL_CONTROL.Add(model);
                //}
            }
        }

        public void Reversar_ReporteOperatividadMetal(CC_OPERATIVIDAD_METAL Control)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_OPERATIVIDAD_METAL.FirstOrDefault(x => x.IdOperatividadMetal == Control.IdOperatividadMetal);
                if (model != null)
                {
                    model.EstadoReporte = Control.EstadoReporte;
                    model.AprobadoPor = Control.AprobadoPor;
                    model.FechaAprobacion = Control.FechaAprobacion;
                    model.FechaModificacionLog = Control.FechaIngresoLog;
                    model.TerminalModificacionLog = Control.TerminalIngresoLog;
                    model.UsuarioModificacionLog = Control.UsuarioIngresoLog;
                    db.SaveChanges();
                }
                //else
                //{
                //    db.CC_CONDICION_PERSONAL_CONTROL.Add(model);
                //}
            }
        }

    }
}