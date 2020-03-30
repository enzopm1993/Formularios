using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Nomina
{
    public class ClsNomina
    {
        public List<ModeloVistaPersonalPresente> ObtenerTablasPersonalAsistente(DateTime fechaIni, DateTime fechaFin)
        {
            List<ModeloVistaPersonalPresente> objTablas = new List<ModeloVistaPersonalPresente>();
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var asistentes = db.sp_obtener_asistenia_inicio_vs_actual(fechaIni, fechaFin).ToList();               

                foreach (sp_obtener_asistenia_inicio_vs_actual_Result1 item in asistentes)
                {                    

                    objTablas.Add(new ModeloVistaPersonalPresente
                    {
                        Fecha = String.Format("{0:dd/MM/yyyy}", item.Fecha),    
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

        public sp_ObtenerInfoEmpleadoParaSAP_Result ObtenerInfoEmpleadoParaSAP(string cedula)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.sp_ObtenerInfoEmpleadoParaSAP(cedula).FirstOrDefault();
            }
        }
    }
}