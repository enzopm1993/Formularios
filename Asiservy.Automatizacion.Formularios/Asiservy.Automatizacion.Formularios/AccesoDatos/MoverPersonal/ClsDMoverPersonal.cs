using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.MoverPersonal
{
    public class ClsDMoverPersonal
    {
        public string MoverPersonal(string Cedula, string CentroCostos, string Recurso, string Linea, string Cargo)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return "";
            }
        }
    }
}