using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.General
{
    public class clsDParametro
    {
        ASIS_PRODEntities entities = null;

        public List<PARAMETRO> ConsultaParametros(PARAMETRO Filtros)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                
                IEnumerable<PARAMETRO> poParametro = entities.PARAMETRO;
                if (Filtros != null && Filtros.IdParametro>0)
                {
                    poParametro = poParametro.Where(x => x.IdParametro == Filtros.IdParametro);
                }
                if (Filtros != null && !string.IsNullOrEmpty(Filtros.Codigo))
                {
                    poParametro = poParametro.Where(x => x.Codigo == Filtros.Codigo);
                }
                if (Filtros != null && !string.IsNullOrEmpty(Filtros.EstadoRegistro))
                {
                    poParametro = poParametro.Where(x => x.EstadoRegistro == Filtros.EstadoRegistro);
                }

                return poParametro.ToList();
            }
        }
        public string GuardarModificarParametro(PARAMETRO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                string Respuesta = string.Empty;
                var poParametro = entities.PARAMETRO.FirstOrDefault(x =>
                x.IdParametro == model.IdParametro
                || x.Codigo == model.Codigo);

                if (poParametro != null)
                {
                    poParametro.Descripcion = model.Descripcion;
                    poParametro.Observacion = model.Observacion;
                    poParametro.Valor = model.Valor;
                    poParametro.Codigo = model.Codigo;
                    poParametro.EstadoRegistro = model.EstadoRegistro;
                    poParametro.FechaModificacionLog = DateTime.Now;
                    poParametro.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poParametro.TerminalModificacionLog = model.TerminalIngresoLog;
                }
                else
                {
                    entities.PARAMETRO.Add(model);
                }
                entities.SaveChanges();
                Respuesta = clsAtributos.MsjRegistroGuardado;
                return Respuesta;
            }
        }



    }
}