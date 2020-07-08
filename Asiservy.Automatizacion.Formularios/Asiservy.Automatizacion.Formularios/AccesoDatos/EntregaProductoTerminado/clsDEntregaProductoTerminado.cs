using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.EntregaProductoTerminado
{
    public class clsDEntregaProductoTerminado
    {
        #region PRODUCTO TERMINADO
        public List<spConsultaProductoTerminado> ConsultaControlProductoTerminado(DateTime Fecha, string LineaNegocio,string Linea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaProductoTerminado(Fecha, LineaNegocio).Where(x=> x.Linea == Linea).ToList();
                return lista;
            }
        }

        public bool ConsultaControlProductoTerminado(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var Control = entities.PRODUCTO_TERMINADO.FirstOrDefault(x=> x.IdProductoTerminado == IdControl);
                var result = Control.EstadoReporte??false;
                return result;
            }
        }

        public RespuestaGeneral GuardarModificarControl(PRODUCTO_TERMINADO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.PRODUCTO_TERMINADO.FirstOrDefault(x => x.IdProductoTerminado == control.IdProductoTerminado);
                if (result != null)
                {
                    result.FechaVencimiento = control.FechaVencimiento;
                    result.CodigoProducto = control.CodigoProducto.ToUpper();
                    result.Observacion = string.IsNullOrEmpty(control.Observacion)? control.Observacion : control.Observacion.ToUpper();
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.CodigoProducto = control.CodigoProducto.ToUpper();
                    control.Observacion = string.IsNullOrEmpty(control.Observacion) ? control.Observacion : control.Observacion.ToUpper();
                    control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    control.FechaIngresoLog = DateTime.Now;
                    entities.PRODUCTO_TERMINADO.Add(control);
                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }

        public RespuestaGeneral EliminarProductoTerminado(PRODUCTO_TERMINADO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.PRODUCTO_TERMINADO.FirstOrDefault(x => x.IdProductoTerminado == control.IdProductoTerminado);
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
        #endregion

        #region PRODUCTO TERMINADO DETALLE
        public List<spConsultaProductoTerminadoDetalle> ConsultaControlProductoTerminadoDetalle(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaProductoTerminadoDetalle(IdControl).ToList();
                return lista;
            }
        }
        public RespuestaGeneral GuardarModificarDetalle(PRODUCTO_TERMINADO_DETALLE control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.PRODUCTO_TERMINADO_DETALLE.FirstOrDefault(x => x.IdProductoTerminadoDetalle == control.IdProductoTerminadoDetalle);
                if (result != null)
                {
                    result.Turno = control.Turno;
                    result.HoraInicio = control.HoraInicio;
                    result.HoraFin = control.HoraFin;
                    result.Personal = control.Personal;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    control.FechaIngresoLog = DateTime.Now;
                    entities.PRODUCTO_TERMINADO_DETALLE.Add(control);
                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }

        public RespuestaGeneral EliminarProductoTerminadoDetalle(PRODUCTO_TERMINADO_DETALLE control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.PRODUCTO_TERMINADO_DETALLE.FirstOrDefault(x => x.IdProductoTerminadoDetalle == control.IdProductoTerminadoDetalle);
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
        #endregion

        #region PRODUCTO TERMINADO MATERIALES
        public List<spConsultaProductoTerminadoMateriales> ConsultaControlProductoTerminadoMateriales(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaProductoTerminadoMateriales(IdControl).ToList();
                return lista;
            }
        }
        public RespuestaGeneral GuardarModificarMateriales(PRODUCTO_TERMINADO_MATERIALES control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.PRODUCTO_TERMINADO_MATERIALES.FirstOrDefault(x => x.IdMateriales == control.IdMateriales 
                                                                                || (x.IdProductoTerminado==control.IdProductoTerminado
                                                                                    && x.CodigoMaterial == control.CodigoMaterial
                                                                                    && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
                if (result != null)
                {
                    result.Recibido = control.Recibido;
                    result.Desechado = control.Desechado;
                    result.Usado = control.Usado;
                    result.CodigoMaterial = control.CodigoMaterial;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    control.FechaIngresoLog = DateTime.Now;
                    entities.PRODUCTO_TERMINADO_MATERIALES.Add(control);
                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }

        public RespuestaGeneral EliminarProductoTerminadoMateriales(PRODUCTO_TERMINADO_MATERIALES control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.PRODUCTO_TERMINADO_MATERIALES.FirstOrDefault(x => x.IdMateriales == control.IdMateriales);
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
        #endregion

        #region PRODUCTO TERMINADO DANIADOS
        public List<spConsultaProductoTerminadoDaniados> ConsultaControlProductoTerminadoDaniados(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaProductoTerminadoDaniados(IdControl).ToList();
                return lista;
            }
        }
        public RespuestaGeneral GuardarModificarDaniados(PRODUCTO_TERMINADO_DANIADOS control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.PRODUCTO_TERMINADO_DANIADOS.FirstOrDefault(x => x.IdProductosDaniados == control.IdProductosDaniados | (x.IdProductoTerminado == control.IdProductoTerminado && x.Codigo == control.Codigo && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
                if (result != null)
                {
                    result.Cantidad = control.Cantidad;
                    result.Codigo = control.Codigo;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    control.FechaIngresoLog = DateTime.Now;
                    entities.PRODUCTO_TERMINADO_DANIADOS.Add(control);
                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }

        public RespuestaGeneral EliminarProductoTerminadoDaniado(PRODUCTO_TERMINADO_DANIADOS control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.PRODUCTO_TERMINADO_DANIADOS.FirstOrDefault(x => x.IdProductosDaniados == control.IdProductosDaniados);
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
        #endregion

        #region PRODUCTO TERMINADO TIEMPOS PARA
        public List<spConsultaProductoTerminadoTiempoPara> ConsultaControlProductoTerminadoTiempoPara(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaProductoTerminadoTiempoPara(IdControl).ToList();
                return lista;
            }
        }
        public RespuestaGeneral GuardarModificarTiempoPara(PRODUCTO_TERMINADO_TIEMPO_PARADAS control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.PRODUCTO_TERMINADO_TIEMPO_PARADAS.FirstOrDefault(x => x.IdTiempoParada == control.IdTiempoParada);
                if (result != null)
                {
                    result.HoraInicio = control.HoraInicio;
                    result.HoraFin = control.HoraFin;
                    result.Causa = control.Causa;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    control.FechaIngresoLog = DateTime.Now;
                    entities.PRODUCTO_TERMINADO_TIEMPO_PARADAS.Add(control);
                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }

        public RespuestaGeneral EliminarProductoTerminadoTiempoParada(PRODUCTO_TERMINADO_TIEMPO_PARADAS control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.PRODUCTO_TERMINADO_TIEMPO_PARADAS.FirstOrDefault(x => x.IdTiempoParada == control.IdTiempoParada);
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
        #endregion



        #region BANDEJA CC
       
       

        public List<spConsultaProductoTerminadoBandejaCC> ConsultaControlProductoTerminadoBandejaCC(DateTime FechaDesde, DateTime FechaHasta, bool Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaProductoTerminadoBandejaCC(FechaDesde, FechaHasta, Estado).ToList();
            }
        }

        //public List<PRODUCTO_TERMINADO> ConsultaProductoTerminadoPendiente()
        //{
        //    using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
        //    {
        //        return entities.PRODUCTO_TERMINADO.Where(x => x.EstadoReporte==false && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
        //    }
        //}


        public void Aprobar_ReporteProductoTerminado(PRODUCTO_TERMINADO controlCloro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.PRODUCTO_TERMINADO.FirstOrDefault(x => x.IdProductoTerminado == controlCloro.IdProductoTerminado);
                if (model != null)
                {
                    model.EstadoReporte = controlCloro.EstadoReporte;
                    model.AprobadoPor = controlCloro.AprobadoPor;
                    model.FechaAprobacion = controlCloro.FechaAprobacion;
                    model.FechaModificacionLog = controlCloro.FechaIngresoLog;
                    model.TerminalModificacionLog = controlCloro.TerminalIngresoLog;
                    model.UsuarioModificacionLog = controlCloro.UsuarioIngresoLog;
                    db.SaveChanges();
                }

            }
        }
        #endregion

        #region REPORTE
        public List<spConsultaProductoTerminadoReporte> ReporteConsultaControlProductoTerminado(DateTime Fecha, string Linea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaProductoTerminadoReporte(Fecha, Linea).ToList();
                return lista;
            }
        }
        #endregion
    }
}