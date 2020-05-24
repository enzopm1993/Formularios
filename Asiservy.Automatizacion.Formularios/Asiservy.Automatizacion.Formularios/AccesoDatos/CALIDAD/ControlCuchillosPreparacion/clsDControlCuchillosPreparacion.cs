using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ControlCuchillosPreparacion
{
    public class clsDControlCuchillosPreparacion
    {
        public List<CC_CUCHILLOS_PREPARACION> ConsultarCuchilloPreparacion(string codigoCuchillo, int op)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = (from c in db.CC_CUCHILLOS_PREPARACION                            
                             orderby c.IdCuchilloPreparacion descending
                             select new {c.IdCuchilloPreparacion, c.CodigoCuchillo, c.DescripcionCuchillo, c.EstadoRegistro, c.FechaIngresoLog,
                             c.FechaModificacionLog, c.TerminalIngresoLog, c.TerminalModificacionLog, c.UsuarioIngresoLog, c.UsuarioModificacionLog}).ToList();
                if (op==1)
                {
                    lista = (from c in db.CC_CUCHILLOS_PREPARACION
                             where(c.CodigoCuchillo== codigoCuchillo)
                                 orderby c.IdCuchilloPreparacion descending
                                 select new
                                 {
                                     c.IdCuchilloPreparacion,
                                     c.CodigoCuchillo,
                                     c.DescripcionCuchillo,
                                     c.EstadoRegistro,
                                     c.FechaIngresoLog,
                                     c.FechaModificacionLog,
                                     c.TerminalIngresoLog,
                                     c.TerminalModificacionLog,
                                     c.UsuarioIngresoLog,
                                     c.UsuarioModificacionLog
                                 }).Take(1).ToList();
                }
                List<CC_CUCHILLOS_PREPARACION> listacabecera = new List<CC_CUCHILLOS_PREPARACION>();
                CC_CUCHILLOS_PREPARACION cabecera;
                foreach (var item in lista)
                {
                    cabecera = new CC_CUCHILLOS_PREPARACION();
                    cabecera.IdCuchilloPreparacion = item.IdCuchilloPreparacion;
                    cabecera.CodigoCuchillo = item.CodigoCuchillo;
                    cabecera.DescripcionCuchillo = item.DescripcionCuchillo;
                    cabecera.EstadoRegistro = item.EstadoRegistro;
                    cabecera.FechaIngresoLog = item.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    cabecera.FechaModificacionLog = item.FechaModificacionLog;
                    cabecera.UsuarioModificacionLog = item.UsuarioModificacionLog;
                    listacabecera.Add(cabecera);
                }
                return listacabecera;
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
       public int GuardarModificarControlCuchilloPreparacion(CC_CUCHILLOS_PREPARACION_CTRL GuardarModigicar, bool siAprobar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CUCHILLOS_PREPARACION_CTRL.FirstOrDefault(x => x.IdControlCuchillo == GuardarModigicar.IdControlCuchillo && x.EstadoRegistro == GuardarModigicar.EstadoRegistro);
                if (model != null)
                {
                    if (siAprobar)
                    {
                        model.AprobadoPor = GuardarModigicar.UsuarioIngresoLog;
                        model.FechaAprobado = GuardarModigicar.FechaAprobado;
                        model.EstadoReporte = GuardarModigicar.EstadoReporte;
                        valor = 2;
                    }
                    else
                    {
                        model.Observacion = GuardarModigicar.Observacion;                      
                        valor = 1;
                    }
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
                }
                else
                {
                    db.CC_CUCHILLOS_PREPARACION_CTRL.Add(GuardarModigicar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarControlCuchilloPreparacion(CC_CUCHILLOS_PREPARACION_CTRL Eliminar)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CUCHILLOS_PREPARACION_CTRL.FirstOrDefault(x => x.IdControlCuchillo == Eliminar.IdControlCuchillo);
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

        //--------------------------------------HORA--------------------------------------------------------------------------
        public int GuardarModificarHora(CC_CUCHILLOS_PREPARACION_HORA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CUCHILLOS_PREPARACION_HORA.FirstOrDefault(x => x.IdHora == guardarModificar.IdHora && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.Descripcion = guardarModificar.Descripcion;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_CUCHILLOS_PREPARACION_HORA.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarHora(CC_CUCHILLOS_PREPARACION_HORA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CUCHILLOS_PREPARACION_HORA.FirstOrDefault(x => x.IdHora == guardarModificar.IdHora && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.EstadoRegistro = guardarModificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_CUCHILLOS_PREPARACION_HORA.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public List<CC_CUCHILLOS_PREPARACION_HORA> ConsultarHora(int idControlCuchillo)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = (from c in db.CC_CUCHILLOS_PREPARACION_HORA
                             where (c.IdControlCuchillo == idControlCuchillo && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                             orderby c.Hora descending
                             select new { c.IdControlCuchillo,c.IdHora, c.Hora, c.EstadoRegistro, c.Descripcion, c.FechaIngresoLog, c.UsuarioIngresoLog}).ToList();
                List<CC_CUCHILLOS_PREPARACION_HORA> listacabecera = new List<CC_CUCHILLOS_PREPARACION_HORA>();
                CC_CUCHILLOS_PREPARACION_HORA cabecera;
                foreach (var item in lista)
                {
                    cabecera = new CC_CUCHILLOS_PREPARACION_HORA();
                    cabecera.IdHora = item.IdHora;
                    cabecera.IdControlCuchillo = item.IdControlCuchillo;
                    cabecera.Hora = item.Hora;
                    cabecera.Descripcion = item.Descripcion;
                    cabecera.FechaIngresoLog = item.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    cabecera.FechaModificacionLog = item.FechaIngresoLog;
                    cabecera.UsuarioModificacionLog = item.UsuarioIngresoLog;
                    listacabecera.Add(cabecera);
                }
                return listacabecera;
            }
        }

        //--------------------------------------CONTROL CUCHILLO DETALLE--------------------------------------------------------------------------
        public int GuardarModificarControlCuchilloDetalle(CC_CUCHILLOS_PREPARACION_CTRL_DET GuardarModigicar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CUCHILLOS_PREPARACION_CTRL_DET.FirstOrDefault(x => x.IdControlCuchilloDetalle == GuardarModigicar.IdControlCuchilloDetalle && x.EstadoRegistro == GuardarModigicar.EstadoRegistro);
                if (model != null)
                {
                    model.Estado = GuardarModigicar.Estado;
                    model.IdCuchilloPreparacion = GuardarModigicar.IdCuchilloPreparacion;
                    model.CedulaEmpleado = GuardarModigicar.CedulaEmpleado;
                    model.Observacion = GuardarModigicar.Observacion;
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_CUCHILLOS_PREPARACION_CTRL_DET.Add(GuardarModigicar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarControlCuchilloDetalle(CC_CUCHILLOS_PREPARACION_CTRL_DET Eliminar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_CUCHILLOS_PREPARACION_CTRL_DET.FirstOrDefault(x => x.IdControlCuchilloDetalle == Eliminar.IdControlCuchilloDetalle);
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
        public List<sp_Cuchillos_Preparacion_Det> ConsultarDetalle(int idHora, int op)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.sp_Cuchillos_Preparacion_Det(idHora, op).ToList();
                return listado;
            }
        }

        public List<sp_Control_Cuchillos_Preparacion> ConsultarBandeja(int idControlCuchillo, int op)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.sp_Control_Cuchillos_Preparacion(idControlCuchillo, op).ToList();
                return listado;
            }
        }

        public List<CC_CUCHILLOS_PREPARACION_CTRL> ConsultarcabeceraFechaID(DateTime fechaDesde, DateTime fechaHasta, bool estado, int op=0)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = (from c in db.CC_CUCHILLOS_PREPARACION_CTRL
                             where (c.Fecha >= fechaDesde && c.Fecha <= fechaHasta && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo && c.EstadoReporte==estado)
                             orderby c.Fecha descending
                             select new { c.IdControlCuchillo, c.Fecha, c.EstadoReporte, c.Observacion, c.FechaIngresoLog, c.UsuarioIngresoLog, c.FechaAprobado, c.AprobadoPor }).ToList();
                if (op==1)
                {
                    lista = (from c in db.CC_CUCHILLOS_PREPARACION_CTRL
                                 where (c.Fecha >= fechaDesde && c.Fecha <= fechaHasta && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                                 orderby c.Fecha descending
                                 select new { c.IdControlCuchillo, c.Fecha, c.EstadoReporte, c.Observacion, c.FechaIngresoLog, c.UsuarioIngresoLog, c.FechaAprobado, c.AprobadoPor }).ToList();
                }
                List<CC_CUCHILLOS_PREPARACION_CTRL> listacabecera = new List<CC_CUCHILLOS_PREPARACION_CTRL>();
                CC_CUCHILLOS_PREPARACION_CTRL cabecera;
                foreach (var item in lista)
                {
                    cabecera = new CC_CUCHILLOS_PREPARACION_CTRL();
                    cabecera.IdControlCuchillo = item.IdControlCuchillo;
                    cabecera.Fecha = item.Fecha;
                    cabecera.EstadoReporte = item.EstadoReporte;
                    cabecera.Observacion = item.Observacion;
                    cabecera.FechaIngresoLog = item.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    cabecera.FechaAprobado = item.FechaAprobado;
                    cabecera.AprobadoPor = item.AprobadoPor;
                    listacabecera.Add(cabecera);
                }
                return listacabecera;
            }
        }

        public CC_CUCHILLOS_PREPARACION_CTRL ConsultarCabecera(DateTime fecha, int idControlCuchillo)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                CC_CUCHILLOS_PREPARACION_CTRL lista;
                if (idControlCuchillo == 0)
                {
                    lista = db.CC_CUCHILLOS_PREPARACION_CTRL.FirstOrDefault(x => x.Fecha.Year == fecha.Year && x.Fecha.Month == fecha.Month && x.Fecha.Day == fecha.Day && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else
                {
                    lista = db.CC_CUCHILLOS_PREPARACION_CTRL.FirstOrDefault(x =>x.IdControlCuchillo==idControlCuchillo && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                CC_CUCHILLOS_PREPARACION_CTRL cabecera= new CC_CUCHILLOS_PREPARACION_CTRL();
                if (lista!=null)
                {
                    cabecera.IdControlCuchillo = lista.IdControlCuchillo;
                    cabecera.Fecha = lista.Fecha;
                    cabecera.EstadoReporte = lista.EstadoReporte;
                    cabecera.Observacion = lista.Observacion;
                    cabecera.AprobadoPor = lista.AprobadoPor;
                    cabecera.FechaAprobado = lista.FechaAprobado;
                    return cabecera;
                }
                return lista;
            }
        }
    }
}