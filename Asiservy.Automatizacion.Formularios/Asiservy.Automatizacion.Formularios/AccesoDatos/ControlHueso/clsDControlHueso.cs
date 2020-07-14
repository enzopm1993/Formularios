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
        clsDAsistencia clsDAsistencia { get; set; } = null;
        clsDApiOrdenFabricacion clsDApiOrdenFabricacion { get; set; } = null;
        clsDApiProduccion clsDApiProduccion { get;set; } = null;

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
        public List<spConsultaControlHueso> ConsultaControlHueso(DateTime Fecha, string CodLinea, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.spConsultaControlHueso(Fecha,CodLinea,Turno).ToList();
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
                List<spConsultaMovimientoPersonalDiario> detalle = null;
                
                var ControlHueso = entities.CONTROL_HUESO.FirstOrDefault(x =>
                x.Linea == doControl.Linea              
                && x.HoraFin == doControl.HoraFin
                && x.HoraInicio == doControl.HoraInicio               
                && x.Fecha == doControl.Fecha
                && x.Linea==doControl.Linea
                && ((doControl.Turno == clsAtributos.TurnoUno && (x.Turno == clsAtributos.TurnoUno || x.Turno == null))
                || (doControl.Turno == clsAtributos.TurnoDos && x.Turno == doControl.Turno))
                // && x.Turno==doControl.Turno
                && x.TipoControlHueso == doControl.TipoControlHueso
                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (ControlHueso == null)
                {
                    if (doControl.TipoControlHueso == clsAtributos.Hueso || doControl.TipoControlHueso == clsAtributos.Roto)
                    {
                        TimeSpan HoraIni = new TimeSpan(doControl.HoraInicio.Hour, doControl.HoraInicio.Minute, 0);
                        detalle = clsDAsistencia.ConsultaMovimientoPersonalDiario( doControl.HoraInicio, HoraIni, doControl.Linea,doControl.Turno).Where(x=> x.CodCargo==clsAtributos.CargoLimpiadora).ToList();
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
                    return doControl.IdControlHueso;
                }
                else
                {
                    return 0;
                }
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

        public List<spConsultaControlAvanceDiarioPorLinea> ConsultaControlAvanceDiarioPorLinea(DateTime FechaDesde, DateTime FechaHasta,string Turno, string Linea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {           
                GenerarAvanceOrdenesApi(FechaDesde,FechaHasta, Linea);
                List<spConsultaControlAvanceDiarioPorLinea> Listado = new List<spConsultaControlAvanceDiarioPorLinea>();
                Listado = entities.spConsultaControlAvanceDiarioPorLinea(FechaDesde,FechaHasta,Turno,Linea).ToList();
                return Listado;
            }

        }

        public void GenerarAvanceOrdenesApi(DateTime FechaDesde, DateTime? FechaHasta,string Linea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                if (FechaHasta == null)
                {
                    FechaHasta = FechaDesde;
                }
                List<CONTROL_AVANCE_API> ListadoControlAvanceApi = new List<CONTROL_AVANCE_API>();
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                var ordendesFabricacion = entities.CONTROL_HUESO.Where(x =>
                x.Fecha >= FechaDesde
               && x.Fecha <= FechaHasta
               && x.Linea == Linea
                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).Select(x => x.OrdenFabricacion).Distinct();

                //Consulta de servicio 
                var detalleOrden = clsDApiOrdenFabricacion.ConsultaDatosLotePorRangoFecha(FechaDesde, FechaHasta ?? FechaDesde);

                //recorrer las ordenes de fabricacion para actualizar los datos o agregar.
                foreach (int x in ordendesFabricacion)
                {
                    var ListaLotes = detalleOrden.Where(o => int.Parse(o.OrdenFabricacion) == x).ToList();
                    if(ListaLotes == null || ListaLotes.Count==0)
                    {
                        ListaLotes = clsDApiOrdenFabricacion.ConsultaLotesPorOrdenFabricacionLinea2(x,Linea);
                    }
                    foreach (var detalle in ListaLotes)
                    {
                        var modelControlAvanceApi = entities.CONTROL_AVANCE_API.FirstOrDefault(y => y.OrdenFabricacion == x && y.Lote == detalle.Lote);
                        if (modelControlAvanceApi == null)
                        {
                            if (!ListadoControlAvanceApi.Any(lista => lista.OrdenFabricacion == x && lista.Lote == detalle.Lote))
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
                        else
                        {
                            modelControlAvanceApi.Promedio = decimal.Parse(detalle.Promedio);
                            modelControlAvanceApi.Talla = detalle.Talla;
                            modelControlAvanceApi.Especie = detalle.Especie;
                            modelControlAvanceApi.Lote = detalle.Lote;
                            modelControlAvanceApi.Limpieza = detalle.Limpieza;
                            modelControlAvanceApi.Peso = int.Parse(double.Parse(detalle.Peso).ToString());
                            modelControlAvanceApi.Piezas = int.Parse(double.Parse(detalle.Piezas).ToString());                           
                        }
                    }
                }
                if (ListadoControlAvanceApi.Any())
                {
                    var prueba = ListadoControlAvanceApi.Distinct().ToList();
                    entities.CONTROL_AVANCE_API.AddRange(ListadoControlAvanceApi.Distinct());                   
                }
                entities.SaveChanges();
            }
        }

        

        public void GenerarAvanceOrdenesApi2(DateTime FechaDesde, DateTime? FechaHasta)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                if (FechaHasta == null)
                {
                    FechaHasta = FechaDesde;
                }
                List<CONTROL_AVANCE_API> ListadoControlAvanceApi = new List<CONTROL_AVANCE_API>();
                clsDApiOrdenFabricacion = new clsDApiOrdenFabricacion();
                var ordendesFabricacion = entities.CONTROL_HUESO.Where(x =>
                x.Fecha >= FechaDesde
               && x.Fecha <= FechaHasta
               && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).Select(x => x.OrdenFabricacion).Distinct();

                //Consulta de servicio 
                var detalleOrden = clsDApiOrdenFabricacion.ConsultaDatosLotePorRangoFecha(FechaDesde, FechaHasta ?? FechaDesde);

                //recorrer las ordenes de fabricacion para actualizar los datos o agregar.
                foreach (int x in ordendesFabricacion)
                {
                    var ListaLotes = detalleOrden.Where(o => int.Parse(o.OrdenFabricacion) == x).ToList();
                    if (ListaLotes == null || ListaLotes.Count == 0)
                    {
                        ListaLotes = clsDApiOrdenFabricacion.ConsultaLotesPorOFCompleto(x);
                    }
                    foreach (var detalle in ListaLotes)
                    {
                        var modelControlAvanceApi = entities.CONTROL_AVANCE_API.FirstOrDefault(y => y.OrdenFabricacion == x && y.Lote == detalle.Lote);
                        if (modelControlAvanceApi == null)
                        {
                            if (!ListadoControlAvanceApi.Any(lista => lista.OrdenFabricacion == x && lista.Lote == detalle.Lote))
                            {
                                ListadoControlAvanceApi.Add(new CONTROL_AVANCE_API
                                {
                                    OrdenFabricacion = x,
                                    Limpieza = detalle.Limpieza,
                                    Lote = detalle.Lote,
                                    Peso = detalle.Peso != null ? int.Parse(double.Parse(detalle.Peso).ToString()) : 0,
                                    Piezas = detalle.Piezas != null ? int.Parse(double.Parse(detalle.Piezas).ToString()) : 0,
                                    Talla = detalle.Talla,
                                    Promedio = detalle.Promedio != null ? decimal.Parse(detalle.Promedio) : 0,
                                    Especie = detalle.Especie,
                                    Producto = detalle.Producto,
                                    LomoReal = detalle.Lomos,
                                    MigaReal = detalle.Migas
                                });
                            }
                        }
                        else
                        {
                            modelControlAvanceApi.Promedio = detalle.Promedio != null ? decimal.Parse(detalle.Promedio) : 0;
                            modelControlAvanceApi.Talla = detalle.Talla;
                            modelControlAvanceApi.LomoReal = detalle.Lomos;
                            modelControlAvanceApi.MigaReal = detalle.Migas;
                            modelControlAvanceApi.Especie = detalle.Especie;
                            modelControlAvanceApi.Lote = detalle.Lote;
                            modelControlAvanceApi.Limpieza = detalle.Limpieza;
                            modelControlAvanceApi.Peso = detalle.Peso != null ? int.Parse(double.Parse(detalle.Peso).ToString()) : 0;
                            modelControlAvanceApi.Piezas = detalle.Piezas != null ? int.Parse(double.Parse(detalle.Piezas).ToString()) : 0;
                        }
                    }
                }
                if (ListadoControlAvanceApi.Any())
                {
                    entities.CONTROL_AVANCE_API.AddRange(ListadoControlAvanceApi.Distinct());
                }
                entities.SaveChanges();
            }
        }

        

        public List<spConsultaAvanceDiarioPorLimpiadora> ConsultaControlAvanceDiarioPorLimpiadora(DateTime Fecha, string Linea,string turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                GenerarAvanceOrdenesApi(Fecha,null, Linea);
                clsDApiProduccion = new clsDApiProduccion();
                GenerarRendimientos();
                List<spConsultaAvanceDiarioPorLimpiadora> Listado;
                Listado = entities.spConsultaAvanceDiarioPorLimpiadora(Fecha, Linea, turno).ToList();
                return Listado;
            }

        }

        public List<spKpiAvancePorLimpiadora> ConsultaKpiAvanceDiarioPorLimpiadora(DateTime Fecha, string Cedula)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
               
                List<spKpiAvancePorLimpiadora> Listado;
                Listado = entities.spKpiAvancePorLimpiadora(Fecha, Cedula).ToList();
                return Listado;
            }

        }


        public List<spConsultaReporteAvanceDiario> ConsultaControlAvanceDiario (DateTime Fecha, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                GenerarAvanceOrdenesApi2(Fecha, null);
                List<spConsultaReporteAvanceDiario> Listado = new List<spConsultaReporteAvanceDiario>();
                Listado = entities.spConsultaReporteAvanceDiario(Fecha, Turno).ToList();
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



        public List<spConsultaReporteRendimientoLote> ConsultaReporteRendimientoPorLte(DateTime Fecha, string Turno)
        {
            clsDApiProduccion = new clsDApiProduccion();
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                GenerarAvanceOrdenesApi2(Fecha, Fecha);
                GenerarRendimientos();
                List<spConsultaReporteRendimientoLote> Listado;
                Listado = entities.spConsultaReporteRendimientoLote(Fecha, Turno).ToList();
                return Listado;
            }

        }


        public void GenerarRendimientos()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                clsDApiProduccion = new clsDApiProduccion();
                var Rendimientos = clsDApiProduccion.ConsultaRendimientos();
                if (Rendimientos != null)
                {
                    entities.Database.ExecuteSqlCommand("TRUNCATE TABLE RENDIMIENTO_KILO_HORA");
                    foreach (var r in Rendimientos)
                    {
                        entities.RENDIMIENTO_KILO_HORA.Add(
                           new RENDIMIENTO_KILO_HORA()
                           {
                               Codigo = r.U_SYP_ITEM_CODE,
                               Periodo = r.U_SYP_PERIODO,
                               Talla = r.U_SYP_TALLA,
                               LimpiezaSimpleLomo = r.U_SYP_LS_LOMO,
                               LimpiezaSimpleMiga = r.U_SYP_LS_MIGA,
                               LimpiezaIntermediaLomo = r.U_SYP_LI_LOMO,
                               LimpiezaIntermediaMiga = r.U_SYP_LI_MIGA,
                               LimpiezaDobleLomo = r.U_SYP_LD_LOMO,
                               LimpiezaDobleMiga = r.U_SYP_LD_MIGA
                           }
                           );
                    }
                    entities.SaveChanges();
                }
            }
        }

        public List<RENDIMIENTO_KILO_HORA> ConsultaRendimientos()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                GenerarRendimientos();
                return entities.RENDIMIENTO_KILO_HORA.AsNoTracking().ToList();
            }
        }




    }
}