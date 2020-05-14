using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.TemperaturaTermoencogidoSellado
{
    public class clsDTemperaturaTermoencogidoSellado
    {
        public List<sp_Control_Termoencogido_Sellado> ConsultarTermoencogidoSellado(DateTime fechaDesde, DateTime fechaHasta, int op) {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                var lista = db.sp_Control_Termoencogido_Sellado(fechaDesde, fechaHasta, op).ToList();
                return lista;
            }
        }

        public int GuardarModificarTermoencogidoSellado(CC_TEMPERATURA_TERMOENCOGIDO_SELLADO GuardarModigicar, bool siAprobar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
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
                        model.Observacion = GuardarModigicar.Observacion;                       
                        valor = 1;
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

        public bool ConsultarEstadoReporte(int id)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO.FirstOrDefault(x => x.Id == id && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (listado.EstadoReporte)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
    }
}