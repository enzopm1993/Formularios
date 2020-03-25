using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.PRODUCCION.ProductoPouchCuarentena
{
    public class clsDProductoPouchCuarentena
    {
        public object[] GuardarCabeceraControl(CABECERA_PRODUCTO_POUCH_CUARENTENA poCabControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarCabecera = db.CABECERA_PRODUCTO_POUCH_CUARENTENA.Where(x => x.FechaProduccion == poCabControl.FechaProduccion && x.Turno == poCabControl.Turno
                  && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (buscarCabecera == null)
                {
                    db.CABECERA_PRODUCTO_POUCH_CUARENTENA.Add(poCabControl);
                    db.SaveChanges();
                    resultado[0] = "000";
                    resultado[1] = "Registro ingresado con éxito";
                    resultado[2] = poCabControl;
                }
                else
                {
                    resultado[0] = "002";
                    resultado[1] = "Error, el registro ya existe";
                    resultado[2] = poCabControl;
                }
                return resultado;
            }
        }
        public object[] GuardarDetalleControl(DETALLE_PRODUCTO_POUCH_CUARENTENA poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscardetalle = db.DETALLE_PRODUCTO_POUCH_CUARENTENA.Where(x => x.Pallet == poDetalleControl.Pallet && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (buscardetalle == null)
                {
                    db.DETALLE_PRODUCTO_POUCH_CUARENTENA.Add(poDetalleControl);
                    db.SaveChanges();
                    resultado[0] = "000";
                    resultado[1] = "Registro ingresado con éxito";
                    resultado[2] = poDetalleControl;
                }
                else
                {
                    resultado[0] = "002";
                    resultado[1] = "Error, el registro ya existe";
                    resultado[2] = poDetalleControl;
                }
                return resultado;
            }
        }
        public object[] GuardarSubDetalleControl(SUBDETALLE_PRODUCTO_POUCH_CUARENTENA poSubDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscardetalle = db.SUBDETALLE_PRODUCTO_POUCH_CUARENTENA.Where(x => x.IdDetalleProductoPouchCuarentena==poSubDetalleControl.IdDetalleProductoPouchCuarentena
                &&x.IdCocheAutoclave == poSubDetalleControl.IdCocheAutoclave
                && x.NCarro==poSubDetalleControl.NCarro&&x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (buscardetalle == null)
                {
                    db.SUBDETALLE_PRODUCTO_POUCH_CUARENTENA.Add(poSubDetalleControl);
                    db.SaveChanges();
                    resultado[0] = "000";
                    resultado[1] = "Registro ingresado con éxito";
                    resultado[2] = poSubDetalleControl;
                }
                else
                {
                    resultado[0] = "002";
                    resultado[1] = "Error, el registro ya existe";
                    resultado[2] = poSubDetalleControl;
                }
                return resultado;
            }
        }
        public object[] ActualizarSubDetalleControl(SUBDETALLE_PRODUCTO_POUCH_CUARENTENA poSubDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscardetalle = db.SUBDETALLE_PRODUCTO_POUCH_CUARENTENA.Find(poSubDetalleControl.IdSubDetalleProdPouchCuarentena);
                buscardetalle.FechaModificacionLog = poSubDetalleControl.FechaIngresoLog;
                buscardetalle.UsuarioModificacionLog = poSubDetalleControl.UsuarioIngresoLog;
                buscardetalle.TerminalModificacionLog = poSubDetalleControl.TerminalIngresoLog;
                buscardetalle.Funda = poSubDetalleControl.Funda;
                db.SaveChanges();
                resultado[0] = "001";
                resultado[1] = "Registro actualizado con éxito";
                resultado[2] = poSubDetalleControl;
                return resultado;
            }
        }
        public object[] ActualizarDetalleControl(DETALLE_PRODUCTO_POUCH_CUARENTENA poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscardetalle = db.DETALLE_PRODUCTO_POUCH_CUARENTENA.Find(poDetalleControl.IdDetalleProdPouchCuarentena);
                buscardetalle.FechaModificacionLog = poDetalleControl.FechaIngresoLog;
                buscardetalle.UsuarioModificacionLog = poDetalleControl.UsuarioIngresoLog;
                buscardetalle.TerminalModificacionLog = poDetalleControl.TerminalIngresoLog;
                buscardetalle.HoraInicio = poDetalleControl.HoraInicio;
                buscardetalle.HoraFin = poDetalleControl.HoraFin;
                db.SaveChanges();
                resultado[0] = "001";
                resultado[1] = "Registro actualizado con éxito";
                resultado[2] = poDetalleControl;
                return resultado;
            }
        }
        public List<DETALLE_PRODUCTO_POUCH_CUARENTENA> ConsultarDetalleControl(int piIdCabecera)
        {
            using (var db = new ASIS_PRODEntities())
            {
                var resultado = db.DETALLE_PRODUCTO_POUCH_CUARENTENA.Where(x => x.IdCabProdPouchCuarentena == piIdCabecera && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                return resultado;
            }
        }
        public List<SUBDETALLE_PRODUCTO_POUCH_CUARENTENA> ConsultarSubDetalleControl(int piIdDetalle)
        {
            using (var db = new ASIS_PRODEntities())
            {
                var resultado = db.SUBDETALLE_PRODUCTO_POUCH_CUARENTENA.Where(x => x.IdDetalleProductoPouchCuarentena == piIdDetalle && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                return resultado;
            }
        }
        public object[] ActualizarCabeceraControl(CABECERA_PRODUCTO_POUCH_CUARENTENA poCabControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarCabecera = db.CABECERA_PRODUCTO_POUCH_CUARENTENA.Find(poCabControl.IdCabProdPouchCuarentena);
                buscarCabecera.FechaModificacionLog = poCabControl.FechaIngresoLog;
                buscarCabecera.UsuarioModificacionLog = poCabControl.UsuarioIngresoLog;
                buscarCabecera.TerminalModificacionLog = poCabControl.TerminalIngresoLog;

                buscarCabecera.OrdenFabricacion = poCabControl.OrdenFabricacion;
                buscarCabecera.Producto = poCabControl.Producto;
                buscarCabecera.CodigoProducto = poCabControl.CodigoProducto;
                buscarCabecera.FechaTerminado = poCabControl.FechaTerminado;
                buscarCabecera.TamanoFunda = poCabControl.TamanoFunda;
                buscarCabecera.Codigo = poCabControl.Codigo;
                buscarCabecera.PedidoVenta = poCabControl.PedidoVenta;
                buscarCabecera.Cliente = poCabControl.Cliente;
                buscarCabecera.TotalCajas = poCabControl.TotalCajas;

                db.SaveChanges();
                resultado[0] = "001";
                resultado[1] = "Registro actualizado con éxito";
                resultado[2] = poCabControl;
                return resultado;
            }
        }
        public CABECERA_PRODUCTO_POUCH_CUARENTENA ConsultarCabeceraControl(CABECERA_PRODUCTO_POUCH_CUARENTENA poCaabeceraControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                return db.CABECERA_PRODUCTO_POUCH_CUARENTENA.Where(x => x.FechaProduccion == poCaabeceraControl.FechaProduccion && x.Turno == poCaabeceraControl.Turno
                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
            }
        }
        public object[] InactivarSubDetalle(SUBDETALLE_PRODUCTO_POUCH_CUARENTENA poSubDetalle)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarSubDetalle = db.SUBDETALLE_PRODUCTO_POUCH_CUARENTENA.FirstOrDefault(x => x.IdSubDetalleProdPouchCuarentena == poSubDetalle.IdSubDetalleProdPouchCuarentena);
                buscarSubDetalle.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                buscarSubDetalle.FechaModificacionLog = poSubDetalle.FechaIngresoLog;
                buscarSubDetalle.UsuarioModificacionLog = poSubDetalle.UsuarioIngresoLog;
                buscarSubDetalle.TerminalModificacionLog = poSubDetalle.TerminalIngresoLog;
                db.SaveChanges();
                resultado[0] = "002";
                resultado[1] = "Registro Inactivado con éxito";
                resultado[2] = poSubDetalle;
                return resultado;
            }
        }
        public object[] InactivarDetalle(DETALLE_PRODUCTO_POUCH_CUARENTENA poDetalle)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarSubDetalle = db.DETALLE_PRODUCTO_POUCH_CUARENTENA.FirstOrDefault(x => x.IdDetalleProdPouchCuarentena == poDetalle.IdDetalleProdPouchCuarentena);
                buscarSubDetalle.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                buscarSubDetalle.FechaModificacionLog = poDetalle.FechaIngresoLog;
                buscarSubDetalle.UsuarioModificacionLog = poDetalle.UsuarioIngresoLog;
                buscarSubDetalle.TerminalModificacionLog = poDetalle.TerminalIngresoLog;
                db.SaveChanges();
                resultado[0] = "002";
                resultado[1] = "Registro Inactivado con éxito";
                resultado[2] = poDetalle;
                return resultado;
            }
        }
        public object[] InactivarCabecera(CABECERA_PRODUCTO_POUCH_CUARENTENA poCabecera)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarCabecera = db.CABECERA_PRODUCTO_POUCH_CUARENTENA.FirstOrDefault(x => x.IdCabProdPouchCuarentena == poCabecera.IdCabProdPouchCuarentena);
                buscarCabecera.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                buscarCabecera.FechaModificacionLog = poCabecera.FechaIngresoLog;
                buscarCabecera.UsuarioModificacionLog = poCabecera.UsuarioIngresoLog;
                buscarCabecera.TerminalModificacionLog = poCabecera.TerminalIngresoLog;
                db.SaveChanges();
                resultado[0] = "002";
                resultado[1] = "Registro Inactivado con éxito";
                resultado[2] = poCabecera;
                return resultado;
            }
        }
    }
}