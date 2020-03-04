using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.MapeoProductoTunel
{
    public class clsDMapeoProductoTunel
    {
        clsDApiProduccion clsDApiProduccion = null;
        public List<spConsultaMapeoProductoTunel> ConsultaMapeoProductoTunel(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaMapeoProductoTunel(Fecha).ToList();
                return lista;
            }
        }
        public RespuestaGeneral GuardarModificarControl(MAPEO_PRODUCTO_TUNEL control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.MAPEO_PRODUCTO_TUNEL.FirstOrDefault(x => x.IdMapeoProductoTunel == control.IdMapeoProductoTunel);
                if (result != null)
                {
                    //result.FechaVencimiento = control.FechaVencimiento;
                    //result.CodigoProducto = control.CodigoProducto;
                    result.Fin = control.Fin;
                    result.Lote = control.Lote;
                    result.TipoLimpieza = control.TipoLimpieza;
                    result.Observacion = control.Observacion;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    control.FechaIngresoLog = DateTime.Now;
                    entities.MAPEO_PRODUCTO_TUNEL.Add(control);
                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }

        public RespuestaGeneral EliminarProductoTerminado(MAPEO_PRODUCTO_TUNEL control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.MAPEO_PRODUCTO_TUNEL.FirstOrDefault(x => x.IdMapeoProductoTunel == control.IdMapeoProductoTunel);
                if (result != null)
                {
                    result.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                    entities.SaveChanges();
                }
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }



        public List<spConsultaMapeoProductoTunelDetalle> ConsultaMapeoProductoTunelDetalle(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaMapeoProductoTunelDetalle(IdControl).ToList();
                return lista;
            }
        }
        public RespuestaGeneral GuardarModificarControlDetalle(MAPEO_PRODUCTO_TUNEL_DETALLE control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.MAPEO_PRODUCTO_TUNEL_DETALLE.FirstOrDefault(x => x.IdMapeoProductoTunelDetalle == control.IdMapeoProductoTunelDetalle);
                if (result != null)
                {
                    //result.FechaVencimiento = control.FechaVencimiento;
                    //result.CodigoProducto = control.CodigoProducto;
                    result.Textura = control.Textura;
                    result.Tunel = control.Tunel;
                    result.Coche = control.Coche;
                    result.Producto = control.Producto;
                    result.Especie = control.Especie;
                    result.Fundas = control.Fundas;
                    result.HoraInicio = control.HoraInicio;
                    result.HoraFin = control.HoraFin;
                    result.HoraFinLote = control.HoraFinLote;
                    result.TotalFunda = control.TotalFunda;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    control.FechaIngresoLog = DateTime.Now;
                    entities.MAPEO_PRODUCTO_TUNEL_DETALLE.Add(control);
                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }

        public RespuestaGeneral EliminarProductoTerminadoDetalle(MAPEO_PRODUCTO_TUNEL_DETALLE control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.MAPEO_PRODUCTO_TUNEL_DETALLE.FirstOrDefault(x => x.IdMapeoProductoTunelDetalle == control.IdMapeoProductoTunelDetalle);
                if (result != null)
                {
                    result.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                    entities.SaveChanges();
                }
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }

        public List<spReporteMapeoProductoTunelDetalle> ConsultaReporteMapeoProductoTunel(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                clsDApiProduccion = new clsDApiProduccion();
                var lista = entities.spReporteMapeoProductoTunelDetalle(Fecha).ToList();
                var texturas = clsDApiProduccion.ConsultarObservaciones();
                foreach(var x in lista)
                {
                    var poTextura = texturas.FirstOrDefault(y => y.Codigo == x.CodTextura);
                    x.Textura = poTextura != null ? poTextura.Descripcion:"";
                }

                return lista;
            }
        }
    }
}