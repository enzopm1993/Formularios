using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Asistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia
{
    public class clsDCambioPersonal
    {
        public List<BitacoraCambioPersonalModelView>  ConsultarBitacoraCambioPersonal (BitacoraCambioPersonalModelView filtros)
        {
            using(ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<BitacoraCambioPersonalModelView> ListadoCambioPersonal = new List<BitacoraCambioPersonalModelView>();

                IEnumerable<BITACORA_CAMBIO_PERSONAL> Listado = entities.BITACORA_CAMBIO_PERSONAL;
                if (!string.IsNullOrEmpty(filtros.CodLinea))
                {
                    Listado = Listado.Where(x=> x.CodLinea== filtros.CodLinea);
                }
                if (!string.IsNullOrEmpty(filtros.CodArea))
                {
                    Listado = Listado.Where(x => x.CodArea == filtros.CodArea);
                }
                if (!string.IsNullOrEmpty(filtros.Cedula))
                {
                    Listado = Listado.Where(x => x.Cedula == filtros.Cedula);
                }

                var fechaHasta = filtros.FechaHasta.Date.AddDays(1);
                 Listado = Listado.Where(x => x.FechaIngresoLog.Value.Date >= filtros.FechaDesde.Date);
                 Listado = Listado.Where(x => x.FechaIngresoLog.Value.Date <= fechaHasta.Date);


                foreach(var x in Listado.ToList())
                {
                    var Linea = entities.spConsultaLinea(x.Cedula).FirstOrDefault();
                    var Area = entities.spConsultaArea(x.Cedula).FirstOrDefault();
                    var Cargo = entities.spConsultaCargos(x.Cedula).FirstOrDefault();

                    ListadoCambioPersonal.Add(new BitacoraCambioPersonalModelView
                    {
                        Cedula = x.Cedula,
                        CodCargo = x.CodCargo,
                        Cargo = Cargo.Descripcion,
                        Area= Area.Descripcion,
                        CodArea = x.CodArea,
                        CodLinea = x.CodLinea,
                        Linea= Linea.Descripcion,
                        FechaIngresoLog = x.FechaIngresoLog,
                        Tipo = x.Tipo,
                        IdBitacoraCambioPersonal = x.IdBitacoraCambioPersonal,
                        TerminalIngresoLog = x.TerminalIngresoLog,
                        UsuarioIngresoLog = x.UsuarioIngresoLog
                    });
                }

                //ListadoCambioPersonal = Listado.Select(x => new BitacoraCambioPersonalModelView
                //{
                //    Cedula=x.Cedula,
                //    CodCargo=x.CodCargo,
                //    CodArea=x.CodArea,
                //    CodLinea=x.CodLinea,
                //    FechaIngresoLog =x.FechaIngresoLog,
                //    Tipo=x.Tipo,
                //    IdBitacoraCambioPersonal = x.IdBitacoraCambioPersonal,
                //    TerminalIngresoLog=x.TerminalIngresoLog,
                //    UsuarioIngresoLog=x.UsuarioIngresoLog
                //}).ToList();

                return ListadoCambioPersonal;
            }

        }

       
        public string GuardarCambioDePersonal(List<CAMBIO_PERSONAL> pListCambioPersonal,List<BITACORA_CAMBIO_PERSONAL> Bitacora, string tipo)
        {
            CAMBIO_PERSONAL CambioPersonal;
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                int ContadorEmpleados = pListCambioPersonal.Count;
                if (tipo == "prestar")
                {
                    foreach (var item in pListCambioPersonal.ToArray())
                    {
                        if (db.CAMBIO_PERSONAL.Any(x => x.Cedula == item.Cedula))
                        {
                            pListCambioPersonal.Remove(item);
                            Bitacora.Remove(Bitacora.Where(x=>x.Cedula==item.Cedula).FirstOrDefault());
                        }
                    }
                    if (ContadorEmpleados > 0 && pListCambioPersonal.Count == 0)
                    {
                        return "Todos los empleados seleccionados ya habian sido movidos";
                    }
                    else
                    {
                        db.CAMBIO_PERSONAL.AddRange(pListCambioPersonal);
                        db.BITACORA_CAMBIO_PERSONAL.AddRange(Bitacora);
                        db.SaveChanges();
                        return "Empleado(s) movido(s) con éxito";
                    }
                    
                }
                else if (tipo == "regresar")
                {
                    foreach (var item in pListCambioPersonal.ToArray())
                    {
                        CambioPersonal = db.CAMBIO_PERSONAL.Where(x => x.Cedula == item.Cedula).FirstOrDefault();
                        CambioPersonal.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                        CambioPersonal.FechaModificacionLog = DateTime.Now;
                        CambioPersonal.TerminalModificacionLog = item.TerminalIngresoLog;
                        CambioPersonal.UsuarioModificacionLog = item.UsuarioIngresoLog;
                        db.SaveChanges();
                    }
                    db.BITACORA_CAMBIO_PERSONAL.AddRange(Bitacora);
                    db.SaveChanges();
                    return "Empleado(s) regresado(s) con éxito";
                }
                return "";
            }
            
        }
    }
}