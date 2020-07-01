using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.General
{
    public class clsDPeriodo
    {
        
        public bool ValidarPeriodoFechaDesde(DateTime ddFechaDesde, DateTime ddFechaHasta,int Id)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                bool valida = false;
               // DateTime FechaHasta = Fecha.AddDays(1).Date;

                var periodo = entities.PERIODO.FirstOrDefault(x =>
                x.IdPeriodo!=Id && (
                (x.FechaDesde <= ddFechaDesde
                && x.FechaHasta >= ddFechaDesde)
                || (x.FechaDesde <= ddFechaHasta && x.FechaHasta>=ddFechaHasta))
                );
                if (periodo != null)
                {
                    valida = true;

                }               

                return valida;
            }
        }


        public bool ValidaFechaPeriodo(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                bool valida = true;
                //DateTime FechaHasta = Fecha.AddDays(1).Date;

                var periodo = entities.PERIODO.FirstOrDefault(x =>
                x.Estado == clsAtributos.PeriodoBloqueado
                && x.FechaDesde <= Fecha
                && x.FechaHasta >= Fecha
                );

                if (periodo != null){
                    valida = false;
                }

                return valida;
            }
        }


        public List<PERIODO> ConsultaPeriodos(PERIODO Filtros)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {

                IEnumerable<PERIODO> poPeriodo = entities.PERIODO;
                if (Filtros != null && Filtros.IdPeriodo > 0)
                {
                    poPeriodo = poPeriodo.Where(x => x.IdPeriodo == Filtros.IdPeriodo);
                }               
                if (Filtros != null && !string.IsNullOrEmpty(Filtros.Estado))
                {
                    poPeriodo = poPeriodo.Where(x => x.Estado == Filtros.Estado);
                }

                return poPeriodo.ToList();
            }
        }
        public string GuardarModificarPeriodo(PERIODO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                string Respuesta = string.Empty;
                var poPeriodo = entities.PERIODO.FirstOrDefault(x =>
                x.IdPeriodo == model.IdPeriodo);

                if (poPeriodo != null)
                {
                    poPeriodo.Descripcion = model.Descripcion.ToUpper();     
                    poPeriodo.Estado = model.Estado;
                    poPeriodo.FechaDesde = model.FechaDesde;
                    poPeriodo.FechaHasta = model.FechaHasta;
                    poPeriodo.FechaModificacionLog = DateTime.Now;
                    poPeriodo.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poPeriodo.TerminalModificacionLog = model.TerminalIngresoLog;
                }
                else
                {
                    entities.PERIODO.Add(model);
                }
                entities.SaveChanges();
                Respuesta = clsAtributos.MsjRegistroGuardado;
                return Respuesta;
            }
        }
    }
}