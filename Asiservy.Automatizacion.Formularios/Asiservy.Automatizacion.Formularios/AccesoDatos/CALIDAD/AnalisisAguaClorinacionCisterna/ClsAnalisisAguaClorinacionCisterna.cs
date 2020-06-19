using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.AnalisisAguaClorinacionCisterna
{
    public class ClsAnalisisAguaClorinacionCisterna
    {
        public List<CC_ANALISIS_AGUA_CLORINACION_MANT> ConsultarMantenimientoCisterna(string estadoRegistro=null)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (estadoRegistro==null)
                {
                    var lista = db.CC_ANALISIS_AGUA_CLORINACION_MANT.ToList();
                    return lista;
                }
                else
                {
                    var listaActivos = db.CC_ANALISIS_AGUA_CLORINACION_MANT.Where(v=> v.EstadoRegistro==estadoRegistro).ToList();
                    return listaActivos;
                }                
            }
        }
        public int GuardarModificarMantenimientoCisterna(CC_ANALISIS_AGUA_CLORINACION_MANT guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_ANALISIS_AGUA_CLORINACION_MANT.FirstOrDefault(x => x.NombreCisterna.Replace(" ", string.Empty).ToUpper() == guardarmodificar.NombreCisterna.Replace(" ", string.Empty).ToUpper() && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
                if (validarNombreRepetido != null && guardarmodificar.NombreCisterna.Replace(" ", string.Empty).ToUpper() == validarNombreRepetido.NombreCisterna.Replace(" ", string.Empty).ToUpper()
                   && guardarmodificar.IdCisterna != validarNombreRepetido.IdCisterna)
                {
                    valor = 3;
                    return valor;
                }

                var model = db.CC_ANALISIS_AGUA_CLORINACION_MANT.FirstOrDefault(x => x.IdCisterna == guardarmodificar.IdCisterna);
                if (model != null)
                {

                    if (model.EstadoRegistro == "I")
                    {
                        valor = 2;
                        return valor;
                    }
                    else
                    {
                        model.NombreCisterna = guardarmodificar.NombreCisterna;
                        model.DescripcionCisterna = guardarmodificar.DescripcionCisterna;
                        model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                        model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                        model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                        valor = 1;
                    }
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
                var validarNombreRepetido = db.CC_ANALISIS_AGUA_CLORINACION_MANT.FirstOrDefault(x => x.NombreCisterna.Replace(" ", string.Empty).ToUpper() == guardarmodificar.NombreCisterna.Replace(" ", string.Empty).ToUpper() 
                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (validarNombreRepetido != null && guardarmodificar.EstadoRegistro==clsAtributos.EstadoRegistroActivo)
                {
                    valor = 2;
                    return valor;
                }

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
        public List<sp_Analisis_Agua_Clorinacion> ConsultarDetalleClorinacionCisterna(int idAnalisisAguaControl, int op)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listaDetalleDia = db.sp_Analisis_Agua_Clorinacion(idAnalisisAguaControl, op).ToList();
                return listaDetalleDia;               
            }
        }
        public int GuardarModificarClorinacionCisterna(CC_ANALISIS_AGUA_CLORINACION_CONTROL guardarModificar, int siAprobar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_ANALISIS_AGUA_CLORINACION_CONTROL.FirstOrDefault(x => x.IdAnalisisAguaControl == guardarModificar.IdAnalisisAguaControl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    if (siAprobar == 1)
                    {
                        model.EstadoReporte = guardarModificar.EstadoReporte;
                        model.AprobadoPor = guardarModificar.UsuarioIngresoLog;
                        model.FechaAprobado = guardarModificar.FechaAprobado;
                        valor = 2;//APRROBADO
                    }
                    else
                    {
                        if (guardarModificar.Fecha != DateTime.MinValue)
                        {
                            model.Fecha = guardarModificar.Fecha;
                            model.Observaciones = guardarModificar.Observaciones;
                            valor = 1;//ACTUALIZAR
                        }
                        else valor = 3;//ERROR DE FECHA
                    }
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                }
                else
                {
                    if (guardarModificar.Fecha != DateTime.MinValue)
                    {
                        db.CC_ANALISIS_AGUA_CLORINACION_CONTROL.Add(guardarModificar);
                    }
                    else valor = 3;
                }
                db.SaveChanges();
                return valor;
            }
        }
        public CC_ANALISIS_AGUA_CLORINACION_CONTROL ConsultarEstadoReporte(int idAnalisisAguaControl, DateTime fechaControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                CC_ANALISIS_AGUA_CLORINACION_CONTROL listado;
                if (idAnalisisAguaControl == 0 && fechaControl > DateTime.MinValue)
                {
                    listado = db.CC_ANALISIS_AGUA_CLORINACION_CONTROL.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                        && x.Fecha.Day == fechaControl.Day
                                                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else
                {
                    listado = db.CC_ANALISIS_AGUA_CLORINACION_CONTROL.FirstOrDefault(x => x.IdAnalisisAguaControl == idAnalisisAguaControl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                CC_ANALISIS_AGUA_CLORINACION_CONTROL cabecera;
                if (listado != null)
                {
                    cabecera = new CC_ANALISIS_AGUA_CLORINACION_CONTROL();
                    cabecera.IdAnalisisAguaControl = listado.IdAnalisisAguaControl;
                    cabecera.Fecha = listado.Fecha;
                    cabecera.Observaciones = listado.Observaciones;
                    cabecera.EstadoReporte = listado.EstadoReporte;
                    cabecera.FechaIngresoLog = listado.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = listado.UsuarioIngresoLog;
                    cabecera.FechaAprobado = listado.FechaAprobado;
                    cabecera.AprobadoPor = listado.AprobadoPor;
                    return cabecera;
                }
                return listado;
            }
        }
        public int EliminarClorinacionCisterna(CC_ANALISIS_AGUA_CLORINACION_CONTROL guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_ANALISIS_AGUA_CLORINACION_CONTROL.FirstOrDefault(x => x.IdAnalisisAguaControl == guardarModificar.IdAnalisisAguaControl);
                if (model != null)
                {
                    model.EstadoRegistro = guardarModificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }
        public int GuardarModificarClorinacionDetalle(CC_ANALISIS_AGUA_CLORINACION_DETALLE guardarModificar, bool siActualizar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarHora = db.CC_ANALISIS_AGUA_CLORINACION_DETALLE.FirstOrDefault(v=> v.Hora==guardarModificar.Hora && v.IdAnalisisAguaControl==guardarModificar.IdAnalisisAguaControl);
                if (!siActualizar && validarHora!=null)
                {
                    valor = 2;
                    return valor;
                }
                var model = db.CC_ANALISIS_AGUA_CLORINACION_DETALLE.FirstOrDefault(x => x.IdAnalisisAguaDetalle == guardarModificar.IdAnalisisAguaDetalle && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.IdCisterna = guardarModificar.IdCisterna;
                    model.Hora = guardarModificar.Hora;
                    model.STD = guardarModificar.STD;
                    model.DT = guardarModificar.DT;
                    model.CL = guardarModificar.CL;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;//ACTUALIZAR                   
                }
                else
                {
                    db.CC_ANALISIS_AGUA_CLORINACION_DETALLE.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int EliminarClorinacionCisternaDetalle(CC_ANALISIS_AGUA_CLORINACION_DETALLE guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_ANALISIS_AGUA_CLORINACION_DETALLE.FirstOrDefault(x => x.IdAnalisisAguaDetalle == guardarmodificar.IdAnalisisAguaDetalle);
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
        public List<CC_ANALISIS_AGUA_CLORINACION_CONTROL> ConsultarBadejaEstado(DateTime fechaDesde, DateTime FechaHasta, bool estadoReporte)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<CC_ANALISIS_AGUA_CLORINACION_CONTROL> listado;
                if (estadoReporte)
                {
                    listado = db.CC_ANALISIS_AGUA_CLORINACION_CONTROL.Where(x => x.EstadoReporte == estadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                              && x.Fecha >= fechaDesde && x.Fecha <= FechaHasta).OrderByDescending(v=>v.Fecha).ToList();
                }
                else
                {
                    listado = db.CC_ANALISIS_AGUA_CLORINACION_CONTROL.Where(x => x.EstadoReporte == estadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                               && x.Fecha <= FechaHasta).OrderByDescending(v => v.Fecha).ToList();
                }
                CC_ANALISIS_AGUA_CLORINACION_CONTROL cabecera;
                List<CC_ANALISIS_AGUA_CLORINACION_CONTROL> listaCabecera = new List<CC_ANALISIS_AGUA_CLORINACION_CONTROL>();
                if (listado.Any())
                {
                    foreach (var item in listado)
                    {
                        cabecera = new CC_ANALISIS_AGUA_CLORINACION_CONTROL();
                        cabecera.IdAnalisisAguaControl = item.IdAnalisisAguaControl;
                        cabecera.Fecha = item.Fecha;
                        cabecera.Observaciones = item.Observaciones;
                        cabecera.EstadoReporte = item.EstadoReporte;
                        cabecera.FechaIngresoLog = item.FechaIngresoLog;
                        cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                        cabecera.FechaAprobado = item.FechaAprobado;
                        cabecera.AprobadoPor = item.AprobadoPor;
                        listaCabecera.Add(cabecera);
                    }
                }
                return listaCabecera;
            }
        }
        public List<CC_ANALISIS_AGUA_CLORINACION_CONTROL> ConsultarReporteRangoFecha(DateTime fechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.CC_ANALISIS_AGUA_CLORINACION_CONTROL.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                           && x.Fecha >= fechaDesde && x.Fecha <= FechaHasta).OrderByDescending(c => c.Fecha).ToList();

                CC_ANALISIS_AGUA_CLORINACION_CONTROL cabecera;
                List<CC_ANALISIS_AGUA_CLORINACION_CONTROL> listaCabecera = new List<CC_ANALISIS_AGUA_CLORINACION_CONTROL>();
                if (listado.Any())
                {
                    foreach (var item in listado)
                    {
                        cabecera = new CC_ANALISIS_AGUA_CLORINACION_CONTROL();
                        cabecera.IdAnalisisAguaControl = item.IdAnalisisAguaControl;
                        cabecera.Fecha = item.Fecha;
                        cabecera.Observaciones = item.Observaciones;
                        cabecera.EstadoReporte = item.EstadoReporte;
                        cabecera.FechaIngresoLog = item.FechaIngresoLog;
                        cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                        cabecera.UsuarioModificacionLog = item.UsuarioModificacionLog;
                        cabecera.FechaModificacionLog = item.FechaModificacionLog;
                        cabecera.FechaAprobado = item.FechaAprobado;
                        cabecera.AprobadoPor = item.AprobadoPor;
                        listaCabecera.Add(cabecera);
                    }
                }
                return listaCabecera;
            }
        }
    }
}