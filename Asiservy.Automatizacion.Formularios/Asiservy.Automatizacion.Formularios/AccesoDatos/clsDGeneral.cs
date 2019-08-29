using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDGeneral
    {
        ASIS_PRODEntities entities = null;
        public List<spConsultaCargos> ConsultaCargos()
        {
            try
            {
                entities = new ASIS_PRODEntities();
                return entities.spConsultaCargos("0").ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<spConsultaCodigosEnfermedad> ConsultaCodigosEnfermedad()
        {
            try
            {
                entities = new ASIS_PRODEntities();
                return entities.spConsultaCodigosEnfermedad("0").ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<spConsultaArea> ConsultaAreas()
        {
            try
            {
                entities = new ASIS_PRODEntities();
                return entities.spConsultaArea("0").ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<spConsultaLinea> ConsultaLineas()
        {
            try
            {
                entities = new ASIS_PRODEntities();
                return entities.spConsultaLinea("0").ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}