﻿using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Seguridad
{
    public class clsDOpcion
    {

        public List<OPCION> ConsultarOpciones(OPCION filtros)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var query = from o in entities.OPCION select o;
                if (filtros != null)
                {
                    if (!string.IsNullOrEmpty(filtros.EstadoRegistro))
                    {
                        query = query.Where(x => x.EstadoRegistro == filtros.EstadoRegistro);
                    }

                    if (!string.IsNullOrEmpty(filtros.Clase))
                    {
                        query = query.Where(x => x.Clase == filtros.Clase);
                    }
                }
                return query.ToList();
            }
        }

        public string GuardarModificarOpcion(OPCION doOpcion)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poOpcion = entities.OPCION.FirstOrDefault(x => x.IdOpcion == doOpcion.IdOpcion);
                if (poOpcion != null)
                {
                    poOpcion.Nombre = doOpcion.Nombre;
                    poOpcion.Formulario = doOpcion.Formulario;
                    poOpcion.Clase = doOpcion.Clase;
                    poOpcion.Padre = doOpcion.Padre;
                    poOpcion.EstadoRegistro = doOpcion.EstadoRegistro;
                    poOpcion.FechaModificacionLog = doOpcion.FechaCreacionLog;
                    poOpcion.UsuarioModificacionLog = doOpcion.UsuarioCreacionLog;
                    poOpcion.TerminalModificacionLog = doOpcion.TerminalCreacionLog;
                }
                else
                {
                    entities.OPCION.Add(doOpcion);
                }
                entities.SaveChanges();
                return clsAtributos.MsjRegistroGuardado;
            }
        }


    }
}