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
                               where x.FechaProduccion == fechaProduccion && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo
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

    }
}