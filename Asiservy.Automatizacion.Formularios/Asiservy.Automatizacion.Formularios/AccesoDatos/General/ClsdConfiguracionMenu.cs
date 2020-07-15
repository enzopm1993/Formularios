using Asiservy.Automatizacion.Datos.Datos;
using System.Collections.Generic;
using System.Linq;


namespace Asiservy.Automatizacion.Formularios.AccesoDatos.General
{
    public class ClsdConfiguracionMenu
    {

        #region FONDO LOGIN
        public List<FONDO_LOGIN> ConsultaLoginFondos()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.FONDO_LOGIN.AsNoTracking().ToList();
            }
        }


        public void GuardarModificarLoginFondo(FONDO_LOGIN model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.FONDO_LOGIN.FirstOrDefault(x => x.IdFondoLogin == model.IdFondoLogin);
                if (poControl != null)
                {
                    poControl.Descripcion = model.Descripcion;
                    poControl.Imagen = model.Imagen;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.FONDO_LOGIN.Add(model);
                }
                entities.SaveChanges();
            }
        }
        #endregion




    }
}