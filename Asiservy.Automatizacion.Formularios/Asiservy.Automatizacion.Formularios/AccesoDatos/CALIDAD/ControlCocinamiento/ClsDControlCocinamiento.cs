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
        public CC_COCINAMIENTO_CTRL ConsultarEstadoReporte(int idAnalisis, DateTime fechaControl)
        {
            CC_COCINAMIENTO_CTRL listado;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (idAnalisis == 0 && fechaControl > DateTime.MinValue)
                {
                    listado = db.CC_COCINAMIENTO_CTRL.FirstOrDefault(x => x.FechaProduccion.Year == fechaControl.Year && x.FechaProduccion.Month == fechaControl.Month
                                                                                        && x.FechaProduccion.Day == fechaControl.Day
                                                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else
                {
                    listado = db.CC_COCINAMIENTO_CTRL.FirstOrDefault(x => x.IdCocinamientoCtrl == idAnalisis && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                CC_COCINAMIENTO_CTRL cabecera;
                if (listado != null)
                {
                    cabecera = new CC_COCINAMIENTO_CTRL();
                    cabecera.IdCocinamientoCtrl = listado.IdCocinamientoCtrl;
                    cabecera.FechaProduccion = listado.FechaProduccion;
                    cabecera.ObservacionC = listado.ObservacionC;
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
    }
}