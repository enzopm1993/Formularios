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

        public FONDO_LOGIN ConsultaLoginFondoActivo()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.FONDO_LOGIN.AsNoTracking().FirstOrDefault(x=> x.Vigente);
            }
        }


        public void GuardarModificarLoginFondo(FONDO_LOGIN model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                if (model.Vigente)
                {
                    var fondos = entities.FONDO_LOGIN.ToList();
                    foreach(var f in fondos)
                    {
                        f.Vigente = false;
                    }
                }       
                var poControl = entities.FONDO_LOGIN.FirstOrDefault(x => x.IdFondoLogin == model.IdFondoLogin);
                if (poControl != null)
                {
                    poControl.Descripcion = model.Descripcion;
                    if (model.Imagen != null) { poControl.Imagen = model.Imagen; }
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

        public void ActivarLoginFondo(FONDO_LOGIN model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
              
                var fondos = entities.FONDO_LOGIN.ToList();
                foreach (var f in fondos)
                {
                    f.Vigente = false;
                }
               
                var poControl = entities.FONDO_LOGIN.FirstOrDefault(x => x.IdFondoLogin == model.IdFondoLogin);
                if (poControl != null)
                {
                    poControl.Vigente = true;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
               
                entities.SaveChanges();
            }
        }
        #endregion




    }
}