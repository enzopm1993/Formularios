using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ParametroCalidad
{
    public class ClsdParametroCalidad
    {

        public List<CC_PARAMETRO_CALIDAD> ConsultaManteminetoParametroCalidad()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_PARAMETRO_CALIDAD.ToList();
                return lista;
            }
        }

        public CC_PARAMETRO_CALIDAD ConsultaManteminetoParametroCalidad(string Cod)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_PARAMETRO_CALIDAD.FirstOrDefault(x=> x.CodParametro == Cod);
                return lista;
            }
        }        

        public void GuardarModificarMantenimientoParametroCalidad(CC_PARAMETRO_CALIDAD model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_PARAMETRO_CALIDAD.FirstOrDefault(x => x.CodParametro == model.CodParametro);
                if (poControl != null)
                {
                    poControl.Nombre = model.Nombre;
                    poControl.Observacion = model.Observacion;
                    poControl.Maximo = model.Maximo;
                    poControl.Minimo = model.Minimo;
                    poControl.ColorDentroRango = model.ColorDentroRango;
                    poControl.ColorFueraRango = model.ColorFueraRango;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                    entities.SaveChanges();
                }
            }
        }
              
    }
}