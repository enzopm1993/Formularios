using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ControlBalanza
{
    public class clsDControlBalanza
    {
        public List<spConsultaControlBalanza> ConsultarControlBalance(DateTime FechaProduccion)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.spConsultaControlBalanza(FechaProduccion).ToList();
                return listado;
            }
        }
        public void GuardarModificarControlBalanza(CONTROL_BALANZA Control)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CONTROL_BALANZA.FirstOrDefault(x => x.IdControlBalanza == Control.IdControlBalanza || (x.Cedula==Control.Cedula &&x.Fecha == Control.Fecha && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo));
                if(model!= null)
                {
                    model.Codigo = Control.Codigo;
                    model.Observacion = Control.Observacion;
                    model.FechaModificacionLog = DateTime.Now;
                    model.TerminalModificacionLog = Control.TerminalIngresoLog;
                    model.UsuarioModificacionLog = Control.UsuarioIngresoLog;
                }
                else
                {
                    Control.FechaIngresoLog = DateTime.Now;
                    db.CONTROL_BALANZA.Add(Control);
                }
                db.SaveChanges();
            }
        }


        public void EliminarControl(CONTROL_BALANZA Control)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var model = db.CONTROL_BALANZA.FirstOrDefault(x => x.IdControlBalanza == Control.IdControlBalanza);
                if (model != null)
                {
                    model.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    model.FechaModificacionLog = DateTime.Now;
                    model.TerminalModificacionLog = Control.TerminalIngresoLog;
                    model.UsuarioModificacionLog = Control.UsuarioIngresoLog;
                    db.SaveChanges();
                }
            }
        }
        public List<spReporteConsultaControlBalanza> ConsultarReporteControlBalance(DateTime FechaDesde, DateTime FechaHasta)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var listado = db.spReporteConsultaControlBalanza(FechaDesde,FechaHasta).ToList();
                return listado;
            }
        }
    }
}