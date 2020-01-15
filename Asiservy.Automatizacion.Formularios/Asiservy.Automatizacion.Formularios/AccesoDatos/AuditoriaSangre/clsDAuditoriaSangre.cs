using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.AuditoriaSangre
{
    public class clsDAuditoriaSangre
    {
        public void EliminarAuditoriaSangre(CONTROL_AUDITORIASANGRE AuditoriaSangre)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                
                var BuscarAuditoriaSangre = db.CONTROL_AUDITORIASANGRE.Find(AuditoriaSangre.IdControlAuditoriaSangre);
                if (BuscarAuditoriaSangre != null)
                {  
                    BuscarAuditoriaSangre.EstadoRegistro = AuditoriaSangre.EstadoRegistro;
                    BuscarAuditoriaSangre.FechaModificacionLog = DateTime.Now;  
                    BuscarAuditoriaSangre.Observacion = AuditoriaSangre.Observacion;
                    BuscarAuditoriaSangre.TerminalModificacionLog = AuditoriaSangre.TerminalCreacionLog;
                    BuscarAuditoriaSangre.UsuarioModificacionLog = AuditoriaSangre.UsuarioCreacionLog;
                    db.SaveChanges();

                }              
            }
        }

        public void  GuardarActualizarAuditoriaSangre(CONTROL_AUDITORIASANGRE AuditoriaSangre)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
               
               
                var BuscarAuditoriaSangre = db.CONTROL_AUDITORIASANGRE.Find(AuditoriaSangre.IdControlAuditoriaSangre);
                if (BuscarAuditoriaSangre == null)
                {
                    db.CONTROL_AUDITORIASANGRE.Add(AuditoriaSangre);
                    db.SaveChanges();

                }
                else
                {
                    BuscarAuditoriaSangre.EstadoRegistro = AuditoriaSangre.EstadoRegistro;
                    BuscarAuditoriaSangre.FechaModificacionLog = DateTime.Now;
                    BuscarAuditoriaSangre.Porcentaje = AuditoriaSangre.Porcentaje;
                    BuscarAuditoriaSangre.TipoAuditoria = AuditoriaSangre.TipoAuditoria;
                    BuscarAuditoriaSangre.Observacion = AuditoriaSangre.Observacion;
                    BuscarAuditoriaSangre.TerminalModificacionLog = AuditoriaSangre.TerminalCreacionLog;
                    BuscarAuditoriaSangre.UsuarioModificacionLog = AuditoriaSangre.UsuarioCreacionLog;
                    db.SaveChanges();

                }
               // return db.spConsultarAuditoriaSangreDiaria(AuditoriaSangre.FechaAuditoria).ToList();
            }
                
        }
        public List<spConsultarAuditoriaSangreDiaria> ConsultarAuditoriaSangreDiaria(DateTime FechaProduccion)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultarAuditoriaSangreDiaria(FechaProduccion).ToList();
            }
        }

        public List<spReporteAuditoriaSangre> ConsultarReporteAuditoriaSangre(string CodLinea, DateTime Fecha, string Tipo) 
        {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                return db.spReporteAuditoriaSangre(CodLinea,Fecha, Tipo).ToList();
            }
        }
    }
}