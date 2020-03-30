using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.OperatividadMetal
{
    public class clsDOperatividadMetal
    {
        public CC_OPERATIVIDAD_METAL ConsultaOperatividadMetal(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                CC_OPERATIVIDAD_METAL lista = entities.CC_OPERATIVIDAD_METAL.Where(x =>
                            x.Fecha == Fecha
                            && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                return lista;
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
                var poControl = entities.CC_OPERATIVIDAD_METAL_DETALLE.FirstOrDefault(x => x.IdOperatividadMetalDetalle == model.IdOperatividadMetalDetalle);
                if (poControl != null)
                {
                    poControl.AceroInoxidable = model.AceroInoxidable;
                    poControl.Ferroso = model.Ferroso;
                    poControl.NoFerroso = model.NoFerroso;
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



    }
}