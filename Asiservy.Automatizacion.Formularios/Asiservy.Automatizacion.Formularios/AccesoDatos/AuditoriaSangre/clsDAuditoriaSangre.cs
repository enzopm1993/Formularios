using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.AuditoriaSangre
{
    public class clsDAuditoriaSangre
    {
        public List<spConsultarAuditoriaSangreDiaria>  GuardarActualizarAuditoriaSangre(CONTROL_AUDITORIASANGRE AuditoriaSangre)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var BuscarAuditoriaSangre = db.CONTROL_AUDITORIASANGRE.Where(x => x.FechaCreacionLog == AuditoriaSangre.FechaCreacionLog && x.Cedula == AuditoriaSangre.Cedula).ToList();
                if (BuscarAuditoriaSangre.Count == 0)
                {
                    db.CONTROL_AUDITORIASANGRE.Add(AuditoriaSangre);
                    db.SaveChanges();

                    //return db.spConsultarAuditoriaSangreDiaria().ToList();

                }
                return db.spConsultarAuditoriaSangreDiaria().ToList();
            }
                
        }
    }
}