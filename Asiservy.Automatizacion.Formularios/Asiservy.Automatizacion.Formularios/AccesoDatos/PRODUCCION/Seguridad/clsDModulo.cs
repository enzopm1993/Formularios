using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Seguridad
{
    public class clsDModulo
    {
        public List<MODULO> ConsultarModulos(MODULO filtros)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var query = from o in entities.MODULO select o;
                if (filtros != null)
                {
                    if (!string.IsNullOrEmpty(filtros.EstadoRegistro))
                    {
                        query = query.Where(x => x.EstadoRegistro == filtros.EstadoRegistro);
                    }

                    if (filtros.IdModulo>0)
                    {
                        query = query.Where(x => x.IdModulo == filtros.IdModulo);
                    }
                }
                return query.ToList();
            }
        }

        public string GuardarModificarModulo(MODULO doModulo)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poModulo = entities.MODULO.FirstOrDefault(x => x.IdModulo == doModulo.IdModulo);
                if (poModulo != null)
                {
                    poModulo.Nombre = doModulo.Nombre;
                    poModulo.Orden = doModulo.Orden;
                    poModulo.EstadoRegistro = doModulo.EstadoRegistro;
                    poModulo.FechaModificacionLog = doModulo.FechaIngresoLog;
                    poModulo.UsuarioModificacionLog = doModulo.UsuarioIngresoLog;
                    poModulo.TerminalModificacionLog = doModulo.TerminalIngresoLog;
                }
                else
                {
                    entities.MODULO.Add(doModulo);
                }
                entities.SaveChanges();
                return clsAtributos.MsjRegistroGuardado;
            }
        }

    }
}