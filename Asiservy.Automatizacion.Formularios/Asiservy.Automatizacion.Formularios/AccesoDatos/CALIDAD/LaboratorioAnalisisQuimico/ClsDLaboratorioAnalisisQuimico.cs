using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.LaboratorioAnalisisQuimico
{
    public class ClsDLaboratorioAnalisisQuimico
    {
        public int GuardarModificarAnalisisQuimico(CC_ANALISIS_QUIMICO_PRECOCCION_CTRL guardarModificar, int siAprobar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.FirstOrDefault(x => x.Fecha == guardarModificar.Fecha && x.Turno == guardarModificar.Turno && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (siAprobar != 1 && validarNombreRepetido != null && guardarModificar.IdAnalisis != validarNombreRepetido.IdAnalisis)
                {
                    valor = 5;
                    return valor;
                }
                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.FirstOrDefault(x => x.IdAnalisis == guardarModificar.IdAnalisis && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
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
                            model.ObservacionCtrl = guardarModificar.ObservacionCtrl;
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
                        db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.Add(guardarModificar);
                    }
                    else valor = 3;
                }
                db.SaveChanges();
                return valor;
            }
        }
        public CC_ANALISIS_QUIMICO_PRECOCCION_CTRL ConsultarEstadoReporte(int idAnalisis, DateTime fechaControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                CC_ANALISIS_QUIMICO_PRECOCCION_CTRL listado;
                if (idAnalisis == 0 && fechaControl > DateTime.MinValue)
                {
                    listado = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                        && x.Fecha.Day == fechaControl.Day
                                                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else
                {
                    listado = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.FirstOrDefault(x => x.IdAnalisis == idAnalisis && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                CC_ANALISIS_QUIMICO_PRECOCCION_CTRL cabecera;
                if (listado != null)
                {
                    cabecera = new CC_ANALISIS_QUIMICO_PRECOCCION_CTRL();
                    cabecera.IdAnalisis = listado.IdAnalisis;
                    cabecera.Fecha = listado.Fecha;
                    cabecera.ObservacionCtrl = listado.ObservacionCtrl;
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
        public CC_ANALISIS_QUIMICO_PRECOCCION_CTRL ConsultarCabeceraTurno(string turno, DateTime fechaControl)
        {
            CC_ANALISIS_QUIMICO_PRECOCCION_CTRL listado;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (turno == null)
                {
                    listado = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.Include(x=>x.CC_ANALISIS_QUIMICO_PRECOCCION_DET).FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                    && x.Fecha.Day == fechaControl.Day
                                                                                    && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else
                {
                    listado = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.Include(x => x.CC_ANALISIS_QUIMICO_PRECOCCION_DET).FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                        && x.Fecha.Day == fechaControl.Day && x.Turno == turno
                                                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
            }
            return listado;
        }
        public int EliminarAnalisisQuimico(CC_ANALISIS_QUIMICO_PRECOCCION_CTRL guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.FirstOrDefault(x => x.IdAnalisis == guardarModificar.IdAnalisis);
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
        public int GuardarModificarDetalle(CC_ANALISIS_QUIMICO_PRECOCCION_DET guardarModificar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_DET.FirstOrDefault(x => x.IdAnalisisDetalle == guardarModificar.IdAnalisisDetalle && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.ObservacionDet = guardarModificar.ObservacionDet;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;//ACTUALIZAR                   
                }
                else
                {
                    db.CC_ANALISIS_QUIMICO_PRECOCCION_DET.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int EliminarDetalle(CC_ANALISIS_QUIMICO_PRECOCCION_DET guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_DET.Where(x => x.IdAnalisisDetalle == guardarmodificar.IdAnalisisDetalle && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        item.EstadoRegistro = guardarmodificar.EstadoRegistro;
                        item.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                        item.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                        item.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                        valor = 1;
                    }
                    db.SaveChanges();
                }
                return valor;
            }
        }
    }
}