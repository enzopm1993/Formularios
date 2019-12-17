using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.ControlPesoEnlatado
{
    public class clsDControlPesoEnlatado
    {

        public List<spConsultaControlPesoEnlatado> ConsultarControlPesoEnlatado(DateTime Fecha, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {                
                    return entities.spConsultaControlPesoEnlatado(Fecha, Turno).ToList();                
            }
        }
        public void GuardarModificarControlPesoEnlatado(CONTROL_PESO_ENLATADO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var control = entities.CONTROL_PESO_ENLATADO.FirstOrDefault(x => x.IdControlPesoEnlatado == model.IdControlPesoEnlatado);
                if (control != null)
                {
                    control.EstadoRegistro = model.EstadoRegistro;
                    control.FechaModificacionLog = DateTime.Now;
                    control.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    control.TerminalModificacionLog = model.TerminalIngresoLog;
                }
                else
                {
                    entities.CONTROL_PESO_ENLATADO.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public bool ValidaControlPesoEnlatado(CONTROL_PESO_ENLATADO model)
        {
            var valida = true;
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {

                var control = entities.CONTROL_PESO_ENLATADO.FirstOrDefault(x => 
                x.Fecha== model.Fecha 
                && x.Peso == model.Peso
                && x.LineaEnlatado == model.LineaEnlatado
                && x.OrdenFabricacion == model.OrdenFabricacion
                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);

                if(control!= null)
                {
                    valida = false;
                }

            }
            return valida;
        }



        public List<spConsultaControlPesoEnlatadoDetalle> ConsultarControlPesoEnlatadoDetalle(int id)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaControlPesoEnlatadoDetalle(id).ToList();
            }
        }
        

        public List<spConsultaControlPesoEnlatadoSubDetalle> ConsultarControlPesoEnlatadoSubDetalle(int id)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaControlPesoEnlatadoSubDetalle(id).ToList();
            }
        }

        public List<spReporteControlPesoEnlatadoSubDetalle> ConsultarReporteControlPesoEnlatadoSubDetalle(DateTime fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spReporteControlPesoEnlatadoSubDetalle(fecha).ToList();
            }
        }

        public List<spReporteControlPesoEnlatadoDetalle> ConsultaReporteControlPesoEnlatadoDetalle(DateTime fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spReporteControlPesoEnlatadoDetalle(fecha).ToList();
            }
        }

        public void GuardarModificarControlPesoEnlatadoDetalle(CONTROL_PESO_ENLATADO_DETALLE model) {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var control = entities.CONTROL_PESO_ENLATADO_DETALLE.FirstOrDefault(x => x.IdControlPesoEnlatadoDetallado == model.IdControlPesoEnlatadoDetallado);
                if (control != null)
                {
                    control.TemperaturaAgua = model.TemperaturaAgua;
                    control.TemperaturaAceite = model.TemperaturaAceite;
                    control.EstadoRegistro = model.EstadoRegistro;
                    control.FechaModificacionLog = DateTime.Now;
                    control.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    control.TerminalModificacionLog = model.TerminalIngresoLog;
                }
                else
                {
                    entities.CONTROL_PESO_ENLATADO_DETALLE.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public bool ValidarControlPesoEnlatadoSubDetalle(CONTROL_PESO_ENLATADO_SUBDETALLE model)
        {
            bool valida = true;
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var control = entities.CONTROL_PESO_ENLATADO_SUBDETALLE.FirstOrDefault(x => 
                x.IdControlPesoEnlatadoSubdetalle != model.IdControlPesoEnlatadoSubdetalle
                && x.IdControlPesoEnlatadoDetallado == model.IdControlPesoEnlatadoDetallado
                && x.Muestra == model.Muestra
                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);

                if(control!= null)
                {
                    valida = false;
                }

            }
            return valida;
        }

        public void GuardarModificarControlPesoEnlatadoSubDetalle(CONTROL_PESO_ENLATADO_SUBDETALLE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var control = entities.CONTROL_PESO_ENLATADO_SUBDETALLE.FirstOrDefault(x => x.IdControlPesoEnlatadoSubdetalle == model.IdControlPesoEnlatadoSubdetalle);
                if (control != null)
                {
                    control.Muestra = model.Muestra;
                    control.Peso = model.Peso;
                    control.EstadoRegistro = model.EstadoRegistro;
                    control.FechaModificacionLog = DateTime.Now;
                    control.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    control.TerminalModificacionLog = model.TerminalIngresoLog;
                }
                else
                {
                    entities.CONTROL_PESO_ENLATADO_SUBDETALLE.Add(model);
                }
                entities.SaveChanges();
            }
        }


    }
}