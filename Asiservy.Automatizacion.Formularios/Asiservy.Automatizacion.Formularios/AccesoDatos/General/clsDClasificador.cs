using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDClasificador
    {
        ASIS_PRODEntities entities = null;

        public List<CLASIFICADOR> ConsultarClasificador(string dsGrupo, int diCodigo=0)
        {
            entities = new ASIS_PRODEntities();
            if(diCodigo!=0)
                return entities.CLASIFICADOR.Where(x => x.Grupo == dsGrupo && x.Codigo==diCodigo && x.EstadoRegistro=="A").ToList();
            else
                return entities.CLASIFICADOR.Where(x => x.Grupo == dsGrupo && x.Codigo != diCodigo && x.EstadoRegistro == "A").ToList();

        }

    }
}