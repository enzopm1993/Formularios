using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models
{
    public class OrdenFabricacionAvance: IEquatable<OrdenFabricacionAvance>
    {
        public int OrdenFabricacion { get; set; }
        public string Lote { get; set; }
        public string Producto { get; set; }
        public string Especie { get; set; }
        public string Limpieza { get; set; }
        public string Talla { get; set; }
        public string Peso { get; set; }
        public string Piezas { get; set; }
        public string Promedio { get; set; }
        public string Barco { get; set; }
        public decimal Lomos { get; set; }
        public decimal Migas { get; set; }
        public decimal Recuperado { get; set; }
        public string Cliente { get; set; }
        public DateTime? Fecha { get; set; }

        public bool Equals(OrdenFabricacionAvance other)
        {
            if (OrdenFabricacion == other.OrdenFabricacion && Fecha == other.Fecha)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hashOrdenFabricacion = OrdenFabricacion.GetHashCode();
            int hashFecha = Fecha == null ? 0 : Fecha.GetHashCode();

            return hashOrdenFabricacion ^ hashFecha;
        }
    }
}