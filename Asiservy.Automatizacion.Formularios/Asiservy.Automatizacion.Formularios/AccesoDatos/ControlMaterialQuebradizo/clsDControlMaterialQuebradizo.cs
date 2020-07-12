using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ControlMaterialQuebradizo
{
    public class clsDControlMaterialQuebradizo
    {

        public List<spConsultaControlMaterialQuebradizoDetalle> ConsultaControlMaterialQuebradizo(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaControlMaterialQuebradizoDetalle(IdControl).ToList();
            }
        }

        public CONTROL_MATERIAL ConsultaControlMaterial(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CONTROL_MATERIAL.FirstOrDefault(x=> x.IdControlMaterial==IdControl);
            }
        }

        public int ValidaControlMaterialQuebradizo(DateTime Fecha, string Linea,string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var model = entities.CONTROL_MATERIAL.FirstOrDefault(x => x.Fecha == Fecha && x.Linea == Linea && x.Turno == Turno && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    return model.IdControlMaterial;
                }
                else
                {
                    return 0;
                }
            }
        }

        public void ElimnarControlMaterialQuebradizo(CONTROL_MATERIAL model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var control = entities.CONTROL_MATERIAL.FirstOrDefault(x => x.IdControlMaterial== model.IdControlMaterial);
                if (control != null)
                {
                    control.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    control.UsuarioModificacionLog = model.UsuarioModificacionLog;
                    control.TerminalModificacionLog = model.TerminalModificacionLog;
                    control.FechaModificacionLog = DateTime.Now;
                    entities.SaveChanges();
                }

            }
       }

        public void GuardarObservacionControl(int id, string Observacion)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var control = entities.CONTROL_MATERIAL.FirstOrDefault(x => x.IdControlMaterial == id);
                if (control != null)
                {
                    control.Observacion = Observacion;
                    entities.SaveChanges();
                }
            }

        }

            public void GuardarModificarControlMaterialDetalle(CONTROL_MATERIAL_DETALLE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var detalle = entities.CONTROL_MATERIAL_DETALLE.FirstOrDefault(x=> x.IdControlMaterialDetalle == model.IdControlMaterialDetalle);
                if (detalle != null)
                {
                    detalle.TotalMaterial = model.TotalMaterial;
                    detalle.BuenEstado = model.BuenEstado;
                    detalle.DadoBaja = model.DadoBaja;
                    detalle.Observacion = model.Observacion;

                    entities.SaveChanges();
                }
            }

        }

        public int GenerarControlMaterialQuebradizo(CONTROL_MATERIAL model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    var control = entities.CONTROL_MATERIAL.FirstOrDefault(x => x.Fecha == model.Fecha && x.Linea == model.Linea && x.Turno == model.Turno && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    if (control != null)
                    {
                        return control.IdControlMaterial;
                    }
                    else
                    {
                        entities.CONTROL_MATERIAL.Add(model);
                        entities.SaveChanges();

                        control = entities.CONTROL_MATERIAL.FirstOrDefault(x => x.Fecha == model.Fecha && x.Linea == model.Linea && x.Turno == model.Turno && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                        var detalle = entities.MATERIAL_LINEA.Where(x => x.Linea == control.Linea && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();
                        if (detalle.Any())
                        {
                            foreach (var x in detalle)
                            {
                                entities.CONTROL_MATERIAL_DETALLE.Add(new CONTROL_MATERIAL_DETALLE
                                {
                                    IdControlMaterial = control.IdControlMaterial,
                                    CodigoMaterial = x.Codigo,
                                    EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                                    FechaIngresoLog = DateTime.Now,
                                    TerminalIngresoLog = model.TerminalIngresoLog,
                                    UsuarioIngresoLog = model.UsuarioIngresoLog
                                });
                            }
                            entities.SaveChanges();
                        }
                        transaction.Commit();
                        return control.IdControlMaterial;

                    }
                  
                }


            }
        }




        public List<spConsultaAsignaMaterialesLinea> ConsultaMaterialLinea(string Linea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaAsignaMaterialesLinea(Linea).ToList();
            }
        }
        public void GuardarModificarMaterialLinea(MATERIAL_LINEA model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var material = entities.MATERIAL_LINEA.FirstOrDefault(x => x.IdMaterialLinea == model.IdMaterialLinea
                || (x.Linea==model.Linea && x.Codigo == model.Codigo)
                );

                if (material != null)
                {                   
                    material.EstadoRegistro = model.EstadoRegistro;
                    material.FechaModificacionLog = DateTime.Now;
                    material.TerminalModificacionLog = model.TerminalIngresoLog;
                    material.UsuarioModificacionLog = model.UsuarioIngresoLog;
                }
                else
                {
                    entities.MATERIAL_LINEA.Add(model);
                }
                entities.SaveChanges();

            }
        }


        public List<MATERIAL_QUEBRADIZO> ConsultaMaterialQuebradizo()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.MATERIAL_QUEBRADIZO.ToList();
            }
        }

        public void ModificarMaterialQuebradizo(MATERIAL_QUEBRADIZO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var material = entities.MATERIAL_QUEBRADIZO.FirstOrDefault(x => x.IdMaterial == model.IdMaterial);
                if (material != null)
                {
                    material.Nombre = model.Nombre;
                    material.EstadoRegistro = model.EstadoRegistro;
                    material.FechaModificacionLog = DateTime.Now;
                    material.TerminalModificacionLog = model.TerminalIngresoLog;
                    material.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    entities.SaveChanges();
                }
                
            }
        }


        public List<spReporteControlMaterialQuebradizo> ConsultaReporteControlMaterial(DateTime FechaDesde, DateTime FechaHasta, string Linea, string turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spReporteControlMaterialQuebradizo(FechaDesde, FechaHasta, Linea, turno).ToList();                
            }
        }

    }
}