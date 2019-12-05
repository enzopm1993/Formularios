using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Asistencia;
using Asiservy.Automatizacion.Formularios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia
{
    public class clsDCambioPersonal
    {
        public static bool Between(DateTime entrada, DateTime fecha1, DateTime fecha2)
        {
            return (entrada > fecha1 && entrada < fecha2);
        }
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

       
        public string GuardarCambioDePersonal(List<CAMBIO_PERSONAL> pListCambioPersonal/*,List<BITACORA_CAMBIO_PERSONAL> Bitacora*/, string tipo)
        {
            List<String> listCedulas = new List<String>();
            DateTime? psfecha = pListCambioPersonal.FirstOrDefault().FechaInicio;
            TimeSpan? psHora = pListCambioPersonal.FirstOrDefault().HoraInicio;
            string psusuario = pListCambioPersonal.FirstOrDefault().UsuarioIngresoLog;
            string psterminal = pListCambioPersonal.FirstOrDefault().TerminalIngresoLog;
            List<RespuestaGeneral> RespuestaGeneral=new List<RespuestaGeneral>();
            DateTime FechaActual=DateTime.Now;
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
               
                if (tipo == "prestar")
                {
                    foreach (var item in pListCambioPersonal.ToArray())
                    {
                        if (item.FechaInicio.Value.Add(item.HoraInicio.Value) < FechaActual)
                        {//entrada > fecha1 && entrada < fecha2
                            //var ValidacionMoverAFechasAnteriores = (from c in db.CAMBIO_PERSONAL
                            //         where /*Between(item.FechaInicio.Value.Add(item.HoraInicio.Value), c.FechaInicio.Value.Add(c.HoraInicio.Value), c.FechaFin.Value.Add(c.HoraFin.Value))*/
                            //         (((item.FechaInicio.Value>= c.FechaInicio.Value) &&(item.FechaInicio.Value<= c.FechaFin.Value))&&((item.HoraInicio.Value>=c.HoraInicio.Value)&&(item.HoraInicio.Value<=c.HoraFin.Value))) &&c.Vigente==false
                            //         select c).ToList();
                            var ValidacionMoverAFechasAnteriores = db.spConsultaCambioPersonalFecha(item.FechaInicio, item.HoraInicio).ToList();
                            if (ValidacionMoverAFechasAnteriores.Count > 0)
                            {
                                RespuestaGeneral.Add(new Models.RespuestaGeneral {Codigo=1,Mensaje="no se pudo mover: "+item.Cedula+ " ,por que ya habia sido movido en la fecha indicada",Respuesta=false });
                                pListCambioPersonal.Remove(item);
                            }
                            else
                            {
                                RespuestaGeneral.Add(new Models.RespuestaGeneral { Codigo = 0, Mensaje = "Empleado: " + item.Cedula + " ,movido con éxito", Respuesta=true });
                            }
                        }
                    }
                    bool verificaSiTodosSeMovieron=true;
                    string RespuestaFinal="";
                    foreach (var item in RespuestaGeneral)
                    {
                        RespuestaFinal =  RespuestaFinal+item.Mensaje+"\n";
                        verificaSiTodosSeMovieron = verificaSiTodosSeMovieron && item.Respuesta;
                    }
                    db.CAMBIO_PERSONAL.AddRange(pListCambioPersonal);
                    db.SaveChanges();
                    if (verificaSiTodosSeMovieron)
                    {
                        return "Empleado(s) movido(s) con éxito";
                    }
                    else
                    {

                        return RespuestaFinal;
                    }
                    
                }
                else if (tipo == "regresar")
                {
                    foreach (var item in pListCambioPersonal.ToArray())
                    {
                        listCedulas.Add(item.Cedula); 
                    }
                    string[] CedulasArray = listCedulas.ToArray();
                    var ActualizarCambioPersonal = db.CAMBIO_PERSONAL.Where(p => CedulasArray.Contains(p.Cedula)&&p.Vigente==true).ToList();
                    //ActualizarCambioPersonal.ForEach(p =>
                    //{
                    //    p.FechaFin = psfecha;
                    //    p.HoraFin = psHora;
                    //    p.Vigente = false;
                    //    p.FechaModificacionLog = DateTime.Now;
                    //    p.UsuarioModificacionLog = psusuario;
                    //    p.TerminalModificacionLog = psterminal;
                    //});

                    //***
                    foreach (var item in ActualizarCambioPersonal)
                    {
                        //if (Between(psfecha.Value.Add(psHora.Value), item.FechaInicio.Value.Add(item.HoraInicio.Value),))
                        if(psfecha.Value.Add(psHora.Value)> item.FechaInicio.Value.Add(item.HoraInicio.Value))
                        {
                            item.FechaFin = psfecha;
                            item.HoraFin = psHora;
                            item.Vigente = false;
                            item.FechaModificacionLog = DateTime.Now;
                            item.UsuarioModificacionLog = psusuario;
                            item.TerminalModificacionLog = psterminal;
                        }
                        else
                        {
                            item.FechaFin = item.FechaInicio;
                            item.HoraFin = item.HoraInicio;
                            item.Vigente = false;
                            item.UsuarioModificacionLog = psusuario;
                            item.TerminalModificacionLog = psterminal;
                        }
                        
                    }
                    //**
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