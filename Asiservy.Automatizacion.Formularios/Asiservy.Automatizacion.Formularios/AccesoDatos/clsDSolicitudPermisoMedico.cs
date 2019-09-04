using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDSolicitudPermisoMedico
    {
        public List<sp_GrupoEnfermedades> ConsultaGrupoEnfermedades(string Tipo, string Grupo, string Subgrupo)
        {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {

                return db.sp_GrupoEnfermedades(Tipo, Grupo, Subgrupo).ToList();
            }
        }
    }
}