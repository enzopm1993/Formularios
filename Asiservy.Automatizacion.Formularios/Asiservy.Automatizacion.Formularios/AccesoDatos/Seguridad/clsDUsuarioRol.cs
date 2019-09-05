using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Seguridad
{
    public class clsDUsuarioRol
    {

         public List<USUARIO_ROL> ConsultaUsuarioRol(USUARIO_ROL filtros)
        {
            using(ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var query = from u in entities.USUARIO_ROL select u;

                if(filtros != null)
                {
                    if (!string.IsNullOrEmpty(filtros.EstadoRegistro))
                    {
                        query = query.Where(x=> x.EstadoRegistro==filtros.EstadoRegistro);
                    }

                }


                return query.ToList();
            }
        }
    }
}