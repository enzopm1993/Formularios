using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
namespace Asiservy.Automatizacion.Formularios.Models.Seguridad
{
    public class RolViewModel
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public string Estado { get; set; }
        public List<ROL> ListaRoles { get; set; }

    }
}