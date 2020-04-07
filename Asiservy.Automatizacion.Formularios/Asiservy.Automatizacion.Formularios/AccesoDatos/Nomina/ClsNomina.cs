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

                foreach (sp_obtener_asistenia_inicio_vs_actual_Result item in asistentes)
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

        public List<ModeloVistaPersonalPresenteBiometrico> ObtenerAsistenciaBiomentrico(DateTime fechaIni, DateTime fechaFin)
        {


            List<ModeloVistaPersonalPresenteBiometrico> objTablas = new List<ModeloVistaPersonalPresenteBiometrico>();
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var asistentes = db.sp_obtenerAsistenciaBiometrico(fechaIni, fechaFin).ToList();

                foreach (sp_obtenerAsistenciaBiometrico_Result item in asistentes)
                {

                    objTablas.Add(new ModeloVistaPersonalPresenteBiometrico
                    {
                        FECHA = String.Format("{0:dd/MM/yyyy}", item.FECHA),
                        CEDULA = item.CEDULA,
                        NOMBRES = item.NOMBRES,
                        COD_LINEA = item.COD_LINEA,
                        COD_CENTRO_COSTO = item.COD_CENTRO_COSTO,
                        COD_RECURSO = item.COD_RECURSO,
                        ROL = item.ROL,
                        LINEA = item.LINEA,
                        RECURSO = item.RECURSO,
                        CENTRO_COSTO = item.CENTRO_COSTO,
                        ESTADO_ASISTENCIA = item.ESTADO_ASISTENCIA,
                        ESTADO_ASISTENCIA_EXONERADO = item.ESTADO_ASISTENCIA_EXONERADO,
                        TIPO_PROCESO = item.TIPO_PROCESO,
                        HORA_ENTRADA = item.HORA_ENTRADA,
                        HORA_SALIDA = item.HORA_SALIDA,
                        NOVEDAD = item.NOVEDAD
                    });
                }

            }

            return objTablas;
           
        }
    }
}