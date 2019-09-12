using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDEmpleado
    {

        public List<spConsutaEmpleadosFiltro> ConsultaEmpleadosFiltro(string dsLinea, string dsArea, string dsCargo)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                List<spConsutaEmpleadosFiltro> pListEmpleados = null;
                if (string.IsNullOrEmpty(dsLinea))
                    dsLinea = "0";
                if (string.IsNullOrEmpty(dsArea))
                    dsArea = "0";
                if (string.IsNullOrEmpty(dsCargo))
                    dsCargo = "0";

                pListEmpleados = db.spConsutaEmpleadosFiltro(dsArea, dsLinea, dsCargo).ToList();
                return pListEmpleados;
            }
        }

        public List<spConsutaEmpleadosFiltro> ConsultaEmpleadosFiltroCambioPersonal(string dsLinea, string dsArea, string dsCargo,string psTipo)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (string.IsNullOrEmpty(dsLinea))
                    dsLinea = "0";
                if (string.IsNullOrEmpty(dsArea))
                    dsArea = "0";
                if (string.IsNullOrEmpty(dsCargo))
                    dsCargo = "0";

                List<spConsutaEmpleadosFiltro> pListEmpleados = null;
                List<spConsutaEmpleadosFiltro> pListEmpleadoR = new List<spConsutaEmpleadosFiltro>();
                pListEmpleados = db.spConsutaEmpleadosFiltro(dsArea, dsLinea, dsCargo).ToList();
                if (psTipo == clsAtributos.TipoPrestar)
                {
                    foreach (var item in pListEmpleados.ToArray())
                    {
                        if (db.CAMBIO_PERSONAL.Any(x => x.Cedula == item.CEDULA&&x.EstadoRegistro==clsAtributos.EstadoRegistroActivo))
                        {
                            pListEmpleados.Remove(item);
                        }
                    }
                    return pListEmpleados;
                }
                else
                {
                    foreach (var item in pListEmpleados)
                    {
                        if (db.CAMBIO_PERSONAL.Any(x => x.Cedula == item.CEDULA&&x.EstadoRegistro==clsAtributos.EstadoRegistroActivo))
                        {
                            pListEmpleadoR.Add(item);
                        }
                    }
                    return pListEmpleadoR;
                }
                
                
            }
        }
        public List<spConsutaEmpleados> ConsultaEmpleado(string dsCedula)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())            {

                List<spConsutaEmpleados> pListEmpleados = null;
                if (!string.IsNullOrEmpty(dsCedula))
                    pListEmpleados = db.spConsutaEmpleados(dsCedula).ToList();
                return pListEmpleados;
            }
        }
    }
}