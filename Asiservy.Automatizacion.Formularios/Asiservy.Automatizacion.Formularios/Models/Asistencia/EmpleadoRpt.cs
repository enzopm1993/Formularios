using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Asistencia
{
    public class EmpleadoRpt : IEquatable<EmpleadoRpt>
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        #region IEquatable<ConsultaOpcionesxRolViewModel> Members

        public bool Equals(EmpleadoRpt other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (Object.ReferenceEquals(this, other)) return true;

            return this.Cedula.Equals(other.Cedula);
        }
        public override int GetHashCode()
        {
            int hashDescription = this.Cedula == null ? 0 : this.Cedula.GetHashCode();
            return hashDescription;
        }

        #endregion
    }

}