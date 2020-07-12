//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Asiservy.Automatizacion.Datos.Datos;

//namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.LaboratorioAnalisisQuimico
//{
//    public class ClsDLaboratorioAnalisisQuimico
//    {
//        public List<CC_ANALISIS_QUIMICO_PRECOCCION_TURNO> ConsultarMantenimiento(string estadoRegistro = null)
//        {
//            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
//            {
//                if (estadoRegistro == null)
//                {
//                    var lista = db.CC_ANALISIS_QUIMICO_PRECOCCION_TURNO.ToList();
//                    return lista;
//                }
//                else
//                {
//                    var listaVerivicacion = db.CC_ANALISIS_QUIMICO_PRECOCCION_TURNO.Where(v => v.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
//                    return listaVerivicacion;
//                }               
//            }
//        }
//        public int GuardarModificarMantenimiento(CC_ANALISIS_QUIMICO_PRECOCCION_TURNO guardarmodificar)
//        {
//            int valor = 0;
//            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
//            {
//                var validarNombreRepetido = db.CC_ANALISIS_QUIMICO_PRECOCCION_TURNO.FirstOrDefault(x => x.Nombre.Replace(" ", string.Empty).ToUpper() == guardarmodificar.Nombre.Replace(" ", string.Empty).ToUpper() && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
//                if (validarNombreRepetido != null && guardarmodificar.IdTurno != validarNombreRepetido.IdTurno)
//                {
//                    valor = 3;
//                    return valor;
//                }

//                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_TURNO.FirstOrDefault(x => x.IdTurno == guardarmodificar.IdTurno);
//                if (model != null)
//                {
//                    if (model.EstadoRegistro == "I")
//                    {
//                        valor = 2;
//                        return valor;
//                    }
//                    else
//                    {
//                        model.Nombre = guardarmodificar.Nombre;
//                        model.DescripcionMant = guardarmodificar.DescripcionMant;
//                        model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
//                        model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
//                        model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
//                        valor = 1;
//                    }
//                }
//                else
//                {
//                    db.CC_ANALISIS_QUIMICO_PRECOCCION_TURNO.Add(guardarmodificar);
//                }
//                db.SaveChanges();
//                return valor;
//            }
//        }
//        public int EliminarMantenimiento(CC_ANALISIS_QUIMICO_PRECOCCION_TURNO guardarmodificar)
//        {
//            int valor = 0;
//            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
//            {
//                var validarNombreRepetido = db.CC_ANALISIS_QUIMICO_PRECOCCION_TURNO.FirstOrDefault(x => x.Nombre.Replace(" ", string.Empty).ToUpper() == guardarmodificar.Nombre.Replace(" ", string.Empty).ToUpper()
//                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
//                if (validarNombreRepetido != null && guardarmodificar.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
//                {
//                    valor = 2;
//                    return valor;
//                }

//                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_TURNO.FirstOrDefault(x => x.IdTurno == guardarmodificar.IdTurno);
//                if (model != null)
//                {
//                    model.EstadoRegistro = guardarmodificar.EstadoRegistro;
//                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
//                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
//                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
//                    valor = 1;
//                    db.SaveChanges();
//                }
//                return valor;
//            }
//        }
//        public int GuardarModificarAnalisisQuimico(CC_ANALISIS_QUIMICO_PRECOCCION_CTRL guardarModificar, int siAprobar)
//        {
//            int valor = 0;//GUARDDADO NUEVO
//            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
//            {
//                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.FirstOrDefault(x => x.IdAnalisis == guardarModificar.IdAnalisis && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
//                if (model != null)
//                {
//                    if (siAprobar == 1)
//                    {
//                        model.EstadoReporte = guardarModificar.EstadoReporte;
//                        model.AprobadoPor = guardarModificar.UsuarioIngresoLog;
//                        model.FechaAprobado = guardarModificar.FechaAprobado;
//                        valor = 2;//APRROBADO
//                    }
//                    else
//                    {
//                        if (guardarModificar.Fecha != DateTime.MinValue)
//                        {
//                            model.Fecha = guardarModificar.Fecha;
//                            model.ObservacionCtrl = guardarModificar.ObservacionCtrl;
//                            valor = 1;//ACTUALIZAR
//                        }
//                        else valor = 3;//ERROR DE FECHA
//                    }
//                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
//                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
//                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
//                }
//                else
//                {
//                    if (guardarModificar.Fecha != DateTime.MinValue)
//                    {
//                        db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.Add(guardarModificar);
//                    }
//                    else valor = 3;
//                }
//                db.SaveChanges();
//                return valor;
//            }
//        }
//        public CC_ANALISIS_QUIMICO_PRECOCCION_CTRL ConsultarEstadoReporte(int idAnalisis, DateTime fechaControl)
//        {
//            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
//            {
//                CC_ANALISIS_QUIMICO_PRECOCCION_CTRL listado;
//                if (idAnalisis == 0 && fechaControl > DateTime.MinValue)
//                {
//                    listado = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
//                                                                                        && x.Fecha.Day == fechaControl.Day
//                                                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
//                }
//                else
//                {
//                    listado = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.FirstOrDefault(x => x.IdAnalisis == idAnalisis && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
//                }
//                CC_ANALISIS_QUIMICO_PRECOCCION_CTRL cabecera;
//                if (listado != null)
//                {
//                    cabecera = new CC_ANALISIS_QUIMICO_PRECOCCION_CTRL();
//                    cabecera.IdAnalisis = listado.IdAnalisis;
//                    cabecera.Fecha = listado.Fecha;
//                    cabecera.ObservacionCtrl = listado.ObservacionCtrl;
//                    cabecera.EstadoReporte = listado.EstadoReporte;
//                    cabecera.FechaIngresoLog = listado.FechaIngresoLog;
//                    cabecera.UsuarioIngresoLog = listado.UsuarioIngresoLog;
//                    cabecera.FechaAprobado = listado.FechaAprobado;
//                    cabecera.AprobadoPor = listado.AprobadoPor;
//                    return cabecera;
//                }
//                return listado;
//            }
//        }
//        public int EliminarAnalisisQuimico(CC_ANALISIS_QUIMICO_PRECOCCION_CTRL guardarModificar)
//        {
//            int valor = 0;
//            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
//            {
//                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.FirstOrDefault(x => x.IdAnalisis == guardarModificar.IdAnalisis);
//                if (model != null)
//                {
//                    model.EstadoRegistro = guardarModificar.EstadoRegistro;
//                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
//                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
//                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
//                    valor = 1;
//                    db.SaveChanges();
//                }
//                return valor;
//            }
//        }
//        public int GuardarModificarDetalle(CC_ANALISIS_QUIMICO_PRECOCCION_DET guardarModificar)
//        {
//            int valor = 0;//GUARDDADO NUEVO
//            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
//            {

//                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_DET.FirstOrDefault(x => x.IdAnalisisDetalle == guardarModificar.IdAnalisisDetalle && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
//                if (model != null)
//                {
//                    model.IdTurno = guardarModificar.IdTurno;
//                    model.ObservacionDet = guardarModificar.ObservacionDet;
//                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
//                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
//                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
//                    valor = 1;//ACTUALIZAR                   
//                }
//                else
//                {
//                    db.CC_ANALISIS_QUIMICO_PRECOCCION_DET.Add(guardarModificar);
//                }
//                db.SaveChanges();
//                return valor;
//            }
//        }
//        public int EliminarDetalle(CC_ANALISIS_QUIMICO_PRECOCCION_DET guardarmodificar)
//        {
//            int valor = 0;
//            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
//            {
//                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_DET.Where(x => x.IdAnalisisDetalle == guardarmodificar.IdAnalisisDetalle && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
//                if (model != null)
//                {
//                    foreach (var item in model)
//                    {
//                        item.EstadoRegistro = guardarmodificar.EstadoRegistro;
//                        item.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
//                        item.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
//                        item.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
//                        valor = 1;
//                    }
//                    db.SaveChanges();
//                }
//                return valor;
//            }
//        }
//    }
//}