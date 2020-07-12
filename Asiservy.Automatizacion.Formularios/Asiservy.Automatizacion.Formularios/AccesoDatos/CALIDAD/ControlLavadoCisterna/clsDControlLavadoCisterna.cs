using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ControlLavadoCisterna
{
    public class clsDControlLavadoCisterna
    {
        public List<sp_Control_Lavado_Cisterna> ConsultarLavadoCisterna(DateTime fechaDesde, DateTime fechaHasta,int idLavadoCisterna, int op)
        {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                var lista = db.sp_Control_Lavado_Cisterna(fechaDesde, fechaHasta, idLavadoCisterna, op).ToList();
                return lista;
            }
        }

        public int GuardarModificarLavadoCisterna(CC_LAVADO_CISTERNA guardarmodificar, int siAprobar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                var model = db.CC_LAVADO_CISTERNA.FirstOrDefault(x=> x.IdLavadoCisterna==guardarmodificar.IdLavadoCisterna && x.EstadoRegistro==guardarmodificar.EstadoRegistro);
                if (model!=null){
                    if (siAprobar == 0)
                    {
                        model.Fecha = guardarmodificar.Fecha;
                        model.QuimUtilizados = guardarmodificar.QuimUtilizados;
                        model.Observacion = guardarmodificar.Observacion;
                        valor = 1;
                    } else if (siAprobar==1) {
                        model.EstadoReporte = guardarmodificar.EstadoReporte;
                        model.FechaAprobado = guardarmodificar.FechaAprobado;
                        model.AprobadoPor = guardarmodificar.UsuarioIngresoLog;
                        valor = 2;
                    }
                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;                    
                }
                else
                {
                    db.CC_LAVADO_CISTERNA.Add(guardarmodificar);
                }
                db.SaveChanges();
                return valor;
            }            
        }

        public int EliminarLavadoCisterna(CC_LAVADO_CISTERNA registroEliminar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db=new ASIS_PRODEntities()) {
                var model = db.CC_LAVADO_CISTERNA.FirstOrDefault(x=> x.IdLavadoCisterna== registroEliminar.IdLavadoCisterna);
                if (model!=null)
                {
                    model.EstadoRegistro = registroEliminar.EstadoRegistro;
                    model.FechaModificacionLog = registroEliminar.FechaIngresoLog;
                    model.TerminalModificacionLog = registroEliminar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = registroEliminar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }

        //----------------------------------------GUARDAR TABLA INTERMEDIA-------------------------------------------------------------
        public int GuardarModificarLavadoCisternaIntermedia(CC_INTERMEDIA_CTRL_MANT_CISTERNA guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                //SOLO CREO, NO ACTUALIZO, CUANDO EL USUARIO ACTULIZA EL REGISTRO PRINCIPAL EN LA TABLA INTERMEDIA CREO NUEVOS REGISTROS Y ELIMINO LOS ANTERIORES
                //CORRESPONDIENTES AL REGISTRO PRINCIPAL
                db.CC_INTERMEDIA_CTRL_MANT_CISTERNA.Add(guardarmodificar);
                
                db.SaveChanges();
                return valor;
            }

        }

        public int EliminarLavadoCisternaIntermedia(CC_INTERMEDIA_CTRL_MANT_CISTERNA registroEliminar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_INTERMEDIA_CTRL_MANT_CISTERNA.FirstOrDefault(x => x.IdIntermedia == registroEliminar.IdIntermedia);
                if (model != null)
                {
                    model.EstadoRegistro = registroEliminar.EstadoRegistro;
                    model.FechaModificacionLog = registroEliminar.FechaIngresoLog;
                    model.TerminalModificacionLog = registroEliminar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = registroEliminar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }

        public List<CC_LAVADO_CISTERNA> ConsultarReporteCabecera(DateTime fechaDesde, DateTime fechaHasta)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = (from c in db.CC_LAVADO_CISTERNA
                             where (c.Fecha >= fechaDesde && c.Fecha<=fechaHasta && c.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                             orderby c.Fecha descending
                             select new { c.IdLavadoCisterna, c.Fecha, c.EstadoReporte, c.FechaIngresoLog, c.UsuarioIngresoLog, c.FechaAprobado, c.AprobadoPor, c.FechaModificacionLog, c.UsuarioModificacionLog }).ToList();
                List<CC_LAVADO_CISTERNA> listacabecera = new List<CC_LAVADO_CISTERNA>();
                CC_LAVADO_CISTERNA cabecera;
                foreach (var item in lista)
                {
                    cabecera = new CC_LAVADO_CISTERNA();
                    cabecera.IdLavadoCisterna = item.IdLavadoCisterna;
                    cabecera.Fecha = item.Fecha;
                    cabecera.EstadoReporte = item.EstadoReporte;
                    cabecera.FechaIngresoLog = item.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    cabecera.FechaAprobado = item.FechaAprobado;
                    cabecera.FechaModificacionLog = item.FechaModificacionLog;
                    cabecera.UsuarioModificacionLog = item.UsuarioModificacionLog;
                    cabecera.AprobadoPor = item.AprobadoPor;
                    listacabecera.Add(cabecera);
                }
                return listacabecera;
            }
        }
    }
}