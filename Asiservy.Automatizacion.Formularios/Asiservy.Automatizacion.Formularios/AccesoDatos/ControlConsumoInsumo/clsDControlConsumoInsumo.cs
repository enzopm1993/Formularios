using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ControlConsumoInsumo
{
    public class clsDControlConsumoInsumo
    {
        // clsDApiOrdenFabricacion clsDApiOrdenFabricacion = null;

        #region CONTROL CONSUMO INSUMOS
        public RespuestaGeneral GuardarModificarControl(CONTROL_CONSUMO_INSUMO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONTROL_CONSUMO_INSUMO.FirstOrDefault(x => x.IdControlConsumoInsumos == control.IdControlConsumoInsumos);
                if (result != null)
                {
                    result.HoraInicio = control.HoraInicio;
                    result.HoraFin = control.HoraFin;
                    result.Observacion = control.Observacion;              
                    result.Turno = control.Turno;
                    result.PesoEscurido = control.PesoEscurido;
                    result.PesoNeto = control.PesoNeto;
                    result.Lomo = control.Lomo;
                    result.Miga = control.Miga;
                    result.Tapa = control.Tapa;
                    result.Aceite = control.Aceite;
                    result.Agua = control.Agua;
                    result.CaldoVegetal = control.CaldoVegetal;               
                    result.DesperdicioAceite = control.DesperdicioAceite;
                    result.DesperdicioLiquido = control.DesperdicioLiquido;
                    result.DesperdicioSolido = control.DesperdicioSolido;
                    result.Empleados = control.Empleados;
                    result.Cajas = control.Cajas;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                  

                }
                else
                {
                    result.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    result.FechaIngresoLog = DateTime.Now;

                    entities.CONTROL_CONSUMO_INSUMO.Add(result);
                 
                }

                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };


            }
        }

        public RespuestaGeneral EliminarControlConsumoInsumo(CONTROL_CONSUMO_INSUMO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONTROL_CONSUMO_INSUMO.FirstOrDefault(x => x.IdControlConsumoInsumos == control.IdControlConsumoInsumos);
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


        public List<spConsultaControlConsumoInsumo> ConsultaControlConsumoInsumo(DateTime Fecha, string LineaNegocio, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaControlConsumoInsumo(Fecha, LineaNegocio, Turno).ToList();
                return lista;               
            }
        }
        #endregion


        #region CONTROL DETALLE ENLATADO
        public RespuestaGeneral GuardarModificarDetalleEnlatado(CONSUMO_DETALLE_LATA control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONSUMO_DETALLE_LATA.FirstOrDefault(x => x.IdProcesoDetalleLata == control.IdProcesoDetalleLata);
                if (result != null)
                {
                    result.Lotes = control.Lotes;
                    result.Pallet = control.Pallet;
                    result.Bultos = control.Bultos;
                    result.FechaFabricacion = control.FechaFabricacion;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.FechaIngresoLog = DateTime.Now;
                    entities.CONSUMO_DETALLE_LATA.Add(control);

                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }
        public RespuestaGeneral EliminarDetalleEnlatado(CONSUMO_DETALLE_LATA control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONSUMO_DETALLE_LATA.FirstOrDefault(x => x.IdProcesoDetalleLata == control.IdProcesoDetalleLata);
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
        public List<spConsultaConsumoDetalleLata> ConsultaConsumoDetalleLata(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaConsumoDetalleLata(IdControl).ToList();
                return lista;
            }
        }
        #endregion


        #region CONSUMO DETALLE POUCH

        public RespuestaGeneral GuardarModificarDetallePouch(CONSUMO_DETALLE_POUCH control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONSUMO_DETALLE_POUCH.FirstOrDefault(x => x.IdProcesoDetallePouch == control.IdProcesoDetallePouch);
                if (result != null)
                {
                    result.Lotes = control.Lotes;
                    result.Cajas = control.Cajas;
                    result.FechaFabricacion = control.FechaFabricacion;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.FechaIngresoLog = DateTime.Now;
                    entities.CONSUMO_DETALLE_POUCH.Add(result);

                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }
        public RespuestaGeneral EliminarDetallePouch(CONSUMO_DETALLE_POUCH control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONSUMO_DETALLE_POUCH.FirstOrDefault(x => x.IdProcesoDetallePouch == control.IdProcesoDetallePouch);
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
        public List<CONSUMO_DETALLE_POUCH> ConsultaConsumoDetallePouch(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CONSUMO_DETALLE_POUCH.Where(x => x.IdControlConsumoInsumos == IdControl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                return lista;
            }
        }

        #endregion


        #region CONSUMO DANIADO
        public List<CONSUMO_DETALLE_DANIADO> ConsultaConsumoDaniado(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CONSUMO_DETALLE_DANIADO.Where(x => x.IdControlConsumoInsumos == IdControl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                return lista;
            }
        }
        public RespuestaGeneral GuardarModificarConsumoDaniado(CONSUMO_DETALLE_DANIADO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONSUMO_DETALLE_DANIADO.FirstOrDefault(x => x.IdConsumoDetalleDaniado == control.IdConsumoDetalleDaniado);
                if (result != null)
                {
                    result.Latas = control.Latas;
                    result.Tapas = control.Tapas;
                    result.Fundas = control.Fundas;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.FechaIngresoLog = DateTime.Now;
                    entities.CONSUMO_DETALLE_DANIADO.Add(control);

                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }
        public RespuestaGeneral EliminarConsumoDaniado(CONSUMO_DETALLE_DANIADO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONSUMO_DETALLE_DANIADO.FirstOrDefault(x => x.IdConsumoDetalleDaniado == control.IdConsumoDetalleDaniado);
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
        public List<CONSUMO_DETALLE_ADITIVO> ConsultaConsumoDetalleAditivo(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CONSUMO_DETALLE_ADITIVO.Where(x => x.IdControlConsumoInsumos == IdControl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                return lista;
            }
        }

        public List<CONSUMO_TIEMPO_MUERTO> ConsultaConsumoDetalleTiempoMuerto(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CONSUMO_TIEMPO_MUERTO.Where(x => x.IdControlConsumoInsumos == IdControl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                return lista;
            }
        }

    }
}