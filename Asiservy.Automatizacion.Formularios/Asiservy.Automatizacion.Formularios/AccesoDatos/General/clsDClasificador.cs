using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDClasificador
    {
        ASIS_PRODEntities entities = null;


        public string GuardarModificarClasificador(Clasificador model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                string Respuesta = string.Empty;
                var poClasificador = entities.CLASIFICADOR.FirstOrDefault(x => x.IdClasificador == model.IdClasificador);
                if (poClasificador != null)
                {
                    poClasificador.Descripcion = model.Descripcion;
                    poClasificador.Grupo = model.Grupo;
                    poClasificador.Codigo = model.Codigo;
                    poClasificador.EstadoRegistro = model.EstadoRegistro;
                    poClasificador.FechaModificacionLog = DateTime.Now;
                    poClasificador.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poClasificador.TerminalModificacionLog = model.TerminalIngresoLog;
                }else
                {

                    entities.CLASIFICADOR.Add(new CLASIFICADOR {
                        Codigo = model.Codigo,
                        TerminalIngresoLog = model.TerminalIngresoLog,
                        UsuarioIngresoLog = model.UsuarioIngresoLog,
                        Grupo = model.Grupo,
                        Descripcion = model.Descripcion,
                        EstadoRegistro = model.EstadoRegistro,
                        FechaIngresoLog= DateTime.Now
                    });
                }
                entities.SaveChanges();
                Respuesta = clsAtributos.MsjRegistroGuardado;
                return Respuesta;
            }
        }


        public List<Clasificador> ConsultaClasificadorGrupos()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<Clasificador> ListadoClasificador = new List<Clasificador>();
                IEnumerable<CLASIFICADOR> poClasificador = entities.CLASIFICADOR.Where(x => x.Codigo == clsAtributos.CodigoClasificadorGrupo);
                foreach (var x in poClasificador.OrderBy(x => x.IdClasificador).ToList())
                {

                    ListadoClasificador.Add(new Clasificador
                    {
                        Codigo = x.Codigo,
                        EstadoRegistro = x.EstadoRegistro,
                        Grupo = x.Grupo,
                        Descripcion = x.Descripcion,
                        FechaIngresoLog = x.FechaIngresoLog,
                        FechaModificacionLog = x.FechaModificacionLog,
                        GrupoNombre = x.Descripcion,
                        IdClasificador = x.IdClasificador,
                        TerminalIngresoLog = x.TerminalIngresoLog,
                        TerminalModificacionLog = x.TerminalModificacionLog,
                        UsuarioIngresoLog = x.UsuarioIngresoLog,
                        UsuarioModificacionLog = x.UsuarioModificacionLog
                    });

                }
                return ListadoClasificador;
            }

        }

        public List<Clasificador> ConsultaClasificador(Clasificador Filtros)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<Clasificador> ListadoClasificador = new List<Clasificador>();

                IEnumerable<CLASIFICADOR> poClasificador = entities.CLASIFICADOR;
                if (Filtros != null && !string.IsNullOrEmpty(Filtros.Grupo))
                {
                    poClasificador = poClasificador.Where(x => x.Grupo == Filtros.Grupo);
                }
                if (Filtros != null && !string.IsNullOrEmpty(Filtros.Codigo))
                {
                    poClasificador = poClasificador.Where(x => x.Codigo == Filtros.Codigo);
                }
                if (Filtros != null && !string.IsNullOrEmpty(Filtros.EstadoRegistro))
                {
                    poClasificador = poClasificador.Where(x => x.EstadoRegistro == Filtros.EstadoRegistro);
                }

                
                foreach (var x in poClasificador.OrderBy(x => x.IdClasificador).ToList())
                {
                    if (x.Codigo != "0")
                    {
                        var DescripcionGrupo = entities.CLASIFICADOR.FirstOrDefault(y => y.Grupo == x.Grupo && y.Codigo == "0");
                        ListadoClasificador.Add(new Clasificador
                        {
                            Codigo = x.Codigo,
                            EstadoRegistro = x.EstadoRegistro,
                            Grupo = x.Grupo,
                            Descripcion = x.Descripcion,
                            FechaIngresoLog = x.FechaIngresoLog,
                            FechaModificacionLog = x.FechaModificacionLog,
                            GrupoNombre = DescripcionGrupo != null ? DescripcionGrupo.Descripcion : "",
                            IdClasificador = x.IdClasificador,
                            TerminalIngresoLog = x.TerminalIngresoLog,
                            TerminalModificacionLog = x.TerminalModificacionLog,
                            UsuarioIngresoLog = x.UsuarioIngresoLog,
                            UsuarioModificacionLog = x.UsuarioModificacionLog
                        });
                    }
                }



                return ListadoClasificador.ToList();
            }
        }

        public List<CLASIFICADOR> ConsultarClasificador(string dsGrupo, int diCodigo=0)
        {
            entities = new ASIS_PRODEntities();
            if(diCodigo!=0)
                return entities.CLASIFICADOR.Where(x => x.Grupo == dsGrupo && x.Codigo==diCodigo+"" && x.EstadoRegistro=="A").ToList();
            else
                return entities.CLASIFICADOR.Where(x => x.Grupo == dsGrupo && x.Codigo != diCodigo+"" && x.EstadoRegistro == "A").ToList();

        }

    }
}