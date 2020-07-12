using System;
using System.Collections.Generic;
using System.Linq;
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

        public CC_CONTROL_LAVADO_DESINFECCION_MANOS ConsultarCabeceraTurno(int turno, DateTime fechaControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                CC_CONTROL_LAVADO_DESINFECCION_MANOS listado;

                if (turno == 0)
                {
                    listado = db.CC_CONTROL_LAVADO_DESINFECCION_MANOS.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                   && x.Fecha.Day == fechaControl.Day
                                                                                   && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else
                {
                    listado = db.CC_CONTROL_LAVADO_DESINFECCION_MANOS.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                        && x.Fecha.Day == fechaControl.Day && x.Turno == turno
                                                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                CC_CONTROL_LAVADO_DESINFECCION_MANOS cabecera;
                if (listado != null)
                {
                    cabecera = new CC_CONTROL_LAVADO_DESINFECCION_MANOS();
                    cabecera.IdDesinfeccionManos = listado.IdDesinfeccionManos;
                    cabecera.Fecha = listado.Fecha;
                    cabecera.Observacion = listado.Observacion;
                    cabecera.EstadoReporte = listado.EstadoReporte;
                    cabecera.FechaIngresoLog = listado.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = listado.UsuarioIngresoLog;
                    cabecera.FechaAprobado = listado.FechaAprobado;
                    cabecera.AprobadoPor = listado.AprobadoPor;
                    cabecera.Turno = listado.Turno;
                    return cabecera;
                }
                return listado;

            }
        }

        public int GuardarModificarControlLavadoDesinfeccionManos(CC_CONTROL_LAVADO_DESINFECCION_MANOS GuardarModigicar, bool siAprobar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_CONTROL_LAVADO_DESINFECCION_MANOS.FirstOrDefault(x => x.Turno == GuardarModigicar.Turno && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (siAprobar&& validarNombreRepetido != null && GuardarModigicar.IdDesinfeccionManos != validarNombreRepetido.IdDesinfeccionManos)
                {
                    valor = 3;
                    return valor;
                }

                var model = db.CC_CONTROL_LAVADO_DESINFECCION_MANOS.FirstOrDefault(x => x.IdDesinfeccionManos == GuardarModigicar.IdDesinfeccionManos && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    if (siAprobar)
                    {
                        model.AprobadoPor = GuardarModigicar.UsuarioIngresoLog;
                        model.FechaAprobado = GuardarModigicar.FechaAprobado;
                        model.EstadoReporte = GuardarModigicar.EstadoReporte;
                        valor = 2;
                    }
                    else
                    {
                        model.Turno = GuardarModigicar.Turno;
                        model.Fecha = GuardarModigicar.Fecha;
                        model.Observacion = GuardarModigicar.Observacion;
                        model.EstadoReporte = GuardarModigicar.EstadoReporte;                       
                        valor = 1;
                    }
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
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

        public List<sp_Reporte_Lavado_Desinfeccion_Manos> ReporteControlLavadoDesinfeccion(DateTime fechaDesde, DateTime fechaHasta, int idDesinfeccionManos, int op)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.sp_Reporte_Lavado_Desinfeccion_Manos(fechaDesde, fechaHasta, idDesinfeccionManos, op).ToList();
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

        public List<CC_CONTROL_LAVADO_DESINFECCION_MANOS> ReporteConsultarcabecera(DateTime fechaDesde, DateTime fechaHasta)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = (from c in db.CC_CONTROL_LAVADO_DESINFECCION_MANOS
                             where (c.Fecha >= fechaDesde && c.Fecha <= fechaHasta && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                             orderby c.Fecha descending
                             select new { c.IdDesinfeccionManos, c.Fecha, c.Hora, c.EstadoReporte, c.Observacion, c.FechaIngresoLog, c.UsuarioIngresoLog, c.FechaAprobado, c.AprobadoPor,c.Turno }).ToList();
                List<CC_CONTROL_LAVADO_DESINFECCION_MANOS> listacabecera = new List<CC_CONTROL_LAVADO_DESINFECCION_MANOS>();
                CC_CONTROL_LAVADO_DESINFECCION_MANOS cabecera;
                foreach (var item in lista)
                {
                    cabecera = new CC_CONTROL_LAVADO_DESINFECCION_MANOS();
                    cabecera.IdDesinfeccionManos = item.IdDesinfeccionManos;
                    cabecera.Fecha = item.Fecha;
                    cabecera.Hora = item.Hora;
                    cabecera.EstadoReporte = item.EstadoReporte;
                    cabecera.Observacion = item.Observacion;
                    cabecera.FechaIngresoLog = item.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    cabecera.FechaAprobado = item.FechaAprobado;
                    cabecera.AprobadoPor = item.AprobadoPor;
                    cabecera.Turno = item.Turno;
                    listacabecera.Add(cabecera);
                }
                return listacabecera;
            }
        }

        public CC_CONTROL_LAVADO_DESINFECCION_MANOS ConsultarEstadoReporte(int idDesinfeccionManos)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.CC_CONTROL_LAVADO_DESINFECCION_MANOS.FirstOrDefault(x=> x.IdDesinfeccionManos== idDesinfeccionManos && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
                return listado;
            }
        }
    }
}