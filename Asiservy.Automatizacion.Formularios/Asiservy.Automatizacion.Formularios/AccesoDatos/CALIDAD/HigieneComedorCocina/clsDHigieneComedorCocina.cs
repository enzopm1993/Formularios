using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.HigieneComedorCocina
{
    public class clsDHigieneComedorCocina
    {
        public List<CC_HIGIENE_COMEDOR_COCINA_MANT> ConsultaHigieneMant()
        {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                var lista = db.CC_HIGIENE_COMEDOR_COCINA_MANT.ToList();
                List<CC_HIGIENE_COMEDOR_COCINA_MANT> listaMantenimiento = new List<CC_HIGIENE_COMEDOR_COCINA_MANT>();
                CC_HIGIENE_COMEDOR_COCINA_MANT mantenimiento;
                foreach (var item in lista)
                {
                    mantenimiento = new CC_HIGIENE_COMEDOR_COCINA_MANT();
                    mantenimiento.Categoria = item.Categoria;
                    mantenimiento.IdMantenimiento = item.IdMantenimiento;
                    mantenimiento.Nombre = item.Nombre;
                    mantenimiento.Observacion = item.Observacion;
                    mantenimiento.FechaIngresoLog = item.FechaIngresoLog;
                    mantenimiento.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    mantenimiento.EstadoRegistro = item.EstadoRegistro;
                    listaMantenimiento.Add(mantenimiento);
                }
                return listaMantenimiento;
            }
        }

        public List<CC_HIGIENE_COMEDOR_COCINA_MANT> ConsultaHigieneMantActivos(string estadoRegistro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_HIGIENE_COMEDOR_COCINA_MANT.Where(x=> x.EstadoRegistro== estadoRegistro).ToList();
                List<CC_HIGIENE_COMEDOR_COCINA_MANT> listaMantenimiento = new List<CC_HIGIENE_COMEDOR_COCINA_MANT>();
                CC_HIGIENE_COMEDOR_COCINA_MANT mantenimiento;
                foreach (var item in lista)
                {
                    mantenimiento = new CC_HIGIENE_COMEDOR_COCINA_MANT();
                    mantenimiento.Categoria = item.Categoria;
                    mantenimiento.IdMantenimiento = item.IdMantenimiento;
                    mantenimiento.Nombre = item.Nombre;
                    mantenimiento.Observacion = item.Observacion;
                    mantenimiento.FechaIngresoLog = item.FechaIngresoLog;
                    mantenimiento.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    mantenimiento.EstadoRegistro = item.EstadoRegistro;
                    listaMantenimiento.Add(mantenimiento);
                }
                return listaMantenimiento;
            }
        }

        public List<sp_Control_Higine_Comedor_Cocina> ConsultaHigieneSP(int idControlHigiene, DateTime fechaDesde, DateTime fechaHasta, int op)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {                
                var lista = db.sp_Control_Higine_Comedor_Cocina(idControlHigiene, fechaDesde, fechaHasta, op).ToList();              
                return lista;
            }
        }

        public int GuardarModificarMant(CC_HIGIENE_COMEDOR_COCINA_MANT guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_HIGIENE_COMEDOR_COCINA_MANT.FirstOrDefault(x => x.IdMantenimiento == guardarModificar.IdMantenimiento);
                if (model != null)
                {
                    model.Nombre = guardarModificar.Nombre;
                    model.Categoria = guardarModificar.Categoria;
                    model.Observacion = guardarModificar.Observacion;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_HIGIENE_COMEDOR_COCINA_MANT.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarMant(CC_HIGIENE_COMEDOR_COCINA_MANT guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_HIGIENE_COMEDOR_COCINA_MANT.FirstOrDefault(x => x.IdMantenimiento == guardarModificar.IdMantenimiento);
                if (model != null)
                {
                    model.EstadoRegistro = guardarModificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }             
                return valor;
            }
        }

        public List<CC_HIGIENE_COMEDOR_COCINA_CTRL> ConsultarHigieneControl(DateTime fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_HIGIENE_COMEDOR_COCINA_CTRL.Where(x => x.Fecha.Year==fecha.Year && x.Fecha.Month==fecha.Month && x.Fecha.Day==fecha.Day && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();
                List<CC_HIGIENE_COMEDOR_COCINA_CTRL> listacabecera = new List<CC_HIGIENE_COMEDOR_COCINA_CTRL>();
                CC_HIGIENE_COMEDOR_COCINA_CTRL cabecera;
                foreach (var item in lista)
                {
                    cabecera = new CC_HIGIENE_COMEDOR_COCINA_CTRL();
                    cabecera.IdControlHigiene = item.IdControlHigiene;
                    cabecera.Fecha = item.Fecha;
                    cabecera.Hora = item.Hora;
                    cabecera.EstadoReporte = item.EstadoReporte;
                    cabecera.Observacion = item.Observacion;
                    cabecera.FirmaControl = item.FirmaControl;
                    cabecera.AprobadoPor = item.AprobadoPor;
                    cabecera.FechaAprobado = item.FechaAprobado;
                    listacabecera.Add(cabecera);
                }
                return listacabecera;
            }
        }

        public int GuardarModificarHigieneControl(CC_HIGIENE_COMEDOR_COCINA_CTRL guardarModificar, int siAprobar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_HIGIENE_COMEDOR_COCINA_CTRL.FirstOrDefault(x => x.IdControlHigiene == guardarModificar.IdControlHigiene && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    if (siAprobar == 1)
                    {
                        model.EstadoReporte = guardarModificar.EstadoReporte;
                        model.FechaAprobado = guardarModificar.FechaIngresoLog;
                        model.AprobadoPor = guardarModificar.UsuarioIngresoLog;
                        valor = 2;//APRROBADO
                    }
                    else
                    {
                        model.Fecha = guardarModificar.Fecha;
                        model.Observacion = guardarModificar.Observacion;
                        model.Hora = guardarModificar.Hora;
                        model.Observacion = guardarModificar.Observacion;
                        valor = 1;//ACTUALIZAR
                    }
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;                   
                }
                else
                {
                    db.CC_HIGIENE_COMEDOR_COCINA_CTRL.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarHigieneControl(CC_HIGIENE_COMEDOR_COCINA_CTRL guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_HIGIENE_COMEDOR_COCINA_CTRL.FirstOrDefault(x => x.IdControlHigiene == guardarModificar.IdControlHigiene);
                if (model != null)
                {
                    model.EstadoRegistro = guardarModificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }        

        public int GuardarModificarControlDetalle(CC_HIGIENE_COMEDOR_COCINA_CTRL_DET guardarModificar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_HIGIENE_COMEDOR_COCINA_CTRL_DET.FirstOrDefault(x => x.IdControlDetalle == guardarModificar.IdControlDetalle && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.LimpiezaEstado = guardarModificar.LimpiezaEstado;
                    model.AccionCorrectiva = guardarModificar.AccionCorrectiva;
                    model.Observacion = guardarModificar.Observacion;                    
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;//ACTUALIZAR
                }
                else
                {
                    db.CC_HIGIENE_COMEDOR_COCINA_CTRL_DET.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }       

        public CC_HIGIENE_COMEDOR_COCINA_CTRL ConsultarImagenFirma(int idControlHigiene, bool esFirmaControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_HIGIENE_COMEDOR_COCINA_CTRL.FirstOrDefault(x => x.IdControlHigiene == idControlHigiene && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);                
                return lista;
            }            
        }

        public int GuardarImagenFirma(CC_HIGIENE_COMEDOR_COCINA_CTRL guardarmodificar, bool esFirmaControl)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_HIGIENE_COMEDOR_COCINA_CTRL.FirstOrDefault(x => x.IdControlHigiene == guardarmodificar.IdControlHigiene && x.EstadoRegistro == guardarmodificar.EstadoRegistro);
                if (model != null)
                {
                    if (esFirmaControl == true)
                    {
                        model.FirmaControl = guardarmodificar.FirmaControl;
                    }
                    else
                    {
                        model.FirmaAprobado = guardarmodificar.FirmaAprobado;
                    }
                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                    valor = 1;
                }
                db.SaveChanges();
                return valor;
            }
        }

        public List<CC_HIGIENE_COMEDOR_COCINA_CTRL> BandejaConsultarHigieneControl(bool estadoReReporte, DateTime fechaDesde, DateTime fechaHasta)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                //var lista = db.CC_HIGIENE_COMEDOR_COCINA_CTRL.Where(x => x.EstadoReporte == estadoReReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo &&
                //                                                    x.Fecha.Date >= fechaDesde.Date && x.Fecha.Date <= fechaHasta.Date).ToList();
                //var lista = db.CC_HIGIENE_COMEDOR_COCINA_CTRL.Where(x=> x.Fecha.Date >= fechaDesde.Date);
                var lista = (from c in db.CC_HIGIENE_COMEDOR_COCINA_CTRL
                             where (c.Fecha >= fechaDesde && c.Fecha <= fechaHasta && c.EstadoReporte == estadoReReporte && c.EstadoRegistro==clsAtributos.EstadoRegistroActivo)
                             select new { c.IdControlHigiene, c.Fecha, c.Hora, c.EstadoReporte, c.Observacion, c.FechaIngresoLog, c.UsuarioIngresoLog }).ToList();
                List<CC_HIGIENE_COMEDOR_COCINA_CTRL> listacabecera = new List<CC_HIGIENE_COMEDOR_COCINA_CTRL>();              
                CC_HIGIENE_COMEDOR_COCINA_CTRL cabecera;
                foreach (var item in lista)
                {
                    cabecera = new CC_HIGIENE_COMEDOR_COCINA_CTRL();
                    cabecera.IdControlHigiene = item.IdControlHigiene;
                    cabecera.Fecha = item.Fecha;
                    cabecera.Hora = item.Hora;
                    cabecera.EstadoReporte = item.EstadoReporte;
                    cabecera.Observacion = item.Observacion;
                    cabecera.FechaIngresoLog = item.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    listacabecera.Add(cabecera);
                }
                return listacabecera;
            }
        }
    }
}