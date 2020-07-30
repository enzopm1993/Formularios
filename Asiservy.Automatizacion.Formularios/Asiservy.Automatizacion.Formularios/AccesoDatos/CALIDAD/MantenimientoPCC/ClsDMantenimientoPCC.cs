using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MantenimientoPCC
{
    public class ClsDMantenimientoPCC
    {
        public List<CC_PCC_MANTENIMIENTO> ConsultarRegistro()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_PCC_MANTENIMIENTO.AsNoTracking().ToList();
                //List<CC_PCC_MANTENIMIENTO> listaObjeto = new List<CC_PCC_MANTENIMIENTO>();
                //CC_PCC_MANTENIMIENTO objeto;
                //foreach (var item in lista)
                //{
                //    objeto = new CC_PCC_MANTENIMIENTO();
                //    objeto.IdObjeto = item.IdObjeto;
                //    objeto.NombreObjeto = item.NombreObjeto;
                //    objeto.DescripcionObjeto = item.DescripcionObjeto;
                //    objeto.UsuarioIngresoLog = item.UsuarioIngresoLog;
                //    objeto.FechaIngresoLog = item.FechaIngresoLog;
                //    objeto.EstadoRegistro = item.EstadoRegistro;
                //    listaObjeto.Add(objeto);
                //}
                return lista;
            }
        }
        public int GuardarModificarRegistro(CC_PCC_MANTENIMIENTO guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_PCC_MANTENIMIENTO.FirstOrDefault(x => x.Numero == guardarModificar.Numero && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
                if (validarNombreRepetido!=null && validarNombreRepetido.IdPcc != guardarModificar.IdPcc)
                {
                    valor = 3;
                    return valor;
                }
                var model = db.CC_PCC_MANTENIMIENTO.FirstOrDefault(x => x.IdPcc == guardarModificar.IdPcc);
                if (model != null)
                {
                    if (model.EstadoRegistro == "I")
                    {
                        valor = 2;
                        return valor;
                    }
                    else
                    {
                        model.Numero = guardarModificar.Numero;
                        model.DescripcionMant = guardarModificar.DescripcionMant;
                        model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                        model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                        model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                        valor = 1;
                    }
                }
                else
                {
                    db.CC_PCC_MANTENIMIENTO.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int EliminarRegistro(CC_PCC_MANTENIMIENTO guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_PCC_MANTENIMIENTO.FirstOrDefault(x => x.Numero == guardarModificar.Numero && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (validarNombreRepetido != null && guardarModificar.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                {
                    valor = 2;
                    return valor;
                }
                var model = db.CC_PCC_MANTENIMIENTO.FirstOrDefault(x => x.IdPcc == guardarModificar.IdPcc);
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
        public int ConsultarRegistroActivoID(int id)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_PCC_MANTENIMIENTO.Where(x => x.IdPcc == id && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).Count();
                return lista;
            }
        }
    }
}