using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.PRODUCCION.EsterilizacionConserva
{
    public class clsDEsterilizacionConserva
    {
        public CABECERA_CONTROL_ESTERILIZACION_CONSERVAS ConsultarCabeceraEsterilizacionConserva(CABECERA_CONTROL_ESTERILIZACION_CONSERVAS poEsterilizacionConserva)
        {
            using (var db=new ASIS_PRODEntities())
            {
                return db.CABECERA_CONTROL_ESTERILIZACION_CONSERVAS.Where(x => x.Fecha == poEsterilizacionConserva.Fecha && x.Turno == poEsterilizacionConserva.Turno && x.TipoLinea== poEsterilizacionConserva.TipoLinea && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
            }
        }
        public CABECERA_CONTROL_ESTERILIZACION_CONSERVAS GuardarCabEsterilizacionConserva(CABECERA_CONTROL_ESTERILIZACION_CONSERVAS poEsterilizacionConserva)
        {
            using (var db = new ASIS_PRODEntities())
            {
                db.CABECERA_CONTROL_ESTERILIZACION_CONSERVAS.Add(poEsterilizacionConserva);
                db.SaveChanges();
                return poEsterilizacionConserva;
            }
        }

    }
}