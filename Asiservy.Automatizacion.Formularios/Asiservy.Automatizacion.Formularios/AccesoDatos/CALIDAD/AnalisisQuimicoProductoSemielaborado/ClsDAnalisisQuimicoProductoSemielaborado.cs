using Asiservy.Automatizacion.Datos.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.CALIDAD.AnalisisQuimicoProductoSemielaborado
{
    public class ClsDAnalisisQuimicoProductoSemielaborado
    {
        public CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA ConsultarCabeceraControl(DateTime FechaProduccion,string Turno)
        {
            using (var db = new ASIS_PRODEntities())
            {
                return db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA.Where(x => x.Fecha == FechaProduccion&&x.Turno==Turno&& x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
            }
        }
        public object[] GuardarCabeceraControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA poCabeceraControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarCabecera = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA.Where(x => x.Fecha == poCabeceraControl.Fecha &&
                x.Turno==poCabeceraControl.Turno&&x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (buscarCabecera == null)
                {
                    db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA.Add(poCabeceraControl);
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
        public object[] ActualizarCabeceraControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA poCabControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabecera = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA.Find(poCabControl.IdAnalisisQuimicoProductoSe);
                if (BuscarCabecera.EstadoControl == true)
                {
                    resultado[0] = "003";
                    resultado[1] = "El control ya se encuentra aprobado, no puede ser modificado";
                    resultado[2] = new
                    {
                        BuscarCabecera.IdAnalisisQuimicoProductoSe,
                        BuscarCabecera.Observacion
                    };
                }
                else
                {
                    BuscarCabecera.Observacion = poCabControl.Observacion;
                    
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
        public object[] InactivarCabeceraControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA poCabControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabeceraControl = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA.Find(poCabControl.IdAnalisisQuimicoProductoSe);
                if (BuscarCabeceraControl.EstadoControl == true)
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
        public object[] GuardarDetalleControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarabecera = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA.Find(poDetalleControl.IdCabeceraAnalisisQuimicoProductoSe);
                if (buscarabecera.EstadoControl == true)
                {
                    resultado[0] = "003";
                    resultado[1] = "El control se encuetra aprobado, no puede ser modificado";
                    resultado[2] = poDetalleControl;
                }
                else
                {
                    var buscarDetalle = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE.Where(x => x.Lote == poDetalleControl.Lote&&x.OrdenFabricacion==poDetalleControl.OrdenFabricacion && poDetalleControl.IdCabeceraAnalisisQuimicoProductoSe == x.IdCabeceraAnalisisQuimicoProductoSe && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();

                    if (buscarDetalle == null)
                    {
                        db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE.Add(poDetalleControl);
                        db.SaveChanges();
                        resultado[0] = "000";
                        resultado[1] = "Registro ingresado con éxito";
                        resultado[2] = new
                        {
                            poDetalleControl.EstadoRegistro,
                            poDetalleControl.FechaIngresoLog,
                            poDetalleControl.FechaModificacionLog,
                            poDetalleControl.IdCabeceraAnalisisQuimicoProductoSe,
                            poDetalleControl.IdDetalleAnalisisQuimicoProductoSe,
                            poDetalleControl.Lote,
                            poDetalleControl.Proveedor,
                            poDetalleControl.Especie,
                            poDetalleControl.Talla,
                            poDetalleControl.Cliente,
                            poDetalleControl.TerminalIngresoLog,
                            poDetalleControl.TerminalModificacionLog,
                            poDetalleControl.UsuarioIngresoLog,
                            poDetalleControl.UsuarioModificacionLog
                        };
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
        public object[] ActualizarDetalleControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];

                var buscarabecera = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA.Find(poDetalleControl.IdCabeceraAnalisisQuimicoProductoSe);
                if (buscarabecera.EstadoControl == true)
                {
                    resultado[0] = "003";
                    resultado[1] = "El control se encuetra aprobado, no puede ser modificado";
                    resultado[2] = poDetalleControl;
                }
                else
                {
                    var buscardetalle = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE.Find(poDetalleControl.IdDetalleAnalisisQuimicoProductoSe);
                    buscardetalle.FechaModificacionLog = poDetalleControl.FechaIngresoLog;
                    buscardetalle.UsuarioModificacionLog = poDetalleControl.UsuarioIngresoLog;
                    buscardetalle.TerminalModificacionLog = poDetalleControl.TerminalIngresoLog;
                    buscardetalle.Proveedor = poDetalleControl.Proveedor;
                    buscardetalle.Especie = poDetalleControl.Especie;
                    buscardetalle.Talla = poDetalleControl.Talla;
                    buscardetalle.Cliente = poDetalleControl.Cliente;
                    db.SaveChanges();
                    db.SaveChanges();
                    resultado[0] = "001";
                    resultado[1] = "Registro actualizado con éxito";
                    resultado[2] = poDetalleControl;
                }
                return resultado;
            }
        }
        public List<CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE> ConsultarDetalleControl(int idCabeceraControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                var resultado = (from d in db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE
                                 join cab in db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA on new { IdAnalisisQuimicoProductoSe = d.IdCabeceraAnalisisQuimicoProductoSe, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { cab.IdAnalisisQuimicoProductoSe, cab.EstadoRegistro }

                                 where d.IdCabeceraAnalisisQuimicoProductoSe == idCabeceraControl && d.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                 select d).ToList();
                return resultado;
            }
        }
        public List<CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO> ConsultarSubDetalleControl(int idDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                var resultado = (from d in db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO
                                 join cab in db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE on new { IdDetalleAnalisisQuimicoProductoSe = d.IdDetalleAnalisisQuimicoProductoSe, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { cab.IdDetalleAnalisisQuimicoProductoSe, cab.EstadoRegistro }

                                 where d.IdDetalleAnalisisQuimicoProductoSe == idDetalleControl && d.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                 select d).ToList();
                return resultado;
            }
        }
        public object[] GuardarSubDetalleControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO poSubDetalle,int IdCabecera)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarabecera = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA.Find(IdCabecera);
                if (buscarabecera.EstadoControl == true)
                {
                    resultado[0] = "003";
                    resultado[1] = "El control se encuetra aprobado, no puede ser modificado";
                    resultado[2] = poSubDetalle;
                }
                else
                {
                  
                        db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO.Add(poSubDetalle);
                        db.SaveChanges();
                        resultado[0] = "000";
                        resultado[1] = "Registro ingresado con éxito";
                        resultado[2] = new
                        {
                            poSubDetalle.EstadoRegistro,
                            poSubDetalle.FechaIngresoLog,
                            poSubDetalle.FechaModificacionLog,
                            poSubDetalle.IdTipoAnalisisQuimicoProductoSe,
                            poSubDetalle.IdDetalleAnalisisQuimicoProductoSe,
                            poSubDetalle.TipoProducto,
                            poSubDetalle.SalEmpaque,
                            poSubDetalle.SalProceso,
                            poSubDetalle.HistaminaEmpaque,
                            poSubDetalle.HistaminaProceso,
                            poSubDetalle.HumedadProceso,
                            poSubDetalle.TerminalIngresoLog,
                            poSubDetalle.TerminalModificacionLog,
                            poSubDetalle.UsuarioIngresoLog,
                            poSubDetalle.UsuarioModificacionLog
                        };
                   
                }

                return resultado;
            }
        }
        public object[] ActualizarSubDetalleControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO poSubDetalleControl,int IdCabecera)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];

                var buscarabecera = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA.Find(IdCabecera);
                if (buscarabecera.EstadoControl == true)
                {
                    resultado[0] = "003";
                    resultado[1] = "El control se encuetra aprobado, no puede ser modificado";
                    resultado[2] = poSubDetalleControl;
                }
                else
                {
                    var buscarSubDetalle = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO.Find(poSubDetalleControl.IdTipoAnalisisQuimicoProductoSe);
                    buscarSubDetalle.FechaModificacionLog = poSubDetalleControl.FechaIngresoLog;
                    buscarSubDetalle.UsuarioModificacionLog = poSubDetalleControl.UsuarioIngresoLog;
                    buscarSubDetalle.TerminalModificacionLog = poSubDetalleControl.TerminalIngresoLog;
                    buscarSubDetalle.TipoProducto = poSubDetalleControl.TipoProducto;
                    buscarSubDetalle.SalEmpaque = poSubDetalleControl.SalEmpaque;
                    buscarSubDetalle.SalProceso = poSubDetalleControl.SalProceso;
                    buscarSubDetalle.HistaminaEmpaque = poSubDetalleControl.HistaminaEmpaque;
                    buscarSubDetalle.HistaminaProceso = poSubDetalleControl.HistaminaProceso;
                    buscarSubDetalle.HumedadProceso = poSubDetalleControl.HumedadProceso;
                    db.SaveChanges();
                    db.SaveChanges();
                    resultado[0] = "001";
                    resultado[1] = "Registro actualizado con éxito";
                    resultado[2] = poSubDetalleControl;
                }
                return resultado;
            }
        }
        public object[] InactivarSubDetalleControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO poSubDetalle, int IdCabecera)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabeceraControl = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA.Find(IdCabecera);
                if (BuscarCabeceraControl.EstadoControl == true)
                {
                    resultado[0] = "003";
                    resultado[1] = "No es posible inactivar el control, por que se encuentra aprobado";
                    resultado[2] = poSubDetalle;
                }
                else
                {
                    var BuscarSubDetalle = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO.FirstOrDefault(x => x.IdTipoAnalisisQuimicoProductoSe == poSubDetalle.IdTipoAnalisisQuimicoProductoSe
                      && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);

                    BuscarSubDetalle.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    BuscarSubDetalle.FechaModificacionLog = poSubDetalle.FechaIngresoLog;
                    BuscarSubDetalle.UsuarioModificacionLog = poSubDetalle.UsuarioIngresoLog;
                    BuscarSubDetalle.TerminalModificacionLog = poSubDetalle.TerminalIngresoLog;
                    db.SaveChanges();
                    resultado[0] = "002";
                    resultado[1] = "Registro Inactivado con éxito";
                    resultado[2] = poSubDetalle;
                }
                return resultado;
            }
        }
        public object[] InactivarSubDetalleControl(CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE poDetalle, int IdCabecera)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabeceraControl = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA.Find(IdCabecera);
                if (BuscarCabeceraControl.EstadoControl == true)
                {
                    resultado[0] = "003";
                    resultado[1] = "No es posible inactivar el control, por que se encuentra aprobado";
                    resultado[2] = poDetalle;
                }
                else
                {
                    var BuscarDetalle = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE.FirstOrDefault(x => x.IdDetalleAnalisisQuimicoProductoSe == poDetalle.IdDetalleAnalisisQuimicoProductoSe
                      && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);

                    BuscarDetalle.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                    BuscarDetalle.FechaModificacionLog = poDetalle.FechaIngresoLog;
                    BuscarDetalle.UsuarioModificacionLog = poDetalle.UsuarioIngresoLog;
                    BuscarDetalle.TerminalModificacionLog = poDetalle.TerminalIngresoLog;
                    db.SaveChanges();
                    resultado[0] = "002";
                    resultado[1] = "Registro Inactivado con éxito";
                    resultado[2] = poDetalle;
                }
                return resultado;
            }
        }
        public List<CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA> ConsultarBandejaAnalisisQuimicoProductoSemielaborado(DateTime? FechaInicio, DateTime? FechaFin, bool EstadoControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                if (EstadoControl == clsAtributos.EstadoReportePendiente)
                {
                    //return db.CC_EVALUACION_LOMO_MIGA_BANDEJA_CABECERA.Where(x => (x.EstadoRegistro == clsAtributos.EstadoRegistroActivo & x.EstadoControl == clsAtributos.EstadoReportePendiente)).ToList();
                    var respuesta = (from x in db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA
                                     join d in db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE on new { IdCabecera = x.IdAnalisisQuimicoProductoSe, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { IdCabecera = d.IdCabeceraAnalisisQuimicoProductoSe, d.EstadoRegistro }
                                     join s in db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO on new { IdDetalle = d.IdDetalleAnalisisQuimicoProductoSe, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { IdDetalle = s.IdDetalleAnalisisQuimicoProductoSe, s.EstadoRegistro }
                                     where x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && (x.EstadoControl == clsAtributos.EstadoReportePendiente || x.EstadoControl == null)
                                     select x).Distinct().ToList();

                    return respuesta;
                }
                else
                {
                    var respuesta = (from x in db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA
                                     join d in db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE on new { IdCabecera = x.IdAnalisisQuimicoProductoSe, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { IdCabecera = d.IdCabeceraAnalisisQuimicoProductoSe, d.EstadoRegistro }
                                     join s in db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO on new { IdDetalle = d.IdDetalleAnalisisQuimicoProductoSe, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { IdDetalle = s.IdDetalleAnalisisQuimicoProductoSe, s.EstadoRegistro }
                                     where x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && (x.Fecha >= FechaInicio && x.Fecha <= FechaFin) &&
                                     x.EstadoControl == clsAtributos.EstadoReporteActivo
                                     select x).Distinct().ToList();
                    return respuesta;
                }
            }

        }
        public List<SPReporteAnalisisQuimicoProductoSe> ConsultaReporte(int idCabecera)
        {
            using (var db = new ASIS_PRODEntities())
            {
                return db.SPReporteAnalisisQuimicoProductoSe(idCabecera).ToList();
            }
        }
        public string AprobarControl(int idCabecera, string usuario, string terminal, DateTime Fecha)
        {
            using (var db = new ASIS_PRODEntities())
            {

                var buscarCabecera = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA.Find(idCabecera);
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
        public string ReversarControl(int IdControl, string usuario, string terminal)
        {
            using (var db = new ASIS_PRODEntities())
            {

                var buscarControl = db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA.Find(IdControl);
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
        public List<CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA> ConsultarCabReportes(DateTime FechaDesde, DateTime FechaHasta)
        {
            using (var db = new ASIS_PRODEntities())
            {
                var respuesta = (from x in db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA
                                 join d in db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_DETALLE on new { Id = x.IdAnalisisQuimicoProductoSe, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { Id = d.IdCabeceraAnalisisQuimicoProductoSe, d.EstadoRegistro }
                                 join t in db.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_TIPO on new { Id = d.IdDetalleAnalisisQuimicoProductoSe, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { Id = t.IdDetalleAnalisisQuimicoProductoSe, t.EstadoRegistro }

                                 where x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && (x.Fecha >= FechaDesde && x.Fecha <= FechaHasta)
                                 select new
                                 {
                                     x.Fecha,
                                     x.AprobadoPor,
                                     x.EstadoControl,
                                     x.EstadoRegistro,
                                     x.FechaAprobacion,
                                     x.FechaIngresoLog,
                                     x.FechaModificacionLog,
                                     x.IdAnalisisQuimicoProductoSe,
                                     x.Observacion,
                                     x.TerminalIngresoLog,
                                     x.TerminalModificacionLog,
                                     x.UsuarioIngresoLog,
                                     x.UsuarioModificacionLog,
                                     x.Turno
                                 }).Distinct().ToList();

                List<CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA> b = (from x in respuesta
                                                                               select new CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_CABECERA {
                                                                                   Fecha = x.Fecha,
                                                                                   AprobadoPor = x.AprobadoPor,
                                                                                   EstadoControl = x.EstadoControl,
                                                                                   EstadoRegistro = x.EstadoRegistro,
                                                                                   FechaAprobacion = x.FechaAprobacion,
                                                                                   FechaIngresoLog = x.FechaIngresoLog,
                                                                                   FechaModificacionLog = x.FechaModificacionLog,
                                                                                   IdAnalisisQuimicoProductoSe = x.IdAnalisisQuimicoProductoSe,
                                                                                   Observacion = x.Observacion,
                                                                                   TerminalIngresoLog = x.TerminalIngresoLog,
                                                                                   TerminalModificacionLog = x.TerminalModificacionLog,
                                                                                   UsuarioIngresoLog = x.UsuarioIngresoLog,
                                                                                   UsuarioModificacionLog = x.UsuarioModificacionLog,
                                                                                   Turno=x.Turno
                                                                               }).ToList();
                                                                       
                return b;
            }
        }
    }
}