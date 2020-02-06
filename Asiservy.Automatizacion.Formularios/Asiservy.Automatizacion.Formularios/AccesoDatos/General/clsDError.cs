using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.Entity.Validation;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDError
    {
        string path= @"D:\Errores.txt";

        public string ControlError(string usuario, string host, string controlador, string observacion, Exception ex, DbEntityValidationException  Dbex  )
        {

            string MensajeError = string.Empty;
            string MensajeCorto = string.Empty;
            if (ex != null)
            {
                MensajeError = ex.Message+" METODO:"+ ex.TargetSite;
                MensajeCorto = ex.Message;
            }
            else
            {
                foreach (var eve in Dbex.EntityValidationErrors)
                {
                    MensajeError = "ENTIDAD " + eve.Entry.Entity.GetType().Name + " ESTADO: " + eve.Entry.State;
                    foreach (var ve in eve.ValidationErrors)
                    {
                        MensajeError = MensajeError + " - PROPIEDAD: " + ve.PropertyName + ", ERROR: " + ve.ErrorMessage + Environment.NewLine;
                    }
                }
                MensajeCorto = Dbex.Message;
            }

            
            GrabarError(new ERROR
            {
                Controlador = controlador,
                Mensaje = MensajeError,
                Observacion = observacion,
                FechaIngreso = DateTime.Now,
                TerminalIngreso = host,
                UsuarioIngreso = usuario
            });

            return MensajeCorto;
        }




        public void GrabarError(ERROR doError)
        {
            try
            {                
                using (ASIS_PRODEntities db = new ASIS_PRODEntities())
                {
                    db.ERROR.Add(doError);
                    db.SaveChanges();
                }
            }
            catch(Exception)
            {
                // doError
                //guardar en archivo txt
                
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(doError.FechaIngreso + "\t" + doError.Controlador+"\t"+doError.Mensaje+"\t"+doError.Observacion+"\t"+doError.TerminalIngreso+"\t"+doError.UsuarioIngreso);
                    }

            }
            
        }
    }
}