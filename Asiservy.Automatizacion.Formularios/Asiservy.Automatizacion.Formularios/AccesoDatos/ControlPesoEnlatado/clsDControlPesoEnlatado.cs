using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ControlPesoEnlatado
{
    public class clsDControlPesoEnlatado
    {

        public List<spConsultaControlPesoEnlatado> ConsultarControlPesoEnlatado(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                
                    return entities.spConsultaControlPesoEnlatado(Fecha).ToList();                
            }
        }
        public void GuardarModificarControlHoraMaquina(CONTROL_PESO_ENLATADO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var control = entities.CONTROL_PESO_ENLATADO.FirstOrDefault(x => x.IdControlPesoEnlatado == model.IdControlPesoEnlatado || (x.OrdenFabricacion == model.OrdenFabricacion && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
                if (control != null)
                {
                    control.EstadoRegistro = model.EstadoRegistro;
                    control.FechaModificacionLog = DateTime.Now;
                    control.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    control.TerminalModificacionLog = model.TerminalIngresoLog;
                }
                else
                {
                    entities.CONTROL_PESO_ENLATADO.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public bool ValidaControlPesoEnlatado(CONTROL_PESO_ENLATADO model)
        {
            var valida = true;
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {

                var control = entities.CONTROL_PESO_ENLATADO.FirstOrDefault(x => 
                x.Fecha== model.Fecha 
                && x.Peso == model.Peso
                && x.LineaEnlatado == model.LineaEnlatado
                && x.OrdenFabricacion == model.OrdenFabricacion
                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);

                if(control!= null)
                {
                    valida = false;
                }

            }
            return valida;
        }



    }
}