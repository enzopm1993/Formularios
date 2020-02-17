using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Nomina
{
    public class ClsNomina
    {
        public ModeloVistaTablasPersonalPreesente ObtenerTablasPersonalAsistente(DateTime fecha)
        {
            ModeloVistaTablasPersonalPreesente objTablas = new ModeloVistaTablasPersonalPreesente();
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var asistentesIniciales = db.sp_obtener_asistenia_inicio_vs_actual(fecha).ToList();               

                objTablas.ListaAsistenciaTodos =  new List<ModeloVistaPersonalPresentePrueba>();
                objTablas.ListaAsistenciaInicial = new List<ModeloVistaPersonalPresente>();
                foreach (sp_obtener_asistenia_inicio_vs_actual_Result item in asistentesIniciales)
                {
                    objTablas.ListaAsistenciaInicial.Add(new ModeloVistaPersonalPresente {
                        CodCentroCosto = item.CodCentroCosto,
                        CentroCosto = item.CentroCosto,
                        CodRecurso = item.CodRecurso,
                        Recurso = item.Recurso,
                        CodCargo = item.CodCargo,
                        Cargo = item.Cargo,
                        CodLinea = item.CodLinea,
                        Linea = item.Linea,
                        Cedula = item.Cedula,
                        Nombre = item.Nombre,
                        Asistencia ="Inicial"
                    });

                    objTablas.ListaAsistenciaTodos.Add(new ModeloVistaPersonalPresentePrueba
                    {                       
                        CentroCosto = item.CentroCosto,
                        Recurso = item.Recurso,
                        Cargo = item.Cargo,
                        Linea = item.Linea,
                        Cedula = item.Cedula,
                        Nombre = item.Nombre,
                        Asistencia = "Inicial"
                    });
                }

                var asistentesActuales = db.sp_obtener_asistenia_inicio_vs_actual(fecha).ToList();
                objTablas.ListaAsistenciaActual = new List<ModeloVistaPersonalPresente>();
                foreach (sp_obtener_asistenia_inicio_vs_actual_Result item in asistentesActuales)
                {
                    objTablas.ListaAsistenciaActual.Add(new ModeloVistaPersonalPresente
                    {
                        CodCentroCosto = item.CodCentroCosto,
                        CentroCosto = item.CentroCosto,
                        CodRecurso = item.CodRecurso,
                        Recurso = item.Recurso,
                        CodCargo = item.CodCargo,
                        Cargo = item.Cargo,
                        CodLinea = item.CodLinea,
                        Linea = item.Linea,
                        Cedula = item.Cedula,
                        Nombre = item.Nombre,
                        Asistencia = "Actual"
                    });
                    objTablas.ListaAsistenciaTodos.Add(new ModeloVistaPersonalPresentePrueba
                    {
                        CentroCosto = item.CentroCosto,
                        Recurso = item.Recurso,
                        Cargo = item.Cargo,
                        Linea = item.Linea,
                        Cedula = item.Cedula,
                        Nombre = item.Nombre,
                        Asistencia = "Actual"
                    });
                }
            }

            return objTablas;
        }

    }
}