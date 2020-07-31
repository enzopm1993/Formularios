using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.Mantenimientos
{
    public class ClsDMantenimientoTemperatura
    {
        public List<CC_MANTENIMIENTO_TEMPERATURA> ConsultarRegistro()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_MANTENIMIENTO_TEMPERATURA.AsNoTracking().ToList();              
                return lista;
            }
        }
        public int GuardarModificarRegistro(CC_MANTENIMIENTO_TEMPERATURA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_MANTENIMIENTO_TEMPERATURA.FirstOrDefault(x => x.CodFormulario == guardarModificar.CodFormulario && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (validarNombreRepetido != null && validarNombreRepetido.IdTemperaturaMant != guardarModificar.IdTemperaturaMant)
                {
                    valor = 3;
                    return valor;
                }
                var model = db.CC_MANTENIMIENTO_TEMPERATURA.FirstOrDefault(x => x.IdTemperaturaMant == guardarModificar.IdTemperaturaMant);
                if (model != null)
                {
                    if (model.EstadoRegistro == "I")
                    {
                        valor = 2;
                        return valor;
                    }
                    else
                    {
                        model.CodFormulario = guardarModificar.CodFormulario;
                        model.Descripcion = guardarModificar.Descripcion;
                        model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                        model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                        model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                        valor = 1;
                    }
                }
                else
                {
                    db.CC_MANTENIMIENTO_TEMPERATURA.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int EliminarRegistro(CC_MANTENIMIENTO_TEMPERATURA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_MANTENIMIENTO_TEMPERATURA.FirstOrDefault(x => x.CodFormulario == guardarModificar.CodFormulario && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (validarNombreRepetido != null && guardarModificar.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                {
                    valor = 2;
                    return valor;
                }
                var model = db.CC_MANTENIMIENTO_TEMPERATURA.FirstOrDefault(x => x.IdTemperaturaMant == guardarModificar.IdTemperaturaMant);
                if (model != null)
                {
                    model.EstadoRegistro = guardarModificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    db.SaveChanges();
                    valor = 1;
                }
                return valor;
            }
        }
        //public int ConsultarRegistroActivoID(int id)
        //{
        //    using (ASIS_PRODEntities db = new ASIS_PRODEntities())
        //    {
        //        var lista = db.CC_COCINAMIENTO_MANT.Where(x => x.IdMantCocinamiento == id && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).Count();
        //        return lista;
        //    }
        //}
    }
}