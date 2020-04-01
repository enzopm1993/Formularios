using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.LavadoDesinfeccionManos
{
    public class clsDLavadoDesinfeccionManos
    {
        public List<sp_Control_Lavado_Desinfeccion_Manos> ConsultarControlLavadoDesinfeccionManos(DateTime fechaDesde,DateTime fechaHasta, int opcion)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.sp_Control_Lavado_Desinfeccion_Manos(fechaDesde, fechaHasta, opcion).ToList();
                return listado;
            }
        }

        public int GuardarModificarControlLavadoDesinfeccionManos(CC_CONTROL_LAVADO_DESINFECCION_MANOS GuardarModigicar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CONTROL_LAVADO_DESINFECCION_MANOS.FirstOrDefault(x => x.IdDesinfeccionManos == GuardarModigicar.IdDesinfeccionManos && x.EstadoRegistro == GuardarModigicar.EstadoRegistro);
                if (model != null)
                {
                    model.Fecha = GuardarModigicar.Fecha;
                    model.Observacion = GuardarModigicar.Observacion;
                    model.EstadoRegistro = GuardarModigicar.EstadoRegistro;
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_CONTROL_LAVADO_DESINFECCION_MANOS.Add(GuardarModigicar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int EliminarControlLavadoDesinfeccionManos(CC_CONTROL_LAVADO_DESINFECCION_MANOS GuardarModigicar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CONTROL_LAVADO_DESINFECCION_MANOS.FirstOrDefault(x => x.IdDesinfeccionManos == GuardarModigicar.IdDesinfeccionManos);
                if (model != null)
                {
                    model.EstadoRegistro = GuardarModigicar.EstadoRegistro;
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }               
                return valor;
            }
        }
        //-----------------------------------------------------------DETALLE----------------------------------------------------------------------------
        public List<sp_Control_Lavado_Desinfeccion_Manos_Detalle> ConsultarControlLavadoDesinfeccionManosDetalle(int IdDesinfeccionManos, int opcion)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.sp_Control_Lavado_Desinfeccion_Manos_Detalle(IdDesinfeccionManos, opcion).ToList();
                return listado;
            }
        }

        public int GuardarModificarControlLavadoDesinfeccionManosDetalle(CC_CONTROL_LAVADO_DESINFECCION_MANOS_DETALLE GuardarModigicar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CONTROL_LAVADO_DESINFECCION_MANOS_DETALLE.FirstOrDefault(x => x.IdDesinfeccionManosDetalle == GuardarModigicar.IdDesinfeccionManosDetalle && x.EstadoRegistro == GuardarModigicar.EstadoRegistro);
                if (model != null)
                {
                    model.IdDesinfeccionManos = GuardarModigicar.IdDesinfeccionManos;
                    model.Hora = GuardarModigicar.Hora;
                    model.CodigoLinea = GuardarModigicar.CodigoLinea;
                    model.EstadoCumplimiento = GuardarModigicar.EstadoCumplimiento;
                    model.EstadoRegistro = GuardarModigicar.EstadoRegistro;
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_CONTROL_LAVADO_DESINFECCION_MANOS_DETALLE.Add(GuardarModigicar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarLavadoDesinfeccionManosDetalle(CC_CONTROL_LAVADO_DESINFECCION_MANOS_DETALLE GuardarModigicar)
        {            
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CONTROL_LAVADO_DESINFECCION_MANOS_DETALLE.FirstOrDefault(x => x.IdDesinfeccionManosDetalle == GuardarModigicar.IdDesinfeccionManosDetalle);
                if (model != null)
                {
                    model.EstadoRegistro = GuardarModigicar.EstadoRegistro;
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;                    
                    db.SaveChanges();
                    return 1;
                } else  return 0;
            }
        }
    }
}