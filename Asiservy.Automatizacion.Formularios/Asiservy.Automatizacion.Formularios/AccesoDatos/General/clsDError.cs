using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDError
    {
        string path= @"D:\Errores.txt";
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