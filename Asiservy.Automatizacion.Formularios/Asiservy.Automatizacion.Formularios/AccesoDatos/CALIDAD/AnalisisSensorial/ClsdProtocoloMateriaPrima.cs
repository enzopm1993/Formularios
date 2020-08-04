using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.AnalisisSensorial
{
    public class ClsdProtocoloMateriaPrima
    {
        #region PROTOCOLO MATERIA PRIMA ANÁLISIS SENSORIAL
       
        public List<CC_PROTOCOLO_MATERIA_PRIMA_AS> ConsultaProtocoloMateriaPrima()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var lista = entities.CC_PROTOCOLO_MATERIA_PRIMA_AS.AsNoTracking().ToList();
                return lista;
            }
        }

        public void GuardarModificarParamtroMateriaPrima(CC_PROTOCOLO_MATERIA_PRIMA_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_PROTOCOLO_MATERIA_PRIMA_AS.FirstOrDefault(x => x.IdProtocoloMateriaPrima == model.IdProtocoloMateriaPrima );
                if (poControl != null)
                {
                    poControl.FechaEvaluacion = model.FechaEvaluacion;
                    poControl.FechaDescarga = model.FechaDescarga;
                    poControl.Lote = model.Lote;
                    poControl.LoteDescarga = model.LoteDescarga;
                    poControl.CodigoProtocolo = model.CodigoProtocolo;
                    poControl.Pcc = model.Pcc;
                    poControl.Observacion = model.Observacion != null ? model.Observacion.ToUpper() : model.Observacion;
                    poControl.EstadoRegistro = model.EstadoRegistro;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                }
                else
                {
                    model.Observacion = model.Observacion!=null? model.Observacion.ToUpper():model.Observacion;
                    entities.CC_PROTOCOLO_MATERIA_PRIMA_AS.Add(model);
                }
                entities.SaveChanges();
            }
        }

        public void EliminarParametroMateriaPrima(CC_PROTOCOLO_MATERIA_PRIMA_AS model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var poControl = entities.CC_PROTOCOLO_MATERIA_PRIMA_AS.FirstOrDefault(x => x.IdProtocoloMateriaPrima == model.IdProtocoloMateriaPrima );
                if (poControl != null)
                {
                    poControl.EstadoRegistro = model.EstadoRegistro;
                    poControl.TerminalModificacionLog = model.TerminalIngresoLog;
                    poControl.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poControl.FechaModificacionLog = model.FechaIngresoLog;
                    entities.SaveChanges();
                }

            }
        }

        public CC_PROTOCOLO_MATERIA_PRIMA_AS ConsultaProtocoloMateriaPrima(DateTime Fecha)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.CC_PROTOCOLO_MATERIA_PRIMA_AS.FirstOrDefault(x => x.Fecha == Fecha
                                                                && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
            }
        }
        #endregion

    }
}