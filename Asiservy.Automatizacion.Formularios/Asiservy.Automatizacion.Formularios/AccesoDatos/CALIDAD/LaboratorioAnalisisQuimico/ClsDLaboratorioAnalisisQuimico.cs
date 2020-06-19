using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.LaboratorioAnalisisQuimico
{
    public class ClsDLaboratorioAnalisisQuimico
    {
        public List<CC_ANALISIS_QUIMICO_PRECOCCION_TURNO> ConsultarMantenimiento(string estadoRegistro = null)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (estadoRegistro == null)
                {
                    var lista = db.CC_ANALISIS_QUIMICO_PRECOCCION_TURNO.ToList();
                    return lista;
                }
                else
                {
                    var listaVerivicacion = db.CC_ANALISIS_QUIMICO_PRECOCCION_TURNO.Where(v => v.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();
                    return listaVerivicacion;
                }               
            }
        }
        public int GuardarModificarMantenimiento(CC_ANALISIS_QUIMICO_PRECOCCION_TURNO guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_ANALISIS_QUIMICO_PRECOCCION_TURNO.FirstOrDefault(x => x.Nombre.Replace(" ", string.Empty).ToUpper() == guardarmodificar.Nombre.Replace(" ", string.Empty).ToUpper() && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (validarNombreRepetido != null && guardarmodificar.IdTurno != validarNombreRepetido.IdTurno)
                {
                    valor = 3;
                    return valor;
                }

                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_TURNO.FirstOrDefault(x => x.IdTurno == guardarmodificar.IdTurno);
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
                        model.DescripcionMant = guardarmodificar.DescripcionMant;
                        model.FechaModificacionLog = guardarmodificar.FechaIngresoLog;
                        model.TerminalModificacionLog = guardarmodificar.TerminalIngresoLog;
                        model.UsuarioModificacionLog = guardarmodificar.UsuarioIngresoLog;
                        valor = 1;
                    }
                }
                else
                {
                    db.CC_ANALISIS_QUIMICO_PRECOCCION_TURNO.Add(guardarmodificar);
                }
                db.SaveChanges();
                return valor;
            }
        }
        public int EliminarMantenimiento(CC_ANALISIS_QUIMICO_PRECOCCION_TURNO guardarmodificar)
        {
            int valor = 0;
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validarNombreRepetido = db.CC_ANALISIS_QUIMICO_PRECOCCION_TURNO.FirstOrDefault(x => x.Nombre.Replace(" ", string.Empty).ToUpper() == guardarmodificar.Nombre.Replace(" ", string.Empty).ToUpper()
                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (validarNombreRepetido != null && guardarmodificar.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                {
                    valor = 2;
                    return valor;
                }

                var model = db.CC_ANALISIS_QUIMICO_PRECOCCION_TURNO.FirstOrDefault(x => x.IdTurno == guardarmodificar.IdTurno);
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
    }
}