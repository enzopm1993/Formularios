using Asiservy.Automatizacion.Datos.Datos;
using Asiservy.Automatizacion.Formularios.Models.Asistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia
{
    public class clsDCuchillo
    {
        clsDEmpleado clsDEmpleado = null;

        public List<EmpleadoCuchilloViewModel> ConsultarEmpleadosCuchilloPorLinea(string Linea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<EmpleadoCuchilloViewModel> ListadoEmpleadoCuchillo = new List<EmpleadoCuchilloViewModel>();
                var consulta = entities.spConsutaEmpleadosCuchillos(Linea).ToList();
                if(consulta != null)
                {
                    foreach(var x in consulta)
                    {
                        ListadoEmpleadoCuchillo.Add(new EmpleadoCuchilloViewModel
                        {
                           // Cedula = 
                        });
                    }
                }





                return ListadoEmpleadoCuchillo;
            }
        }
            

        public string GuardarModificarEmpleadoCuchillo(EmpleadoCuchilloViewModel model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var CuchilloAsignado = entities.EMPLEADO_CUCHILLO.FirstOrDefault(x=>
                x.NumeroCuchillo == model.NumeroCuchillo 
                && x.ColorCuchillo == model.ColorCuchillo
                &&x.Cedula != model.Cedula);
                if (CuchilloAsignado != null)
                    return "Cuchillo ya esta asignado a otro empleado";


                var EmpleadoCuchillo = entities.EMPLEADO_CUCHILLO.FirstOrDefault(x=> 
                x.IdEmpleadoCuchillo == model.IdEmpleadoCuchillo
                || (x.Cedula == model.Cedula && x.NumeroCuchillo == model.NumeroCuchillo && x.ColorCuchillo ==model.ColorCuchillo)
                );
                if (EmpleadoCuchillo != null)
                {
                    EmpleadoCuchillo.NumeroCuchillo = model.NumeroCuchillo;
                    EmpleadoCuchillo.ColorCuchillo = model.ColorCuchillo;
                    EmpleadoCuchillo.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    EmpleadoCuchillo.TerminalModificacionLog = model.TerminalIngresoLog;
                    EmpleadoCuchillo.FechaModificacionLog = model.FechaIngresoLog;
                    EmpleadoCuchillo.EstadoRegistro = model.EstadoRegistro;
                }
                else
                {
                    entities.EMPLEADO_CUCHILLO.Add(new EMPLEADO_CUCHILLO {
                        NumeroCuchillo = model.NumeroCuchillo,
                        ColorCuchillo = model.ColorCuchillo,
                        Cedula = model.Cedula,
                        FechaIngresoLog = model.FechaIngresoLog,
                        TerminalIngresoLog = model.TerminalIngresoLog,
                        UsuarioIngresoLog = model.UsuarioIngresoLog,
                        EstadoRegistro = model.EstadoRegistro

                    });
                }
                entities.SaveChanges();

                return clsAtributos.MsjRegistroGuardado;
            }
        }
        
            public List<EmpleadoCuchilloViewModel> ConsultarEmpleadoCuchillo(EmpleadoCuchilloViewModel filtros)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                clsDEmpleado = new clsDEmpleado();
                IEnumerable<EMPLEADO_CUCHILLO> EmpleadosCuchillos = entities.EMPLEADO_CUCHILLO;

                if (filtros.NumeroCuchillo > 0)
                {
                    EmpleadosCuchillos = EmpleadosCuchillos.Where(x => x.NumeroCuchillo == filtros.NumeroCuchillo);
                }

                if (!string.IsNullOrEmpty(filtros.Cedula))
                {
                    EmpleadosCuchillos = EmpleadosCuchillos.Where(x => x.Cedula == filtros.Cedula);
                }

                if (!string.IsNullOrEmpty(filtros.ColorCuchillo))
                {
                    EmpleadosCuchillos = EmpleadosCuchillos.Where(x => x.ColorCuchillo == filtros.ColorCuchillo);
                }


                List<EmpleadoCuchilloViewModel> Listado = (from c in EmpleadosCuchillos
                                                 join color in entities.CLASIFICADOR on c.ColorCuchillo equals color.Codigo
                                                 where color.Grupo == clsAtributos.CodigoGrupoColorCuchillo
                                                 select new EmpleadoCuchilloViewModel
                                                 {
                                                     IdEmpleadoCuchillo = c.IdEmpleadoCuchillo,
                                                     Cedula = c.Cedula,
                                                     ColorCuchillo = color.Descripcion,
                                                     EstadoRegistro = c.EstadoRegistro,
                                                     FechaIngresoLog = c.FechaIngresoLog,
                                                     FechaModificacionLog = c.FechaModificacionLog,
                                                     NumeroCuchillo = c.NumeroCuchillo,
                                                     TerminalIngresoLog = c.TerminalIngresoLog,
                                                     TerminalModificacionLog = c.TerminalModificacionLog,
                                                     UsuarioIngresoLog = c.UsuarioIngresoLog,
                                                     UsuarioModificacionLog = c.UsuarioModificacionLog
                                                 }
                                                 ).ToList();

                foreach (var x in Listado)
                {
                    var empleado = clsDEmpleado.ConsultaEmpleado(x.Cedula).FirstOrDefault();
                    if(empleado!=null)
                        x.Nombre = empleado.NOMBRES??"";

                }


                return Listado;
            }
        }

        public List<CUCHILLO> ConsultarCuchillos(CUCHILLO filtros)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                IEnumerable<CUCHILLO> Cuchillos = entities.CUCHILLO;

                if (filtros.NumeroCuchillo > 0)
                {
                    Cuchillos = Cuchillos.Where(x => x.NumeroCuchillo == filtros.NumeroCuchillo);
                }

                if (!string.IsNullOrEmpty(filtros.ColorCuchillo))
                {
                    Cuchillos = Cuchillos.Where(x => x.ColorCuchillo == filtros.ColorCuchillo);
                }

                IEnumerable<CUCHILLO> Listado = (from c in Cuchillos
                                                 join color in entities.CLASIFICADOR on c.ColorCuchillo equals color.Codigo
                                                 where color.Grupo==clsAtributos.CodigoGrupoColorCuchillo
                                                 select new CUCHILLO {
                                                     ColorCuchillo = color.Descripcion,
                                                     EstadoRegistro =c.EstadoRegistro,
                                                     FechaIngresoLog=c.FechaIngresoLog,
                                                     FechaModificacionLog=c.FechaModificacionLog,
                                                     NumeroCuchillo=c.NumeroCuchillo,
                                                     TerminalIngresoLog = c.TerminalIngresoLog,
                                                     TerminalModificacionLog = c.TerminalModificacionLog,
                                                     UsuarioIngresoLog=c.UsuarioIngresoLog,
                                                     UsuarioModificacionLog=c.UsuarioModificacionLog
                                                 }
                                                 );

              


                return Listado.ToList();
            }

        }

        public  string GuardarModificarCuchillo(CUCHILLO model)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {

              var Listado = entities.CUCHILLO.FirstOrDefault(x=> x.NumeroCuchillo == model.NumeroCuchillo && x.ColorCuchillo == model.ColorCuchillo);
                if (Listado != null)
                {
                    Listado.EstadoRegistro = model.EstadoRegistro;
                    Listado.FechaModificacionLog = model.FechaIngresoLog;
                    Listado.TerminalModificacionLog = model.TerminalIngresoLog;
                    Listado.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    
                }
                else
                {
                    entities.CUCHILLO.Add(model);
                }
                entities.SaveChanges();

             return clsAtributos.MsjRegistroGuardado;
            }

        }
    }
}