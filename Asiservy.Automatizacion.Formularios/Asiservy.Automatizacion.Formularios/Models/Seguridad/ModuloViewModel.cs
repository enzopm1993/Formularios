using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Seguridad
{
    public class ModuloViewModel : IEquatable<ModuloViewModel>
    {
        public int? IdModulo { get; set; }
        public string NombreModulo{get;set;}
        public int? Orden { get; set; }

        #region IEquatable<ConsultaOpcionesxRolViewModel> Members

        public bool Equals(ModuloViewModel other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (Object.ReferenceEquals(this, other)) return true;

            return this.IdModulo.Equals(other.IdModulo);
        }


        public override int GetHashCode()
        {
            int hashDescription = this.IdModulo == null ? 0 : this.IdModulo.GetHashCode();

            return hashDescription;
        }

        #endregion
    }
}