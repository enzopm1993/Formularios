using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.LimpiezaDesinfeccionPlanta
{
    public class clsDLimpiezaDesinfeccionPlanta
    {
        public List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS> ConsultarObjetos() {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS.ToList();
                List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS> listaObjeto = new List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS>();
                CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS objeto;
                foreach (var item in lista)
                {
                    objeto = new CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS();
                    objeto.IdObjeto = item.IdObjeto;
                    objeto.NombreObjeto = item.NombreObjeto;
                    objeto.DescripcionObjeto = item.DescripcionObjeto;
                    objeto.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    objeto.FechaIngresoLog = item.FechaIngresoLog;
                    objeto.EstadoRegistro = item.EstadoRegistro;
                    listaObjeto.Add(objeto);
                }
                return listaObjeto;
            }
        }

        public List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS> ConsultarObjetosActivos(string estadoRegistro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS.Where(x => x.EstadoRegistro == estadoRegistro).OrderBy(v=>v.NombreObjeto).ToList();
                List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS> listaObjeto = new List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS>();
                CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS objeto;
                foreach (var item in lista)
                {
                    objeto = new CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS();
                    objeto.IdObjeto = item.IdObjeto;
                    objeto.NombreObjeto = item.NombreObjeto;
                    objeto.DescripcionObjeto = item.DescripcionObjeto;
                    objeto.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    objeto.FechaIngresoLog = item.FechaIngresoLog;
                    objeto.EstadoRegistro = item.EstadoRegistro;
                    listaObjeto.Add(objeto);
                }
                return listaObjeto;
            }
        }

        public int ConsultarObjetosActivosID(int idObjeto)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS.Where(x => x.IdObjeto == idObjeto && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).Count();
                return lista;
            }
        }

        public List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA> ConsultarIntermediaActivos(int idAuditoria)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA.Where(x => x.IdAuditoria == idAuditoria && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA> listaObjeto = new List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA>();
                CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA objeto;
                foreach (var item in lista)
                {
                    objeto = new CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA();
                    objeto.IdObjeto = item.IdObjeto;
                    objeto.IdMantenimiento = item.IdMantenimiento;
                    objeto.IdAuditoria = item.IdAuditoria;
                    objeto.EstadoRegistro = item.EstadoRegistro;
                    listaObjeto.Add(objeto);
                }
                return listaObjeto;
            }
        }

        public int GuardarModificarObjeto(CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS.FirstOrDefault(x => x.NombreObjeto.Replace(" ", string.Empty).ToUpper() == guardarModificar.NombreObjeto.Replace(" ", string.Empty).ToUpper());
                if (validarNombreRepetido != null && guardarModificar.NombreObjeto.Replace(" ", string.Empty).ToUpper() == validarNombreRepetido.NombreObjeto.Replace(" ", string.Empty).ToUpper()
                   && guardarModificar.IdObjeto != validarNombreRepetido.IdObjeto)
                {
                    valor = 3;
                    return valor;
                }

                var model = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS.FirstOrDefault(x => x.IdObjeto == guardarModificar.IdObjeto);
                if (model != null)
                {

                    if (model.EstadoRegistro == "I")
                    {
                        valor = 2;
                        return valor;
                    }
                    else
                    {
                        model.NombreObjeto = guardarModificar.NombreObjeto;
                        model.DescripcionObjeto = guardarModificar.DescripcionObjeto;
                        model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                        model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                        model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                        valor = 1;
                    }
                }
                else
                {
                    db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarObjeto(CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS.FirstOrDefault(x => x.IdObjeto == guardarModificar.IdObjeto);
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

        public List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA> ConsultarAreaAuditoria()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities()) {
                var lista = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA.ToList();
                List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA> listaArea = new List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA>();
                CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA objArea;
                foreach (var item in lista) {
                    objArea = new CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA();
                    objArea.IdAuditoria = item.IdAuditoria;
                    objArea.NombreAuditoria = item.NombreAuditoria;
                    objArea.DescripcionAuditoria = item.DescripcionAuditoria;
                    objArea.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    objArea.FechaIngresoLog = item.FechaIngresoLog;
                    objArea.EstadoRegistro = item.EstadoRegistro;
                    listaArea.Add(objArea);
                }
                return listaArea;
            }
        }

        public List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA> ConsultarAreaAuditoriaActivos(string estadoRegistro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA.Where(x => x.EstadoRegistro == estadoRegistro).OrderBy(x => x.NombreAuditoria).ToList();
                List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA> listaArea = new List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA>();
                CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA objArea;
                foreach (var item in lista)
                {
                    objArea = new CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA();
                    objArea.IdAuditoria = item.IdAuditoria;
                    objArea.NombreAuditoria = item.NombreAuditoria.ToUpper();
                    objArea.DescripcionAuditoria = item.DescripcionAuditoria;
                    objArea.UsuarioIngresoLog = item.UsuarioIngresoLog;
                    objArea.FechaIngresoLog = item.FechaIngresoLog;
                    objArea.EstadoRegistro = item.EstadoRegistro;
                    listaArea.Add(objArea);
                }
                return listaArea;
            }
        }

        public int GuardarModificarAreaAuditoria(CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA.FirstOrDefault(x => x.NombreAuditoria.Replace(" ", string.Empty).ToUpper() == guardarModificar.NombreAuditoria.Replace(" ", string.Empty).ToUpper());
                if (validarNombreRepetido != null && guardarModificar.NombreAuditoria.Replace(" ", string.Empty).ToUpper() == validarNombreRepetido.NombreAuditoria.Replace(" ", string.Empty).ToUpper()
                   && guardarModificar.IdAuditoria != validarNombreRepetido.IdAuditoria)
                {
                    valor = 6;
                    return valor;
                }
                var model = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA.FirstOrDefault(x => x.IdAuditoria == guardarModificar.IdAuditoria);
                if (model != null)
                {

                    if (model.EstadoRegistro == "I")
                    {
                        valor = 2;
                        return valor;
                    }
                    else
                    {
                        model.NombreAuditoria = guardarModificar.NombreAuditoria;
                        model.DescripcionAuditoria = guardarModificar.DescripcionAuditoria;
                        model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                        model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                        model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                        valor = 1;
                    }
                }
                else
                {
                    db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarAreaAuditoria(CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA.FirstOrDefault(x => x.IdAuditoria == guardarModificar.IdAuditoria);
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

        public int GuardarModificarIntermedia(CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA.FirstOrDefault(x => x.IdAuditoria == guardarModificar.IdAuditoria && x.IdObjeto == guardarModificar.IdObjeto && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.IdObjeto = guardarModificar.IdObjeto;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;
                }
                else
                {
                    db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarIntermedia(CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA.FirstOrDefault(x => x.IdMantenimiento == guardarModificar.IdMantenimiento);
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
        //-------------------------------------------------------------CONTROL----------------------------------------------------------------------------
        public int GuardarModificarLimpiezaCabecera(CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA guardarModificar, int siAprobar)
        {
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA.FirstOrDefault(x =>x.Fecha==guardarModificar.Fecha && x.Turno== guardarModificar.Turno && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
                if (siAprobar != 1 && validarNombreRepetido != null && guardarModificar.IdLimpiezaDesinfeccionPlanta != validarNombreRepetido.IdLimpiezaDesinfeccionPlanta)
                {
                    valor = 4;
                    return valor;
                }

                var model = db.CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA.FirstOrDefault(x => x.IdLimpiezaDesinfeccionPlanta == guardarModificar.IdLimpiezaDesinfeccionPlanta && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
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
                            model.ObservacionControl = guardarModificar.ObservacionControl;
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
                        db.CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA.Add(guardarModificar);
                    }
                    else valor = 3;
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarLimpiezaCabecera(CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA.FirstOrDefault(x => x.IdLimpiezaDesinfeccionPlanta == guardarModificar.IdLimpiezaDesinfeccionPlanta);
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

        public int GuardarModificarLimpiezaDetalle(CC_LIMPIEZA_DESINFECCION_PLANTA_DETALLE guardarModificar, int idAuditoria)
        {           
            int valor = 0;//GUARDDADO NUEVO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarHora = (from det in db.CC_LIMPIEZA_DESINFECCION_PLANTA_DETALLE
                                   join inter in db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA on det.IdMantenimiento equals inter.IdMantenimiento
                                   where (inter.IdAuditoria == idAuditoria && inter.EstadoRegistro == clsAtributos.EstadoRegistroActivo &&
                                   det.EstadoRegistro == clsAtributos.EstadoRegistroActivo && det.IdMantenimiento == guardarModificar.IdMantenimiento
                                   && det.HoraAuditoria == guardarModificar.HoraAuditoria)
                                   select new { det.HoraAuditoria, inter.IdMantenimiento, det.IdDetalle}).FirstOrDefault();
                if (validarHora != null && validarHora.IdDetalle != guardarModificar.IdDetalle)
                {
                    valor = 3;
                    return valor;
                }                

                var model = db.CC_LIMPIEZA_DESINFECCION_PLANTA_DETALLE.FirstOrDefault(x => x.IdDetalle == guardarModificar.IdDetalle && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    //model.Turno = guardarModificar.Turno;
                    model.HoraAuditoria = guardarModificar.HoraAuditoria;
                    model.Limpieza = guardarModificar.Limpieza;
                    model.Desinfeccion = guardarModificar.Desinfeccion;
                    model.ObservacionDetalle = guardarModificar.ObservacionDetalle;                   
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;//ACTUALIZAR                   
                }
                else
                {
                    db.CC_LIMPIEZA_DESINFECCION_PLANTA_DETALLE.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarLimpiezaDetalle(CC_LIMPIEZA_DESINFECCION_PLANTA_DETALLE guardarModificar)
        {
            int valor = 0;//ERROR ELIMINADO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_LIMPIEZA_DESINFECCION_PLANTA_DETALLE.FirstOrDefault(x => x.IdDetalle == guardarModificar.IdDetalle && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.EstadoRegistro = guardarModificar.EstadoRegistro;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;//ELIMINADO
                    db.SaveChanges();
                }
                return valor;
            }
        }

        public int GuardarModificarAccionCorrectiva(CC_LIMPIEZA_DESINFECCION_PLANTA_DETALLE guardarModificar)
        {
            int valor = 0;//ERROR ELIMINADO
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_LIMPIEZA_DESINFECCION_PLANTA_DETALLE.FirstOrDefault(x => x.IdDetalle == guardarModificar.IdDetalle && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (model != null)
                {
                    model.HoraAccionCorrectiva = guardarModificar.HoraAccionCorrectiva;
                    model.PersonaAccionCorrectiva = guardarModificar.PersonaAccionCorrectiva;
                    model.AccionCorrectiva = guardarModificar.AccionCorrectiva;
                    if (!string.IsNullOrEmpty(guardarModificar.RutaFoto))
                        model.RutaFoto = guardarModificar.RutaFoto;
                    model.Rotation = guardarModificar.Rotation;
                    model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                    model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                    model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                    valor = 1;//ELIMINADO
                    db.SaveChanges();
                }
                return valor;
            }
        }

        public CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA ConsultarEstadoReporte(int idLimpiezaDesinfeccionPlanta, DateTime fechaControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA listado;
                if (idLimpiezaDesinfeccionPlanta == 0 && fechaControl > DateTime.MinValue)
                {
                    listado = db.CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                        && x.Fecha.Day == fechaControl.Day
                                                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else
                {
                    listado = db.CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA.FirstOrDefault(x => x.IdLimpiezaDesinfeccionPlanta == idLimpiezaDesinfeccionPlanta && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA cabecera;
                if (listado != null)
                {
                    cabecera = new CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA();
                    cabecera.IdLimpiezaDesinfeccionPlanta = listado.IdLimpiezaDesinfeccionPlanta;
                    cabecera.Fecha = listado.Fecha;
                    cabecera.ObservacionControl = listado.ObservacionControl;
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

        public CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA ConsultarCabeceraTurno(int turno, DateTime fechaControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA listado;

                if (turno == 0)
                {
                    listado = db.CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                   && x.Fecha.Day == fechaControl.Day
                                                                                   && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                else
                {
                    listado = db.CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA.FirstOrDefault(x => x.Fecha.Year == fechaControl.Year && x.Fecha.Month == fechaControl.Month
                                                                                        && x.Fecha.Day == fechaControl.Day && x.Turno == turno
                                                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                }
                CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA cabecera;
                if (listado != null)
                {
                    cabecera = new CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA();
                    cabecera.IdLimpiezaDesinfeccionPlanta = listado.IdLimpiezaDesinfeccionPlanta;
                    cabecera.Fecha = listado.Fecha;
                    cabecera.ObservacionControl = listado.ObservacionControl;
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

        public CC_LIMPIEZA_DESINFECCION_PLANTA_DETALLE ConsultarHoraAuditoria(DateTime hora, int idMantenimiento)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.CC_LIMPIEZA_DESINFECCION_PLANTA_DETALLE.FirstOrDefault(x => x.IdMantenimiento == idMantenimiento && x.HoraAuditoria==hora);
                                
                return listado;
            }
        }

        public List<ListaIntermedia> ConsultarIntermediaJoinObjeto(int idAuditoria)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = (from inter in db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA
                             join obj in db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS on inter.IdObjeto equals obj.IdObjeto
                             where (inter.IdAuditoria == idAuditoria && inter.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                   && obj.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                             select new { inter.IdAuditoria, inter.IdObjeto, obj.NombreObjeto, inter.EstadoRegistro, inter.IdMantenimiento }).ToList();
                ListaIntermedia intermedia;
                List<ListaIntermedia> listaIntermedia = new List<ListaIntermedia>();
                if (lista.Count!=0)
                {                    
                    foreach (var item in lista)
                    {
                        intermedia = new ListaIntermedia();
                        intermedia.IdMantenimiento = item.IdMantenimiento;
                        intermedia.IdAuditoria = item.IdAuditoria;
                        intermedia.IdObjeto = item.IdObjeto;
                        intermedia.NombreObjeto = item.NombreObjeto;
                        intermedia.EstadoRegistro = item.EstadoRegistro;
                        listaIntermedia.Add(intermedia);
                    }
                }
                return listaIntermedia;
            }
        }       

        public List<sp_Limpieza_Desinfeccion_Planta> ConsultarJoinDetalle(int idLimpiezaDesinfeccionPlanta, int op, int idAuditoria)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.sp_Limpieza_Desinfeccion_Planta(idLimpiezaDesinfeccionPlanta, op, idAuditoria).ToList();
                return lista;
            }                 
        }

        #region BANDEJA
        public List<CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA> ConsultarBadejaEstado( DateTime fechaDesde, DateTime FechaHasta, bool estadoReporte)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA> listado;
                if(estadoReporte)
                {
                     listado = db.CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA.Where(x => x.EstadoReporte == estadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                               && x.Fecha >= fechaDesde && x.Fecha <= FechaHasta).ToList();
                }
                else
                {
                    listado = db.CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA.Where(x => x.EstadoReporte == estadoReporte && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                               && x.Fecha <= FechaHasta).ToList();
                }                   
                CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA cabecera;
                List<CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA> listaCabecera = new List<CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA>();
                if (listado.Any())
                {
                    foreach (var item in listado)
                    {
                        cabecera = new CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA();
                        cabecera.IdLimpiezaDesinfeccionPlanta = item.IdLimpiezaDesinfeccionPlanta;
                        cabecera.Fecha = item.Fecha;
                        cabecera.ObservacionControl = item.ObservacionControl;
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
        #endregion
        #region REPORTE
        public List<CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA> ConsultarReporteRangoFecha(DateTime fechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                                                           && x.Fecha >= fechaDesde && x.Fecha <= FechaHasta).OrderByDescending(c=> c.Fecha).ToList();

                CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA cabecera;
                List<CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA> listaCabecera = new List<CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA>();
                if (listado.Any())
                {
                    foreach (var item in listado)
                    {
                        cabecera = new CC_LIMPIEZA_DESINFECCION_PLANTA_CABECERA();
                        cabecera.IdLimpiezaDesinfeccionPlanta = item.IdLimpiezaDesinfeccionPlanta;
                        cabecera.Fecha = item.Fecha;
                        cabecera.ObservacionControl = item.ObservacionControl;
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
        #endregion
    }
    public class ListaIntermedia
    {
        public int IdMantenimiento { get; set; }
        public int IdAuditoria { get; set; }
        public int IdObjeto { get; set; }
        public string NombreObjeto { get; set; }
        public string EstadoRegistro { get; set; }
    }
}