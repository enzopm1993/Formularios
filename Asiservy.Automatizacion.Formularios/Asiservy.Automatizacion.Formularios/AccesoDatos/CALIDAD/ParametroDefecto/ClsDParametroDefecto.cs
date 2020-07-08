using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.CALIDAD;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ParametroDefecto
{
    public class ClsDParametroDefecto
    {
        public object[] GuardarCabeceraControl(CC_PARAMETRO_DEFECTO_CABECERA poCabeceraControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarCabecera = db.CC_PARAMETRO_DEFECTO_CABECERA.Where(x => x.Formulario == poCabeceraControl.Formulario &&
               x.Tipo==poCabeceraControl.Tipo&&x.NivelLimpieza==poCabeceraControl.NivelLimpieza).FirstOrDefault();
                if (buscarCabecera == null)
                {
                    db.CC_PARAMETRO_DEFECTO_CABECERA.Add(poCabeceraControl);
                    db.SaveChanges();
                    resultado[0] = "000";
                    resultado[1] = "Registro ingresado con éxito";
                    resultado[2] = poCabeceraControl;
                }
                else
                {
                    resultado[0] = "002";
                    resultado[1] = "El registro ya existe, ¿desea actualizarlo?";
                    poCabeceraControl.IdParametroDefecto = buscarCabecera.IdParametroDefecto;
                    resultado[2] = poCabeceraControl;
                }
                return resultado;
            }
        }
        public object[] ActualizarCabecera(CC_PARAMETRO_DEFECTO_CABECERA poCabControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabecera = db.CC_PARAMETRO_DEFECTO_CABECERA.Find(poCabControl.IdParametroDefecto);
                BuscarCabecera.ColorDentroDeRango = poCabControl.ColorDentroDeRango;
                BuscarCabecera.ColorFueraDeRango = poCabControl.ColorFueraDeRango;
                BuscarCabecera.FechaModificacionLog = poCabControl.FechaIngresoLog;
                BuscarCabecera.UsuarioModificacionLog = poCabControl.UsuarioIngresoLog;
                BuscarCabecera.TerminalIngresoLog = poCabControl.TerminalIngresoLog;
                BuscarCabecera.EstadoRegistro = poCabControl.EstadoRegistro;
                db.SaveChanges();
                resultado[0] = "001";
                resultado[1] = "Registro actualizado con éxito";
                resultado[2] = poCabControl;
              
                return resultado;
            }
        }
        public List<ParametroDefectoViewModel> ConsultarCabecerasParametroDefecto()
        {
            using (var db = new ASIS_PRODEntities())
            {
                List<ParametroDefectoViewModel> resultado =
                (from r in db.CC_PARAMETRO_DEFECTO_CABECERA
                 join colordr in db.CLASIFICADOR on new
                 { r.ColorDentroDeRango, clsAtributos.EstadoRegistroActivo, Grupo = clsAtributos.CodGrupoColores } equals new { ColorDentroDeRango = colordr.Codigo, EstadoRegistroActivo = colordr.EstadoRegistro, colordr.Grupo }
                 join colorfr in db.CLASIFICADOR on new
                 { r.ColorFueraDeRango, clsAtributos.EstadoRegistroActivo, Grupo = clsAtributos.CodGrupoColores } equals new { ColorFueraDeRango = colorfr.Codigo, EstadoRegistroActivo = colorfr.EstadoRegistro, colorfr.Grupo }
                 join cf in db.CLASIFICADOR on new
                 { r.Formulario,clsAtributos.EstadoRegistroActivo, Grupo=clsAtributos.CodGrupoFormularios } equals new { Formulario=cf.Codigo, EstadoRegistroActivo=cf.EstadoRegistro,cf.Grupo }
                 join ct in db.CLASIFICADOR on new
                 { r.Tipo, clsAtributos.EstadoRegistroActivo, Grupo = clsAtributos.CodGrupoTipoProducto } equals new { Tipo = ct.Codigo, EstadoRegistroActivo = ct.EstadoRegistro, ct.Grupo }
                 join cn in db.CLASIFICADOR on new
                 { r.NivelLimpieza, clsAtributos.EstadoRegistroActivo, Grupo = clsAtributos.CodigoGrupoTipoLimpiezaPescado } equals new { NivelLimpieza = cn.Codigo, EstadoRegistroActivo = cn.EstadoRegistro, cn.Grupo }
                 where cf.Codigo != "0"&& cn.Codigo != "0"&& ct.Codigo != "0"
                 select new ParametroDefectoViewModel
                {
                    Tipo = r.Tipo,
                    TipoNombre=ct.Descripcion,
                    Formulario = r.Formulario,
                    FormularioNombre=cf.Descripcion,
                    ColorDentroDeRango = r.ColorDentroDeRango,
                    ColorFueraDeRango = r.ColorFueraDeRango,
                    EstadoRegistro = r.EstadoRegistro,
                    IdParametroDefecto = r.IdParametroDefecto,
                    NivelLimpieza = r.NivelLimpieza,
                    NivelLimpiezaNombre=cn.Descripcion,
                    ColorDentroDeRangoNombre=colordr.Descripcion,
                    ColorFueraDeRangoNombre=colorfr.Descripcion
                }).ToList();
                int? Total = 0;
                foreach (var item in resultado)
                {
                    Total = (from t in db.CC_PARAMETRO_DEFECTO_DETALLE
                             where t.IdCabeceraParametro == item.IdParametroDefecto
                             select t.Maximo).Sum();
                    item.Maximo = Total != null ? Total.Value : 0;
                }
                return resultado;
            }
        }
        public List<CC_MANTENIMIENTO_DEFECTO> ConsultarDefectos()
        {
            using (var db = new ASIS_PRODEntities())
            {
                return db.CC_MANTENIMIENTO_DEFECTO.ToList();
            }
        }
        public object[] GuardarMantDefecto(CC_MANTENIMIENTO_DEFECTO poDefecto)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarCabecera = db.CC_MANTENIMIENTO_DEFECTO.FirstOrDefault(x => x.Nombre == poDefecto.Nombre); 
                if (buscarCabecera == null)
                {
                    db.CC_MANTENIMIENTO_DEFECTO.Add(poDefecto);
                    db.SaveChanges();
                    resultado[0] = "000";
                    resultado[1] = "Registro ingresado con éxito";
                    resultado[2] = poDefecto;
                }
                else
                {
                    resultado[0] = "002";
                    resultado[1] = "Error, el defecto ya existe";
      
                    resultado[2] = poDefecto;
                }
                return resultado;
            }
        }
        public object[] ActualizarMantDefecto(CC_MANTENIMIENTO_DEFECTO poDefecto)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarDefecto = db.CC_MANTENIMIENTO_DEFECTO.Find(poDefecto.IdDefecto);
                buscarDefecto.Nombre = poDefecto.Nombre;
                buscarDefecto.EstadoRegistro = poDefecto.EstadoRegistro;
                buscarDefecto.FechaModificacionLog = poDefecto.FechaIngresoLog;
                buscarDefecto.UsuarioModificacionLog = poDefecto.UsuarioIngresoLog;
                buscarDefecto.TerminalIngresoLog = poDefecto.TerminalIngresoLog;
                buscarDefecto.EstadoRegistro = poDefecto.EstadoRegistro;
                db.SaveChanges();
                resultado[0] = "001";
                resultado[1] = "Registro actualizado con éxito";
                resultado[2] = poDefecto;

                return resultado;
            }
        }
        public object[] GuardarDefectoDetalle(CC_PARAMETRO_DEFECTO_DETALLE podefectodetalle)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarDetalle = db.CC_PARAMETRO_DEFECTO_DETALLE.Where(x => x.IdCabeceraParametro == podefectodetalle.IdCabeceraParametro &&
               x.Defecto == podefectodetalle.Defecto).FirstOrDefault();
                if (buscarDetalle == null)
                {
                    db.CC_PARAMETRO_DEFECTO_DETALLE.Add(podefectodetalle);
                    db.SaveChanges();
                    resultado[0] = "000";
                    resultado[1] = "Registro ingresado con éxito";
                    resultado[2] = podefectodetalle;
                }
                else
                {
                    resultado[0] = "002";
                    resultado[1] = "Error, el registro para ese defecto ya existe";
                    resultado[2] = podefectodetalle;
                }
                return resultado;
            }
        }
        public object[] ActualizarDefectoDetalle(CC_PARAMETRO_DEFECTO_DETALLE podefectodetalle)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscardetalle = db.CC_PARAMETRO_DEFECTO_DETALLE.Find(podefectodetalle.IdParametroDefectoDetalle);
                buscardetalle.Maximo = podefectodetalle.Maximo;
                
                buscardetalle.UsuarioModificacionLog = podefectodetalle.UsuarioIngresoLog;
                buscardetalle.TerminalIngresoLog = podefectodetalle.TerminalIngresoLog;
                buscardetalle.EstadoRegistro = podefectodetalle.EstadoRegistro;
                db.SaveChanges();
                resultado[0] = "001";
                resultado[1] = "Registro actualizado con éxito";
                resultado[2] = podefectodetalle;

                return resultado;
            }
        }
        public List<DetalleDefectoViewModel> ConsultarDetalleParametroDefecto(int IdCabecera)
        {
            using (var db = new ASIS_PRODEntities())
            {
                var resultado = (from d in db.CC_PARAMETRO_DEFECTO_DETALLE
                                 join def in db.CC_MANTENIMIENTO_DEFECTO on new {d.Defecto, EstadoRegistro=clsAtributos.EstadoRegistroActivo }
                                 equals new { Defecto=def.IdDefecto,def.EstadoRegistro }
                                 where d.IdCabeceraParametro == IdCabecera
                                 select new DetalleDefectoViewModel { Defecto = d.Defecto
                                 ,DefectoNombre=def.Nombre,
                                 EstadoRegistro=d.EstadoRegistro,
                                 IdCabeceraParametro=d.IdCabeceraParametro,
                                 IdParametroDefectoDetalle=d.IdParametroDefectoDetalle,
                                 Maximo=d.Maximo}
                               ).ToList();
                return resultado;
            }
        }
    }
}