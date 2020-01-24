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
                    result.PesoEscrundido = control.PesoEscrundido;
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
                    result.UnidadesProducidas = control.UnidadesProducidas;
                    result.UnidadesRecibidas = control.UnidadesRecibidas;
                    result.UnidadesSobrantes = control.UnidadesSobrantes;
                    //result.CodigoMaterial = control.CodigoMaterial;
                    result.CodigoProducto = control.CodigoProducto;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                  

                }
                else
                {
                    control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    control.FechaIngresoLog = DateTime.Now;

                    entities.CONTROL_CONSUMO_INSUMO.Add(control);
                 
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

        public CONTROL_CONSUMO_INSUMO ConsultaControlConsumoInsumo(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CONTROL_CONSUMO_INSUMO.FirstOrDefault(x=> x.IdControlConsumoInsumos==IdControl);
                return lista;
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

        public spGeneraDatosProcesoConsumoInsumo ConsultaDatosProceso(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spGeneraDatosProcesoConsumoInsumo(IdControl).FirstOrDefault();
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
        public List<spConsultaConsumoDetallePouch> ConsultaConsumoDetallePouch(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaConsumoDetallePouch(IdControl).ToList();
                return lista;
            }
        }

        #endregion


        #region CONSUMO DANIADO
        public List<spConsultaConsumoEnvasesFundas> ConsultaConsumoDaniado(int IdControl,string Tipo)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaConsumoEnvasesFundas(IdControl, Tipo).ToList();
                return lista;
            }
        }
        public RespuestaGeneral GuardarModificarConsumoDaniado(CONSUMO_DETALLE_DANIADO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONSUMO_DETALLE_DANIADO.FirstOrDefault(x => x.IdConsumoDetalleDaniado == control.IdConsumoDetalleDaniado || (x.IdControlConsumoInsumos==control.IdControlConsumoInsumos && x.Codigo==control.Codigo && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo));
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

        #region ADITIVOS
        public List<spConsultaConsumoDetalleAditivo> ConsultaConsumoDetalleAditivo(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
              //  var lista = entities.CONSUMO_DETALLE_ADITIVO.Where(x => x.IdControlConsumoInsumos == IdControl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                var lista = entities.spConsultaConsumoDetalleAditivo(IdControl).ToList();
                return lista;
            }
        }
        public RespuestaGeneral GuardarModificarAditivo(CONSUMO_DETALLE_ADITIVO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONSUMO_DETALLE_ADITIVO.FirstOrDefault(x => x.IdConsumoAditivo == control.IdConsumoAditivo);
                if (result != null)
                {
                    result.Aditivo = control.Aditivo;
                    result.Peso= control.Peso;
                    result.Lote = control.Lote;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.FechaIngresoLog = DateTime.Now;
                    entities.CONSUMO_DETALLE_ADITIVO.Add(control);

                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }
        public RespuestaGeneral EliminarAditivo(CONSUMO_DETALLE_ADITIVO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONSUMO_DETALLE_ADITIVO.FirstOrDefault(x => x.IdConsumoAditivo == control.IdConsumoAditivo);
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

        #region TIEMPOS MUERTOS
        public List<spConsultaConsumoTiempoMuerto> ConsultaConsumoDetalleTiempoMuerto(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaConsumoTiempoMuerto(IdControl).ToList();
                return lista;
            }
        }
        public RespuestaGeneral GuardarModificarTiempoMuerto(CONSUMO_TIEMPO_MUERTO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONSUMO_TIEMPO_MUERTO.FirstOrDefault(x => x.IdTiempoMuertos == control.IdTiempoMuertos);
                if (result != null)
                {
                    result.HoraPara = control.HoraPara;
                    result.HoraReinicio = control.HoraReinicio;
                    result.Observacion = control.Observacion;
                    result.Tipo = control.Tipo;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.FechaIngresoLog = DateTime.Now;
                    entities.CONSUMO_TIEMPO_MUERTO.Add(control);

                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }
        public RespuestaGeneral EliminarTiempoMuerto(CONSUMO_TIEMPO_MUERTO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONSUMO_TIEMPO_MUERTO.FirstOrDefault(x => x.IdTiempoMuertos == control.IdTiempoMuertos);
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


        #region PROCEDENCIA PESCADO
        public List<spConsultaConsumoProcedenciaPescado> ConsultaConsumoDetalleProcedencia(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaConsumoProcedenciaPescado(IdControl).ToList();
                return lista;
            }
        }
        public RespuestaGeneral GuardarModificarProcedencia(CONSUMO_PROCEDENCIA_PESCADO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONSUMO_PROCEDENCIA_PESCADO.FirstOrDefault(x => x.IdProcedenciaPescado == control.IdProcedenciaPescado);
                if (result != null)
                {
                    result.Lote = control.Lote;
                    result.Procedencia = control.Procedencia;
                    result.Observacion = control.Observacion;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
                }
                else
                {
                    control.FechaIngresoLog = DateTime.Now;
                    entities.CONSUMO_PROCEDENCIA_PESCADO.Add(control);

                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };
            }
        }
        public RespuestaGeneral EliminarProcedencia(CONSUMO_PROCEDENCIA_PESCADO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONSUMO_PROCEDENCIA_PESCADO.FirstOrDefault(x => x.IdProcedenciaPescado == control.IdProcedenciaPescado);
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
    }
}