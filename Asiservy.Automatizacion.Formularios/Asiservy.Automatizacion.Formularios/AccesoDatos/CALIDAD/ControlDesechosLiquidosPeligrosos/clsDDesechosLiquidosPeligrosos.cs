using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ControlDesechosLiquidosPeligrosos
{
    public class clsDDesechosLiquidosPeligrosos
    {
        public List<sp_Control_Desechos_Liquidos_Peligrosos> ConsultarDesechosLiquidos(DateTime fechaDesde, DateTime fechaHasta, int idDesechosLiquidos, int op)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.sp_Control_Desechos_Liquidos_Peligrosos(fechaDesde, fechaHasta, idDesechosLiquidos, op).ToList();
                return lista;
            }
        }

        public int GuardarModificarDesechosLiquidos(CC_DESECHOS_LIQUIDOS_PELIGROSOS guardarmodificar, int siAprobar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_DESECHOS_LIQUIDOS_PELIGROSOS.FirstOrDefault(x => x.IdDesechosLiquidos == guardarmodificar.IdDesechosLiquidos && x.EstadoRegistro == guardarmodificar.EstadoRegistro);
                if (model != null)
                {
                    if (siAprobar == 0)
                    {
                        model.FechaMES = guardarmodificar.FechaMES;
                    }
                    else if (siAprobar == 1)
                    {
                        model.EstadoReporte = guardarmodificar.EstadoReporte;
                        model.AprobadoPor = guardarmodificar.AprobadoPor;
                    }
                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_DESECHOS_LIQUIDOS_PELIGROSOS.Add(guardarmodificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarDesechosLiquidos(CC_DESECHOS_LIQUIDOS_PELIGROSOS registroEliminar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_DESECHOS_LIQUIDOS_PELIGROSOS.FirstOrDefault(x => x.IdDesechosLiquidos == registroEliminar.IdDesechosLiquidos);
                if (model != null)
                {
                    model.EstadoRegistro = registroEliminar.EstadoRegistro;
                    model.FechaModificacionLog = registroEliminar.FechaIngresoLog;
                    model.TerminalModificacionLog = registroEliminar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = registroEliminar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }

        public int GuardarModificarDesechosLiquidosDetalle(CC_DESECHOS_LIQUIDOS_PELIGROSOS_DETALLE guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_DESECHOS_LIQUIDOS_PELIGROSOS_DETALLE.FirstOrDefault(x => x.IdDesechosLiquidosDetalle == guardarmodificar.IdDesechosLiquidosDetalle && x.EstadoRegistro == guardarmodificar.EstadoRegistro);
                if (model != null)
                {
                    
                    model.FechaDIA = guardarmodificar.FechaDIA;
                    model.Laboratorio = guardarmodificar.Laboratorio;
                    model.Otros = guardarmodificar.Otros;
                    model.Observaciones = guardarmodificar.Observaciones;
                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_DESECHOS_LIQUIDOS_PELIGROSOS_DETALLE.Add(guardarmodificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarDesechosLiquidosDetalle(CC_DESECHOS_LIQUIDOS_PELIGROSOS_DETALLE registroEliminar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_DESECHOS_LIQUIDOS_PELIGROSOS_DETALLE.FirstOrDefault(x => x.IdDesechosLiquidosDetalle == registroEliminar.IdDesechosLiquidosDetalle);
                if (model != null)
                {
                    model.EstadoRegistro = registroEliminar.EstadoRegistro;
                    model.FechaModificacionLog = registroEliminar.FechaIngresoLog;
                    model.TerminalModificacionLog = registroEliminar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = registroEliminar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }

    }
}