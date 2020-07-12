using System;


namespace Asiservy.Automatizacion.Formularios.Models.CALIDAD
{
    public class CabeceraEvaluacionProductoEnfundadoViewModel
    {
        public int IdEvaluacionProductoEnfundado { get; set; }
        public Nullable<System.DateTime> FechaProduccion { get; set; }
        public Nullable<int> OrdenFabricacion { get; set; }
        public string Turno { get; set; }
        public string Cliente { get; set; }
        public string Marca { get; set; }
        public string Destino { get; set; }
        public string Proveedor { get; set; }
        public string Lote { get; set; }
        public string Batch { get; set; }
        public Nullable<bool> Lomo { get; set; }
        public Nullable<bool> Trozo { get; set; }
        public Nullable<bool> Miga { get; set; }
        public string NivelLimpieza { get; set; }
        public string NivelLimpiezaDescripcion { get; set; }
        public string Observacion { get; set; }
        public string EstadoRegistro { get; set; }
        public System.DateTime FechaIngresoLog { get; set; }
        public string UsuarioIngresoLog { get; set; }
        public string TerminalIngresoLog { get; set; }
        public Nullable<System.DateTime> FechaModificacionLog { get; set; }
        public string UsuarioModificacionLog { get; set; }
        public string TerminalModificacionLog { get; set; }
        public Nullable<bool> EstadoControl { get; set; }
        public string AprobadoPor { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
        public string ImagenCodigo { get; set; }
        public string ImagenProducto1 { get; set; }
        public string ImagenProducto2 { get; set; }
        public string ImagenProducto3 { get; set; }
        public Nullable<int> RotacionImagenCod { get; set; }
        public Nullable<int> RotacionImagenProd1 { get; set; }
        public Nullable<int> RotacionImagenProd2 { get; set; }
        public Nullable<int> RotacionImagenProd3 { get; set; }
        //public byte[] FirmaControl { get; set; }
        //public byte[] FirmaAprobacion { get; set; }
    }
}