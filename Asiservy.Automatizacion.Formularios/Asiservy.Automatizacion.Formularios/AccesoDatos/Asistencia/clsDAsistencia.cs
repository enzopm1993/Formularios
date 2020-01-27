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
        clsDEmpleado ClsdEmpleado = null;

        public Boolean CosultarAsistenciaEmpleado(string cedula, DateTime fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())  
            {
                var ConsultaAsistencia = db.ASISTENCIA.Where(x => x.Cedula == cedula && fecha == x.Fecha&&x.EstadoRegistro==clsAtributos.EstadoRegistroActivo && (x.EstadoAsistencia == clsAtributos.EstadoPresente || x.EstadoAsistencia == clsAtributos.EstadoAtraso)
                  && x.HoraSalida == null).ToList();
               if(ConsultaAsistencia.Count>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        public List<spConsultaAsistenciaFinalizar> ConsultarAsistenciaFinalizar(DateTime Fecha, string CodLinea, string Turno)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaAsistenciaFinalizar(Fecha, CodLinea, Turno).ToList();
            }
        }
        public List<spConsultaPersonalADondeFueronMovidos> ConsultaPrestadosxLinea(string codlinea, DateTime? Fecha, TimeSpan? Hora)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaPersonalADondeFueronMovidos(codlinea, Fecha, Hora).ToList();
            }
        }
        public List<spConsultaPersonalMovidoaLinea> ConsultaPersonalMovidoaLinea(string codlinea, DateTime? Fecha, TimeSpan? Hora)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaPersonalMovidoaLinea(codlinea, Fecha, Hora).ToList();
            }
        }
        public List<spConsultaPersonalMovidoaLinea> ConsultaPersonalMovidoaLineaModal(string codlinea, DateTime? Fecha, TimeSpan? Hora)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var ConsultarPersonalMovido= db.spConsultaPersonalMovidoaLinea(codlinea, Fecha, Hora).ToList();
                var AsistenciaBuscar = db.ASISTENCIA.Where(x => x.Fecha == Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();//traigo la asistencia de todas las lineas en la fecha ingresada
                List<spConsultaPersonalMovidoaLinea> PersonalMovidoAEstaLineaFiltrado = (from p in ConsultarPersonalMovido
                                                                                         join asis in AsistenciaBuscar
                                                                                                on p.CEDULA equals asis.Cedula into pp
                                                                                                from asis in pp.DefaultIfEmpty()
                                                                                                where asis == null
                                                                                                select p).ToList();
                return PersonalMovidoAEstaLineaFiltrado;
            }
        }
        public string ModificarAsistencia(ASISTENCIA model)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var Asistencia = db.ASISTENCIA.FirstOrDefault(x => x.IdAsistencia == model.IdAsistencia);
                if (Asistencia != null)
                {
                    //Asistencia.Hora = model.Hora;
                    //Asistencia.EstadoAsistencia = model.EstadoAsistencia;
                    //Asistencia.Observacion = model.Observacion;
                    //Asistencia.FechaModificacionLog = DateTime.Now;
                    //Asistencia.UsuarioModificacionLog = model.UsuarioCreacionLog;
                    //Asistencia.TerminalModificacionLog = model.TerminalCreacionLog;
                    //db.SaveChanges();

                   
                    string Resultado = ActualizarAsistencia(new ASISTENCIA
                    {
                        Cedula = Asistencia.Cedula,
                        Hora = model.Hora,
                        Observacion = model.Observacion,
                        EstadoAsistencia = model.EstadoAsistencia,
                        UsuarioModificacionLog = model.UsuarioCreacionLog,
                        TerminalModificacionLog = model.TerminalCreacionLog,
                        FechaModificacionLog = DateTime.Now,
                        Fecha = Asistencia.Fecha,
                        CentroCostos = Asistencia.CentroCostos,
                        Recurso = Asistencia.Recurso,
                        Linea = Asistencia.Linea,
                        Cargo = Asistencia.Cargo,
                        Turno = Asistencia.Turno,
                        EstadoRegistro = clsAtributos.EstadoRegistroActivo
                    });
                    return Resultado;
                    //return clsAtributos.MsjRegistroGuardado;
                }
                else
                {
                    return clsAtributos.MsjRegistroError;

                }


            }
        }

        public List<ASISTENCIA> ConsultaFaltantesFinalizarAsistencia(string Linea, DateTime Fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var result = db.ASISTENCIA.Where(x =>
                            x.Linea == Linea
                            && x.Fecha < Fecha
                            && x.FechaFin == null
                            && (x.EstadoAsistencia == clsAtributos.EstadoPresente || x.EstadoAsistencia == clsAtributos.EstadoAtraso)
                            && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                            ).ToList();

                List<ASISTENCIA> resultado = new List<ASISTENCIA>();
                foreach (var x in result)
                {
                    if(resultado.FirstOrDefault(y=> y.Fecha == x.Fecha)==null)
                    {
                        resultado.Add(x);
                    }
                }

                return resultado;
            }
        }
        public List<ASISTENCIA> ConsultaFaltantesFinalizarAsistenciaTodos(DateTime Fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                var result = db.ASISTENCIA.Where(x =>
                            x.Fecha < Fecha
                            && x.FechaFin == null
                            && (x.EstadoAsistencia == clsAtributos.EstadoPresente || x.EstadoAsistencia == clsAtributos.EstadoAtraso)
                            ).ToList();
                return result;
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
        public int ConsultarExistenciaAsistenciaGeneral(string cedula, string Turno, DateTime Fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                BuscarControlador = db.spConsutaEmpleados(cedula).ToList().FirstOrDefault();
                //pListAsistencia = db.sp_ConsultaAsistenciaDiaria(BuscarControlador.CODIGOLINEA + "", Turno, Fecha).ToList();
             
                pListAsistenciaExiste = db.ASISTENCIA.Where(x => x.Fecha == Fecha && x.Linea == BuscarControlador.CODIGOLINEA && x.Turno == Turno).ToList();

            }

            if (pListAsistenciaExiste.Count == 0)
                return 0;
            else
                return 1;
        }
        public int ConsultarExistenciaAsistencia(string cedula, string Turno, DateTime Fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                DateTime fechaInicio = Convert.ToDateTime(Fecha.ToShortDateString());
                DateTime fechaFin = Convert.ToDateTime(Fecha.AddDays(1).ToShortDateString());
                BuscarControlador = db.spConsutaEmpleados(cedula).ToList().FirstOrDefault();


                //IQueryable<ASISTENCIA> query = (from a in db.ASISTENCIA
                //                                join b in db.CAMBIO_PERSONAL on new { a.Cedula, EstadoRegistro = clsAtributos.EstadoRegistroActivo, CodLinea = BuscarControlador.CODIGOLINEA } equals new { b.Cedula, b.EstadoRegistro, b.CodLinea } into c
                //                                from b in c.DefaultIfEmpty()
                //                                where a.Fecha >= fechaInicio && a.Fecha < fechaFin
                //             && a.Linea == BuscarControlador.CODIGOLINEA//para que me traiga solo la asistencia de la linea que pertenece
                //             && a.Turno == Turno
                //             && b == null
                //                                select a);
                //pListAsistenciaExiste = query.ToList();
                var ConsultarSIExisteAsistencia = db.ASISTENCIA.Where(x => x.Linea == BuscarControlador.CODIGOLINEA && x.Fecha == Fecha && x.Turno == Turno && x.Generado == clsAtributos.Procesos).ToList();
                if (ConsultarSIExisteAsistencia.Count == 0)
                    //if (pListAsistenciaExiste.Count == 0)
                    return 0;
                else
                    return 1;
            }
            
        }
        public int ConsultarExistenciaAsistenciaPrestados(string cedula, string Turno, DateTime Fecha)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                BuscarControlador = db.spConsutaEmpleados(cedula).ToList().FirstOrDefault();
                //pListAsistenciaMovidos = db.sp_ConsultaAsistenciaDiariaPersonalMovido(BuscarControlador.CODIGOLINEA + "", Convert.ToInt32(Turno), Fecha).ToList();
                var buscarSiSeGeneroAsistenciaPrestados = db.ASISTENCIA.Where(x => x.Linea == BuscarControlador.CODIGOLINEA && x.Fecha == Fecha && x.Turno == Turno && x.Generado == clsAtributos.Prestado).ToList();
                if (buscarSiSeGeneroAsistenciaPrestados.Count == 0)
                    //if (pListAsistenciaMovidos.ToList().Count == 0)
                    return 0;
                else
                    return 1;
            }
            
        }
        public ControlDeAsistenciaGeneralViewModel ObtenerAsistenciaGeneralDiaria(string CodLinea, int BanderaExiste, string usuario, string terminal, string turno, DateTime Fecha, TimeSpan Hora)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<ASISTENCIA> ControlAsistencia = null;
                clsDCambioPersonal clsDCambioPersonal = new clsDCambioPersonal();

                ControlDeAsistenciaGeneralViewModel ControlAsistenciaGeneralViewModel = null;
                if (BanderaExiste == 0)
                {

                    //List<spConsutaEmpleadosFiltro> ListaEmpleados = db.spConsutaEmpleadosFiltro("0", CodLinea, "0").Where(x => x.CODIGOCARGO != "221").ToList();
                    List<spConsultarEmpleadosxTurno> ListaEmpleados = db.spConsultarEmpleadosxTurno(CodLinea, turno, Fecha, Hora).ToList();//corregir parametros null mandar fecha y hora
                    ControlAsistencia = new List<ASISTENCIA>();
                    foreach (var item in ListaEmpleados)
                    {
                        //var FueMovidoAOtraArea = clsDCambioPersonal.ConsultarCambioPersonal(item.CEDULA);
                        //if (FueMovidoAOtraArea == null)
                        ControlAsistencia.Add(new ASISTENCIA { Cedula = item.CEDULA, Fecha = Fecha, EstadoAsistencia = clsAtributos.EstadoFalta, Linea = CodLinea, Turno = turno, Observacion = "", UsuarioCreacionLog = usuario, TerminalCreacionLog = terminal, FechaCreacionLog = DateTime.Now, EstadoRegistro = "A", CentroCostos = item.CODIGOAREA, Recurso = item.RECURSO, Cargo = item.CODIGOCARGO });

                    }
                    var PersonalMovidoAEstaLinea = clsDCambioPersonal.ConsultarCambioPersonalxLinea(CodLinea, turno, Fecha, Hora);
                    //**
                    //SI YA SE GENERO LA ASISTENCIA EN LINEA_X Y LE DOY PRESENTE A LAS 7AM Y LUEGO  PRESTO A ESA PERSONA A LAS 7:30 A CONTROL_RECUPERADO Y CONTROL, Y EN
                    //XONTROL RECUPERADO Y CONTRO GENERO LA ASISTENCIA GENERAL A LAS 8AM (ESA PERSONA NO DEBE SALIR)
                    var AsistenciaBuscar = db.ASISTENCIA.Where(x => x.Fecha == Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();//traigo la asistencia de todas las lineas en la fecha ingresada
                    List<spConsultarCambioPersonalxLineaxTurno> PersonalMovidoAEstaLineaFiltrado = (from p in PersonalMovidoAEstaLinea
                                                                                                    join asis in AsistenciaBuscar
                                                                                                    on p.Cedula equals asis.Cedula into pp
                                                                                                    from asis in pp.DefaultIfEmpty()
                                                                                                    where asis == null
                                                                                                    select p).ToList();
                    //**
                    //foreach (var item in PersonalMovidoAEstaLinea)
                    foreach (var item in PersonalMovidoAEstaLineaFiltrado)
                    {
                        ControlAsistencia.Add(new ASISTENCIA { Cedula = item.Cedula, Fecha = Fecha, EstadoAsistencia = clsAtributos.EstadoFalta, Linea = item.CodLinea, Turno = turno, Observacion = "", UsuarioCreacionLog = usuario, TerminalCreacionLog = terminal, FechaCreacionLog = DateTime.Now, EstadoRegistro = "A", CentroCostos = item.CentroCosto, Recurso = item.Recurso, Cargo = item.CodCargo, Generado=clsAtributos.General });

                    }
                    db.ASISTENCIA.AddRange(ControlAsistencia);
                    if (ControlAsistencia.Count > 0)//solo si se va a crear al menos un registristro en asistencia se guarda en contro_asistencia
                    {
                        db.CONTROL_ASISTENCIA.Add(new CONTROL_ASISTENCIA
                        {
                            Fecha = Fecha,
                            Linea = CodLinea,
                            Turno = turno,
                            Hora = Hora,
                            EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                            Generado = clsAtributos.General,
                            FechaIngresoLog = DateTime.Now,
                            TerminalIngresoLog = terminal,
                            UsuarioIngresoLog = usuario
                        });
                    }
                    db.SaveChanges();
                    pListAsistenciaGeneral = db.sp_ConsultaAsistenciaGeneralDiaria(CodLinea, Convert.ToInt32(turno), Fecha,Hora).ToList();
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
                    var control_asistencia = db.CONTROL_ASISTENCIA.Where(x => x.Fecha == Fecha && x.Linea == CodLinea && x.Turno == turno && x.Generado == clsAtributos.General).FirstOrDefault();
                    pListAsistenciaGeneral = db.sp_ConsultaAsistenciaGeneralDiaria(CodLinea, Convert.ToInt32(turno), Fecha,control_asistencia.Hora).ToList();
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
        public string InactivarEmpleadosCambioPersonal(string[] ArrayCedulas)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                List<MOVIMIENTO_PERSONAL_DIARIO> BuscarMovimientoPersonalDiario;
                List<string> NoSePudoInactivar = new List<string>();
                //Desactivo los registros en CAMBIO_PERSONAL
                var BuscarCambioPersonal = db.CAMBIO_PERSONAL.Where(x => ArrayCedulas.Contains(x.Cedula)).ToList();

                foreach (var item in BuscarCambioPersonal)
                {
                    //verifico que no se haya generado asistencia para poder desactivar el registro en cambio de personal
                    BuscarMovimientoPersonalDiario = db.MOVIMIENTO_PERSONAL_DIARIO.Where(x => x.FechaInicio == item.Fecha && x.HoraInicio < item.HoraInicio).ToList();
                    if (BuscarMovimientoPersonalDiario.Count == 0)
                    {
                        item.EstadoRegistro = "I";
                    }
                    else
                    {
                        NoSePudoInactivar.Add(item.Cedula + "No se pudo inactivar por que la asistencia ya habia sido generada");
                    }
                }
                //Desactivo todos los registros en MOVIMIENTO_PERSONAL_DIARIO

                db.SaveChanges();
                if (NoSePudoInactivar.Count == 0)
                    return "Empleado(s) Inactivados con éxito";
                else
                {
                    string mensaje = string.Empty;
                    foreach (var item in NoSePudoInactivar)
                    {
                        mensaje += item + "\n";
                    }
                    return mensaje;
                }
            }
        }
        public ControlDeAsistenciaViewModel ObtenerAsistenciaDiaria(string CodLinea, int BanderaExiste, string usuario, string terminal, string turno, DateTime Fecha, TimeSpan HoraServidor)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {

                List<ASISTENCIA> ControlAsistencia = null;
                clsDCambioPersonal clsDCambioPersonal = new clsDCambioPersonal();

                ControlDeAsistenciaViewModel ControlAsistenciaViewModel = null;
                if (BanderaExiste == 0)
                {

                    //List<spConsutaEmpleadosFiltro> ListaEmpleados = db.spConsutaEmpleadosFiltro("0", CodLinea, "0").Where(x => x.CODIGOCARGO != "221").ToList();
                    List<spConsultarEmpleadosxTurno> ListaEmpleados = db.spConsultarEmpleadosxTurno(CodLinea, turno, Fecha, HoraServidor).ToList();
                    //AGREGUE 2020-01-15: Si se genero aistencia(PRESTADOS) en linea 5 a las 07:00am de un empleado que fue movido a esa línea con diferente cargo , si se genera asistencia proncesos 6:30 am no deberia aparecer por que ya existe en tabla asistencia 
                    var AsistenciaBuscar = db.ASISTENCIA.Where(x => x.Fecha == Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();//traigo la asistencia de todas las lineas en la fecha ingresada
                    List<spConsultarEmpleadosxTurno> PersonalMovidoAEstaLineaFiltrado = (from p in ListaEmpleados
                                                                                                    join asis in AsistenciaBuscar
                                                                                                    on p.CEDULA equals asis.Cedula into pp
                                                                                                    from asis in pp.DefaultIfEmpty()
                                                                                                    where asis == null
                                                                                                    select p).ToList();
                    //**FIN 2020-01-15
                    ControlAsistencia = new List<ASISTENCIA>();
                    foreach (var item in PersonalMovidoAEstaLineaFiltrado)
                    {
                        //var FueMovidoAOtraArea = clsDCambioPersonal.ConsultarCambioPersonal(item.CEDULA);
                        //if (FueMovidoAOtraArea==null)
                        ControlAsistencia.Add(new ASISTENCIA { Cedula = item.CEDULA, Fecha = Fecha, EstadoAsistencia = clsAtributos.EstadoFalta, Linea = item.CODIGOLINEA, Turno = turno, Observacion = "", UsuarioCreacionLog = usuario, TerminalCreacionLog = terminal, FechaCreacionLog = DateTime.Now, EstadoRegistro = "A", CentroCostos = item.CODIGOAREA, Recurso = item.RECURSO, Cargo = item.CODIGOCARGO, Generado=clsAtributos.Procesos });
                    }
                    db.ASISTENCIA.AddRange(ControlAsistencia);
                    db.CONTROL_ASISTENCIA.Add(new CONTROL_ASISTENCIA { Fecha = Fecha, Linea = CodLinea, Turno = turno, Hora = HoraServidor, EstadoRegistro = clsAtributos.EstadoRegistroActivo, Generado = clsAtributos.Procesos,
                    FechaIngresoLog=DateTime.Now,TerminalIngresoLog=terminal,UsuarioIngresoLog=usuario });

                    db.SaveChanges();
                    pListAsistencia = db.sp_ConsultaAsistenciaDiaria(CodLinea, Convert.ToInt32(turno), Fecha, HoraServidor).ToList();
                    //pListAsistencia.ForEach(x => x.Hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")));
                    foreach (var item in pListAsistencia)
                    {
                        item.Hora = item.EstadoAsistencia == clsAtributos.EstadoFalta ? TimeSpan.Parse(DateTime.Now.ToString("HH:mm")) : item.Hora;
                        //item.HoraSalida = item.HoraSalida == null ? TimeSpan.Parse(DateTime.Now.ToString("HH:mm")) : item.HoraSalida;
                    }
                    ControlAsistenciaViewModel = new ControlDeAsistenciaViewModel
                    {
                        ControlAsistencia = pListAsistencia.OrderBy(z => z.NOMBRES).ToList()
                    };
                }
                else
                {
                    var control_asistencia=db.CONTROL_ASISTENCIA.Where(x => x.Fecha == Fecha && x.Linea == CodLinea && x.Turno == turno && x.Generado == clsAtributos.Procesos).FirstOrDefault();
                    pListAsistencia = db.sp_ConsultaAsistenciaDiaria(CodLinea, Convert.ToInt32(turno), Fecha,control_asistencia.Hora).ToList();
                    //pListAsistencia.ForEach(x => x.Hora = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")));
                    foreach (var item in pListAsistencia)
                    {
                        item.Hora = item.EstadoAsistencia == clsAtributos.EstadoFalta ? TimeSpan.Parse(DateTime.Now.ToString("HH:mm")) : item.Hora;
                        //item.HoraSalida = item.HoraSalida == null ? TimeSpan.Parse(DateTime.Now.ToString("HH:mm")) : item.HoraSalida;
                    }
                    ControlAsistenciaViewModel = new ControlDeAsistenciaViewModel
                    {
                        ControlAsistencia = pListAsistencia.OrderBy(z => z.NOMBRES).ToList()
                    };
                }


                return ControlAsistenciaViewModel;
            }
        }

        public ControlDeAsistenciaPrestadosViewModel ObtenerAsistenciaDiariaMovidos(string CodLinea, int BanderaExiste, string usuario, string terminal, string turno, DateTime Fecha, TimeSpan Hora)
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
                    var EmpleadosMovidos = clsdCambioPersonal.ConsultarCambioPersonalxLinea(CodLinea, turno, Fecha, Hora);
                    //AGREGUE 2020-01-15: Si se genero aistencia(procesos) de Juan a las 06:40am en su linea y cargo, pero en CambioPersonal tiene horaInicio 07:00 a otro cargo, ya no debería generarse su asistencia en AsistenciaPrestados por que se crearian 2 registros en tabla de asistencia 
                    var AsistenciaBuscar = db.ASISTENCIA.Where(x => x.Fecha == Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo).ToList();//traigo la asistencia de todas las lineas en la fecha ingresada
                    List<spConsultarCambioPersonalxLineaxTurno> PersonalMovidoAEstaLineaFiltrado = (from p in EmpleadosMovidos
                                                                                                    join asis in AsistenciaBuscar
                                                                                                    on p.Cedula equals asis.Cedula into pp
                                                                                                    from asis in pp.DefaultIfEmpty()
                                                                                                    where asis == null
                                                                                                    select p).ToList();
                    //**FIN 2020-01-15
                    foreach (var item in PersonalMovidoAEstaLineaFiltrado)
                    //foreach (var item in EmpleadosMovidos)
                    {
                        ControlAsistencia.Add(new ASISTENCIA { Cedula = item.Cedula, Fecha = Fecha, EstadoAsistencia = clsAtributos.EstadoFalta, Linea = item.CodLinea, Turno = turno, Observacion = "", UsuarioCreacionLog = usuario, TerminalCreacionLog = terminal, FechaCreacionLog = DateTime.Now, EstadoRegistro = "A", CentroCostos = item.CentroCosto, Recurso = item.Recurso, Cargo = item.CodCargo,Generado=clsAtributos.Prestado });
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
                    if (ControlAsistencia.Count > 0)//solo si se va a crear al menos un registristro en asistencia se guarda en contro_asistencia
                    {
                        db.CONTROL_ASISTENCIA.Add(new CONTROL_ASISTENCIA
                        {
                            Fecha = Fecha,
                            Linea = CodLinea,
                            Turno = turno,
                            Hora = Hora,
                            EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                            Generado = clsAtributos.Prestado,
                            FechaIngresoLog = DateTime.Now,
                            TerminalIngresoLog = terminal,
                            UsuarioIngresoLog = usuario
                        });
                    }
                    
                    db.SaveChanges();
                    pListAsistenciaMovidos = db.sp_ConsultaAsistenciaDiariaPersonalMovido(CodLinea, Convert.ToInt32(turno), Fecha).ToList();

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
                    pListAsistenciaMovidos = db.sp_ConsultaAsistenciaDiariaPersonalMovido(CodLinea, Convert.ToInt32(turno), Fecha).ToList();
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
                List<int?> CuchillosBlancos = db.sp_ObtenerCuchillosSobrantes(clsAtributos.CodigoColorCuchilloBlanco, Fecha).ToList();
                List<int?> CuchillosRojos = db.sp_ObtenerCuchillosSobrantes(clsAtributos.CodigoColorCuchilloRojo, Fecha).ToList();
                List<int?> CuchillosNegros = db.sp_ObtenerCuchillosSobrantes(clsAtributos.CodigoColorCuchilloNegro, Fecha).ToList();
                List<ControlDeAsistenciaPrestadosViewModel.Cuchillos> CuchillosBlancosSobrantes = new List<ControlDeAsistenciaPrestadosViewModel.Cuchillos>();
                List<ControlDeAsistenciaPrestadosViewModel.Cuchillos> CuchillosRojosSobrantes = new List<ControlDeAsistenciaPrestadosViewModel.Cuchillos>();
                List<ControlDeAsistenciaPrestadosViewModel.Cuchillos> CuchillosNegrosSobrantes = new List<ControlDeAsistenciaPrestadosViewModel.Cuchillos>();
                foreach (var item in CuchillosBlancos)
                {
                    CuchillosBlancosSobrantes.Add(new ControlDeAsistenciaPrestadosViewModel.Cuchillos { Id = item, Numero = item });
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
        public string GuardarAsistenciaSalida(string Cedula, DateTime Fecha, TimeSpan Hora, string Tipo, int IdMovimientoPersonalDiario, string Turno, string CodLinea)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                if (Tipo == "MarcarSalida")
                {
                    //MOVIMIENTO_PERSONAL_DIARIO BuscarMovimientoPersonal = db.MOVIMIENTO_PERSONAL_DIARIO.Where(z => z.Cedula == Cedula && z.FechaInicio == Fecha && z.HoraFin == null).FirstOrDefault();
                    MOVIMIENTO_PERSONAL_DIARIO BuscarMovimientoPersonal = db.MOVIMIENTO_PERSONAL_DIARIO.Find(IdMovimientoPersonalDiario);
                    //ASISTENCIA BuscarAsistencia = db.ASISTENCIA.Where(a => a.Cedula == Cedula && a.Fecha == Fecha).FirstOrDefault();
                    ASISTENCIA BuscarAsistencia = db.ASISTENCIA.Where(a => a.Cedula == Cedula && a.Fecha == Fecha && a.Turno == Turno).FirstOrDefault();
                    if (BuscarMovimientoPersonal != null)
                    {
                        //valido que la fecha y hora de finalizar asistencia ingresada por el usuario sea mayor a la fecha y hora en que inicio en la linea
                        if (BuscarMovimientoPersonal.FechaInicio.Value.Add(BuscarMovimientoPersonal.HoraInicio.Value) < Fecha.Add(Hora))
                        {
                            BuscarMovimientoPersonal.HoraFin = Hora;
                            BuscarMovimientoPersonal.FechaFin = Fecha;
                            BuscarMovimientoPersonal.FinalizaAsistencia = true;
                        }
                        else
                        {
                            return "2";//no se puede finalizar 
                        }

                    }
                    if (BuscarAsistencia != null)
                    {
                        BuscarAsistencia.HoraSalida = Hora;
                        BuscarAsistencia.FechaFin = Fecha;
                    }
                }
                else
                {
                    //MOVIMIENTO_PERSONAL_DIARIO BuscarMovimientoPersonal = db.MOVIMIENTO_PERSONAL_DIARIO.Where(z => z.Cedula == Cedula && z.FechaInicio == Fecha && z.HoraFin != null).FirstOrDefault();
                    MOVIMIENTO_PERSONAL_DIARIO BuscarMovimientoPersonal = db.MOVIMIENTO_PERSONAL_DIARIO.Find(IdMovimientoPersonalDiario);
                    ASISTENCIA BuscarAsistencia = db.ASISTENCIA.Where(a => a.Cedula == Cedula && a.Fecha == Fecha).FirstOrDefault();
                    if (BuscarMovimientoPersonal != null)
                    {
                        BuscarMovimientoPersonal.HoraFin = null;
                        BuscarMovimientoPersonal.FechaFin = null;
                    }
                    if (BuscarAsistencia != null)
                    {
                        BuscarAsistencia.HoraSalida = null;
                    }
                }

                db.SaveChanges();
                return "Asistencia salida ingresada";
            }
        }
        public string ActualizarAsistencia(ASISTENCIA psAsistencia)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    DateTime Fechainicio = Convert.ToDateTime(psAsistencia.Fecha.Value.ToShortDateString());
                    DateTime FechaFin = Convert.ToDateTime(psAsistencia.Fecha.Value.AddDays(1).ToShortDateString());
                    var BuscarEnAsistencia = db.ASISTENCIA.Where(x => x.Cedula == psAsistencia.Cedula && (x.Fecha >= Fechainicio && x.Fecha < FechaFin)).FirstOrDefault();
                    BuscarEnAsistencia.EstadoAsistencia = psAsistencia.EstadoAsistencia;
                    if (!string.IsNullOrEmpty(psAsistencia.Observacion))
                        BuscarEnAsistencia.Observacion = psAsistencia.Observacion;
                    if (psAsistencia.Hora != null)
                        BuscarEnAsistencia.Hora = psAsistencia.Hora;
                    BuscarEnAsistencia.FechaModificacionLog = psAsistencia.FechaModificacionLog;
                    BuscarEnAsistencia.UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                    BuscarEnAsistencia.TerminalModificacionLog = psAsistencia.TerminalModificacionLog;

                    //**ingresar en MOVIMIENTO_PERSONAL_DIARIO  (entrada > fecha1 && entrada < fecha2)
                    if (psAsistencia.EstadoAsistencia == clsAtributos.EstadoFalta)//si en asistencia, despues de haberle dado asistencia le cambian el estado a falta
                    {
                        //var BuscarMovimientoPersonalActivo= (from m in db.MOVIMIENTO_PERSONAL_DIARIO
                        //                                     where m.FechaInicio == Fechainicio && m.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                        //                                     select m).FirstOrDefault();

                        //traigo el último registro ingresado en MOVIMIENTO_PERSONAL con la fecha indicada

                        //var BuscarMovimientoPersonalActivo = (from m in db.MOVIMIENTO_PERSONAL_DIARIO
                        //                                      where m.FechaInicio == Fechainicio && m.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                        //                                      orderby m.IdCambioPersonal descending
                        //                                      select m).FirstOrDefault();

                        //Busco en MOVIMIENTO
                        var BuscarMovimientoPersonalActivo = (from m in db.MOVIMIENTO_PERSONAL_DIARIO
                                                              where m.FechaInicio == Fechainicio && m.EstadoRegistro == clsAtributos.EstadoRegistroActivo & m.Cedula == psAsistencia.Cedula
                                                              select m).ToList();
                        if (BuscarMovimientoPersonalActivo.Count > 0 /*!= null*/)
                        {
                            foreach (var item in BuscarMovimientoPersonalActivo)
                            {
                                item.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                                item.UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                                item.FechaModificacionLog = DateTime.Now;
                                item.TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                            }
                            //BuscarMovimientoPersonalActivo.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                        }

                    }
                    else
                    {

                        //código para corregir , por que se puede dar guardar asistencia desde combos de "estados de asistencia", para que no se cree doble registro en movimiento_personal, cuando de presente se pasa a atraso o de atraso a presente

                        var BuscarMovimientoPersonalActivo = (from m in db.MOVIMIENTO_PERSONAL_DIARIO
                                                              where m.FechaInicio == Fechainicio && m.EstadoRegistro == clsAtributos.EstadoRegistroActivo & m.Cedula == psAsistencia.Cedula
                                                              select m).ToList();
                        if (BuscarMovimientoPersonalActivo.Count > 0 /*!= null*/)
                        {
                            foreach (var item in BuscarMovimientoPersonalActivo)
                            {
                                item.EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                                item.UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                                item.FechaModificacionLog = DateTime.Now;
                                item.TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                            }

                        }
                        db.SaveChanges();
                        //fin código para corregir 

                        //**
                        //Si se Marca "Asistencia Presente" a un empleado, entonces genero un nuevo registro en MOVIMIENTO_PERSONAL_DIARIO
                        List<MOVIMIENTO_PERSONAL_DIARIO> NuevoMovimientoPersonalAsistencia = new List<MOVIMIENTO_PERSONAL_DIARIO>();
                        //debo buscar si el registro ya existe pero esta  inactivado
                        //---i
                        MOVIMIENTO_PERSONAL_DIARIO BuscarRegInactivos;
                        var BuscarMovimientoPersonalDiario = db.MOVIMIENTO_PERSONAL_DIARIO.Where(x => x.Cedula == psAsistencia.Cedula && x.FechaInicio == psAsistencia.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroInactivo).ToList();
                        int NumeroRegistros = BuscarMovimientoPersonalDiario.Count;
                        int indice = 0;
                        //--f
                        //--i
                        BuscarRegInactivos = BuscarMovimientoPersonalDiario.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroInactivo).FirstOrDefault();
                        //if (NumeroRegistros >= 1)
                        if (BuscarRegInactivos != null)
                        {
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaInicio = null;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraInicio = null;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaFin = null;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraFin = null;
                            //BuscarMovimientoPersonalDiario[indice].EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                            //BuscarMovimientoPersonalDiario[indice].FechaInicio = psAsistencia.Fecha;
                            //BuscarMovimientoPersonalDiario[indice].HoraInicio = psAsistencia.Hora;
                            //BuscarMovimientoPersonalDiario[indice].UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                            //BuscarMovimientoPersonalDiario[indice].TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                            //BuscarMovimientoPersonalDiario[indice].FechaModificacionLog = DateTime.Now;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaInicio = psAsistencia.Fecha;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraInicio = psAsistencia.Hora;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).Turno = psAsistencia.Turno;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaModificacionLog = DateTime.Now;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).Cedula = psAsistencia.Cedula;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CodLinea = psAsistencia.Linea;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CentroCosto = psAsistencia.CentroCostos;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CodCargo = psAsistencia.Cargo;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).Recurso = psAsistencia.Recurso;
                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).Asistencia = true;
                        }
                        //--f
                        else
                        {
                            NuevoMovimientoPersonalAsistencia.Add(new MOVIMIENTO_PERSONAL_DIARIO
                            {
                                Cedula = psAsistencia.Cedula,
                                CodLinea = psAsistencia.Linea,
                                CentroCosto = psAsistencia.CentroCostos,
                                CodCargo = psAsistencia.Cargo,
                                Recurso = psAsistencia.Recurso,
                                FechaInicio = psAsistencia.Fecha,
                                HoraInicio = psAsistencia.Hora,
                                Asistencia = true,
                                Turno = psAsistencia.Turno,
                                EstadoRegistro = psAsistencia.EstadoRegistro,
                                FechaIngresoLog = psAsistencia.FechaModificacionLog,
                                TerminalIngresoLog = psAsistencia.TerminalModificacionLog,
                                UsuarioIngresoLog = psAsistencia.UsuarioModificacionLog
                            });
                        }

                        //Busco en CAMBIO_PERSONAL donde la fecha de inicio sea igual a la fecha de la asistencia
                        //List<CAMBIO_PERSONAL> BuscarCambioPersonal = db.CAMBIO_PERSONAL.Where(x => x.Cedula == psAsistencia.Cedula && x.Fecha == psAsistencia.Fecha && x.HoraInicio != null && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && (x.FechaFin == psAsistencia.Fecha || x.FechaFin == null) /*&& x.HoraInicio > psAsistencia.Hora*/).ToList();

                        //**cambio consulta de "Buscar CambioPersonal" para traer tambien cuando la fechaFinCambioPer==FechaAsistencia y la fechaInicioCambioP<FechaAsistencia por el caso de la persona que pudo ser prestada un dia antes y regresada el dia siguiente
                        List<CAMBIO_PERSONAL> BuscarCambioPersonal = db.CAMBIO_PERSONAL.Where(x =>
                        (
                            x.Cedula == psAsistencia.Cedula && x.Fecha == psAsistencia.Fecha && x.HoraInicio != null
                            && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo && (x.FechaFin == psAsistencia.Fecha || x.FechaFin == null)
                        ) ||
                        (
                            x.Cedula == psAsistencia.Cedula && x.FechaFin == psAsistencia.Fecha && x.Fecha < psAsistencia.Fecha && x.EstadoRegistro == clsAtributos.EstadoRegistroActivo
                        )
                        ).OrderBy(x => x.Fecha).ToList();
                        //**
                        ClsdEmpleado = new clsDEmpleado();
                        spConsultaEspecificaEmpleadosxCedula BuscarEmpleadoDataL;
                        /*if (BuscarCambioPersonal != null )*///si encuentra que el empleado fue movido en esa fecha
                        if (BuscarCambioPersonal.Count > 0)
                        {
                            foreach (var item in BuscarCambioPersonal)
                            {
                                /*if (item.HoraInicio != null)*///verifico que no haya sido movido a inicio de jornada
                                //**cambie el if para que entre solo si la fechaFInicio==FechaAsistencia por el caso de la persona que pudo ser prestada un dia antes y regresada el dia siguiente 
                                if (item.HoraInicio != null &&item.Fecha==psAsistencia.Fecha)
                                //**
                                {
                                    if (item.HoraInicio > psAsistencia.Hora)//verifico que la HoraInicio que fue movido sea mayor a la hora de la asistencia
                                    {
                                        //actualizo la horaFin y FechaFin del primer registro en MOVIMIENTO_PERSONAL_DIARIO
                                        //if (NumeroRegistros == 0)
                                        if (BuscarRegInactivos == null)
                                        {
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().FechaFin = item.Fecha;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().HoraFin = item.HoraInicio;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().FechaModificacionLog = DateTime.Now;
                                        }
                                        else
                                        {
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaFin = item.Fecha;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraFin = item.HoraInicio;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaModificacionLog = DateTime.Now;
                                        }

                                        indice += 1;
                                        BuscarRegInactivos = BuscarMovimientoPersonalDiario.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroInactivo).FirstOrDefault();
                                        //genero un nuevo registro con fecha y hora de inicio a donde fue movido el empleado
                                        //if (NumeroRegistros >= indice)
                                        if (BuscarRegInactivos != null)
                                        {
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaInicio = null;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraInicio = null;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaFin = null;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraFin = null;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CodLinea = item.CodLinea;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CentroCosto = item.CentroCosto;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CodCargo = item.CodCargo;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).Recurso = item.Recurso;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).Turno = psAsistencia.Turno;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaInicio = item.Fecha;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraInicio = item.HoraInicio;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaModificacionLog = DateTime.Now;
                                        }
                                        else
                                        {
                                            NuevoMovimientoPersonalAsistencia.Add(new MOVIMIENTO_PERSONAL_DIARIO
                                            {
                                                Cedula = item.Cedula,
                                                CodLinea = item.CodLinea,
                                                CentroCosto = item.CentroCosto,
                                                CodCargo = item.CodCargo,
                                                Recurso = item.Recurso,
                                                FechaInicio = item.Fecha,
                                                HoraInicio = item.HoraInicio,
                                                EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                                                Asistencia = false,
                                                Turno = psAsistencia.Turno,
                                                TerminalIngresoLog = psAsistencia.TerminalModificacionLog,
                                                UsuarioIngresoLog = psAsistencia.UsuarioModificacionLog,
                                                FechaIngresoLog = DateTime.Now
                                            });
                                        }


                                    }
                                    else
                                    {
                                        //if (NumeroRegistros == 0)
                                        if (BuscarRegInactivos == null)
                                        {
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().FechaFin = item.Fecha;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().HoraFin = item.HoraInicio;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().FechaModificacionLog = DateTime.Now;
                                        }
                                        else
                                        {
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaFin = item.Fecha;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraFin = item.HoraInicio;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaModificacionLog = DateTime.Now;
                                        }
                                        indice += 1;
                                        BuscarRegInactivos = BuscarMovimientoPersonalDiario.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroInactivo).FirstOrDefault();
                                        //genero un nuevo registro con fecha y hora de inicio a donde fue movido el empleado
                                        //if (NumeroRegistros >= indice)
                                        if (BuscarRegInactivos != null)
                                        {
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaInicio = null;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraInicio = null;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaFin = null;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraFin = null;

                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CodLinea = item.CodLinea;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CentroCosto = item.CentroCosto;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CodCargo = item.CodCargo;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).Recurso = item.Recurso;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).Turno = psAsistencia.Turno;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaInicio = psAsistencia.Fecha;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraInicio = psAsistencia.Hora;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaModificacionLog = DateTime.Now;
                                        }
                                        else
                                        {
                                            NuevoMovimientoPersonalAsistencia.Add(new MOVIMIENTO_PERSONAL_DIARIO
                                            {
                                                Cedula = item.Cedula,
                                                CodLinea = item.CodLinea,
                                                CentroCosto = item.CentroCosto,
                                                CodCargo = item.CodCargo,
                                                Recurso = item.Recurso,
                                                FechaInicio = item.Fecha,
                                                HoraInicio = psAsistencia.Hora,
                                                EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                                                Asistencia = false,
                                                Turno = psAsistencia.Turno,
                                                TerminalIngresoLog = psAsistencia.TerminalModificacionLog,
                                                UsuarioIngresoLog = psAsistencia.UsuarioModificacionLog,
                                                FechaIngresoLog = DateTime.Now
                                            });
                                        }
                                    }
                                }
                                //pregunto si el empleado que fue movido tiene fecha de regreso a donde pertenece igual a la fecha que se marco asistencia
                                if (item.FechaFin != null && item.Horafin != null && psAsistencia.Fecha == item.Fecha)
                                {
                                    //consulto linea,cargo,recurso y centro de costo donde el empleado pertenece en DataLife
                                    BuscarEmpleadoDataL = ClsdEmpleado.ConsultarEmpleadoxCedula(psAsistencia.Cedula);
                                    //modifico el 2do registro en MOVIMIENTO_PERSONAL_DIARIO para actualizar Fecha fin y hora fin
                                    if (item.Horafin > psAsistencia.Hora)
                                    {
                                        //if (NumeroRegistros == 0)
                                        if (BuscarRegInactivos == null)
                                        {
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().FechaFin = item.FechaFin;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().HoraFin = item.Horafin;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().FechaModificacionLog = DateTime.Now;
                                        }
                                        else
                                        {
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaFin = item.FechaFin;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraFin = item.Horafin;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaModificacionLog = DateTime.Now;
                                        }
                                        indice += 1;
                                        BuscarRegInactivos = BuscarMovimientoPersonalDiario.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroInactivo).FirstOrDefault();
                                        //genero un nuevo registro para el retorno a la linea donde pertenece el empleado con fechainicio y hora inicio igual a la fecha de retorno
                                        //if (NumeroRegistros >= indice)
                                        if (BuscarRegInactivos != null)
                                        {
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaInicio = null;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraInicio = null;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaFin = null;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraFin = null;

                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CodLinea = BuscarEmpleadoDataL.LINEA;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CentroCosto = BuscarEmpleadoDataL.CENTRO_COSTOS;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CodCargo = BuscarEmpleadoDataL.CARGO;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).Recurso = BuscarEmpleadoDataL.RECURSO;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).Turno = psAsistencia.Turno;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaInicio = item.FechaFin;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraInicio = item.Horafin;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaModificacionLog = DateTime.Now;
                                        }
                                        else
                                        {
                                            NuevoMovimientoPersonalAsistencia.Add(new MOVIMIENTO_PERSONAL_DIARIO
                                            {
                                                Cedula = psAsistencia.Cedula,
                                                CodLinea = BuscarEmpleadoDataL.LINEA,
                                                CentroCosto = BuscarEmpleadoDataL.CENTRO_COSTOS,
                                                CodCargo = BuscarEmpleadoDataL.CARGO,
                                                Recurso = BuscarEmpleadoDataL.RECURSO,
                                                FechaInicio = item.FechaFin,
                                                HoraInicio = item.Horafin,
                                                EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                                                Asistencia = false,
                                                Turno = psAsistencia.Turno,
                                                TerminalIngresoLog = psAsistencia.TerminalModificacionLog,
                                                UsuarioIngresoLog = psAsistencia.UsuarioModificacionLog,
                                                FechaIngresoLog = DateTime.Now
                                            });
                                        }
                                    }
                                    else
                                    {
                                        //if (NumeroRegistros == 0)
                                        if (BuscarRegInactivos == null)
                                        {
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().FechaFin = item.FechaFin;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().HoraFin = item.Horafin;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                                            NuevoMovimientoPersonalAsistencia.LastOrDefault().FechaModificacionLog = DateTime.Now;
                                        }
                                        else
                                        {
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).EstadoRegistro = clsAtributos.EstadoRegistroInactivo;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaFin = item.FechaFin;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraFin = item.Horafin;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaModificacionLog = DateTime.Now;
                                        }
                                        indice += 1;
                                        BuscarRegInactivos = BuscarMovimientoPersonalDiario.Where(x => x.EstadoRegistro == clsAtributos.EstadoRegistroInactivo).FirstOrDefault();
                                        //genero un nuevo registro para el retorno a la linea donde pertenece el empleado con fechainicio y hora inicio igual a la fecha de retorno
                                        //if (NumeroRegistros >= indice)
                                        if (BuscarRegInactivos != null)
                                        {
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaInicio = null;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraInicio = null;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaFin = null;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraFin = null;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CodLinea = BuscarEmpleadoDataL.LINEA;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CentroCosto = BuscarEmpleadoDataL.CENTRO_COSTOS;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).CodCargo = BuscarEmpleadoDataL.CARGO;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).Recurso = BuscarEmpleadoDataL.RECURSO;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).Turno = psAsistencia.Turno;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).EstadoRegistro = clsAtributos.EstadoRegistroActivo;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaInicio = item.FechaFin;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).HoraInicio = psAsistencia.Hora;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).UsuarioModificacionLog = psAsistencia.UsuarioModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).TerminalModificacionLog = psAsistencia.TerminalModificacionLog;
                                            BuscarMovimientoPersonalDiario.Find(x => x.IdCambioPersonal == BuscarRegInactivos.IdCambioPersonal).FechaModificacionLog = DateTime.Now;
                                        }
                                        else
                                        {
                                            NuevoMovimientoPersonalAsistencia.Add(new MOVIMIENTO_PERSONAL_DIARIO
                                            {
                                                Cedula = psAsistencia.Cedula,
                                                CodLinea = BuscarEmpleadoDataL.LINEA,
                                                CentroCosto = BuscarEmpleadoDataL.CENTRO_COSTOS,
                                                CodCargo = BuscarEmpleadoDataL.CARGO,
                                                Recurso = BuscarEmpleadoDataL.RECURSO,
                                                FechaInicio = item.FechaFin,
                                                HoraInicio = psAsistencia.Hora,
                                                EstadoRegistro = clsAtributos.EstadoRegistroActivo,
                                                Asistencia = false,
                                                Turno = psAsistencia.Turno,
                                                TerminalIngresoLog = psAsistencia.TerminalModificacionLog,
                                                UsuarioIngresoLog = psAsistencia.UsuarioModificacionLog,
                                                FechaIngresoLog = DateTime.Now
                                            });
                                        }
                                    }


                                }
                            }
                        }
                        db.MOVIMIENTO_PERSONAL_DIARIO.AddRange(NuevoMovimientoPersonalAsistencia);
                    }

                    //**
                    db.SaveChanges();
                    transaction.Commit();
                }
                return "Registro actualizado con éxito";
            }

        }

        public List<spReporteAsistencia> ConsultarRptAsistencia(DateTime FechaInicio, DateTime FechaFin, string Linea, string Turno)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spReporteAsistencia(FechaInicio, FechaFin, Turno, Linea).ToList();
            }
        }


        public List<spConsultaMovimientoPersonalDiario> ConsultaMovimientoPersonalDiario(DateTime Fecha, TimeSpan Hora, string Linea)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.spConsultaMovimientoPersonalDiario(Fecha, Hora, Linea).ToList();
            }
        }
        public CONTROL_ASISTENCIA ConsultarControlAsistencia(DateTime Fecha, string CodLinea, string turno,string Generado)
        {
            using (ASIS_PRODEntities db = new ASIS_PRODEntities())
            {
                return db.CONTROL_ASISTENCIA.Where(x => x.Fecha == Fecha && x.Linea == CodLinea && x.Turno == turno && x.Generado == Generado).FirstOrDefault();
            }

        }
    }
}