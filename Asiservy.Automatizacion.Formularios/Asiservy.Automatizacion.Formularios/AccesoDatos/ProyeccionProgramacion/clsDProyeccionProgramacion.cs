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
        public int ValidarProyeccionProgramacion(PROYECCION_PROGRAMACION model)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var proyeccion = db.PROYECCION_PROGRAMACION.FirstOrDefault(x => x.FechaProduccion == model.FechaProduccion && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
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


        public void GenerarProyeccionProgramacion(PROYECCION_PROGRAMACION model)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var proyeccion = db.PROYECCION_PROGRAMACION.FirstOrDefault(x=> x.FechaProduccion == model.FechaProduccion && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if(proyeccion == null)
                {
                    db.PROYECCION_PROGRAMACION.Add(model);
                    db.SaveChanges();
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

        public void GuardarModificarProyeccionProgramacionDetalle(PROYECCION_PROGRAMACION_DETALLE model)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                //var proyeccion = db.PROYECCION_PROGRAMACION.FirstOrDefault(x => x.IdProyeccionProgramacion==model.IdProyeccionProgramacion);
                var detalle = db.PROYECCION_PROGRAMACION_DETALLE.FirstOrDefault(x => x.IdProyeccionProgramacionDetalle == model.IdProyeccionProgramacionDetalle);
                if (detalle != null)
                {
                    detalle.Lote = model.Lote;
                    detalle.Observacion = model.Observacion;
                    detalle.OrdenFabricacion = model.OrdenFabricacion;
                    detalle.Lineas = model.Lineas;
                    detalle.HoraProcesoInicio = model.HoraProcesoInicio;
                    detalle.HoraProcesoFin = model.HoraProcesoFin;
                    detalle.Toneladas = model.Toneladas;
                    detalle.Destino = model.Destino;
                    detalle.TipoLimpieza = model.TipoLimpieza;
                    detalle.Especie = model.Especie;
                    //detalle.
                }
            }

        }


        //public List<ProyeccionProgramacionViewModel> GuardarActualizarProyeccionProgramacion(PROYECCION_PROGRAMACION ProyeccionProgramacion)
        //{
        //    using (ASIS_PRODEntities db = new ASIS_PRODEntities())
        //    {
        //        var BuscarProyeccion = db.PROYECCION_PROGRAMACION.Find(ProyeccionProgramacion.IdProyeccionProgramacion);
        //        //DateTime fecha = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());
        //        DateTime? fecha = ProyeccionProgramacion.FechaProduccion;
        //        if (BuscarProyeccion == null)
        //        {
        //            db.PROYECCION_PROGRAMACION.Add(ProyeccionProgramacion);
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            if (string.IsNullOrEmpty(ProyeccionProgramacion.Lineas))//Si el update viene de la vista ProyeccionProgramacion
        //            {
        //                BuscarProyeccion.Lote = ProyeccionProgramacion.Lote;
        //                BuscarProyeccion.Destino = ProyeccionProgramacion.Destino;
        //                BuscarProyeccion.FechaModificacionLog = ProyeccionProgramacion.FechaCreacionLog;
        //                BuscarProyeccion.UsuarioModificacionLog = ProyeccionProgramacion.UsuarioCreacionLog;
        //                BuscarProyeccion.TerminalModificacionLog = ProyeccionProgramacion.TerminalCreacionLog;
        //                BuscarProyeccion.FechaProduccion = ProyeccionProgramacion.FechaProduccion;
        //                BuscarProyeccion.Observacion = ProyeccionProgramacion.Observacion;
        //                BuscarProyeccion.Toneladas = ProyeccionProgramacion.Toneladas;
        //                BuscarProyeccion.TipoLimpieza = ProyeccionProgramacion.TipoLimpieza;
        //                BuscarProyeccion.Especie = ProyeccionProgramacion.Especie;
        //                BuscarProyeccion.Talla = ProyeccionProgramacion.Talla;
        //                db.SaveChanges();
        //            }
        //            else//Si el update viene de la vista Proyeccion Programacion Editar
        //            {
        //                BuscarProyeccion.Lineas = ProyeccionProgramacion.Lineas;
        //                BuscarProyeccion.HoraInicio = ProyeccionProgramacion.HoraInicio;
        //                BuscarProyeccion.HoraFin = ProyeccionProgramacion.HoraFin;
        //                BuscarProyeccion.Observacion = ProyeccionProgramacion.Observacion;
        //                BuscarProyeccion.FechaModificacionLog = ProyeccionProgramacion.FechaCreacionLog;
        //                BuscarProyeccion.UsuarioModificacionLog = ProyeccionProgramacion.UsuarioCreacionLog;
        //                BuscarProyeccion.TerminalModificacionLog = ProyeccionProgramacion.TerminalCreacionLog;
        //                db.SaveChanges();
        //            }

        //        }
        //        var ListProyeccionProgramacion = (from p in db.PROYECCION_PROGRAMACION
        //                                          join c in db.CLASIFICADOR on new { Codigo = p.TipoLimpieza, Grupo = clsAtributos.CodigoGrupoTipoLimpiezaPescado, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { c.Codigo, c.Grupo, c.EstadoRegistro }
        //                                          join d in db.CLASIFICADOR on new { Codigo = p.Destino, Grupo = clsAtributos.CodigoGrupoDestinoProduccion,EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { d.Codigo, d.Grupo,d.EstadoRegistro }
        //                                          //join e in db.CLASIFICADOR on new { Codigo = p.Especie, Grupo = clsAtributos.CodigoGrupoEspeciePescado, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { e.Codigo, e.Grupo, e.EstadoRegistro }
        //                                          //join f in db.CLASIFICADOR on new { Codigo = p.Talla, Grupo = clsAtributos.CodigoGrupoTallaPescado, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { f.Codigo, f.Grupo, f.EstadoRegistro }
        //                                          where p.FechaProduccion == fecha
        //                                          select new ProyeccionProgramacionViewModel
        //                                          {
        //                                              Lote = p.Lote,
        //                                              Tonelada = p.Toneladas,
        //                                              Destino = d.Descripcion,
        //                                              TipoLimpieza = c.Descripcion,
        //                                              Observacion = p.Observacion,
        //                                              CodDestino = p.Destino,
        //                                              IdTipoLimpieza = p.TipoLimpieza,
        //                                              FechaProduccion = p.FechaProduccion,
        //                                              IdProyeccion = p.IdProyeccionProgramacion,
        //                                              Lineas=p.Lineas,
        //                                              HoraFin=p.HoraFin,
        //                                              HoraInicio=p.HoraInicio,
        //                                              Especie=p.Especie,
        //                                              Talla=p.Talla

        //                                          }).ToList();
        //        return ListProyeccionProgramacion;
        //    }

        //}


    }
}