using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
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

        public bool ValidarUsuarioRol(string IdUsuario, int rol)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var existe = entities.USUARIO_ROL.Any(x => x.IdUsuario == IdUsuario &&
                x.EstadoRegistro == clsAtributos.EstadoRegistroActivo &&
                x.IdRol == rol
                );
                return existe;             
            }
        }

        public List<int?> ConsultaRolesUsuario(string dsIdUsuario)
        {
           
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poRoles = entities.USUARIO_ROL.Where(x => x.IdUsuario == dsIdUsuario && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).Select(x => x.IdRol).ToList();
                return poRoles;

            }
        }

        public bool ValidarPermisoOpcion(string dsUsuario, string dsopcion)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                if (dsopcion == "home" || dsopcion == "Home" || dsopcion== "HomeError")
                {
                    return true;
                }

                var model = (from u in entities.USUARIO_ROL
                             join or in entities.OPCION_ROL on u.IdRol equals or.IdRol
                             join op in entities.OPCION on or.IdOpcion equals op.IdOpcion
                             where u.IdUsuario == dsUsuario 
                             && u.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                             && or.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                             && op.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                             && op.Clase=="H"
                             && op.Formulario == dsopcion select u).FirstOrDefault();
                if (model != null || string.IsNullOrEmpty(dsopcion))
                    return true;
                else
                    return false;


            }
        }

        public Object[] ConsultarRolesDeUsuario(string psrolid)
        {
            using (Asiservy.Automatizacion.Datos.Datos.ASIS_PRODEntities db = new Asiservy.Automatizacion.Datos.Datos.ASIS_PRODEntities())
            {
                //List<string> pListPadrestotal = new List<string>();
                //List<string> pListHijostotal = new List<string>();
                List<spConsultaOpcionesPorRol> pListPadrestotal = new List<spConsultaOpcionesPorRol>();
                List<spConsultaOpcionesPorRol> pListHijostotal = new List<spConsultaOpcionesPorRol>();

                var piRol = (from r in db.USUARIO_ROL
                             where r.IdUsuario == psrolid && r.EstadoRegistro=="A"
                             select r.IdRol).ToList();
                foreach (var item in piRol)
                {
                    //var pListPadres = (from r in db.OPCION_ROL
                    //                   join rn in db.OPCION on r.IdOpcion equals rn.IdOpcion
                    //                   where r.IdRol == item && rn.Clase == "P"
                    //                   select rn.Nombre).ToList();
                    List<spConsultaOpcionesPorRol> pListPadres = db.spConsultaOpcionesPorRol(item).Where(x=>x.Clase=="P").ToList();
                    pListPadrestotal.AddRange(pListPadres);
                }
                List<spConsultaOpcionesPorRol> ListModulos = pListPadrestotal.Where(x => x.IdModulo != null).ToList();
                foreach (var item in piRol)
                {
                    //var pListHijos = (from r in db.OPCION_ROL
                    //                  join rn in db.OPCION on r.IdOpcion equals rn.IdOpcion
                    //                  where r.IdRol == item && rn.Clase == "H"
                    //                  select rn.Formulario).ToList();
                    var pListHijos = db.spConsultaOpcionesPorRol(item).Where(x => x.Clase == "H").ToList();
                    pListHijostotal.AddRange(pListHijos);
                }
                List<ConsultaOpcionesxRolViewModel> pListPadresfilter = new List<ConsultaOpcionesxRolViewModel>();
                List<ConsultaOpcionesxRolViewModel> pListHijosfilter = new List<ConsultaOpcionesxRolViewModel>();
                List<ModuloViewModel> pListModulosFilter = new List<ModuloViewModel>();
                pListHijosfilter = pListHijostotal.ConvertAll(x=> new ConsultaOpcionesxRolViewModel
                {
                    IdOpcion = x.IdOpcion,
                    Clase = x.Clase,
                    Formulario = x.Formulario,
                    Nombre = x.Nombre,
                    Padre = x.Padre,
                    Url = x.Url,
                    Orden=x.Orden,
                    IdModulo=x.IdModulo
                });
                pListPadresfilter = pListPadrestotal.ConvertAll(x => new ConsultaOpcionesxRolViewModel
                {
                    Clase = x.Clase,
                    Formulario = x.Formulario,
                    IdOpcion = x.IdOpcion,
                    Nombre = x.Nombre,
                    Padre = x.Padre,
                    Url = x.Url,
                    Orden=x.Orden,
                    IdModulo=x.IdModulo
                });
                pListModulosFilter = ListModulos.ConvertAll(x => new ModuloViewModel
                {
                    IdModulo=x.IdModulo,
                    NombreModulo=x.NombreModulo,
                    Orden=x.OrdenModulo
                });
                object[] oresultado = new object[3];
                //oresultado[0] = pListPadrestotal;
                //oresultado[1] = pListHijostotal;
                oresultado[0] = pListPadresfilter.Distinct().OrderBy(Z => Z.Orden).ToList();
                oresultado[1] = pListHijosfilter.Distinct().OrderBy(Z => Z.Orden).ToList();
                oresultado[2] = pListModulosFilter.Distinct().OrderBy(z=>z.Orden).ToList();
                return oresultado;

            }
        }
    }
}