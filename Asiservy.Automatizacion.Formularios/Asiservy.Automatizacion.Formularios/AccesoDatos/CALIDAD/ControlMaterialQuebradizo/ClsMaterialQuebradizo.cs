using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.ControlMaterialQuebradizo
{
    public class ClsMaterialQuebradizo
    {
        public List<CC_MATERIAL_QUEBRADIZO_MANT> ConsultarMantenimiento(string estadoRegistro = null, string verificacion=null)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (estadoRegistro == null && verificacion==null)
                {
                    var lista = db.CC_MATERIAL_QUEBRADIZO_MANT.ToList();
                    return lista;
                }
                else if(!string.IsNullOrEmpty(verificacion)){ 
                    var listaVerivicacion = db.CC_MATERIAL_QUEBRADIZO_MANT.Where(v => v.EstadoRegistro == clsAtributos.EstadoRegistroActivo && v.TipoVerificacion.ToUpper()==verificacion.ToUpper()).ToList();
                    return listaVerivicacion;
                }
                else
                {
                    var listaVerivicacionTodos = db.CC_MATERIAL_QUEBRADIZO_MANT.Where(v => v.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                    return listaVerivicacionTodos;
                }
            }
        }
        public int GuardarModificarMantenimiento(CC_MATERIAL_QUEBRADIZO_MANT guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_MATERIAL_QUEBRADIZO_MANT.FirstOrDefault(x => x.Nombre.Replace(" ", string.Empty).ToUpper() == guardarmodificar.Nombre.Replace(" ", string.Empty).ToUpper() && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (validarNombreRepetido != null && guardarmodificar.IdMantenimiento != validarNombreRepetido.IdMantenimiento)
                {
                    valor = 3;
                    return valor;
                }

                var model = db.CC_MATERIAL_QUEBRADIZO_MANT.FirstOrDefault(x => x.IdMantenimiento == guardarmodificar.IdMantenimiento);
                if (model != null)
                {

                    if (model.EstadoRegistro == "I")
                    {
                        valor = 2;
                        return valor;
                    }
                    else
                    {
                        model.Nombre = guardarmodificar.Nombre;
                        model.TipoVerificacion = guardarmodificar.TipoVerificacion;
                        model.Descripcion = guardarmodificar.Descripcion;
                        model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                        model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                        model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                        valor = 1;
                    }
                }
                else
                {
                    db.CC_MATERIAL_QUEBRADIZO_MANT.Add(guardarmodificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int EliminarMantenimiento(CC_MATERIAL_QUEBRADIZO_MANT guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_MATERIAL_QUEBRADIZO_MANT.FirstOrDefault(x => x.Nombre.Replace(" ", string.Empty).ToUpper() == guardarmodificar.Nombre.Replace(" ", string.Empty).ToUpper()
                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (validarNombreRepetido != null && guardarmodificar.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                {
                    valor = 2;
                    return valor;
                }

                var model = db.CC_MATERIAL_QUEBRADIZO_MANT.FirstOrDefault(x => x.IdMantenimiento == guardarmodificar.IdMantenimiento);
                if (model != null)
                {
                    model.EstadoRegistro = guardarmodificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }
        public List<CC_MATERIAL_QUEBRADIZO_MANT_MATERIAL> ConsultarMantenimientoMaterial(string estadoRegistro = null)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (estadoRegistro == null)
                {
                    var lista = db.CC_MATERIAL_QUEBRADIZO_MANT_MATERIAL.ToList();
                    return lista;
                }
                else
                {
                    var listaActivos = db.CC_MATERIAL_QUEBRADIZO_MANT_MATERIAL.Where(v => v.EstadoRegistro == estadoRegistro).ToList();
                    return listaActivos;
                }
            }
        }
        public int GuardarModificarMantenimientoMaterial(CC_MATERIAL_QUEBRADIZO_MANT_MATERIAL guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_MATERIAL_QUEBRADIZO_MANT_MATERIAL.FirstOrDefault(x => x.Nombre.Replace(" ", string.Empty).ToUpper() == guardarmodificar.Nombre.Replace(" ", string.Empty).ToUpper() && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (validarNombreRepetido != null && guardarmodificar.IdMantMaterial != validarNombreRepetido.IdMantMaterial)
                {
                    valor = 3;
                    return valor;
                }

                var model = db.CC_MATERIAL_QUEBRADIZO_MANT_MATERIAL.FirstOrDefault(x => x.IdMantMaterial == guardarmodificar.IdMantMaterial);
                if (model != null)
                {

                    if (model.EstadoRegistro == "I")
                    {
                        valor = 2;
                        return valor;
                    }
                    else
                    {
                        model.Nombre = guardarmodificar.Nombre;
                        model.Orden = guardarmodificar.Orden;
                        model.DescripcionMant = guardarmodificar.DescripcionMant;
                        model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                        model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                        model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                        valor = 1;
                    }
                }
                else
                {
                    db.CC_MATERIAL_QUEBRADIZO_MANT_MATERIAL.Add(guardarmodificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int EliminarMantenimientoMaterial(CC_MATERIAL_QUEBRADIZO_MANT_MATERIAL guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_MATERIAL_QUEBRADIZO_MANT_MATERIAL.FirstOrDefault(x => x.Nombre.Replace(" ", string.Empty).ToUpper() == guardarmodificar.Nombre.Replace(" ", string.Empty).ToUpper()
                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (validarNombreRepetido != null && guardarmodificar.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                {
                    valor = 2;
                    return valor;
                }

                var model = db.CC_MATERIAL_QUEBRADIZO_MANT_MATERIAL.FirstOrDefault(x => x.IdMantMaterial == guardarmodificar.IdMantMaterial);
                if (model != null)
                {
                    model.EstadoRegistro = guardarmodificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                    valor = 1;
                    db.SaveChanges();
                }
                return valor;
            }
        }
        public int GuardarModificarMaterialQuebradizo(CC_MATERIAL_QUEBRADIZO_CTRL guardarModificar, int siAprobar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_MATERIAL_QUEBRADIZO_CTRL.FirstOrDefault(x => x.Fecha == guardarModificar.Fecha && x.Turno == guardarModificar.Turno && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (siAprobar != 1 && validarNombreRepetido != null && guardarModificar.IdMaterial != validarNombreRepetido.IdMaterial)
                {
                    valor = 5;
                    return valor;
                }

                var model = db.CC_MATERIAL_QUEBRADIZO_CTRL.FirstOrDefault(x => x.IdMaterial == guardarModificar.IdMaterial && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    if (siAprobar == 1)
                    {
                        model.EstadoReporte = guardarModificar.EstadoReporte;
                        model.AprobadoPor = guardarModificar.UsuarioIngresoLog;
                        model.FechaAprobado = guardarModificar.FechaAprobado;
                        valor = 2;//APRROBADO
                    }
                    else
                    {
                        if (guardarModificar.Fecha != DateTime.MinValue)
                        {
                            model.Turno = guardarModificar.Turno;
                            model.Fecha = guardarModificar.Fecha;
                            model.ObservacionCtrl = guardarModificar.ObservacionCtrl;
                            valor = 1;//ACTUALIZAR
                        }
                        else valor = 3;//ERROR DE FECHA
                    }
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                }
                else
                {
                    if (guardarModificar.Fecha != DateTime.MinValue)
                    {
                        db.CC_MATERIAL_QUEBRADIZO_CTRL.Add(guardarModificar);
                    }
                    else valor = 3;
                }
                db.SaveChanges();
                return valor;
            }
        }
        public CC_MATERIAL_QUEBRADIZO_CTRL ConsultarEstadoReporte(int idMaterial)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                CC_MATERIAL_QUEBRADIZO_CTRL listado;
                listado = db.CC_MATERIAL_QUEBRADIZO_CTRL.FirstOrDefault(x => x.IdMaterial == idMaterial && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);

                CC_MATERIAL_QUEBRADIZO_CTRL cabecera;
                if (listado != null)
                {
                    cabecera = new CC_MATERIAL_QUEBRADIZO_CTRL();
                    cabecera.IdMaterial = listado.IdMaterial;
                    cabecera.Fecha = listado.Fecha;
                    cabecera.ObservacionCtrl = listado.ObservacionCtrl;
                    cabecera.EstadoReporte = listado.EstadoReporte;
                    cabecera.FechaIngresoLog = listado.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = listado.UsuarioIngresoLog;
                    cabecera.FechaAprobado = listado.FechaAprobado;
                    cabecera.AprobadoPor = listado.AprobadoPor;
                    return cabecera;
                }
                return listado;
            }
        }
        public CC_MATERIAL_QUEBRADIZO_CTRL ConsultarCabeceraTurno(string turno, DateTime fechaControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                CC_MATERIAL_QUEBRADIZO_CTRL listado;

                if (turno == "0")
                {
                    listado = db.CC_MATERIAL_QUEBRADIZO_CTRL.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                   && x.Fecha.Day == fechaControl.Day
                                                                                   && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else
                {
                    listado = db.CC_MATERIAL_QUEBRADIZO_CTRL.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                        && x.Fecha.Day == fechaControl.Day && x.Turno == turno
                                                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                CC_MATERIAL_QUEBRADIZO_CTRL cabecera;
                if (listado != null)
                {
                    cabecera = new CC_MATERIAL_QUEBRADIZO_CTRL();
                    cabecera.IdMaterial = listado.IdMaterial;
                    cabecera.Fecha = listado.Fecha;
                    cabecera.ObservacionCtrl = listado.ObservacionCtrl;
                    cabecera.EstadoReporte = listado.EstadoReporte;
                    cabecera.FechaIngresoLog = listado.FechaIngresoLog;
                    cabecera.UsuarioIngresoLog = listado.UsuarioIngresoLog;
                    cabecera.FechaAprobado = listado.FechaAprobado;
                    cabecera.AprobadoPor = listado.AprobadoPor;
                    cabecera.Turno = listado.Turno;
                    return cabecera;
                }
                return listado;

            }
        }
        public int EliminarMaterialQuebradizo(CC_MATERIAL_QUEBRADIZO_CTRL guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_MATERIAL_QUEBRADIZO_CTRL.FirstOrDefault(x => x.IdMaterial == guardarModificar.IdMaterial);
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
        public int GuardarModificarDetalle(CC_MATERIAL_QUEBRADIZO_DET guardarModificar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                
                var model = db.CC_MATERIAL_QUEBRADIZO_DET.FirstOrDefault(x => x.IdMaterialDetalle == guardarModificar.IdMaterialDetalle && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.IdMantMaterial = guardarModificar.IdMantMaterial;
                    model.IdMantenimiento = guardarModificar.IdMantenimiento;
                    model.EstadoVerificacion = guardarModificar.EstadoVerificacion;
                    model.Observaciones = guardarModificar.Observaciones;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;//ACTUALIZAR                   
                }
                else
                {
                    db.CC_MATERIAL_QUEBRADIZO_DET.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int EliminarDetalle(CC_MATERIAL_QUEBRADIZO_DET guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_MATERIAL_QUEBRADIZO_DET.Where(x => x.IdMaterialDetalle == guardarmodificar.IdMaterialDetalle && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    foreach(var item in model) {
                        item.EstadoRegistro = guardarmodificar.EstadoRegistro;
                        item.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                        item.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                        item.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                        valor = 1;                       
                    }
                    db.SaveChanges();
                }
                return valor;
            }
        }
        public List<sp_Material_Quebradizo_Calidad> ConsultarDetalle(int idMaterial, int op, int idArea=0)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listaDetalleDia = db.sp_Material_Quebradizo_Calidad(idMaterial,idArea, op).ToList();
                return listaDetalleDia;
            }
        }
        public int GuardarModificarAccionCorrectiva(CC_MATERIAL_QUEBRADIZO_ACCI_CORRECTIVA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_MATERIAL_QUEBRADIZO_ACCI_CORRECTIVA.FirstOrDefault(x => x.IdAccion == guardarModificar.IdAccion && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.DescripcionAccion = guardarModificar.DescripcionAccion;
                    if (!string.IsNullOrEmpty(guardarModificar.RutaFoto))
                        model.RutaFoto = guardarModificar.RutaFoto;
                    model.Rotation = guardarModificar.Rotation;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;
                    
                }
                else
                {
                    db.CC_MATERIAL_QUEBRADIZO_ACCI_CORRECTIVA.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int EliminarAccionCorrectiva(CC_MATERIAL_QUEBRADIZO_ACCI_CORRECTIVA guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_MATERIAL_QUEBRADIZO_ACCI_CORRECTIVA.Where(x => x.IdAccion == guardarmodificar.IdAccion && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        item.EstadoRegistro = guardarmodificar.EstadoRegistro;
                        item.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                        item.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                        item.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                        valor = 1;
                    }
                    db.SaveChanges();
                }
                return valor;
            }
        }
        public List<CC_MATERIAL_QUEBRADIZO_CTRL> ConsultarBadejaEstado(DateTime fechaDesde, DateTime FechaHasta, bool estadoReporte)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<CC_MATERIAL_QUEBRADIZO_CTRL> listado;
                if (estadoReporte)
                {
                    listado = db.CC_MATERIAL_QUEBRADIZO_CTRL.Where(x => x.EstadoReporte == estadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                              && x.Fecha >= fechaDesde && x.Fecha <= FechaHasta).OrderByDescending(v => v.Fecha).ToList();
                }
                else
                {
                    listado = db.CC_MATERIAL_QUEBRADIZO_CTRL.Where(x => x.EstadoReporte == estadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                               && x.Fecha <= FechaHasta).OrderByDescending(v => v.Fecha).ToList();
                }
                CC_MATERIAL_QUEBRADIZO_CTRL cabecera;
                List<CC_MATERIAL_QUEBRADIZO_CTRL> listaCabecera = new List<CC_MATERIAL_QUEBRADIZO_CTRL>();
                if (listado.Any())
                {
                    foreach (var item in listado)
                    {
                        cabecera = new CC_MATERIAL_QUEBRADIZO_CTRL();
                        cabecera.IdMaterial = item.IdMaterial;
                        cabecera.Fecha = item.Fecha;
                        cabecera.ObservacionCtrl = item.ObservacionCtrl;
                        cabecera.EstadoReporte = item.EstadoReporte;
                        cabecera.FechaIngresoLog = item.FechaIngresoLog;
                        cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                        cabecera.FechaAprobado = item.FechaAprobado;
                        cabecera.AprobadoPor = item.AprobadoPor;
                        cabecera.Turno = item.Turno;
                        listaCabecera.Add(cabecera);
                    }
                }
                return listaCabecera;
            }
        }
        public List<CC_MATERIAL_QUEBRADIZO_CTRL> ConsultarReporteRangoFecha(DateTime fechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.CC_MATERIAL_QUEBRADIZO_CTRL.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                           && x.Fecha >= fechaDesde && x.Fecha <= FechaHasta).OrderByDescending(c => c.Fecha).ToList();

                CC_MATERIAL_QUEBRADIZO_CTRL cabecera;
                List<CC_MATERIAL_QUEBRADIZO_CTRL> listaCabecera = new List<CC_MATERIAL_QUEBRADIZO_CTRL>();
                if (listado.Any())
                {
                    foreach (var item in listado)
                    {
                        cabecera = new CC_MATERIAL_QUEBRADIZO_CTRL();
                        cabecera.IdMaterial = item.IdMaterial;
                        cabecera.Fecha = item.Fecha;
                        cabecera.ObservacionCtrl = item.ObservacionCtrl;
                        cabecera.EstadoReporte = item.EstadoReporte;
                        cabecera.FechaIngresoLog = item.FechaIngresoLog;
                        cabecera.UsuarioIngresoLog = item.UsuarioIngresoLog;
                        cabecera.UsuarioModificacionLog = item.UsuarioModificacionLog;
                        cabecera.FechaModificacionLog = item.FechaModificacionLog;
                        cabecera.FechaAprobado = item.FechaAprobado;
                        cabecera.AprobadoPor = item.AprobadoPor;
                        cabecera.Turno = item.Turno;
                        listaCabecera.Add(cabecera);
                    }
                }
                return listaCabecera;
            }
        }
    }
}