using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDError
    {
        public void GrabarError(ERROR doError)
        {
            try
            {
                using (ASIS_PRODEntities db = new ASIS_PRODEntities())
                {
                    db.ERROR.Add(doError);
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                //guardar en archivo txt
            }
            
        }
    }
}