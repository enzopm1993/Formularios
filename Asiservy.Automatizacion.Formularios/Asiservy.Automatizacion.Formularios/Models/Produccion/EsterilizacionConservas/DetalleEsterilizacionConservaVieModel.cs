using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Produccion.EsterilizacionConservas
{
    public class DetalleEsterilizacionConservaVieModel
    {
        public int Autoclave { get; set; }
        public int Esterilizada { get; set; }
        public string Producto { get; set; }
        public int IdDetalleControlEsterilizacionConserva { get; set; }
        public int? TemperaturaInicial { get; set; }
        public DateTime? HoraInicioViento { get; set; }
        public DateTime? HoraCierreViento { get; set; }
        public int? TemperaturaTermDigital { get; set; }
        public DateTime? HoraInicioLlenado { get; set; }
        public DateTime? HoraInicioCalentamiento { get; set; }
        public DateTime? HoraInicioEsterilizacion { get; set; }
        public DateTime? HoraFinalEsterilizacion { get; set; }
        public int IdCabCoche { get; set; }
        public DateTime? TiempoEnfriamiento { get; set; }
        public int? TemperaturaProductoSalida { get; set; }
    }
}