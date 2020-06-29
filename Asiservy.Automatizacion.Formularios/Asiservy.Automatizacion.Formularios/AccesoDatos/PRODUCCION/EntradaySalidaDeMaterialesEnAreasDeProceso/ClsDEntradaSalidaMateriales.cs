using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Asiservy.Automatizacion.Formularios.AccesoDatos.PRODUCCION.EntradaySalidaDeMaterialesEnAreasDeProceso
{
    public class ClsDEntradaSalidaMateriales
    {
        public List<ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL> ConsultaMaterialQuebradizo()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL.ToList();
            }
        }
        public void ModificarMaterial(ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var material = entities.ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL.FirstOrDefault(x => x.IdMaterial == model.IdMaterial);
                if (material != null)
                {
                    material.Nombre = model.Nombre;
                    material.Descripcion = model.Descripcion;
                    material.EstadoRegistro = model.EstadoRegistro;
                    material.FechaModificacionLog = DateTime.Now;
                    material.TerminalModificacionLog = model.TerminalIngresoLog;
                    material.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    entities.SaveChanges();
                }

            }
        }
        public void GuardarMaterial(ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                entities.ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL.Add(model);
                entities.SaveChanges();
            }
        }
    }
}