using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.PRODUCCION.CocheAutoclave
{
    public class clsDCcocheAutoclave
    {
        public List<spConsultaCocheAutoclave> ConsultaMapeoProductoTunel(DateTime Fecha, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaCocheAutoclave(Fecha, Turno).ToList();
                return lista;
            }
        }



    }
}