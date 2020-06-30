using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Produccion.EntradaYSalidaDeMateriales;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Asiservy.Automatizacion.Formularios.AccesoDatos.PRODUCCION.EntradaySalidaDeMaterialesEnAreasDeProceso
{
    public class ClsDEntradaSalidaMateriales
    {
        #region Mantenimiento
        public List<ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL> ConsultaMaterial()
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL.ToList();
            }
        }
        public void ModificarMaterial(ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var material = entities.ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL.FirstOrDefault(x => x.IdMaterial == model.IdMaterial);
                if (material != null)
                {
                    material.Nombre = model.Nombre;
                    material.Descripcion = model.Descripcion;
                    material.EstadoRegistro = model.EstadoRegistro;
                    material.FechaModificacionLog = DateTime.Now;
                    material.TerminalModificacionLog = model.TerminalIngresoLog;
                    material.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    entities.SaveChanges();
                }

            }
        }
        public void GuardarMaterial(ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                entities.ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL.Add(model);
                entities.SaveChanges();
            }
        }
        #endregion
        #region Control
        public ENTRADA_SALIDA_MATERIAL_CABECERA ConsultarCabeceraControl(DateTime Fecha, string Turno)
        {
            using (var db = new ASIS_PRODEntities())
            {
                return db.ENTRADA_SALIDA_MATERIAL_CABECERA.Where(x => x.Fecha == Fecha && x.Turno == Turno && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
            }
        }
        public object[] GuardarCabeceraControl(ENTRADA_SALIDA_MATERIAL_CABECERA poCabeceraControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarCabecera = db.ENTRADA_SALIDA_MATERIAL_CABECERA.Where(x => x.Fecha == poCabeceraControl.Fecha &&
                x.Turno == poCabeceraControl.Turno && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();
                if (buscarCabecera == null)
                {
                    db.ENTRADA_SALIDA_MATERIAL_CABECERA.Add(poCabeceraControl);
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
        public object[] ActualizarCabeceraControl(ENTRADA_SALIDA_MATERIAL_CABECERA poCabControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabecera = db.ENTRADA_SALIDA_MATERIAL_CABECERA.Find(poCabControl.IdControlEntradaSalidaMateriales);
                if (BuscarCabecera.EstadoControl)
                {
                    resultado[0] = "003";
                    resultado[1] = "El control ya se encuentra aprobado, no puede ser modificado";
                    resultado[2] = new
                    {
                        BuscarCabecera.IdControlEntradaSalidaMateriales,
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
        public object[] InactivarCabeceraControl(ENTRADA_SALIDA_MATERIAL_CABECERA poCabControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabeceraControl = db.ENTRADA_SALIDA_MATERIAL_CABECERA.Find(poCabControl.IdControlEntradaSalidaMateriales);
                if (BuscarCabeceraControl.EstadoControl)
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
        public object[] GuardarDetalleControl(ENTRADA_SALIDA_MATERIAL_DETALLE poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarabecera = db.ENTRADA_SALIDA_MATERIAL_CABECERA.Find(poDetalleControl.IdCabeceraEntradaSalidaMaterial);
                if (buscarabecera.EstadoControl)
                {
                    resultado[0] = "003";
                    resultado[1] = "El control se encuetra aprobado, no puede ser modificado";
                    resultado[2] = poDetalleControl;
                }
                else
                {
                    var buscarDetalle = db.ENTRADA_SALIDA_MATERIAL_DETALLE.Where(x => x.Material == poDetalleControl.Material&& x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).FirstOrDefault();

                    if (buscarDetalle == null)
                    {
                        db.ENTRADA_SALIDA_MATERIAL_DETALLE.Add(poDetalleControl);
                        db.SaveChanges();
                        resultado[0] = "000";
                        resultado[1] = "Registro ingresado con éxito";
                        resultado[2] = new
                        {
                            poDetalleControl.EstadoRegistro,
                            poDetalleControl.FechaIngresoLog,
                            poDetalleControl.FechaModificacionLog,
                            poDetalleControl.IdCabeceraEntradaSalidaMaterial,
                            poDetalleControl.IdDetalleEntradaSalidaMateriales,
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
        public object[] ActualizarDetalleControl(ENTRADA_SALIDA_MATERIAL_DETALLE poDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];

                var buscarabecera = db.ENTRADA_SALIDA_MATERIAL_CABECERA.Find(poDetalleControl.IdCabeceraEntradaSalidaMaterial);
                if (buscarabecera.EstadoControl)
                {
                    resultado[0] = "003";
                    resultado[1] = "El control se encuetra aprobado, no puede ser modificado";
                    resultado[2] = poDetalleControl;
                }
                else
                {
                    var buscardetalle = db.ENTRADA_SALIDA_MATERIAL_DETALLE.Find(poDetalleControl.IdDetalleEntradaSalidaMateriales);
                    buscardetalle.FechaModificacionLog = poDetalleControl.FechaIngresoLog;
                    buscardetalle.UsuarioModificacionLog = poDetalleControl.UsuarioIngresoLog;
                    buscardetalle.TerminalModificacionLog = poDetalleControl.TerminalIngresoLog;
                    buscardetalle.Ingreso = poDetalleControl.Ingreso;
                    db.SaveChanges();
                    resultado[0] = "001";
                    resultado[1] = "Registro actualizado con éxito";
                    resultado[2] = poDetalleControl;
                }
                return resultado;
            }
        }
        public List<EntradaSalidaMaterialDetalleViewModel> ConsultarDetalleControl(int idCabeceraControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                var resultado = (from d in db.ENTRADA_SALIDA_MATERIAL_DETALLE
                                 join cab in db.ENTRADA_SALIDA_MATERIAL_CABECERA on new { IdControlEntradaSalidaMateriales = d.IdCabeceraEntradaSalidaMaterial, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { cab.IdControlEntradaSalidaMateriales, cab.EstadoRegistro }
                                 join mat in db.ENTRADA_SALIDA_MATERIAL_MANT_MATERIAL on new {Id=d.IdDetalleEntradaSalidaMateriales, EstadoRegistro= clsAtributos.EstadoRegistroActivo }
                                 equals new { Id=mat.IdMaterial,mat.EstadoRegistro}
                                 where d.IdCabeceraEntradaSalidaMaterial == idCabeceraControl && d.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                 select new EntradaSalidaMaterialDetalleViewModel {IdCabeceraEntradaSalidaMaterial=d.IdCabeceraEntradaSalidaMaterial,
                                 IdDetalleEntradaSalidaMateriales=d.IdDetalleEntradaSalidaMateriales,
                                 Ingreso=d.Ingreso,
                                 Material=d.Material,
                                 MaterialNombre=mat.Nombre}).ToList();
                return resultado;
            }
        }
        public List<ENTRADA_SALIDA_MATERIAL_SUBDETALLE> ConsultarSubDetalleControl(int idDetalleControl)
        {
            using (var db = new ASIS_PRODEntities())
            {
                var resultado = (from d in db.ENTRADA_SALIDA_MATERIAL_SUBDETALLE
                                 join cab in db.ENTRADA_SALIDA_MATERIAL_DETALLE on new { IdDetalleEntradaSalidaMateriales = d.IdDetalleEntradaSalidaMaterial, EstadoRegistro = clsAtributos.EstadoRegistroActivo } equals new { cab.IdDetalleEntradaSalidaMateriales, cab.EstadoRegistro }

                                 where d.IdDetalleEntradaSalidaMaterial == idDetalleControl && d.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                                 select d).ToList();
                return resultado;
            }
        }
        public object[] GuardarSubDetalleControl(ENTRADA_SALIDA_MATERIAL_SUBDETALLE poSubDetalle, int IdCabecera)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var buscarabecera = db.ENTRADA_SALIDA_MATERIAL_CABECERA.Find(IdCabecera);
                if (buscarabecera.EstadoControl)
                {
                    resultado[0] = "003";
                    resultado[1] = "El control se encuetra aprobado, no puede ser modificado";
                    resultado[2] = poSubDetalle;
                }
                else
                {
                    var BuscarSubDetalle = db.ENTRADA_SALIDA_MATERIAL_SUBDETALLE.FirstOrDefault(x => x.IdDetalleEntradaSalidaMaterial == poSubDetalle.IdDetalleEntradaSalidaMaterial
                      && x.Hora == poSubDetalle.Hora && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo);
                    if (BuscarSubDetalle == null)
                    {
                        db.ENTRADA_SALIDA_MATERIAL_SUBDETALLE.Add(poSubDetalle);
                        db.SaveChanges();
                        resultado[0] = "000";
                        resultado[1] = "Registro ingresado con éxito";
                        resultado[2] = new
                        {
                            poSubDetalle.EstadoRegistro,
                            poSubDetalle.FechaIngresoLog,
                            poSubDetalle.FechaModificacionLog,
                            poSubDetalle.IdSubDetalleEntradaSalidaMaterial,
                            poSubDetalle.IdDetalleEntradaSalidaMaterial,
                            poSubDetalle.Hora,
                            poSubDetalle.Salida,
                            poSubDetalle.TerminalIngresoLog,
                            poSubDetalle.TerminalModificacionLog,
                            poSubDetalle.UsuarioIngresoLog,
                            poSubDetalle.UsuarioModificacionLog
                        };
                    }
                    else
                    {
                        resultado[0] = "003";
                        resultado[1] = "Error, ya existe un registro ingresado para la hora indicada";
                        resultado[2] = poSubDetalle;
                    }
                   

                }

                return resultado;
            }
        }
        public object[] ActualizarSubDetalleControl(ENTRADA_SALIDA_MATERIAL_SUBDETALLE poSubDetalleControl, int IdCabecera)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];

                var buscarabecera = db.ENTRADA_SALIDA_MATERIAL_CABECERA.Find(IdCabecera);
                if (buscarabecera.EstadoControl)
                {
                    resultado[0] = "003";
                    resultado[1] = "El control se encuetra aprobado, no puede ser modificado";
                    resultado[2] = poSubDetalleControl;
                }
                else
                {
                    var buscarSubDetalle = db.ENTRADA_SALIDA_MATERIAL_SUBDETALLE.Find(poSubDetalleControl.IdSubDetalleEntradaSalidaMaterial);
                    buscarSubDetalle.FechaModificacionLog = poSubDetalleControl.FechaIngresoLog;
                    buscarSubDetalle.UsuarioModificacionLog = poSubDetalleControl.UsuarioIngresoLog;
                    buscarSubDetalle.TerminalModificacionLog = poSubDetalleControl.TerminalIngresoLog;
                    buscarSubDetalle.Salida = poSubDetalleControl.Salida;
                    db.SaveChanges();
                    resultado[0] = "001";
                    resultado[1] = "Registro actualizado con éxito";
                    resultado[2] = poSubDetalleControl;
                }
                return resultado;
            }
        }
        public object[] InactivarSubDetalleControl(ENTRADA_SALIDA_MATERIAL_SUBDETALLE poSubDetalle, int IdCabecera)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabeceraControl = db.ENTRADA_SALIDA_MATERIAL_CABECERA.Find(IdCabecera);
                if (BuscarCabeceraControl.EstadoControl)
                {
                    resultado[0] = "003";
                    resultado[1] = "No es posible inactivar el control, por que se encuentra aprobado";
                    resultado[2] = poSubDetalle;
                }
                else
                {
                    var BuscarSubDetalle = db.ENTRADA_SALIDA_MATERIAL_SUBDETALLE.FirstOrDefault(x => x.IdSubDetalleEntradaSalidaMaterial == poSubDetalle.IdSubDetalleEntradaSalidaMaterial
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
        public object[] InactivarDetalleControl(ENTRADA_SALIDA_MATERIAL_DETALLE poDetalle, int IdCabecera)
        {
            using (var db = new ASIS_PRODEntities())
            {
                object[] resultado = new object[3];
                var BuscarCabeceraControl = db.ENTRADA_SALIDA_MATERIAL_CABECERA.Find(IdCabecera);
                if (BuscarCabeceraControl.EstadoControl)
                {
                    resultado[0] = "003";
                    resultado[1] = "No es posible inactivar el control, por que se encuentra aprobado";
                    resultado[2] = poDetalle;
                }
                else
                {
                    var BuscarDetalle = db.ENTRADA_SALIDA_MATERIAL_DETALLE.FirstOrDefault(x => x.IdDetalleEntradaSalidaMateriales == poDetalle.IdDetalleEntradaSalidaMateriales
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

        #endregion
    }
}