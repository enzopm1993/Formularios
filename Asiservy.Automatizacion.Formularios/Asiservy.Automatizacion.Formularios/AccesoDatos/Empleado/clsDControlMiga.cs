using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Empleado
{
    public class clsDControlMiga
    {

        public void GuardarModificarControlMiga(CONTROL_MIGA model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var ControlMiga = entities.CONTROL_MIGA.FirstOrDefault(x=> x.IdControlHuesoMiga == model.IdControlHuesoMiga);
                if (ControlMiga != null)
                {
                    ControlMiga.Miga = model.Miga;
                    ControlMiga.FechaModificacionLog = DateTime.Now;
                    ControlMiga.TerminalModificacionLog = model.TerminalIngresoLog;
                    ControlMiga.UsuarioModificacionLog = model.UsuarioIngresoLog;
                }
                else
                {
                    entities.CONTROL_MIGA.Add(model);
                }
                entities.SaveChanges();
            }
        }
    }
}