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
               //     result.OrdenFabricacion = control.OrdenFabricacion;
               //     result.OrdenVenta = control.OrdenVenta;
               //     result.Fecha = control.Fecha;
                    result.Turno = control.Turno;
                    result.PesoEscurido = control.PesoEscurido;
                    result.PesoNeto = control.PesoNeto;
                    result.Lomo = control.Lomo;
                    result.Miga = control.Miga;
                    result.Tapa = control.Tapa;
                    result.Aceite = control.Aceite;
                    result.Agua = control.Agua;
                    result.Envase = control.Envase;
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


        public List<spConsultaControlConsumoInsumo> ConsultaControlConsumoInsumo(DateTime Fecha, string LineaNegocio, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaControlConsumoInsumo(Fecha, LineaNegocio, Turno).ToList();
                return lista;               
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

        public List<CONSUMO_DETALLE_POUCH> ConsultaConsumoDetallePouch(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CONSUMO_DETALLE_POUCH.Where(x => x.IdControlConsumoInsumos == IdControl && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                return lista;
            }
        }


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