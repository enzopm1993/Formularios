using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.Models.Seguridad
{
    public class ConsultaOpcionesxRolViewModel : IEquatable<ConsultaOpcionesxRolViewModel>
    {
        public int? IdOpcion { get; set; }
        public string Nombre { get; set; }
        public string Formulario { get; set; }
        public string Clase { get; set; }
        public Nullable<int> Padre { get; set; }
        public string Url { get; set; }
        public int? Orden { get; set; }
        public int? IdModulo { get; set; }

        #region IEquatable<ConsultaOpcionesxRolViewModel> Members

        public bool Equals(ConsultaOpcionesxRolViewModel other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (Object.ReferenceEquals(this, other)) return true;

            return this.IdOpcion.Equals(other.IdOpcion);
        }


        public override int GetHashCode()
        {
            int hashDescription = this.IdOpcion == null ? 0 : this.IdOpcion.GetHashCode();

            return hashDescription;
        }

        #endregion
    }
}