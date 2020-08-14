using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ControlCocinamiento
{
    public class ClsDControlCocinamiento
    {
        public int GuardarModificarCocinamiento(CC_COCINAMIENTO_CTRL guardarModificar, int siAprobar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var fechaRepetida = db.CC_COCINAMIENTO_CTRL.FirstOrDefault(x => x.FechaProduccion == guardarModificar.FechaProduccion && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (fechaRepetida != null && guardarModificar.IdCocinamientoCtrl != fechaRepetida.IdCocinamientoCtrl)
                {
                    valor = 5;
                    return valor;
                }
                var model = db.CC_COCINAMIENTO_CTRL.FirstOrDefault(x => x.IdCocinamientoCtrl == guardarModificar.IdCocinamientoCtrl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
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
                        if (guardarModificar.FechaProduccion != DateTime.MinValue)
                        {
                            if (!string.IsNullOrEmpty(guardarModificar.ObservacionC))
                                model.ObservacionC = guardarModificar.ObservacionC.ToUpper();
                            else
                                model.ObservacionC = guardarModificar.ObservacionC;
                            model.FechaProduccion = guardarModificar.FechaProduccion;
                            model.FechaAsignada = guardarModificar.FechaAsignada;
                            model.PCC = guardarModificar.PCC;
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
                    if (guardarModificar.FechaProduccion != DateTime.MinValue)
                    {
                        if (!string.IsNullOrEmpty(guardarModificar.ObservacionC))
                            guardarModificar.ObservacionC = guardarModificar.ObservacionC.ToUpper();
                        db.CC_COCINAMIENTO_CTRL.Add(guardarModificar);
                    }
                    else valor = 3;
                }
                db.SaveChanges();
                return valor;
            }
        }
        public dynamic ConsultarEstadoReporte(int idCocinamientoCtrl, DateTime fechaControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (idCocinamientoCtrl == 0 && fechaControl > DateTime.MinValue)
                {
                    var listado = (from x in db.CC_COCINAMIENTO_CTRL.AsNoTracking()
                                   where x.FechaProduccion.Year == fechaControl.Year && x.FechaProduccion.Month == fechaControl.Month && x.FechaProduccion.Day == fechaControl.Day
                                   && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                   select new
                                   {
                                       x.AprobadoPor,
                                       x.EstadoRegistro,
                                       x.EstadoReporte,
                                       x.FechaAprobado,
                                       x.FechaAsignada,
                                       x.FechaIngresoLog,
                                       x.FechaModificacionLog,
                                       x.FechaProduccion,
                                       x.IdCocinamientoCtrl,
                                       x.ObservacionC,
                                       x.TerminalIngresoLog,
                                       x.TerminalModificacionLog,
                                       x.UsuarioIngresoLog,
                                       x.UsuarioModificacionLog
                                   }).FirstOrDefault();
                    return listado;
                }
                else
                {
                    var listado = (from x in db.CC_COCINAMIENTO_CTRL.AsNoTracking()
                                   where x.IdCocinamientoCtrl == idCocinamientoCtrl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                   select new
                                   {
                                       x.AprobadoPor,
                                       x.EstadoRegistro,
                                       x.EstadoReporte,
                                       x.FechaAprobado,
                                       x.FechaAsignada,
                                       x.FechaIngresoLog,
                                       x.FechaModificacionLog,
                                       x.FechaProduccion,
                                       x.IdCocinamientoCtrl,
                                       x.ObservacionC,
                                       x.TerminalIngresoLog,
                                       x.TerminalModificacionLog,
                                       x.UsuarioIngresoLog,
                                       x.UsuarioModificacionLog
                                   }).FirstOrDefault();
                    return listado;
                }
            }
        }
        public dynamic ConsultarCabecera(DateTime fechaProduccion)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = (from x in db.CC_COCINAMIENTO_CTRL.AsNoTracking()
                               where x.FechaProduccion == fechaProduccion && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                               select new
                               {
                                   x.AprobadoPor,
                                   x.EstadoRegistro,
                                   x.EstadoReporte,
                                   x.FechaAprobado,
                                   x.FechaAsignada,
                                   x.FechaIngresoLog,
                                   x.FechaModificacionLog,
                                   x.FechaProduccion,
                                   x.IdCocinamientoCtrl,
                                   x.ObservacionC,
                                   x.TerminalIngresoLog,
                                   x.TerminalModificacionLog,
                                   x.UsuarioIngresoLog,
                                   x.UsuarioModificacionLog,
                                   x.PCC
                               }).FirstOrDefault();
                return listado;
            }
        }
        public int EliminarCocinamiento(CC_COCINAMIENTO_CTRL guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_COCINAMIENTO_CTRL.FirstOrDefault(x => x.IdCocinamientoCtrl == guardarModificar.IdCocinamientoCtrl);
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
        public CC_COCINAMIENTO_DET ConsultarDetalleExiste(int idCocinamientoDet)
        {
            CC_COCINAMIENTO_DET detalle;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                detalle = db.CC_COCINAMIENTO_DET.Where(x => x.IdCocinamientoDet == idCocinamientoDet && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
            }
            return detalle;
        }
        public int GuardarModificarDetalle(CC_COCINAMIENTO_DET guardarModificar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_COCINAMIENTO_DET.FirstOrDefault(x => x.IdCocinamientoDet == guardarModificar.IdCocinamientoDet && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
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
                    db.CC_COCINAMIENTO_DET.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int GuardarModificarSubDetalle(CC_COCINAMIENTO_SUBDET guardarModificar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                var model = db.CC_COCINAMIENTO_SUBDET.FirstOrDefault(x => x.IdCocinamientoSubDet == guardarModificar.IdCocinamientoSubDet && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.Temperatura = guardarModificar.Temperatura;
                    model.NumCoche = guardarModificar.NumCoche;
                    model.TomaMuestra = guardarModificar.TomaMuestra;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;//ACTUALIZAR                   
                }
                else
                {
                    db.CC_COCINAMIENTO_SUBDET.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int EliminarSubDetalle(CC_COCINAMIENTO_SUBDET guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_COCINAMIENTO_SUBDET.Where(x => x.IdCocinamientoSubDet == guardarmodificar.IdCocinamientoSubDet && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
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
        public List<sp_Control_Cocinamiento> ConsultarDetalleDia(DateTime fechaControl, int op)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var elemento = db.sp_Control_Cocinamiento(op, fechaControl).ToList();
                return elemento;
            }
        }
        public List<sp_Control_Cocinamiento_Sin_Imagenes> ConsultarDetDiaSinImagenes(DateTime fechaControl, int op)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var elemento = db.sp_Control_Cocinamiento_Sin_Imagenes(op, fechaControl).ToList();
                return elemento;
            }
        }
        public List<dynamic> ConsultarSubDetalle(int idCocinamientoCtrl, int idCocinamientoDet)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var subDetalle = (from d in db.CC_COCINAMIENTO_DET
                                  join sd in db.CC_COCINAMIENTO_SUBDET on new { d.IdCocinamientoDet, idCocinamientoCtrl = d.IdCocinamientoCtrl } equals new { sd.IdCocinamientoDet, idCocinamientoCtrl }
                                  where sd.IdCocinamientoDet == idCocinamientoDet && d.EstadoRegistro == clsAtributos.EstadoRegistroActivo && sd.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                  select new
                                  {
                                      sd.IdCocinamientoSubDet,
                                      sd.IdCocinamientoDet,
                                      sd.NumCoche,
                                      sd.Temperatura,
                                      sd.TomaMuestra
                                  });
                return subDetalle.ToList<dynamic>();
            }
        }
        public dynamic ConsultarDetalle(int idCocinamientoCtrl, string lote, int ordenFabricacion)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var subDetalle = (from d in db.CC_COCINAMIENTO_DET.AsNoTracking() where d.OrdenFabricacion == ordenFabricacion && d.Lote == lote && d.IdCocinamientoCtrl == idCocinamientoCtrl && d.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                  select new { d.IdCocinamientoDet, d.Lote, d.ObservacionDet, d.OrdenFabricacion, d.TerminalIngresoLog, d.TerminalModificacionLog,
                                      d.UsuarioIngresoLog, d.UsuarioModificacionLog, d.EstadoRegistro, d.FechaIngresoLog, d.FechaModificacionLog, d.IdCocinamientoCtrl }).FirstOrDefault();
                return subDetalle;
            }
        }
        public int GuardarModificarFoto(CC_COCINAMIENTO_IMAGEN guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_COCINAMIENTO_IMAGEN.FirstOrDefault(x => x.IdImagen == guardarModificar.IdImagen && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.ObservacionImagen = guardarModificar.ObservacionImagen;
                    if (!string.IsNullOrEmpty(guardarModificar.RutaImagen))
                        model.RutaImagen = guardarModificar.RutaImagen;
                    model.Rotation = guardarModificar.Rotation;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_COCINAMIENTO_IMAGEN.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public List<CC_COCINAMIENTO_IMAGEN> ConsultarImagen(int idCocinamientoDet)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listaImagen = db.CC_COCINAMIENTO_IMAGEN.Where(x => x.IdCocinamientoDet == idCocinamientoDet && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                return listaImagen;
            }
        }
        public int EliminarImagen(CC_COCINAMIENTO_IMAGEN guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_COCINAMIENTO_IMAGEN.Where(x => x.IdImagen == guardarmodificar.IdImagen && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
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
        public List<CC_COCINAMIENTO_CTRL> ConsultarBadejaEstado(DateTime fechaDesde, DateTime FechaHasta, bool estadoReporte)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<CC_COCINAMIENTO_CTRL> listado;
                if (estadoReporte)
                {
                    listado = db.CC_COCINAMIENTO_CTRL.AsNoTracking().Where(x => x.EstadoReporte == estadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                              && x.FechaProduccion >= fechaDesde && x.FechaProduccion <= FechaHasta).OrderByDescending(v => v.FechaProduccion).ToList();
                }
                else
                {
                    listado = db.CC_COCINAMIENTO_CTRL.AsNoTracking().Where(x => x.EstadoReporte == estadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                                ).OrderByDescending(v => v.FechaProduccion).ToList();
                }
                CC_COCINAMIENTO_CTRL cabecera;
                List<CC_COCINAMIENTO_CTRL> listaCabecera = new List<CC_COCINAMIENTO_CTRL>();
                if (listado.Any())
                {
                    foreach (var item in listado)
                    {
                        cabecera = new CC_COCINAMIENTO_CTRL();
                        cabecera.IdCocinamientoCtrl = item.IdCocinamientoCtrl;
                        cabecera.FechaProduccion = item.FechaProduccion;
                        cabecera.ObservacionC = item.ObservacionC;
                        cabecera.EstadoReporte = item.EstadoReporte;
                        cabecera.FechaIngresoLog = item.FechaIngresoLog;
                        cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                        cabecera.FechaAprobado = item.FechaAprobado;
                        cabecera.AprobadoPor = item.AprobadoPor;
                        cabecera.FechaAsignada = item.FechaAsignada;
                        cabecera.PCC = item.PCC;
                        listaCabecera.Add(cabecera);
                    }
                }
                return listaCabecera;
            }
        }
        public List<sp_Analisis_Quimico_Precoccion_Barco> ConsultarBarcoFecha(DateTime fechaAsignada, int op = 1)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var elemento = db.sp_Analisis_Quimico_Precoccion_Barco(op, fechaAsignada).ToList();
                return elemento;
            }
        }
    }
    public class Temperatura
    {
        public int IdCocinamientoCtrl { get; set; }
        public DateTime FechaProduccion { get; set; }
        public DateTime FechaAsignada { get; set; }
        public int IdCocinamientoDet { get; set; }
        public string Lote { get; set; }
        public int OrdenFabricacion { get; set; }
        public int Parada { get; set; }
        public string Talla { get; set; }
        public string Barco { get; set; }
        public string BarcoAsignado { get; set; }
        public int Coche { get; set; }
        public int Cocina { get; set; }
        public string Especie { get; set; }
        public List<ListaTemperatura> listaTemperatura { get; set; }
        public List<ListaImagenes> listaImagenes { get; set; }
    }
    public class ListaTemperatura
    {
        public int? IdCocinamientoSubDet { get; set; }
        public int? NumCoche { get; set; }
        public int? Temperatura { get; set; }
        public int? TomaMuestra { get; set; }       
    }
    public class ListaImagenes {
        public int? IdImagen { get; set; }
        public string ObservacionImagen { get; set; }
        public string RutaImagen { get; set; }
        public int? Rotation { get; set; }
    }
}