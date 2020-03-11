using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.MonitoreoDescongelado
{
    public class clsDMonitoreoDescongelado
    {
        public List<spConsultaMonitoreoDescongelado> ConsultaMonitoreoDescongelado(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaMonitoreoDescongelado(Fecha).ToList();
                return lista;
            }
        }

        public MONITOREO_DESCONGELADO ConsultaMonitoreoDescongelado(DateTime Fecha, string Tanque, string Lote,string Tipo)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.MONITOREO_DESCONGELADO.Where(x=> 
                            x.Fecha==Fecha
                            &&x.Tanque == Tanque 
                            && x.Tipo == Tipo
                            && x.Lote == Lote
                            && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                return lista;
            }
        }

        public void GuardarModificarMonitoreoDescongelado(MONITOREO_DESCONGELADO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.MONITOREO_DESCONGELADO.FirstOrDefault(x => x.IdMonitoreoDescongelado == model.IdMonitoreoDescongelado);
                if (poControl != null)
                {
                    poControl.TemperaturaAgua = model.TemperaturaAgua;
                    poControl.Muestra1 = model.Muestra1;
                    poControl.Muestra2 = model.Muestra2;
                    poControl.Muestra3 = model.Muestra3;
                    poControl.Hora = model.Hora;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.MONITOREO_DESCONGELADO.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarMonitoreoDescongelado(MONITOREO_DESCONGELADO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.MONITOREO_DESCONGELADO.FirstOrDefault(x => x.IdMonitoreoDescongelado == model.IdMonitoreoDescongelado);
                if (poControl != null)
                {
                    poControl.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                    entities.SaveChanges();
                }

            }
        }
    }
}