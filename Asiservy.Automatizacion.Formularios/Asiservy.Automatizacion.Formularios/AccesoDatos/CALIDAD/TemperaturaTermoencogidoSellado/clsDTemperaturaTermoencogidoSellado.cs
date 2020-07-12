using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.TemperaturaTermoencogidoSellado
{
    public class clsDTemperaturaTermoencogidoSellado
    {
        //public List<sp_Control_Termoencogido_Sellado> ConsultarTermoencogidoSellado(DateTime fechaDesde, DateTime fechaHasta, int op) {
        //    using (ASIS_PRODEntities db=new ASIS_PRODEntities())
        //    {
        //        var lista = db.sp_Control_Termoencogido_Sellado(fechaDesde, fechaHasta, op).ToList();
        //        return lista;
        //    }
        //}
        public List<CC_TEMPERATURA_TERMOENCOGIDO_SELLADO> ConsultarBadejaEstado(DateTime fechaDesde, DateTime FechaHasta, bool estadoReporte)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<CC_TEMPERATURA_TERMOENCOGIDO_SELLADO> listado;
                if (estadoReporte)
                {
                    listado = db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO.Where(x => x.EstadoReporte == estadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                              && x.Fecha >= fechaDesde && x.Fecha <= FechaHasta).OrderByDescending(v => v.Fecha).ToList();
                }
                else
                {
                    listado = db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO.Where(x => x.EstadoReporte == estadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                               && x.Fecha <= FechaHasta).OrderByDescending(v => v.Fecha).ToList();
                }
                CC_TEMPERATURA_TERMOENCOGIDO_SELLADO cabecera;
                List<CC_TEMPERATURA_TERMOENCOGIDO_SELLADO> listaCabecera = new List<CC_TEMPERATURA_TERMOENCOGIDO_SELLADO>();
                if (listado.Any())
                {
                    foreach (var item in listado)
                    {
                        cabecera = new CC_TEMPERATURA_TERMOENCOGIDO_SELLADO();
                        cabecera.Id = item.Id;
                        cabecera.Fecha = item.Fecha;
                        cabecera.Observacion = item.Observacion;
                        cabecera.EstadoReporte = item.EstadoReporte;
                        cabecera.FechaIngresoLog = item.FechaIngresoLog;
                        cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                        cabecera.FechaAprobado = item.FechaAprobado;
                        cabecera.AprobadoPor = item.AprobadoPor;
                        cabecera.Turno = item.Turno;
                        listaCabecera.Add(cabecera);
                    }
                }
                return listaCabecera;
            }
        }
        public int GuardarModificarTermoencogidoSellado(CC_TEMPERATURA_TERMOENCOGIDO_SELLADO GuardarModigicar, bool siAprobar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO.FirstOrDefault(x => x.Fecha == GuardarModigicar.Fecha && x.Turno == GuardarModigicar.Turno && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (!siAprobar && validarNombreRepetido != null && GuardarModigicar.Id != validarNombreRepetido.Id)
                {
                    valor = 5;
                    return valor;
                }

                var model = db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO.FirstOrDefault(x => x.Id == GuardarModigicar.Id && x.EstadoRegistro == GuardarModigicar.EstadoRegistro);
                if (model != null)
                {
                    if (siAprobar)
                    {
                        model.AprobadoPor = GuardarModigicar.UsuarioIngresoLog;
                        model.FechaAprobado = GuardarModigicar.FechaAprobado;
                        model.EstadoReporte = GuardarModigicar.EstadoReporte;
                        valor = 2;
                    }
                    else
                    {
                        if (GuardarModigicar.Fecha != DateTime.MinValue)
                        {
                            model.Turno = GuardarModigicar.Turno;
                            model.Fecha = GuardarModigicar.Fecha;
                            model.Observacion = GuardarModigicar.Observacion;
                            valor = 1;//ACTUALIZAR
                        }
                        else valor = 3;//ERROR DE FECHA
                    }
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
                }
                else
                {
                    db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO.Add(GuardarModigicar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarTermoencogidoSellado(CC_TEMPERATURA_TERMOENCOGIDO_SELLADO GuardarModigicar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO.FirstOrDefault(x => x.Id == GuardarModigicar.Id);
                if (model != null)
                {
                    model.EstadoRegistro = GuardarModigicar.EstadoRegistro;
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }

        //DETALLE
        public List<sp_Control_Termoencogido_Sellado_Detalle> ConsultarTermoencogidoSelladoDetalle(DateTime fechaDesde, DateTime fechaHasta,int idCabecera, int op)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.sp_Control_Termoencogido_Sellado_Detalle(fechaDesde, fechaHasta,idCabecera, op).ToList();
                return lista;
            }
        }

        public int GuardarModificarTermoencogidoSelladoDetalle(CC_TEMPERATURA_TERMOENCOGIDO_SELLADO_DETALLE GuardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                var model = db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO_DETALLE.FirstOrDefault(c=> c.Id==GuardarModificar.Id && c.EstadoRegistro==GuardarModificar.EstadoRegistro);
                if (model!=null)
                {
                    model.HoraVerificacion = GuardarModificar.HoraVerificacion;
                    model.Temperatura = GuardarModificar.Temperatura;
                    model.CorrectoSellado = GuardarModificar.CorrectoSellado;
                    model.Observacion = GuardarModificar.Observacion;
                    model.FechaModificacionLog = GuardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO_DETALLE.Add(GuardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarTermoencogidoSelladoDetalle(CC_TEMPERATURA_TERMOENCOGIDO_SELLADO_DETALLE GuardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO_DETALLE.FirstOrDefault(x => x.Id == GuardarModificar.Id);
                if (model != null)
                {
                    model.EstadoRegistro = GuardarModificar.EstadoRegistro;
                    model.FechaModificacionLog = GuardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModificar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }

        public CC_TEMPERATURA_TERMOENCOGIDO_SELLADO ConsultarEstadoReporte(int id)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO.FirstOrDefault(x => x.Id == id && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                return listado;

            }
        }

        public CC_TEMPERATURA_TERMOENCOGIDO_SELLADO ConsultarCabeceraTurno(DateTime fechaControl, string turno)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                CC_TEMPERATURA_TERMOENCOGIDO_SELLADO listado;

                if (turno == "0")
                {
                    listado = db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                   && x.Fecha.Day == fechaControl.Day
                                                                                   && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else
                {
                    listado = db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                        && x.Fecha.Day == fechaControl.Day && x.Turno == turno
                                                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                CC_TEMPERATURA_TERMOENCOGIDO_SELLADO cabecera;
                if (listado != null)
                {
                    cabecera = new CC_TEMPERATURA_TERMOENCOGIDO_SELLADO();
                    cabecera.Id = listado.Id;
                    cabecera.Fecha = listado.Fecha;
                    cabecera.Observacion = listado.Observacion;
                    cabecera.EstadoReporte = listado.EstadoReporte;
                    cabecera.FechaIngresoLog = listado.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = listado.UsuarioIngresoLog;
                    cabecera.FechaAprobado = listado.FechaAprobado;
                    cabecera.AprobadoPor = listado.AprobadoPor;
                    cabecera.Turno = listado.Turno;
                    return cabecera;
                }
                return listado;

            }
        }

        public List<CC_TEMPERATURA_TERMOENCOGIDO_SELLADO> ReporteConsultarcabecera(DateTime fechaDesde, DateTime fechaHasta)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = (from c in db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO
                             where (c.Fecha >= fechaDesde && c.Fecha <= fechaHasta && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                             orderby c.Fecha descending
                             select new { c.Id, c.Fecha, c.EstadoReporte, c.Observacion, c.FechaIngresoLog, c.UsuarioIngresoLog, c.FechaAprobado, c.AprobadoPor, c.Turno }).ToList();
                List<CC_TEMPERATURA_TERMOENCOGIDO_SELLADO> listacabecera = new List<CC_TEMPERATURA_TERMOENCOGIDO_SELLADO>();
                CC_TEMPERATURA_TERMOENCOGIDO_SELLADO cabecera;
                foreach (var item in lista)
                {
                    cabecera = new CC_TEMPERATURA_TERMOENCOGIDO_SELLADO();
                    cabecera.Id = item.Id;
                    cabecera.Fecha = item.Fecha;
                    cabecera.EstadoReporte = item.EstadoReporte;
                    cabecera.Observacion = item.Observacion;
                    cabecera.FechaIngresoLog = item.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    cabecera.FechaAprobado = item.FechaAprobado;
                    cabecera.AprobadoPor = item.AprobadoPor;
                    cabecera.Turno = item.Turno;
                    listacabecera.Add(cabecera);
                }
                return listacabecera;
            }
        }
    }
}