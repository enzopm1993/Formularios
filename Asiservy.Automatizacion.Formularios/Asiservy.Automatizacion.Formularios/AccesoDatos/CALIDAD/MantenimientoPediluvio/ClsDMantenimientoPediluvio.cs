using Asiservy.Automatizacion.Datos.Datos;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MantenimientoPediluvio
{
    public class ClsDMantenimientoPediluvio
    {
        public List<CC_MANTENIMIENTO_PEDILUVIO> ConsultarMantenimientoPediluvio()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.CC_MANTENIMIENTO_PEDILUVIO.ToList();
                return listado;
            }
        }

        public int GuardarModificarMantenimientoPediluvio(CC_MANTENIMIENTO_PEDILUVIO GuardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_MANTENIMIENTO_PEDILUVIO.FirstOrDefault(x => x.IdMantenimientoPediluvio == GuardarModificar.IdMantenimientoPediluvio);
                if (model != null)
                {
                    model.Descripcion = GuardarModificar.Descripcion;
                    model.Area = GuardarModificar.Area;
                    model.EstadoRegistro = GuardarModificar.EstadoRegistro;
                    model.FechaModificacionLog = GuardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_MANTENIMIENTO_PEDILUVIO.Add(GuardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarMantenimientoPediluvio(CC_MANTENIMIENTO_PEDILUVIO GuardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_MANTENIMIENTO_PEDILUVIO.FirstOrDefault(x => x.IdMantenimientoPediluvio == GuardarModificar.IdMantenimientoPediluvio);
                if (model != null)
                {
                    model.EstadoRegistro = GuardarModificar.EstadoRegistro;
                    model.FechaModificacionLog = GuardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = GuardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = GuardarModificar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }
    }
}