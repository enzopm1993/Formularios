using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Seguridad
{
    public class clsDOpcionRol
    {

        public List<OpcionRolViewModel> ConsultarOpcionRol()
        {
            using(ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                var ListaOpcionRol = (from or in db.OPCION_ROL
                                      join o in db.OPCION on or.IdOpcion equals o.IdOpcion
                                      join rol in db.ROL on or.IdRol equals rol.IdRol
                                      select new OpcionRolViewModel
                                      {
                                          IdOpcionRol = or.IdOpcionRol,
                                          IdOpcion = or.IdOpcion,
                                          IdRol = or.IdRol,
                                          NombreOpcion = o.Nombre,
                                          NombreRol = rol.Descripcion,
                                          Estado = or.EstadoRegistro
                                      }
                      );
                return ListaOpcionRol.ToList();
            }
        }
        public string GuardarOpcionRol(OPCION_ROL OpcionRol, string usuario, string terminal)
        {
            string psMensaje;
            using(ASIS_PRODEntities db =new ASIS_PRODEntities())
            {
                OPCION_ROL BuscarOpcionRol = db.OPCION_ROL.Find(OpcionRol.IdOpcionRol);
                var existe = (from or in db.OPCION_ROL
                              where or.IdOpcion == OpcionRol.IdOpcion && or.IdRol == OpcionRol.IdRol
                              select or);
                //if (BuscarOpcionRol != null || (BuscarOpcionRol.IdRol== OpcionRol.IdRol&& BuscarOpcionRol.IdOpcion== OpcionRol.IdOpcion))
                if (BuscarOpcionRol != null || (existe.Count()>0))
                {
                    if (BuscarOpcionRol != null)
                    {
                        BuscarOpcionRol.UsuarioModificacionlog = usuario;
                        BuscarOpcionRol.TerminalModificacionlog = terminal;
                        BuscarOpcionRol.FechaModificacionlog = DateTime.Now;
                        BuscarOpcionRol.EstadoRegistro = OpcionRol.EstadoRegistro;
                        db.SaveChanges();
                        psMensaje = "Registro Actualizado con éxito";
                    }
                    else
                    {
                        existe.FirstOrDefault().UsuarioModificacionlog = usuario;
                        existe.FirstOrDefault().TerminalModificacionlog = terminal;
                        existe.FirstOrDefault().FechaModificacionlog = DateTime.Now;
                        existe.FirstOrDefault().EstadoRegistro = OpcionRol.EstadoRegistro;
                        db.SaveChanges();
                        psMensaje = "El Rol seleccionado ya tenia la opción asignada, Registro actualizado ";
                    }


                }
                else
                {
                    OpcionRol.UsuarioCreacionlog = usuario;
                    OpcionRol.FechaCreacionlog = DateTime.Now;
                    OpcionRol.TerminalCreacionlog = terminal;
                    db.OPCION_ROL.Add(OpcionRol);
                    db.SaveChanges();
                    psMensaje = "Registro ingresado con éxito";
                }
            }
            return psMensaje;
        }
    }
}