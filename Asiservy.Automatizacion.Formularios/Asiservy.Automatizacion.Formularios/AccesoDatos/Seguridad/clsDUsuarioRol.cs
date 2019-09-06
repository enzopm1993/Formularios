using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Seguridad
{
    public class clsDUsuarioRol
    {
        clsApiUsuario clsApiUsuario = null;


        public string GuardarModificarUsuarioRol(UsuarioRolViewModel model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poUsuarioRol = entities.USUARIO_ROL.FirstOrDefault(x=>
                (x.IdUsuarioRol == model.IdUsuarioRol)||
                (x.IdUsuario==model.IdUsuario && x.IdRol == model.IdRol));               

                if (poUsuarioRol != null)
                {
                    poUsuarioRol.EstadoRegistro = model.EstadoRegistro;
                    poUsuarioRol.FechaModificacionlog = model.FechaCreacionlog;
                    poUsuarioRol.UsuarioModificacionlog = model.UsuarioCreacionlog;
                    poUsuarioRol.TerminalModificacionlog = model.TerminalCreacionlog;

                }else{

                    USUARIO_ROL UsuarioRol = new USUARIO_ROL
                    {
                        IdUsuarioRol = model.IdUsuarioRol,
                        IdRol = model.IdRol,
                        IdUsuario = model.IdUsuario,
                        EstadoRegistro = model.EstadoRegistro,
                        FechaCreacionlog = model.FechaCreacionlog,
                        UsuarioCreacionlog = model.UsuarioCreacionlog,
                        TerminalCreacionlog = model.TerminalCreacionlog
                    };
                    entities.USUARIO_ROL.Add(UsuarioRol);
                }

                entities.SaveChanges();
                return clsAtributos.MsjRegistroGuardado;
            }
        }
            public List<UsuarioRolViewModel> ConsultaUsuarioRol(USUARIO_ROL filtros)
        {
            using(ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                clsApiUsuario = new clsApiUsuario();
                var query = from u in entities.USUARIO_ROL select u;
                var usuarios = clsApiUsuario.ConsultaListaUsuariosSap();
                if (filtros != null)
                {
                    if (!string.IsNullOrEmpty(filtros.EstadoRegistro))
                    {
                        query = query.Where(x=> x.EstadoRegistro==filtros.EstadoRegistro);
                    }

                }
                var ListaUsuarioRol = (from q in query
                                       join r in entities.ROL on q.IdRol equals r.IdRol                                      
                                       select new UsuarioRolViewModel
                                       {
                                           IdUsuarioRol = q.IdUsuarioRol,
                                           IdUsuario = q.IdUsuario,
                                           EstadoRegistro = q.EstadoRegistro,
                                           IdRol = q.IdRol,                                           
                                           Rol = r.Descripcion
                                       }).ToList();

                var Lista = (from l in ListaUsuarioRol
                                  join u in usuarios on l.IdUsuario equals u.Cedula
                                  select new UsuarioRolViewModel {
                                      IdUsuarioRol = l.IdUsuarioRol,
                                      IdUsuario = l.IdUsuario,
                                      EstadoRegistro = l.EstadoRegistro,
                                      IdRol = l.IdRol,
                                      Rol = l.Rol,
                                      Usuario = u.Nombre

                                  }).ToList();
                                 
                return Lista;
            }
        }
    }
}