using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ControlHueso
{
    public class clsDControlHueso
    {
        clsDAsistencia clsDAsistencia = null;
        clsDApiOrdenFabricacion clsDApiOrdenFabricacion = null;

        public RespuestaGeneral GuardarModificarControl(CONTROL_HUESO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONTROL_HUESO.FirstOrDefault(x => x.IdControlHueso == control.IdControlHueso);
                if (result != null)
                {
                    result.HoraInicio = control.HoraInicio;
                    result.HoraFin = control.HoraFin;
                    result.Observacion = control.Observacion;
                    result.Limpieza = control.Limpieza;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                    entities.SaveChanges();
                    return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta=true };

                }
                else
                {
                    return new RespuestaGeneral { Mensaje = "No se encontró ningun control", Respuesta = false};

                }

            }
        }

        public string GuardarModificarControlHueso(CONTROL_HUESO_DETALLE detalle)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONTROL_HUESO_DETALLE.FirstOrDefault(x=>x.IdControlHuesoDetalle == detalle.IdControlHuesoDetalle);
                if(result!=null)
                {
                    result.CantidadHueso = detalle.CantidadHueso;
                    result.UsuarioModificacionLog = detalle.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = detalle.TerminalIngresoLog;
                    entities.SaveChanges();
                }

                return clsAtributos.MsjRegistroGuardado;
            }
        }

        public List<spConsultaControlHuesoDetalle> ConsultaControlHuesoDetalle(int id)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.spConsultaControlHuesoDetalle(id).ToList();
                return result;
            }
        }
        public List<spConsultaControlHueso> ConsultaControlHueso(DateTime Fecha, string CodLinea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.spConsultaControlHueso(Fecha).Where(x=> x.CodLinea == CodLinea).ToList();
                return result;
            }
        }

        public int ValidaControlHueso(CONTROL_HUESO doControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var FechaActual = DateTime.Now.Date;
                var ControlHueso = entities.CONTROL_HUESO.FirstOrDefault(x =>
                x.Linea == doControl.Linea              
               // && x.Hora == doControl.Hora
                && x.Fecha == FechaActual);
                if (ControlHueso != null)
                    return ControlHueso.IdControlHueso;
                else
                    return 0;
            }
        }

        public void InactivarControlHueso(CONTROL_HUESO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var ControlHueso = entities.CONTROL_HUESO.FirstOrDefault(x =>
              x.IdControlHueso == model.IdControlHueso);

                ControlHueso.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                ControlHueso.FechaModificacionLog = model.FechaIngresoLog;
                ControlHueso.TerminalModificacionLog = model.TerminalIngresoLog;
                ControlHueso.UsuarioModificacionLog = model.UsuarioIngresoLog;

                entities.SaveChanges();
            }

        }

        public int GenerarControlHueso(CONTROL_HUESO doControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                clsDAsistencia = new clsDAsistencia();
                List<spConsultaMovimientoPersonalDiario> detalle = new List<spConsultaMovimientoPersonalDiario>();
                var FechaActual = DateTime.Now.Date;
                
                var ControlHueso = entities.CONTROL_HUESO.FirstOrDefault(x =>
                x.Linea == doControl.Linea              
                && x.HoraFin == doControl.HoraFin
                && x.HoraInicio == doControl.HoraInicio               
                && x.Fecha == doControl.Fecha
                && x.Linea==doControl.Linea
                && x.TipoControlHueso == doControl.TipoControlHueso
                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (ControlHueso == null)
                {
                    if (doControl.TipoControlHueso == clsAtributos.Hueso || doControl.TipoControlHueso == clsAtributos.Roto)
                    {
                        detalle = clsDAsistencia.ConsultaMovimientoPersonalDiario( doControl.Fecha,doControl.HoraInicio ,doControl.Linea).Where(x=> x.CodCargo==clsAtributos.CargoLimpiadora).ToList();
                        foreach (var x in detalle)
                        {
                            doControl.CONTROL_HUESO_DETALLE.Add(new CONTROL_HUESO_DETALLE
                            {
                                CantidadHueso = 0,
                                Cedula = x.Cedula,
                                EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                                FechaIngresoLog = DateTime.Now,
                                UsuarioIngresoLog = doControl.UsuarioIngresoLog,
                                TerminalIngresoLog = doControl.TerminalIngresoLog
                            });
                        }
                    }
                    entities.CONTROL_HUESO.Add(doControl);
                    entities.SaveChanges();
                }
                else
                {
                    return 0;
                }

                var idControlHueso = entities.CONTROL_HUESO.FirstOrDefault(x =>
                   x.Linea == doControl.Linea
                   && x.Lote == doControl.Lote
                   && x.HoraInicio == doControl.HoraInicio
                   && x.HoraFin == doControl.HoraFin
                   && x.OrdenFabricacion == doControl.OrdenFabricacion
                   && x.Fecha == doControl.Fecha
                  // && x.FechaIngresoLog == doControl.FechaIngresoLog
                   && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                return idControlHueso.IdControlHueso;
            }
        }   

       

        public List<spConsultaLimpiadorasControlHueso> ConsultaLimpiadorasControlHueso(string Linea, DateTime Fecha)
        {
            using(ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<spConsultaLimpiadorasControlHueso> Listado = new List<spConsultaLimpiadorasControlHueso>();
                Listado = entities.spConsultaLimpiadorasControlHueso(Linea, Fecha).ToList();
                return Listado;
            }
        }

        public List<spConsultaControlAvanceDiarioPorLinea> ConsultaControlAvanceDiarioPorLinea(DateTime Fecha, string Linea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<CONTROL_AVANCE_API> ListadoControlAvanceApi = new List<CONTROL_AVANCE_API>();
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                var ordendesFabricacion = entities.CONTROL_HUESO.Where(x =>
                x.Fecha == Fecha
                && x.Linea == Linea
                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).Select(x=> x.OrdenFabricacion).Distinct();

                foreach(int x in ordendesFabricacion)
                {
                    var detalleOrden = clsDApiOrdenFabricacion.ConsultaLotesPorOrdenFabricacionLinea2(x, Linea);
                    foreach (var detalle in detalleOrden)                    {
                        
                        var modelControlAvanceApi = entities.CONTROL_AVANCE_API.FirstOrDefault(y => y.OrdenFabricacion == x && y.Lote == detalle.Lote);
                        if(modelControlAvanceApi == null)
                        {                           
                            ListadoControlAvanceApi.Add(new CONTROL_AVANCE_API
                            {           
                                OrdenFabricacion = x,
                                Limpieza = detalle.Limpieza,
                                Lote = detalle.Lote,
                                Peso = int.Parse(double.Parse(detalle.Peso).ToString()),
                                Piezas = int.Parse(double.Parse(detalle.Piezas).ToString()),
                                Talla = detalle.Talla,
                                Promedio = decimal.Parse(detalle.Promedio),
                                Especie = detalle.Especie,
                                Producto = detalle.Producto
                            });
                        }                      
                    }                    
                }
                if (ListadoControlAvanceApi.Any())
                {
                    entities.CONTROL_AVANCE_API.AddRange(ListadoControlAvanceApi);
                    entities.SaveChanges();
                }

                List<spConsultaControlAvanceDiarioPorLinea> Listado = new List<spConsultaControlAvanceDiarioPorLinea>();
                Listado = entities.spConsultaControlAvanceDiarioPorLinea(Fecha,Linea).ToList();
                return Listado;
            }

        }

        public List<spConsultaAvanceDiarioPorLimpiadora> ConsultaControlAvanceDiarioPorLimpiadora(DateTime Fecha, string Linea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<spConsultaAvanceDiarioPorLimpiadora> Listado = new List<spConsultaAvanceDiarioPorLimpiadora>();
                Listado = entities.spConsultaAvanceDiarioPorLimpiadora(Fecha, Linea).ToList();
                return Listado;
            }

        }

        public List<AVANCE_KILOS_HORA> ConsultaEficienciaAvanceKilosHora()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<AVANCE_KILOS_HORA> Listado = new List<AVANCE_KILOS_HORA>();
                Listado = entities.AVANCE_KILOS_HORA.ToList();
                return Listado;
            }
        }

        public void GuardarModificarEficienciaAvanceKilosHora(AVANCE_KILOS_HORA model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                
                var result = entities.AVANCE_KILOS_HORA.FirstOrDefault(x=> x.IdAvanceKilosHora == model.IdAvanceKilosHora);
                if(result!= null){
                    result.Intermedia = model.Intermedia;
                    result.Sencilla = model.Sencilla;
                    result.Doble = model.Doble;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = model.TerminalIngresoLog;
                    result.UsuarioModificacionLog = model.UsuarioIngresoLog;
                }
                else
                {
                    entities.AVANCE_KILOS_HORA.Add(model);
                }
                entities.SaveChanges();
                
            }
        }

    }
}