using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Empleado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos
{
    public class clsDEmpleado
    {
        public List<spConsultaEmpleadoCargoPorLinea> ConsultaEmpleadoCargoLinea(string Linea)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaEmpleadoCargoPorLinea(Linea).ToList();
            }
        }

        public List<spConsultaPersonalNominaPorLinea> ConsultaPersonalNominaPorLinea()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaPersonalNominaPorLinea().ToList();
            }
        }
        public List<spConsultaDistribucionPorLinea> spConsultaDistribucionPorLinea(string Linea, DateTime Fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaDistribucionPorLinea(Fecha, Linea).ToList();
            }
        }
        public List<spConsultaPresentesPorAreaLinea_Result> spConsultaPresentesPorAreaLinea( DateTime Fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaPresentesPorAreaLinea(Fecha).ToList();
            }
        }
        public void GuardarModificarEmpleadoTurno(EmpleadoViewModel model)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var EmpleadoTurno = db.EMPLEADO_TURNO.FirstOrDefault(X=> X.Cedula == model.Cedula);
                if(EmpleadoTurno!= null)
                {
                    EmpleadoTurno.Turno = model.Turno;
                    EmpleadoTurno.CodLinea = model.CodLinea;
                    EmpleadoTurno.FechaModificacionLog = DateTime.Now;
                    EmpleadoTurno.EstadoRegistro = model.EstadoRegistro;
                    EmpleadoTurno.UsuarioModificacionLog = model.UsuarioIngreso;
                    EmpleadoTurno.TerminalModificacionLog = model.TerminalIngreso;

                    db.BITACORA_EMPLEADO_TURNO.Add(new BITACORA_EMPLEADO_TURNO
                    {
                        Cedula = model.Cedula,
                        CodLinea = model.CodLinea,
                        EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                        FechaIngresoLog = DateTime.Now,
                        Observacion = "Asigna Turno",
                        TerminalIngresoLog = model.TerminalIngreso,
                        Turno = model.Turno,
                        UsuarioIngresoLog = model.UsuarioIngreso
                    });
                }
                else
                {
                    db.EMPLEADO_TURNO.Add(new EMPLEADO_TURNO
                    {
                        Cedula= model.Cedula,
                        Turno= model.Turno,
                        CodLinea = model.CodLinea,
                        EstadoRegistro =model.EstadoRegistro,
                        FechaIngresoLog = model.FechaIngreso,
                        UsuarioIngresoLog = model.UsuarioIngreso,
                        TerminalIngresoLog = model.TerminalIngreso
                    });
                    db.BITACORA_EMPLEADO_TURNO.Add(new BITACORA_EMPLEADO_TURNO
                    {
                        Cedula = model.Cedula,
                        CodLinea = model.CodLinea,
                        EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                        FechaIngresoLog = DateTime.Now,
                        Observacion = "Asigna Turno",
                        TerminalIngresoLog = model.TerminalIngreso,
                        Turno = model.Turno,
                        UsuarioIngresoLog = model.UsuarioIngreso
                    });
                }
                db.SaveChanges();

            }
        }

        public List<EmpleadoViewModel> ConsultaEmpleadoTurno(string Linea, DateTime Fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<EmpleadoViewModel> ListaEmpleadoTurno = new List<EmpleadoViewModel>();

                var poEmpleadoTurno = db.spConsutaEmpleadosTurnos(Linea, Fecha).ToList();

                foreach(var x in poEmpleadoTurno)
                {
                    ListaEmpleadoTurno.Add(new EmpleadoViewModel {
                        Cedula = x.Cedula,
                        Nombre = x.Nombre,
                        CodLinea=x.CodLinea,
                        Linea = x.Linea,
                         Turno = x.TURNO,
                         Prestado = x.Prestado??false
                    });
                }
                return ListaEmpleadoTurno;
            }
        }

        public List<spConsutaReporteEmpleadosTurnos> ConsultaReporteEmpleadoTurno(string Linea, string Turno)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {              
                List<spConsutaReporteEmpleadosTurnos> poEmpleadoTurno = db.spConsutaReporteEmpleadosTurnos(Linea,Turno).ToList();   
                
                return poEmpleadoTurno;
            }
        }

        public void ActualizarEmpleadoTurno(EMPLEADO_TURNO model)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var poEmpleadoTurno = db.EMPLEADO_TURNO.FirstOrDefault(x=> x.Cedula == model.Cedula);
                if (poEmpleadoTurno != null)
                {
                    poEmpleadoTurno.CodLinea = model.CodLinea;
                    poEmpleadoTurno.Turno = "1";
                    poEmpleadoTurno.FechaModificacionLog = DateTime.Now;
                    poEmpleadoTurno.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    poEmpleadoTurno.TerminalModificacionLog = model.TerminalModificacionLog;

                    db.BITACORA_EMPLEADO_TURNO.Add(new BITACORA_EMPLEADO_TURNO
                    {
                        Cedula = model.Cedula,
                        CodLinea = model.CodLinea,
                        EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                        FechaIngresoLog = DateTime.Now,
                        Observacion = "Cambio de Area",
                        TerminalIngresoLog = model.TerminalIngresoLog,
                        Turno = "1",
                        UsuarioIngresoLog = model.UsuarioIngresoLog
                    });


                    db.SaveChanges();
                }
            }
        }

        public List<spConsultaEmpleadosPersonalPrestadoFiltro> ConsultaEmpleadosCambioPersonalFiltro(DateTime Fecha, TimeSpan Hora, string dsLinea, string dsArea, string dsCargo)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                List<spConsultaEmpleadosPersonalPrestadoFiltro> pListEmpleados = null;
                if (string.IsNullOrEmpty(dsLinea))
                    dsLinea = "0";
                if (string.IsNullOrEmpty(dsArea))
                    dsArea = "0";
                if (string.IsNullOrEmpty(dsCargo))
                    dsCargo = "0";

                pListEmpleados = db.spConsultaEmpleadosPersonalPrestadoFiltro(Fecha,Hora, dsLinea).ToList();
                return pListEmpleados;
            }
        }


        public List<spConsutaEmpleadosFiltro> ConsultaEmpleadosFiltro(string dsLinea, string dsArea, string dsCargo)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                List<spConsutaEmpleadosFiltro> pListEmpleados = null;
                if (string.IsNullOrEmpty(dsLinea))
                    dsLinea = "0";
                if (string.IsNullOrEmpty(dsArea))
                    dsArea = "0";
                if (string.IsNullOrEmpty(dsCargo))
                    dsCargo = "0";

                pListEmpleados = db.spConsutaEmpleadosFiltro(dsArea, dsLinea, dsCargo).ToList();
                return pListEmpleados;
            }
        }
        public spConsultaEspecificaEmpleadosxCedula ConsultarEmpleadoxCedula(string Cedula)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                spConsultaEspecificaEmpleadosxCedula BuscarEmpleaado = db.spConsultaEspecificaEmpleadosxCedula(Cedula).FirstOrDefault();
                return BuscarEmpleaado;
            }
        }
        public List<spConsultarCaambioPersonalxCedula> ConsultarDondeFueMovido(List<spConsutaEmpleadosFiltroCambioPersonal> ListEmpleados)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                //List<spConsutaEmpleadosFiltroCambioPersonal> Empleados = new List<spConsutaEmpleadosFiltroCambioPersonal>();
                List<spConsultarCaambioPersonalxCedula> Empleados = new List<spConsultarCaambioPersonalxCedula>();

                spConsultarCaambioPersonalxCedula consultarempleado = null;
                foreach (var item in ListEmpleados)
                {
                    consultarempleado = db.spConsultarCaambioPersonalxCedula(item.CEDULA).FirstOrDefault();
                    //Empleados.Add(new spConsutaEmpleadosFiltroCambioPersonal { CEDULA = consultarempleado.CEDULA, LINEA = consultarempleado.LINEA });
                    Empleados.Add(new spConsultarCaambioPersonalxCedula { CEDULA = consultarempleado.CEDULA, LINEA = consultarempleado.LINEA, FECHAMOVIDO = consultarempleado.FECHAMOVIDO });

                }

                return Empleados;
            }
        }
        public List<spConsutaEmpleadosFiltroCambioPersonal> ConsultaEmpleadosFiltroCambioPersonal(string dsLinea, string dsArea, string dsCargo,string psRecurso,string psTipo)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (string.IsNullOrEmpty(dsLinea))
                    dsLinea = "0";
                if (string.IsNullOrEmpty(dsArea))
                    dsArea = "0";
                if (string.IsNullOrEmpty(dsCargo))
                    dsCargo = "0";
                if (string.IsNullOrEmpty(psRecurso))
                    psRecurso = "0";
                

                List<spConsutaEmpleadosFiltroCambioPersonal> pListEmpleados = null;
                //List<spConsutaEmpleadosFiltroCambioPersonal> pListEmpleadoR = new List<spConsutaEmpleadosFiltroCambioPersonal>();
                pListEmpleados = db.spConsutaEmpleadosFiltroCambioPersonal(dsArea, dsLinea, dsCargo,psRecurso,psTipo).ToList();
               
                return pListEmpleados;
                
            }
            
        }
        public List<spConsutaEmpleados> ConsultaEmpleado(string dsCedula)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())            {

                List<spConsutaEmpleados> pListEmpleados = null;
                if (!string.IsNullOrEmpty(dsCedula))
                    pListEmpleados = db.spConsutaEmpleados(dsCedula).ToList();
                return pListEmpleados;
            }
        }
        public List<spConsutaEmpleadosFiltro> RetornaPersonalSinCuchilloAsignado(List<spConsutaEmpleadosFiltro> ListPersonas)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                //foreach (var item in ListPersonas.ToArray())
                //{
                //    if (db.EMPLEADO_CUCHILLO.Where(x => x.Cedula == item.CEDULA).ToList().Count == 0)
                //    {
                //        ListPersonas.Remove(ListPersonas.Where(z=>z.CEDULA==item.CEDULA).FirstOrDefault());
                //    }
                //}
                var EmpleadoxCuchillo = db.EMPLEADO_CUCHILLO.ToList();
                var EmpleadosSinCuchillo = (from e in ListPersonas
                                            join ec in EmpleadoxCuchillo 
                                                    on new { Cedula=e.CEDULA, EstadoRegistro=clsAtributos.EstadoRegistroActivo } equals new { ec.Cedula, ec.EstadoRegistro } into EmpCuchi
                                            from ec in EmpCuchi.DefaultIfEmpty()
                                            where   ec == null 
                                            select new spConsutaEmpleadosFiltro
                                            {
                                                CEDULA = e.CEDULA,
                                                NOMBRES = e.NOMBRES
                                            }).ToList();
           
                return EmpleadosSinCuchillo;
            }
        }

        public TimeSpan? ConsultaFechaInicioJornada(string Linea, DateTime Fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var result = db.CONTROL_ASISTENCIA.FirstOrDefault(x => x.Fecha == Fecha && x.Linea == Linea);
                if (result != null)
                { return result.Hora; }
                else
                {
                    return null;
                }
            }
        }
    }
}