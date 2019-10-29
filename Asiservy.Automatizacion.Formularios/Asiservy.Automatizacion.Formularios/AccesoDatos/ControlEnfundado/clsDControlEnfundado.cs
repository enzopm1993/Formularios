using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ControlEnfundado
{
    public class clsDControlEnfundado
    {
        clsDEmpleado clsDEmpleado = null;

        public string GuardarModificarControlEnfundado(CONTROL_ENFUNDADO_DETALLE detalle)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.CONTROL_ENFUNDADO_DETALLE.FirstOrDefault(x => x.IdControlEnfundadoDetalle == detalle.IdControlEnfundadoDetalle);
                if (result != null)
                {
                    result.Fundas = detalle.Fundas;
                    result.UsuarioModificacionLog = detalle.UsuarioIngresoLog;
                    result.FechaModificacionLog = DateTime.Now;
                    result.TerminalModificacionLog = detalle.TerminalIngresoLog;
                    entities.SaveChanges();
                }

                return clsAtributos.MsjRegistroGuardado;
            }
        }


        public int GenerarControlHueso(CONTROL_ENFUNDADO doControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                clsDEmpleado = new clsDEmpleado();
                //List<spConsultaLimpiadorasControlHueso> detalle = new List<spConsultaLimpiadorasControlHueso>();
               // var FechaActual = DateTime.Now.Date;
               
                var ControlEnfundado = entities.CONTROL_ENFUNDADO.FirstOrDefault(x =>   
                x.Hora == doControl.Hora &&
                x.Fecha == doControl.Fecha
                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (ControlEnfundado == null)
                {
                  
                        var empleados = clsDEmpleado.ConsultaEmpleadosFiltro(null,null,clsAtributos.CargoEmpacado);
                        foreach (var x in empleados)
                        {
                            doControl.CONTROL_ENFUNDADO_DETALLE.Add(new CONTROL_ENFUNDADO_DETALLE
                            {
                                Fundas = 0,                                                            
                                Cedula = x.CEDULA,
                                EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                                FechaIngresoLog = DateTime.Now,
                                UsuarioIngresoLog = doControl.UsuarioIngresoLog,
                                TerminalIngresoLog = doControl.TerminalIngresoLog
                            });
                        }
                    entities.CONTROL_ENFUNDADO.Add(doControl);
                    entities.SaveChanges();                }
                else
                {
                    return 0;
                }

                var idControlEnfundado = entities.CONTROL_ENFUNDADO.FirstOrDefault(x =>
                x.Hora == doControl.Hora 
                && x.Fecha == doControl.Fecha
                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);

                return idControlEnfundado.IdControlEnfundado;
            }
        }

        public List<spConsultaControlEnfundadoDetalle> ConsultaControlEnfundadoDetalle(int id)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.spConsultaControlEnfundadoDetalle(id).ToList();
                return result;
            }
        }
        public List<spConsultaControlEnfundado> ConsultaControlEnfundado(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var result = entities.spConsultaControlEnfundado(Fecha).ToList();
                return result;
            }
        }
    }
}