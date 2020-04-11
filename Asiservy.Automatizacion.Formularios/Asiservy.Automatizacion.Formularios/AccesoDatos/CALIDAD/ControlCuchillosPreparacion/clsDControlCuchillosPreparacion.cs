using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ControlCuchillosPreparacion
{
    public class clsDControlCuchillosPreparacion
    {
        public List<sp_Consultar_Cuchillos_Preparacion> ConsultarCuchilloPreparacion(string CodigoCuchillo, int opcion)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.sp_Consultar_Cuchillos_Preparacion(CodigoCuchillo, opcion).ToList();
                return listado;
            }
        }

        public void GuardarModificarCuchilloPreparacion(CC_CUCHILLOS_PREPARACION GuardarModigicar)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CUCHILLOS_PREPARACION.FirstOrDefault(x => x.IdCuchilloPreparacion == GuardarModigicar.IdCuchilloPreparacion || x.CodigoCuchillo == GuardarModigicar.CodigoCuchillo);
                if (model != null)
                {
                    model.DescripcionCuchillo = GuardarModigicar.DescripcionCuchillo;
                    //model.CodigoCuchillo = GuardarModigicar.CodigoCuchillo;
                    model.EstadoRegistro = GuardarModigicar.EstadoRegistro;
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
                }
                else
                {
                    db.CC_CUCHILLOS_PREPARACION.Add(GuardarModigicar);
                }
                db.SaveChanges();
            }
        }

        //-----------------------------------------CONTROL CUCHILLO----------------------------------------------------------------------------
        public List<sp_Control_Cuchillos_Preparacion> ConsultarControlCuchilloPreparacion(DateTime fecha, int IdControlCuchillo, int opcion)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.sp_Control_Cuchillos_Preparacion(fecha,null, IdControlCuchillo, opcion).ToList();
                return listado;
            }
        }

        public int GuardarModificarControlCuchilloPreparacion(CC_CONTROL_CUCHILLOS_PREPARACION GuardarModigicar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CONTROL_CUCHILLOS_PREPARACION.FirstOrDefault(x => x.IdControlCuchillo == GuardarModigicar.IdControlCuchillo && x.EstadoRegistro == GuardarModigicar.EstadoRegistro);
                if (model != null)
                {
                    model.Observacion = GuardarModigicar.Observacion;
                    model.Hora = GuardarModigicar.Hora;
                    model.EstadoRegistro = GuardarModigicar.EstadoRegistro;
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_CONTROL_CUCHILLOS_PREPARACION.Add(GuardarModigicar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarControlCuchilloPreparacion(CC_CONTROL_CUCHILLOS_PREPARACION Eliminar)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CONTROL_CUCHILLOS_PREPARACION.FirstOrDefault(x => x.IdControlCuchillo == Eliminar.IdControlCuchillo);
                if (model != null)
                {
                    model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    model.FechaModificacionLog = Eliminar.FechaModificacionLog;                    
                    model.TerminalModificacionLog = Eliminar.TerminalModificacionLog;
                    model.UsuarioModificacionLog = Eliminar.UsuarioModificacionLog;
                    db.SaveChanges();
                    return 1;
                }
                return 0;
            }
        }

        //--------------------------------------CONTROL CUCHILLO DETALLE--------------------------------------------------------------------------
        public List<sp_Control_Cuchillos_Preparacion_Detalle> ConsultarControlCuchilloDetalle(int idCuchilloPreparacion, int idControlCuchillo, int idControlCuchilloDetalle, int opcion)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.sp_Control_Cuchillos_Preparacion_Detalle(idCuchilloPreparacion, idControlCuchilloDetalle, idControlCuchillo,  opcion).ToList();
                return listado;
            }
        }

        public int GuardarModificarControlCuchilloDetalle(CC_CONTROL_CUCHILLOS_PREPARACION_DETALLE GuardarModigicar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CONTROL_CUCHILLOS_PREPARACION_DETALLE.FirstOrDefault(x => x.IdControlCuchilloDetalle == GuardarModigicar.IdControlCuchilloDetalle && x.EstadoRegistro == GuardarModigicar.EstadoRegistro);
                if (model != null)
                {
                    model.Estado = GuardarModigicar.Estado;
                    model.EstadoRegistro = GuardarModigicar.EstadoRegistro;
                    model.IdCuchilloPreparacion = GuardarModigicar.IdCuchilloPreparacion;
                    model.CedulaEmpleado = GuardarModigicar.CedulaEmpleado;
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_CONTROL_CUCHILLOS_PREPARACION_DETALLE.Add(GuardarModigicar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarControlCuchilloDetalle(CC_CONTROL_CUCHILLOS_PREPARACION_DETALLE Eliminar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CONTROL_CUCHILLOS_PREPARACION_DETALLE.FirstOrDefault(x => x.IdControlCuchilloDetalle == Eliminar.IdControlCuchilloDetalle);
                if (model != null)
                {
                    model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    model.FechaModificacionLog = Eliminar.FechaModificacionLog;
                    model.TerminalModificacionLog = Eliminar.TerminalModificacionLog;
                    model.UsuarioModificacionLog = Eliminar.UsuarioModificacionLog;
                    db.SaveChanges();
                    valor=1;
                }
                return valor;
            }
        }

        //--------------------------------------CONTROL CUCHILLO DETALLE--------------------------------------------------------------
        public List<sp_Reporte_Control_Cuchillos_Preparacion> ReporteControlCuchilloPreparacion(DateTime filtroFechaDesde, DateTime filtroFechaHasta, int opcion)
        {
            using (ASIS_PRODEntities db= new ASIS_PRODEntities())
            {
                var listado = db.sp_Reporte_Control_Cuchillos_Preparacion(filtroFechaDesde, filtroFechaHasta, opcion).ToList();
                return listado;
            }
        }
    }
}