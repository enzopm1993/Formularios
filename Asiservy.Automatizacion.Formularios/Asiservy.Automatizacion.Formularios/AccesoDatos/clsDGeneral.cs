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
        public List<spConsultaCargos> ConsultaCargos(string dsCodigo)
        {
            
                entities = new ASIS_PRODEntities();
                return entities.spConsultaCargos(dsCodigo).ToList();
           
        }

        public List<sp_GrupoEnfermedades> ConsultaCodigosGrupoSubEnfermedad(string tipo, string Grupo, string SubGrupo)
        {
            
                //entities = new ASIS_PRODEntities();
                //return entities.spConsultaCodigosEnfermedad("0").ToList();
            using(ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                
                return db.sp_GrupoEnfermedades(tipo, Grupo, SubGrupo).ToList();
            }
            
        }
        public List<spConsultaArea> ConsultaAreas(string dsCodigo)
        {
            
                entities = new ASIS_PRODEntities();
                return entities.spConsultaArea(dsCodigo).ToList();
            
        }
        public List<spConsultaLinea> ConsultaLineas()
        {
            entities = new ASIS_PRODEntities();
            return entities.spConsultaLinea("0").ToList();
        }

    }
}