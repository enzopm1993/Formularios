using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;


namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Empleado
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

        public List<spConsultaControlHueso> ConsultaControlHueso(int id)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.spConsultaControlHueso(id).ToList();
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
                && x.Hora == doControl.Hora
                && x.Fecha == FechaActual);
                if (ControlHueso != null)
                    return ControlHueso.IdControlHueso;
                else
                    return 0;
            }
        }

        public int GenerarControlHueso(CONTROL_HUESO doControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<spConsultaLimpiadorasControlHueso> detalle = new List<spConsultaLimpiadorasControlHueso>();
                var FechaActual = DateTime.Now.Date;
                doControl.Fecha = FechaActual;
                var ControlHueso = entities.CONTROL_HUESO.FirstOrDefault(x=> 
                x.Linea == doControl.Linea
                && x.Lote == doControl.Lote
                && x.Hora == doControl.Hora
                && x.Fecha ==FechaActual);
                if(ControlHueso== null)
                {
                    detalle = ConsultaLimpiadorasControlHueso(doControl.Linea);
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
                    entities.CONTROL_HUESO.Add(doControl);
                    entities.SaveChanges();
                }
                else
                {
                    return ControlHueso.IdControlHueso;
                }
              
                var idControlHueso = entities.CONTROL_HUESO.FirstOrDefault(x =>
                   x.Linea == doControl.Linea
                   && x.Lote == doControl.Lote
                   && x.Hora == doControl.Hora
                   && x.Fecha == FechaActual);
                return idControlHueso.IdControlHueso;
            }
        }   


        public List<spConsultaLimpiadorasControlHueso> ConsultaLimpiadorasControlHueso(string Linea)
        {
            using(ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<spConsultaLimpiadorasControlHueso> Listado = new List<spConsultaLimpiadorasControlHueso>();
                Listado = entities.spConsultaLimpiadorasControlHueso(Linea).ToList();
                return Listado;
            }
        }
    }
}