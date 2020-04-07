using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ControlLavadoCisterna
{
    public class clsDControlLavadoCisterna
    {
        public List<sp_Control_Lavado_Cisterna> ConsultarLavadoCisterna(DateTime fechaDesde, DateTime fechaHasta, int op)
        {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                var lista = db.sp_Control_Lavado_Cisterna(fechaDesde, fechaHasta, op).ToList();
                return lista;
            }
        }

        public int GuardarModificarLavadoCisterna(CC_LAVADO_CISTERNA guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                var model = db.CC_LAVADO_CISTERNA.FirstOrDefault(x=> x.IdLavadoCisterna==guardarmodificar.IdLavadoCisterna && x.EstadoRegistro==guardarmodificar.EstadoRegistro);
                if (model!=null){
                    model.Fecha = guardarmodificar.Fecha;
                    model.QuimUtilizados = guardarmodificar.QuimUtilizados;
                    model.Observacion = guardarmodificar.Observacion;
                    model.EstadoRegistro = guardarmodificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                    valor = 1;
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
                var model = db.CC_INTERMEDIA_CTRL_MANT_CISTERNA.FirstOrDefault(x => x.IdIntermedia == guardarmodificar.IdIntermedia && x.EstadoRegistro == guardarmodificar.EstadoRegistro);
                if (model != null)
                {
                    model.IdMantCisterna = guardarmodificar.IdMantCisterna;
                    model.IdCtrlLavadoCisterna = guardarmodificar.IdCtrlLavadoCisterna;                    
                    model.EstadoRegistro = guardarmodificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_INTERMEDIA_CTRL_MANT_CISTERNA.Add(guardarmodificar);
                }
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
    }
}