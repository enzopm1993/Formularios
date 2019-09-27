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
        public List<spConsultaPersonalNominaPorLinea> ConsultaPersonalNominaPorLinea()
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaPersonalNominaPorLinea().ToList();
            }
        }
        public List<spConsultaDistribucionPorLinea> spConsultaDistribucionPorLinea(string Linea)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaDistribucionPorLinea(DateTime.Now,Linea).ToList();
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

        public List<EmpleadoViewModel> ConsultaEmpleadoTurno(string Linea)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<EmpleadoViewModel> ListaEmpleadoTurno = new List<EmpleadoViewModel>();

                var poEmpleadoTurno = db.spConsutaEmpleadosTurnos(Linea).ToList();

                foreach(var x in poEmpleadoTurno)
                {
                    ListaEmpleadoTurno.Add(new EmpleadoViewModel {
                        Cedula = x.CEDULA,
                        Nombre = x.NOMBRES,
                        CodLinea=x.CODLINEA,
                        Linea = x.LINEA,
                         Turno = x.TURNO
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

        public List<spConsutaEmpleadosFiltro> ConsultaEmpleadosFiltroCambioPersonal(string dsLinea, string dsArea, string dsCargo,string psTipo)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (string.IsNullOrEmpty(dsLinea))
                    dsLinea = "0";
                if (string.IsNullOrEmpty(dsArea))
                    dsArea = "0";
                if (string.IsNullOrEmpty(dsCargo))
                    dsCargo = "0";

                List<spConsutaEmpleadosFiltro> pListEmpleados = null;
                List<spConsutaEmpleadosFiltro> pListEmpleadoR = new List<spConsutaEmpleadosFiltro>();
                pListEmpleados = db.spConsutaEmpleadosFiltro(dsArea, dsLinea, dsCargo).ToList();
                if (psTipo == clsAtributos.TipoPrestar)
                {
                    foreach (var item in pListEmpleados.ToArray())
                    {
                        if (db.CAMBIO_PERSONAL.Any(x => x.Cedula == item.CEDULA&&x.EstadoRegistro==clsAtributos.EstadoRegistroActivo))
                        {
                            pListEmpleados.Remove(item);
                        }
                    }
                    return pListEmpleados;
                }
                else
                {
                    foreach (var item in pListEmpleados)
                    {
                        if (db.CAMBIO_PERSONAL.Any(x => x.Cedula == item.CEDULA&&x.EstadoRegistro==clsAtributos.EstadoRegistroActivo))
                        {
                            pListEmpleadoR.Add(item);
                        }
                    }
                    return pListEmpleadoR;
                }
                
                
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
    }
}