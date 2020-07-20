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

        public List<ModeloVistaHorasHombre> ObtenerReporteHorasHombre(DateTime fechaIni, DateTime fechaFin)
        {
            List<ModeloVistaHorasHombre> objRetorna = new List<ModeloVistaHorasHombre>();
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var rptData = db.ASY_ReporteHorasMovimientoPersonalDetallado(fechaIni, fechaFin).ToList();

                foreach (ASY_ReporteHorasMovimientoPersonalDetallado_Result item in rptData)
                {

                    objRetorna.Add(new ModeloVistaHorasHombre
                    {
                        Fecha = String.Format("{0:dd/MM/yyyy}", item.Fecha),
                        Cedula = item.Cedula,
                        Nombre = item.NOMBRES,
                        CodCentroCosto = item.CodCentroCosto,
                        CentroCosto = item.CentroCosto,
                        CodLinea = item.CodLinea,
                        Linea = item.Linea,
                        CodRecurso = item.CodRecurso,
                        Recurso = item.Recurso,
                        CodCargo = item.CodCargo,
                        Cargo = item.Cargo,
                        Turno = item.Turno,
                       // HoraInico = item.INICIO,
                        //HoraFin = item.FIN,
                        HorasReloj = item.HORAS_RELOJ,
                        DescuentoAlmuerzo = item.DESCUENTO_ALMUERZO,
                        DescuentoCena = item.DESCUENTO_CENA,
                        HorasLaboradas = item.HORAS_LABORADAS,
                        NoFinAsistencia = item.NO_FIN_ASISTENCIA,
                        TipoRol = item.TIPO_ROL
                    });
                }

            }

            return objRetorna;
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

        public List<ModelViewDatosEmpleados> ListaEmpleadosDatosPersonales()
        {
            List<ModelViewDatosEmpleados> obtRespuesta = new List<ModelViewDatosEmpleados>();
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var results = db.EmpleadosDatosPersonales().ToList();

                foreach (EmpleadosDatosPersonales_Result item in results)
                {

                    obtRespuesta.Add(new ModelViewDatosEmpleados
                    {
                        CODTRA = item.CODTRA.ToString(),
                        AREA = item.AREA.ToString(),
                        CARGO = item.CARGO.ToString(),
                        LINEA = item.LINEA.ToString(),
                        CEDULA = item.CEDULA.ToString(),
                        NOMBRES = item.NOMBRES.ToString(),
                        DIRECCION = item.DIRECCION.ToString(),
                        BARRIO = item.BARRIO.ToString(),
                        TELEFONO = item.TELEFONO.ToString(),
                        CELULAR = item.CELULAR.ToString(),
                        CORREO = item.CORREO.ToString()
                    });
                }

            }

            return obtRespuesta;

        }

        public bool insertarLogCambio(ModelViewDatosEmpleados datosNuevos, string usuario, string terminal)
        {
            bool resp = false;

            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                BITACORA_CAMBIO_DATOS objetoNuevo = new BITACORA_CAMBIO_DATOS();

                objetoNuevo.usuarioIngresa = usuario;
                objetoNuevo.fechaCrea = DateTime.Now;
                objetoNuevo.terminalIngresa = terminal;
                objetoNuevo.DIRECCION = datosNuevos.DIRECCION;
                objetoNuevo.BARRIO = datosNuevos.BARRIO;
                objetoNuevo.TELEFONO = datosNuevos.TELEFONO;
                objetoNuevo.CELULAR = datosNuevos.CELULAR;
                objetoNuevo.CORREO = datosNuevos.CORREO;
                objetoNuevo.CODTRA = datosNuevos.CODTRA;

                db.BITACORA_CAMBIO_DATOS.Add(objetoNuevo);
                db.SaveChanges();
                if (objetoNuevo.id > 0)
                {
                    resp = true;
                }
            }



            return resp;
        }

    }
}