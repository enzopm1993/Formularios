using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Nomina
{
    public class ModeloVistaHorasHombre
    {
        public string Fecha { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string CodCentroCosto { get; set; }
        public string CentroCosto { get; set; }
        public string CodLinea { get; set; }
        public string Linea { get; set; }
        public string CodRecurso { get; set; }
        public string Recurso { get; set; }
        public string CodCargo { get; set; }
        public string Cargo { get; set; }
        public string Turno { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public decimal? HorasReloj { get; set; }
        public decimal? DescuentoAlmuerzo { get; set; }
        public decimal? DescuentoCena { get; set; }
        public decimal? HorasLaboradas { get; set; }
        public bool? NoFinAsistencia { get; set; }
        public string TipoRol { get; set; }
    }
}