using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.Mantenimientos
{
    public class clsDMantenimientoSabor
    {
        public List<CC_MANTENIMIENTO_SABOR> ConsultaManteminetoSabor()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_MANTENIMIENTO_SABOR.ToList();
                return lista;
            }
        }

        public void GuardarModificarMantenimientoSabor(CC_MANTENIMIENTO_SABOR model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_SABOR.FirstOrDefault(x => x.IdSabor == model.IdSabor);
                if (poControl != null)
                {
                    poControl.Descripcion = model.Descripcion;
                    poControl.Abreviatura = model.Abreviatura;
                    poControl.EstadoRegistro = model.EstadoRegistro;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.CC_MANTENIMIENTO_SABOR.Add(model);
                }
                entities.SaveChanges();
            }
        }
    }
}