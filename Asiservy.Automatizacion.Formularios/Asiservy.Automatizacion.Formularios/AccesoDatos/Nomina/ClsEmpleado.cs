using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Nomina
{
    public class ClsEmpleado
    {
        public string CEDULA { get; set; }
        public string NOMBRES { get; set; }
        public string DIRECCION { get; set; }
        public string BARRIO { get; set; }
        public string TELEFONO { get; set; }
        public string FECNAC { get; set; }
        public string NOMDEPART { get; set; }
        public string NOMBRECARGO { get; set; }
        public string CELULAR { get; set; }
        public string GENERO { get; set; }
        public string CTABANCARIA { get; set; }
        public string CORREO { get; set; }
        public List<CargaFamiliar> CargasFamiliares { get; set; }
    }

    public class CargaFamiliar
    {
        public string tipo_carga { get; set; }
        public string nombre { get; set; }
        public string fecha_nace { get; set; }
    }
}