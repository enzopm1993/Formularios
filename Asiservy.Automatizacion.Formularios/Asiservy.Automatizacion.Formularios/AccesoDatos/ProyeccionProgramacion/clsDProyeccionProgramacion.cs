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

        public List<spConsultaProyeccionProgramacion> ConsultaProyeccionProgramacion (int Id)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                List<spConsultaProyeccionProgramacion> Listado = new List<spConsultaProyeccionProgramacion>();
                Listado = db.spConsultaProyeccionProgramacion(Id).ToList();
                return Listado;
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


        public void EditarProyeccionProgramacion(DateTime Fecha, bool? Ingreso, bool? EditaProduccion, bool? EditaPreparacion, bool?Finaliza)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var proyeccion = db.PROYECCION_PROGRAMACION.FirstOrDefault(x => x.FechaProduccion == Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (proyeccion != null)
                {
                    if (Ingreso != null)
                    {
                        proyeccion.IngresoPreparacion = Ingreso?? proyeccion.IngresoPreparacion;
                    }
                    if (EditaProduccion != null)
                    {
                        proyeccion.EditaProduccion = EditaProduccion ?? proyeccion.EditaProduccion;
                    }
                    if (EditaPreparacion != null)
                    {
                        proyeccion.EditarPreparacion = EditaPreparacion ?? proyeccion.EditarPreparacion;
                    }
                    if (Finaliza != null)
                    {
                        proyeccion.Finaliza = Finaliza ?? proyeccion.Finaliza;
                    }
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
                        detalle.Lote = model.Lote;
                        detalle.Observacion = model.Observacion;
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
                        detalle.Observacion = model.Observacion;
                    }
                    if (proceso == 3)
                    {
                        detalle.HoraEviceradoInicio = model.HoraEviceradoInicio;
                        detalle.HoraEviceradoFin = model.HoraEviceradoFin;
                        detalle.HoraDescongeladoInicio = model.HoraDescongeladoInicio;
                        detalle.HoraDescongeladoFin = model.HoraDescongeladoFin;
                        detalle.HoraCoccionInicio = model.HoraCoccionInicio;
                        detalle.HoraCoccionFin = model.HoraCoccionFin;
                        detalle.Observacion = model.Observacion;

                    }

                    detalle.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    detalle.FechaModificacionLog = DateTime.Now;
                    detalle.TerminalModificacionLog = model.TerminalIngresoLog;
                }
                else
                {
                    db.PROYECCION_PROGRAMACION_DETALLE.Add(detalle);
                }
                db.SaveChanges();
            }

        }

    }
}