using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Nomina
{
    public class ClsNomina
    {
        public List<ModeloVistaPersonalPresente> ObtenerTablasPersonalAsistente(DateTime fecha)
        {
            List<ModeloVistaPersonalPresente> objTablas = new List<ModeloVistaPersonalPresente>();
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var asistentes = db.sp_obtener_asistenia_inicio_vs_actual(fecha).ToList();               

                foreach (sp_obtener_asistenia_inicio_vs_actual_Result item in asistentes)
                {                    

                    objTablas.Add(new ModeloVistaPersonalPresente
                    {                       
                        CentroCosto = item.CentroCosto,
                        Recurso = item.Recurso,
                        Cargo = item.Cargo,
                        Linea = item.Linea,
                        Cedula = item.Cedula,
                        Nombre = item.Nombre,
                        Asistencia = item.Asistencia
                    });
                }

            }

            return objTablas;
        }

    }
}