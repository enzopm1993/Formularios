using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.HigieneComedorCocina
{
    public class clsDHigieneComedorCocina
    {
        public List<CC_HIGIENE_C_C_MANT_OBJETOS> ConsultarObjetos() {
            using ( ASIS_PRODEntities db =new ASIS_PRODEntities())
            {
                var lista = db.CC_HIGIENE_C_C_MANT_OBJETOS.ToList();
                List<CC_HIGIENE_C_C_MANT_OBJETOS> listaObjeto = new List<CC_HIGIENE_C_C_MANT_OBJETOS>();
                CC_HIGIENE_C_C_MANT_OBJETOS objeto;
                foreach (var item in lista)
                {
                    objeto = new CC_HIGIENE_C_C_MANT_OBJETOS();
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

        public List<CC_HIGIENE_C_C_MANT_OBJETOS> ConsultarObjetosActivos(string estadoRegistro)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_HIGIENE_C_C_MANT_OBJETOS.Where(x=> x.EstadoRegistro==estadoRegistro).ToList();
                List<CC_HIGIENE_C_C_MANT_OBJETOS> listaObjeto = new List<CC_HIGIENE_C_C_MANT_OBJETOS>();
                CC_HIGIENE_C_C_MANT_OBJETOS objeto;
                foreach (var item in lista)
                {
                    objeto = new CC_HIGIENE_C_C_MANT_OBJETOS();
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

        public List<CC_HIGIENE_C_C_MANT_INTERMEDIA> ConsultarIntermediaActivos(int idAuditoria)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var lista = db.CC_HIGIENE_C_C_MANT_INTERMEDIA.Where(x => x.IdAuditoria == idAuditoria && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).ToList();
                List<CC_HIGIENE_C_C_MANT_INTERMEDIA> listaObjeto = new List<CC_HIGIENE_C_C_MANT_INTERMEDIA>();
                CC_HIGIENE_C_C_MANT_INTERMEDIA objeto;
                foreach (var item in lista)
                {
                    objeto = new CC_HIGIENE_C_C_MANT_INTERMEDIA();
                    objeto.IdObjeto = item.IdObjeto;
                    objeto.IdMantenimiento = item.IdMantenimiento;
                    objeto.IdAuditoria = item.IdAuditoria;
                    objeto.EstadoRegistro = item.EstadoRegistro;
                    listaObjeto.Add(objeto);
                }
                return listaObjeto;
            }
        }

        public int GuardarModificarObjeto(CC_HIGIENE_C_C_MANT_OBJETOS guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_HIGIENE_C_C_MANT_OBJETOS.FirstOrDefault(x => x.IdObjeto == guardarModificar.IdObjeto);
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
                    db.CC_HIGIENE_C_C_MANT_OBJETOS.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarObjeto(CC_HIGIENE_C_C_MANT_OBJETOS guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_HIGIENE_C_C_MANT_OBJETOS.FirstOrDefault(x => x.IdObjeto == guardarModificar.IdObjeto);
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

        public List<CC_HIGIENE_C_C_MANT_AREA_AUDITORIA> ConsultarAreaAuditoria()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities()) {
                var lista = db.CC_HIGIENE_C_C_MANT_AREA_AUDITORIA.ToList();
                List<CC_HIGIENE_C_C_MANT_AREA_AUDITORIA> listaArea = new List<CC_HIGIENE_C_C_MANT_AREA_AUDITORIA>();
                CC_HIGIENE_C_C_MANT_AREA_AUDITORIA objArea;
                foreach (var item in lista) {
                    objArea = new CC_HIGIENE_C_C_MANT_AREA_AUDITORIA();
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

        public int GuardarModificarAreaAuditoria(CC_HIGIENE_C_C_MANT_AREA_AUDITORIA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_HIGIENE_C_C_MANT_AREA_AUDITORIA.FirstOrDefault(x => x.IdAuditoria == guardarModificar.IdAuditoria);
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
                    db.CC_HIGIENE_C_C_MANT_AREA_AUDITORIA.Add(guardarModificar); 
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarAreaAuditoria(CC_HIGIENE_C_C_MANT_AREA_AUDITORIA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_HIGIENE_C_C_MANT_AREA_AUDITORIA.FirstOrDefault(x => x.IdAuditoria == guardarModificar.IdAuditoria);
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

        public int GuardarModificarIntermedia(CC_HIGIENE_C_C_MANT_INTERMEDIA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_HIGIENE_C_C_MANT_INTERMEDIA.FirstOrDefault(x => x.IdAuditoria == guardarModificar.IdAuditoria && x.IdObjeto==guardarModificar.IdObjeto && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
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
                    db.CC_HIGIENE_C_C_MANT_INTERMEDIA.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }

        public int EliminarIntermedia(CC_HIGIENE_C_C_MANT_INTERMEDIA guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CC_HIGIENE_C_C_MANT_INTERMEDIA.FirstOrDefault(x => x.IdMantenimiento == guardarModificar.IdMantenimiento);
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
    }
}