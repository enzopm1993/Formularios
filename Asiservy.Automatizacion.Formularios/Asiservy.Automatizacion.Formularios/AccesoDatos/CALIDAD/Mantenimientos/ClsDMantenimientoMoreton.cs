using Asiservy.Automatizacion.Datos.Datos;
using System.Collections.Generic;
using System.Linq;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.Mantenimientos
{
    public class ClsDMantenimientoMoreton
    {
        public List<CC_MANTENIMIENTO_MORETON> ConsultaManteminetoMoreton()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.CC_MANTENIMIENTO_MORETON.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
            }
        }
    }
}