using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.Mantenimientos
{
    public class clsDMantenimientoTextura
    {
        public List<CC_MANTENIMIENTO_TEXTURA> ConsultaManteminetoTextura()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_MANTENIMIENTO_TEXTURA.ToList();
                return lista;
            }
        }

        public void GuardarModificarMantenimientoTextura(CC_MANTENIMIENTO_TEXTURA model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_MANTENIMIENTO_TEXTURA.FirstOrDefault(x => x.IdTextura == model.IdTextura);
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
                    entities.CC_MANTENIMIENTO_TEXTURA.Add(model);
                }
                entities.SaveChanges();
            }
        }
    }
}