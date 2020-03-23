using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Marcaciones
{
    public class ClsMarcaciones
    {
        public List<MarcacionesEmpleadoLineaViewModel> ObtenerMarcaciones(DateTime fechaIni, DateTime fechaFin, string cedula, string desde)
        {
            List<MarcacionesEmpleadoLineaViewModel> resultado = new List<MarcacionesEmpleadoLineaViewModel>();
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var asistentes = db.spObtieneMarcaciones_Empleado(fechaIni, fechaFin,cedula, desde).ToList();

                foreach (spObtieneMarcaciones_Empleado_Result item in asistentes)
                {

                    resultado.Add(new MarcacionesEmpleadoLineaViewModel
                    {
                        FECHA_MARCA = item.FECHA_MARCA.ToString(),
                        LINEA = item.LINEA.ToString(),
                        DIA = item.DIA.ToString(),
                        CEDULA = item.CEDULA.ToString(),
                        EMPLEADO = item.EMPLEADO.ToString(),
                        INGRESO = item.INGRESO.ToString(),
                        SALIDA = item.SALIDA.ToString()
                    });
                }

            }

            return resultado;
        }
    }
}