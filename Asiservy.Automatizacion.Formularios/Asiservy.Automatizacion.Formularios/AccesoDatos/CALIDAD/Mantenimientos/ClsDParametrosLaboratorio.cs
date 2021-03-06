﻿using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.Mantenimientos
{
    public class ClsDParametrosLaboratorio
    {
        public List<dynamic> ConsultarMantenimiento(string estadoRegistro = null)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (estadoRegistro == null)
                {

                    var lista = (from par in db.CC_PARAMETROS_LABORATORIO
                                 join clasif in db.CLASIFICADOR on new { par.CodFormClasif, clsAtributos.codPrecoccion } equals new { CodFormClasif= clasif.Codigo, codPrecoccion=clasif.Grupo }
                                 from  clasd in db.CLASIFICADOR.Where(v=> v.Codigo==par.CodArea && v.Grupo==clsAtributos.codArea && v.EstadoRegistro==clsAtributos.EstadoRegistroActivo).DefaultIfEmpty()
                                 orderby par.FechaIngresoLog descending
                                 select new
                                 {
                                     par.CodFormClasif,
                                     par.CodArea,                                     
                                     par.DescripcionParametro,
                                     par.EstadoRegistro,
                                     par.FechaIngresoLog,
                                     par.FechaModificacionLog,
                                     par.IdParametro,
                                     par.NombreParametro,
                                     par.TerminalIngresoLog,
                                     par.TerminalModificacionLog,
                                     par.Mascara,
                                     par.ValorMax,
                                     par.UsuarioIngresoLog,
                                     par.ValorMin,
                                     clasif.Descripcion,
                                     descripcionArea = clasd.Descripcion
                                 });
                    return lista.ToList<dynamic>();

                }
                else
                {
                    var listaActivos = (from par in db.CC_PARAMETROS_LABORATORIO
                                        join clasif in db.CLASIFICADOR on new { par.CodFormClasif, par.EstadoRegistro, clsAtributos.codPrecoccion } equals new { CodFormClasif = clasif.Codigo, EstadoRegistro = clsAtributos.EstadoRegistroActivo, codPrecoccion = clasif.Grupo }
                                        from clasd in db.CLASIFICADOR.Where(v => v.Codigo == par.CodArea && v.Grupo == clsAtributos.codArea && v.EstadoRegistro == clsAtributos.EstadoRegistroActivo).DefaultIfEmpty()
                                        orderby par.FechaIngresoLog descending
                                        select new
                                        {
                                     par.CodFormClasif,
                                     par.CodArea,
                                     par.DescripcionParametro,
                                     par.EstadoRegistro,
                                     par.FechaIngresoLog,
                                     par.FechaModificacionLog,
                                     par.IdParametro,
                                     par.NombreParametro,
                                     par.TerminalIngresoLog,
                                     par.TerminalModificacionLog,
                                     par.Mascara,
                                     par.ValorMax,
                                     par.UsuarioIngresoLog,
                                     par.ValorMin,
                                     clasif.Descripcion,
                                     descripcionArea= clasd.Descripcion
                                        });
                    return listaActivos.ToList<dynamic>();
                }

            }
        }
        public int GuardarModificarMantenimiento(CC_PARAMETROS_LABORATORIO guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_PARAMETROS_LABORATORIO.FirstOrDefault(x => x.NombreParametro.Replace(" ", string.Empty).ToUpper() == guardarModificar.NombreParametro.Replace(" ", string.Empty).ToUpper() 
                                                                                        && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo &&x.CodFormClasif==guardarModificar.CodFormClasif
                                                                                        && x.CodArea==guardarModificar.CodArea);
                if (validarNombreRepetido != null && guardarModificar.IdParametro != validarNombreRepetido.IdParametro)
                {
                    valor = 3;
                    return valor;
                }

                var model = db.CC_PARAMETROS_LABORATORIO.FirstOrDefault(x => x.IdParametro == guardarModificar.IdParametro);
                if (model != null)
                {
                    if (model.EstadoRegistro == "I")
                    {
                        valor = 2;
                        return valor;
                    }
                    else
                    {
                        model.CodArea = guardarModificar.CodArea;
                        model.Mascara = guardarModificar.Mascara;
                        model.NombreParametro = guardarModificar.NombreParametro.ToUpper();
                        model.CodFormClasif = guardarModificar.CodFormClasif;
                        model.DescripcionParametro = guardarModificar.DescripcionParametro;
                        model.ValorMax = guardarModificar.ValorMax;
                        model.ValorMin = guardarModificar.ValorMin;
                        model.FechaModificacionLog = guardarModificar.FechaIngresoLog;
                        model.TerminalModificacionLog = guardarModificar.TerminalIngresoLog;
                        model.UsuarioModificacionLog = guardarModificar.UsuarioIngresoLog;
                        valor = 1;
                    }
                }
                else
                {
                    guardarModificar.NombreParametro = guardarModificar.NombreParametro.ToUpper();
                    db.CC_PARAMETROS_LABORATORIO.Add(guardarModificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int EliminarMantenimiento(CC_PARAMETROS_LABORATORIO guardarModificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_PARAMETROS_LABORATORIO.FirstOrDefault(x => x.NombreParametro.Replace(" ", string.Empty).ToUpper() == guardarModificar.NombreParametro.Replace(" ", string.Empty).ToUpper()
                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && x.CodFormClasif==guardarModificar.CodFormClasif && x.CodArea == guardarModificar.CodArea);
                if (validarNombreRepetido != null && guardarModificar.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                {
                    valor = 2;
                    return valor;
                }

                var model = db.CC_PARAMETROS_LABORATORIO.FirstOrDefault(x => x.IdParametro == guardarModificar.IdParametro);
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
        public List<CC_PARAMETROS_LABORATORIO> ConsultarParametrosFormularios(string CodFormulario)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.CC_PARAMETROS_LABORATORIO.Where(x => x.CodFormClasif == CodFormulario).ToList();
            }
        }
        public List<CC_PARAMETROS_LABORATORIO> ConsultarParametrosxArea(string Area)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.CC_PARAMETROS_LABORATORIO.Where(x => x.CodArea == Area).ToList();
            }
        }
    }
}