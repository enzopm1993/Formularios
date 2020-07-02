using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.CloroCisternaDescongelado
{
    public class clsDCloroCisternaDescongelado
    {
       
        public CC_CLORO_CISTERNA_DESCONGELADO ConsultarCabeceraTurno(string turno, DateTime fechaControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                CC_CLORO_CISTERNA_DESCONGELADO listado;

                if (turno == "0")
                {
                    listado = db.CC_CLORO_CISTERNA_DESCONGELADO.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                   && x.Fecha.Day == fechaControl.Day
                                                                                   && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else
                {
                    listado = db.CC_CLORO_CISTERNA_DESCONGELADO.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                        && x.Fecha.Day == fechaControl.Day && x.Turno == turno
                                                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                CC_CLORO_CISTERNA_DESCONGELADO cabecera;
                if (listado != null)
                {
                    cabecera = new CC_CLORO_CISTERNA_DESCONGELADO();
                    cabecera.IdCloroCisterna = listado.IdCloroCisterna;
                    cabecera.Fecha = listado.Fecha;
                    cabecera.Observaciones = listado.Observaciones;
                    cabecera.EstadoReporte = listado.EstadoReporte;
                    cabecera.FechaIngresoLog = listado.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = listado.UsuarioIngresoLog;
                    cabecera.FechaAprobacion = listado.FechaAprobacion;
                    cabecera.AprobadoPor = listado.AprobadoPor;
                    cabecera.Turno = listado.Turno;
                    return cabecera;
                }
                return listado;

            }
        }

        public int GuardarModificar_ReporteCloroCisternaDescongelado(CC_CLORO_CISTERNA_DESCONGELADO controlCloro)
        {
            using (ASIS_PRODEntities db =new ASIS_PRODEntities())
            {
                int valor = 0;
                //var validarNombreRepetido = db.CC_CLORO_CISTERNA_DESCONGELADO.FirstOrDefault(x => x.Turno == controlCloro.Turno && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                //if (validarNombreRepetido != null && controlCloro.IdCloroCisterna != validarNombreRepetido.IdCloroCisterna)
                //{
                //    valor = 3;
                //    return valor;
                //}
                var model = db.CC_CLORO_CISTERNA_DESCONGELADO.FirstOrDefault(x => x.IdCloroCisterna == controlCloro.IdCloroCisterna  && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model !=null)
                {
                    model.Turno = controlCloro.Turno;
                    model.Observaciones = controlCloro.Observaciones;
                    model.AprobadoPor = controlCloro.AprobadoPor;
                    model.FechaAprobacion = controlCloro.FechaAprobacion;
                    model.FechaModificacionLog = controlCloro.FechaIngresoLog;
                    model.TerminalModificacionLog = controlCloro.TerminalIngresoLog;
                    model.UsuarioModificacionLog = controlCloro.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {                   
                    db.CC_CLORO_CISTERNA_DESCONGELADO.Add(controlCloro);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int Eliminar_ReporteCloroCisternaDescongelado(CC_CLORO_CISTERNA_DESCONGELADO controlCloro)
        {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                var model = db.CC_CLORO_CISTERNA_DESCONGELADO.FirstOrDefault(x=> x.IdCloroCisterna==controlCloro.IdCloroCisterna);
                if (model!=null)
                {
                    model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    model.FechaModificacionLog = controlCloro.FechaModificacionLog;
                    model.FechaAprobacion = controlCloro.FechaAprobacion;
                    model.TerminalModificacionLog = controlCloro.TerminalModificacionLog;
                    model.UsuarioModificacionLog = controlCloro.UsuarioModificacionLog;
                    db.SaveChanges();
                    return 1;
                }
                return 0;
            }
        }
      
        public List<CC_CLORO_CISTERNA_DESCONGELADO> ConsultarBadejaEstado(DateTime fechaDesde, DateTime FechaHasta, bool estadoReporte)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<CC_CLORO_CISTERNA_DESCONGELADO> listado;
                if (estadoReporte)
                {
                    listado = db.CC_CLORO_CISTERNA_DESCONGELADO.Where(x => x.EstadoReporte == estadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                              && x.Fecha >= fechaDesde && x.Fecha <= FechaHasta).ToList();
                }
                else
                {
                    listado = db.CC_CLORO_CISTERNA_DESCONGELADO.Where(x => x.EstadoReporte == estadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                               && x.Fecha <= FechaHasta).ToList();
                }
                CC_CLORO_CISTERNA_DESCONGELADO cabecera;
                List<CC_CLORO_CISTERNA_DESCONGELADO> listaCabecera = new List<CC_CLORO_CISTERNA_DESCONGELADO>();
                if (listado.Any())
                {
                    foreach (var item in listado)
                    {
                        cabecera = new CC_CLORO_CISTERNA_DESCONGELADO();
                        cabecera.IdCloroCisterna = item.IdCloroCisterna;
                        cabecera.Fecha = item.Fecha;
                        cabecera.Observaciones = item.Observaciones;
                        cabecera.EstadoReporte = item.EstadoReporte;
                        cabecera.FechaIngresoLog = item.FechaIngresoLog;
                        cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                        cabecera.FechaAprobacion = item.FechaAprobacion;
                        cabecera.AprobadoPor = item.AprobadoPor;
                        cabecera.Turno = item.Turno;
                        listaCabecera.Add(cabecera);
                    }
                }
                return listaCabecera;
            }
        }
        public List<CC_CLORO_CISTERNA_DESCONGELADO_DETALLE> ConsultarDetalle (int idCloroCisterna)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {                
                var listado = db.CC_CLORO_CISTERNA_DESCONGELADO_DETALLE.Where(x => x.IdCloroCisternaCabecera == idCloroCisterna && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                CC_CLORO_CISTERNA_DESCONGELADO_DETALLE cabecera;
                List<CC_CLORO_CISTERNA_DESCONGELADO_DETALLE> listadoDetalle = new List<CC_CLORO_CISTERNA_DESCONGELADO_DETALLE>();
                if (listado != null)
                {
                    foreach (var item in listado)
                    {
                        cabecera = new CC_CLORO_CISTERNA_DESCONGELADO_DETALLE();
                        cabecera.IdCloroCisternaDetalle = item.IdCloroCisternaDetalle;
                        cabecera.IdCloroCisternaCabecera = item.IdCloroCisternaCabecera;
                        cabecera.Hora = item.Hora;
                        cabecera.Observaciones = item.Observaciones;
                        cabecera.Ppm_Cloro = item.Ppm_Cloro;
                        cabecera.FechaIngresoLog = item.FechaIngresoLog;
                        cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                        cabecera.Cisterna = item.Cisterna;
                        listadoDetalle.Add(cabecera);
                    }                 
                }
                return listadoDetalle;
            }
        }

        public void GuardarModificar_ReporteCloroCisternaDescongeladoDetalle(CC_CLORO_CISTERNA_DESCONGELADO_DETALLE controlCloro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CLORO_CISTERNA_DESCONGELADO_DETALLE.FirstOrDefault(x => x.IdCloroCisternaDetalle == controlCloro.IdCloroCisternaDetalle && (x.IdCloroCisternaCabecera ==controlCloro.IdCloroCisternaCabecera && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
                if (model != null)
                {
                    model.Observaciones = controlCloro.Observaciones;
                    model.Hora = controlCloro.Hora;
                    model.Ppm_Cloro = controlCloro.Ppm_Cloro;
                    model.Cisterna = controlCloro.Cisterna;                    
                    model.FechaModificacionLog = controlCloro.FechaIngresoLog;
                    model.TerminalModificacionLog = controlCloro.TerminalIngresoLog;
                    model.UsuarioModificacionLog = controlCloro.UsuarioIngresoLog;
                }
                else
                {                    
                    db.CC_CLORO_CISTERNA_DESCONGELADO_DETALLE.Add(controlCloro);
                }
                db.SaveChanges();
            }
        }
        public int Eliminar_ReporteCloroCisternaDescongeladoDetalle(CC_CLORO_CISTERNA_DESCONGELADO_DETALLE controlCloro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CLORO_CISTERNA_DESCONGELADO_DETALLE.FirstOrDefault(x => x.IdCloroCisternaDetalle == controlCloro.IdCloroCisternaDetalle);
                if (model != null)
                {
                    model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    model.FechaModificacionLog = controlCloro.FechaModificacionLog;
                    model.TerminalModificacionLog = controlCloro.TerminalModificacionLog;
                    model.UsuarioModificacionLog = controlCloro.UsuarioModificacionLog;
                    db.SaveChanges();
                    return 1;
                }
                return 0;
            }
        }
       
        public void Aprobar_ReporteCloroCisternaDescongelado(CC_CLORO_CISTERNA_DESCONGELADO controlCloro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CLORO_CISTERNA_DESCONGELADO.FirstOrDefault(x => x.IdCloroCisterna == controlCloro.IdCloroCisterna  && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.EstadoReporte = controlCloro.EstadoReporte;
                    model.AprobadoPor = controlCloro.AprobadoPor;
                    model.FechaAprobacion = controlCloro.FechaAprobacion;
                    db.SaveChanges();
                }               
            }
        }
        public List<sp_CloroCisternaDescongelado> ConsultarCloroCisternaRangoFecha(DateTime fechaDesde, DateTime fechaHasta, int idCloroCisterna, int op)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.sp_CloroCisternaDescongelado(fechaDesde, fechaHasta, idCloroCisterna, op).ToList();
                return listado;
            }
        }

        public List<CC_CLORO_CISTERNA_DESCONGELADO> ReporteConsultarcabecera(DateTime fechaDesde, DateTime fechaHasta)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = (from c in db.CC_CLORO_CISTERNA_DESCONGELADO
                             where (c.Fecha >= fechaDesde && c.Fecha <= fechaHasta && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                             orderby c.Fecha descending
                             select new { c.IdCloroCisterna, c.Fecha, c.EstadoReporte, c.Observaciones, c.FechaIngresoLog, c.UsuarioIngresoLog, c.FechaAprobacion, c.AprobadoPor, c.Turno }).ToList();
                List<CC_CLORO_CISTERNA_DESCONGELADO> listacabecera = new List<CC_CLORO_CISTERNA_DESCONGELADO>();
                CC_CLORO_CISTERNA_DESCONGELADO cabecera;
                foreach (var item in lista)
                {
                    cabecera = new CC_CLORO_CISTERNA_DESCONGELADO();
                    cabecera.IdCloroCisterna = item.IdCloroCisterna;
                    cabecera.Fecha = item.Fecha;
                    cabecera.EstadoReporte = item.EstadoReporte;
                    cabecera.Observaciones = item.Observaciones;
                    cabecera.FechaIngresoLog = item.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    cabecera.FechaAprobacion = item.FechaAprobacion;
                    cabecera.AprobadoPor = item.AprobadoPor;
                    cabecera.Turno = item.Turno;
                    listacabecera.Add(cabecera);
                }
                return listacabecera;
            }
        }

        public CC_CLORO_CISTERNA_DESCONGELADO ConsultarEstadoReporte(long idCloroCisterna)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = (from c in db.CC_CLORO_CISTERNA_DESCONGELADO
                             where (c.IdCloroCisterna == idCloroCisterna && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                             orderby c.Fecha descending
                             select new { c.IdCloroCisterna, c.Fecha, c.EstadoReporte, c.Observaciones, c.FechaIngresoLog, c.UsuarioIngresoLog, c.FechaAprobacion, c.AprobadoPor }).FirstOrDefault();
                
                CC_CLORO_CISTERNA_DESCONGELADO cabecera= new CC_CLORO_CISTERNA_DESCONGELADO();
               if(lista!=null)
                {
                    cabecera.IdCloroCisterna = lista.IdCloroCisterna;
                    cabecera.Fecha = lista.Fecha;
                    cabecera.EstadoReporte = lista.EstadoReporte;
                    cabecera.Observaciones = lista.Observaciones;
                    cabecera.FechaIngresoLog = lista.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = lista.UsuarioIngresoLog;
                    cabecera.FechaAprobacion = lista.FechaAprobacion;
                    cabecera.AprobadoPor = lista.AprobadoPor;
                    return cabecera;
                }
                return cabecera;
            }
        }
    }
}