using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.LimpiezaDesinfeccionPlanta
{
    public class clsDLimpiezaDesinfeccionPlanta
    {
        public List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS> ConsultarObjetos() {
            using ( ASIS_PRODEntities db =new ASIS_PRODEntities())
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
                var lista = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS.Where(x=> x.EstadoRegistro==estadoRegistro).ToList();
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
                var lista = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_OBJETOS.Where(x => x.IdObjeto == idObjeto && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).Count();                
                return lista;
            }
        }

        public List<CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA> ConsultarIntermediaActivos(int idAuditoria)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA.Where(x => x.IdAuditoria == idAuditoria && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();
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

        public int GuardarModificarAreaAuditoria(CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_AREA_AUDITADA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
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
                var model = db.CC_LIMPIEZA_DESINFECCION_PLANTA_MANT_INTERMEDIA.FirstOrDefault(x => x.IdAuditoria == guardarModificar.IdAuditoria && x.IdObjeto==guardarModificar.IdObjeto && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
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
        public object[] GuardarCabeceraControl(CC_LIMPIEZA_DESINFECION_PLANTA_CABECERA poCabControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscaeControl = db.CC_LIMPIEZA_DESINFECION_PLANTA_CABECERA.Where(x => x.Fecha == poCabControl.Fecha &&
                x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (buscaeControl == null)
                {
                    db.CC_LIMPIEZA_DESINFECION_PLANTA_CABECERA.Add(poCabControl);
                    db.SaveChanges();
                    resultado[0] = "000";
                    resultado[1] = "Registro ingresado con éxito";
                    resultado[2] = poCabControl;
                }
                else
                {
                    resultado[0] = "002";
                    resultado[1] = "Error, el registro ya existe";
                    resultado[2] = poCabControl;
                }
                return resultado;

            }
        }
        public CC_LIMPIEZA_DESINFECION_PLANTA_CABECERA ConsultarCabecera(DateTime Fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.CC_LIMPIEZA_DESINFECION_PLANTA_CABECERA.Where(x => x.Fecha == Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
            }
        }
        public object[] ActualizarCabeceraControl(CC_LIMPIEZA_DESINFECION_PLANTA_CABECERA poCabControl)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarcabecera = db.CC_LIMPIEZA_DESINFECION_PLANTA_CABECERA.Find(poCabControl.IdLimpiezaDesinfeccionPlanta);
                if (buscarcabecera.EstadoControl==true)
                {
                    resultado[0] = "003";
                    resultado[1] = "No se pudo actualizar el control, debido a que ya se encuentra aprobado";
                    resultado[2] = poCabControl;
                    return resultado;
                }
                else
                {
                    //buscarcabecera.FirmaControl = poCabControl.FirmaControl;
                    buscarcabecera.UsuarioModificacionLog = poCabControl.UsuarioIngresoLog;
                    buscarcabecera.TerminalModificacionLog = poCabControl.TerminalIngresoLog;
                    buscarcabecera.FechaModificacionLog = DateTime.Now;
                    resultado[0] = "002";
                    resultado[1] = "Registro actualizado con éxito";
                    resultado[2] = poCabControl;
                    return resultado;
                }
            }

        }
      
    }
}