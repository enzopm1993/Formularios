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
                    BuscarProyeccion.Lote = ProyeccionProgramacion.Lote;
                    db.SaveChanges();
                }
                var ListProyeccionProgramacion = (from p in db.PROYECCION_PROGRAMACION
                                                  join c in db.CLASIFICADOR on new { Codigo = p.TipoLimpieza, Grupo = clsAtributos.CodigoGrupoTipoLimpiezaPescado } equals new { c.Codigo, c.Grupo }
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
                                                      IdProyeccion = p.IdProyeccionProgramacion
                                                  }).ToList();
                return ListProyeccionProgramacion;
            }

        }

        public List<ProyeccionProgramacionViewModel> ConsultarProyeccionProgramacion()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                DateTime fecha = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());
                var ListProyeccionProgramacion = (from p in db.PROYECCION_PROGRAMACION
                                                  join c in db.CLASIFICADOR on new { Codigo = p.TipoLimpieza, Grupo = clsAtributos.CodigoGrupoTipoLimpiezaPescado, EstadoRegistro =clsAtributos.EstadoRegistroActivo} equals new { c.Codigo, c.Grupo, c.EstadoRegistro }
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
                                                      IdProyeccion = p.IdProyeccionProgramacion
                                                  }).ToList();
                return ListProyeccionProgramacion;
            }
        }
    }
}