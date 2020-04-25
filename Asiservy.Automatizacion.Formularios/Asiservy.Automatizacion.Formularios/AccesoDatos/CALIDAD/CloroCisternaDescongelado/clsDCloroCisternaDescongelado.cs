using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.CloroCisternaDescongelado
{
    public class clsDCloroCisternaDescongelado
    {
        public List<sp_Reporte_CloroCisternaDescongelado> Consultar_ReporteCloroCisternaDescongelado(DateTime fecha)
        {
            using (ASIS_PRODEntities db= new ASIS_PRODEntities())
            {
                var listado = db.sp_Reporte_CloroCisternaDescongelado(fecha).ToList();
                return listado;
            }
        }
        public void GuardarModificar_ReporteCloroCisternaDescongelado(CC_CLORO_CISTERNA_DESCONGELADO controlCloro)
        {
            using (ASIS_PRODEntities db =new ASIS_PRODEntities())
            {
                var model = db.CC_CLORO_CISTERNA_DESCONGELADO.FirstOrDefault(x => x.IdCloroCisterna == controlCloro.IdCloroCisterna || (x.Fecha==controlCloro.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
                if (model !=null)
                {
                    model.Observaciones = controlCloro.Observaciones;
                    model.AprobadoPor = controlCloro.AprobadoPor;
                    model.FechaAprobacion = controlCloro.FechaAprobacion;
                    model.FechaModificacionLog = controlCloro.FechaIngresoLog;
                    model.TerminalModificacionLog = controlCloro.TerminalIngresoLog;
                    model.UsuarioModificacionLog = controlCloro.UsuarioIngresoLog;
                }
                else
                {
                    db.CC_CLORO_CISTERNA_DESCONGELADO.Add(controlCloro);
                }
                db.SaveChanges();
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
        public List<sp_Reporte_CloroCisternaDescongeladoDetalle> Consultar_ReporteCloroCisternaDescongeladoDetalle(DateTime fecha, long IdCloroCisterna)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.sp_Reporte_CloroCisternaDescongeladoDetalle(fecha, IdCloroCisterna).ToList();
                return listado;
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
        public List<sp_Reporte_CloroCisternaDescongeladoBandeja> Consultar_PendientesCloroCisternaDescongelado()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.sp_Reporte_CloroCisternaDescongeladoBandeja().ToList();
                return listado;
            }
        }

        public List<sp_Reporte_CloroCisternaDescongeladoBandejaAprobados> Consultar_AprobadosCloroCisternaDescongelado(DateTime fechaInicio, DateTime fechaFin)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.sp_Reporte_CloroCisternaDescongeladoBandejaAprobados(fechaInicio, fechaFin).ToList();
                return listado;
            }
        }
        public void Aprobar_ReporteCloroCisternaDescongelado(CC_CLORO_CISTERNA_DESCONGELADO controlCloro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CLORO_CISTERNA_DESCONGELADO.FirstOrDefault(x => x.IdCloroCisterna == controlCloro.IdCloroCisterna || (x.Fecha == controlCloro.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
                if (model != null)
                {
                    model.Observaciones = controlCloro.Observaciones;
                    model.EstadoReporte = controlCloro.EstadoReporte;
                    model.AprobadoPor = controlCloro.AprobadoPor;
                    model.FechaAprobacion = controlCloro.FechaAprobacion;
                    model.FechaModificacionLog = controlCloro.FechaIngresoLog;
                    model.TerminalModificacionLog = controlCloro.TerminalIngresoLog;
                    model.UsuarioModificacionLog = controlCloro.UsuarioIngresoLog;
                }
                else
                {
                    db.CC_CLORO_CISTERNA_DESCONGELADO.Add(model);
                }
                db.SaveChanges();
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
    }
}