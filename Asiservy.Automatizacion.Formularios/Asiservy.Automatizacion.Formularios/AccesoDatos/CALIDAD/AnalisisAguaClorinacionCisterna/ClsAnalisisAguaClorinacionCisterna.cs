using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.AnalisisAguaClorinacionCisterna
{
    public class ClsAnalisisAguaClorinacionCisterna
    {
        public List<CC_ANALISIS_AGUA_CLORINACION_MANT> ConsultarMantenimientoCisterna()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_ANALISIS_AGUA_CLORINACION_MANT.ToList();
                return lista;
            }
        }
        public int GuardarModificarMantenimientoCisterna(CC_ANALISIS_AGUA_CLORINACION_MANT guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_ANALISIS_AGUA_CLORINACION_MANT.FirstOrDefault(x => x.IdCisterna == guardarmodificar.IdCisterna);
                if (model != null)
                {
                    model.DescripcionCisterna = guardarmodificar.DescripcionCisterna;
                    model.EstadoRegistro = guardarmodificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_ANALISIS_AGUA_CLORINACION_MANT.Add(guardarmodificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarMantenimientoCisterna(CC_ANALISIS_AGUA_CLORINACION_MANT guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_ANALISIS_AGUA_CLORINACION_MANT.FirstOrDefault(x => x.IdCisterna == guardarmodificar.IdCisterna);
                if (model != null)
                {
                    model.EstadoRegistro = guardarmodificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }
    }
}