using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Seguridad
{
    public class clsDNivelUsuario
    {
        public List<NivelUsuarioViewModel> ConsultarNivelUsuario(NIVEL_USUARIO filtros)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var query = from o in entities.NIVEL_USUARIO select o;
                clsDEmpleado clsDEmpleado = new clsDEmpleado();
                List<NivelUsuarioViewModel> Lista = new List<NivelUsuarioViewModel>();
                var Empleados = clsDEmpleado.ConsultaEmpleado(filtros.IdUsuario);

                if (!string.IsNullOrEmpty(filtros.EstadoRegistro))
                {
                    query = query.Where(x => x.EstadoRegistro == filtros.EstadoRegistro);
                }

                    Lista = (from q in query.ToList()
                            join e in Empleados on q.IdUsuario equals e.CEDULA
                            select new NivelUsuarioViewModel
                            {
                                IdNivelUsuario=q.IdNivelUsuario,
                                IdUsuario=q.IdUsuario,
                                EstadoRegistro=q.EstadoRegistro,
                                Nivel =q.Nivel,
                                Usuario= e.NOMBRES

                            }).ToList();
                
                return Lista;

            }
        }

        public string GuardarModificarNivelUsuario(NIVEL_USUARIO doNivelUsuario)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poNivelUsuario = entities.NIVEL_USUARIO.FirstOrDefault(x => x.IdNivelUsuario == doNivelUsuario.IdNivelUsuario);
                if (poNivelUsuario != null)
                {
                    poNivelUsuario.Nivel = doNivelUsuario.Nivel;
                    poNivelUsuario.FechaModificacionLog = doNivelUsuario.FechaModificacionLog;
                    poNivelUsuario.TerminalModificacionLog = doNivelUsuario.TerminalModificacionLog;
                    poNivelUsuario.UsuarioModificacionLog = doNivelUsuario.UsuarioModificacionLog;
                }
                else
                {
                    entities.NIVEL_USUARIO.Add(doNivelUsuario);
                }
                entities.SaveChanges();
                return clsAtributos.MsjRegistroGuardado;
            }
        }
    }
}