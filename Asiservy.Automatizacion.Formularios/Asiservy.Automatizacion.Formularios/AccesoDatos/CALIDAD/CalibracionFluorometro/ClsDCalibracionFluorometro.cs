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
                var validarNombre = db.CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT.FirstOrDefault(x=> x.NombEstandar.Replace(" ", string.Empty).ToUpper()==guardarModificar.NombEstandar.Replace(" ", string.Empty).ToUpper());
               
                var model = db.CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT.FirstOrDefault(x => x.IdEstandar == guardarModificar.IdEstandar);
                if (validarNombre != null && validarNombre.IdEstandar == guardarModificar.IdEstandar)
                {
                    if (model.EstadoRegistro == "I")
                    {
                        valor = 2;
                        return valor;
                    }
                    else
                    {
                        model.Orden = guardarModificar.Orden;
                        model.DatoNumerico = guardarModificar.DatoNumerico;
                        model.DescEstandar = guardarModificar.DescEstandar;
                        model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                        model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                        model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                        valor = 1;
                    }
                }
                else if(validarNombre==null)
                {                   
                    db.CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT.Add(guardarModificar);
                   
                }
                else
                {
                    valor = 4;
                    return valor;
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

        public List<CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT> ListarEstandar(int op=0)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT.Take(200).OrderBy(x=> x.Orden).ToList();
                List<CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT> listaEstandar = new List<CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT>();
                CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT estadar;
                if (op==1)
                {
                    lista = db.CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT.Where(c=> c.EstadoRegistro==clsAtributos.EstadoRegistroActivo).Take(200).OrderBy(x => x.Orden).ToList();                    
                }                
                foreach (var item in lista)
                {
                    estadar = new CC_CALIBRACION_FLUOROMETRO_ESTANDAR_MANT();
                    estadar.IdEstandar = item.IdEstandar;
                    estadar.NombEstandar = item.NombEstandar;
                    estadar.DescEstandar = item.DescEstandar;
                    estadar.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    estadar.FechaIngresoLog = item.FechaIngresoLog;
                    estadar.EstadoRegistro = item.EstadoRegistro;
                    estadar.DatoNumerico = item.DatoNumerico;
                    estadar.Orden = item.Orden;
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
                        if (guardarModificar.FechaHora != DateTime.MinValue)
                        {
                            model.FechaHora = guardarModificar.FechaHora;
                            model.CoeficienteDeterminacion = guardarModificar.CoeficienteDeterminacion;
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
                    if (guardarModificar.FechaHora != DateTime.MinValue)
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

        //public int EliminarCalibracionFluorDetalle(CC_CALIBRACION_FLUOROMETRO_DET guardarModificar)
        //{
        //    int valor = 0;
        //    using (ASIS_PRODEntities db = new ASIS_PRODEntities())
        //    {
        //        var model = db.CC_CALIBRACION_FLUOROMETRO_DET.FirstOrDefault(x => x.IdCalibracionFluorDetalle == guardarModificar.IdCalibracionFluorDetalle);
        //        if (model != null)
        //        {
        //            model.EstadoRegistro = guardarModificar.EstadoRegistro;
        //            model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
        //            model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
        //            model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
        //            valor = 1;
        //            db.SaveChanges();
        //        }
        //        return valor;
        //    }
        //}

        public CC_CALIBRACION_FLUOROMETRO_CTRL ConsultarCalibracionFluorIdFecha(int idCalibracionFluor, DateTime fecha)
        {
            CC_CALIBRACION_FLUOROMETRO_CTRL calibracionID;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                calibracionID = new CC_CALIBRACION_FLUOROMETRO_CTRL();
                if (idCalibracionFluor != 0 && fecha==DateTime.MinValue)
                {
                    calibracionID = db.CC_CALIBRACION_FLUOROMETRO_CTRL.FirstOrDefault(c => c.IdCalibracionFluor == idCalibracionFluor
                                                                        && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else if(fecha!=DateTime.MinValue)
                {
                    calibracionID = db.CC_CALIBRACION_FLUOROMETRO_CTRL.FirstOrDefault(c => c.FechaHora.Year == fecha.Year && c.FechaHora.Month==fecha.Month && c.FechaHora.Day==fecha.Day
                                                                       && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                return calibracionID;
            }
        }

        public List<CC_CALIBRACION_FLUOROMETRO_CTRL> ConsultarFluorRangoFecha(DateTime fechaDesde, DateTime fechaHasta)
        {           
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
               var lista = db.CC_CALIBRACION_FLUOROMETRO_CTRL.Where(c => c.FechaHora >= fechaDesde && c.FechaHora<=fechaHasta                                                                  
                                                                    && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                                                                    .OrderBy(x => x.FechaHora).ToList();
                List<CC_CALIBRACION_FLUOROMETRO_CTRL> listaEstandar = new List<CC_CALIBRACION_FLUOROMETRO_CTRL>();
                CC_CALIBRACION_FLUOROMETRO_CTRL estadar;
                foreach (var item in lista)
                {
                    estadar = new CC_CALIBRACION_FLUOROMETRO_CTRL();
                    estadar.IdCalibracionFluor = item.IdCalibracionFluor;
                    estadar.FechaHora = item.FechaHora;
                    estadar.CoeficienteDeterminacion = item.CoeficienteDeterminacion;
                    estadar.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    estadar.FechaIngresoLog = item.FechaIngresoLog;
                    estadar.EstadoRegistro = item.EstadoRegistro;
                    listaEstandar.Add(estadar);
                }
                return listaEstandar;
            }
        }

        public List<CC_CALIBRACION_FLUOROMETRO_CTRL> BandejaConsultarCalibracionFluorometro(bool estadoReReporte, DateTime fechaDesde, DateTime fechaHasta)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<CC_CALIBRACION_FLUOROMETRO_CTRL> lista;
                if(estadoReReporte)
                {
                     lista = db.CC_CALIBRACION_FLUOROMETRO_CTRL.Where(c => c.FechaHora >= fechaDesde && c.FechaHora <= fechaHasta
                                                                    && c.EstadoReporte == estadoReReporte
                                                                    && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                                                                    .OrderBy(x => x.FechaHora).ToList();
                }
                else
                {
                     lista = db.CC_CALIBRACION_FLUOROMETRO_CTRL.Where(c => c.EstadoReporte == estadoReReporte
                                                                        && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                                                                        .OrderBy(x => x.FechaHora).ToList();
                }
                List<CC_CALIBRACION_FLUOROMETRO_CTRL> listaCabecera = new List<CC_CALIBRACION_FLUOROMETRO_CTRL>();
                CC_CALIBRACION_FLUOROMETRO_CTRL cabecera;
                foreach (var item in lista)
                {
                    cabecera = new CC_CALIBRACION_FLUOROMETRO_CTRL();
                    cabecera.IdCalibracionFluor = item.IdCalibracionFluor;
                    cabecera.FechaHora = item.FechaHora;
                    cabecera.EstadoReporte = item.EstadoReporte;
                    cabecera.CoeficienteDeterminacion = item.CoeficienteDeterminacion;
                    cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    cabecera.FechaIngresoLog = item.FechaIngresoLog;
                    cabecera.EstadoRegistro = item.EstadoRegistro;
                    listaCabecera.Add(cabecera);
                }
                return listaCabecera;
            }
        }

        //public List<CC_CALIBRACION_FLUOROMETRO_DET> ConsultarCalibreFluorDetalleID(int idCalibracionFluorDetalle)
        //{
        //    using (ASIS_PRODEntities db = new ASIS_PRODEntities())
        //    {
        //        var lista = db.CC_CALIBRACION_FLUOROMETRO_DET.Where(c => c.IdCalibracionFluorDetalle== idCalibracionFluorDetalle && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
        //                                                            .OrderBy(x => x.IdEstandar).ToList();
        //        return lista;
        //    }
        //}

        public List<sp_Calibracion_Fluorometro> ConsultarRanfoFechaInner(DateTime? fechaDesde, DateTime? fechaHasta, int idCalibracionFluor, int op)
        {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                var lista = db.sp_Calibracion_Fluorometro(fechaDesde, fechaHasta, idCalibracionFluor, op).ToList();
                return lista;
            }
        }
    }
}