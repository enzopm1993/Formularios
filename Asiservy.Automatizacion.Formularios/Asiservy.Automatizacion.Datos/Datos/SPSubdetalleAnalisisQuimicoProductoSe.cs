//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Asiservy.Automatizacion.Datos.Datos
{
    using System;
    
    public partial class SPSubdetalleAnalisisQuimicoProductoSe
    {
        public int IdDetalle { get; set; }
        public string CodTipoProducto { get; set; }
        public string TipoProducto { get; set; }
        public int ParametroLaboratorio { get; set; }
        public int IdClasificador { get; set; }
        public string CodArea { get; set; }
        public string Area { get; set; }
        public string NombreParametro { get; set; }
        public string DescripcionParametro { get; set; }
        public Nullable<bool> CalcMinMax { get; set; }
        public Nullable<decimal> Cantidad { get; set; }
    }
}
