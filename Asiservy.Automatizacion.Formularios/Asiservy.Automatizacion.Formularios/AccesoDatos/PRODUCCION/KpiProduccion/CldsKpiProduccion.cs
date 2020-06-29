using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.PRODUCCION.KpiEnvaseLata
{
    public class ClsdKpiProduccion
    {
        public List<spConsultaKpiEnvaseLata> ConsultaKpiEnvaseLatas(DateTime FechaDesde, DateTime FechaHasta, string Turno, String Linea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaKpiEnvaseLata(FechaDesde, FechaHasta, Turno,Linea).ToList();
                return lista;
            }
        }

    }
}