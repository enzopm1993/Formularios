using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MantenimientoCisterna
{
    public class clsDMantenimientoCisterna
    {
        public List<CC_MANTENIMIENTO_CISTERNA> ConsultarMantenimientoCisterna()
        {
            using (ASIS_PRODEntities db= new ASIS_PRODEntities())
            {
                var lista = db.CC_MANTENIMIENTO_CISTERNA.ToList();
                return lista;
            }
        }
        public int GuardarModificarMantenimientoCisterna(CC_MANTENIMIENTO_CISTERNA guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                var model = db.CC_MANTENIMIENTO_CISTERNA.FirstOrDefault(x=> x.IdCisterna ==guardarmodificar.IdCisterna);
                if (model!=null)
                {
                    model.NDescripcion = guardarmodificar.NDescripcion;
                    model.Ubicacion = guardarmodificar.Ubicacion;
                    model.Asignacion = guardarmodificar.Asignacion;
                    model.Tipo = guardarmodificar.Tipo;
                    model.Capacidad = guardarmodificar.Capacidad;
                    model.EstadoRegistro = guardarmodificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_MANTENIMIENTO_CISTERNA.Add(guardarmodificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarMantenimientoCisterna(CC_MANTENIMIENTO_CISTERNA guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_MANTENIMIENTO_CISTERNA.FirstOrDefault(x => x.IdCisterna == guardarmodificar.IdCisterna );
                if (model != null)
                {                    
                    model.EstadoRegistro = guardarmodificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_MANTENIMIENTO_CISTERNA.Add(guardarmodificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
    }
}