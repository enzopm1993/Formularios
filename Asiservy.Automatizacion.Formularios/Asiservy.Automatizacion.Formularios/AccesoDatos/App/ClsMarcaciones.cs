using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.App
{
    public class ClsMarcaciones
    {
        public List<RegistroMarcacion> Marcaciones { get; set; }
        public List<RegistroLogMarcaciones> LogMarcaciones { get; set; }
    }
    public class RegistroMarcacion
    {
        public string FECHA_MARCA { get; set; }
        public string DIA { get; set; }
        public string INGRESO { get; set; }
        public string ALMUERZO { get; set; }
        public string CENA { get; set; }
        public string SALIDA { get; set; }
        public string DIA_MUESTRA { get; set; }
        public string ATRASO { get; set; }
    }
    public class RegistroLogMarcaciones
    {
        public string FECHA_MARCA { get; set; }
        public string HORA { get; set; }
        public string TIPO_MARCACION { get; set; }
    }
}