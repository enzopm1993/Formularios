using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.ControlCocheLinea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ControlCocheLinea
{
    public class clsDControlCocheLinea
    {
        clsDGeneral clsDGeneral = null;
        public List<spConsultaReporteControlCochePorLineas> ConsultaReporteControlCochePorLinea(DateTime fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaReporteControlCochePorLineas(fecha).ToList();
            }
        }

        public List<ControlCocheLineaViewModel> ConsultarControlCocheLinea(ControlCocheLineaViewModel filtros)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                IEnumerable<CONTROL_COCHE_LINEA> poControl = entities.CONTROL_COCHE_LINEA;
                List<CONTROL_COCHE_LINEA> ListadoControl;
                List<ControlCocheLineaViewModel> ListadoFinal = new List<ControlCocheLineaViewModel>();
                if (filtros != null)
                {
                    if (filtros.IdControlCocheLinea > 0)
                    {
                        poControl = poControl.Where(x => x.IdControlCocheLinea == filtros.IdControlCocheLinea);
                    }

                    poControl = poControl.Where(x=> x.Fecha ==  filtros.Fecha);
                }

                ListadoControl = poControl.ToList();
                clsDGeneral = new clsDGeneral();
                foreach (var x in ListadoControl)
                {
                    var Linea = clsDGeneral.ConsultaLineas(x.Linea).FirstOrDefault();
                    ListadoFinal.Add(new ControlCocheLineaViewModel {
                        IdControlCocheLinea = x.IdControlCocheLinea,
                        Linea = x.Linea,
                        Coches=x.Coches,
                        DescripcionLinea = Linea.Descripcion,
                        Fecha = x.Fecha,
                        FechaIngresoLog = x.FechaIngresoLog,
                        FechaModificacionLog = x.FechaModificacionLog,
                        HoraFin = x.HoraFin,
                        HoraInicio = x.HoraInicio,
                        Observacion = x.Observacion,
                        Talla = x.Talla,
                        TerminalIngresoLog = x.TerminalIngresoLog,
                        TerminalModificacionLog = x.TerminalModificacionLog,
                        UsuarioIngresoLog = x.UsuarioIngresoLog,
                        UsuarioModificacionLog = x.UsuarioModificacionLog
                    });
                }
                return ListadoFinal;

            }

        }
        public string GuardarModificarControlCochePorLinea(CONTROL_COCHE_LINEA model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var ControlCoche = entities.CONTROL_COCHE_LINEA.FirstOrDefault(x=> x.IdControlCocheLinea == model.IdControlCocheLinea);

                if(ControlCoche != null)
                {
                    ControlCoche.HoraInicio = model.HoraInicio;
                    ControlCoche.HoraFin = model.HoraFin;
                    ControlCoche.Coches = model.Coches;
                    ControlCoche.Talla = model.Talla;
                    ControlCoche.Observacion = model.Observacion;
                    ControlCoche.FechaModificacionLog = DateTime.Now;
                    ControlCoche.TerminalModificacionLog = model.TerminalIngresoLog;
                    ControlCoche.UsuarioModificacionLog = model.UsuarioIngresoLog;
                }
                else
                {
                    entities.CONTROL_COCHE_LINEA.Add(model);
                }
                entities.SaveChanges();
                return clsAtributos.MsjRegistroGuardado;

                    
            }
        }
    }
}