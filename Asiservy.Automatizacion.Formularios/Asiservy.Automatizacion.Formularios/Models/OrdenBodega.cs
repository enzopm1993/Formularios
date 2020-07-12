

namespace Asiservy.Automatizacion.Formularios.Models
{
    public class OrdenBodega
    {
        public int UnidadesControlCalidad { get; set; }
        public int UnidadesRechazadas { get; set; }
        public int UnidadesReproceso { get; set; }
        public int UnidadesConDefecto { get; set; }
        public int CajasEntregadas { get; set; }
        public int LataSueltas { get; set; }

    }
}