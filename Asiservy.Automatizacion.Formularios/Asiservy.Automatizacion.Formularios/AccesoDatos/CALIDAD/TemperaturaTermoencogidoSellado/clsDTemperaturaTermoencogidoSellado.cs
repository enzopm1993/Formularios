﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.TemperaturaTermoencogidoSellado
{
    public class clsDTemperaturaTermoencogidoSellado
    {
        public List<sp_Control_Termoencogido_Sellado> ConsultarTermoencogidoSellado(DateTime fecha, int op) {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                var lista = db.sp_Control_Termoencogido_Sellado(fecha,op).ToList();
                return lista;
            }
        }

        public int GuardarModificarTermoencogidoSellado(CC_TEMPERATURA_TERMOENCOGIDO_SELLADO GuardarModigicar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_TEMPERATURA_TERMOENCOGIDO_SELLADO.FirstOrDefault(x => x.Id == GuardarModigicar.Id && x.EstadoRegistro == GuardarModigicar.EstadoRegistro);
                if (model != null)
                {
                    model.Observacion = GuardarModigicar.Observacion;
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
                    valor = 1;
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
        public List<sp_Control_Termoencogido_Sellado_Detalle> ConsultarTermoencogidoSelladoDetalle(int id,int idCabecera, int op)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.sp_Control_Termoencogido_Sellado_Detalle(id,idCabecera, op).ToList();
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
    }
}