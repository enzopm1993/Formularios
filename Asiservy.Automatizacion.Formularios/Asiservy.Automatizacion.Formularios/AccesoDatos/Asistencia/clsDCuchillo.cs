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

        public void ActualizarControlCuchiillo(string cedula, string color)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                var ControlCuchillo = entities.CONTROL_CUCHILLO.Where(x=>x.Cedula== cedula).ToList();
                if (ControlCuchillo.Any())
                {
                    foreach (var x in ControlCuchillo)
                    {
                        if (color == clsAtributos.CodigoColorCuchilloBlanco)
                            x.CuchilloBlanco = 0;
                        if (color == clsAtributos.CodigoColorCuchilloRojo)
                            x.CuchilloRojo = 0;
                        if (color == clsAtributos.CodigoColorCuchilloNegro)
                            x.CuchilloNegro = 0;
                    }
                    entities.SaveChanges();
                }

            }
        }


        public string GuardarModificarControlCuchillo(CONTROL_CUCHILLO model,bool check)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                DateTime FechaDesde = DateTime.Now.Date;
                DateTime FechaHasta = DateTime.Now.AddDays(1).Date;
                DateTime fechaInicio =Convert.ToDateTime(DateTime.Now.ToShortDateString());
                DateTime fechaFin = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());

                //
                if (model.EstadoCuchillo == clsAtributos.Entrada)
                {
                    if (model.CuchilloBlanco != 0)
                    {
                        bool ExisteBlanco = entities.CONTROL_CUCHILLO.Where(x => x.Fecha >= fechaInicio && x.Tipo=="P" && x.Fecha < fechaFin).Any(z => z.CuchilloBlanco == model.CuchilloBlanco);
                        if (ExisteBlanco && check)
                            return "No es posible asignar el cuchillo, por que ya ha sido prestado";
                    }
                    if (model.CuchilloRojo != 0)
                    {
                        bool ExisteRojo = entities.CONTROL_CUCHILLO.Where(x => x.Fecha >= fechaInicio && x.Tipo == "P" && x.Fecha < fechaFin).Any(z => z.CuchilloRojo == model.CuchilloRojo);
                        if (ExisteRojo && check)
                            return "No es posible asignar el cuchillo, por que ya ha sido prestado";
                    }
                    if (model.CuchilloNegro != 0)
                    {
                        bool ExisteNegro = entities.CONTROL_CUCHILLO.Where(x => x.Fecha >= fechaInicio && x.Tipo == "P" && x.Fecha < fechaFin).Any(z => z.CuchilloNegro == model.CuchilloNegro);
                        if (ExisteNegro && check)
                            return "No es posible asignar el cuchillo, por que ya ha sido prestado";
                    }
                }
                else {
                    if (check)
                    {
                        if (model.EstadoCuchillo != clsAtributos.Salida)
                        {
                            var estado = (int.Parse(model.EstadoCuchillo) - 1).ToString();
                            bool validar = entities.CONTROL_CUCHILLO.Where(x => x.Fecha >= fechaInicio && x.Fecha < fechaFin
                                                                          && x.EstadoCuchillo == estado
                                                                          && x.Cedula == model.Cedula
                                                                          && ((x.CuchilloBlanco == model.CuchilloBlanco && model.CuchilloBlanco > 0)
                                                                             || (x.CuchilloRojo == model.CuchilloRojo && model.CuchilloRojo > 0)
                                                                             || (x.CuchilloNegro == model.CuchilloNegro && model.CuchilloNegro > 0))).Any();
                            if (!validar)
                                return "No se ha marcado el estado anterior para este cuchillo";
                        }
                        else
                        {
                            bool validar = entities.CONTROL_CUCHILLO.Where(x => x.Fecha >= fechaInicio && x.Fecha < fechaFin
                                                                          && x.EstadoCuchillo == clsAtributos.Entrada
                                                                          && x.Cedula == model.Cedula
                                                                          && ((x.CuchilloBlanco == model.CuchilloBlanco && model.CuchilloBlanco > 0)
                                                                             || (x.CuchilloRojo == model.CuchilloRojo && model.CuchilloRojo > 0)
                                                                             || (x.CuchilloNegro == model.CuchilloNegro && model.CuchilloNegro > 0))).Any();
                            if (!validar)
                                return "No se ha marcado la entrega de este cuchillo";
                        }
                    }
                }
              
                //validacion de que nno exista el cuchillo en control de cuchillo
               
                var controlCuchillo = entities.CONTROL_CUCHILLO.FirstOrDefault(x=> x.Cedula == model.Cedula
                && x.EstadoCuchillo == model.EstadoCuchillo && x.Fecha > FechaDesde && x.Fecha < FechaHasta);
                if (controlCuchillo != null)
                {
                    controlCuchillo.CuchilloBlanco = model.CuchilloBlanco!=0 ? 
                                                    check? model.CuchilloBlanco:0
                                                    : controlCuchillo.CuchilloBlanco;
                    controlCuchillo.CuchilloRojo = model.CuchilloRojo != 0 ?
                                                    check ? model.CuchilloRojo : 0
                                                    : controlCuchillo.CuchilloRojo;
                    controlCuchillo.CuchilloNegro = model.CuchilloNegro != 0 ?
                                                    check ? model.CuchilloNegro : 0
                                                    : controlCuchillo.CuchilloNegro;
                    controlCuchillo.Fecha = DateTime.Now;
                    controlCuchillo.FechaModificacionLog = DateTime.Now;
                    controlCuchillo.UsuarioModificacionLog = model.UsuarioIngresoLog;
                    controlCuchillo.TerminalModificacionLog = model.TerminalIngresoLog;

                }
                else
                {
                    entities.CONTROL_CUCHILLO.Add(model);
                }
                entities.SaveChanges();

                return clsAtributos.MsjRegistroGuardado;
            }
        }
        public List<ControlCuchilloViewModel> ConsultaControlCuchillos()
        {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                DateTime fechaInicio=Convert.ToDateTime(DateTime.Now.ToShortDateString());
                DateTime FechaFin=Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());
                List<CONTROL_CUCHILLO> controlCuchillos = db.CONTROL_CUCHILLO.Where(x => x.Fecha >= fechaInicio && x.Fecha < FechaFin).ToList();
                List<ControlCuchilloViewModel> ControlCuchillosViewModel = new List<ControlCuchilloViewModel>();
                foreach (var x in controlCuchillos)
                {
                    ControlCuchillosViewModel.Add(new ControlCuchilloViewModel
                    {
                        Cedula = x.Cedula,
                        CuchilloBlanco = x.CuchilloBlanco,
                        CuchilloRojo = x.CuchilloRojo,
                        CuchilloNegro = x.CuchilloNegro,
                    });
                }
                return ControlCuchillosViewModel;
            }
        }
        public List<ControlCuchilloViewModel> ConsultarEmpleadosCuchilloPorLinea(string Linea, string Estado)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                List<ControlCuchilloViewModel> ListadoEmpleadoCuchillo = new List<ControlCuchilloViewModel>();
                var consulta = entities.spConsutaEmpleadosCuchillos(Linea, Estado).ToList();
                if(consulta != null)
                {
                    foreach(var x in consulta)
                    {
                        ListadoEmpleadoCuchillo.Add(new ControlCuchilloViewModel
                        {
                            Cedula = x.Cedula,
                            Nombre = x.Nombre,
                            CuchilloBlanco = x.CuchilloBlanco,
                            ValidaBlanco=x.ValidaBlanco,
                            CuchilloRojo=x.CuchilloRojo,
                            ValidaRojo=x.ValidaRojo,
                            CuchilloNegro=x.CuchilloNegro,
                            ValidaNegro=x.ValidaNegro
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
        
            public List<EmpleadoCuchilloViewModel> ConsultarEmpleadoCuchillo(EmpleadoCuchilloViewModel filtros, string Linea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                clsDEmpleado = new clsDEmpleado();
                IEnumerable<EMPLEADO_CUCHILLO> EmpleadosCuchillos = entities.EMPLEADO_CUCHILLO;
                List<EmpleadoCuchilloViewModel> ListadoEmpleadoCuchillo = new List<EmpleadoCuchilloViewModel>();


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
                    if (empleado != null && empleado.CODIGOLINEA == Linea)
                    {
                        x.Nombre = empleado.NOMBRES ?? "";
                        ListadoEmpleadoCuchillo.Add(x);
                    }
                }


                return ListadoEmpleadoCuchillo;
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

        public List<spConsultaReporteControlCuchillo> ConsultaControlCuchillo(DateTime Fecha, string Linea)
        {
            using (ASIS_PRODEntities entities = new ASIS_PRODEntities())
            {
                return entities.spConsultaReporteControlCuchillo(Fecha,Linea).ToList();
               
            }

        }
    }
}