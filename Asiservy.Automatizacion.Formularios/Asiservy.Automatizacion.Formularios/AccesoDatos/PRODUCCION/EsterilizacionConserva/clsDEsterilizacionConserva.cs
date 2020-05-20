using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Produccion.EsterilizacionConservas;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.PRODUCCION.EsterilizacionConserva
{
    public class clsDEsterilizacionConserva
    {
        public CABECERA_CONTROL_ESTERILIZACION_CONSERVAS ConsultarCabeceraEsterilizacionConserva(CABECERA_CONTROL_ESTERILIZACION_CONSERVAS poEsterilizacionConserva)
        {
            using (var db=new ASIS_PRODEntities())
            {
                return db.CABECERA_CONTROL_ESTERILIZACION_CONSERVAS.Where(x => x.Fecha == poEsterilizacionConserva.Fecha && x.Turno == poEsterilizacionConserva.Turno && x.TipoLinea== poEsterilizacionConserva.TipoLinea && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
            }
        }
        public object[] GuardarCabEsterilizacionConserva(CABECERA_CONTROL_ESTERILIZACION_CONSERVAS poEsterilizacionConserva)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarCabecera = db.CABECERA_CONTROL_ESTERILIZACION_CONSERVAS.Where(x => x.Fecha == poEsterilizacionConserva.Fecha && x.Turno == poEsterilizacionConserva.Turno
                  && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (buscarCabecera == null)
                {
                    db.CABECERA_CONTROL_ESTERILIZACION_CONSERVAS.Add(poEsterilizacionConserva);
                    db.SaveChanges();
                    resultado[0] = "000";
                    resultado[1] = "Registro ingresado con éxito";
                    resultado[2] = poEsterilizacionConserva;
                }
                else
                {
                    resultado[0] = "002";
                    resultado[1] = "Error, el registro ya existe";
                    resultado[2] = buscarCabecera;
                }
                
                return resultado;
            }
        }
        public object[] ActualizarCabEsterilizacionConserva(CABECERA_CONTROL_ESTERILIZACION_CONSERVAS poEsterilizacionConserva)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabeceraControl = db.CABECERA_CONTROL_ESTERILIZACION_CONSERVAS.FirstOrDefault(x => x.IdCabControlEsterilizado == poEsterilizacionConserva.IdCabControlEsterilizado);
                BuscarCabeceraControl.Observacion = poEsterilizacionConserva.Observacion;
                BuscarCabeceraControl.FechaModificacionLog = poEsterilizacionConserva.FechaIngresoLog;
                BuscarCabeceraControl.UsuarioModificacionLog = poEsterilizacionConserva.UsuarioIngresoLog;
                BuscarCabeceraControl.TerminalModificacionLog = poEsterilizacionConserva.TerminalIngresoLog;
                db.SaveChanges();
                resultado[0] = "001";
                resultado[1] = "Registro actualizado con éxito";
                resultado[2] = poEsterilizacionConserva;
                return resultado;
            }
        }
        public object[] InactivarCabEsterilizacionConserva(CABECERA_CONTROL_ESTERILIZACION_CONSERVAS poEsterilizacionConserva)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabeceraControl = db.CABECERA_CONTROL_ESTERILIZACION_CONSERVAS.FirstOrDefault(x => x.IdCabControlEsterilizado == poEsterilizacionConserva.IdCabControlEsterilizado);
                BuscarCabeceraControl.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                BuscarCabeceraControl.FechaModificacionLog = poEsterilizacionConserva.FechaIngresoLog;
                BuscarCabeceraControl.UsuarioModificacionLog = poEsterilizacionConserva.UsuarioIngresoLog;
                BuscarCabeceraControl.TerminalModificacionLog = poEsterilizacionConserva.TerminalIngresoLog;
                db.SaveChanges();
                resultado[0] = "002";
                resultado[1] = "Registro Inactivado con éxito";
                resultado[2] = poEsterilizacionConserva;
                return resultado;
            }
        }
        public object[] InactivarDetalleEsterilizacionConserva(DETALLE_CONTROL_ESTERILIZACION_CONSERVA poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarDetalleControl = db.DETALLE_CONTROL_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.IdDetalleControlEsterilizacionConserva == poDetalleControl.IdDetalleControlEsterilizacionConserva);
                BuscarDetalleControl.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                BuscarDetalleControl.FechaModificacionLog = poDetalleControl.FechaIngresoLog;
                BuscarDetalleControl.UsuarioModificacionLog = poDetalleControl.UsuarioIngresoLog;
                BuscarDetalleControl.TerminalModificacionLog = poDetalleControl.TerminalIngresoLog;
                db.SaveChanges();
                resultado[0] = "002";
                resultado[1] = "Registro Inactivado con éxito";
                resultado[2] = poDetalleControl;
                return resultado;
            }
        }
        public object[] GuardarDetalleEsterilizacion(DETALLE_CONTROL_ESTERILIZACION_CONSERVA poDetalleEsterilizacion)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                db.DETALLE_CONTROL_ESTERILIZACION_CONSERVA.Add(poDetalleEsterilizacion);
                db.SaveChanges();
                resultado[0] = "000";
                resultado[1] = "Registro ingresado con éxito";
                //resultado[2] = poDetalleEsterilizacion;
                resultado[2] = new
                {
                    poDetalleEsterilizacion.EstadoRegistro,
                    poDetalleEsterilizacion.FechaIngresoLog,
                    poDetalleEsterilizacion.FechaModificacionLog,
                    poDetalleEsterilizacion.HoraCierreViento,
                    poDetalleEsterilizacion.HoraFinalEsterilizacion,
                    poDetalleEsterilizacion.HoraInicioCalentamiento,
                    poDetalleEsterilizacion.HoraInicioEsterilizacion,
                    poDetalleEsterilizacion.HoraInicioLlenado,
                    poDetalleEsterilizacion.HoraInicioViento,
                    poDetalleEsterilizacion.IdCabControlEsterilizacionConservas,
                    poDetalleEsterilizacion.IdCabeceraCoche,
                    poDetalleEsterilizacion.IdDetalleControlEsterilizacionConserva,
                    poDetalleEsterilizacion.TemperaturaInicial,
                    poDetalleEsterilizacion.TemperaturaProductoSalida,
                    poDetalleEsterilizacion.TemperaturaTermDigital,
                    poDetalleEsterilizacion.TerminalIngresoLog,
                    poDetalleEsterilizacion.TerminalModificacionLog,
                    poDetalleEsterilizacion.TiempoEnfriamiento,
                    poDetalleEsterilizacion.UsuarioIngresoLog,
                    poDetalleEsterilizacion.UsuarioModificacionLog
                };
                return resultado;
            }
        }
        public object[] ModificarDetalleEsterilizacion(DETALLE_CONTROL_ESTERILIZACION_CONSERVA poDetalleEsterilizacion)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarDetalle=db.DETALLE_CONTROL_ESTERILIZACION_CONSERVA.Find(poDetalleEsterilizacion.IdDetalleControlEsterilizacionConserva);
                BuscarDetalle.FechaModificacionLog = poDetalleEsterilizacion.FechaIngresoLog;
                BuscarDetalle.HoraCierreViento = poDetalleEsterilizacion.HoraCierreViento;
                BuscarDetalle.HoraFinalEsterilizacion = poDetalleEsterilizacion.HoraFinalEsterilizacion;
                BuscarDetalle.HoraInicioCalentamiento = poDetalleEsterilizacion.HoraInicioCalentamiento;
                BuscarDetalle.HoraInicioEsterilizacion = poDetalleEsterilizacion.HoraInicioEsterilizacion;
                BuscarDetalle.HoraInicioLlenado = poDetalleEsterilizacion.HoraInicioLlenado;
                BuscarDetalle.HoraInicioViento = poDetalleEsterilizacion.HoraInicioViento;
                BuscarDetalle.TemperaturaInicial = poDetalleEsterilizacion.TemperaturaInicial;
                BuscarDetalle.TemperaturaProductoSalida = poDetalleEsterilizacion.TemperaturaProductoSalida;
                BuscarDetalle.TemperaturaTermDigital = poDetalleEsterilizacion.TemperaturaTermDigital;
                BuscarDetalle.TerminalModificacionLog = poDetalleEsterilizacion.TerminalIngresoLog;
                BuscarDetalle.TiempoEnfriamiento = poDetalleEsterilizacion.TiempoEnfriamiento;
                BuscarDetalle.UsuarioModificacionLog = poDetalleEsterilizacion.UsuarioIngresoLog;
                foreach (var item in BuscarDetalle.TIPO_ESTERILIZACION_CONSERVA)
                {
                    if (item.Tipo == clsAtributos.Inicio)
                    {
                        item.Panel = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Inicio).Panel;
                        item.Chart = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Inicio).Chart;
                        item.TermometroDigital = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Inicio).TermometroDigital;
                        item.PresionManometro = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Inicio).PresionManometro;
                        item.HoraChequeo = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Inicio).HoraChequeo;
                    }
                    if (item.Tipo == clsAtributos.Medio)
                    {
                        item.Panel = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Medio).Panel;
                        item.Chart = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Medio).Chart;
                        item.TermometroDigital = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Medio).TermometroDigital;
                        item.PresionManometro = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Medio).PresionManometro;
                        item.HoraChequeo = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Medio).HoraChequeo;
                    }
                    if (item.Tipo == clsAtributos.Final)
                    {
                        item.Panel = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Final).Panel;
                        item.Chart = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Final).Chart;
                        item.TermometroDigital = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Final).TermometroDigital;
                        item.PresionManometro = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Final).PresionManometro;
                        item.HoraChequeo = poDetalleEsterilizacion.TIPO_ESTERILIZACION_CONSERVA.FirstOrDefault(x => x.Tipo == clsAtributos.Final).HoraChequeo;
                    }
                    item.FechaModificacionLog = poDetalleEsterilizacion.FechaIngresoLog;
                    item.UsuarioModificacionLog = poDetalleEsterilizacion.UsuarioIngresoLog;
                    item.TerminalModificacionLog = poDetalleEsterilizacion.TerminalIngresoLog;
                }
                db.SaveChanges();
                resultado[0] = "001";
                resultado[1] = "Registro actualizado con éxito";
                resultado[2] = poDetalleEsterilizacion;
                return resultado;
            }
        }
        public List<DetalleEsterilizacionConservaVieModel> ConsultarDetalleEsterilizacion(int piCabeceraControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                List<DetalleEsterilizacionConservaVieModel> resultado = (from d in db.DETALLE_CONTROL_ESTERILIZACION_CONSERVA
                                 join c in db.COCHE_AUTOCLAVE on new { IdCocheAutoclave=d.IdCabeceraCoche, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { c.IdCocheAutoclave, c.EstadoRegistro}
                                 where d.IdCabControlEsterilizacionConservas == piCabeceraControl && d.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                 select new DetalleEsterilizacionConservaVieModel{Autoclave= c.Autoclave,Esterilizada= c.Parada,Producto=c.Producto,
                                     IdDetalleControlEsterilizacionConserva=d.IdDetalleControlEsterilizacionConserva,TemperaturaInicial=d.TemperaturaInicial,
                                     HoraInicioViento=d.HoraInicioViento,HoraCierreViento=d.HoraCierreViento,TemperaturaTermDigital=d.TemperaturaTermDigital
                                 ,HoraInicioLlenado=d.HoraInicioLlenado,HoraInicioCalentamiento=d.HoraInicioCalentamiento,
                                     HoraInicioEsterilizacion=d.HoraInicioEsterilizacion,HoraFinalEsterilizacion=d.HoraFinalEsterilizacion,
                                     TiempoEnfriamiento=d.TiempoEnfriamiento,TemperaturaProductoSalida=d.TemperaturaProductoSalida,IdCabCoche=d.IdCabeceraCoche}).ToList();
                return resultado;
            }

        }
        public List<TIPO_ESTERILIZACION_CONSERVA> ConsultarTiposEsterilizacion(int piDetalleEsterilizacion)
        {
            using (var db = new ASIS_PRODEntities())
            {
                var resultado = (from t in db.TIPO_ESTERILIZACION_CONSERVA
                                 where t.IdDetalleControlEsterilizacion == piDetalleEsterilizacion && t.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                 select t).ToList();
                return resultado;
            }
        }
        public List<spReporteEsterilizacionDetalle> ConsultarDetalleReporteControlEsterilizacion(DateTime Fecha, string Turno, string Linea)
        {
            using (var db = new ASIS_PRODEntities())
            {
                db.Database.CommandTimeout = 180;
                return db.spReporteEsterilizacionDetalle(Fecha, Turno, Linea).ToList();
            }
        }
        public List<COCHE_AUTOCLAVE_DETALLE> ConsultarReporteDetallesCoches(int[] IdCoches)
        {
            using (var db = new ASIS_PRODEntities())
            {
                List<COCHE_AUTOCLAVE_DETALLE> resultado = (from d in db.COCHE_AUTOCLAVE_DETALLE
                                 where IdCoches.Contains(d.IdCocheAutoclave)
                                 select d).ToList();
                return resultado;
            }
        }
        public List<TIPO_ESTERILIZACION_CONSERVA> ConsultarTiposReporteEsterilizado(int[] IdDetalle)
        {
            using (var db = new ASIS_PRODEntities())
            {
                List<TIPO_ESTERILIZACION_CONSERVA> resultado = (from t in db.TIPO_ESTERILIZACION_CONSERVA
                                                                where IdDetalle.Contains(t.IdDetalleControlEsterilizacion)
                                                                select t).ToList();
                return resultado;
            }
        }

    }
}