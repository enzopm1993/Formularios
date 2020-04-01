using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Calidad;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.EvaluacionDeLomosyMigasEnBandeja
{
    public class clsDEvaluacionDeLomosYMigasEnBandeja
    {
        public CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA ConsultarCabeceraControl(DateTime FechaProduccion)
        {
            using (var db = new ASIS_PRODEntities())
            {
                return db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Where(x => x.FechaProduccion == FechaProduccion && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
            }
        }
        public object[] GuardarCabeceraControl(CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA poCabeceraControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarCabecera = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Where(x => x.FechaProduccion == poCabeceraControl.FechaProduccion &&
                x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (buscarCabecera == null)
                {
                    db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Add(poCabeceraControl);
                    db.SaveChanges();
                    resultado[0] = "000";
                    resultado[1] = "Registro ingresado con éxito";
                    resultado[2] = poCabeceraControl;
                }
                else
                {
                    resultado[0] = "002";
                    resultado[1] = "Error, el registro ya existe";
                    resultado[2] = buscarCabecera;
                }
                return resultado;
            }
        }
        public object[] ActualizarCabeceraControl(CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA poCabControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabecera = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Find(poCabControl.IdEvaluacionDeLomosYMigasEnBandejas);
                BuscarCabecera.Lomo = poCabControl.Lomo;
                BuscarCabecera.Miga = poCabControl.Miga;
                BuscarCabecera.NivelLimpieza = poCabControl.NivelLimpieza;
                BuscarCabecera.Observacion = poCabControl.Observacion;
                BuscarCabecera.OrdenFabricacion = poCabControl.OrdenFabricacion;
                BuscarCabecera.Pouch = poCabControl.Pouch;
                BuscarCabecera.FechaModificacionLog = poCabControl.FechaIngresoLog;
                BuscarCabecera.UsuarioModificacionLog = poCabControl.UsuarioIngresoLog;
                BuscarCabecera.TerminalIngresoLog = poCabControl.TerminalIngresoLog;
                db.SaveChanges();
                resultado[0] = "001";
                resultado[1] = "Registro actualizado con éxito";
                resultado[2] = poCabControl;
                return resultado;
            }
        }
        public object[] InactivarCabeceraControl(CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA poCabControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabeceraControl = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Find(poCabControl.IdEvaluacionDeLomosYMigasEnBandejas);
                BuscarCabeceraControl.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                BuscarCabeceraControl.FechaModificacionLog = poCabControl.FechaIngresoLog;
                BuscarCabeceraControl.UsuarioModificacionLog = poCabControl.UsuarioIngresoLog;
                BuscarCabeceraControl.TerminalModificacionLog = poCabControl.TerminalIngresoLog;
                db.SaveChanges();
                resultado[0] = "002";
                resultado[1] = "Registro Inactivado con éxito";
                resultado[2] = poCabControl;
                return resultado;
            }
         }
        public object[] GuardarDetalleControl(CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarDetalle = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE.Where(x => x.Hora == poDetalleControl.Hora).FirstOrDefault();
                if (buscarDetalle == null)
                {
                    db.CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE.Add(poDetalleControl);
                    db.SaveChanges();
                    resultado[0] = "000";
                    resultado[1] = "Registro ingresado con éxito";
                    resultado[2] = poDetalleControl;
                }
                else
                {
                    resultado[0] = "002";
                    resultado[1] = "Error, el registro ya existe";
                    resultado[2] = poDetalleControl;
                }
                return resultado;
            }
        }
        public object[] ActualizarDetalleControl(CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscardetalle = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE.Find(poDetalleControl.IdDetalleEvaluacionLomoyMigasEnBandeja);
                buscardetalle.FechaModificacionLog = poDetalleControl.FechaIngresoLog;
                buscardetalle.UsuarioModificacionLog = poDetalleControl.UsuarioIngresoLog;
                buscardetalle.TerminalModificacionLog = poDetalleControl.TerminalIngresoLog;
                buscardetalle.Linea = poDetalleControl.Linea;
                buscardetalle.buque = poDetalleControl.buque;
                buscardetalle.Lote = poDetalleControl.Lote;
                buscardetalle.Sabor = poDetalleControl.Sabor;
                buscardetalle.Textura = poDetalleControl.Textura;
                buscardetalle.Color = poDetalleControl.Color;
                buscardetalle.Olor = poDetalleControl.Olor;
                buscardetalle.Moretones = poDetalleControl.Moretones;
                buscardetalle.HematomasProfundos = poDetalleControl.HematomasProfundos;
                buscardetalle.Proteina = poDetalleControl.Proteina;
                buscardetalle.Trozo = poDetalleControl.Trozo;
                buscardetalle.Venas = poDetalleControl.Venas;
                buscardetalle.Espinas = poDetalleControl.Espinas;
                buscardetalle.Sangre = poDetalleControl.Sangre;
                buscardetalle.Escamas = poDetalleControl.Escamas;
                buscardetalle.Piel = poDetalleControl.Piel;
                db.SaveChanges();
                db.SaveChanges();
                resultado[0] = "001";
                resultado[1] = "Registro actualizado con éxito";
                resultado[2] = poDetalleControl;
                return resultado;
            }
        }
        public object[] InactivarDetalle(CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarDetalle = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE.Find(poDetalleControl.IdDetalleEvaluacionLomoyMigasEnBandeja);
                buscarDetalle.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                buscarDetalle.FechaModificacionLog = poDetalleControl.FechaIngresoLog;
                buscarDetalle.UsuarioModificacionLog = poDetalleControl.UsuarioIngresoLog;
                buscarDetalle.TerminalModificacionLog = poDetalleControl.TerminalIngresoLog;
                db.SaveChanges();
                resultado[0] = "002";
                resultado[1] = "Registro Inactivado con éxito";
                resultado[2] = poDetalleControl;
                return resultado;
            }
        }
        public  List<DetalleEvaluacionLomosMIgasBandejaViewModel> ConsultarDetalleControl(int idCabeceraControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                var resultado = (from d in db.CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE
                                join c in db.CC_MANTENIMIENTO_COLOR on new {d.Color, EstadoRegistro=clsAtributos.EstadoRegistroActivo } equals new { Color=c.IdColor, c.EstadoRegistro }
                                join o in db.CC_MANTENIMIENTO_OLOR on new {d.Olor, EstadoRegistro = clsAtributos.EstadoRegistroActivo }equals new { Olor=o.IdOlor, o.EstadoRegistro}
                                join s in db.CC_MANTENIMIENTO_SABOR on new { d.Sabor, EstadoRegistro =clsAtributos.EstadoRegistroActivo} equals new { Sabor=s.IdSabor,s.EstadoRegistro}
                                join t in db.CC_MANTENIMIENTO_TEXTURA on new { d.Textura, EstadoRegistro =clsAtributos.EstadoRegistroActivo} equals new {Textura=t.IdTextura,t.EstadoRegistro }
                                join p in db.CC_MANTENIMIENTO_PROTEINA on new {d.Proteina, EstadoRegistro=clsAtributos.EstadoRegistroActivo } equals new {Proteina=p.IdProteina,p.EstadoRegistro }
                                select new DetalleEvaluacionLomosMIgasBandejaViewModel { Buque=d.buque,CodColor=c.IdColor,CodOlor=o.IdOlor,CodProteinas=p.IdProteina,CodSabor=s.IdSabor,
                                CodTextura=t.IdTextura,Color=c.Descripcion,Escamas=d.Escamas,Espinas=d.Espinas,HematomasProfundos=d.HematomasProfundos,Hora=d.Hora,Linea=d.Linea,Lote=d.Lote,
                                Moretones=d.Moretones,Olor=o.Descripcion,Piel=d.Piel,Proteinas=p.Descripcion,Sabor=s.Descripcion,Sangre=d.Sangre,Textura=t.Descripcion,Trozos=d.Trozo,Venas=d.Venas}).ToList();
                return resultado;
            }
        }
    }
}