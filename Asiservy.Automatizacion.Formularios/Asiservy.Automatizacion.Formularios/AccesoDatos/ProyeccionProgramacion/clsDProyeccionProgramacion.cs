using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.ProyeccionProgramacion;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ProyeccionProgramacion
{

    public class clsDProyeccionProgramacion
    {

        public List<spConsultaProyeccionProgramacion> ConsultaProyeccionProgramacionDetalle (int Id)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                List<spConsultaProyeccionProgramacion> Listado = new List<spConsultaProyeccionProgramacion>();
                Listado = db.spConsultaProyeccionProgramacion(Id).ToList();
                return Listado;
            }
        }
        public List<spConsultaProyeccionProgramacion> ConsultaProyeccionProgramacionReporte(DateTime fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var pro = db.PROYECCION_PROGRAMACION.FirstOrDefault(x => x.FechaProduccion == fecha);

                List<spConsultaProyeccionProgramacion> Listado = new List<spConsultaProyeccionProgramacion>();
                if(pro!=null)
                    Listado = db.spConsultaProyeccionProgramacion(pro.IdProyeccionProgramacion).ToList();
                return Listado;
            }
        }

        public PROYECCION_PROGRAMACION ConsultaProyeccionProgramacion(int Id)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {               
                var proyeccion = db.PROYECCION_PROGRAMACION.FirstOrDefault(x=> x.IdProyeccionProgramacion == Id);
                return proyeccion;
            }
        }
        public List<PROYECCION_PROGRAMACION> ConsultaProyeccionProgramacion(DateTime fechaDesde, DateTime fechaHasta)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                fechaHasta = fechaHasta.AddDays(1);
                var proyeccion = db.PROYECCION_PROGRAMACION.Where(x =>
                x.FechaProduccion >= fechaDesde
                && x.FechaProduccion < fechaHasta).ToList();

                return proyeccion;
            }
        }
        public int ValidarProyeccionProgramacion(DateTime Fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var proyeccion = db.PROYECCION_PROGRAMACION.FirstOrDefault(x => x.FechaProduccion == Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (proyeccion != null)
                {
                    return proyeccion.IdProyeccionProgramacion;
                }
                else
                {
                    return 0;
                }
            }
        }


        public int GenerarProyeccionProgramacion(PROYECCION_PROGRAMACION model)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var proyeccion = db.PROYECCION_PROGRAMACION.FirstOrDefault(x=> x.FechaProduccion == model.FechaProduccion && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if(proyeccion == null)
                {
                    db.PROYECCION_PROGRAMACION.Add(model);
                    db.SaveChanges();
                    return db.PROYECCION_PROGRAMACION.FirstOrDefault(x => x.FechaProduccion == model.FechaProduccion && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).IdProyeccionProgramacion;
                }
                else
                {
                    return proyeccion.IdProyeccionProgramacion;
                }
            }
        }


        public void EditarProyeccionProgramacion(int idProyeccion, bool? Ingreso, bool? EditaProduccion, bool? EditaPreparacion, bool?Finaliza, string usuario, string terminal, string Observacion=null)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                BITACORA_PROYECCION bitacora = new BITACORA_PROYECCION();
                var proyeccion = db.PROYECCION_PROGRAMACION.FirstOrDefault(x => x.IdProyeccionProgramacion == idProyeccion);
                if (proyeccion != null)
                {
                    if (Ingreso != null)
                    {                        
                        proyeccion.IngresoPreparacion = Ingreso?? proyeccion.IngresoPreparacion;
                        bitacora.Observacion +=proyeccion.IngresoPreparacion ? "Ingreso Activo- ":"Ingreso Finalizado- "; 
                    }
                    if (EditaProduccion != null)
                    {
                        proyeccion.EditaProduccion = EditaProduccion ?? proyeccion.EditaProduccion;
                        bitacora.Observacion += proyeccion.EditaProduccion ? "Edita Produccion Activo- " : "Edita Produccion Finalizado- ";

                    }
                    if (EditaPreparacion != null)
                    {
                        proyeccion.EditarPreparacion = EditaPreparacion ?? proyeccion.EditarPreparacion;
                        bitacora.Observacion += proyeccion.EditarPreparacion ? "Edita Preparacion Activo- " : "Edita Preparacion Finalizado- ";

                    }
                    if (Finaliza != null)
                    {
                        proyeccion.Finaliza = Finaliza ?? proyeccion.Finaliza;
                        proyeccion.Observacion = Observacion;
                        bitacora.Observacion += proyeccion.IngresoPreparacion ? "Proyeccion Activo " : "Proyeccion Finalizada ";

                    }
                    proyeccion.UsuarioModificacionLog = usuario;
                    proyeccion.TerminalModificacionLog = terminal;
                    proyeccion.FechaModificacionLog = DateTime.Now;

                    bitacora.IdProyeccionProgramacion = proyeccion.IdProyeccionProgramacion;
                    bitacora.UsuarioIngresoLog = usuario;
                    bitacora.TerminalIngresoLog = terminal;
                    bitacora.FechaIngresoLog = DateTime.Now;
                    bitacora.EstadoRegistro = clsAtributos.EstadoRegistroActivo;

                    db.BITACORA_PROYECCION.Add(bitacora);
                    db.SaveChanges();
                }
            }
        }

        

        public void GuardarModificarProyeccionProgramacionDetalle(PROYECCION_PROGRAMACION_DETALLE model, int proceso)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                //var proyeccion = db.PROYECCION_PROGRAMACION.FirstOrDefault(x => x.IdProyeccionProgramacion==model.IdProyeccionProgramacion);
                var detalle = db.PROYECCION_PROGRAMACION_DETALLE.FirstOrDefault(x => x.IdProyeccionProgramacionDetalle == model.IdProyeccionProgramacionDetalle);
                if (detalle != null)
                {
                    if (proceso == 1)
                    {
                        detalle.Lote = model.Lote.ToUpper();
                        detalle.Observacion = model.Observacion!=null?model.Observacion.ToUpper():"";
                        detalle.OrdenFabricacion = model.OrdenFabricacion;  
                        detalle.Toneladas = model.Toneladas;
                        detalle.Destino = model.Destino;
                        detalle.TipoLimpieza = model.TipoLimpieza;
                        detalle.Especie = model.Especie;
                    }
                    if (proceso == 2)
                    {
                        detalle.Lineas = model.Lineas;
                        detalle.HoraProcesoInicio = model.HoraProcesoInicio;
                        detalle.HoraProcesoFin = model.HoraProcesoFin;
                        detalle.Observacion = model.Observacion != null ? model.Observacion.ToUpper() : "";
                    }
                    if (proceso == 3)
                    {
                        detalle.HoraEviceradoInicio = model.HoraEviceradoInicio;
                        detalle.HoraEviceradoFin = model.HoraEviceradoFin;
                        detalle.HoraDescongeladoInicio = model.HoraDescongeladoInicio;
                        detalle.HoraDescongeladoFin = model.HoraDescongeladoFin;
                        detalle.HoraCoccionInicio = model.HoraCoccionInicio;
                        detalle.HoraCoccionFin = model.HoraCoccionFin;
                        detalle.Cocina = model.Cocina;
                        detalle.Requerimiento = model.Requerimiento;
                        detalle.TotalCoches = model.TotalCoches;
                        detalle.TemperaturaFinal = model.TemperaturaFinal;
                        detalle.Observacion = model.Observacion != null ? model.Observacion.ToUpper() : "";

                    }

                    detalle.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    detalle.FechaModificacionLog = DateTime.Now;
                    detalle.TerminalModificacionLog = model.TerminalIngresoLog;
                }
                else
                {
                    db.PROYECCION_PROGRAMACION_DETALLE.Add(model);
                }
                db.SaveChanges();
            }

        }

        public void InactivarProyeccionProgramacion (PROYECCION_PROGRAMACION model)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var proyeccion = db.PROYECCION_PROGRAMACION.FirstOrDefault(x => x.IdProyeccionProgramacion == model.IdProyeccionProgramacion);
                if (proyeccion != null)
                {
                    BITACORA_PROYECCION bitacora = new BITACORA_PROYECCION();
                    proyeccion.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    proyeccion.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    proyeccion.TerminalModificacionLog = model.TerminalIngresoLog;
                    proyeccion.FechaModificacionLog = DateTime.Now;

                    bitacora.Observacion = "Inactivar Proyeccion";
                    bitacora.IdProyeccionProgramacion = proyeccion.IdProyeccionProgramacion;
                    bitacora.UsuarioIngresoLog = model.UsuarioIngresoLog; 
                    bitacora.TerminalIngresoLog = model.TerminalIngresoLog; 
                    bitacora.FechaIngresoLog = DateTime.Now;
                    bitacora.EstadoRegistro = clsAtributos.EstadoRegistroActivo;

                    db.BITACORA_PROYECCION.Add(bitacora);

                    db.SaveChanges();
                }
            }
        }

    }
}