using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Empleado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Empleado
{
    public class clsDEmpleadoEsfero
    {

        public string GuardarModificarControlEsfero(CONTROL_ESFERO model, string tipo)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var FechaActual = DateTime.Now.Date;
                var control = db.CONTROL_ESFERO.FirstOrDefault(x=> x.Cedula == model.Cedula && x.Fecha == FechaActual);
                if(control != null)
                {
                    if (tipo == "1")
                        control.HoraInicio = model.HoraInicio;
                    else
                        control.HoraFin = model.HoraInicio;
                    control.FechaModificacionLog = DateTime.Now;
                    control.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    control.TerminalModificacionLog = model.TerminalIngresoLog;
                    db.SaveChanges();
                }

                return clsAtributos.MsjRegistroGuardado;
            }
        }

        public List<spConsutaControlEsferos> ConsultaControlEsfero(string dsLinea, string tipo)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var FechaActual = DateTime.Now.Date;
                var ListadoControl= db.spConsutaControlEsferos(dsLinea, FechaActual,tipo).ToList();
                return ListadoControl;
            }
        }

        public void GenerarControlEmpleadoEsfero(string dsLinea, string dsUsuario, string dsTerminal)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var FechaActual = DateTime.Now.Date;
                var Control = db.CONTROL_ESFERO.FirstOrDefault(x => x.Fecha == FechaActual);
                if(Control == null)
                {
                    var EmpleadoEsfero = this.ConsultaEmpleadoEsfero(dsLinea).Where(x=> x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    foreach(var x in EmpleadoEsfero)
                    {
                        db.CONTROL_ESFERO.Add(new CONTROL_ESFERO {
                            Cedula = x.Cedula,
                            Fecha = DateTime.Now,
                            EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                            FechaIngresoLog = DateTime.Now,
                            Linea = dsLinea,
                            TerminalIngresoLog= dsTerminal,
                            UsuarioIngresoLog = dsUsuario
                        });
                    }
                    db.SaveChanges();
                }

            }
        }


        public List<EmpleadoEsferoViewModel> ConsultaEmpleadoEsfero(string linea)
        {

            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<EmpleadoEsferoViewModel> ListadoEmpleadoEsfero = new List<EmpleadoEsferoViewModel>();
                IEnumerable<spConsutaEmpleadoEsfero> poEmpleadoEsfero = db.spConsutaEmpleadoEsfero(linea);
                foreach (var x in poEmpleadoEsfero.ToList())
                {
                    ListadoEmpleadoEsfero.Add(new EmpleadoEsferoViewModel
                    {
                        Cedula = x.Cedula,
                        EstadoRegistro = x.EstadoRegistro,
                        FechaIngresoLog = x.FechaIngresoLog,
                        Nombre = x.Nombre,
                        NumeroEsfero = x.NumeroEsfero,
                        TerminalIngresoLog = x.TerminalIngresoLog,
                        UsuarioIngresoLog = x.UsuarioIngresoLog
                    });
                }
                return ListadoEmpleadoEsfero;
            }

        }


            public string GuardarMoficicarEsfero(EmpleadoEsferoViewModel model)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var validaEsfero = db.EMPLEADO_ESFERO.FirstOrDefault(x => x.NumeroEsfero==model.NumeroEsfero
                && x.Cedula != model.Cedula && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                if (validaEsfero != null) return "Esfero ya esta asignado a otro empleado: "+validaEsfero.Cedula;

                var EmpleadoEsfero = db.EMPLEADO_ESFERO.FirstOrDefault(x=> x.Cedula== model.Cedula);
                if (EmpleadoEsfero != null)
                {
                    EmpleadoEsfero.NumeroEsfero = model.NumeroEsfero;
                    EmpleadoEsfero.EstadoRegistro = model.EstadoRegistro;
                    EmpleadoEsfero.FechaModificacionLog = DateTime.Now;
                    EmpleadoEsfero.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    EmpleadoEsfero.TerminalModificacionLog = model.TerminalIngresoLog;
                }
                else
                {
                    db.EMPLEADO_ESFERO.Add(new EMPLEADO_ESFERO {
                        Cedula= model.Cedula,
                        NumeroEsfero = model.NumeroEsfero,
                        EstadoRegistro = model.EstadoRegistro,
                        FechaIngresoLog = DateTime.Now,
                        TerminalIngresoLog = model.TerminalIngresoLog,
                        UsuarioIngresoLog = model.UsuarioIngresoLog

                    });
                }
                db.SaveChanges();
                return clsAtributos.MsjRegistroGuardado;
            }

        }

        public List<EmpleadoEsferoViewModel> ConsultaEmpleadosFiltro(string dsLinea)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                List<EmpleadoEsferoViewModel> pListEmpleados = null;
                //if (string.IsNullOrEmpty(dsLinea))
                //    dsLinea = "0";
                //if (string.IsNullOrEmpty(dsArea))
                //    dsArea = "0";
                //if (string.IsNullOrEmpty(dsCargo))
                //    dsCargo = "0";

                //pListEmpleados = db.EMPLEADO_ESFERO.Where();
                return pListEmpleados;
            }
        }

    }
}