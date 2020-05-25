using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MantenimientoMoretones
{
    public class ClsDMantenimientoMoretones
    {
        public List<CC_MANTENIMIENTO_MORETON> ConsultarMantenimientoMoretones()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.CC_MANTENIMIENTO_MORETON.ToList();
                return listado;
            }
        }

        public int GuardarModificarMantenimientoMoretones(CC_MANTENIMIENTO_MORETON GuardarModigicar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_MANTENIMIENTO_MORETON.FirstOrDefault(x => x.IdMoreton == GuardarModigicar.IdMoreton);
                if (model != null)
                {
                    model.Descripcion = GuardarModigicar.Descripcion;
                    model.Abreviatura = GuardarModigicar.Abreviatura;
                    model.EstadoRegistro = GuardarModigicar.EstadoRegistro;
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_MANTENIMIENTO_MORETON.Add(GuardarModigicar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarMantenimientoColor(CC_MANTENIMIENTO_MORETON GuardarModigicar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_MANTENIMIENTO_MORETON.FirstOrDefault(x => x.IdMoreton == GuardarModigicar.IdMoreton);
                if (model != null)
                {
                    model.EstadoRegistro = GuardarModigicar.EstadoRegistro;
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }
    }
}