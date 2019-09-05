using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Seguridad
{
    public class clsDOpcion
    {

        public List<OPCION> ConsultarOpciones()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.OPCION.ToList();
            }
        }
        public List<ROL> ConsultarRoles()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.ROL.ToList();
            }
        }
        public string GuardarRol(ROL poRol,string usuario,string terminal)
        {
            string psMensaje = string.Empty;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var piBuscarRol = db.ROL.Where(x => x.IdRol == poRol.IdRol).Select(x => x.IdRol);
                if(piBuscarRol.Count()>0)
                {
                    var poRolU=db.ROL.Find(piBuscarRol.FirstOrDefault());
                    poRolU.Descripcion = poRol.Descripcion;
                    poRolU.EstadoRegistro = poRol.EstadoRegistro;
                    poRolU.UsuarioModificacionLog = usuario;
                    poRolU.TerminalModificacionLog = terminal;
                    poRolU.FechaModificacionLog = DateTime.Now;
                    db.SaveChanges();
                    psMensaje = "Rol Actualizado con éxito";
                    return psMensaje;
                }
                else
                {
                    poRol.UsuarioCreacionLog = usuario;
                    poRol.TerminalCreacionLog = terminal;
                    poRol.FechaCreacionLog = DateTime.Now;
                    db.ROL.Add(poRol);
                    db.SaveChanges();
                    psMensaje = "Rol ingresado con éxito";
                    return psMensaje;
                } 
            }
        }
    }
}