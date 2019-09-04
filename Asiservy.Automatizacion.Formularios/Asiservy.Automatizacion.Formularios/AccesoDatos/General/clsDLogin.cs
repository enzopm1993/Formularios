using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDLogin
    {

        public string ConsultarUsuarioExiste(string psusuario, string psclave)
        {
            using (Asiservy.Automatizacion.Datos.Datos.ASIS_PRODEntities db = new Asiservy.Automatizacion.Datos.Datos.ASIS_PRODEntities())
            {
                string psCodigoUsuario = db.spConsultarUsuario(psusuario, psclave).FirstOrDefault();
                return psCodigoUsuario;
            }
            
        }

        public List<int?> ConsultaRolesUsuario(string dsIdUsuario)
        {
           
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poRoles = entities.USUARIO_ROL.Where(x => x.IdUsuario == dsIdUsuario).Select(x => x.IdRol).ToList();
                return poRoles;

            }
        }

        public Object[] ConsultarRolesDeUsuario(string psrolid)
        {
            using (Asiservy.Automatizacion.Datos.Datos.ASIS_PRODEntities db = new Asiservy.Automatizacion.Datos.Datos.ASIS_PRODEntities())
            {
                List<string> pListPadrestotal = new List<string>();
                List<string> pListHijostotal = new List<string>();
                
                var piRol = (from r in db.USUARIO_ROL
                             where r.IdUsuario == psrolid
                             select r.IdRol).ToList();
                foreach (var item in piRol)
                {
                    var pListPadres = (from r in db.OPCION_ROL
                                       join rn in db.OPCION on r.IdOpcion equals rn.IdOpcion
                                       where r.IdRol == item && rn.Clase == "P"
                                       select rn.Nombre).ToList();
                    pListPadrestotal.AddRange(pListPadres);
                }
                //var pListPadres = (from r in db.OPCION_ROL
                //                   join rn in db.OPCION on r.IdOpcion equals rn.IdOpcion
                //                   where r.IdRol == piRol && rn.Clase=="P"
                //                   select rn.Nombre).ToList();

                foreach (var item in piRol)
                {
                    var pListHijos = (from r in db.OPCION_ROL
                                      join rn in db.OPCION on r.IdOpcion equals rn.IdOpcion
                                      where r.IdRol == item && rn.Clase == "H"
                                      select rn.Formulario).ToList();
                    pListHijostotal.AddRange(pListHijos);
                }
                //var pListHijos = (from r in db.OPCION_ROL
                //                  join rn in db.OPCION on r.IdOpcion equals rn.IdOpcion
                //                  where r.IdRol==1&& rn.Clase=="H"
                //                  select rn.Formulario).ToList();

                object[] oresultado = new object[2];
                oresultado[0] = pListPadrestotal;
                oresultado[1] = pListHijostotal;

                return oresultado;

            }
        }
    }
}