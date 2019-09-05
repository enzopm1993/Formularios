using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Seguridad
{
    public class clsDOpcion
    {

        public List<OPCION> ConsultarOpciones()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.OPCION.ToList();
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