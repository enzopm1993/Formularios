using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asiservy.Automatizacion.Datos.Datos;

using Asiservy.Automatizacion.Formularios.Models.Asistencia;

namespace Asiservy.Automatizacion.Formularios.AccesoDatos.Asistencia
{
    public class clsDAsistencia
    {
        List<sp_ConsultaAsistenciaDiaria> pListAsistencia = null;

        List<sp_ConsultaAsistenciaDiariaPersonalMovido> pListAsistenciaMovidos = null;
        spConsutaEmpleados BuscarControlador = null;
        public int ConsultarExistenciaAsistencia(string cedula)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                BuscarControlador = db.spConsutaEmpleados(cedula).ToList().FirstOrDefault();
                pListAsistencia = db.sp_ConsultaAsistenciaDiaria(BuscarControlador.CODIGOLINEA+"",1).ToList();
            }
            if (pListAsistencia.ToList().Count == 0)
                return 0;
            else
                return 1;
        }
        public int ConsultarExistenciaAsistenciaPrestados(string cedula)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                BuscarControlador = db.spConsutaEmpleados(cedula).ToList().FirstOrDefault();
                pListAsistenciaMovidos = db.sp_ConsultaAsistenciaDiariaPersonalMovido(BuscarControlador.CODIGOLINEA + "", 1).ToList();
            }
            if (pListAsistenciaMovidos.ToList().Count == 0)
                return 0;
            else
                return 1;
        }
        public ControlDeAsistenciaViewModel ObtenerAsistenciaDiaria(string CodLinea, int BanderaExiste, string usuario, string terminal)
        {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                
                List<ASISTENCIA> ControlAsistencia = null;
                clsDCambioPersonal clsDCambioPersonal = new clsDCambioPersonal();

                ControlDeAsistenciaViewModel ControlAsistenciaViewModel = null;
                if (BanderaExiste == 0)
                {

                    List<spConsutaEmpleadosFiltro> ListaEmpleados = db.spConsutaEmpleadosFiltro("0", CodLinea, "0").Where(x => x.CODIGOCARGO != "221").ToList();
                    ControlAsistencia = new List<ASISTENCIA>();
                    foreach (var item in ListaEmpleados)
                    {
                        var FueMovidoAOtraArea = clsDCambioPersonal.ConsultarCambioPersonal(item.CEDULA);
                        if (FueMovidoAOtraArea==null)
                        ControlAsistencia.Add(new ASISTENCIA { Cedula = item.CEDULA, Fecha = DateTime.Now, EstadoAsistencia = clsAtributos.EstadoFalta, Linea = item.CODIGOLINEA, Turno="1", Observacion="", UsuarioCreacionLog=usuario,TerminalCreacionLog=terminal, FechaCreacionLog=DateTime.Now, EstadoRegistro="A" });

                    }
                    db.ASISTENCIA.AddRange(ControlAsistencia);
                    db.SaveChanges();
                    pListAsistencia = db.sp_ConsultaAsistenciaDiaria(CodLinea,1).ToList();
                    pListAsistencia.ForEach(x => x.Hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")));
                    ControlAsistenciaViewModel = new ControlDeAsistenciaViewModel
                    {
                        ControlAsistencia = pListAsistencia.OrderBy(z => z.NOMBRES).ToList()
                    };
                }
                else
                {
                    pListAsistencia = db.sp_ConsultaAsistenciaDiaria(CodLinea,1).ToList();
                    pListAsistencia.ForEach(x => x.Hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")));
                    ControlAsistenciaViewModel = new ControlDeAsistenciaViewModel
                    {
                        ControlAsistencia = pListAsistencia.OrderBy(z => z.NOMBRES).ToList()
                    };
                }
                    
               
                return ControlAsistenciaViewModel;
            }
        }

        public ControlDeAsistenciaPrestadosViewModel ObtenerAsistenciaDiariaMovidos(string CodLinea, int BanderaExiste, string usuario, string terminal)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                List<ASISTENCIA> ControlAsistencia = null;
                clsDCambioPersonal clsDCambioPersonal = new clsDCambioPersonal();

                ControlDeAsistenciaPrestadosViewModel ControlDeAsistenciaPrestadosViewModel = null;
                if (BanderaExiste == 0)
                {
                    ControlAsistencia = new List<ASISTENCIA>();
                    var EmpleadosMovidos = db.CAMBIO_PERSONAL.Where(z => z.CodLinea == CodLinea && z.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    foreach (var item in EmpleadosMovidos)
                    {
                       ControlAsistencia.Add(new ASISTENCIA { Cedula = item.Cedula, Fecha = DateTime.Now, EstadoAsistencia = clsAtributos.EstadoFalta, Linea = item.CodLinea, Turno = "1", Observacion = "", UsuarioCreacionLog = item.UsuarioIngresoLog, TerminalCreacionLog = terminal, FechaCreacionLog = DateTime.Now, EstadoRegistro = "A" });
                    }
                    //List<spConsutaEmpleadosFiltro> ListaEmpleados = db.spConsutaEmpleadosFiltro("0", CodLinea, "0").Where(x => x.CODIGOCARGO != "221").ToList();
                    //ControlAsistencia = new List<ASISTENCIA>();
                    //foreach (var item in ListaEmpleados)
                    //{
                    //    var FueMovidoAOtraArea = clsDCambioPersonal.ConsultarCambioPersonal(item.CEDULA);
                    //    if (FueMovidoAOtraArea != null)
                    //        ControlAsistencia.Add(new ASISTENCIA { Cedula = item.CEDULA, Fecha = DateTime.Now, EstadoAsistencia = clsAtributos.EstadoFalta, Linea = item.CODIGOLINEA, Turno = "1", Observacion = "", UsuarioCreacionLog = usuario, TerminalCreacionLog = terminal, FechaCreacionLog = DateTime.Now, EstadoRegistro = "A" });

                    //}
                    db.ASISTENCIA.AddRange(ControlAsistencia);
                    db.SaveChanges();
                    pListAsistenciaMovidos = db.sp_ConsultaAsistenciaDiariaPersonalMovido(CodLinea, 1).ToList();
                    pListAsistenciaMovidos.ForEach(x => x.Hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")));
                    ControlDeAsistenciaPrestadosViewModel = new ControlDeAsistenciaPrestadosViewModel
                    {
                        ControlAsistencia = pListAsistenciaMovidos.OrderBy(z => z.NOMBRES).ToList()
                    };
                }
                else
                {
                    pListAsistenciaMovidos = db.sp_ConsultaAsistenciaDiariaPersonalMovido(CodLinea, 1).ToList();
                    pListAsistenciaMovidos.ForEach(x => x.Hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")));
                    ControlDeAsistenciaPrestadosViewModel = new ControlDeAsistenciaPrestadosViewModel
                    {
                        ControlAsistencia = pListAsistenciaMovidos.OrderBy(z => z.NOMBRES).ToList()
                    };
                }
                List<int?> CuchillosBlancos = new List<int?>();
                List<int?> CuchillosRojos = new List<int?>();
                List<int?> CuchillosNegros = new List<int?>();
                List<int?> CuchilloBlancoNoAsignado;
                List<int?> CuchilloNegroNoAsignado;
                List<int?> CuchilloRojoNoAsignado;
                var EmpleadosXCuchillos = db.EMPLEADO_CUCHILLO.ToList();
                var ControlCuchillos = db.CONTROL_CUCHILLO.Where(x => x.Fecha.ToShortDateString() == DateTime.Now.ToShortDateString()).ToList();
                foreach (var item in ControlCuchillos)
                {
                    CuchilloBlancoNoAsignado = (from c in EmpleadosXCuchillos
                             where c.Cedula == item.Cedula && c.CuchilloBlanco != item.CuchilloBlanco
                             select c.CuchilloBlanco).ToList();
                    if (CuchilloBlancoNoAsignado.Count > 0)
                    {
                        CuchillosBlancos.Add(CuchilloBlancoNoAsignado.FirstOrDefault());
                    }
                    CuchilloRojoNoAsignado = (from c in EmpleadosXCuchillos
                                                where c.Cedula == item.Cedula && c.CuchilloRojo != item.CuchilloRojo
                                                select c.CuchilloRojo).ToList();
                    if (CuchilloRojoNoAsignado.Count > 0)
                    {
                        CuchillosRojos.Add(CuchilloRojoNoAsignado.FirstOrDefault());
                    }
                    CuchilloNegroNoAsignado = (from c in EmpleadosXCuchillos
                                                where c.Cedula == item.Cedula && c.CuchilloNegro != item.CuchilloNegro
                                                select c.CuchilloNegro).ToList();
                    if (CuchilloNegroNoAsignado.Count > 0)
                    {
                        CuchillosNegros.Add(CuchilloNegroNoAsignado.FirstOrDefault());
                    }
                }

                return ControlDeAsistenciaPrestadosViewModel;
            }
        }

        public string ActualizarAsistencia(ASISTENCIA psAsistencia)
        {
            using(ASIS_PRODEntities db =new  ASIS_PRODEntities())
            {
                DateTime Fechainicio = DateTime.Now.AddDays(-1);
                DateTime FechaFin = DateTime.Now.AddDays(1);
                var BuscarEnAsistencia = db.ASISTENCIA.Where(x => x.Cedula == psAsistencia.Cedula && (x.Fecha > Fechainicio && x.Fecha < FechaFin)).FirstOrDefault();
                BuscarEnAsistencia.EstadoAsistencia = psAsistencia.EstadoAsistencia;
                if (!string.IsNullOrEmpty(psAsistencia.Observacion))
                BuscarEnAsistencia.Observacion = psAsistencia.Observacion;
                if (psAsistencia.Hora != null)
                BuscarEnAsistencia.Hora = psAsistencia.Hora;
                BuscarEnAsistencia.FechaModificacionLog = psAsistencia.FechaModificacionLog;
                BuscarEnAsistencia.UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                BuscarEnAsistencia.TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                db.SaveChanges();
                return "Registro actualizado con éxito";
            }
            
        }
    }
}