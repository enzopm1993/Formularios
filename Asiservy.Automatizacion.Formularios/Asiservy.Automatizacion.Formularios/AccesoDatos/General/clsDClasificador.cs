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
                        GrupoMombre = x.Descripcion,
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
                    string Grupo = string.Empty;
                    if (x.Codigo == "0")
                    {
                        Grupo = x.Descripcion;
                    }
                    else
                    {
                        ListadoClasificador.Add(new Clasificador
                        {
                            Codigo = x.Codigo,
                            EstadoRegistro = x.EstadoRegistro,
                            Grupo = x.Grupo,
                            Descripcion = x.Descripcion,
                            FechaIngresoLog = x.FechaIngresoLog,
                            FechaModificacionLog = x.FechaModificacionLog,
                            GrupoMombre = Grupo,
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