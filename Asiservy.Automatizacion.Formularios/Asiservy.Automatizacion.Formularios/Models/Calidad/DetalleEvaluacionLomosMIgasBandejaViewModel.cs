using System;


namespace Asiservy.Automatizacion.Formularios.Models.Calidad
{
    public class DetalleEvaluacionLomosMIgasBandejaViewModel
    {
        public bool? Lomo { get; set; }
        public bool? Miga { get; set; }
        public DateTime? FechaControl { get; set; }
        public int IdCabecera { get; set; }
        public int IdDetalle {get;set;}
        public TimeSpan? Hora { get; set; }
        public string Linea { get; set; }
        public string Buque { get; set; }
        public string Lote { get; set; }
        public int CodSabor { get; set; }
        public string Sabor { get; set; }
        public int CodTextura { get; set; }
        public string Textura { get; set; }
        public int CodColor { get; set; }
        public string Color { get; set; }
        public int CodOlor { get; set; }
        public string Olor { get; set; }
        public int? CodMoretones { get; set; }
        public string Moretones { get; set; }
        //public int? HematomasProfundos { get; set; }
        public int CodProteinas { get; set; }
        public string Proteinas { get; set; }
        public decimal? Trozos { get; set; }
        public int? Venas { get; set; }
        public int? Espinas { get; set; }
        public int? Sangre { get; set; }
        public int? Escamas { get; set; }
        public int? Piel { get; set; }
        public bool? Aprobado { get; set; }

    }
}