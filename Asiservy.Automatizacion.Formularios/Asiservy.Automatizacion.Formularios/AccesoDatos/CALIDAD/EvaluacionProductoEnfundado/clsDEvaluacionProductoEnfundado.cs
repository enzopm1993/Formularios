using System;
using System.Collections.Generic;
using System.Linq;
using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.CALIDAD;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.EvaluacionProductoEnfundado
{
    public class clsDEvaluacionProductoEnfundado
    {
        public object[] GuardarCabeceraControl(CC_EVALUACION_PRODUCTO_ENFUNDADO poCabeceraControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarCabecera = db.CC_EVALUACION_PRODUCTO_ENFUNDADO.Where(x => x.FechaProduccion == poCabeceraControl.FechaProduccion &&
                x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (buscarCabecera == null)
                {
                    db.CC_EVALUACION_PRODUCTO_ENFUNDADO.Add(poCabeceraControl);
                    db.SaveChanges();
                    resultado[0] = "000";
                    resultado[1] = "Registro ingresado con éxito";
                    resultado[2] = poCabeceraControl;
                }
                else
                {
                    resultado[0] = "002";
                    resultado[1] = "Error, el registro ya existe";
                    resultado[2] = poCabeceraControl;
                }
                return resultado;
            }
        }
        public CC_EVALUACION_PRODUCTO_ENFUNDADO ConsultarCabeceraControl(DateTime FechaProduccion)
        {
            using (var db = new ASIS_PRODEntities())
            {
                return db.CC_EVALUACION_PRODUCTO_ENFUNDADO.Where(x => x.FechaProduccion == FechaProduccion && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
            }
        }
        public object[] ActualizarCabeceraControl(CC_EVALUACION_PRODUCTO_ENFUNDADO poCabControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabecera = db.CC_EVALUACION_PRODUCTO_ENFUNDADO.Find(poCabControl.IdEvaluacionProductoEnfundado);
                if (BuscarCabecera.EstadoControl == true)
                {
                    resultado[0] = "003";
                    resultado[1] = "No es posible actualizar el control, por que ya se encuentra aprobado";
                    resultado[2] = poCabControl;
                }
                else
                {
                    BuscarCabecera.Lomo = poCabControl.Lomo;
                    BuscarCabecera.Miga = poCabControl.Miga;
                    BuscarCabecera.Trozo = poCabControl.Trozo;
                    BuscarCabecera.Cliente = poCabControl.Cliente;
                    BuscarCabecera.Marca = poCabControl.Marca;
                    BuscarCabecera.Destino = poCabControl.Destino;
                    BuscarCabecera.Proveedor = poCabControl.Proveedor;
                    BuscarCabecera.Batch = poCabControl.Batch;
                    BuscarCabecera.Lote = poCabControl.Lote;
                    BuscarCabecera.NivelLimpieza = poCabControl.NivelLimpieza;
                    BuscarCabecera.Observacion = poCabControl.Observacion;
                    BuscarCabecera.OrdenFabricacion = poCabControl.OrdenFabricacion;
                    BuscarCabecera.FechaModificacionLog = poCabControl.FechaIngresoLog;
                    BuscarCabecera.UsuarioModificacionLog = poCabControl.UsuarioIngresoLog;
                    BuscarCabecera.TerminalIngresoLog = poCabControl.TerminalIngresoLog;
                    if (!string.IsNullOrEmpty(poCabControl.ImagenCodigo))
                        BuscarCabecera.ImagenCodigo = poCabControl.ImagenCodigo;
                    if (!string.IsNullOrEmpty(poCabControl.ImagenProducto1))
                        BuscarCabecera.ImagenProducto1 = poCabControl.ImagenProducto1;
                    if (!string.IsNullOrEmpty(poCabControl.ImagenProducto2))
                        BuscarCabecera.ImagenProducto2 = poCabControl.ImagenProducto2;
                    if (!string.IsNullOrEmpty(poCabControl.ImagenProducto3))
                        BuscarCabecera.ImagenProducto3 = poCabControl.ImagenProducto3;
                    BuscarCabecera.RotacionImagenCod = poCabControl.RotacionImagenCod;
                    BuscarCabecera.RotacionImagenProd1 = poCabControl.RotacionImagenProd1;
                    BuscarCabecera.RotacionImagenProd2 = poCabControl.RotacionImagenProd2;
                    BuscarCabecera.RotacionImagenProd3 = poCabControl.RotacionImagenProd3;
                    db.SaveChanges();
                    resultado[0] = "001";
                    resultado[1] = "Registro actualizado con éxito";
                    resultado[2] = poCabControl;
                }
                return resultado;
            }
        }
        public object[] InactivarCabeceraControl(CC_EVALUACION_PRODUCTO_ENFUNDADO poCabControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabeceraControl = db.CC_EVALUACION_PRODUCTO_ENFUNDADO.Find(poCabControl.IdEvaluacionProductoEnfundado);
                if (BuscarCabeceraControl.EstadoControl == true)
                {
                    resultado[0] = "003";
                    resultado[1] = "No se pudo inactivar el registro, por que ya se encuentra aprobado";
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
        public object[] GuardarDetalleControl(CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarDetalle = db.CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE.Where(x => x.Hora == poDetalleControl.Hora).FirstOrDefault();
                if (buscarDetalle == null)
                {
                    db.CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE.Add(poDetalleControl);
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
        public object[] ActualizarDetalleControl(CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarCabecera = db.CC_EVALUACION_PRODUCTO_ENFUNDADO.Find(poDetalleControl.IdCabeceraEvaluacionProductoEnfundado);
                if(buscarCabecera.EstadoControl==true)
                {
                    resultado[0] = "003";
                    resultado[1] = "No se puede actualizar el registro, por que el control ya se encuentra aprobado";
                    resultado[2] = poDetalleControl;
                }
                else
                {
                    var buscardetalle = db.CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE.Find(poDetalleControl.IdDetalleEvaluacionProductoEnfundado);
                    buscardetalle.FechaModificacionLog = poDetalleControl.FechaIngresoLog;
                    buscardetalle.UsuarioModificacionLog = poDetalleControl.UsuarioIngresoLog;
                    buscardetalle.TerminalModificacionLog = poDetalleControl.TerminalIngresoLog;
                    buscardetalle.Empacador = poDetalleControl.Empacador;
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
                    buscardetalle.Otro = poDetalleControl.Otro;
                    buscardetalle.Miga = poDetalleControl.Miga;
                    db.SaveChanges();
                    db.SaveChanges();
                    resultado[0] = "001";
                    resultado[1] = "Registro actualizado con éxito";
                    resultado[2] = poDetalleControl;
                }
                return resultado;
            }
        }
        public object[] InactivarDetalle(CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarCabecera = db.CC_EVALUACION_PRODUCTO_ENFUNDADO.Find(poDetalleControl.IdCabeceraEvaluacionProductoEnfundado);
                if(buscarCabecera.EstadoControl==true)
                {
                    resultado[0] = "003";
                    resultado[1] = "No se puede Inactivar el registro, por que el control se encuentra aprobado";
                    resultado[2] = poDetalleControl;
                }
                else
                {
                    var buscarDetalle = db.CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE.Find(poDetalleControl.IdDetalleEvaluacionProductoEnfundado);
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
        public List<DetalleEvaluacionProductoEnfundadoViewModel> ConsultarDetalleControl(int idCabeceraControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                IEnumerable<spConsutaEmpleadosFiltro> pListEmpleados;
                pListEmpleados = db.spConsutaEmpleadosFiltro("0", "0", clsAtributos.CargoEmpacado).ToList();
                var resultado = (from d in db.CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE
                                 join c in db.CC_MANTENIMIENTO_COLOR on new { d.Color, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { Color = c.IdColor, c.EstadoRegistro }
                                 join o in db.CC_MANTENIMIENTO_OLOR on new { d.Olor, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { Olor = o.IdOlor, o.EstadoRegistro }
                                 join s in db.CC_MANTENIMIENTO_SABOR on new { d.Sabor, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { Sabor = s.IdSabor, s.EstadoRegistro }
                                 join t in db.CC_MANTENIMIENTO_TEXTURA on new { d.Textura, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { Textura = t.IdTextura, t.EstadoRegistro }
                                 join p in db.CC_MANTENIMIENTO_PROTEINA on new { d.Proteina, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { Proteina = p.IdProteina, p.EstadoRegistro }
                                 join cab in db.CC_EVALUACION_PRODUCTO_ENFUNDADO on new { IdEvaluacionProductoEnfundado = d.IdCabeceraEvaluacionProductoEnfundado, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { cab.IdEvaluacionProductoEnfundado, cab.EstadoRegistro }
                                 //join emp in pListEmpleados on d.Empacador equals emp.CEDULA
                                 where d.IdCabeceraEvaluacionProductoEnfundado == idCabeceraControl && d.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                 select new DetalleEvaluacionProductoEnfundadoViewModel
                                 {
                                     Buque = d.buque,
                                     CodColor = c.IdColor,
                                     CodOlor = o.IdOlor,
                                     CodProteinas = p.IdProteina,
                                     CodSabor = s.IdSabor,
                                     CodTextura = t.IdTextura,
                                     Color = c.Descripcion,
                                     Escamas = d.Escamas,
                                     Espinas = d.Espinas,
                                     HematomasProfundos = d.HematomasProfundos,
                                     Hora = d.Hora,
                                     empacador=d.Empacador,
                                     
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
                                     IdDetalle = d.IdDetalleEvaluacionProductoEnfundado,
                                     IdCabecera = idCabeceraControl,
                                     Aprobado = cab.EstadoControl,
                                     Miga=d.Miga,
                                     Otro=d.Otro,
                                     FechaControl=cab.FechaProduccion

                                 }).ToList();
                var ResultadoFInal = (from r in resultado
                                      join e in pListEmpleados on r.empacador equals e.CEDULA
                                      select new DetalleEvaluacionProductoEnfundadoViewModel
                                      {
                                          Buque = r.Buque,
                                          CodColor = r.CodColor,
                                          CodOlor = r.CodOlor,
                                          CodProteinas = r.CodProteinas,
                                          CodSabor = r.CodSabor,
                                          CodTextura = r.CodTextura,
                                          Color = r.Color,
                                          Escamas = r.Escamas,
                                          Espinas = r.Espinas,
                                          HematomasProfundos = r.HematomasProfundos,
                                          Hora = r.Hora,
                                          empacador = r.empacador,
                                          NombreEmpacador = e.NOMBRES,
                                          Lote = r.Lote,
                                          Moretones = r.Moretones,
                                          Olor = r.Olor,
                                          Piel = r.Piel,
                                          Proteinas = r.Proteinas,
                                          Sabor = r.Sabor,
                                          Sangre = r.Sangre,
                                          Textura = r.Textura,
                                          Trozos = r.Trozos,
                                          Venas = r.Venas,
                                          IdDetalle = r.IdDetalle,
                                          IdCabecera = idCabeceraControl,
                                          Aprobado = r.Aprobado,
                                          Miga = r.Miga,
                                          Otro = r.Otro,
                                          FechaControl = r.FechaControl
                                      }).ToList();
                return ResultadoFInal;
            }
        }
        //public string GuardarImagenFirma(byte[] firma, int IdCabecera, string Tipo, string Usuario, string Terminal)
        //{
        //    using (var db = new ASIS_PRODEntities())
        //    {
        //        var buscarControl = db.CC_EVALUACION_PRODUCTO_ENFUNDADO.Find(IdCabecera);
        //        if (Tipo == "Control")
        //        {
        //            buscarControl.FirmaControl = firma;
        //        }
        //        else
        //        {
        //            buscarControl.FirmaAprobacion = firma;
        //        }
        //        buscarControl.FechaModificacionLog = DateTime.Now;
        //        buscarControl.UsuarioModificacionLog = Usuario;
        //        buscarControl.TerminalModificacionLog = Terminal;
        //        db.SaveChanges();
        //        return "Firma guardada correctamente";
        //    }
        //}
        //public byte[] ConsultarFirmaControl(int IdCabecera)
        //{
        //    using (var db = new ASIS_PRODEntities())
        //    {
        //        var buscarControl = db.CC_EVALUACION_PRODUCTO_ENFUNDADO.Find(IdCabecera);
        //        return buscarControl.FirmaControl;
        //    }
        //}
        public List<CabeceraEvaluacionProductoEnfundadoViewModel> ConsultarBandejaEvaluacionLomosyMiga(DateTime? FechaInicio, DateTime? FechaFin, bool EstadoControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                if (EstadoControl == clsAtributos.EstadoReportePendiente)
                {
                    //return db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Where(x => (x.EstadoRegistro == clsAtributos.EstadoRegistroActivo & x.EstadoControl == clsAtributos.EstadoReportePendiente)).ToList();
                    var respuesta = (from x in db.CC_EVALUACION_PRODUCTO_ENFUNDADO
                                     join cl in db.CLASIFICADOR on new { Codigo = x.NivelLimpieza, Grupo = "008", EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { cl.Codigo, cl.Grupo, cl.EstadoRegistro }
                                     join d in db.CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE on new { IdCabeceraEvaluacionProductoEnfundado = x.IdEvaluacionProductoEnfundado, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { d.IdCabeceraEvaluacionProductoEnfundado, d.EstadoRegistro }
                                     where x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && (x.EstadoControl == clsAtributos.EstadoReportePendiente||x.EstadoControl==null) && cl.Codigo != "0"
                                     select new CabeceraEvaluacionProductoEnfundadoViewModel
                                     {
                                         Cliente = x.Cliente,
                                         Trozo = x.Trozo,
                                         EstadoControl = x.EstadoControl,
                                         EstadoRegistro = x.EstadoRegistro,
                                         FechaIngresoLog = x.FechaIngresoLog,
                                         FechaModificacionLog = x.FechaModificacionLog,
                                         FechaProduccion = x.FechaProduccion,
                                         IdEvaluacionProductoEnfundado = x.IdEvaluacionProductoEnfundado,
                                         Lomo = x.Lomo,
                                         Miga = x.Miga,
                                         NivelLimpieza = x.NivelLimpieza,
                                         Observacion = x.Observacion,
                                         OrdenFabricacion = x.OrdenFabricacion,
                                         TerminalIngresoLog = x.TerminalIngresoLog,
                                         TerminalModificacionLog = x.TerminalModificacionLog,

                                         UsuarioIngresoLog = x.UsuarioIngresoLog,
                                         UsuarioModificacionLog = x.UsuarioModificacionLog,
                                         NivelLimpiezaDescripcion = cl.Descripcion,
                                         AprobadoPor = x.AprobadoPor,
                                         FechaAprobacion = x.FechaAprobacion,
                                         Batch=x.Batch,
                                         Destino=x.Destino,
                                         Lote=x.Lote,
                                         Marca=x.Marca,
                                         Proveedor=x.Proveedor,
                                         ImagenCodigo=x.ImagenCodigo,
                                         ImagenProducto1=x.ImagenProducto1,
                                         ImagenProducto2=x.ImagenProducto2,
                                         ImagenProducto3=x.ImagenProducto3,
                                         RotacionImagenCod=x.RotacionImagenCod,
                                         RotacionImagenProd1=x.RotacionImagenProd1,
                                         RotacionImagenProd2=x.RotacionImagenProd2,
                                         RotacionImagenProd3=x.RotacionImagenProd3
                                     }).Distinct().ToList();

                    return respuesta;
                }
                else
                {
                    var respuesta = (from x in db.CC_EVALUACION_PRODUCTO_ENFUNDADO
                                     join cl in db.CLASIFICADOR on new { Codigo = x.NivelLimpieza, Grupo = "008", EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { cl.Codigo, cl.Grupo, cl.EstadoRegistro }
                                     join d in db.CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE on new { IdCabeceraEvaluacionProductoEnfundado = x.IdEvaluacionProductoEnfundado, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { d.IdCabeceraEvaluacionProductoEnfundado, d.EstadoRegistro }
                                     where x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && (x.FechaProduccion >= FechaInicio && x.FechaProduccion <= FechaFin) && x.EstadoControl == clsAtributos.EstadoReporteActivo && cl.Codigo != "0"
                                     select new CabeceraEvaluacionProductoEnfundadoViewModel
                                     {
                                         Cliente = x.Cliente,
                                         Trozo = x.Trozo,
                                         EstadoControl = x.EstadoControl,
                                         EstadoRegistro = x.EstadoRegistro,
                                         FechaIngresoLog = x.FechaIngresoLog,
                                         FechaModificacionLog = x.FechaModificacionLog,
                                         FechaProduccion = x.FechaProduccion,
                                         IdEvaluacionProductoEnfundado = x.IdEvaluacionProductoEnfundado,
                                         Lomo = x.Lomo,
                                         Miga = x.Miga,
                                         NivelLimpieza = x.NivelLimpieza,
                                         Observacion = x.Observacion,
                                         OrdenFabricacion = x.OrdenFabricacion,
                                         TerminalIngresoLog = x.TerminalIngresoLog,
                                         TerminalModificacionLog = x.TerminalModificacionLog,
                                         UsuarioIngresoLog = x.UsuarioIngresoLog,
                                         UsuarioModificacionLog = x.UsuarioModificacionLog,
                                         NivelLimpiezaDescripcion = cl.Descripcion,
                                         AprobadoPor = x.AprobadoPor,
                                         FechaAprobacion = x.FechaAprobacion,
                                         Batch = x.Batch,
                                         Destino = x.Destino,
                                         Lote = x.Lote,
                                         Marca = x.Marca,
                                         Proveedor = x.Proveedor
                                     }).Distinct().ToList();
                    return respuesta;
                }
            }

        }
        public string AprobarControl(int idCabecera, string usuario, string terminal,DateTime Fecha)
        {
            using (var db = new ASIS_PRODEntities())
            {

                var buscarCabecera = db.CC_EVALUACION_PRODUCTO_ENFUNDADO.Find(idCabecera);
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
        //public object[] ConsultarFirmaAprobacion(int IdCabecera)
        //{
        //    using (var db = new ASIS_PRODEntities())
        //    {
        //        var buscarControl = db.CC_EVALUACION_PRODUCTO_ENFUNDADO.Find(IdCabecera);
        //        object[] resultado = new object[2];
        //        resultado[0] = buscarControl.FirmaAprobacion;
        //        resultado[1] = buscarControl.EstadoControl;
        //        return resultado;
        //    }
        //}
        public List<spReporteEvaluacionProductoEnfundado> ConsultarReporte(DateTime Fecha)
        {
            using (var db = new ASIS_PRODEntities())
            {
                return db.spReporteEvaluacionProductoEnfundado(Fecha).ToList();
            }
        }
        public CC_EVALUACION_PRODUCTO_ENFUNDADO ConsultarFotosControl(int IdControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                return db.CC_EVALUACION_PRODUCTO_ENFUNDADO.Find(IdControl);
            }
        }
        public string ReversarControl(int IdControl, string usuario, string terminal)
        {
            using (var db = new ASIS_PRODEntities())
            {

                var buscarControl = db.CC_EVALUACION_PRODUCTO_ENFUNDADO.Find(IdControl);
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
        public List<CabeceraEvaluacionProductoEnfundadoViewModel> ConsultarCabReportes(DateTime FechaDesde, DateTime FechaHasta)
        {
            using (var db = new ASIS_PRODEntities())
            {
                var respuesta = (from x in db.CC_EVALUACION_PRODUCTO_ENFUNDADO
                                 join cl in db.CLASIFICADOR on new { Codigo = x.NivelLimpieza, Grupo = "008", EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { cl.Codigo, cl.Grupo, cl.EstadoRegistro }
                                 join d in db.CC_EVALUACION_PRODUCTO_ENFUNDADO_DETALLE on new { IdCabeceraEvaluacionProductoEnfundado = x.IdEvaluacionProductoEnfundado, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { d.IdCabeceraEvaluacionProductoEnfundado, d.EstadoRegistro }
                                 where x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && (x.FechaProduccion >= FechaDesde && x.FechaProduccion <= FechaHasta) && cl.Codigo != "0"
                                 select new CabeceraEvaluacionProductoEnfundadoViewModel
                                 {
                                     Cliente = x.Cliente,
                           
                                     EstadoControl = x.EstadoControl,
                                     EstadoRegistro = x.EstadoRegistro,
                                     FechaIngresoLog = x.FechaIngresoLog,
                                     FechaModificacionLog = x.FechaModificacionLog,
                                     FechaProduccion = x.FechaProduccion,
                                     IdEvaluacionProductoEnfundado = x.IdEvaluacionProductoEnfundado,
                                     Lomo = x.Lomo,
                                     Miga = x.Miga,
                                     NivelLimpieza = x.NivelLimpieza,
                                     Observacion = x.Observacion,
                                     OrdenFabricacion = x.OrdenFabricacion,
                                     Marca=x.Marca,
                                     Destino=x.Destino,
                                     Proveedor=x.Proveedor,
                                     Lote=x.Proveedor,
                                     Batch=x.Batch,
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
}