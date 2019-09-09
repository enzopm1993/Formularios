﻿using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDGeneral
    {
       
        public List<spConsultaCargos> ConsultaCargos(string dsCodigo)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaCargos(dsCodigo).ToList();
            }
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

            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaArea(dsCodigo).ToList();

            }
            
        }
        public List<spConsultaLinea> ConsultaLineas()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaLinea("0").ToList();
            }
        }

        public String ConsultarLineaUsuario(string Identificacion)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                string Linea = string.Empty;
                var Empleado =  entities.spConsutaEmpleados(Identificacion).FirstOrDefault();
                if (Empleado != null)
                {
                    var poLinea= entities.spConsultaLinea(Empleado.CODIGOLINEA).FirstOrDefault();
                    if (poLinea != null)
                    {
                        Linea = poLinea.Descripcion ?? "";
                    }
                }

                return Linea;
            }
        }

    }
}