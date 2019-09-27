using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ProyeccionProgramacion
{

    public class clsDProyeccionProgramacion
    {
        public string GuardarActualizarProyeccionProgramacion()
        {
            using(ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                DateTime fecha =Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());
                var ListProyeccionProgramacion = (from p in db.PROYECCION_PROGRAMACION
                                                  join c in db.CLASIFICADOR on new {Codigo=p.lim, Grupo=clsAtributos.CodigoGrupoTipoLimpiezaPescado } equals new { c.Codigo, c.Grupo }
                                                  join d in db.CLASIFICADOR on new { Codigo = p.Destino, Grupo = clsAtributos.CodigoGrupoTipoLimpiezaPescado } equals new { c.Codigo, c.Grupo }
                                                  select new {Lote=p.Lote, toneladas=p.Toneladas, destino=c.Descripcion }).ToList();
            }
            return "";
        }
    }
}