using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.HigieneComedorCocina
{
    public class clsDHigieneComedorCocina
    {
        public List<CC_HIGIENE_COMEDOR_COCINA_MANT> ConsultaHigieneMant()
        {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                var lista = db.CC_HIGIENE_COMEDOR_COCINA_MANT.ToList();
                return lista;
            }
        }

        public int GuardarModificarMant(CC_HIGIENE_COMEDOR_COCINA_MANT guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_HIGIENE_COMEDOR_COCINA_MANT.FirstOrDefault(x => x.IdMantenimiento == guardarModificar.IdMantenimiento);
                if (model != null)
                {
                    model.Nombre = guardarModificar.Nombre;
                    model.Categoria = guardarModificar.Categoria;
                    model.Observacion = guardarModificar.Observacion;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_HIGIENE_COMEDOR_COCINA_MANT.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarMant(CC_HIGIENE_COMEDOR_COCINA_MANT guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_HIGIENE_COMEDOR_COCINA_MANT.FirstOrDefault(x => x.IdMantenimiento == guardarModificar.IdMantenimiento);
                if (model != null)
                {
                    model.EstadoRegistro = guardarModificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }             
                return valor;
            }
        }
    }
}