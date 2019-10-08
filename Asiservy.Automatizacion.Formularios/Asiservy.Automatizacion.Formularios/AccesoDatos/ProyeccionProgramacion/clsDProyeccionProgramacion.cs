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
        public List<ProyeccionProgramacionViewModel> GuardarActualizarProyeccionProgramacion(PROYECCION_PROGRAMACION ProyeccionProgramacion)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var BuscarProyeccion = db.PROYECCION_PROGRAMACION.Find(ProyeccionProgramacion.IdProyeccionProgramacion);
                DateTime fecha = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());
                if (BuscarProyeccion == null)
                {
                    db.PROYECCION_PROGRAMACION.Add(ProyeccionProgramacion);
                    db.SaveChanges();
                }
                else
                {
                    if (string.IsNullOrEmpty(ProyeccionProgramacion.Lineas))//Si el update viene de la vista ProyeccionProgramacion
                    {
                        BuscarProyeccion.Lote = ProyeccionProgramacion.Lote;
                        BuscarProyeccion.Destino = ProyeccionProgramacion.Destino;
                        BuscarProyeccion.FechaModificacionLog = ProyeccionProgramacion.FechaCreacionLog;
                        BuscarProyeccion.UsuarioModificacionLog = ProyeccionProgramacion.UsuarioCreacionLog;
                        BuscarProyeccion.TerminalModificacionLog = ProyeccionProgramacion.TerminalCreacionLog;
                        BuscarProyeccion.FechaProduccion = ProyeccionProgramacion.FechaProduccion;
                        BuscarProyeccion.Observacion = ProyeccionProgramacion.Observacion;
                        BuscarProyeccion.Toneladas = ProyeccionProgramacion.Toneladas;
                        BuscarProyeccion.TipoLimpieza = ProyeccionProgramacion.TipoLimpieza;

                        db.SaveChanges();
                    }
                    else//Si el update viene de la vista Proyeccion Programacion Editar
                    {
                        BuscarProyeccion.Lineas = ProyeccionProgramacion.Lineas;
                        BuscarProyeccion.HoraInicio = ProyeccionProgramacion.HoraInicio;
                        BuscarProyeccion.HoraFin = ProyeccionProgramacion.HoraFin;
                        BuscarProyeccion.FechaModificacionLog = ProyeccionProgramacion.FechaCreacionLog;
                        BuscarProyeccion.UsuarioModificacionLog = ProyeccionProgramacion.UsuarioCreacionLog;
                        BuscarProyeccion.TerminalModificacionLog = ProyeccionProgramacion.TerminalCreacionLog;
                        db.SaveChanges();
                    }
                    
                }
                var ListProyeccionProgramacion = (from p in db.PROYECCION_PROGRAMACION
                                                  join c in db.CLASIFICADOR on new { Codigo = p.TipoLimpieza, Grupo = clsAtributos.CodigoGrupoTipoLimpiezaPescado, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { c.Codigo, c.Grupo, c.EstadoRegistro }
                                                  join d in db.CLASIFICADOR on new { Codigo = p.Destino, Grupo = clsAtributos.CodigoGrupoDestinoProduccion } equals new { d.Codigo, d.Grupo }
                                                  where p.FechaProduccion == fecha
                                                  select new ProyeccionProgramacionViewModel
                                                  {
                                                      Lote = p.Lote,
                                                      Tonelada = p.Toneladas,
                                                      Destino = d.Descripcion,
                                                      TipoLimpieza = c.Descripcion,
                                                      Observacion = p.Observacion,
                                                      CodDestino = p.Destino,
                                                      IdTipoLimpieza = p.TipoLimpieza,
                                                      FechaProduccion = p.FechaProduccion,
                                                      IdProyeccion = p.IdProyeccionProgramacion,
                                                      Lineas=p.Lineas,
                                                      HoraFin=p.HoraFin,
                                                      HoraInicio=p.HoraInicio
                                                  }).ToList();
                return ListProyeccionProgramacion;
            }

        }

        public List<ProyeccionProgramacionViewModel> ConsultarProyeccionProgramacion(DateTime? ddFecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                DateTime fecha;
                if (ddFecha == null)
                    fecha = DateTime.Now;
                else
                    fecha = ddFecha ?? DateTime.Now;
                var ListProyeccionProgramacion = (from p in db.PROYECCION_PROGRAMACION
                                                  join c in db.CLASIFICADOR on new { Codigo = p.TipoLimpieza, Grupo = clsAtributos.CodigoGrupoTipoLimpiezaPescado, EstadoRegistro =clsAtributos.EstadoRegistroActivo} equals new { c.Codigo, c.Grupo, c.EstadoRegistro }
                                                  join d in db.CLASIFICADOR on new { Codigo = p.Destino, Grupo = clsAtributos.CodigoGrupoDestinoProduccion } equals new { d.Codigo, d.Grupo }
                                                  where p.FechaProduccion > fecha
                                                  //where p.FechaProduccion == DateTime.Now
                                                  select new ProyeccionProgramacionViewModel
                                                  {
                                                      Lote = p.Lote,
                                                      Tonelada = p.Toneladas,
                                                      Destino = d.Descripcion,
                                                      TipoLimpieza = c.Descripcion,
                                                      Observacion = p.Observacion,
                                                      CodDestino = p.Destino,
                                                      IdTipoLimpieza = p.TipoLimpieza,
                                                      FechaProduccion = p.FechaProduccion,
                                                      IdProyeccion = p.IdProyeccionProgramacion,
                                                      Lineas=p.Lineas,
                                                      HoraInicio=p.HoraInicio,
                                                      HoraFin=p.HoraFin
                                                  }).ToList();
                return ListProyeccionProgramacion;
            }
        }
    }
}