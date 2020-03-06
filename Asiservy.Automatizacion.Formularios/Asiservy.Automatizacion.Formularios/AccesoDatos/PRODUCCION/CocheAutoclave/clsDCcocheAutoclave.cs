using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.PRODUCCION.CocheAutoclave
{
    public class clsDCcocheAutoclave
    {
        public List<spConsultaCocheAutoclave> ConsultaCocheAutoclave(DateTime Fecha, string Turno)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaCocheAutoclave(Fecha, Turno).ToList();
                return lista;
            }
        }

        public void GuardarModificarCocheAutoclave(COCHE_AUTOCLAVE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poMapeo = entities.COCHE_AUTOCLAVE.FirstOrDefault(x=>x.IdCocheAutoclave==model.IdCocheAutoclave);
                if(poMapeo!= null)
                {
                    poMapeo.Autoclave = model.Autoclave;
                    poMapeo.Parada = model.Parada;
                    poMapeo.Lote = model.Lote;
                    poMapeo.Envase = model.Envase;
                    poMapeo.CodigoProducto = model.CodigoProducto;
                    poMapeo.Observacion = model.Observacion;
                    poMapeo.OrdenFabricacion = model.OrdenFabricacion;
                    poMapeo.TerminalModificacionLog = model.TerminalIngresoLog;
                    poMapeo.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poMapeo.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.COCHE_AUTOCLAVE.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarCocheAutoclave(COCHE_AUTOCLAVE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poMapeo = entities.COCHE_AUTOCLAVE.FirstOrDefault(x => x.IdCocheAutoclave == model.IdCocheAutoclave);
                if (poMapeo != null)
                {
                    poMapeo.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    poMapeo.TerminalModificacionLog = model.TerminalIngresoLog;
                    poMapeo.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poMapeo.FechaModificacionLog = model.FechaIngresoLog;
                    entities.SaveChanges();
                }              
             
            }
        }


        public List<spConsultaCocheAutoclaveDetalle> ConsultaCocheAutoclaveDetalle(int IdControl)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spConsultaCocheAutoclaveDetalle(IdControl).ToList();
                return lista;
            }
        }

        public void GuardarModificarCocheAutoclaveDetalle(COCHE_AUTOCLAVE_DETALLE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poMapeo = entities.COCHE_AUTOCLAVE_DETALLE.FirstOrDefault(x => x.IdCocheAutoclaveDetalle == model.IdCocheAutoclaveDetalle);
                if (poMapeo != null)
                {
                    poMapeo.Coche = model.Coche;
                    poMapeo.Tarjeta = model.Tarjeta;
                    poMapeo.HoraInicio = model.HoraInicio;
                    poMapeo.LineaProduccion = model.LineaProduccion;
                    poMapeo.TerminalModificacionLog = model.TerminalIngresoLog;
                    poMapeo.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poMapeo.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    entities.COCHE_AUTOCLAVE_DETALLE.Add(model);
                }
                entities.SaveChanges();
            }
        }
        public void EliminarCocheAutoclaveDetalle(COCHE_AUTOCLAVE_DETALLE model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poMapeo = entities.COCHE_AUTOCLAVE_DETALLE.FirstOrDefault(x => x.IdCocheAutoclaveDetalle == model.IdCocheAutoclaveDetalle);
                if (poMapeo != null)
                {
                    poMapeo.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    poMapeo.TerminalModificacionLog = model.TerminalIngresoLog;
                    poMapeo.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poMapeo.FechaModificacionLog = model.FechaIngresoLog;
                    entities.SaveChanges();
                }

            }
        }

        public List<int> ConsultarOrdenesFabricacion(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.COCHE_AUTOCLAVE.Where(x=>x.Fecha==Fecha && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo).Select(x=> x.OrdenFabricacion).Distinct().ToList();
                return lista;
            }
        }


        public List<spReporteCocheAutoclaveDetalle> ConsultaReporteCocheAutoclaveDetalle(int Orden)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.spReporteCocheAutoclaveDetalle(Orden).ToList();
                return lista;
            }
        }




    }
}