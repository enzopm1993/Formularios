using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.ControlPesoyCodificacion;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ControlPesoyCodificacionLomosyMigas
{
    public class ClsdControlPesoCodificacionLomosyMigas
    {
        public object[] GuardarCabeceraControl(CABECERA_CONTROL_PESO_CODIFICACION Cabecera_Control)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabecera = db.CABECERA_CONTROL_PESO_CODIFICACION.Where(z => z.FechaProduccion == Cabecera_Control.FechaProduccion && z.Turno == Cabecera_Control.Turno
                  && z.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (BuscarCabecera==null)
                {
                    db.CABECERA_CONTROL_PESO_CODIFICACION.Add(Cabecera_Control);
                    db.SaveChanges();
                    resultado[0] = "111";
                    resultado[1] = "Registro Ingresado con éxito";
                    resultado[2] = db.CABECERA_CONTROL_PESO_CODIFICACION.Where(x => x.FechaProduccion == Cabecera_Control.FechaProduccion && x.Turno == Cabecera_Control.Turno).Select(x => new { x.IdCabeceraControlPesoYCodificacion, x.Observacion, x.SaldoAnterior, x.SolicitudProceso, x.Utilizadas }).FirstOrDefault(); ;
                    return resultado;
                }
                else
                {
                    resultado[0] = "666";
                    resultado[1] = "Error, Ya existe un control para la fecha y turno ingresado";
                    resultado[2] = "";
                    return resultado;
                }
            }
        }
        public object[] ActualizarCabeceraControl(CABECERA_CONTROL_PESO_CODIFICACION Cabecera_Control)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabeceraControl = db.CABECERA_CONTROL_PESO_CODIFICACION.Find(Cabecera_Control.IdCabeceraControlPesoYCodificacion);
                BuscarCabeceraControl.UsuarioModificacionLog = Cabecera_Control.UsuarioCreacionLog;
                BuscarCabeceraControl.FechaModificacionLog = DateTime.Now;
                BuscarCabeceraControl.TerminalModificacionLog = Cabecera_Control.TerminalCreacionLog;
                BuscarCabeceraControl.Observacion = Cabecera_Control.Observacion;
                BuscarCabeceraControl.SaldoAnterior = Cabecera_Control.SaldoAnterior;
                BuscarCabeceraControl.SolicitudProceso = Cabecera_Control.SolicitudProceso;
                BuscarCabeceraControl.Utilizadas = Cabecera_Control.Utilizadas;
                db.SaveChanges();
                resultado[0] = "111";
                resultado[1] = "Registro actualizado con éxito";
                resultado[2] = db.CABECERA_CONTROL_PESO_CODIFICACION.Where(x => x.FechaProduccion == Cabecera_Control.FechaProduccion && x.Turno == Cabecera_Control.Turno).Select(x=>new {x.IdCabeceraControlPesoYCodificacion,x.Observacion,x.SaldoAnterior,x.SolicitudProceso,x.Utilizadas }).FirstOrDefault(); ;
                return resultado;

            }
        }
        public object[] ConsultarCabControl(CABECERA_CONTROL_PESO_CODIFICACION Cabecera_Control)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabecera = db.CABECERA_CONTROL_PESO_CODIFICACION.Where(z => z.FechaProduccion == Cabecera_Control.FechaProduccion && z.Turno == Cabecera_Control.Turno
                  && z.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (BuscarCabecera != null)
                {
                    resultado[0] = "111";
                    resultado[1] = BuscarCabecera.IdCabeceraControlPesoYCodificacion;
                    resultado[2] =new {BuscarCabecera.Observacion,BuscarCabecera.SaldoAnterior,BuscarCabecera.SolicitudProceso, BuscarCabecera.Utilizadas };
                    return resultado;
                }
                else
                {
                    resultado[0] = "222";
                    resultado[1] = "No existe el control";
                    return resultado;
                }
            }
        }

        public object[] GuardarHorasControl(DETALLE_CONTROL_PESO_CODIFICACION Hora)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[2];
                var buscarHoraDet = db.DETALLE_CONTROL_PESO_CODIFICACION.Where(z => z.Hora == Hora.Hora && z.IdCabeceraControlPesoCodificacion == Hora.IdCabeceraControlPesoCodificacion
                  && z.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (buscarHoraDet == null)
                {
                    db.DETALLE_CONTROL_PESO_CODIFICACION.Add(Hora);
                    db.SaveChanges();
                    resultado[0] = "111";
                    resultado[1] = "Registro ingresado con éxito";
                    return resultado;
                }
                else
                {
                    resultado[0] = "666";
                    resultado[1] = "Error, Ya existe un control para la Hora ingresada";
                    return resultado;
                }
            }
        }
        public object[] ActualizarHorasControl(DETALLE_CONTROL_PESO_CODIFICACION Hora)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[2];
                var BuscarHoraDetalle = db.DETALLE_CONTROL_PESO_CODIFICACION.Find(Hora.IdDetalleControlPeso);
                BuscarHoraDetalle.UsuarioModificacionLog = Hora.UsuarioCreacionLog;
                BuscarHoraDetalle.FechaModificacionLog = DateTime.Now;
                BuscarHoraDetalle.TerminalModificacionLog = Hora.TerminalCreacionLog;
                BuscarHoraDetalle.TemperaturaAguaTermoencogido = Hora.TemperaturaAguaTermoencogido;
                db.SaveChanges();
                resultado[0] = "111";
                resultado[1] = "Registro actualizado con éxito";
                return resultado;
               
            }
        }
        public object[] ActualizarMuestrasPorHora(DETALLE_HORAS_CONTROL_PESO_CODIFICACION Muestra)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[2];
                var BuscarHoraDetalle = db.DETALLE_HORAS_CONTROL_PESO_CODIFICACION.Find(Muestra.IdDetalleHorasControlPesoCodificacion);
                BuscarHoraDetalle.UsuarioModificacionLog = Muestra.UsuarioCreacionLog;
                BuscarHoraDetalle.FechaModificacionLog = DateTime.Now;
                BuscarHoraDetalle.TerminalModificacionLog = Muestra.TerminalCreacionLog;
                BuscarHoraDetalle.Cantidad = Muestra.Cantidad;
                db.SaveChanges();
                resultado[0] = "111";
                resultado[1] = "Registro actualizado con éxito";
                return resultado;

            }
        }
        public object[] GuardarMuestrasPorHora(DETALLE_HORAS_CONTROL_PESO_CODIFICACION Muestra)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[2];
                var buscarHoraDet = db.DETALLE_HORAS_CONTROL_PESO_CODIFICACION.Where(z => z.IdDetalleControlPesoCodificacion == Muestra.IdDetalleControlPesoCodificacion && z.NumeroMuestra == Muestra.NumeroMuestra
                  && z.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (buscarHoraDet == null)
                {
                    db.DETALLE_HORAS_CONTROL_PESO_CODIFICACION.Add(Muestra);
                    db.SaveChanges();
                    resultado[0] = "111";
                    resultado[1] = "Registro ingresado con éxito";
                    return resultado;
                }
                else
                {
                    resultado[0] = "666";
                    resultado[1] = "Error, Ya existe un control para la fecha y turno ingresado";
                    return resultado;
                }
            }
        }
        public object[] InactivarHora(DETALLE_CONTROL_PESO_CODIFICACION ControlHora)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[2];
                var BuscarHora = db.DETALLE_CONTROL_PESO_CODIFICACION.Find(ControlHora.IdDetalleControlPeso);
                BuscarHora.UsuarioModificacionLog = ControlHora.UsuarioCreacionLog;
                BuscarHora.FechaModificacionLog = DateTime.Now;
                BuscarHora.TerminalModificacionLog = ControlHora.TerminalCreacionLog;
                BuscarHora.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                db.SaveChanges();
                resultado[0] = "111";
                resultado[1] = "Registro Inactivado con éxito";
                return resultado;

            }
        }
        public object[] InactivarMuestra(DETALLE_HORAS_CONTROL_PESO_CODIFICACION Muestra)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[2];
                var BuscarHora = db.DETALLE_HORAS_CONTROL_PESO_CODIFICACION.Find(Muestra.IdDetalleHorasControlPesoCodificacion);
                BuscarHora.UsuarioModificacionLog = Muestra.UsuarioCreacionLog;
                BuscarHora.FechaModificacionLog = DateTime.Now;
                BuscarHora.TerminalModificacionLog = Muestra.TerminalCreacionLog;
                BuscarHora.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                db.SaveChanges();
                resultado[0] = "111";
                resultado[1] = "Registro Inactivado con éxito";
                return resultado;

            }
        }
        public List<DETALLE_CONTROL_PESO_CODIFICACION> ConsultarHorasControl(int Cabecera_Control)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                
                return db.DETALLE_CONTROL_PESO_CODIFICACION.Where(x => x.IdCabeceraControlPesoCodificacion == Cabecera_Control && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                
            }
        }
        public List<DETALLE_HORAS_CONTROL_PESO_CODIFICACION> ConsultarMuestrasPorHora(int IdDetalleControlPesoCodificacion)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                return db.DETALLE_HORAS_CONTROL_PESO_CODIFICACION.Where(x =>x.IdDetalleControlPesoCodificacion== IdDetalleControlPesoCodificacion && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();

            }
        }
        public object[] GuardarUsoControl(DETALLE_USO_CONTROL_PESO_CODIFICACION UsoControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[2];
                var buscarHoraDet = db.DETALLE_USO_CONTROL_PESO_CODIFICACION.Where(z =>  z.Codigo == UsoControl.Codigo&&z.IdCabeceraControlPesoCodificacion==UsoControl.IdCabeceraControlPesoCodificacion
                  && z.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (buscarHoraDet == null)
                {
                    db.DETALLE_USO_CONTROL_PESO_CODIFICACION.Add(UsoControl);
                    db.SaveChanges();
                    resultado[0] = "111";
                    resultado[1] = "Registro ingresado con éxito";
                    return resultado;
                }
                else
                {
                    resultado[0] = "666";
                    resultado[1] = "Error, El control de uso ingresado ya existe";
                    return resultado;
                }
            }
        }
        public object[] ActualizarUsosControl(DETALLE_USO_CONTROL_PESO_CODIFICACION UsosControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[2];
                var BuscarUsoDetalle = db.DETALLE_USO_CONTROL_PESO_CODIFICACION.Find(UsosControl.IdDescripcionUso);
                BuscarUsoDetalle.UsuarioModificacionLog = UsosControl.UsuarioCreacionLog;
                BuscarUsoDetalle.FechaModificacionLog = DateTime.Now;
                BuscarUsoDetalle.TerminalModificacionLog = UsosControl.TerminalCreacionLog;
                BuscarUsoDetalle.Cantidad = UsosControl.Cantidad;
                db.SaveChanges();
                resultado[0] = "111";
                resultado[1] = "Registro actualizado con éxito";
                return resultado;

            }
        }
        public object[] InactivarUso(DETALLE_USO_CONTROL_PESO_CODIFICACION ControlUso)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[2];
                var BuscarUso = db.DETALLE_USO_CONTROL_PESO_CODIFICACION.Find(ControlUso.IdDescripcionUso);
                BuscarUso.UsuarioModificacionLog = ControlUso.UsuarioCreacionLog;
                BuscarUso.FechaModificacionLog = DateTime.Now;
                BuscarUso.TerminalModificacionLog = ControlUso.TerminalCreacionLog;
                BuscarUso.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                db.SaveChanges();
                resultado[0] = "111";
                resultado[1] = "Registro Inactivado con éxito";
                return resultado;

            }
        }
        public List<ControlUsosViewModel> ConsultarUsosControl(int Cabecera_Control)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                List<ControlUsosViewModel> resultado = (from u in db.DETALLE_USO_CONTROL_PESO_CODIFICACION
                                 join c in db.CLASIFICADOR on new { u.Codigo, Grupo = "032", EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { c.Codigo, c.Grupo, c.EstadoRegistro }
                                 where u.IdCabeceraControlPesoCodificacion==Cabecera_Control&&u.EstadoRegistro==clsAtributos.EstadoRegistroActivo
                                 select new ControlUsosViewModel { IdDescripcionUso = u.IdDescripcionUso, Codigo = u.Codigo, Descripcion = c.Descripcion, Cantidad = u.Cantidad }).ToList();
                return resultado;
            }
        }
        public object[] GuardarLoteControl(DETALLE_LOTE_CONTROL_PESO_CODIFICACION LoteControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[2];
                var buscarLoteDetalle = db.DETALLE_LOTE_CONTROL_PESO_CODIFICACION.Where(z => z.IdCabeceraControlPesoCodificacion == LoteControl.IdCabeceraControlPesoCodificacion
                && z.FechaOrdenFabricacion == LoteControl.FechaOrdenFabricacion&&z.OrdenFabricacion==LoteControl.OrdenFabricacion
                  && z.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (buscarLoteDetalle == null)
                {
                    db.DETALLE_LOTE_CONTROL_PESO_CODIFICACION.Add(LoteControl);
                    db.SaveChanges();
                    resultado[0] = "111";
                    resultado[1] = "Registro ingresado con éxito";
                    return resultado;
                }
                else
                {
                    resultado[0] = "666";
                    resultado[1] = "Error, El control de uso ingresado ya existe";
                    return resultado;
                }
            }
        }
        public object[] ActualizarLoteControl(DETALLE_LOTE_CONTROL_PESO_CODIFICACION LoteControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[2];
                var BuscarDetalleLote = db.DETALLE_LOTE_CONTROL_PESO_CODIFICACION.Find(LoteControl.IdDetalleLote);
                BuscarDetalleLote.UsuarioModificacionLog = LoteControl.UsuarioCreacionLog;
                BuscarDetalleLote.FechaModificacionLog = DateTime.Now;
                BuscarDetalleLote.TerminalModificacionLog = LoteControl.TerminalCreacionLog;
                BuscarDetalleLote.Lote = LoteControl.Lote;
                BuscarDetalleLote.Lomo = LoteControl.Lomo;
                BuscarDetalleLote.Miga = LoteControl.Miga;
                db.SaveChanges();
                resultado[0] = "1119";
                resultado[1] = "Registro actualizado con éxito";
                return resultado;

            }
        }
        public List<DETALLE_LOTE_CONTROL_PESO_CODIFICACION> ConsultarLotesControl(int Cabecera_Control)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                List<DETALLE_LOTE_CONTROL_PESO_CODIFICACION> resultado = (from u in db.DETALLE_LOTE_CONTROL_PESO_CODIFICACION
                                                        where u.IdCabeceraControlPesoCodificacion == Cabecera_Control && u.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                        select u).ToList();
                return resultado;
            }
        }
        public object[] InactivarLote(DETALLE_LOTE_CONTROL_PESO_CODIFICACION ControlLote)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[2];
                var BuscarDetLote = db.DETALLE_LOTE_CONTROL_PESO_CODIFICACION.Find(ControlLote.IdDetalleLote);
                BuscarDetLote.UsuarioModificacionLog = ControlLote.UsuarioCreacionLog;
                BuscarDetLote.FechaModificacionLog = DateTime.Now;
                BuscarDetLote.TerminalModificacionLog = ControlLote.TerminalCreacionLog;
                BuscarDetLote.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                db.SaveChanges();
                resultado[0] = "111";
                resultado[1] = "Registro Inactivado con éxito";
                return resultado;

            }
        }
        public object[] InactivarCabControl(CABECERA_CONTROL_PESO_CODIFICACION ControlLote)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[2];
                var BuscarCabControl = db.CABECERA_CONTROL_PESO_CODIFICACION.Find(ControlLote.IdCabeceraControlPesoYCodificacion);
                BuscarCabControl.UsuarioModificacionLog = ControlLote.UsuarioCreacionLog;
                BuscarCabControl.FechaModificacionLog = DateTime.Now;
                BuscarCabControl.TerminalModificacionLog = ControlLote.TerminalCreacionLog;
                BuscarCabControl.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                db.SaveChanges();
                resultado[0] = "111";
                resultado[1] = "Registro Inactivado con éxito";
                return resultado;

            }
        }
    }
}