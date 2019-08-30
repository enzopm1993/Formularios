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
            
                entities = new ASIS_PRODEntities();
                return entities.spConsultaCargos("0").ToList();
           
        }

        public List<spConsultaCodigosEnfermedad> ConsultaCodigosEnfermedad()
        {
            
                entities = new ASIS_PRODEntities();
                return entities.spConsultaCodigosEnfermedad("0").ToList();
            
        }
        public List<spConsultaArea> ConsultaAreas()
        {
            
                entities = new ASIS_PRODEntities();
                return entities.spConsultaArea("0").ToList();
            
        }
        public List<spConsultaLinea> ConsultaLineas()
        {
            entities = new ASIS_PRODEntities();
            return entities.spConsultaLinea("0").ToList();
        }

    }
}