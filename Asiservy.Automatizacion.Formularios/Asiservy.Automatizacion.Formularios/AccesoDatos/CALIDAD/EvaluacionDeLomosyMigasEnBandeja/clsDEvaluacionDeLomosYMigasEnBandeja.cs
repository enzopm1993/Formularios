using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Calidad;
using Asiservy.Automatizacion.Formularios.Models.CALIDAD;

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
                if (BuscarCabecera.EstadoControl == true)
                {
                    resultado[0] = "003";
                    resultado[1] = "El control ya se encuentra aprobado, no puede ser modificado";
                    resultado[2] = poCabControl;
                }
                else
                {
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
                }
                return resultado;
            }
        }
        public object[] InactivarCabeceraControl(CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA poCabControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabeceraControl = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Find(poCabControl.IdEvaluacionDeLomosYMigasEnBandejas);
                if(BuscarCabeceraControl.EstadoControl==true)
                {
                    resultado[0] = "003";
                    resultado[1] = "No es posible inactivar el control, por que se encuentra aprobado";
                    resultado[2] = poCabControl;
                }
                else
                {
                    BuscarCabeceraControl.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    BuscarCabeceraControl.FechaModificacionLog = poCabControl.FechaIngresoLog;
                    BuscarCabeceraControl.UsuarioModificacionLog = poCabControl.UsuarioIngresoLog;
                    BuscarCabeceraControl.TerminalModificacionLog = poCabControl.TerminalIngresoLog;
                    db.SaveChanges();
                    resultado[0] = "002";
                    resultado[1] = "Registro Inactivado con éxito";
                    resultado[2] = poCabControl;
                }
                return resultado;
            }
         }
        public object[] GuardarDetalleControl(CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarabecera = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Find(poDetalleControl.IdCabeceraEvaluacionLomosYMigasEnBandeja);
                if (buscarabecera.EstadoControl == true)
                {
                    resultado[0] = "003";
                    resultado[1] = "El control se encuetra aprobado, no puede ser modificado";
                    resultado[2] = poDetalleControl;
                }
                else
                {
                    var buscarDetalle = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE.Where(x => x.Hora == poDetalleControl.Hora && poDetalleControl.IdCabeceraEvaluacionLomosYMigasEnBandeja == x.IdCabeceraEvaluacionLomosYMigasEnBandeja).FirstOrDefault();

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
                }
                
                return resultado;
            }
        }
        public object[] ActualizarDetalleControl(CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                
                var buscarabecera = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Find(poDetalleControl.IdCabeceraEvaluacionLomosYMigasEnBandeja);
                if (buscarabecera.EstadoControl == true)
                {
                    resultado[0] = "003";
                    resultado[1] = "El control se encuetra aprobado, no puede ser modificado";
                    resultado[2] = poDetalleControl;
                }
                else
                {
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
                    //buscardetalle.HematomasProfundos = poDetalleControl.HematomasProfundos;
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
                }
                return resultado;
            }
        }
        public object[] InactivarDetalle(CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarCabecera = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Find(poDetalleControl.IdCabeceraEvaluacionLomosYMigasEnBandeja);
                if(buscarCabecera.EstadoControl==true)
                {
                    resultado[0] = "003";
                    resultado[1] = "No se pudo inactivar el registro, por que se encuentra Aprobado";
                    resultado[2] = poDetalleControl;
                }
                else
                {
                    var buscarDetalle = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE.Find(poDetalleControl.IdDetalleEvaluacionLomoyMigasEnBandeja);
                    buscarDetalle.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    buscarDetalle.FechaModificacionLog = poDetalleControl.FechaIngresoLog;
                    buscarDetalle.UsuarioModificacionLog = poDetalleControl.UsuarioIngresoLog;
                    buscarDetalle.TerminalModificacionLog = poDetalleControl.TerminalIngresoLog;
                    db.SaveChanges();
                    resultado[0] = "002";
                    resultado[1] = "Registro Inactivado con éxito";
                    resultado[2] = poDetalleControl;
                }
                return resultado;
            }
        }
        public string AprobarControl(int idCabecera,string usuario,string terminal,DateTime Fecha/*,byte[] firma*/)
        {
            using (var db = new ASIS_PRODEntities())
            {

                var buscarCabecera = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Find(idCabecera);
                buscarCabecera.FechaModificacionLog = DateTime.Now;
                buscarCabecera.UsuarioModificacionLog = usuario;
                buscarCabecera.TerminalModificacionLog = terminal;
                buscarCabecera.AprobadoPor = usuario;
                buscarCabecera.FechaAprobacion = Fecha;
                buscarCabecera.EstadoControl = true;
                //buscarCabecera.FirmaAprobacion = firma;
                db.SaveChanges();
           
                return "El control ha sido aprobado";
            }
        }
        public  List<DetalleEvaluacionLomosMIgasBandejaViewModel> ConsultarDetalleControl(int idCabeceraControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                var resultado = (from d in db.CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE
                                 join c in db.CC_MANTENIMIENTO_COLOR on new { d.Color, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { Color = c.IdColor, c.EstadoRegistro }
                                 join o in db.CC_MANTENIMIENTO_OLOR on new { d.Olor, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { Olor = o.IdOlor, o.EstadoRegistro }
                                 join s in db.CC_MANTENIMIENTO_SABOR on new { d.Sabor, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { Sabor = s.IdSabor, s.EstadoRegistro }
                                 join t in db.CC_MANTENIMIENTO_TEXTURA on new { d.Textura, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { Textura = t.IdTextura, t.EstadoRegistro }
                                 join p in db.CC_MANTENIMIENTO_PROTEINA on new { d.Proteina, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { Proteina = p.IdProteina, p.EstadoRegistro }
                                 join cab in db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA on new { IdEvaluacionDeLomosYMigasEnBandejas=d.IdCabeceraEvaluacionLomosYMigasEnBandeja, EstadoRegistro =clsAtributos.EstadoRegistroActivo} equals new {cab.IdEvaluacionDeLomosYMigasEnBandejas, cab.EstadoRegistro }
                                 where d.IdCabeceraEvaluacionLomosYMigasEnBandeja == idCabeceraControl && d.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                 select new DetalleEvaluacionLomosMIgasBandejaViewModel
                                 {
                                     FechaControl=cab.FechaProduccion,
                                     Buque = d.buque,
                                     CodColor = c.IdColor,
                                     CodOlor = o.IdOlor,
                                     CodProteinas = p.IdProteina,
                                     CodSabor = s.IdSabor,
                                     CodTextura = t.IdTextura,
                                     Color = c.Descripcion,
                                     Escamas = d.Escamas,
                                     Espinas = d.Espinas,
                                   //  HematomasProfundos = d.HematomasProfundos,
                                     //Hora = d.Hora,
                                     Linea = d.Linea,
                                     Lote = d.Lote,
                                     Moretones = d.Moretones,
                                     Olor = o.Descripcion,
                                     Piel = d.Piel,
                                     Proteinas = p.Descripcion,
                                     Sabor = s.Descripcion,
                                     Sangre = d.Sangre,
                                     Textura = t.Descripcion,
                                     Trozos = d.Trozo,
                                     Venas = d.Venas,
                                     IdDetalle = d.IdDetalleEvaluacionLomoyMigasEnBandeja,
                                     IdCabecera=idCabeceraControl,
                                     Aprobado=cab.EstadoControl
                                 }).ToList();
                return resultado;
            }
        }
        public List<spReporteEvaluacionLomosMigasBandeja> ConsultarReporte(int IdEvaluacionDeLomosYMigasEnBandejas)
        {
            using (var db = new ASIS_PRODEntities())
            {
               return db.spReporteEvaluacionLomosMigasBandeja(IdEvaluacionDeLomosYMigasEnBandejas).ToList();
            }
        }
        public List<CabeceraEvaluacionLomosMigasViewModel> ConsultarBandejaEvaluacionLomosyMiga(DateTime? FechaInicio, DateTime? FechaFin,bool EstadoControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                if (EstadoControl == clsAtributos.EstadoReportePendiente)
                {
                    //return db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Where(x => (x.EstadoRegistro == clsAtributos.EstadoRegistroActivo & x.EstadoControl == clsAtributos.EstadoReportePendiente)).ToList();
                    var respuesta = (from x in db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA
                                     join cl in db.CLASIFICADOR on new { Codigo = x.NivelLimpieza, Grupo = "008", EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { cl.Codigo, cl.Grupo, cl.EstadoRegistro }
                                     join d in db.CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE on new { IdCabeceraEvaluacionLomosYMigasEnBandeja=x.IdEvaluacionDeLomosYMigasEnBandejas, EstadoRegistro=clsAtributos.EstadoRegistroActivo } equals new {d.IdCabeceraEvaluacionLomosYMigasEnBandeja,  d.EstadoRegistro }
                                     where x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && (x.EstadoControl == clsAtributos.EstadoReportePendiente || x.EstadoControl==null) && cl.Codigo != "0"
                                     select new CabeceraEvaluacionLomosMigasViewModel
                                     {
                                         Cliente = x.Cliente,
                                         Empaque = x.Empaque,
                                         Enlatado = x.Enlatado,
                                         EstadoControl = x.EstadoControl,
                                         EstadoRegistro = x.EstadoRegistro,
                                         FechaIngresoLog = x.FechaIngresoLog,
                                         FechaModificacionLog = x.FechaModificacionLog,
                                         FechaProduccion = x.FechaProduccion,
                                         IdEvaluacionDeLomosYMigasEnBandejas = x.IdEvaluacionDeLomosYMigasEnBandejas,
                                         Lomo = x.Lomo,
                                         Miga = x.Miga,
                                         NivelLimpieza = x.NivelLimpieza,
                                         Observacion = x.Observacion,
                                         OrdenFabricacion = x.OrdenFabricacion,
                                         Pouch = x.Pouch,
                                         TerminalIngresoLog = x.TerminalIngresoLog,
                                         TerminalModificacionLog = x.TerminalModificacionLog,
                                         UsuarioIngresoLog = x.UsuarioIngresoLog,
                                         UsuarioModificacionLog = x.UsuarioModificacionLog,
                                         NivelLimpiezaDescripcion=cl.Descripcion,
                                         AprobadoPor=x.AprobadoPor,
                                         FechaAprobacion=x.FechaAprobacion
                                     }).Distinct().ToList();

                    return respuesta;
                }
                else
                {
                    var respuesta = (from x in db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA
                                     join cl in db.CLASIFICADOR on new { Codigo = x.NivelLimpieza, Grupo = "008", EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { cl.Codigo, cl.Grupo, cl.EstadoRegistro }
                                     join d in db.CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE on new { IdCabeceraEvaluacionLomosYMigasEnBandeja = x.IdEvaluacionDeLomosYMigasEnBandejas, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { d.IdCabeceraEvaluacionLomosYMigasEnBandeja, d.EstadoRegistro }
                                     where x.EstadoRegistro == clsAtributos.EstadoRegistroActivo &&(x.FechaProduccion>=FechaInicio&&x.FechaProduccion<=FechaFin) && x.EstadoControl == clsAtributos.EstadoReporteActivo  && cl.Codigo != "0"
                                     select new CabeceraEvaluacionLomosMigasViewModel
                                     {
                                         Cliente = x.Cliente,
                                         Empaque = x.Empaque,
                                         Enlatado = x.Enlatado,
                                         EstadoControl = x.EstadoControl,
                                         EstadoRegistro = x.EstadoRegistro,
                                         FechaIngresoLog = x.FechaIngresoLog,
                                         FechaModificacionLog = x.FechaModificacionLog,
                                         FechaProduccion = x.FechaProduccion,
                                         IdEvaluacionDeLomosYMigasEnBandejas = x.IdEvaluacionDeLomosYMigasEnBandejas,
                                         Lomo = x.Lomo,
                                         Miga = x.Miga,
                                         NivelLimpieza = x.NivelLimpieza,
                                         Observacion = x.Observacion,
                                         OrdenFabricacion = x.OrdenFabricacion,
                                         Pouch = x.Pouch,
                                         TerminalIngresoLog = x.TerminalIngresoLog,
                                         TerminalModificacionLog = x.TerminalModificacionLog,
                                         UsuarioIngresoLog = x.UsuarioIngresoLog,
                                         UsuarioModificacionLog = x.UsuarioModificacionLog,
                                         NivelLimpiezaDescripcion = cl.Descripcion,
                                         AprobadoPor = x.AprobadoPor,
                                         FechaAprobacion = x.FechaAprobacion
                                     }).Distinct().ToList();
                    return respuesta;
                }
            }

        }
        public string ReversarControl(int IdControl, string usuario, string terminal)
        {
            using (var db = new ASIS_PRODEntities())
            {

                var buscarControl = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Find(IdControl);
                buscarControl.FechaModificacionLog = DateTime.Now;
                buscarControl.UsuarioModificacionLog = usuario;
                buscarControl.TerminalModificacionLog = terminal;
                buscarControl.AprobadoPor = null;
                buscarControl.FechaAprobacion = null;
                buscarControl.EstadoControl = false;

                db.SaveChanges();

                return "El control ha sido Reversado";
            }
        }
        public List<CabeceraEvaluacionLomosMigasViewModel> ConsultarCabReportes(DateTime FechaDesde,DateTime FechaHasta)
        {
            using (var db = new ASIS_PRODEntities())
            {
                var respuesta = (from x in db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA
                                 join cl in db.CLASIFICADOR on new { Codigo = x.NivelLimpieza, Grupo = "008", EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { cl.Codigo, cl.Grupo, cl.EstadoRegistro }
                                 join d in db.CC_EVALUACION_LOMO_MIGA_BANDEJA_DETALLE on new { IdCabeceraEvaluacionLomosYMigasEnBandeja = x.IdEvaluacionDeLomosYMigasEnBandejas, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { d.IdCabeceraEvaluacionLomosYMigasEnBandeja, d.EstadoRegistro }
                                 where x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && (x.FechaProduccion>=FechaDesde&&x.FechaProduccion<=FechaHasta) && cl.Codigo != "0"
                                 select new CabeceraEvaluacionLomosMigasViewModel
                                 {
                                     Cliente = x.Cliente,
                                     Empaque = x.Empaque,
                                     Enlatado = x.Enlatado,
                                     EstadoControl = x.EstadoControl,
                                     EstadoRegistro = x.EstadoRegistro,
                                     FechaIngresoLog = x.FechaIngresoLog,
                                     FechaModificacionLog = x.FechaModificacionLog,
                                     FechaProduccion = x.FechaProduccion,
                                     IdEvaluacionDeLomosYMigasEnBandejas = x.IdEvaluacionDeLomosYMigasEnBandejas,
                                     Lomo = x.Lomo,
                                     Miga = x.Miga,
                                     NivelLimpieza = x.NivelLimpieza,
                                     Observacion = x.Observacion,
                                     OrdenFabricacion = x.OrdenFabricacion,
                                     Pouch = x.Pouch,
                                     TerminalIngresoLog = x.TerminalIngresoLog,
                                     TerminalModificacionLog = x.TerminalModificacionLog,
                                     UsuarioIngresoLog = x.UsuarioIngresoLog,
                                     UsuarioModificacionLog = x.UsuarioModificacionLog,
                                     NivelLimpiezaDescripcion = cl.Descripcion,
                                     AprobadoPor = x.AprobadoPor,
                                     FechaAprobacion = x.FechaAprobacion
                                 }).Distinct().ToList();

                return respuesta;
            }
        }
        //public string GuardarImagenFirma(byte[] firma, int IdCabecera, string Tipo,string Usuario, string Terminal)
        //{
        //    using (var db = new ASIS_PRODEntities())
        //    {
        //        var buscarControl = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Find(IdCabecera);
        //        if(buscarControl.EstadoControl==true)
        //        {
        //            return "El control ya se encuentra aprobado, no puede se rmodificado";
        //        }
        //        else
        //        {
        //            //if (Tipo == "Control")
        //            //{
        //            //    buscarControl.FirmaControl = firma;
        //            //}
        //            //else
        //            //{
        //            //    buscarControl.FirmaAprobacion = firma;
        //            //}
        //            buscarControl.FechaModificacionLog = DateTime.Now;
        //            buscarControl.UsuarioModificacionLog = Usuario;
        //            buscarControl.TerminalModificacionLog = Terminal;
        //            db.SaveChanges();
        //            return "Firma guardada correctamente";
        //        }
        //    }
        //}
        //public byte[] ConsultarFirmaControl(int IdCabecera)
        //{
        //    using (var db = new ASIS_PRODEntities())
        //    {
        //        var buscarControl = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Find(IdCabecera);
        //        return buscarControl.FirmaControl;
        //    }
        //}
        //public object[] ConsultarFirmaAprobacion(int IdCabecera)
        //{
        //    using (var db = new ASIS_PRODEntities())
        //    {
        //        var buscarControl = db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Find(IdCabecera);
        //        object[] resultado = new object[2];
        //        resultado[0] = buscarControl.FirmaAprobacion;
        //        resultado[1] = buscarControl.EstadoControl;
        //        return resultado;
        //    }
        //}
    }
}