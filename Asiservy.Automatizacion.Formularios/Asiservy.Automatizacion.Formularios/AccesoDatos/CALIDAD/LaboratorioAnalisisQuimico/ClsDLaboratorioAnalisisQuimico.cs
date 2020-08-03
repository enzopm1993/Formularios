using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.LaboratorioAnalisisQuimico
{
    public class ClsDLaboratorioAnalisisQuimico
    {
        public int GuardarModificarAnalisisQuimico(CC_ANALISIS_QUIMICO_PRECOCCION_CTRL guardarModificar, int siAprobar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                //var validarNombreRepetido = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.FirstOrDefault(x => x.Fecha == guardarModificar.Fecha && x.Turno == guardarModificar.Turno && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                //if (siAprobar != 1 && validarNombreRepetido != null && guardarModificar.IdAnalisis != validarNombreRepetido.IdAnalisis)
                //{
                //    valor = 5;
                //    return valor;
                //}
                var fechaRepetida = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.FirstOrDefault(x => x.Fecha == guardarModificar.Fecha && x.Turno == guardarModificar.Turno && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (fechaRepetida != null && guardarModificar.IdAnalisis != fechaRepetida.IdAnalisis)
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
                            if (!string.IsNullOrEmpty(guardarModificar.ObservacionCtrl))
                                model.ObservacionCtrl = guardarModificar.ObservacionCtrl.ToUpper();
                            else
                                model.ObservacionCtrl = guardarModificar.ObservacionCtrl;
                            model.Fecha = guardarModificar.Fecha;
                            model.Turno = guardarModificar.Turno;
                            //model.FechaAsignada = guardarModificar.FechaAsignada;
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
                        if (!string.IsNullOrEmpty(guardarModificar.ObservacionCtrl))
                            guardarModificar.ObservacionCtrl = guardarModificar.ObservacionCtrl.ToUpper();
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
            CC_ANALISIS_QUIMICO_PRECOCCION_CTRL listado;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                if (idAnalisis == 0 && fechaControl > DateTime.MinValue)
                {
                    listado = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                        && x.Fecha.Day == fechaControl.Day
                                                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else
                {
                    listado = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.Include(c => c.CC_ANALISIS_QUIMICO_PRECOCCION_DET).FirstOrDefault(x => x.IdAnalisis == idAnalisis && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
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

            }
            return listado;
        }
        public CC_ANALISIS_QUIMICO_PRECOCCION_DET ConsultarDetalleExiste(int idAnalisisDetalle)
        {
            CC_ANALISIS_QUIMICO_PRECOCCION_DET detalle;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                detalle = db.CC_ANALISIS_QUIMICO_PRECOCCION_DET.Where(x => x.IdAnalisisDetalle == idAnalisisDetalle && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
            }
            return detalle;
        }
        public List<CC_ANALISIS_QUIMICO_PRECOCCION_DET> ConsultarDetalle(int IdAnalisis)
        {
            List<CC_ANALISIS_QUIMICO_PRECOCCION_DET> detalle;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                detalle = db.CC_ANALISIS_QUIMICO_PRECOCCION_DET.Where(x => x.IdAnalisis == IdAnalisis && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
            if (detalle != null)
            {
                List<CC_ANALISIS_QUIMICO_PRECOCCION_DET> listaDetalle = new List<CC_ANALISIS_QUIMICO_PRECOCCION_DET>();
                CC_ANALISIS_QUIMICO_PRECOCCION_DET objDetalle;
                foreach (var item in detalle)
                {
                    objDetalle = new CC_ANALISIS_QUIMICO_PRECOCCION_DET();
                    objDetalle.IdAnalisisDetalle = item.IdAnalisisDetalle;
                    objDetalle.ObservacionDet = item.ObservacionDet;
                    objDetalle.IdAnalisis = item.IdAnalisis;
                    objDetalle.Cocinador = item.Cocinador;
                    objDetalle.Parada = item.Parada;
                    listaDetalle.Add(objDetalle);
                }
                return listaDetalle;
            }
            return null;
        }
        public List<dynamic> ConsultarElemento(int idAnalisis, int idParametro, int idAnalisisDetalle)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var elemento = (from det in db.CC_ANALISIS_QUIMICO_PRECOCCION_DET
                                join ele in db.CC_ANALISIS_QUIMICO_PRECOCCION_ELEMENTOS on new { det.IdAnalisisDetalle, det.IdAnalisis, d = det.IdAnalisisDetalle } equals new { ele.IdAnalisisDetalle, IdAnalisis = idAnalisis, d = idAnalisisDetalle }
                                join par in db.CC_PARAMETROS_LABORATORIO on new { ele.IdParametro, p = idParametro, ele.EstadoRegistro } equals new { par.IdParametro, p = par.IdParametro, EstadoRegistro = clsAtributos.EstadoRegistroActivo }
                                select new
                                {
                                    det.Cocinador,
                                    det.IdAnalisisDetalle,
                                    det.ObservacionDet,
                                    det.Parada,
                                    ele.IdElemento,
                                    ele.IdParametro,
                                    ele.LoteBarco,
                                    ele.Valor,
                                    par.Mascara,
                                    par.NombreParametro,
                                    par.ValorMax,
                                    par.ValorMin
                                });
                return elemento.ToList<dynamic>();
            }
        }
        public List<sp_Analisis_Quimico_Precoccion> ConsultarDetalleDia(DateTime fechaControl, string turno, int op)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var elemento = db.sp_Analisis_Quimico_Precoccion(op, fechaControl, turno).ToList();               
                return elemento;
            }
        }
        public CC_ANALISIS_QUIMICO_PRECOCCION_CTRL ConsultarCabeceraTurno(string turno, DateTime fechaControl)
        {
            CC_ANALISIS_QUIMICO_PRECOCCION_CTRL listado;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (turno == null)
                {
                    listado = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.AsNoTracking().FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                    && x.Fecha.Day == fechaControl.Day
                                                                                    && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else
                {
                    listado = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.AsNoTracking().FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                        && x.Fecha.Day == fechaControl.Day && x.Turno == turno
                                                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }

            }
            if (listado != null)
            {
                //listado.CC_ANALISIS_QUIMICO_PRECOCCION_DET = null;
                CC_ANALISIS_QUIMICO_PRECOCCION_CTRL cabecera = new CC_ANALISIS_QUIMICO_PRECOCCION_CTRL();
                cabecera.AprobadoPor = listado.AprobadoPor;
                cabecera.EstadoReporte = listado.EstadoReporte;
                cabecera.Fecha = listado.Fecha;
                cabecera.FechaAprobado = listado.FechaAprobado;
                cabecera.IdAnalisis = listado.IdAnalisis;
                cabecera.ObservacionCtrl = listado.ObservacionCtrl;
                cabecera.Turno = listado.Turno;
                cabecera.FechaAsignada = listado.FechaAsignada;
                return cabecera;
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
                    if (!string.IsNullOrEmpty(guardarModificar.ObservacionDet))
                        model.ObservacionDet = guardarModificar.ObservacionDet.ToUpper();
                    else
                        model.ObservacionDet = guardarModificar.ObservacionDet;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;//ACTUALIZAR                   
                }
                else
                {
                    if (!string.IsNullOrEmpty(guardarModificar.ObservacionDet))
                        guardarModificar.ObservacionDet = guardarModificar.ObservacionDet.ToUpper();
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
        public int GuardarModificarElemento(CC_ANALISIS_QUIMICO_PRECOCCION_ELEMENTOS guardarModificar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_ELEMENTOS.FirstOrDefault(x => x.IdElemento == guardarModificar.IdElemento && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.Valor = guardarModificar.Valor;
                    model.LoteBarco = guardarModificar.LoteBarco;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;//ACTUALIZAR                   
                }
                else
                {
                    db.CC_ANALISIS_QUIMICO_PRECOCCION_ELEMENTOS.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int EliminarElemento(CC_ANALISIS_QUIMICO_PRECOCCION_ELEMENTOS guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_ELEMENTOS.Where(x => x.IdElemento == guardarmodificar.IdElemento && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
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
        public int GuardarModificarFoto(CC_ANALISIS_QUIMICO_PRECOCCION_FOTO guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_FOTO.FirstOrDefault(x => x.IdFoto == guardarModificar.IdFoto && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.ObservacionFoto = guardarModificar.ObservacionFoto;
                    if (!string.IsNullOrEmpty(guardarModificar.RutaFoto))
                        model.RutaFoto = guardarModificar.RutaFoto;
                    model.Rotation = guardarModificar.Rotation;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_ANALISIS_QUIMICO_PRECOCCION_FOTO.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public List<CC_ANALISIS_QUIMICO_PRECOCCION_FOTO> ConsultarImagen(int idAnalisisDetalle)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listaImagen = db.CC_ANALISIS_QUIMICO_PRECOCCION_FOTO.Where(x=> x.IdAnalisisDetalle==idAnalisisDetalle && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();
                return listaImagen;
            }
        }
        public int EliminarImagen(CC_ANALISIS_QUIMICO_PRECOCCION_FOTO guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_FOTO.Where(x => x.IdFoto == guardarmodificar.IdFoto && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
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
        public List<CC_ANALISIS_QUIMICO_PRECOCCION_CTRL> ConsultarBadejaEstado(DateTime fechaDesde, DateTime FechaHasta, bool estadoReporte)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<CC_ANALISIS_QUIMICO_PRECOCCION_CTRL> listado;
                if (estadoReporte)
                {
                    listado = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.Where(x => x.EstadoReporte == estadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                              && x.Fecha >= fechaDesde && x.Fecha <= FechaHasta).OrderByDescending(v => v.Fecha).ToList();            
                }
                else
                {
                   listado = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.Where(x => x.EstadoReporte == estadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                               ).OrderByDescending(v => v.Fecha).ToList();                    
                }
                CC_ANALISIS_QUIMICO_PRECOCCION_CTRL cabecera;
                List<CC_ANALISIS_QUIMICO_PRECOCCION_CTRL> listaCabecera = new List<CC_ANALISIS_QUIMICO_PRECOCCION_CTRL>();
                if (listado.Any())
                {
                    foreach (var item in listado)
                    {
                        cabecera = new CC_ANALISIS_QUIMICO_PRECOCCION_CTRL();
                        cabecera.IdAnalisis = item.IdAnalisis;
                        cabecera.Fecha = item.Fecha;
                        cabecera.ObservacionCtrl = item.ObservacionCtrl;
                        cabecera.EstadoReporte = item.EstadoReporte;
                        cabecera.FechaIngresoLog = item.FechaIngresoLog;
                        cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                        cabecera.FechaAprobado = item.FechaAprobado;
                        cabecera.AprobadoPor = item.AprobadoPor;
                        cabecera.Turno = item.Turno;
                        cabecera.FechaAsignada = item.FechaAsignada;
                        listaCabecera.Add(cabecera);
                    }
                }
                return listaCabecera;
            }
        }
        public List<CC_ANALISIS_QUIMICO_PRECOCCION_CTRL> ConsultarReporteRangoFecha(DateTime fechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.CC_ANALISIS_QUIMICO_PRECOCCION_CTRL.AsNoTracking().Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                           && x.Fecha >= fechaDesde && x.Fecha <= FechaHasta).OrderByDescending(c => c.Fecha).ToList();

                CC_ANALISIS_QUIMICO_PRECOCCION_CTRL cabecera;
                List<CC_ANALISIS_QUIMICO_PRECOCCION_CTRL> listaCabecera = new List<CC_ANALISIS_QUIMICO_PRECOCCION_CTRL>();
                if (listado.Any())
                {
                    foreach (var item in listado)
                    {
                        cabecera = new CC_ANALISIS_QUIMICO_PRECOCCION_CTRL();
                        cabecera.IdAnalisis = item.IdAnalisis;
                        cabecera.Fecha = item.Fecha;
                        cabecera.ObservacionCtrl = item.ObservacionCtrl;
                        cabecera.EstadoReporte = item.EstadoReporte;
                        cabecera.FechaIngresoLog = item.FechaIngresoLog;
                        cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                        cabecera.UsuarioModificacionLog = item.UsuarioModificacionLog;
                        cabecera.FechaModificacionLog = item.FechaModificacionLog;
                        cabecera.FechaAprobado = item.FechaAprobado;
                        cabecera.AprobadoPor = item.AprobadoPor;
                        cabecera.Turno = item.Turno;
                        cabecera.FechaAsignada = item.FechaAsignada;
                        listaCabecera.Add(cabecera);
                    }
                }
                return listaCabecera;
            }
        }
    }   
}