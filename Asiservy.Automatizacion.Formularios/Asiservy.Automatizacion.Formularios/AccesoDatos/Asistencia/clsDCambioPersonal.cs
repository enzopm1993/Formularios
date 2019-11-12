using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Asistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                    var Linea = entities.spConsultaLinea(x.CodLinea).FirstOrDefault();
                    var Area = entities.spConsultaArea(x.CodArea).FirstOrDefault();
                    var Empleado = entities.spConsutaEmpleados(x.Cedula).FirstOrDefault();
                    //var Cargo = entities.spConsultaCargos(x.Cedula).FirstOrDefault();

                    ListadoCambioPersonal.Add(new BitacoraCambioPersonalModelView
                    {
                        Cedula = x.Cedula,
                        Nombre = Empleado != null ? Empleado.NOMBRES : "",
                        //CodCargo = x.CodCargo,
                        //Cargo = Cargo.Descripcion,
                        Area = Area!=null? Area.Descripcion:"",
                        CodArea = x.CodArea,
                        CodLinea = x.CodLinea,
                        Linea = Linea != null ? Linea.Descripcion:"",
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
            List<String> listCedulas = new List<String>();
            //CAMBIO_PERSONAL CambioPersonal;
            string psLinea = pListCambioPersonal.FirstOrDefault().CodLinea;
            string psArea = pListCambioPersonal.FirstOrDefault().CentroCosto;
            string psusuario = pListCambioPersonal.FirstOrDefault().UsuarioIngresoLog;
            string psterminal = pListCambioPersonal.FirstOrDefault().TerminalIngresoLog;
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                int ContadorEmpleados = pListCambioPersonal.Count;
                if (tipo == "prestar")
                {
                    foreach (var item in pListCambioPersonal.ToArray())
                    {
                        if (db.CAMBIO_PERSONAL.Any(x => x.Cedula == item.Cedula&&x.EstadoRegistro==clsAtributos.EstadoRegistroActivo))
                        {
                            pListCambioPersonal.Remove(item);
                            Bitacora.Remove(Bitacora.Where(x=>x.Cedula==item.Cedula).FirstOrDefault());
                            ContadorEmpleados--;
                        }
                        else if(db.CAMBIO_PERSONAL.Any(x => x.Cedula == item.Cedula && x.EstadoRegistro == clsAtributos.EstadoRegistroInactivo))
                        {
                            listCedulas.Add(item.Cedula);
                            pListCambioPersonal.Remove(item);
                            //CambioPersonal = db.CAMBIO_PERSONAL.Where(x => x.Cedula == item.Cedula).FirstOrDefault();
                            //CambioPersonal.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                            //CambioPersonal.CodLinea = item.CodLinea;
                            //CambioPersonal.CodArea = item.CodArea;
                            //CambioPersonal.FechaModificacionLog = DateTime.Now;
                            //CambioPersonal.UsuarioModificacionLog = item.UsuarioIngresoLog;
                            //CambioPersonal.TerminalModificacionLog = item.TerminalIngresoLog;

                        }
                    }
                    if (ContadorEmpleados == 0)
                    {
                        return "Todos los empleados seleccionados ya habian sido movidos";
                    }
                    else
                    {
                        if (listCedulas.Count>0)
                        {
                            string[] CedulasArray = listCedulas.ToArray();
                            var ActualizarCambioPersonal = db.CAMBIO_PERSONAL.Where(p => CedulasArray.Contains(p.Cedula)).ToList();
                            ActualizarCambioPersonal.ForEach(p =>
                            {
                                p.EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                                p.CodLinea = psLinea;
                                p.CentroCosto = psArea;
                                p.FechaModificacionLog = DateTime.Now;
                                p.UsuarioModificacionLog = psusuario;
                                p.TerminalModificacionLog = psterminal;
                            });
                        }
                        if (pListCambioPersonal.Count>0)
                        {
                            db.CAMBIO_PERSONAL.AddRange(pListCambioPersonal);
                        }
                        db.BITACORA_CAMBIO_PERSONAL.AddRange(Bitacora);
                        db.SaveChanges();
                        return "Empleado(s) movido(s) con éxito";
                    }
                    
                }
                else if (tipo == "regresar")
                {
                    foreach (var item in pListCambioPersonal.ToArray())
                    {
                        listCedulas.Add(item.Cedula);
                        //CambioPersonal = db.CAMBIO_PERSONAL.Where(x => x.Cedula == item.Cedula).FirstOrDefault();
                        //CambioPersonal.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                        //CambioPersonal.FechaModificacionLog = DateTime.Now;
                        //CambioPersonal.TerminalModificacionLog = item.TerminalIngresoLog;
                        //CambioPersonal.UsuarioModificacionLog = item.UsuarioIngresoLog;
                        //db.SaveChanges();
                    }
                    string[] CedulasArray = listCedulas.ToArray();
                    var ActualizarCambioPersonal = db.CAMBIO_PERSONAL.Where(p => CedulasArray.Contains(p.Cedula)).ToList();
                    ActualizarCambioPersonal.ForEach(p =>
                    {
                        p.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                        p.FechaModificacionLog = DateTime.Now;
                        p.UsuarioModificacionLog = psusuario;
                        p.TerminalModificacionLog = psterminal;
                    });
                    db.BITACORA_CAMBIO_PERSONAL.AddRange(Bitacora);
                    db.SaveChanges();
                    return "Empleado(s) regresado(s) con éxito";
                }
                return "";
            }
            
        }

        public sp_ConsultaEmpleadosMovidos ConsultarCambioPersonal(string cedula)
        {
            sp_ConsultaEmpleadosMovidos poCambioPersonal = null;
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                poCambioPersonal = db.sp_ConsultaEmpleadosMovidos(cedula).FirstOrDefault();
                return poCambioPersonal;
            }
        }
        public List<spConsultarCambioPersonalxLineaxTurno> ConsultarCambioPersonalxLinea(string CodLinea,string Turno)
        {

            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                //return db.CAMBIO_PERSONAL.Where(x => x.CodLinea == CodLinea).ToList();
                return db.spConsultarCambioPersonalxLineaxTurno(CodLinea,Turno).ToList();
            }
        }

        public List<spReporteCambioPersonal> ReporteCambioPersonal(string CodLinea, DateTime? FechaInicio, DateTime? FechaFin)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spReporteCambioPersonal(CodLinea,FechaInicio,FechaFin).ToList();
            }
        }
    }
}