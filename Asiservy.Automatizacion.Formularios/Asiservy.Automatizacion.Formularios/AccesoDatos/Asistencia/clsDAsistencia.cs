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
        List<sp_ConsultaAsistenciaGeneralDiaria> pListAsistenciaGeneral = null;
        List<ASISTENCIA> pListAsistenciaExiste = null;
        clsDCambioPersonal clsdCambioPersonal = null;
        List<sp_ConsultaAsistenciaDiariaPersonalMovido> pListAsistenciaMovidos = null;
        spConsutaEmpleados BuscarControlador = null;

        public string ModificarAsistencia(ASISTENCIA model)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var Asistencia = db.ASISTENCIA.FirstOrDefault(x=> x.IdAsistencia == model.IdAsistencia);
                if (Asistencia != null) {
                    Asistencia.Hora = model.Hora;
                    Asistencia.EstadoAsistencia = model.EstadoAsistencia;
                    Asistencia.Observacion = model.Observacion;
                    Asistencia.FechaModificacionLog = DateTime.Now;
                    Asistencia.UsuarioModificacionLog = model.UsuarioCreacionLog;
                    Asistencia.TerminalModificacionLog = model.TerminalCreacionLog;
                    db.SaveChanges();
                    return clsAtributos.MsjRegistroGuardado;
                }
                else
                {
                    return clsAtributos.MsjRegistroError;

                }


            }
        }

        public List<spConsultaControlAsistencia> ConsultaControlAsistencia(string Linea, DateTime Fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<spConsultaControlAsistencia> Listado = null;
                Listado = db.spConsultaControlAsistencia(Linea, Fecha).ToList(); 

                return Listado;
            }
        }
        public int ConsultarExistenciaAsistenciaGeneral(string cedula, string Turno)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                DateTime fechaInicio = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                DateTime fechaFin = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());
                BuscarControlador = db.spConsutaEmpleados(cedula).ToList().FirstOrDefault();
                ////pListAsistencia = db.sp_ConsultaAsistenciaDiaria(BuscarControlador.CODIGOLINEA+"",1).ToList();
                pListAsistenciaExiste = db.ASISTENCIA.Where(x => x.Fecha >= fechaInicio && x.Fecha < fechaFin && x.Linea== BuscarControlador.CODIGOLINEA &&x.Turno==Turno).ToList();

                //IQueryable<ASISTENCIA> query = (from a in db.ASISTENCIA
                //                                join b in db.CAMBIO_PERSONAL on new { a.Cedula, EstadoRegistro = clsAtributos.EstadoRegistroActivo, CodLinea = BuscarControlador.CODIGOLINEA } equals new { b.Cedula, b.EstadoRegistro, b.CodLinea } into c
                //                                from b in c.DefaultIfEmpty()
                //                                where a.Fecha >= fechaInicio && a.Fecha <= fechaFin
                //             // && b.CodLinea==BuscarControlador.CODIGOLINEA
                //             && a.Turno == Turno
                //             //&& b.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                //             //&& b.Cedula == a.Cedula
                //             && b == null
                //                                select a);
                //pListAsistenciaExiste = query.ToList();
            }
            ////if (pListAsistencia.ToList().Count == 0)
            if (pListAsistenciaExiste.Count == 0)
                return 0;
            else
                return 1;
        }
        public int ConsultarExistenciaAsistencia(string cedula, string Turno)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                DateTime fechaInicio = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(-12);
                DateTime fechaFin= Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());
                BuscarControlador = db.spConsutaEmpleados(cedula).ToList().FirstOrDefault();
                ////pListAsistencia = db.sp_ConsultaAsistenciaDiaria(BuscarControlador.CODIGOLINEA+"",1).ToList();
                //pListAsistenciaExiste = db.ASISTENCIA.Where(x => x.Fecha >= fechaInicio && x.Fecha < fechaFin && x.Linea== BuscarControlador.CODIGOLINEA &&x.Turno==Turno).ToList();

                IQueryable<ASISTENCIA> query = (from a in db.ASISTENCIA 
                                                join b in db.CAMBIO_PERSONAL on new {a.Cedula, EstadoRegistro=clsAtributos.EstadoRegistroActivo, CodLinea= BuscarControlador.CODIGOLINEA } equals new {b.Cedula, b.EstadoRegistro,b.CodLinea } into c
                                                from b in c.DefaultIfEmpty()
                                                where a.Fecha >= fechaInicio && a.Fecha <= fechaFin 
                            // && b.CodLinea==BuscarControlador.CODIGOLINEA
                             && a.Linea== BuscarControlador.CODIGOLINEA//para que me traiga solo la asistencia de la linea que pertenece
                             && a.Turno==Turno 
                             //&& b.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                             //&& b.Cedula == a.Cedula
                             && b == null
                             select a);
                pListAsistenciaExiste = query.ToList();
            }
            ////if (pListAsistencia.ToList().Count == 0)
            if (pListAsistenciaExiste.Count == 0)
                return 0;
            else
                return 1;
        }
        public int ConsultarExistenciaAsistenciaPrestados(string cedula, string Turno)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                BuscarControlador = db.spConsutaEmpleados(cedula).ToList().FirstOrDefault();
                pListAsistenciaMovidos = db.sp_ConsultaAsistenciaDiariaPersonalMovido(BuscarControlador.CODIGOLINEA + "", Convert.ToInt32(Turno)).ToList();
            }
            if (pListAsistenciaMovidos.ToList().Count == 0)
                return 0;
            else
                return 1;
        }
        public ControlDeAsistenciaGeneralViewModel ObtenerAsistenciaGeneralDiaria(string CodLinea, int BanderaExiste, string usuario, string terminal, string turno)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<ASISTENCIA> ControlAsistencia = null;
                clsDCambioPersonal clsDCambioPersonal = new clsDCambioPersonal();

                ControlDeAsistenciaGeneralViewModel ControlAsistenciaGeneralViewModel = null;
                if (BanderaExiste == 0)
                {

                    //List<spConsutaEmpleadosFiltro> ListaEmpleados = db.spConsutaEmpleadosFiltro("0", CodLinea, "0").Where(x => x.CODIGOCARGO != "221").ToList();
                    List<spConsultarEmpleadosxTurno> ListaEmpleados = db.spConsultarEmpleadosxTurno(CodLinea, turno).ToList();
                    ControlAsistencia = new List<ASISTENCIA>();
                    foreach (var item in ListaEmpleados)
                    {
                        //var FueMovidoAOtraArea = clsDCambioPersonal.ConsultarCambioPersonal(item.CEDULA);
                        //if (FueMovidoAOtraArea == null)
                            ControlAsistencia.Add(new ASISTENCIA { Cedula = item.CEDULA, Fecha = DateTime.Now, EstadoAsistencia = clsAtributos.EstadoFalta, Linea = CodLinea, Turno = turno, Observacion = "", UsuarioCreacionLog = usuario, TerminalCreacionLog = terminal, FechaCreacionLog = DateTime.Now, EstadoRegistro = "A" });

                    }
                    var PersonalMovidoAEstaLinea = clsDCambioPersonal.ConsultarCambioPersonalxLinea(CodLinea, turno);
                    foreach (var item in PersonalMovidoAEstaLinea)
                    {
                        ControlAsistencia.Add(new ASISTENCIA { Cedula = item.Cedula, Fecha = DateTime.Now, EstadoAsistencia = clsAtributos.EstadoFalta, Linea = item.CodLinea, Turno = turno, Observacion = "", UsuarioCreacionLog = usuario, TerminalCreacionLog = terminal, FechaCreacionLog = DateTime.Now, EstadoRegistro = "A" });

                    }
                    db.ASISTENCIA.AddRange(ControlAsistencia);
                    db.SaveChanges();
                    pListAsistenciaGeneral = db.sp_ConsultaAsistenciaGeneralDiaria(CodLinea, Convert.ToInt32(turno)).ToList();
                    //pListAsistenciaGeneral.ForEach(x => x.Hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")));
                    foreach (var item in pListAsistenciaGeneral)
                    {
                        item.Hora = item.EstadoAsistencia == clsAtributos.EstadoFalta ? TimeSpan.Parse(DateTime.Now.ToString("HH:mm")) : item.Hora;
                    }
                    ControlAsistenciaGeneralViewModel = new ControlDeAsistenciaGeneralViewModel
                    {
                        ControlAsistencia = pListAsistenciaGeneral.OrderBy(z => z.NOMBRES).ToList()
                    };
                }
                else
                {
                    pListAsistenciaGeneral = db.sp_ConsultaAsistenciaGeneralDiaria(CodLinea, Convert.ToInt32(turno)).ToList();
                    //pListAsistenciaGeneral.ForEach(x => x.Hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")));
                    foreach (var item in pListAsistenciaGeneral)
                    {
                        item.Hora = item.EstadoAsistencia == clsAtributos.EstadoFalta ? TimeSpan.Parse(DateTime.Now.ToString("HH:mm")) : item.Hora;
                    }
                    ControlAsistenciaGeneralViewModel = new ControlDeAsistenciaGeneralViewModel
                    {
                        ControlAsistencia = pListAsistenciaGeneral.OrderBy(z => z.NOMBRES).ToList()
                    };
                }
                return ControlAsistenciaGeneralViewModel;
            }
        }
        public ControlDeAsistenciaViewModel ObtenerAsistenciaDiaria(string CodLinea, int BanderaExiste, string usuario, string terminal,string turno)
        {
            using (ASIS_PRODEntities db=new ASIS_PRODEntities())
            {
                
                List<ASISTENCIA> ControlAsistencia = null;
                clsDCambioPersonal clsDCambioPersonal = new clsDCambioPersonal();

                ControlDeAsistenciaViewModel ControlAsistenciaViewModel = null;
                if (BanderaExiste == 0)
                {

                    //List<spConsutaEmpleadosFiltro> ListaEmpleados = db.spConsutaEmpleadosFiltro("0", CodLinea, "0").Where(x => x.CODIGOCARGO != "221").ToList();
                    List<spConsultarEmpleadosxTurno> ListaEmpleados = db.spConsultarEmpleadosxTurno(CodLinea, turno).ToList();
                    ControlAsistencia = new List<ASISTENCIA>();
                    foreach (var item in ListaEmpleados)
                    {
                        //var FueMovidoAOtraArea = clsDCambioPersonal.ConsultarCambioPersonal(item.CEDULA);
                        //if (FueMovidoAOtraArea==null)
                        ControlAsistencia.Add(new ASISTENCIA { Cedula = item.CEDULA, Fecha = DateTime.Now, EstadoAsistencia = clsAtributos.EstadoFalta, Linea = item.CODIGOLINEA, Turno=turno, Observacion="", UsuarioCreacionLog=usuario,TerminalCreacionLog=terminal, FechaCreacionLog=DateTime.Now, EstadoRegistro="A" });
                    }
                    db.ASISTENCIA.AddRange(ControlAsistencia);
                    db.SaveChanges();
                    pListAsistencia = db.sp_ConsultaAsistenciaDiaria(CodLinea,Convert.ToInt32(turno)).ToList();
                    //pListAsistencia.ForEach(x => x.Hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")));
                    foreach (var item in pListAsistencia)
                    {
                        item.Hora = item.EstadoAsistencia == clsAtributos.EstadoFalta ? TimeSpan.Parse(DateTime.Now.ToString("HH:mm")) : item.Hora;
                    }
                    ControlAsistenciaViewModel = new ControlDeAsistenciaViewModel
                    {
                        ControlAsistencia = pListAsistencia.OrderBy(z => z.NOMBRES).ToList()
                    };
                }
                else
                {
                    pListAsistencia = db.sp_ConsultaAsistenciaDiaria(CodLinea, Convert.ToInt32(turno)).ToList();
                    //pListAsistencia.ForEach(x => x.Hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")));
                    foreach (var item in pListAsistencia)
                    {
                        item.Hora = item.EstadoAsistencia == clsAtributos.EstadoFalta ? TimeSpan.Parse(DateTime.Now.ToString("HH:mm")) : item.Hora;
                    }
                    ControlAsistenciaViewModel = new ControlDeAsistenciaViewModel
                    {
                        ControlAsistencia = pListAsistencia.OrderBy(z => z.NOMBRES).ToList()
                    };
                }
                    
               
                return ControlAsistenciaViewModel;
            }
        }

        public ControlDeAsistenciaPrestadosViewModel ObtenerAsistenciaDiariaMovidos(string CodLinea, int BanderaExiste, string usuario, string terminal,string turno)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                List<ASISTENCIA> ControlAsistencia = null;
                clsDCambioPersonal clsDCambioPersonal = new clsDCambioPersonal();

                ControlDeAsistenciaPrestadosViewModel ControlDeAsistenciaPrestadosViewModel = null;
                if (BanderaExiste == 0)
                {
                    ControlAsistencia = new List<ASISTENCIA>();
                    //var EmpleadosMovidos = db.CAMBIO_PERSONAL.Where(z => z.CodLinea == CodLinea && z.EstadoRegistro == clsAtributos.EstadoRegistroActivo);
                    clsdCambioPersonal = new clsDCambioPersonal();
                    var EmpleadosMovidos = clsdCambioPersonal.ConsultarCambioPersonalxLinea(CodLinea,turno);
                    foreach (var item in EmpleadosMovidos)
                    {
                       ControlAsistencia.Add(new ASISTENCIA { Cedula = item.Cedula, Fecha = DateTime.Now, EstadoAsistencia = clsAtributos.EstadoFalta, Linea = item.CodLinea, Turno = turno, Observacion = "", UsuarioCreacionLog = usuario, TerminalCreacionLog = terminal, FechaCreacionLog = DateTime.Now, EstadoRegistro = "A" });
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
                    pListAsistenciaMovidos = db.sp_ConsultaAsistenciaDiariaPersonalMovido(CodLinea, Convert.ToInt32(turno)).ToList();
                    //pListAsistenciaMovidos.ForEach(x => x.Hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")));
                    foreach (var item in pListAsistenciaMovidos)
                    {
                        item.Hora = item.EstadoAsistencia == clsAtributos.EstadoFalta ? TimeSpan.Parse(DateTime.Now.ToString("HH:mm")) : item.Hora;
                    }
                    ControlDeAsistenciaPrestadosViewModel = new ControlDeAsistenciaPrestadosViewModel
                    {
                        ControlAsistencia = pListAsistenciaMovidos.OrderBy(z => z.NOMBRES).ToList()
                    };
                }
                else
                {
                    pListAsistenciaMovidos = db.sp_ConsultaAsistenciaDiariaPersonalMovido(CodLinea, Convert.ToInt32(turno)).ToList();
                    //pListAsistenciaMovidos.ForEach(x => x.Hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")));
                    foreach (var item in pListAsistenciaMovidos)
                    {
                        item.Hora = item.EstadoAsistencia == clsAtributos.EstadoFalta ? TimeSpan.Parse(DateTime.Now.ToString("HH:mm")) : item.Hora;
                    }
                    ControlDeAsistenciaPrestadosViewModel = new ControlDeAsistenciaPrestadosViewModel
                    {
                        ControlAsistencia = pListAsistenciaMovidos.OrderBy(z => z.NOMBRES).ToList()
                    };
                }
                List<int?> CuchillosBlancos = db.sp_ObtenerCuchillosSobrantes(clsAtributos.CodigoColorCuchilloBlanco).ToList(); 
                List<int?> CuchillosRojos = db.sp_ObtenerCuchillosSobrantes(clsAtributos.CodigoColorCuchilloRojo).ToList();
                List<int?> CuchillosNegros = db.sp_ObtenerCuchillosSobrantes(clsAtributos.CodigoColorCuchilloNegro).ToList();
                List<ControlDeAsistenciaPrestadosViewModel.Cuchillos> CuchillosBlancosSobrantes = new List<ControlDeAsistenciaPrestadosViewModel.Cuchillos>();
                List<ControlDeAsistenciaPrestadosViewModel.Cuchillos> CuchillosRojosSobrantes = new List<ControlDeAsistenciaPrestadosViewModel.Cuchillos>();
                List<ControlDeAsistenciaPrestadosViewModel.Cuchillos> CuchillosNegrosSobrantes = new List<ControlDeAsistenciaPrestadosViewModel.Cuchillos>();
                foreach (var item in CuchillosBlancos)
                {
                    CuchillosBlancosSobrantes.Add(new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Id=item, Numero=item});
                }
                foreach (var item in CuchillosRojos)
                {
                    CuchillosRojosSobrantes.Add(new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Id = item, Numero = item });
                }
                foreach (var item in CuchillosNegros)
                {
                    CuchillosNegrosSobrantes.Add(new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Id = item, Numero = item });
                }
                ControlDeAsistenciaPrestadosViewModel.CuchillosBlancosSobrantes = CuchillosBlancosSobrantes;
                ControlDeAsistenciaPrestadosViewModel.CuchillosRojosSobrantes = CuchillosRojosSobrantes;
                ControlDeAsistenciaPrestadosViewModel.CuchillosNegrosSobrantes = CuchillosNegrosSobrantes;
                return ControlDeAsistenciaPrestadosViewModel;
            }
        }

        public string ActualizarAsistencia(ASISTENCIA psAsistencia)
        {
            using(ASIS_PRODEntities db =new  ASIS_PRODEntities())
            {
                DateTime Fechainicio =Convert.ToDateTime(DateTime.Now.ToShortDateString());
                DateTime FechaFin = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());
                var BuscarEnAsistencia = db.ASISTENCIA.Where(x => x.Cedula == psAsistencia.Cedula && (x.Fecha >= Fechainicio && x.Fecha < FechaFin)).FirstOrDefault();
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