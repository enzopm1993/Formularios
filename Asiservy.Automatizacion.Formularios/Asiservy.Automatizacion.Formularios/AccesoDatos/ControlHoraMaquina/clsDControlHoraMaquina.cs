using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ControlHoraMaquina
{
    public class clsDControlHoraMaquina
    {
        public void GuardarModificarControlHoraMaquina(CONTROL_HORA_MAQUINA model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {              
                var control = entities.CONTROL_HORA_MAQUINA.FirstOrDefault(x => x.IdControlHoraMaquina == model.IdControlHoraMaquina || (x.OrdenFabricacion== model.OrdenFabricacion && x.EstadoRegistro ==clsAtributos.EstadoRegistroActivo));
                if (control != null)
                {
                    control.EstadoRegistro = model.EstadoRegistro;
                    control.FechaModificacionLog = DateTime.Now;
                    control.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    control.TerminalModificacionLog = model.TerminalIngresoLog;
                }
                else
                {
                    entities.CONTROL_HORA_MAQUINA.Add(model); 
                }
                entities.SaveChanges();
            }
        }

        public List<ControlHorasMaquina> ConsultarControlHoraMaquina (DateTime Fecha, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                if (Turno == "0")
                {
                    var result = (from x in entities.CONTROL_HORA_MAQUINA
                                  where x.Fecha == Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                  select new ControlHorasMaquina
                                  {
                                    CONTROL_HORA_MAQUINA_DETALLE=x.CONTROL_HORA_MAQUINA_DETALLE.ToList(),
                                    Cliente= x.Cliente,
                                    Turno=x.Turno,
                                    CodigoProducto=x.CodigoProducto,
                                    EstadoRegistro=x.EstadoRegistro,
                                    Fecha=x.Fecha,
                                    FechaIngresoLog=x.FechaIngresoLog,
                                    FechaModificacionLog=x.FechaModificacionLog,
                                    IdControlHoraMaquina=x.IdControlHoraMaquina,
                                     LineaNegocio=x.LineaNegocio,
                                     OrdenFabricacion=x.OrdenFabricacion,
                                     OrdenVenta=x.OrdenVenta,
                                     PesoNeto=x.PesoNeto,
                                     Producto=x.Producto,
                                     TerminalIngresoLog=x.TerminalIngresoLog,
                                     TerminalModificacionLog=x.TerminalModificacionLog,
                                     UsuarioIngresoLog=x.UsuarioIngresoLog,
                                     UsuarioModificacionLog=x.UsuarioModificacionLog
                                  }
                                 ).ToList();


                    return result;
                }
                else
                {
                    var result = (from x in entities.CONTROL_HORA_MAQUINA
                                  where x.Fecha == Fecha 
                                  && x.Turno == Turno
                                  && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                  select new ControlHorasMaquina
                                  {
                                      CONTROL_HORA_MAQUINA_DETALLE = x.CONTROL_HORA_MAQUINA_DETALLE.ToList(),
                                      Cliente = x.Cliente,
                                      Turno = x.Turno,
                                      CodigoProducto = x.CodigoProducto,
                                      EstadoRegistro = x.EstadoRegistro,
                                      Fecha = x.Fecha,
                                      FechaIngresoLog = x.FechaIngresoLog,
                                      FechaModificacionLog = x.FechaModificacionLog,
                                      IdControlHoraMaquina = x.IdControlHoraMaquina,
                                      LineaNegocio = x.LineaNegocio,
                                      OrdenFabricacion = x.OrdenFabricacion,
                                      OrdenVenta = x.OrdenVenta,
                                      PesoNeto = x.PesoNeto,
                                      Producto = x.Producto,
                                      TerminalIngresoLog = x.TerminalIngresoLog,
                                      TerminalModificacionLog = x.TerminalModificacionLog,
                                      UsuarioIngresoLog = x.UsuarioIngresoLog,
                                      UsuarioModificacionLog = x.UsuarioModificacionLog
                                  }
                                ).ToList();
                    return result;
                }
            }
        }


        public List<spConsultaControlHoraMaquina> ConsultarControlAutoclave(DateTime Fecha, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                if (Turno == "0")
                {
                    return entities.spConsultaControlHoraMaquina(Fecha).ToList();
                }
                else
                {
                    return entities.spConsultaControlHoraMaquina(Fecha).Where(x => x.Turno == Turno).ToList();
                }
            }
        }

        public List<spConsultaControlHoraMaquinaDetalle> ConsultaControlHoraMaquinaDetalle(int IdControl)
        {
           
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {              
                    return entities.spConsultaControlHoraMaquinaDetalle(IdControl).ToList();               
            }
        }


        public void GuardarModificarControlHoraMaquinaDetalle(CONTROL_HORA_MAQUINA_DETALLE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var detalle = entities.CONTROL_HORA_MAQUINA_DETALLE.FirstOrDefault(x => x.IdControlHoraMaquina == model.IdControlHoraMaquina && x.Autoclave == model.Autoclave);
                if(detalle != null)
                {
                    if (model.EstadoRegistro == clsAtributos.EstadoRegistroActivo)
                    {
                        detalle.FechaInicio = model.FechaInicio;
                        detalle.FechaFin = model.FechaFin;
                        detalle.TotalCoches = model.TotalCoches;
                        detalle.TotalHoras = model.TotalHoras;
                    }
                    detalle.FechaModificacionLog = model.FechaIngresoLog;
                    detalle.TerminalModificacionLog = model.TerminalIngresoLog;
                    detalle.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    detalle.EstadoRegistro = model.EstadoRegistro;
                }
                else
                {
                    entities.CONTROL_HORA_MAQUINA_DETALLE.Add(model);
                }
                entities.SaveChanges();


            }
        }


    }
}