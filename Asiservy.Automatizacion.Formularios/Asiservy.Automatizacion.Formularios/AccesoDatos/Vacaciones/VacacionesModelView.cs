using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Vacaciones
{
    public class VacacionesModelView
    {
        public string Linea { get; set; }
        public string Nombres { get; set; }
        public decimal? TotalDias { get; set; }
        public decimal? DiasTomados { get; set; }
        public decimal? Saldo { get; set; }
    }
}