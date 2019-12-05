using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models
{
    public class OrdenFabricacionAutoclave
    {
        public string ORDEN_FABRICACION { get; set; }
        public string OrdenVenta { get; set; }
        public string CodigoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string LineaNegocio { get; set; }
        public string Envase { get; set; }
        public string FormaEnvasado { get; set; }
        public string PesoNeto { get; set; }
    }
}