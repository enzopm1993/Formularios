using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;


namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ControlHueso
{
    public class clsDControlHueso
    {

        public string GuardarModificarControlHueso(CONTROL_HUESO_DETALLE detalle)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONTROL_HUESO_DETALLE.FirstOrDefault(x=>x.IdControlHuesoDetalle == detalle.IdControlHuesoDetalle);
                if(result!=null)
                {
                    result.CantidadHueso = detalle.CantidadHueso;
                    result.UsuarioModificacionLog = detalle.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = detalle.TerminalIngresoLog;
                    entities.SaveChanges();
                }

                return clsAtributos.MsjRegistroGuardado;
            }
        }

        public List<spConsultaControlHuesoDetalle> ConsultaControlHuesoDetalle(int id)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.spConsultaControlHuesoDetalle(id).ToList();
                return result;
            }
        }
        public List<spConsultaControlHueso> ConsultaControlHueso(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.spConsultaControlHueso(Fecha).ToList();
                return result;
            }
        }

        public int ValidaControlHueso(CONTROL_HUESO doControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var FechaActual = DateTime.Now.Date;
                var ControlHueso = entities.CONTROL_HUESO.FirstOrDefault(x =>
                x.Linea == doControl.Linea              
               // && x.Hora == doControl.Hora
                && x.Fecha == FechaActual);
                if (ControlHueso != null)
                    return ControlHueso.IdControlHueso;
                else
                    return 0;
            }
        }

        public void InactivarControlHueso(CONTROL_HUESO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var ControlHueso = entities.CONTROL_HUESO.FirstOrDefault(x =>
              x.IdControlHueso == model.IdControlHueso);

                ControlHueso.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                ControlHueso.FechaModificacionLog = model.FechaIngresoLog;
                ControlHueso.TerminalModificacionLog = model.TerminalIngresoLog;
                ControlHueso.UsuarioModificacionLog = model.UsuarioIngresoLog;

                entities.SaveChanges();
            }

        }

        public int GenerarControlHueso(CONTROL_HUESO doControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<spConsultaLimpiadorasControlHueso> detalle = new List<spConsultaLimpiadorasControlHueso>();
                var FechaActual = DateTime.Now.Date;
                
                var ControlHueso = entities.CONTROL_HUESO.FirstOrDefault(x =>
                x.Linea == doControl.Linea
              
                && x.HoraFin == doControl.HoraFin
                && x.HoraInicio == doControl.HoraInicio
               
                && x.Fecha == doControl.Fecha
                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (ControlHueso == null)
                {
                    if (doControl.TipoControlHueso == clsAtributos.Hueso || doControl.TipoControlHueso == clsAtributos.Roto)
                    {
                        detalle = ConsultaLimpiadorasControlHueso(doControl.Linea, doControl.Fecha);
                        foreach (var x in detalle)
                        {
                            doControl.CONTROL_HUESO_DETALLE.Add(new CONTROL_HUESO_DETALLE
                            {
                                CantidadHueso = 0,
                                Cedula = x.CEDULA,
                                EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                                FechaIngresoLog = DateTime.Now,
                                UsuarioIngresoLog = doControl.UsuarioIngresoLog,
                                TerminalIngresoLog = doControl.TerminalIngresoLog
                            });
                        }
                    }
                    entities.CONTROL_HUESO.Add(doControl);
                    entities.SaveChanges();
                }
                else
                {
                    return 0;
                }

                var idControlHueso = entities.CONTROL_HUESO.FirstOrDefault(x =>
                   x.Linea == doControl.Linea
                   && x.Lote == doControl.Lote
                   && x.HoraInicio == doControl.HoraInicio
                   && x.HoraFin == doControl.HoraFin
                   && x.Fecha == doControl.Fecha);
                return idControlHueso.IdControlHueso;
            }
        }   

       

        public List<spConsultaLimpiadorasControlHueso> ConsultaLimpiadorasControlHueso(string Linea, DateTime Fecha)
        {
            using(ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<spConsultaLimpiadorasControlHueso> Listado = new List<spConsultaLimpiadorasControlHueso>();
                Listado = entities.spConsultaLimpiadorasControlHueso(Linea, Fecha).ToList();
                return Listado;
            }
        }

        public List<spConsultaControlAvanceDiarioPorLinea> ConsultaControlAvanceDiarioPorLinea(DateTime Fecha, string Linea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<spConsultaControlAvanceDiarioPorLinea> Listado = new List<spConsultaControlAvanceDiarioPorLinea>();
                Listado = entities.spConsultaControlAvanceDiarioPorLinea(Fecha,Linea).ToList();
                return Listado;
            }

        }

        public List<spConsultaAvanceDiarioPorLimpiadora> ConsultaControlAvanceDiarioPorLimpiadora(DateTime Fecha, string Linea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<spConsultaAvanceDiarioPorLimpiadora> Listado = new List<spConsultaAvanceDiarioPorLimpiadora>();
                Listado = entities.spConsultaAvanceDiarioPorLimpiadora(Fecha, Linea).ToList();
                return Listado;
            }

        }
    }
}