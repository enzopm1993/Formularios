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

        public List<ControlCuchilloViewModel> ConsultarEmpleadosCuchilloPorLinea(string Linea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<ControlCuchilloViewModel> ListadoEmpleadoCuchillo = new List<ControlCuchilloViewModel>();
                var consulta = entities.spConsutaEmpleadosCuchillos(Linea).ToList();
                if(consulta != null)
                {
                    foreach(var x in consulta)
                    {
                        ListadoEmpleadoCuchillo.Add(new ControlCuchilloViewModel
                        {
                            Cedula =x.Cedula,
                            Nombre=x.Nombre,
                            CuchilloBlanco=x.CuchilloBlanco,
                            CuchilloRojo=x.CuchilloRojo,
                            CuchilloNegro=x.CuchilloNegro
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
                
                if(model.CuchilloBlanco >0)
                {
                    var validacion = entities.EMPLEADO_CUCHILLO.FirstOrDefault(x=> x.CuchilloBlanco == model.CuchilloBlanco
                    && x.EstadoRegistro==clsAtributos.EstadoRegistroActivo && x.Cedula != model.Cedula);
                    if (validacion != null)
                        return "Cuchillo Blanco ya se encuentra asignado a otro empleado("+validacion.Cedula+")";
                }
                if (model.CuchilloRojo >0)
                {
                    var validacion = entities.EMPLEADO_CUCHILLO.FirstOrDefault(x => x.CuchilloRojo == model.CuchilloRojo
                    && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && x.Cedula != model.Cedula);
                    if (validacion != null)
                        return "Cuchillo Rojo ya se encuentra asignado a otro empleado(" + validacion.Cedula + ")";
                }
                if (model.CuchilloNegro > 0)
                {
                    var validacion = entities.EMPLEADO_CUCHILLO.FirstOrDefault(x => x.CuchilloNegro == model.CuchilloNegro
                    && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && x.Cedula != model.Cedula);
                    if (validacion != null)
                        return "Cuchillo Negro ya se encuentra asignado a otro empleado(" + validacion.Cedula + ")";
                }

                var EmpleadoCuchillo = entities.EMPLEADO_CUCHILLO.FirstOrDefault(x=> 
                x.IdEmpleadoCuchillo == model.IdEmpleadoCuchillo
                || (x.Cedula == model.Cedula)
                );
                if (EmpleadoCuchillo != null)
                {
                    EmpleadoCuchillo.CuchilloBlanco = model.CuchilloBlanco??0;                 
                    EmpleadoCuchillo.CuchilloRojo = model.CuchilloRojo??0;                    
                    EmpleadoCuchillo.CuchilloNegro = model.CuchilloNegro??0;
                    EmpleadoCuchillo.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    EmpleadoCuchillo.TerminalModificacionLog = model.TerminalIngresoLog;
                    EmpleadoCuchillo.FechaModificacionLog = model.FechaIngresoLog;
                    EmpleadoCuchillo.EstadoRegistro = model.EstadoRegistro;
                }
                else
                {
                    entities.EMPLEADO_CUCHILLO.Add(new EMPLEADO_CUCHILLO {
                        CuchilloBlanco =model.CuchilloBlanco??0,
                        CuchilloRojo = model.CuchilloRojo??0,
                        CuchilloNegro = model.CuchilloNegro??0,
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

                if (!string.IsNullOrEmpty(filtros.Cedula))
                {
                    EmpleadosCuchillos = EmpleadosCuchillos.Where(x => x.Cedula == filtros.Cedula);
                }               


                List<EmpleadoCuchilloViewModel> Listado = (from c in EmpleadosCuchillos                                                
                                                 select new EmpleadoCuchilloViewModel
                                                 {
                                                     IdEmpleadoCuchillo = c.IdEmpleadoCuchillo,
                                                     Cedula = c.Cedula,
                                                    CuchilloBlanco=c.CuchilloBlanco,
                                                    CuchilloRojo=c.CuchilloRojo,
                                                    CuchilloNegro=c.CuchilloNegro,
                                                     EstadoRegistro = c.EstadoRegistro,
                                                     FechaIngresoLog = c.FechaIngresoLog,
                                                     FechaModificacionLog = c.FechaModificacionLog,                                                   
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