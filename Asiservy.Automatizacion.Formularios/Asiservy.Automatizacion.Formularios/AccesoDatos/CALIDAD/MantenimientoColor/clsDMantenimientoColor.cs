using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MantenimientoColor
{
    public class clsDMantenimientoColor
    {
        public List<CC_MANTENIMIENTO_COLOR> ConsultarMantenimientoColor()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.CC_MANTENIMIENTO_COLOR.ToList();
                return listado;
            }
        }

        public int GuardarModificarMantenimientoColor(CC_MANTENIMIENTO_COLOR GuardarModigicar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_MANTENIMIENTO_COLOR.FirstOrDefault(x => x.IdColor == GuardarModigicar.IdColor);
                if (model != null)
                {
                    model.Descripcion = GuardarModigicar.Descripcion;
                    model.EstadoRegistro = GuardarModigicar.EstadoRegistro;
                    model.FechaModificacionLog = GuardarModigicar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModigicar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModigicar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_MANTENIMIENTO_COLOR.Add(GuardarModigicar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarMantenimientoColor(CC_MANTENIMIENTO_COLOR GuardarModigicar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_MANTENIMIENTO_COLOR.FirstOrDefault(x => x.IdColor == GuardarModigicar.IdColor);
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