using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Seguridad
{
    public class clsDOpcionRol
    {
        public List<ROL> ConsultarRol()
        {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                return db.ROL.Where(x=>x.EstadoRegistro=="A").ToList();
            }              
        }
        public List<OPCION> ConsultarOpcion()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.OPCION.Where(x => x.EstadoRegistro == "A").ToList();
            }
        }
    }
}