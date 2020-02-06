using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.AccesoDatos.General;
using Asiservy.Automatizacion.Formularios.Models;
using Asiservy.Automatizacion.Formularios.Models.ControlConsumoInsumos;
using Asiservy.Automatizacion.Formularios.Models.MantenimientoPallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                    //result.HoraInicio = control.HoraInicio;
                    //result.HoraFin = control.HoraFin;
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
                    result.GrsLataReal = control.GrsLataReal;
                    result.CodigoProducto = control.CodigoProducto;
                    result.UnidadesProducidasTapa = control.UnidadesProducidasTapa;
                    result.UnidadesRecibidasTapa = control.UnidadesRecibidasTapa;
                    result.UnidadesSobrantesTapa = control.UnidadesSobrantesTapa;
                   // result.gr = control.CodigoProducto;
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

        public RespuestaGeneral GuardarModificarControlSaldo(CONTROL_CONSUMO_INSUMO control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONTROL_CONSUMO_INSUMO.FirstOrDefault(x => x.IdControlConsumoInsumos == control.IdControlConsumoInsumos);
                if (result != null)
                {
                    result.SaldoInicialLamina = control.SaldoInicialLamina;
                    result.SaldoInicialUnidad= control.SaldoInicialUnidad;
                    result.SaldoFinalLamina = control.SaldoFinalLamina;
                    result.SaldoFinalUnidad = control.SaldoFinalUnidad;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;
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
        //public List<spConsultaControlConsumoInsumoDetalle> ConsultaConsumoDetalle(int IdControl)
        //{
        //    using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
        //    {
        //        var lista = entities.spConsultaControlConsumoInsumoDetalle(IdControl).ToList();
        //        return lista;
        //    }
        //}
        #endregion

        #region CONTROL CONSUMO INSUMOS DETALLE
        public RespuestaGeneral GuardarModificarControlDetalle(CONTROL_CONSUMO_INSUMO_DETALLE control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONTROL_CONSUMO_INSUMO_DETALLE.FirstOrDefault(x => x.IdControlConsumoInsumoDetalle == control.IdControlConsumoInsumoDetalle);
                if (result != null)
                {
                    result.HoraInicio = control.HoraInicio;
                    result.HoraFin = control.HoraFin;
                    result.UsuarioModificacionLog = control.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = control.TerminalIngresoLog;


                }
                else
                {
                    control.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                    control.FechaIngresoLog = DateTime.Now;
                    entities.CONTROL_CONSUMO_INSUMO_DETALLE.Add(control);
                }
                entities.SaveChanges();
                return new RespuestaGeneral { Mensaje = clsAtributos.MsjRegistroGuardado, Respuesta = true };


            }
        }

        public RespuestaGeneral EliminarControlConsumoInsumoDetalle(CONTROL_CONSUMO_INSUMO_DETALLE control)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONTROL_CONSUMO_INSUMO_DETALLE.FirstOrDefault(x => x.IdControlConsumoInsumoDetalle == control.IdControlConsumoInsumoDetalle);
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

      
        public List<spConsultaControlConsumoInsumoDetalle> ConsultaControlConsumoInsumoDetalle(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaControlConsumoInsumoDetalle(IdControl).ToList();
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
                    result.PalletProveedor = control.PalletProveedor;
                    result.PalletProveedorTapa = control.PalletProveedorTapa;
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
                    entities.CONSUMO_DETALLE_POUCH.Add(control);

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

        #region Mantenimiento Pallet
        public string GuardarPallet(PALLET pallet)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (pallet.IdPallet == 0)
                {
                    var BuscarPallet = db.PALLET.Where(x => x.Proveedor == pallet.Proveedor && x.Envase == pallet.Envase).FirstOrDefault();
                    if (BuscarPallet != null)
                    {
                        return "El pallet ingresado ya existe";
                    }
                    else
                    {
                        db.PALLET.Add(pallet);
                        db.SaveChanges();
                        return "Registro ingresado con éxito";
                    }
                }
                else
                {
                    
                    var modificarpallet=db.PALLET.Find(pallet.IdPallet);
                    modificarpallet.Numero_Pallet = pallet.Numero_Pallet;
                    modificarpallet.Lamina = pallet.Lamina;
                    modificarpallet.Unidades = pallet.Unidades;
                    modificarpallet.FechaModificacionLog = pallet.FechaIngresoLog;
                    modificarpallet.UsuarioModificacionLog = pallet.UsuarioModificacionLog;
                    modificarpallet.TerminalModificacionLog = pallet.TerminalModificacionLog;
                    db.SaveChanges();
                    return "Registro actualizado con éxito";
                }
                
            }
        }
        public List<PALLET> ConsultarPallets()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.PALLET.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }
        public List<SelectListItem> ConsultarPalletsCombo()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
               var lstpallets= db.PALLET.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                List<SelectListItem> SliPallet = new List<SelectListItem>();
                clsDClasificador clsDClasificador = new clsDClasificador();
                List<CLASIFICADOR> ListProveedores = clsDClasificador.ConsultarClasificador(clsAtributos.CodigoGrupoProveedorPallet);
                List<PalletViewModel> ListPallets = (from p in lstpallets
                                                     join pr in ListProveedores on p.Proveedor equals pr.Codigo
                                                     select new PalletViewModel
                                                     {
                                                         IdPallet = p.IdPallet,
                                                         Envase = p.Envase,
                                                         EstadoRegistro = p.EstadoRegistro,
                                                         FechaIngresoLog = p.FechaIngresoLog,
                                                         FechaModificacionLog = p.FechaModificacionLog,
                                                         IdProveedor = p.Proveedor,
                                                         Lamina = p.Lamina,
                                                         Numero_Pallet = p.Numero_Pallet,
                                                         Proveedor = pr.Descripcion,
                                                         TerminalIngresoLog = p.TerminalIngresoLog,
                                                         TerminalModificacionLog = p.TerminalModificacionLog,
                                                         Unidades = p.Unidades,
                                                         UsuarioIngresoLog = p.UsuarioIngresoLog,
                                                         UsuarioModificacionLog = p.UsuarioModificacionLog
                                                     }).ToList();
                foreach (var item in ListPallets)
                {
                    SliPallet.Add(new SelectListItem { Value = item.IdPallet.ToString(), Text = item.Proveedor+"-"+item.Envase });
                }
                return SliPallet;
            }
        }
        #endregion
        #region 
        public ReporteEnvaseEnlatadoViewModel ConsultaReporteControlEnvEnlatado(int IdCabeceraControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                ReporteEnvaseEnlatadoViewModel Resultado = new ReporteEnvaseEnlatadoViewModel();
                Resultado.CabeceraControl = db.CONTROL_CONSUMO_INSUMO.Find(IdCabeceraControl);

                Resultado.DetalleCuerpo = (from c in db.CONSUMO_DETALLE_LATA
                                           join cl in db.PALLET on c.PalletProveedor equals cl.IdPallet
                                           join clasificador in db.CLASIFICADOR on new {Codigo=cl.Proveedor, Grupo="028" } equals new {clasificador.Codigo, clasificador.Grupo }
                                           where c.EstadoRegistro==clsAtributos.EstadoRegistroActivo && c.IdControlConsumoInsumos==IdCabeceraControl
                                           select new DetalleCuerpo
                                           {
                                               Proveedor = clasificador.Descripcion + "-" + cl.Envase,
                                               Bulto = c.Bultos,
                                               Fecha = c.FechaFabricacion,
                                               Linea = "",
                                               Pallet=c.Pallet,
                                               Lote=c.Lotes,
                                               PalletProveedor=c.PalletProveedor
                                           }
                                         ).ToList();
                Resultado.DetalleMermas = (from c in db.CLASIFICADOR
                                           join d in db.CONSUMO_DETALLE_DANIADO on new { codigo = c.Codigo, IdControlConsumoInsumos=IdCabeceraControl,estado=c.EstadoRegistro } equals new { codigo = d.Codigo, IdControlConsumoInsumos = d.IdControlConsumoInsumos, estado=clsAtributos.EstadoRegistroActivo } into mermas
                                           from m in mermas.DefaultIfEmpty()
                                           where c.Codigo != "0" && c.Grupo=="024" &&c.EstadoRegistro==clsAtributos.EstadoRegistroActivo
                                           select new DetalleMermasViewModel { Merma = c.Descripcion, Cuerpo = m.Latas, Tapa = m.Tapas }).ToList();

                int? IdPalletProveedor = (from d in Resultado.DetalleCuerpo
                                         where d.Pallet == 0
                                         select d.PalletProveedor).FirstOrDefault();
                Resultado.ToTalUnidadesSaldoInicial = Resultado.CabeceraControl.SaldoInicialLamina>0?(from p in db.PALLET
                                                       where p.IdPallet == IdPalletProveedor
                                                       select p.Unidades).FirstOrDefault()*Resultado.CabeceraControl.SaldoInicialLamina: Resultado.CabeceraControl.SaldoInicialLamina;

                IdPalletProveedor = (from d in Resultado.DetalleCuerpo
                                          select d.PalletProveedor).LastOrDefault();
                Resultado.TotalUnidadesSaldoFinal = Resultado.CabeceraControl.SaldoFinalLamina > 0 ? (from p in db.PALLET
                                                                                                          where p.IdPallet == IdPalletProveedor
                                                                                                          select p.Unidades).FirstOrDefault() * Resultado.CabeceraControl.SaldoInicialLamina : Resultado.CabeceraControl.SaldoInicialLamina;
                return Resultado;
            }
        }
        #endregion
    }
}