using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.CalibracionFluorometro
{
    public class ClsDCalibracionFluorometro
    {
        public int GuardarModificarEstandar(CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT.FirstOrDefault(x => x.IdEstandar == guardarModificar.IdEstandar);
                if (model != null)
                {

                    if (model.EstadoRegistro == "I")
                    {
                        valor = 2;
                        return valor;
                    }
                    else
                    {
                        model.NombEstandar = guardarModificar.NombEstandar;
                        model.DescEstandar = guardarModificar.DescEstandar;
                        model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                        model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                        model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                        valor = 1;
                    }
                }
                else
                {
                    db.CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarEstandar(CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT.FirstOrDefault(x => x.IdEstandar == guardarModificar.IdEstandar);
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

        public List<CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT> ListarEstandar()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT.Take(200).OrderBy(x=> x.NombEstandar).ToList();
                List<CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT> listaEstandar = new List<CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT>();
                CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT estadar;
                foreach (var item in lista)
                {
                    estadar = new CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT();
                    estadar.IdEstandar = item.IdEstandar;
                    estadar.NombEstandar = item.NombEstandar;
                    estadar.DescEstandar = item.DescEstandar;
                    estadar.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    estadar.FechaIngresoLog = item.FechaIngresoLog;
                    estadar.EstadoRegistro = item.EstadoRegistro;
                    listaEstandar.Add(estadar);
                }
                return listaEstandar;
            }
        }

        public int GuardarModificarCalibracionFluor(CC_CALIBRACION_FLUOROMETRO_CTRL guardarModificar, bool siAprobar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CALIBRACION_FLUOROMETRO_CTRL.FirstOrDefault(x => x.IdCalibracionFluor == guardarModificar.IdCalibracionFluor && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    if (siAprobar)
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
                            model.Observacion = guardarModificar.Observacion;
                            model.Observacion = guardarModificar.Observacion;
                            valor = 1;//ACTUALIZAR
                        }
                        else valor = 3;
                    }
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                }
                else
                {
                    if (guardarModificar.Fecha != DateTime.MinValue)
                    {
                        db.CC_CALIBRACION_FLUOROMETRO_CTRL.Add(guardarModificar);
                    }
                    else valor = 3;
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarCalibracionFluor(CC_CALIBRACION_FLUOROMETRO_CTRL guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CALIBRACION_FLUOROMETRO_CTRL.FirstOrDefault(x => x.IdCalibracionFluor == guardarModificar.IdCalibracionFluor);
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

        public int GuardarModificarCalibracionFluorDetalle(CC_CALIBRACION_FLUOROMETRO_DET guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CALIBRACION_FLUOROMETRO_DET.FirstOrDefault(x => x.IdCalibracionFluorDetalle == guardarModificar.IdCalibracionFluorDetalle &&
                  x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.ValorEstandar = guardarModificar.ValorEstandar;
                    model.Fecha = guardarModificar.Fecha;
                    model.Hora = guardarModificar.Hora;
                    model.CoeficienteDeterminacion = guardarModificar.CoeficienteDeterminacion;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_CALIBRACION_FLUOROMETRO_DET.Add(guardarModificar);
                }                
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarCalibracionFluorDetalle(CC_CALIBRACION_FLUOROMETRO_DET guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CALIBRACION_FLUOROMETRO_DET.FirstOrDefault(x => x.IdCalibracionFluorDetalle == guardarModificar.IdCalibracionFluorDetalle);
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

        public CC_CALIBRACION_FLUOROMETRO_CTRL ConsultarCalibracionFluorIdFecha(int idCalibracionFluor, DateTime? fecha)
        {
            CC_CALIBRACION_FLUOROMETRO_CTRL calibracionID;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                calibracionID = new CC_CALIBRACION_FLUOROMETRO_CTRL();
                if (idCalibracionFluor != 0 && fecha==null)
                {
                    calibracionID = db.CC_CALIBRACION_FLUOROMETRO_CTRL.FirstOrDefault(c => c.IdCalibracionFluor == idCalibracionFluor
                                                                        && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else if(fecha!=null)
                {
                    calibracionID = db.CC_CALIBRACION_FLUOROMETRO_CTRL.FirstOrDefault(c => c.Fecha == fecha
                                                                       && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                return calibracionID;
            }
        }

        public List<CC_CALIBRACION_FLUOROMETRO_CTRL> BandejaConsultarHigieneControl(bool estadoReReporte, DateTime fechaDesde, DateTime fechaHasta)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_CALIBRACION_FLUOROMETRO_CTRL.Where(c => c.Fecha >= fechaDesde && c.Fecha <= fechaHasta 
                                                                    && c.EstadoReporte == estadoReReporte
                                                                    && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                                                                    .OrderBy(x=> x.Fecha).ToList();               
                return lista;
            }
        }

        public List<CC_CALIBRACION_FLUOROMETRO_DET> ConsultarCalibreFluorDetalleID(int idCalibracionFluorDetalle)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_CALIBRACION_FLUOROMETRO_DET.Where(c => c.IdCalibracionFluorDetalle== idCalibracionFluorDetalle && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                                                                    .OrderBy(x => x.Fecha).ToList();
                return lista;
            }
        }
    }
}